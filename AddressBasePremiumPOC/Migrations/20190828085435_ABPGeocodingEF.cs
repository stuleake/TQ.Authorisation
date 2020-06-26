using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TQ.Geocoding.DataLoad.Migrations
{
    public partial class ABPGeocodingEF : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LandAndPropertyIdentifier");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StreetDescriptor",
                table: "StreetDescriptor");

            migrationBuilder.RenameColumn(
                name: "BlupStateDate",
                table: "BasicLandAndPropertyUnit",
                newName: "BlpuStateDate");

            migrationBuilder.RenameColumn(
                name: "BlupState",
                table: "BasicLandAndPropertyUnit",
                newName: "BlpuState");

            migrationBuilder.AlterColumn<string>(
                name: "Language",
                table: "StreetDescriptor",
                maxLength: 3,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 3,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RecordEntryDate",
                table: "Street",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_StreetDescriptor",
                table: "StreetDescriptor",
                columns: new[] { "StreetDescription", "Locality", "TownName", "AdminstrativeArea", "Language", "Usrn" });

            migrationBuilder.CreateTable(
                name: "LandPropertyIdentifier",
                columns: table => new
                {
                    RecordIdentifier = table.Column<short>(nullable: false),
                    ChangeType = table.Column<string>(nullable: false),
                    ProOrder = table.Column<long>(nullable: false),
                    Uprn = table.Column<long>(nullable: false),
                    LpiKey = table.Column<string>(maxLength: 14, nullable: false),
                    Language = table.Column<string>(maxLength: 3, nullable: true),
                    LogicalStatus = table.Column<short>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    EntryDate = table.Column<DateTime>(nullable: false),
                    SaoStartNumber = table.Column<short>(nullable: false),
                    SaoStartSuffix = table.Column<string>(maxLength: 2, nullable: true),
                    SaoEndNumber = table.Column<short>(nullable: false),
                    SaoEndSuffix = table.Column<string>(maxLength: 2, nullable: true),
                    SaoText = table.Column<string>(maxLength: 90, nullable: true),
                    PaoStartNumber = table.Column<long>(nullable: false),
                    PaoStartSuffix = table.Column<string>(maxLength: 2, nullable: true),
                    PaoEndNumber = table.Column<long>(nullable: false),
                    PaoEndSuffix = table.Column<string>(maxLength: 2, nullable: true),
                    PaoText = table.Column<string>(maxLength: 90, nullable: true),
                    Usrn = table.Column<int>(nullable: false),
                    UsrnMatchIndicator = table.Column<short>(nullable: false),
                    AreaName = table.Column<string>(maxLength: 40, nullable: true),
                    Level = table.Column<string>(maxLength: 30, nullable: true),
                    OfficialFlag = table.Column<bool>(nullable: false),
                    LoadId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandPropertyIdentifier", x => x.LpiKey);
                    table.ForeignKey(
                        name: "FK_LandPropertyIdentifier_BLPU",
                        column: x => x.Uprn,
                        principalTable: "BasicLandAndPropertyUnit",
                        principalColumn: "Uprn",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LandPropertyIdentifier_Street",
                        column: x => x.Usrn,
                        principalTable: "Street",
                        principalColumn: "Usrn",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LandPropertyIdentifier_Uprn",
                table: "LandPropertyIdentifier",
                column: "Uprn");

            migrationBuilder.CreateIndex(
                name: "IX_LandPropertyIdentifier_Usrn",
                table: "LandPropertyIdentifier",
                column: "Usrn");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LandPropertyIdentifier");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StreetDescriptor",
                table: "StreetDescriptor");

            migrationBuilder.DropColumn(
                name: "RecordEntryDate",
                table: "Street");

            migrationBuilder.RenameColumn(
                name: "BlpuStateDate",
                table: "BasicLandAndPropertyUnit",
                newName: "BlupStateDate");

            migrationBuilder.RenameColumn(
                name: "BlpuState",
                table: "BasicLandAndPropertyUnit",
                newName: "BlupState");

            migrationBuilder.AlterColumn<string>(
                name: "Language",
                table: "StreetDescriptor",
                maxLength: 3,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 3);

            migrationBuilder.AddPrimaryKey(
                name: "PK_StreetDescriptor",
                table: "StreetDescriptor",
                columns: new[] { "StreetDescription", "Locality", "TownName", "AdminstrativeArea" });

            migrationBuilder.CreateTable(
                name: "LandAndPropertyIdentifier",
                columns: table => new
                {
                    LpiKey = table.Column<string>(maxLength: 14, nullable: false),
                    AreaName = table.Column<string>(maxLength: 40, nullable: true),
                    ChangeType = table.Column<string>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    EntryDate = table.Column<DateTime>(nullable: false),
                    Language = table.Column<string>(maxLength: 3, nullable: true),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    Level = table.Column<string>(maxLength: 30, nullable: true),
                    LoadId = table.Column<int>(nullable: false),
                    LogicalStatus = table.Column<short>(nullable: false),
                    OfficialFlag = table.Column<bool>(nullable: false),
                    PaoEndNumber = table.Column<long>(nullable: false),
                    PaoEndSuffix = table.Column<string>(maxLength: 2, nullable: true),
                    PaoStartNumber = table.Column<long>(nullable: false),
                    PaoStartSuffix = table.Column<string>(maxLength: 2, nullable: true),
                    PaoText = table.Column<string>(maxLength: 90, nullable: true),
                    ProOrder = table.Column<long>(nullable: false),
                    RecordIdentifier = table.Column<short>(nullable: false),
                    SaoEndNumber = table.Column<short>(nullable: false),
                    SaoEndSuffix = table.Column<string>(maxLength: 2, nullable: true),
                    SaoStartNumber = table.Column<short>(nullable: false),
                    SaoStartSuffix = table.Column<string>(maxLength: 2, nullable: true),
                    SaoText = table.Column<string>(maxLength: 90, nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    Uprn = table.Column<long>(nullable: false),
                    Usrn = table.Column<int>(nullable: false),
                    UsrnMatchIndicator = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandAndPropertyIdentifier", x => x.LpiKey);
                    table.ForeignKey(
                        name: "FK_LandAndPropertyIdentifier_BLPU",
                        column: x => x.Uprn,
                        principalTable: "BasicLandAndPropertyUnit",
                        principalColumn: "Uprn",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LandAndPropertyIdentifier_Street",
                        column: x => x.Usrn,
                        principalTable: "Street",
                        principalColumn: "Usrn",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LandAndPropertyIdentifier_Uprn",
                table: "LandAndPropertyIdentifier",
                column: "Uprn");

            migrationBuilder.CreateIndex(
                name: "IX_LandAndPropertyIdentifier_Usrn",
                table: "LandAndPropertyIdentifier",
                column: "Usrn");
        }
    }
}
