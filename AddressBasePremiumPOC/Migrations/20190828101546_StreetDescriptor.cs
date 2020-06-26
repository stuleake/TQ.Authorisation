using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TQ.Geocoding.DataLoad.Migrations
{
    public partial class StreetDescriptor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_StreetDescriptor",
                table: "StreetDescriptor");

            migrationBuilder.DropIndex(
                name: "IX_StreetDescriptor_Usrn",
                table: "StreetDescriptor");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "StreetDescriptor",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AddPrimaryKey(
                name: "PK_StreetDescriptor",
                table: "StreetDescriptor",
                columns: new[] { "Usrn", "Language" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_StreetDescriptor",
                table: "StreetDescriptor");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "StreetDescriptor",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_StreetDescriptor",
                table: "StreetDescriptor",
                columns: new[] { "StreetDescription", "Locality", "TownName", "AdminstrativeArea", "Language", "Usrn" });

            migrationBuilder.CreateIndex(
                name: "IX_StreetDescriptor_Usrn",
                table: "StreetDescriptor",
                column: "Usrn");
        }
    }
}
