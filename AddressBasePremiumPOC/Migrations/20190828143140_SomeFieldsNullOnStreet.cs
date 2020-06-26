using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TQ.Geocoding.DataLoad.Migrations
{
    public partial class SomeFieldsNullOnStreet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<short>(
                name: "StreetSurface",
                table: "Street",
                nullable: true,
                oldClrType: typeof(short));

            migrationBuilder.AlterColumn<short>(
                name: "StreetClassification",
                table: "Street",
                nullable: true,
                oldClrType: typeof(short));

            migrationBuilder.AlterColumn<DateTime>(
                name: "StateDate",
                table: "Street",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<short>(
                name: "State",
                table: "Street",
                nullable: true,
                oldClrType: typeof(short));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<short>(
                name: "StreetSurface",
                table: "Street",
                nullable: false,
                oldClrType: typeof(short),
                oldNullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "StreetClassification",
                table: "Street",
                nullable: false,
                oldClrType: typeof(short),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "StateDate",
                table: "Street",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "State",
                table: "Street",
                nullable: false,
                oldClrType: typeof(short),
                oldNullable: true);
        }
    }
}
