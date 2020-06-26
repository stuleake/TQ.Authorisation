using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TQ.Geocoding.DataLoad.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BasicLandAndPropertyUnit",
                columns: table => new
                {
                    RecordIdentifier = table.Column<short>(nullable: false),
                    ChangeType = table.Column<string>(nullable: false),
                    ProOrder = table.Column<long>(nullable: false),
                    Uprn = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LogicalStatus = table.Column<long>(nullable: false),
                    BlupState = table.Column<long>(nullable: false),
                    BlupStateDate = table.Column<DateTime>(nullable: false),
                    ParentUprn = table.Column<long>(nullable: false),
                    XCoordinate = table.Column<float>(nullable: false),
                    YCoordinate = table.Column<float>(nullable: false),
                    Latitude = table.Column<float>(nullable: false),
                    Longitude = table.Column<float>(nullable: false),
                    Rpc = table.Column<int>(nullable: false),
                    LocalCustodianCode = table.Column<int>(nullable: false),
                    Country = table.Column<string>(maxLength: 1, nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    EntryDate = table.Column<DateTime>(nullable: false),
                    PostcodeLocator = table.Column<string>(maxLength: 8, nullable: true),
                    MultiOccCount = table.Column<int>(nullable: false),
                    LoadId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasicLandAndPropertyUnit", x => x.Uprn);
                });

            migrationBuilder.CreateTable(
                name: "Header",
                columns: table => new
                {
                    OriginalFileId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RecordIdentifier = table.Column<short>(nullable: false),
                    CustodianName = table.Column<string>(maxLength: 40, nullable: true),
                    LocalCustodianCode = table.Column<int>(nullable: false),
                    ProcessDate = table.Column<DateTime>(nullable: false),
                    VolumeNumber = table.Column<short>(nullable: false),
                    EntryDate = table.Column<DateTime>(nullable: false),
                    TimeStamp = table.Column<TimeSpan>(nullable: false),
                    Version = table.Column<string>(maxLength: 7, nullable: true),
                    FileType = table.Column<string>(maxLength: 1, nullable: true),
                    LoadId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Header", x => x.OriginalFileId);
                });

            migrationBuilder.CreateTable(
                name: "Metadata",
                columns: table => new
                {
                    OriginalFileId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RecordIdentifier = table.Column<short>(nullable: false),
                    GazName = table.Column<string>(maxLength: 60, nullable: true),
                    GazScope = table.Column<string>(maxLength: 60, nullable: true),
                    TerOfUse = table.Column<string>(maxLength: 60, nullable: true),
                    LinkedData = table.Column<string>(maxLength: 100, nullable: true),
                    GazOwner = table.Column<string>(maxLength: 15, nullable: true),
                    NgazFreq = table.Column<string>(maxLength: 1, nullable: true),
                    CustodianName = table.Column<string>(maxLength: 40, nullable: true),
                    CustodianUprn = table.Column<long>(nullable: false),
                    LocalCustodianCode = table.Column<short>(nullable: false),
                    CoordSystem = table.Column<string>(maxLength: 40, nullable: true),
                    CoordUnit = table.Column<string>(maxLength: 10, nullable: true),
                    MetaDate = table.Column<DateTime>(nullable: false),
                    ClassScheme = table.Column<string>(maxLength: 60, nullable: true),
                    GazDate = table.Column<DateTime>(nullable: false),
                    Language = table.Column<string>(maxLength: 3, nullable: true),
                    CharacterSet = table.Column<string>(maxLength: 30, nullable: true),
                    LoadId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Metadata", x => x.OriginalFileId);
                });

            migrationBuilder.CreateTable(
                name: "Street",
                columns: table => new
                {
                    RecordIdentifier = table.Column<short>(nullable: false),
                    ChangeType = table.Column<string>(nullable: false),
                    ProOrder = table.Column<long>(nullable: false),
                    Usrn = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RecordType = table.Column<short>(nullable: false),
                    SwaOrgRefNaming = table.Column<int>(nullable: false),
                    State = table.Column<short>(nullable: false),
                    StateDate = table.Column<DateTime>(nullable: false),
                    StreetSurface = table.Column<short>(nullable: false),
                    StreetClassification = table.Column<short>(nullable: false),
                    Version = table.Column<short>(nullable: false),
                    StreetStartDate = table.Column<DateTime>(nullable: false),
                    StreetEndDate = table.Column<DateTime>(nullable: false),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    StreetStartX = table.Column<float>(nullable: false),
                    StreetStartY = table.Column<float>(nullable: false),
                    StreetStartLat = table.Column<float>(nullable: false),
                    StreetStartLong = table.Column<float>(nullable: false),
                    StreetEndX = table.Column<float>(nullable: false),
                    StreetEndY = table.Column<float>(nullable: false),
                    StreetEndLat = table.Column<float>(nullable: false),
                    StreetEndLong = table.Column<float>(nullable: false),
                    StreetTolerance = table.Column<short>(nullable: false),
                    LoadId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Street", x => x.Usrn);
                });

            migrationBuilder.CreateTable(
                name: "Trailer",
                columns: table => new
                {
                    OriginalFileId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RecordIdentifier = table.Column<short>(nullable: false),
                    NextVolumeName = table.Column<short>(nullable: false),
                    RecordCount = table.Column<long>(nullable: false),
                    EntryDate = table.Column<DateTime>(nullable: false),
                    TimeStamp = table.Column<TimeSpan>(nullable: false),
                    LoadId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trailer", x => x.OriginalFileId);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationCrossReference",
                columns: table => new
                {
                    RecordIdentifier = table.Column<short>(nullable: false),
                    ChangeType = table.Column<string>(nullable: false),
                    ProOrder = table.Column<long>(nullable: false),
                    Uprn = table.Column<long>(nullable: false),
                    XrefKey = table.Column<string>(maxLength: 14, nullable: false),
                    CrossReference = table.Column<string>(maxLength: 50, nullable: true),
                    Version = table.Column<short>(nullable: false),
                    Source = table.Column<string>(maxLength: 6, nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    EntryDate = table.Column<DateTime>(nullable: false),
                    LoadId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationCrossReference", x => x.XrefKey);
                    table.ForeignKey(
                        name: "FK_ApplicationCrossReference_BLPU",
                        column: x => x.Uprn,
                        principalTable: "BasicLandAndPropertyUnit",
                        principalColumn: "Uprn",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Classification",
                columns: table => new
                {
                    RecordIdentifier = table.Column<short>(nullable: false),
                    ChangeType = table.Column<string>(nullable: false),
                    ProOrder = table.Column<long>(nullable: false),
                    Uprn = table.Column<long>(nullable: false),
                    ClassKey = table.Column<string>(maxLength: 200, nullable: false),
                    ClassificationCode = table.Column<string>(nullable: true),
                    ClassScheme = table.Column<string>(maxLength: 60, nullable: true),
                    SchemeVersion = table.Column<float>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    EntryDate = table.Column<DateTime>(nullable: false),
                    LoadId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classification", x => x.ClassKey);
                    table.ForeignKey(
                        name: "FK_Classification_BLPU",
                        column: x => x.Uprn,
                        principalTable: "BasicLandAndPropertyUnit",
                        principalColumn: "Uprn",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryPointAddress",
                columns: table => new
                {
                    RecordIdentifier = table.Column<short>(nullable: false),
                    ChangeType = table.Column<string>(nullable: false),
                    ProOrder = table.Column<long>(nullable: false),
                    Uprn = table.Column<long>(nullable: false),
                    Udprn = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganisationName = table.Column<string>(maxLength: 60, nullable: true),
                    DepartmentName = table.Column<string>(maxLength: 60, nullable: true),
                    SubBuildingName = table.Column<string>(maxLength: 30, nullable: true),
                    BuildingName = table.Column<string>(maxLength: 50, nullable: true),
                    BuildingNumber = table.Column<int>(nullable: false),
                    DependentThoroughfare = table.Column<string>(maxLength: 80, nullable: true),
                    ThoroughFare = table.Column<string>(maxLength: 80, nullable: true),
                    DoubleDependentLocality = table.Column<string>(maxLength: 35, nullable: true),
                    DependentLocality = table.Column<string>(maxLength: 35, nullable: true),
                    PostTown = table.Column<string>(maxLength: 30, nullable: true),
                    Postcode = table.Column<string>(maxLength: 8, nullable: true),
                    PostcodeType = table.Column<string>(maxLength: 1, nullable: true),
                    DeliveryPointSuffix = table.Column<string>(maxLength: 2, nullable: true),
                    WelshDependentThoroughfare = table.Column<string>(maxLength: 80, nullable: true),
                    WelshThoroughfare = table.Column<string>(maxLength: 80, nullable: true),
                    WelshDoubleDependentLocality = table.Column<string>(maxLength: 35, nullable: true),
                    WelshDependentLocality = table.Column<string>(maxLength: 35, nullable: true),
                    WelshPostTown = table.Column<string>(maxLength: 30, nullable: true),
                    PoBoxNumber = table.Column<string>(maxLength: 6, nullable: true),
                    ProcessDate = table.Column<DateTime>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    EntryDate = table.Column<DateTime>(nullable: false),
                    LoadId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryPointAddress", x => x.Udprn);
                    table.ForeignKey(
                        name: "FK_DeliveryPointAddress_BLPU",
                        column: x => x.Uprn,
                        principalTable: "BasicLandAndPropertyUnit",
                        principalColumn: "Uprn",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Organisation",
                columns: table => new
                {
                    RecordIdentifier = table.Column<short>(nullable: false),
                    ChangeType = table.Column<string>(nullable: false),
                    ProOrder = table.Column<long>(nullable: false),
                    Uprn = table.Column<long>(nullable: false),
                    OrgKey = table.Column<string>(maxLength: 14, nullable: false),
                    OrganisationName = table.Column<string>(maxLength: 100, nullable: true),
                    LegalName = table.Column<string>(maxLength: 60, nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    EntryDate = table.Column<DateTime>(nullable: false),
                    LoadId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organisation", x => x.OrgKey);
                    table.ForeignKey(
                        name: "FK_Organisation_BLPU",
                        column: x => x.Uprn,
                        principalTable: "BasicLandAndPropertyUnit",
                        principalColumn: "Uprn",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SearchableAddress",
                columns: table => new
                {
                    SearchableAddressId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Uprn = table.Column<long>(nullable: false),
                    OrganisationName = table.Column<string>(maxLength: 60, nullable: true),
                    DepartmentName = table.Column<string>(maxLength: 60, nullable: true),
                    SubBuildingName = table.Column<string>(maxLength: 30, nullable: true),
                    BuildingName = table.Column<string>(maxLength: 50, nullable: true),
                    BuildingNumber = table.Column<int>(nullable: false),
                    PoBoxNumber = table.Column<string>(maxLength: 6, nullable: true),
                    DependentThoroughfare = table.Column<string>(maxLength: 80, nullable: true),
                    ThoroughFare = table.Column<string>(maxLength: 80, nullable: true),
                    DoubleDependentLocality = table.Column<string>(maxLength: 35, nullable: true),
                    DependentLocality = table.Column<string>(maxLength: 35, nullable: true),
                    PostTown = table.Column<string>(maxLength: 30, nullable: true),
                    Postcode = table.Column<string>(maxLength: 10, nullable: true),
                    PostcodeNoSpaces = table.Column<string>(maxLength: 8, nullable: true),
                    WelshDependentThoroughfare = table.Column<string>(maxLength: 80, nullable: true),
                    WelshThoroughfare = table.Column<string>(maxLength: 80, nullable: true),
                    WelshDoubleDependentLocality = table.Column<string>(maxLength: 35, nullable: true),
                    WelshDependentLocality = table.Column<string>(maxLength: 35, nullable: true),
                    WelshPostTown = table.Column<string>(maxLength: 30, nullable: true),
                    XCoordinateEasting = table.Column<float>(nullable: false),
                    YCoordinateNorthing = table.Column<float>(nullable: false),
                    Latitude = table.Column<float>(nullable: false),
                    Longitude = table.Column<float>(nullable: false),
                    LoadId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchableAddress", x => x.SearchableAddressId);
                    table.ForeignKey(
                        name: "FK_SearchableAddress_BLPU",
                        column: x => x.Uprn,
                        principalTable: "BasicLandAndPropertyUnit",
                        principalColumn: "Uprn",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SearchableGeographic",
                columns: table => new
                {
                    Uprn = table.Column<long>(nullable: false),
                    SaoText = table.Column<string>(maxLength: 90, nullable: false),
                    SaoStartNumber = table.Column<short>(nullable: false),
                    SaoStartSuffix = table.Column<string>(maxLength: 2, nullable: true),
                    SaoEndNumber = table.Column<short>(nullable: false),
                    SaoEndSuffix = table.Column<string>(maxLength: 2, nullable: true),
                    PaoText = table.Column<string>(maxLength: 90, nullable: true),
                    PaoStartNumber = table.Column<short>(nullable: false),
                    PaoStartSuffix = table.Column<string>(maxLength: 2, nullable: true),
                    PaoEndNumber = table.Column<short>(nullable: false),
                    PaoEndSuffix = table.Column<string>(maxLength: 2, nullable: true),
                    StreetDescription = table.Column<string>(maxLength: 100, nullable: true),
                    Locality = table.Column<string>(maxLength: 35, nullable: true),
                    TownName = table.Column<string>(maxLength: 30, nullable: true),
                    AdminstrativeArea = table.Column<string>(maxLength: 30, nullable: true),
                    PostcodeLocator = table.Column<string>(maxLength: 8, nullable: true),
                    LoadId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchableGeographic", x => x.SaoText);
                    table.ForeignKey(
                        name: "FK_SearchableGeographic_BLPU",
                        column: x => x.Uprn,
                        principalTable: "BasicLandAndPropertyUnit",
                        principalColumn: "Uprn",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LandAndPropertyIdentifier",
                columns: table => new
                {
                    RecordIdentifier = table.Column<short>(nullable: false),
                    ChangeType = table.Column<string>(nullable: false),
                    ProOrder = table.Column<long>(nullable: false),
                    Uprn = table.Column<long>(nullable: false),
                    LpiKey = table.Column<string>(maxLength: 14, nullable: false),
                    Language = table.Column<string>(maxLength: 3, nullable: true),
                    LogicalStatus = table.Column<short>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    EntryDate = table.Column<DateTime>(nullable: false),
                    SaoStartNumber = table.Column<short>(nullable: false),
                    SaoStartSuffix = table.Column<string>(maxLength: 2, nullable: true),
                    SaoEndNumber = table.Column<short>(nullable: false),
                    SaoEndSuffix = table.Column<string>(maxLength: 2, nullable: true),
                    SaoText = table.Column<string>(maxLength: 90, nullable: true),
                    PaoStartNumber = table.Column<long>(nullable: false),
                    PaoStartSuffix = table.Column<string>(maxLength: 2, nullable: true),
                    PaoEndNumber = table.Column<long>(nullable: false),
                    PaoEndSuffix = table.Column<string>(maxLength: 2, nullable: true),
                    PaoText = table.Column<string>(maxLength: 90, nullable: true),
                    Usrn = table.Column<int>(nullable: false),
                    UsrnMatchIndicator = table.Column<short>(nullable: false),
                    AreaName = table.Column<string>(maxLength: 40, nullable: true),
                    Level = table.Column<string>(maxLength: 30, nullable: true),
                    OfficialFlag = table.Column<bool>(nullable: false),
                    LoadId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandAndPropertyIdentifier", x => x.LpiKey);
                    table.ForeignKey(
                        name: "FK_LandAndPropertyIdentifier_BLPU",
                        column: x => x.Uprn,
                        principalTable: "BasicLandAndPropertyUnit",
                        principalColumn: "Uprn",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LandAndPropertyIdentifier_Street",
                        column: x => x.Usrn,
                        principalTable: "Street",
                        principalColumn: "Usrn",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StreetDescriptor",
                columns: table => new
                {
                    RecordIdentifier = table.Column<short>(nullable: false),
                    ChangeType = table.Column<string>(nullable: false),
                    ProOrder = table.Column<long>(nullable: false),
                    Usrn = table.Column<int>(nullable: false),
                    StreetDescription = table.Column<string>(maxLength: 100, nullable: false),
                    Locality = table.Column<string>(maxLength: 35, nullable: false),
                    TownName = table.Column<string>(maxLength: 30, nullable: false),
                    AdminstrativeArea = table.Column<string>(maxLength: 30, nullable: false),
                    Language = table.Column<string>(maxLength: 3, nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    EntryDate = table.Column<DateTime>(nullable: false),
                    LoadId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StreetDescriptor", x => new { x.StreetDescription, x.Locality, x.TownName, x.AdminstrativeArea });
                    table.ForeignKey(
                        name: "FK_StreetDescriptor_Street",
                        column: x => x.Usrn,
                        principalTable: "Street",
                        principalColumn: "Usrn",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationCrossReference_Uprn",
                table: "ApplicationCrossReference",
                column: "Uprn");

            migrationBuilder.CreateIndex(
                name: "IX_Classification_Uprn",
                table: "Classification",
                column: "Uprn");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryPointAddress_Uprn",
                table: "DeliveryPointAddress",
                column: "Uprn");

            migrationBuilder.CreateIndex(
                name: "IX_LandAndPropertyIdentifier_Uprn",
                table: "LandAndPropertyIdentifier",
                column: "Uprn");

            migrationBuilder.CreateIndex(
                name: "IX_LandAndPropertyIdentifier_Usrn",
                table: "LandAndPropertyIdentifier",
                column: "Usrn");

            migrationBuilder.CreateIndex(
                name: "IX_Organisation_Uprn",
                table: "Organisation",
                column: "Uprn");

            migrationBuilder.CreateIndex(
                name: "IX_SearchableAddress_RQPPostcode",
                table: "SearchableAddress",
                column: "Postcode");

            migrationBuilder.CreateIndex(
                name: "IX_SearchableAddress_Uprn",
                table: "SearchableAddress",
                column: "Uprn");

            migrationBuilder.CreateIndex(
                name: "UX_SearchableAddress_RQPAddress",
                table: "SearchableAddress",
                columns: new[] { "DepartmentName", "OrganisationName", "SubBuildingName", "BuildingName", "BuildingNumber", "PoBoxNumber", "DependentThoroughfare", "ThoroughFare", "DoubleDependentLocality", "DependentLocality", "PostTown" });

            migrationBuilder.CreateIndex(
                name: "IX_SearchableGeographic_Uprn",
                table: "SearchableGeographic",
                column: "Uprn");

            migrationBuilder.CreateIndex(
                name: "UX_SearchableGeographic_Text",
                table: "SearchableGeographic",
                columns: new[] { "SaoText", "SaoStartNumber", "SaoStartSuffix", "SaoEndNumber", "SaoEndSuffix", "PaoText", "PaoStartNumber", "PaoStartSuffix", "PaoEndNumber", "PaoEndSuffix", "StreetDescription", "Locality", "TownName", "AdminstrativeArea", "PostcodeLocator" });

            migrationBuilder.CreateIndex(
                name: "IX_StreetDescriptor_Usrn",
                table: "StreetDescriptor",
                column: "Usrn");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationCrossReference");

            migrationBuilder.DropTable(
                name: "Classification");

            migrationBuilder.DropTable(
                name: "DeliveryPointAddress");

            migrationBuilder.DropTable(
                name: "Header");

            migrationBuilder.DropTable(
                name: "LandAndPropertyIdentifier");

            migrationBuilder.DropTable(
                name: "Metadata");

            migrationBuilder.DropTable(
                name: "Organisation");

            migrationBuilder.DropTable(
                name: "SearchableAddress");

            migrationBuilder.DropTable(
                name: "SearchableGeographic");

            migrationBuilder.DropTable(
                name: "StreetDescriptor");

            migrationBuilder.DropTable(
                name: "Trailer");

            migrationBuilder.DropTable(
                name: "BasicLandAndPropertyUnit");

            migrationBuilder.DropTable(
                name: "Street");
        }
    }
}
