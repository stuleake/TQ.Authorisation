using Microsoft.EntityFrameworkCore.Migrations;

namespace TQ.Geocoding.DataLoad.Migrations
{
    public partial class Field30Staging : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Field30",
                table: "Staging",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Field30",
                table: "Staging");
        }
    }
}
