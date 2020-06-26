﻿using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TQ.Geocoding.DataLoad.Migrations
{
    public partial class DropAll : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "LandPropertyIdentifier");

            migrationBuilder.DropTable(
                name: "Metadata");

            migrationBuilder.DropTable(
                name: "Organisation");

            migrationBuilder.DropTable(
                name: "SearchableAddress");

            migrationBuilder.DropTable(
                name: "SearchableGeographic");

            migrationBuilder.DropTable(
                name: "Staging");

            migrationBuilder.DropTable(
                name: "StagingFilter");

            migrationBuilder.DropTable(
                name: "StagingFilterUprn");

            migrationBuilder.DropTable(
                name: "StreetDescriptor");

            migrationBuilder.DropTable(
                name: "Trailer");

            migrationBuilder.DropTable(
                name: "BasicLandAndPropertyUnit");

            migrationBuilder.DropTable(
                name: "Street");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BasicLandAndPropertyUnit",
                columns: table => new
                {
                    Uprn = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddressBasePostal = table.Column<string>(maxLength: 1, nullable: false),
                    BlpuState = table.Column<long>(nullable: false),
                    BlpuStateDate = table.Column<DateTime>(nullable: true),
                    ChangeType = table.Column<string>(nullable: false),
                    Country = table.Column<string>(maxLength: 1, nullable: false),
                    EndDate = table.Column<DateTime>(nullable: true),
                    EntryDate = table.Column<DateTime>(nullable: false),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    Latitude = table.Column<float>(nullable: false),
                    LoadId = table.Column<int>(nullable: false),
                    LocalCustodianCode = table.Column<int>(nullable: false),
                    LogicalStatus = table.Column<long>(nullable: false),
                    Longitude = table.Column<float>(nullable: false),
                    MultiOccCount = table.Column<int>(nullable: false),
                    ParentUprn = table.Column<long>(nullable: true),
                    PostcodeLocator = table.Column<string>(maxLength: 8, nullable: false),
                    ProOrder = table.Column<long>(nullable: false),
                    RecordIdentifier = table.Column<short>(nullable: false),
                    Rpc = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    XCoordinate = table.Column<float>(nullable: false),
                    YCoordinate = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasicLandAndPropertyUnit", x => x.Uprn);
                });

            migrationBuilder.CreateTable(
                name: "Header",
                columns: table => new
                {
                    HeaderId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CustodianName = table.Column<string>(maxLength: 40, nullable: false),
                    EntryDate = table.Column<DateTime>(nullable: false),
                    FileType = table.Column<string>(maxLength: 1, nullable: false),
                    LoadId = table.Column<int>(nullable: false),
                    LocalCustodianCode = table.Column<int>(nullable: false),
                    ProcessDate = table.Column<DateTime>(nullable: false),
                    RecordIdentifier = table.Column<short>(nullable: false),
                    TimeStamp = table.Column<TimeSpan>(nullable: false),
                    Version = table.Column<string>(maxLength: 7, nullable: false),
                    VolumeNumber = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Header", x => x.HeaderId);
                });

            migrationBuilder.CreateTable(
                name: "Metadata",
                columns: table => new
                {
                    MetadataId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CharacterSet = table.Column<string>(maxLength: 30, nullable: false),
                    ClassScheme = table.Column<string>(maxLength: 60, nullable: false),
                    CoordSystem = table.Column<string>(maxLength: 40, nullable: false),
                    CoordUnit = table.Column<string>(maxLength: 10, nullable: false),
                    CustodianName = table.Column<string>(maxLength: 40, nullable: false),
                    CustodianUprn = table.Column<long>(nullable: false),
                    GazDate = table.Column<DateTime>(nullable: false),
                    GazName = table.Column<string>(maxLength: 60, nullable: false),
                    GazOwner = table.Column<string>(maxLength: 15, nullable: false),
                    GazScope = table.Column<string>(maxLength: 60, nullable: false),
                    Language = table.Column<string>(maxLength: 3, nullable: false),
                    LinkedData = table.Column<string>(maxLength: 100, nullable: false),
                    LoadId = table.Column<int>(nullable: false),
                    LocalCustodianCode = table.Column<short>(nullable: false),
                    MetaDate = table.Column<DateTime>(nullable: false),
                    NgazFreq = table.Column<string>(maxLength: 1, nullable: false),
                    RecordIdentifier = table.Column<short>(nullable: false),
                    TerOfUse = table.Column<string>(maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Metadata", x => x.MetadataId);
                });

            migrationBuilder.CreateTable(
                name: "Staging",
                columns: table => new
                {
                    StagingRecordId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Field1 = table.Column<string>(maxLength: 100, nullable: true),
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
                    Field2 = table.Column<string>(maxLength: 100, nullable: true),
                    Field20 = table.Column<string>(maxLength: 100, nullable: true),
                    Field21 = table.Column<string>(maxLength: 100, nullable: true),
                    Field22 = table.Column<string>(maxLength: 100, nullable: true),
                    Field23 = table.Column<string>(maxLength: 100, nullable: true),
                    Field24 = table.Column<string>(maxLength: 100, nullable: true),
                    Field25 = table.Column<string>(maxLength: 100, nullable: true),
                    Field26 = table.Column<string>(maxLength: 100, nullable: true),
                    Field27 = table.Column<string>(maxLength: 100, nullable: true),
                    Field28 = table.Column<string>(maxLength: 100, nullable: true),
                    Field3 = table.Column<string>(maxLength: 100, nullable: true),
                    Field4 = table.Column<string>(maxLength: 100, nullable: true),
                    Field5 = table.Column<string>(maxLength: 100, nullable: true),
                    Field6 = table.Column<string>(maxLength: 100, nullable: true),
                    Field7 = table.Column<string>(maxLength: 100, nullable: true),
                    Field8 = table.Column<string>(maxLength: 100, nullable: true),
                    Field9 = table.Column<string>(maxLength: 100, nullable: true),
                    RecordIdentifier = table.Column<string>(maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staging", x => x.StagingRecordId);
                });

            migrationBuilder.CreateTable(
                name: "StagingFilter",
                columns: table => new
                {
                    StagingFilterId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClassificationKey = table.Column<string>(maxLength: 14, nullable: true),
                    HeaderId = table.Column<long>(nullable: true),
                    LpiKey = table.Column<string>(maxLength: 14, nullable: true),
                    MetadataId = table.Column<long>(nullable: true),
                    OrgKey = table.Column<string>(maxLength: 14, nullable: true),
                    TrailerId = table.Column<long>(nullable: true),
                    Udprn = table.Column<long>(nullable: true),
                    Uprn = table.Column<long>(nullable: true),
                    Usrn = table.Column<int>(nullable: true),
                    XrefKey = table.Column<string>(maxLength: 14, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StagingFilter", x => x.StagingFilterId);
                });

            migrationBuilder.CreateTable(
                name: "StagingFilterUprn",
                columns: table => new
                {
                    FilterUprn = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StagingFilterUprn", x => x.FilterUprn);
                });

            migrationBuilder.CreateTable(
                name: "Street",
                columns: table => new
                {
                    Usrn = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChangeType = table.Column<string>(nullable: false),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    LoadId = table.Column<int>(nullable: false),
                    ProOrder = table.Column<long>(nullable: false),
                    RecordEntryDate = table.Column<DateTime>(nullable: false),
                    RecordIdentifier = table.Column<short>(nullable: false),
                    RecordType = table.Column<short>(nullable: false),
                    State = table.Column<short>(nullable: false),
                    StateDate = table.Column<DateTime>(nullable: false),
                    StreetClassification = table.Column<short>(nullable: false),
                    StreetEndDate = table.Column<DateTime>(nullable: true),
                    StreetEndLat = table.Column<float>(nullable: false),
                    StreetEndLong = table.Column<float>(nullable: false),
                    StreetEndX = table.Column<float>(nullable: false),
                    StreetEndY = table.Column<float>(nullable: false),
                    StreetStartDate = table.Column<DateTime>(nullable: false),
                    StreetStartLat = table.Column<float>(nullable: false),
                    StreetStartLong = table.Column<float>(nullable: false),
                    StreetStartX = table.Column<float>(nullable: false),
                    StreetStartY = table.Column<float>(nullable: false),
                    StreetSurface = table.Column<short>(nullable: false),
                    StreetTolerance = table.Column<short>(nullable: false),
                    SwaOrgRefNaming = table.Column<int>(nullable: false),
                    Version = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Street", x => x.Usrn);
                });

            migrationBuilder.CreateTable(
                name: "Trailer",
                columns: table => new
                {
                    TrailerId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EntryDate = table.Column<DateTime>(nullable: false),
                    LoadId = table.Column<int>(nullable: false),
                    NextVolumeName = table.Column<short>(nullable: false),
                    RecordCount = table.Column<long>(nullable: false),
                    RecordIdentifier = table.Column<short>(nullable: false),
                    TimeStamp = table.Column<TimeSpan>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trailer", x => x.TrailerId);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationCrossReference",
                columns: table => new
                {
                    XrefKey = table.Column<string>(maxLength: 14, nullable: false),
                    ChangeType = table.Column<string>(nullable: false),
                    CrossReference = table.Column<string>(maxLength: 50, nullable: false),
                    EndDate = table.Column<DateTime>(nullable: true),
                    EntryDate = table.Column<DateTime>(nullable: false),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    LoadId = table.Column<int>(nullable: false),
                    ProOrder = table.Column<long>(nullable: false),
                    RecordIdentifier = table.Column<short>(nullable: false),
                    Source = table.Column<string>(maxLength: 6, nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    Uprn = table.Column<long>(nullable: false),
                    Version = table.Column<short>(nullable: true)
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
                    ClassificationKey = table.Column<string>(maxLength: 14, nullable: false),
                    ChangeType = table.Column<string>(nullable: false),
                    ClassScheme = table.Column<string>(maxLength: 60, nullable: false),
                    ClassificationCode = table.Column<string>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: true),
                    EntryDate = table.Column<DateTime>(nullable: false),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    LoadId = table.Column<int>(nullable: false),
                    ProOrder = table.Column<long>(nullable: false),
                    RecordIdentifier = table.Column<short>(nullable: false),
                    SchemeVersion = table.Column<float>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    Uprn = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classification", x => x.ClassificationKey);
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
                    Udprn = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BuildingName = table.Column<string>(maxLength: 50, nullable: true),
                    BuildingNumber = table.Column<int>(nullable: true),
                    ChangeType = table.Column<string>(nullable: false),
                    DeliveryPointSuffix = table.Column<string>(maxLength: 2, nullable: true),
                    DepartmentName = table.Column<string>(maxLength: 60, nullable: true),
                    DependentLocality = table.Column<string>(maxLength: 35, nullable: true),
                    DependentThoroughfare = table.Column<string>(maxLength: 80, nullable: true),
                    DoubleDependentLocality = table.Column<string>(maxLength: 35, nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true),
                    EntryDate = table.Column<DateTime>(nullable: false),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    LoadId = table.Column<int>(nullable: false),
                    OrganisationName = table.Column<string>(maxLength: 60, nullable: true),
                    PoBoxNumber = table.Column<string>(maxLength: 6, nullable: true),
                    PostTown = table.Column<string>(maxLength: 30, nullable: true),
                    Postcode = table.Column<string>(maxLength: 8, nullable: true),
                    PostcodeType = table.Column<string>(maxLength: 1, nullable: true),
                    ProOrder = table.Column<long>(nullable: false),
                    ProcessDate = table.Column<DateTime>(nullable: false),
                    RecordIdentifier = table.Column<short>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    SubBuildingName = table.Column<string>(maxLength: 30, nullable: true),
                    ThoroughFare = table.Column<string>(maxLength: 80, nullable: true),
                    Uprn = table.Column<long>(nullable: false),
                    WelshDependentLocality = table.Column<string>(maxLength: 35, nullable: true),
                    WelshDependentThoroughfare = table.Column<string>(maxLength: 80, nullable: true),
                    WelshDoubleDependentLocality = table.Column<string>(maxLength: 35, nullable: true),
                    WelshPostTown = table.Column<string>(maxLength: 30, nullable: true),
                    WelshThoroughfare = table.Column<string>(maxLength: 80, nullable: true)
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
                    OrgKey = table.Column<string>(maxLength: 14, nullable: false),
                    ChangeType = table.Column<string>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: true),
                    EntryDate = table.Column<DateTime>(nullable: false),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    LegalName = table.Column<string>(maxLength: 60, nullable: true),
                    LoadId = table.Column<int>(nullable: false),
                    OrganisationName = table.Column<string>(maxLength: 100, nullable: false),
                    ProOrder = table.Column<long>(nullable: false),
                    RecordIdentifier = table.Column<short>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    Uprn = table.Column<long>(nullable: false)
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
                    AddressLine1 = table.Column<string>(maxLength: 250, nullable: true),
                    AddressLine2 = table.Column<string>(maxLength: 250, nullable: true),
                    AddressLine3 = table.Column<string>(maxLength: 250, nullable: true),
                    BuildingName = table.Column<string>(maxLength: 50, nullable: true),
                    BuildingNumber = table.Column<int>(nullable: true),
                    DepartmentName = table.Column<string>(maxLength: 60, nullable: true),
                    DependentLocality = table.Column<string>(maxLength: 35, nullable: true),
                    DependentThoroughfare = table.Column<string>(maxLength: 80, nullable: true),
                    DoubleDependentLocality = table.Column<string>(maxLength: 35, nullable: true),
                    Latitude = table.Column<float>(nullable: false),
                    LoadId = table.Column<int>(nullable: false),
                    Longitude = table.Column<float>(nullable: false),
                    OrganisationName = table.Column<string>(maxLength: 60, nullable: true),
                    PoBoxNumber = table.Column<string>(maxLength: 6, nullable: true),
                    PostTown = table.Column<string>(maxLength: 30, nullable: true),
                    Postcode = table.Column<string>(maxLength: 10, nullable: true),
                    PostcodeNoSpaces = table.Column<string>(maxLength: 8, nullable: true),
                    SingleLineAddress = table.Column<string>(maxLength: 500, nullable: true),
                    SubBuildingName = table.Column<string>(maxLength: 30, nullable: true),
                    ThoroughFare = table.Column<string>(maxLength: 80, nullable: true),
                    Uprn = table.Column<long>(nullable: false),
                    WelshAddressLine1 = table.Column<string>(maxLength: 250, nullable: true),
                    WelshAddressLine2 = table.Column<string>(maxLength: 250, nullable: true),
                    WelshAddressLine3 = table.Column<string>(maxLength: 250, nullable: true),
                    WelshDependentLocality = table.Column<string>(maxLength: 35, nullable: true),
                    WelshDependentThoroughfare = table.Column<string>(maxLength: 80, nullable: true),
                    WelshDoubleDependentLocality = table.Column<string>(maxLength: 35, nullable: true),
                    WelshPostTown = table.Column<string>(maxLength: 30, nullable: true),
                    WelshSingleLineAddress = table.Column<string>(maxLength: 500, nullable: true),
                    WelshThoroughfare = table.Column<string>(maxLength: 80, nullable: true),
                    XCoordinateEasting = table.Column<float>(nullable: false),
                    YCoordinateNorthing = table.Column<float>(nullable: false)
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
                    SearchableGeographicId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AdministrativeArea = table.Column<string>(maxLength: 30, nullable: true),
                    LoadId = table.Column<int>(nullable: false),
                    Locality = table.Column<string>(maxLength: 35, nullable: true),
                    PaoEndNumber = table.Column<short>(nullable: true),
                    PaoEndSuffix = table.Column<string>(maxLength: 2, nullable: true),
                    PaoStartNumber = table.Column<short>(nullable: true),
                    PaoStartSuffix = table.Column<string>(maxLength: 2, nullable: true),
                    PaoText = table.Column<string>(maxLength: 90, nullable: true),
                    PostcodeLocator = table.Column<string>(maxLength: 8, nullable: true),
                    SaoEndNumber = table.Column<short>(nullable: true),
                    SaoEndSuffix = table.Column<string>(maxLength: 2, nullable: true),
                    SaoStartNumber = table.Column<short>(nullable: true),
                    SaoStartSuffix = table.Column<string>(maxLength: 2, nullable: true),
                    SaoText = table.Column<string>(maxLength: 90, nullable: true),
                    StreetDescription = table.Column<string>(maxLength: 100, nullable: true),
                    TownName = table.Column<string>(maxLength: 30, nullable: true),
                    Uprn = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchableGeographic", x => x.SearchableGeographicId);
                    table.ForeignKey(
                        name: "FK_SearchableGeographic_BLPU",
                        column: x => x.Uprn,
                        principalTable: "BasicLandAndPropertyUnit",
                        principalColumn: "Uprn",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LandPropertyIdentifier",
                columns: table => new
                {
                    LpiKey = table.Column<string>(maxLength: 14, nullable: false),
                    AreaName = table.Column<string>(maxLength: 40, nullable: true),
                    ChangeType = table.Column<string>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: true),
                    EntryDate = table.Column<DateTime>(nullable: false),
                    Language = table.Column<string>(maxLength: 3, nullable: false),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    Level = table.Column<string>(maxLength: 30, nullable: true),
                    LoadId = table.Column<int>(nullable: false),
                    LogicalStatus = table.Column<short>(nullable: false),
                    OfficialFlag = table.Column<bool>(nullable: true),
                    PaoEndNumber = table.Column<long>(nullable: true),
                    PaoEndSuffix = table.Column<string>(maxLength: 2, nullable: true),
                    PaoStartNumber = table.Column<long>(nullable: true),
                    PaoStartSuffix = table.Column<string>(maxLength: 2, nullable: true),
                    PaoText = table.Column<string>(maxLength: 90, nullable: true),
                    ProOrder = table.Column<long>(nullable: false),
                    RecordIdentifier = table.Column<short>(nullable: false),
                    SaoEndNumber = table.Column<short>(nullable: true),
                    SaoEndSuffix = table.Column<string>(maxLength: 2, nullable: true),
                    SaoStartNumber = table.Column<short>(nullable: true),
                    SaoStartSuffix = table.Column<string>(maxLength: 2, nullable: true),
                    SaoText = table.Column<string>(maxLength: 90, nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    Uprn = table.Column<long>(nullable: false),
                    Usrn = table.Column<int>(nullable: false),
                    UsrnMatchIndicator = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandPropertyIdentifier", x => x.LpiKey);
                    table.ForeignKey(
                        name: "FK_LandPropertyIdentifier_BLPU",
                        column: x => x.Uprn,
                        principalTable: "BasicLandAndPropertyUnit",
                        principalColumn: "Uprn",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LandPropertyIdentifier_Street",
                        column: x => x.Usrn,
                        principalTable: "Street",
                        principalColumn: "Usrn",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StreetDescriptor",
                columns: table => new
                {
                    Usrn = table.Column<int>(nullable: false),
                    Language = table.Column<string>(maxLength: 3, nullable: false),
                    AdminstrativeArea = table.Column<string>(maxLength: 30, nullable: false),
                    ChangeType = table.Column<string>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: true),
                    EntryDate = table.Column<DateTime>(nullable: false),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    LoadId = table.Column<int>(nullable: false),
                    Locality = table.Column<string>(maxLength: 35, nullable: false),
                    ProOrder = table.Column<long>(nullable: false),
                    RecordIdentifier = table.Column<short>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    StreetDescription = table.Column<string>(maxLength: 100, nullable: false),
                    TownName = table.Column<string>(maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StreetDescriptor", x => new { x.Usrn, x.Language });
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
                name: "IX_LandPropertyIdentifier_Uprn",
                table: "LandPropertyIdentifier",
                column: "Uprn");

            migrationBuilder.CreateIndex(
                name: "IX_LandPropertyIdentifier_Usrn",
                table: "LandPropertyIdentifier",
                column: "Usrn");

            migrationBuilder.CreateIndex(
                name: "IX_Organisation_Uprn",
                table: "Organisation",
                column: "Uprn");

            migrationBuilder.CreateIndex(
                name: "UX_SearchableAddress_Postcode",
                table: "SearchableAddress",
                column: "Postcode");

            migrationBuilder.CreateIndex(
                name: "IX_SearchableAddress_PostcodeNoSpaces",
                table: "SearchableAddress",
                column: "PostcodeNoSpaces");

            migrationBuilder.CreateIndex(
                name: "IX_SearchableAddress_SingleLineAddress",
                table: "SearchableAddress",
                column: "SingleLineAddress");

            migrationBuilder.CreateIndex(
                name: "IX_SearchableAddress_Uprn",
                table: "SearchableAddress",
                column: "Uprn");

            migrationBuilder.CreateIndex(
                name: "IX_SearchableAddress_WelshSingleLineAddress",
                table: "SearchableAddress",
                column: "WelshSingleLineAddress");

            migrationBuilder.CreateIndex(
                name: "IX_SearchableGeographic_Uprn",
                table: "SearchableGeographic",
                column: "Uprn");

            migrationBuilder.CreateIndex(
                name: "UX_SearchableGeographic_Text",
                table: "SearchableGeographic",
                columns: new[] { "SaoText", "SaoStartNumber", "SaoStartSuffix", "SaoEndNumber", "SaoEndSuffix", "PaoText", "PaoStartNumber", "PaoStartSuffix", "PaoEndNumber", "PaoEndSuffix", "StreetDescription", "Locality", "TownName", "AdministrativeArea", "PostcodeLocator" });

            migrationBuilder.CreateIndex(
                name: "IX_Staging_RecordIdentifier",
                table: "Staging",
                columns: new[] { "RecordIdentifier", "StagingRecordId" },
                unique: true);
        }
    }
}
