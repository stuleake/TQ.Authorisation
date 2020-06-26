using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TQ.Geocoding.DataLoad.Migrations
{
    public partial class LandPropertyIdentifier : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<short>(
                name: "SaoStartNumber",
                table: "LandPropertyIdentifier",
                nullable: true,
                oldClrType: typeof(short));

            migrationBuilder.AlterColumn<short>(
                name: "SaoEndNumber",
                table: "LandPropertyIdentifier",
                nullable: true,
                oldClrType: typeof(short));

            migrationBuilder.AlterColumn<long>(
                name: "PaoStartNumber",
                table: "LandPropertyIdentifier",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<long>(
                name: "PaoEndNumber",
                table: "LandPropertyIdentifier",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<bool>(
                name: "OfficialFlag",
                table: "LandPropertyIdentifier",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<string>(
                name: "Language",
                table: "LandPropertyIdentifier",
                maxLength: 3,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 3,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "LandPropertyIdentifier",
                nullable: true,
                oldClrType: typeof(DateTime));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<short>(
                name: "SaoStartNumber",
                table: "LandPropertyIdentifier",
                nullable: false,
                oldClrType: typeof(short),
                oldNullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "SaoEndNumber",
                table: "LandPropertyIdentifier",
                nullable: false,
                oldClrType: typeof(short),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "PaoStartNumber",
                table: "LandPropertyIdentifier",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "PaoEndNumber",
                table: "LandPropertyIdentifier",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "OfficialFlag",
                table: "LandPropertyIdentifier",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Language",
                table: "LandPropertyIdentifier",
                maxLength: 3,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 3);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "LandPropertyIdentifier",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }
    }
}
