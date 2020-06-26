using Microsoft.EntityFrameworkCore.Migrations;

namespace TQ.Geocoding.DataLoad.Migrations
{
    public partial class IX_Staging_RecordIdentifier : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Staging_RecordIdentifier",
                table: "Staging",
                columns: new[] { "RecordIdentifier", "StagingRecordId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Staging_RecordIdentifier",
                table: "Staging");
        }
    }
}
