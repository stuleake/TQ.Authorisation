using Microsoft.EntityFrameworkCore.Migrations;

namespace TQ.Geocoding.DataLoad.Migrations
{
    public partial class Field31thru31Staging : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Field31",
                table: "Staging",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Field32",
                table: "Staging",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Field33",
                table: "Staging",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Field34",
                table: "Staging",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Field35",
                table: "Staging",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Field31",
                table: "Staging");

            migrationBuilder.DropColumn(
                name: "Field32",
                table: "Staging");

            migrationBuilder.DropColumn(
                name: "Field33",
                table: "Staging");

            migrationBuilder.DropColumn(
                name: "Field34",
                table: "Staging");

            migrationBuilder.DropColumn(
                name: "Field35",
                table: "Staging");
        }
    }
}
