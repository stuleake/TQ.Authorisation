using Microsoft.EntityFrameworkCore.Migrations;

namespace TQ.Geocoding.DataLoad.Migrations
{
    public partial class Field36thru40Staging : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Field36",
                table: "Staging",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Field37",
                table: "Staging",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Field38",
                table: "Staging",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Field39",
                table: "Staging",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Field40",
                table: "Staging",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Field36",
                table: "Staging");

            migrationBuilder.DropColumn(
                name: "Field37",
                table: "Staging");

            migrationBuilder.DropColumn(
                name: "Field38",
                table: "Staging");

            migrationBuilder.DropColumn(
                name: "Field39",
                table: "Staging");

            migrationBuilder.DropColumn(
                name: "Field40",
                table: "Staging");
        }
    }
}
