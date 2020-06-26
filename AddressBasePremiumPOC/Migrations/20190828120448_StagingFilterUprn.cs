using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TQ.Geocoding.DataLoad.Migrations
{
    public partial class StagingFilterUprn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StagingUprn");

            migrationBuilder.CreateTable(
                name: "StagingFilterUprn",
                columns: table => new
                {
                    FilterUprn = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StagingFilterUprn", x => x.FilterUprn);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StagingFilterUprn");

            migrationBuilder.CreateTable(
                name: "StagingUprn",
                columns: table => new
                {
                    Uprn = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StagingUprn", x => x.Uprn);
                });
        }
    }
}
