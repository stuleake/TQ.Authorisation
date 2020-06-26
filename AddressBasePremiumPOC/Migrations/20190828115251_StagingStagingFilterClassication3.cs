using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TQ.Geocoding.DataLoad.Migrations
{
    public partial class StagingStagingFilterClassication3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Classification",
                table: "Classification");

            migrationBuilder.DropColumn(
                name: "Field29",
                table: "Staging");

            migrationBuilder.DropColumn(
                name: "Field30",
                table: "Staging");

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

            migrationBuilder.DropColumn(
                name: "ClassKey",
                table: "Classification");

            migrationBuilder.AlterColumn<string>(
                name: "RecordIdentifier",
                table: "Staging",
                maxLength: 2,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 2,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClassificationKey",
                table: "Classification",
                maxLength: 14,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Classification",
                table: "Classification",
                column: "ClassificationKey");

            migrationBuilder.CreateTable(
                name: "StagingFilter",
                columns: table => new
                {
                    StagingFilterId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Uprn = table.Column<long>(nullable: true),
                    Usrn = table.Column<int>(nullable: true),
                    LpiKey = table.Column<string>(maxLength: 14, nullable: true),
                    XrefKey = table.Column<string>(maxLength: 14, nullable: true),
                    ClassificationKey = table.Column<string>(maxLength: 14, nullable: true),
                    Udprn = table.Column<long>(nullable: true),
                    OrgKey = table.Column<string>(maxLength: 14, nullable: true),
                    HeaderId = table.Column<long>(nullable: true),
                    MetadataId = table.Column<long>(nullable: true),
                    TrailerId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StagingFilter", x => x.StagingFilterId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StagingFilter");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Classification",
                table: "Classification");

            migrationBuilder.DropColumn(
                name: "ClassificationKey",
                table: "Classification");

            migrationBuilder.AlterColumn<string>(
                name: "RecordIdentifier",
                table: "Staging",
                maxLength: 2,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 2);

            migrationBuilder.AddColumn<string>(
                name: "Field29",
                table: "Staging",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Field30",
                table: "Staging",
                maxLength: 100,
                nullable: true);

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

            migrationBuilder.AddColumn<string>(
                name: "ClassKey",
                table: "Classification",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Classification",
                table: "Classification",
                column: "ClassKey");
        }
    }
}
