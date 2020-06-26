using Microsoft.EntityFrameworkCore.Migrations;

namespace TQ.Geocoding.DataLoad.Migrations
{
    public partial class SearchableTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UX_SearchableAddress_RQPAddress",
                table: "SearchableAddress");

            migrationBuilder.RenameColumn(
                name: "AdminstrativeArea",
                table: "SearchableGeographic",
                newName: "AdministrativeArea");

            migrationBuilder.RenameIndex(
                name: "IX_SearchableAddress_RQPPostcode",
                table: "SearchableAddress",
                newName: "UX_SearchableAddress_Postcode");

            migrationBuilder.AlterColumn<short>(
                name: "SaoStartNumber",
                table: "SearchableGeographic",
                nullable: true,
                oldClrType: typeof(short));

            migrationBuilder.AlterColumn<short>(
                name: "SaoEndNumber",
                table: "SearchableGeographic",
                nullable: true,
                oldClrType: typeof(short));

            migrationBuilder.AlterColumn<short>(
                name: "PaoStartNumber",
                table: "SearchableGeographic",
                nullable: true,
                oldClrType: typeof(short));

            migrationBuilder.AlterColumn<short>(
                name: "PaoEndNumber",
                table: "SearchableGeographic",
                nullable: true,
                oldClrType: typeof(short));

            migrationBuilder.AlterColumn<int>(
                name: "BuildingNumber",
                table: "SearchableAddress",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "AddressLine1",
                table: "SearchableAddress",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressLine2",
                table: "SearchableAddress",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressLine3",
                table: "SearchableAddress",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SingleLineAddress",
                table: "SearchableAddress",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WelshAddressLine1",
                table: "SearchableAddress",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WelshAddressLine2",
                table: "SearchableAddress",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WelshAddressLine3",
                table: "SearchableAddress",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WelshSingleLineAddress",
                table: "SearchableAddress",
                maxLength: 500,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SearchableAddress_PostcodeNoSpaces",
                table: "SearchableAddress",
                column: "PostcodeNoSpaces");

            migrationBuilder.CreateIndex(
                name: "IX_SearchableAddress_SingleLineAddress",
                table: "SearchableAddress",
                column: "SingleLineAddress");

            migrationBuilder.CreateIndex(
                name: "IX_SearchableAddress_WelshSingleLineAddress",
                table: "SearchableAddress",
                column: "WelshSingleLineAddress");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SearchableAddress_PostcodeNoSpaces",
                table: "SearchableAddress");

            migrationBuilder.DropIndex(
                name: "IX_SearchableAddress_SingleLineAddress",
                table: "SearchableAddress");

            migrationBuilder.DropIndex(
                name: "IX_SearchableAddress_WelshSingleLineAddress",
                table: "SearchableAddress");

            migrationBuilder.DropColumn(
                name: "AddressLine1",
                table: "SearchableAddress");

            migrationBuilder.DropColumn(
                name: "AddressLine2",
                table: "SearchableAddress");

            migrationBuilder.DropColumn(
                name: "AddressLine3",
                table: "SearchableAddress");

            migrationBuilder.DropColumn(
                name: "SingleLineAddress",
                table: "SearchableAddress");

            migrationBuilder.DropColumn(
                name: "WelshAddressLine1",
                table: "SearchableAddress");

            migrationBuilder.DropColumn(
                name: "WelshAddressLine2",
                table: "SearchableAddress");

            migrationBuilder.DropColumn(
                name: "WelshAddressLine3",
                table: "SearchableAddress");

            migrationBuilder.DropColumn(
                name: "WelshSingleLineAddress",
                table: "SearchableAddress");

            migrationBuilder.RenameColumn(
                name: "AdministrativeArea",
                table: "SearchableGeographic",
                newName: "AdminstrativeArea");

            migrationBuilder.RenameIndex(
                name: "UX_SearchableAddress_Postcode",
                table: "SearchableAddress",
                newName: "IX_SearchableAddress_RQPPostcode");

            migrationBuilder.AlterColumn<short>(
                name: "SaoStartNumber",
                table: "SearchableGeographic",
                nullable: false,
                oldClrType: typeof(short),
                oldNullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "SaoEndNumber",
                table: "SearchableGeographic",
                nullable: false,
                oldClrType: typeof(short),
                oldNullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "PaoStartNumber",
                table: "SearchableGeographic",
                nullable: false,
                oldClrType: typeof(short),
                oldNullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "PaoEndNumber",
                table: "SearchableGeographic",
                nullable: false,
                oldClrType: typeof(short),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BuildingNumber",
                table: "SearchableAddress",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "UX_SearchableAddress_RQPAddress",
                table: "SearchableAddress",
                columns: new[] { "DepartmentName", "OrganisationName", "SubBuildingName", "BuildingName", "BuildingNumber", "PoBoxNumber", "DependentThoroughfare", "ThoroughFare", "DoubleDependentLocality", "DependentLocality", "PostTown" });
        }
    }
}
