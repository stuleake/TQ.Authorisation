using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TQ.Geocoding.DataLoad.Migrations
{
    public partial class SearchableGeographicId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SearchableGeographic",
                table: "SearchableGeographic");

            migrationBuilder.AlterColumn<string>(
                name: "SaoText",
                table: "SearchableGeographic",
                maxLength: 90,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 90);

            migrationBuilder.AddColumn<long>(
                name: "SearchableGeographicId",
                table: "SearchableGeographic",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SearchableGeographic",
                table: "SearchableGeographic",
                column: "SearchableGeographicId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SearchableGeographic",
                table: "SearchableGeographic");

            migrationBuilder.DropColumn(
                name: "SearchableGeographicId",
                table: "SearchableGeographic");

            migrationBuilder.AlterColumn<string>(
                name: "SaoText",
                table: "SearchableGeographic",
                maxLength: 90,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 90,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SearchableGeographic",
                table: "SearchableGeographic",
                column: "SaoText");
        }
    }
}
