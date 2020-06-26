using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using TQ.Geocoding.DataLoad.Models;
//using TQ.Geocoding.DataLoad.Models.Classes;
using TQ.Geocoding.DataLoad.HelperClasses;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace TQ.Geocoding.DataLoad.Controllers
{
    public class HomeController : Controller
    {
        ILogger logger;
        public HomeController(ILogger<HomeController> applicationLogger)
        {
            logger = applicationLogger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DataLoad()
        {
            ViewData["Message"] = "Data load page";

            TimeSpan elapsedTimeSpan = LoadDatabaseFromCsv();

            string elapsedTimeFormatted = String.Format("{0:00} hrs {1:00} min {2:00}.{3:00} secs",
                elapsedTimeSpan.Hours, elapsedTimeSpan.Minutes, elapsedTimeSpan.Seconds,
                elapsedTimeSpan.Milliseconds / 10);

            logger.LogInformation(String.Format("ABP cleardown and load completed in {0}", elapsedTimeFormatted));

            ViewData["DataLoadTime"] = elapsedTimeFormatted;

            return View();
        }

        public TimeSpan LoadDatabaseFromCsv()
        {
            Stopwatch stopWatch = Stopwatch.StartNew();

            logger.LogInformation(String.Format("{0} LoadDatabaseFromCsv started", DateTime.Now.ToLongTimeString()));

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var runParameters = configuration.GetSection("RunParameters");
            string connectionString = runParameters["ConnectionString"];
            bool deleteTargetTable = Convert.ToBoolean(runParameters["DeleteTargetTable"]);
            int fileBatchSize = Convert.ToInt32(runParameters["FileBatchSize"]);
            bool writeToLog = Convert.ToBoolean(runParameters["WriteToLog"]);
            string targetTable = runParameters["TargetTable"];
            string targetTablePrimaryKey = runParameters["TargetTablePrimaryKey"];
            string targetTableIndex = runParameters["TargetTableIndex"];
            string sourceFilePath = runParameters["SourceFilePath"];
            bool dropIndexesDuringLoad = Convert.ToBoolean(runParameters["DropIndexesDuringLoad"]);
            int numberOfFieldCols = Convert.ToInt32(runParameters["NumberofFieldColumns"]);
            int sqlLoadBatchSize = Convert.ToInt32(runParameters["SqlLoadBatchSize"]);
            string onlyLoadThisRecordType = runParameters["OnlyLoadThisRecordType"];
            bool applyStagingFilter = Convert.ToBoolean(runParameters["ApplyStagingFilter"]);
            string stagingFilterTable = runParameters["StagingFilterTable"];
            bool actionTheLoad = Convert.ToBoolean(runParameters["ActionTheLoad"]);
            string stagingFilterOutputFile = runParameters["StagingFilterOutputFile"];
            string postLoadStoredProcedureName = runParameters["PostLoadStoredProcedureName"];
            bool changeOnlyUpdates = Convert.ToBoolean(runParameters["ChangeOnlyUpdates"]);

            if (changeOnlyUpdates)
            {
                fileBatchSize = 1;
                // for COU must process files in Volume order (Volume column in Header record and the file name) e.g. AddressBasePremium_COU_2019-06-26_001
                logger.LogInformation(String.Format("{0} LoadDatabaseFromCsv is running in Change Only Update mode - no batching, no filtering", DateTime.Now.ToLongTimeString()));
            }

            if (!Directory.EnumerateFiles(sourceFilePath, "*_*.csv").Any())
            {
                logger.LogInformation(String.Format("{0} LoadDatabaseFromCsv found no files matching pattern '*_*.csv' to process in {1}", DateTime.Now.ToLongTimeString(), sourceFilePath));
                return new TimeSpan();
            }

            if (deleteTargetTable)
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    logger.LogInformation(String.Format("{0} LoadDatabaseFromCsv started truncating target staging table", DateTime.Now.ToLongTimeString()));

                    sqlConnection.Open();

                    using (var sqlCommand = new SqlCommand(String.Format("TRUNCATE TABLE {0}", targetTable), sqlConnection))
                    {
                        sqlCommand.ExecuteNonQuery();
                    }

                    sqlConnection.Close();
                    logger.LogInformation(String.Format("{0} LoadDatabaseFromCsv finished truncating target staging table", DateTime.Now.ToLongTimeString()));
                }
            }

            if (dropIndexesDuringLoad)
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    logger.LogInformation(String.Format("{0} LoadDatabaseFromCsv started dropping index on target staging table", DateTime.Now.ToLongTimeString()));

                    sqlConnection.Open();

                    using (var sqlCommand = new SqlCommand(String.Format("ALTER TABLE {0} DROP CONSTRAINT {1} WITH ( ONLINE = OFF )", targetTable, targetTablePrimaryKey), sqlConnection))
                    {
                        sqlCommand.ExecuteNonQuery();
                    }

                    using (var sqlCommand = new SqlCommand(String.Format("DROP INDEX {0} ON {1}", targetTableIndex, targetTable), sqlConnection))
                    {
                        sqlCommand.ExecuteNonQuery();
                    }

                    sqlConnection.Close();
                    logger.LogInformation(String.Format("{0} LoadDatabaseFromCsv finished dropping index on target staging table", DateTime.Now.ToLongTimeString()));
                }
            }

            try
            {
                StreamWriter stagingFilterOutputFileWriter = new StreamWriter(applyStagingFilter && (stagingFilterOutputFile != string.Empty) ? stagingFilterOutputFile : @"C:\temp\temp.csv");

                if (applyStagingFilter)
                {
                    LoadStagingFilterKeys(stagingFilterTable, connectionString);
                }

                int fileNumber = 0;
                List<string> batchOfFiles = new List<string>();

                // Order the input files as for COU must process in Date then Volume order (Volume column in Header record and in the file name) 
                // e.g. 2019-06-26 and 001 in "AddressBasePremium_COU_2019-06-26_001.csv"
                foreach (string fileName in Directory.EnumerateFiles(sourceFilePath, "*.csv").OrderBy(fileName => Convert.ToDateTime(fileName.Split('_')[2]).Ticks).ThenBy(fileName => fileName.Split('_').Last()))
                {
                    // batch not full so add file to it
                    if (batchOfFiles.Count < fileBatchSize)
                    {
                        batchOfFiles.Add(fileName);
                    }
                    else
                    {
                        // process batch and add file in hand to a new batch
                        foreach (string fileInBatch in batchOfFiles)
                        {
                            fileNumber++;
                            TaskList.Add(LoadFileToDbAsync(connectionString, logger, fileInBatch, fileNumber, numberOfFieldCols, targetTable, sqlLoadBatchSize, onlyLoadThisRecordType, applyStagingFilter, actionTheLoad, stagingFilterOutputFileWriter, changeOnlyUpdates));
                        }
                        Task.WaitAll(TaskList.ToArray());
                        batchOfFiles.Clear();
                        batchOfFiles.Add(fileName);
                    }
                }

                // process any remaining (part-filled) batch
                foreach (string fileInBatch in batchOfFiles)
                {
                    fileNumber++;
                    TaskList.Add(LoadFileToDbAsync(connectionString, logger, fileInBatch, fileNumber, numberOfFieldCols, targetTable, sqlLoadBatchSize, onlyLoadThisRecordType, applyStagingFilter, actionTheLoad, stagingFilterOutputFileWriter, changeOnlyUpdates));
                }

                Task.WaitAll(TaskList.ToArray());
            }
            catch (Exception ex)
            {
                logger.LogError(String.Format("LoadDatabaseFromCsv Exception {0}", ex.Message));
            }

            if (dropIndexesDuringLoad)
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();

                    using (var sqlCommand = new SqlCommand(String.Format("ALTER TABLE {0} ADD  CONSTRAINT {1} PRIMARY KEY CLUSTERED ( [StagingRecordId] ASC )WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]", targetTable, targetTablePrimaryKey), sqlConnection))
                    {
                        sqlCommand.ExecuteNonQuery();
                    }

                    using (var sqlCommand = new SqlCommand(String.Format("CREATE UNIQUE NONCLUSTERED INDEX {0} ON {1} ( [RecordIdentifier] ASC, [StagingRecordId] ASC )WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]", targetTableIndex, targetTable), sqlConnection))
                    {
                        sqlCommand.ExecuteNonQuery();
                    }

                    sqlConnection.Close();
                }
            }

            if (!string.IsNullOrWhiteSpace(postLoadStoredProcedureName))
            {
                RunPostLoadStoredProcedure(postLoadStoredProcedureName, connectionString);
            }

            stopWatch.Stop();
            TimeSpan elapsedTimeSpan = stopWatch.Elapsed;
            string elapsedTimeFormatted = String.Format("{0:00} hrs {1:00} min {2:00}.{3:00} secs",
                    elapsedTimeSpan.Hours, elapsedTimeSpan.Minutes, elapsedTimeSpan.Seconds,
                    elapsedTimeSpan.Milliseconds / 10);

            logger.LogInformation(String.Format("{0} LoadDatabaseFromCsv finished in {1}", DateTime.Now.ToLongTimeString(), elapsedTimeFormatted));

            return (elapsedTimeSpan);
        }

        public void RunPostLoadStoredProcedure(string postLoadStoredProcedureName, string connectionString)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand(postLoadStoredProcedureName, sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    sqlConnection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public static List<Task> TaskList = new List<Task>();

        public static async Task LoadFileToDbAsync(string connectionString, ILogger logger, string fileName, int fileNumber, int numberOfFieldCols, string targetTable, int sqlLoadBatchSize, string onlyLoadThisRecordType, bool applyStagingFilter, bool actionTheLoad, StreamWriter stagingFilterOutputFileWriter, bool changeOnlyUpdates)
        {
            try
            {
                logger.LogInformation(String.Format("{0} LoadDatabaseFromCsv started processing file #{1} {2}", DateTime.Now.ToLongTimeString(), fileNumber, fileName.Split('\\').Last()));

                var dataTable = new DataTable("Source");

                if (actionTheLoad)
                {
                    dataTable.Columns.Add("RecordIdentifier");

                    for (var i = 1; i <= numberOfFieldCols; i++)
                    {
                        dataTable.Columns.Add("Field" + i);
                    }
                }

                SqlConnection changeOnlyUpdatesSqlConnection = new SqlConnection(connectionString);

                changeOnlyUpdatesSqlConnection.Open();

                int lineCount = 0;
                foreach (string line in System.IO.File.ReadAllLines(fileName))
                {
                    string recordIdentifier = line.Split(',')[0].Replace("\"", string.Empty);

                    if ((onlyLoadThisRecordType != string.Empty) && (recordIdentifier != onlyLoadThisRecordType))
                    {
                        continue;
                    }

                    lineCount++;

                    List<string> fields = new List<string>();

                    if (changeOnlyUpdates)
                    {
                        ActionChangeOnlyUpdate(recordIdentifier, line, changeOnlyUpdatesSqlConnection);
                    }
                    else
                    {
                        fields = ExtractFieldsFromRecord(recordIdentifier, line);

                        // If applying a filter, check that key field for this record type is in the staging filter before loading it
                        if (!applyStagingFilter || (applyStagingFilter && RecordQualifiesForLoading(fields.Take(5).ToList())))
                        {
                            if (actionTheLoad)
                            {
                                DataRow dataRow = dataTable.NewRow();
                                dataRow.ItemArray = fields.ToArray();
                                dataTable.Rows.Add(dataRow);
                            }

                            if (applyStagingFilter)
                            {
                                // record was selected by filtering, so write it to a new csv file to expedite the next load
                                stagingFilterOutputFileWriter.WriteLine(line);
                                stagingFilterOutputFileWriter.Flush();
                            }
                        }
                    }
                }

                changeOnlyUpdatesSqlConnection.Close();

                if (actionTheLoad && !changeOnlyUpdates)
                {
                    int totalRowCount = dataTable.Rows.Count;

                    logger.LogInformation(String.Format("{0} LoadDatabaseFromCsv finished building DataTable for file#{1} {2}", DateTime.Now.ToLongTimeString(), fileNumber, fileName.Split('\\').Last()));

                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(connectionString, SqlBulkCopyOptions.UseInternalTransaction | SqlBulkCopyOptions.TableLock))
                    {
                        sqlBulkCopy.DestinationTableName = targetTable;
                        sqlBulkCopy.BatchSize = sqlLoadBatchSize; // with UseInternalTransaction, will write these as a separate transaction
                        sqlBulkCopy.NotifyAfter = totalRowCount;
                        sqlBulkCopy.SqlRowsCopied += (sender, eventArgs) => logger.LogInformation(String.Format("{0} LoadDatabaseFromCsv finishing loading a file of {1} rows", DateTime.Now.ToLongTimeString(), eventArgs.RowsCopied));
                        sqlBulkCopy.BulkCopyTimeout = 0;
                        await sqlBulkCopy.WriteToServerAsync(dataTable);
                    }
                }

                dataTable.Dispose();

                logger.LogInformation(String.Format("{0} LoadDatabaseFromCsv finished processing file #{1}, {2}", DateTime.Now.ToLongTimeString(), fileNumber, fileName.Split('\\').Last()));
            }
            catch (InvalidOperationException ex)
            {
                logger.LogError(String.Format("LoadDatabaseFromCsv InvalidOperationException {0}", ex.Message));
            }

            bool RecordQualifiesForLoading(List<string> fieldsContainingPrimaryKeys)
            {
                switch (fieldsContainingPrimaryKeys[0]) // RecordIdentifier
                {
                    case "11": // Street
                    case "15": // StreetDescriptor
                        return StagingFilterKeys.StagingUsrns.Contains(fieldsContainingPrimaryKeys[3]); 
                    case "21": // BasicLandAndPropertyUnit
                        return StagingFilterKeys.StagingUprns.Contains(fieldsContainingPrimaryKeys[3]); 
                    case "23": // ApplicationCrossReference
                        return StagingFilterKeys.StagingXrefKeys.Contains(fieldsContainingPrimaryKeys[4]);
                    case "24": // LandPropertyIdentier
                        return StagingFilterKeys.StagingLpiKeys.Contains(fieldsContainingPrimaryKeys[4]); 
                    case "28": // DeliveryPointAddress
                        return StagingFilterKeys.StagingUdprns.Contains(fieldsContainingPrimaryKeys[4]);
                    case "31": // Organisation
                        return StagingFilterKeys.StagingOrgKeys.Contains(fieldsContainingPrimaryKeys[4]); 
                    case "32": // Classification
                        return StagingFilterKeys.StagingClassKeys.Contains(fieldsContainingPrimaryKeys[4]);
                    default: // 10 Header, 29 Metadata, 99 Trailer - just ignore these for now as not useful
                        return false;
                }
            }

            List<string> ExtractFieldsFromRecord(string recordIdentifier, string line)
            {
                List<string> fields = new List<string>();

                // Can safely apply simple comma-delimited to these record types
                if (new string[] { "10", "11", "21", "23", "32", "99" }.Contains(recordIdentifier))
                {
                    // Remove leading/trailing double quotes from each field
                    return line.Split(',').Select(x => x.TrimStart('"').TrimEnd('"')).ToList<string>();
                }

                // These record types can contain string fields with embedded commas
                // These are delimited by escaped double-quotes \"blah,blah\" but not every field, 
                // so a comma can be both field contents and a field separator, depending on whether it's inside one of those fields or not
                // 15, 24, 28, 29, 31
                string thisField = string.Empty;
                bool insideString = false;

                for (int letterIndex = 0; letterIndex <= (line.Length - 1); letterIndex++)
                {
                    if (insideString)
                    {
                        if (line[letterIndex] == '\"') // && letterIndex <= (line.Length - 1) && line[letterIndex + 1] == '"')
                        {
                            insideString = false;
                            continue;
                        }

                        // inside a string, write everything including commas into same field
                        thisField = thisField + line[letterIndex];
                    }
                    else
                    {
                        // outside of a string, found start of new string
                        if (line[letterIndex] == '\"')
                        {
                            insideString = true;
                            continue;
                        }
                        else
                        {
                            // outside of a string, a comma delimits a field
                            if (line[letterIndex] == ',')
                            {
                                fields.Add(thisField);
                                thisField = string.Empty;
                            }
                            else
                            {
                                thisField = thisField + line[letterIndex];
                            }
                        }
                    }
                }

                // write last field as we got end of line rather than a delimiting comma
                fields.Add(thisField);
                return fields;
            }

            void ActionChangeOnlyUpdate(string recordIdentifier, string line, SqlConnection changeOnlyUpdatesSqlConnection)
            {
                // Can safely apply simple comma-delimited to these record types
                // "10", "11", "21", "23", "32", "99"
                List<string> fields = line.Split(',').Select(x => x.TrimStart('"').TrimEnd('"')).ToList<string>();

                string changeType = fields[1];
                string processingOrder = fields[2];
                string tableName, primaryKeyName, primaryKeyValue, updateStatement = string.Empty;
                string insertStatement = string.Empty;

                if (!new string[] { "I", "U", "D" }.Contains(changeType))
                {
                    return;
                }

                switch (recordIdentifier)
                {
                    case "11":
                        tableName = "Street";
                        primaryKeyName = "Usrn";
                        primaryKeyValue = fields[3];
                        insertStatement = changeType == "I" ? string.Format("INSERT INTO dbo.Street" +
                            " (RecordIdentifier, ChangeType, ProOrder, Usrn, RecordType, SwaOrgRefNaming, State, StateDate, StreetSurface, StreetClassification, Version, StreetStartDate, StreetEndDate, LastUpdateDate, RecordEntryDate, StreetStartX," +
                            " StreetStartY, StreetStartLat, StreetStartLong, StreetEndX, StreetEndY, StreetEndLat, StreetEndLong, StreetTolerance, LoadId) VALUES (" +
                            " CONVERT(smallint, '{0}'), CONVERT(nvarchar(1), '{1}'), CONVERT(bigint, '{2}'), CONVERT(int, '{3}'), CONVERT(smallint, '{4}'), CONVERT(int, '{5}'), CONVERT(smallint, '{6}')," +
                            " CONVERT(datetime2, CASE WHEN '{7}' = '' THEN null ELSE '{7}' END), CONVERT(smallint, '{8}'), CONVERT(smallint, '{9}'), CONVERT(smallint, '{10}'), CONVERT(datetime2, CASE WHEN '{11}' = '' THEN null ELSE '{11}' END)," +
                            " CONVERT(datetime2, CASE WHEN '{12}' = '' THEN null ELSE '{12}' END), CONVERT(datetime2, CASE WHEN '{13}' = '' THEN null ELSE '{13}' END), CONVERT(datetime2, CASE WHEN '{14}' = '' THEN null ELSE '{14}' END)," +
                            " CONVERT(real, '{15}'), CONVERT(real, '{16}'), CONVERT(real, '{17}'), CONVERT(real, '{18}'), CONVERT(real, '{19}'), CONVERT(real, '{20}'), CONVERT(real, '{21}'), CONVERT(real, '{22}'), CONVERT(smallint, '{23}'), {24} );",
                            recordIdentifier, changeType, processingOrder, primaryKeyValue, fields[4], fields[5], fields[6], fields[7], fields[8],
                            fields[9], fields[10], fields[11], fields[12], fields[13], fields[14], fields[15], fields[16], fields[17], 
                            fields[18], fields[19], fields[20], fields[21], fields[22], fields[23], 2) : string.Empty;
                        updateStatement = changeType == "U" ? string.Format("Update dbo.Street SET" +
                            " RecordIdentifier = CONVERT(smallint, '{0}')," +
                            " ChangeType = CONVERT(nvarchar(1), '{1}')," +
                            " ProOrder = CONVERT(bigint, '{2}')," +
                            " Usrn = CONVERT(int, '{3}')," +
                            " RecordType = CONVERT(smallint, '{4}')," +
                            " SwaOrgRefNaming = CONVERT(int, '{5}')," +
                            " State = CONVERT(smallint, '{6}')," +
                            " StateDate = CONVERT(datetime2, CASE WHEN '{7}' = '' THEN null ELSE '{7}' END)," +
                            " StreetSurface = CONVERT(smallint, '{8}')," +
                            " StreetClassification = CONVERT(smallint, '{9}')," +
                            " Version = CONVERT(smallint, '{10}'), " +
                            " StreetStartDate = CONVERT(datetime2, CASE WHEN '{11}' = '' THEN null ELSE '{11}' END)," +
                            " StreetEndDate = CONVERT(datetime2, CASE WHEN '{12}' = '' THEN null ELSE '{12}' END)," +
                            " LastUpdateDate = CONVERT(datetime2, CASE WHEN '{13}' = '' THEN null ELSE '{13}' END)," +
                            " RecordEntryDate = CONVERT(datetime2, CASE WHEN '{14}' = '' THEN null ELSE '{14}' END)," +
                            " StreetStartX = CONVERT(real, '{15}')," +
                            " StreetStartY = CONVERT(real, '{16}')," +
                            " StreetStartLat = CONVERT(real, '{17}')," +
                            " StreetStartLong = CONVERT(real, '{18}')," +
                            " StreetEndX = CONVERT(real, '{19}')," +
                            " StreetEndY = CONVERT(real, '{20}')," +
                            " StreetEndLat = CONVERT(real, '{21}')," +
                            " StreetEndLong = CONVERT(real, '{22}')," +
                            " StreetTolerance = CONVERT(smallint, '{23}')," +
                            " LoadId = 2" +
                            " WHERE Usrn = {24};",
                            recordIdentifier, changeType, processingOrder, primaryKeyValue,
                            fields[4], fields[5], fields[6], fields[7], fields[8], fields[9], fields[10], fields[11], fields[12],
                            fields[13], fields[14], fields[15], fields[16], fields[17], fields[18], fields[19], fields[20], fields[21],
                            fields[22], fields[23], primaryKeyValue) : string.Empty;
                        break;
                    case "15":  
                        tableName = "StreetDescriptor";
                        primaryKeyName = "Usrn"; // Not in fact the primary key and could be more than one
                        primaryKeyValue = fields[3];
                        break;
                    case "21":  
                        tableName = "BasicLandAndPropertyUnit";
                        primaryKeyName = "Uprn";
                        primaryKeyValue = fields[3];
                        break;
                    case "23":  
                        tableName = "ApplicationCrossReference";
                        primaryKeyName = "XrefKey";
                        primaryKeyValue = "'" + fields[4] + "'";
                        break;
                    case "24":
                        tableName = "LandPropertyIdentifier";
                        primaryKeyName = "LpiKey";
                        primaryKeyValue = "'" + fields[4] + "'";
                        break;
                    case "28":  
                        tableName = "DeliveryPointAddress";
                        primaryKeyName = "Udprn";
                        primaryKeyValue = fields[4];
                        break;
                    case "31":  
                        tableName = "Organisation";
                        primaryKeyName = "OrgKey";
                        primaryKeyValue = "'" + fields[4] + "'";
                        break;
                    case "32": 
                        tableName = "Classification";
                        primaryKeyName = "ClassKey";
                        primaryKeyValue = "'" + fields[4] + "'";
                        break;
                    default: // 10 Header, 29 Metadata, 99 Trailer - just ignore these for now as not useful
                        return;
                }

                string sqlStatement = string.Empty;

                switch (changeType)
                {
                    case "D":
                        sqlStatement = String.Format("DELETE TOP (1) FROM dbo.{0} WHERE {1} = {2};", tableName, primaryKeyName, primaryKeyValue);
                        break;
                    case "U":
                        sqlStatement = updateStatement;
                        break;
                    default:
                        sqlStatement = insertStatement;
                        break;
                }

                try
                {
                    using (var sqlCommand = new SqlCommand(sqlStatement, changeOnlyUpdatesSqlConnection))
                    {
                        sqlCommand.ExecuteNonQuery();

                    }
                }
                catch (Exception ex)
                {
                    logger.LogWarning(String.Format("{0} LoadDatabaseFromCsv ActionChangeOnlyUpdate SQL caused exception so record was not applied {1}", DateTime.Now.ToLongTimeString(), sqlStatement));
                    logger.LogWarning(String.Format("{0} LoadDatabaseFromCsv Exception details = {1}", DateTime.Now.ToLongTimeString(), ex.Message));
                }

            }
        }

        public void LoadStagingFilterKeys(string stagingFilterTable, string connectionString)
        {
            using (var sqlConnectionForFilterLookup = new SqlConnection(connectionString))
            {
                sqlConnectionForFilterLookup.Open();

                using (var sqlCommand = new SqlCommand(String.Format("SELECT Uprn, Usrn, LpiKey, XrefKey, ClassKey, Udprn, OrgKey FROM {0}", stagingFilterTable), sqlConnectionForFilterLookup))
                {
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(0))
                            {
                                StagingFilterKeys.StagingUprns.Add(Convert.ToString(Convert.ToString(reader.GetInt64(0))));
                                continue;
                            }
                            if (!reader.IsDBNull(1))
                            {
                                StagingFilterKeys.StagingUsrns.Add(Convert.ToString(Convert.ToString(reader.GetInt32(1))));
                                continue;
                            }
                            if (!reader.IsDBNull(2))
                            {
                                StagingFilterKeys.StagingLpiKeys.Add(Convert.ToString(reader.GetString(2)));
                                continue;
                            }
                            if (!reader.IsDBNull(3))
                            {
                                StagingFilterKeys.StagingXrefKeys.Add(Convert.ToString(reader.GetString(3)));
                                continue;
                            }
                            if (!reader.IsDBNull(4))
                            {
                                StagingFilterKeys.StagingClassKeys.Add(Convert.ToString(reader.GetString(4)));
                                continue;
                            }
                            if (!reader.IsDBNull(5))
                            {
                                StagingFilterKeys.StagingUdprns.Add(Convert.ToString(Convert.ToString(reader.GetInt64(5))));
                                continue;
                            }
                            if (!reader.IsDBNull(6))
                            {
                                StagingFilterKeys.StagingOrgKeys.Add(Convert.ToString(reader.GetString(6)));
                                continue;
                            }
                        }
                    }
                }

                sqlConnectionForFilterLookup.Close();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
