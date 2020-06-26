using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TQ.Geocoding.DataLoad.Migrations
{
    public partial class AddStaging : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Staging",
                columns: table => new
                {
                    RecordIdentifier = table.Column<string>(maxLength: 2, nullable: true),
                    Field1 = table.Column<string>(maxLength: 100, nullable: true),
                    Field2 = table.Column<string>(maxLength: 100, nullable: true),
                    Field3 = table.Column<string>(maxLength: 100, nullable: true),
                    Field4 = table.Column<string>(maxLength: 100, nullable: true),
                    Field5 = table.Column<string>(maxLength: 100, nullable: true),
                    Field6 = table.Column<string>(maxLength: 100, nullable: true),
                    Field7 = table.Column<string>(maxLength: 100, nullable: true),
                    Field8 = table.Column<string>(maxLength: 100, nullable: true),
                    Field9 = table.Column<string>(maxLength: 100, nullable: true),
                    Field10 = table.Column<string>(maxLength: 100, nullable: true),
                    Field11 = table.Column<string>(maxLength: 100, nullable: true),
                    Field12 = table.Column<string>(maxLength: 100, nullable: true),
                    Field13 = table.Column<string>(maxLength: 100, nullable: true),
                    Field14 = table.Column<string>(maxLength: 100, nullable: true),
                    Field15 = table.Column<string>(maxLength: 100, nullable: true),
                    Field16 = table.Column<string>(maxLength: 100, nullable: true),
                    Field17 = table.Column<string>(maxLength: 100, nullable: true),
                    Field18 = table.Column<string>(maxLength: 100, nullable: true),
                    Field19 = table.Column<string>(maxLength: 100, nullable: true),
                    Field20 = table.Column<string>(maxLength: 100, nullable: true),
                    Field21 = table.Column<string>(maxLength: 100, nullable: true),
                    Field22 = table.Column<string>(maxLength: 100, nullable: true),
                    Field23 = table.Column<string>(maxLength: 100, nullable: true),
                    Field24 = table.Column<string>(maxLength: 100, nullable: true),
                    Field25 = table.Column<string>(maxLength: 100, nullable: true),
                    Field26 = table.Column<string>(maxLength: 100, nullable: true),
                    Field27 = table.Column<string>(maxLength: 100, nullable: true),
                    Field28 = table.Column<string>(maxLength: 100, nullable: true),
                    Field29 = table.Column<string>(maxLength: 100, nullable: true),
                    Field30 = table.Column<string>(maxLength: 100, nullable: true),
                    StagingRecordId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staging", x => x.StagingRecordId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Staging");
        }
    }
}
