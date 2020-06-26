using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using TQ.Geocoding.DataLoad.Models.Context;
using Microsoft.Extensions.Configuration;


namespace TQ.Geocoding.DataLoad.Models.Context
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<GeocodingDataLoadContext>
    {
        public GeocodingDataLoadContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<GeocodingDataLoadContext>();

            var runParameters = configuration.GetSection("RunParameters");
            string connectionString = runParameters["ConnectionString"];

            builder.UseSqlServer(connectionString);

            return new GeocodingDataLoadContext(builder.Options);
        }
    }
}
