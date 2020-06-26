﻿// <auto-generated />
using System;
using TQ.Geocoding.DataLoad.Models.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace TQ.Geocoding.DataLoad.Migrations
{
    [DbContext(typeof(GeocodingDataLoadContext))]
    [Migration("20190813125742_StagingField29")]
    partial class StagingField29
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TQ.Geocoding.DataLoad.Models.Classes.ApplicationCrossReference", b =>
                {
                    b.Property<string>("XrefKey")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(14);

                    b.Property<string>("ChangeType")
                        .IsRequired()
                        .HasConversion(new ValueConverter<string, string>(v => default(string), v => default(string), new ConverterMappingHints(size: 1)));

                    b.Property<string>("CrossReference")
                        .HasMaxLength(50);

                    b.Property<DateTime>("EndDate");

                    b.Property<DateTime>("EntryDate");

                    b.Property<DateTime>("LastUpdateDate");

                    b.Property<int>("LoadId");

                    b.Property<long>("ProOrder");

                    b.Property<short>("RecordIdentifier");

                    b.Property<string>("Source")
                        .HasMaxLength(6);

                    b.Property<DateTime>("StartDate");

                    b.Property<long>("Uprn");

                    b.Property<short>("Version");

                    b.HasKey("XrefKey")
                        .HasName("PK_ApplicationCrossReference");

                    b.HasIndex("Uprn");

                    b.ToTable("ApplicationCrossReference");
                });

            modelBuilder.Entity("TQ.Geocoding.DataLoad.Models.Classes.BasicLandAndPropertyUnit", b =>
                {
                    b.Property<long>("Uprn")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("BlupState");

                    b.Property<DateTime>("BlupStateDate");

                    b.Property<string>("ChangeType")
                        .IsRequired()
                        .HasConversion(new ValueConverter<string, string>(v => default(string), v => default(string), new ConverterMappingHints(size: 1)));

                    b.Property<string>("Country")
                        .HasMaxLength(1);

                    b.Property<DateTime>("EndDate");

                    b.Property<DateTime>("EntryDate");

                    b.Property<DateTime>("LastUpdateDate");

                    b.Property<float>("Latitude");

                    b.Property<int>("LoadId");

                    b.Property<int>("LocalCustodianCode");

                    b.Property<long>("LogicalStatus");

                    b.Property<float>("Longitude");

                    b.Property<int>("MultiOccCount");

                    b.Property<long>("ParentUprn");

                    b.Property<string>("PostcodeLocator")
                        .HasMaxLength(8);

                    b.Property<long>("ProOrder");

                    b.Property<short>("RecordIdentifier");

                    b.Property<int>("Rpc");

                    b.Property<DateTime>("StartDate");

                    b.Property<float>("XCoordinate");

                    b.Property<float>("YCoordinate");

                    b.HasKey("Uprn")
                        .HasName("PK_BasicLandAndPropertyUnit");

                    b.ToTable("BasicLandAndPropertyUnit");
                });

            modelBuilder.Entity("TQ.Geocoding.DataLoad.Models.Classes.Classification", b =>
                {
                    b.Property<string>("ClassKey")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(200);

                    b.Property<string>("ChangeType")
                        .IsRequired()
                        .HasConversion(new ValueConverter<string, string>(v => default(string), v => default(string), new ConverterMappingHints(size: 1)));

                    b.Property<string>("ClassScheme")
                        .HasMaxLength(60);

                    b.Property<string>("ClassificationCode");

                    b.Property<DateTime>("EndDate");

                    b.Property<DateTime>("EntryDate");

                    b.Property<DateTime>("LastUpdateDate");

                    b.Property<int>("LoadId");

                    b.Property<long>("ProOrder");

                    b.Property<short>("RecordIdentifier");

                    b.Property<float>("SchemeVersion");

                    b.Property<DateTime>("StartDate");

                    b.Property<long>("Uprn");

                    b.HasKey("ClassKey")
                        .HasName("PK_Classification");

                    b.HasIndex("Uprn");

                    b.ToTable("Classification");
                });

            modelBuilder.Entity("TQ.Geocoding.DataLoad.Models.Classes.DeliveryPointAddress", b =>
                {
                    b.Property<long>("Udprn")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BuildingName")
                        .HasMaxLength(50);

                    b.Property<int>("BuildingNumber");

                    b.Property<string>("ChangeType")
                        .IsRequired()
                        .HasConversion(new ValueConverter<string, string>(v => default(string), v => default(string), new ConverterMappingHints(size: 1)));

                    b.Property<string>("DeliveryPointSuffix")
                        .HasMaxLength(2);

                    b.Property<string>("DepartmentName")
                        .HasMaxLength(60);

                    b.Property<string>("DependentLocality")
                        .HasMaxLength(35);

                    b.Property<string>("DependentThoroughfare")
                        .HasMaxLength(80);

                    b.Property<string>("DoubleDependentLocality")
                        .HasMaxLength(35);

                    b.Property<DateTime>("EndDate");

                    b.Property<DateTime>("EntryDate");

                    b.Property<DateTime>("LastUpdateDate");

                    b.Property<int>("LoadId");

                    b.Property<string>("OrganisationName")
                        .HasMaxLength(60);

                    b.Property<string>("PoBoxNumber")
                        .HasMaxLength(6);

                    b.Property<string>("PostTown")
                        .HasMaxLength(30);

                    b.Property<string>("Postcode")
                        .HasMaxLength(8);

                    b.Property<string>("PostcodeType")
                        .HasMaxLength(1);

                    b.Property<long>("ProOrder");

                    b.Property<DateTime>("ProcessDate");

                    b.Property<short>("RecordIdentifier");

                    b.Property<DateTime>("StartDate");

                    b.Property<string>("SubBuildingName")
                        .HasMaxLength(30);

                    b.Property<string>("ThoroughFare")
                        .HasMaxLength(80);

                    b.Property<long>("Uprn");

                    b.Property<string>("WelshDependentLocality")
                        .HasMaxLength(35);

                    b.Property<string>("WelshDependentThoroughfare")
                        .HasMaxLength(80);

                    b.Property<string>("WelshDoubleDependentLocality")
                        .HasMaxLength(35);

                    b.Property<string>("WelshPostTown")
                        .HasMaxLength(30);

                    b.Property<string>("WelshThoroughfare")
                        .HasMaxLength(80);

                    b.HasKey("Udprn")
                        .HasName("PK_DeliveryPointAddress");

                    b.HasIndex("Uprn");

                    b.ToTable("DeliveryPointAddress");
                });

            modelBuilder.Entity("TQ.Geocoding.DataLoad.Models.Classes.Header", b =>
                {
                    b.Property<long>("OriginalFileId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CustodianName")
                        .HasMaxLength(40);

                    b.Property<DateTime>("EntryDate");

                    b.Property<string>("FileType")
                        .HasMaxLength(1);

                    b.Property<int>("LoadId");

                    b.Property<int>("LocalCustodianCode");

                    b.Property<DateTime>("ProcessDate");

                    b.Property<short>("RecordIdentifier");

                    b.Property<TimeSpan>("TimeStamp");

                    b.Property<string>("Version")
                        .HasMaxLength(7);

                    b.Property<short>("VolumeNumber");

                    b.HasKey("OriginalFileId")
                        .HasName("PK_Header");

                    b.ToTable("Header");
                });

            modelBuilder.Entity("TQ.Geocoding.DataLoad.Models.Classes.LandAndPropertyIdentifier", b =>
                {
                    b.Property<string>("LpiKey")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(14);

                    b.Property<string>("AreaName")
                        .HasMaxLength(40);

                    b.Property<string>("ChangeType")
                        .IsRequired()
                        .HasConversion(new ValueConverter<string, string>(v => default(string), v => default(string), new ConverterMappingHints(size: 1)));

                    b.Property<DateTime>("EndDate");

                    b.Property<DateTime>("EntryDate");

                    b.Property<string>("Language")
                        .HasMaxLength(3);

                    b.Property<DateTime>("LastUpdateDate");

                    b.Property<string>("Level")
                        .HasMaxLength(30);

                    b.Property<int>("LoadId");

                    b.Property<short>("LogicalStatus");

                    b.Property<bool>("OfficialFlag");

                    b.Property<long>("PaoEndNumber");

                    b.Property<string>("PaoEndSuffix")
                        .HasMaxLength(2);

                    b.Property<long>("PaoStartNumber");

                    b.Property<string>("PaoStartSuffix")
                        .HasMaxLength(2);

                    b.Property<string>("PaoText")
                        .HasMaxLength(90);

                    b.Property<long>("ProOrder");

                    b.Property<short>("RecordIdentifier");

                    b.Property<short>("SaoEndNumber");

                    b.Property<string>("SaoEndSuffix")
                        .HasMaxLength(2);

                    b.Property<short>("SaoStartNumber");

                    b.Property<string>("SaoStartSuffix")
                        .HasMaxLength(2);

                    b.Property<string>("SaoText")
                        .HasMaxLength(90);

                    b.Property<DateTime>("StartDate");

                    b.Property<long>("Uprn");

                    b.Property<int>("Usrn");

                    b.Property<short>("UsrnMatchIndicator");

                    b.HasKey("LpiKey")
                        .HasName("PK_LandAndPropertyIdentifier");

                    b.HasIndex("Uprn");

                    b.HasIndex("Usrn");

                    b.ToTable("LandAndPropertyIdentifier");
                });

            modelBuilder.Entity("TQ.Geocoding.DataLoad.Models.Classes.Metadata", b =>
                {
                    b.Property<long>("OriginalFileId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CharacterSet")
                        .HasMaxLength(30);

                    b.Property<string>("ClassScheme")
                        .HasMaxLength(60);

                    b.Property<string>("CoordSystem")
                        .HasMaxLength(40);

                    b.Property<string>("CoordUnit")
                        .HasMaxLength(10);

                    b.Property<string>("CustodianName")
                        .HasMaxLength(40);

                    b.Property<long>("CustodianUprn");

                    b.Property<DateTime>("GazDate");

                    b.Property<string>("GazName")
                        .HasMaxLength(60);

                    b.Property<string>("GazOwner")
                        .HasMaxLength(15);

                    b.Property<string>("GazScope")
                        .HasMaxLength(60);

                    b.Property<string>("Language")
                        .HasMaxLength(3);

                    b.Property<string>("LinkedData")
                        .HasMaxLength(100);

                    b.Property<int>("LoadId");

                    b.Property<short>("LocalCustodianCode");

                    b.Property<DateTime>("MetaDate");

                    b.Property<string>("NgazFreq")
                        .HasMaxLength(1);

                    b.Property<short>("RecordIdentifier");

                    b.Property<string>("TerOfUse")
                        .HasMaxLength(60);

                    b.HasKey("OriginalFileId")
                        .HasName("PK_Metadata");

                    b.ToTable("Metadata");
                });

            modelBuilder.Entity("TQ.Geocoding.DataLoad.Models.Classes.Organisation", b =>
                {
                    b.Property<string>("OrgKey")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(14);

                    b.Property<string>("ChangeType")
                        .IsRequired()
                        .HasConversion(new ValueConverter<string, string>(v => default(string), v => default(string), new ConverterMappingHints(size: 1)));

                    b.Property<DateTime>("EndDate");

                    b.Property<DateTime>("EntryDate");

                    b.Property<DateTime>("LastUpdateDate");

                    b.Property<string>("LegalName")
                        .HasMaxLength(60);

                    b.Property<int>("LoadId");

                    b.Property<string>("OrganisationName")
                        .HasMaxLength(100);

                    b.Property<long>("ProOrder");

                    b.Property<short>("RecordIdentifier");

                    b.Property<DateTime>("StartDate");

                    b.Property<long>("Uprn");

                    b.HasKey("OrgKey")
                        .HasName("PK_Organisation");

                    b.HasIndex("Uprn");

                    b.ToTable("Organisation");
                });

            modelBuilder.Entity("TQ.Geocoding.DataLoad.Models.Classes.SearchableAddress", b =>
                {
                    b.Property<long>("SearchableAddressId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BuildingName")
                        .HasMaxLength(50);

                    b.Property<int>("BuildingNumber");

                    b.Property<string>("DepartmentName")
                        .HasMaxLength(60);

                    b.Property<string>("DependentLocality")
                        .HasMaxLength(35);

                    b.Property<string>("DependentThoroughfare")
                        .HasMaxLength(80);

                    b.Property<string>("DoubleDependentLocality")
                        .HasMaxLength(35);

                    b.Property<float>("Latitude");

                    b.Property<int>("LoadId");

                    b.Property<float>("Longitude");

                    b.Property<string>("OrganisationName")
                        .HasMaxLength(60);

                    b.Property<string>("PoBoxNumber")
                        .HasMaxLength(6);

                    b.Property<string>("PostTown")
                        .HasMaxLength(30);

                    b.Property<string>("Postcode")
                        .HasMaxLength(10);

                    b.Property<string>("PostcodeNoSpaces")
                        .HasMaxLength(8);

                    b.Property<string>("SubBuildingName")
                        .HasMaxLength(30);

                    b.Property<string>("ThoroughFare")
                        .HasMaxLength(80);

                    b.Property<long>("Uprn");

                    b.Property<string>("WelshDependentLocality")
                        .HasMaxLength(35);

                    b.Property<string>("WelshDependentThoroughfare")
                        .HasMaxLength(80);

                    b.Property<string>("WelshDoubleDependentLocality")
                        .HasMaxLength(35);

                    b.Property<string>("WelshPostTown")
                        .HasMaxLength(30);

                    b.Property<string>("WelshThoroughfare")
                        .HasMaxLength(80);

                    b.Property<float>("XCoordinateEasting");

                    b.Property<float>("YCoordinateNorthing");

                    b.HasKey("SearchableAddressId")
                        .HasName("PK_SearchableAddress");

                    b.HasIndex("Postcode")
                        .HasName("IX_SearchableAddress_RQPPostcode");

                    b.HasIndex("Uprn");

                    b.HasIndex("DepartmentName", "OrganisationName", "SubBuildingName", "BuildingName", "BuildingNumber", "PoBoxNumber", "DependentThoroughfare", "ThoroughFare", "DoubleDependentLocality", "DependentLocality", "PostTown")
                        .HasName("UX_SearchableAddress_RQPAddress");

                    b.ToTable("SearchableAddress");
                });

            modelBuilder.Entity("TQ.Geocoding.DataLoad.Models.Classes.SearchableGeographic", b =>
                {
                    b.Property<long>("SearchableGeographicId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AdminstrativeArea")
                        .HasMaxLength(30);

                    b.Property<int>("LoadId");

                    b.Property<string>("Locality")
                        .HasMaxLength(35);

                    b.Property<short>("PaoEndNumber");

                    b.Property<string>("PaoEndSuffix")
                        .HasMaxLength(2);

                    b.Property<short>("PaoStartNumber");

                    b.Property<string>("PaoStartSuffix")
                        .HasMaxLength(2);

                    b.Property<string>("PaoText")
                        .HasMaxLength(90);

                    b.Property<string>("PostcodeLocator")
                        .HasMaxLength(8);

                    b.Property<short>("SaoEndNumber");

                    b.Property<string>("SaoEndSuffix")
                        .HasMaxLength(2);

                    b.Property<short>("SaoStartNumber");

                    b.Property<string>("SaoStartSuffix")
                        .HasMaxLength(2);

                    b.Property<string>("SaoText")
                        .HasMaxLength(90);

                    b.Property<string>("StreetDescription")
                        .HasMaxLength(100);

                    b.Property<string>("TownName")
                        .HasMaxLength(30);

                    b.Property<long>("Uprn");

                    b.HasKey("SearchableGeographicId")
                        .HasName("PK_SearchableGeographic");

                    b.HasIndex("Uprn");

                    b.HasIndex("SaoText", "SaoStartNumber", "SaoStartSuffix", "SaoEndNumber", "SaoEndSuffix", "PaoText", "PaoStartNumber", "PaoStartSuffix", "PaoEndNumber", "PaoEndSuffix", "StreetDescription", "Locality", "TownName", "AdminstrativeArea", "PostcodeLocator")
                        .HasName("UX_SearchableGeographic_Text");

                    b.ToTable("SearchableGeographic");
                });

            modelBuilder.Entity("TQ.Geocoding.DataLoad.Models.Classes.Staging", b =>
                {
                    b.Property<long>("StagingRecordId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Field1")
                        .HasMaxLength(100);

                    b.Property<string>("Field10")
                        .HasMaxLength(100);

                    b.Property<string>("Field11")
                        .HasMaxLength(100);

                    b.Property<string>("Field12")
                        .HasMaxLength(100);

                    b.Property<string>("Field13")
                        .HasMaxLength(100);

                    b.Property<string>("Field14")
                        .HasMaxLength(100);

                    b.Property<string>("Field15")
                        .HasMaxLength(100);

                    b.Property<string>("Field16")
                        .HasMaxLength(100);

                    b.Property<string>("Field17")
                        .HasMaxLength(100);

                    b.Property<string>("Field18")
                        .HasMaxLength(100);

                    b.Property<string>("Field19")
                        .HasMaxLength(100);

                    b.Property<string>("Field2")
                        .HasMaxLength(100);

                    b.Property<string>("Field20")
                        .HasMaxLength(100);

                    b.Property<string>("Field21")
                        .HasMaxLength(100);

                    b.Property<string>("Field22")
                        .HasMaxLength(100);

                    b.Property<string>("Field23")
                        .HasMaxLength(100);

                    b.Property<string>("Field24")
                        .HasMaxLength(100);

                    b.Property<string>("Field25")
                        .HasMaxLength(100);

                    b.Property<string>("Field26")
                        .HasMaxLength(100);

                    b.Property<string>("Field27")
                        .HasMaxLength(100);

                    b.Property<string>("Field28")
                        .HasMaxLength(100);

                    b.Property<string>("Field29")
                        .HasMaxLength(100);

                    b.Property<string>("Field3")
                        .HasMaxLength(100);

                    b.Property<string>("Field4")
                        .HasMaxLength(100);

                    b.Property<string>("Field5")
                        .HasMaxLength(100);

                    b.Property<string>("Field6")
                        .HasMaxLength(100);

                    b.Property<string>("Field7")
                        .HasMaxLength(100);

                    b.Property<string>("Field8")
                        .HasMaxLength(100);

                    b.Property<string>("Field9")
                        .HasMaxLength(100);

                    b.Property<string>("RecordIdentifier")
                        .HasMaxLength(2);

                    b.HasKey("StagingRecordId")
                        .HasName("PK_Staging");

                    b.ToTable("Staging");
                });

            modelBuilder.Entity("TQ.Geocoding.DataLoad.Models.Classes.Street", b =>
                {
                    b.Property<int>("Usrn")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ChangeType")
                        .IsRequired()
                        .HasConversion(new ValueConverter<string, string>(v => default(string), v => default(string), new ConverterMappingHints(size: 1)));

                    b.Property<DateTime>("LastUpdateDate");

                    b.Property<int>("LoadId");

                    b.Property<long>("ProOrder");

                    b.Property<short>("RecordIdentifier");

                    b.Property<short>("RecordType");

                    b.Property<short>("State");

                    b.Property<DateTime>("StateDate");

                    b.Property<short>("StreetClassification");

                    b.Property<DateTime>("StreetEndDate");

                    b.Property<float>("StreetEndLat");

                    b.Property<float>("StreetEndLong");

                    b.Property<float>("StreetEndX");

                    b.Property<float>("StreetEndY");

                    b.Property<DateTime>("StreetStartDate");

                    b.Property<float>("StreetStartLat");

                    b.Property<float>("StreetStartLong");

                    b.Property<float>("StreetStartX");

                    b.Property<float>("StreetStartY");

                    b.Property<short>("StreetSurface");

                    b.Property<short>("StreetTolerance");

                    b.Property<int>("SwaOrgRefNaming");

                    b.Property<short>("Version");

                    b.HasKey("Usrn")
                        .HasName("PK_Street");

                    b.ToTable("Street");
                });

            modelBuilder.Entity("TQ.Geocoding.DataLoad.Models.Classes.StreetDescriptor", b =>
                {
                    b.Property<string>("StreetDescription")
                        .HasMaxLength(100);

                    b.Property<string>("Locality")
                        .HasMaxLength(35);

                    b.Property<string>("TownName")
                        .HasMaxLength(30);

                    b.Property<string>("AdminstrativeArea")
                        .HasMaxLength(30);

                    b.Property<string>("ChangeType")
                        .IsRequired()
                        .HasConversion(new ValueConverter<string, string>(v => default(string), v => default(string), new ConverterMappingHints(size: 1)));

                    b.Property<DateTime>("EndDate");

                    b.Property<DateTime>("EntryDate");

                    b.Property<string>("Language")
                        .HasMaxLength(3);

                    b.Property<DateTime>("LastUpdateDate");

                    b.Property<int>("LoadId");

                    b.Property<long>("ProOrder");

                    b.Property<short>("RecordIdentifier");

                    b.Property<DateTime>("StartDate");

                    b.Property<int>("Usrn");

                    b.HasKey("StreetDescription", "Locality", "TownName", "AdminstrativeArea")
                        .HasName("PK_StreetDescriptor");

                    b.HasIndex("Usrn");

                    b.ToTable("StreetDescriptor");
                });

            modelBuilder.Entity("TQ.Geocoding.DataLoad.Models.Classes.Trailer", b =>
                {
                    b.Property<long>("OriginalFileId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("EntryDate");

                    b.Property<int>("LoadId");

                    b.Property<short>("NextVolumeName");

                    b.Property<long>("RecordCount");

                    b.Property<short>("RecordIdentifier");

                    b.Property<TimeSpan>("TimeStamp");

                    b.HasKey("OriginalFileId")
                        .HasName("PK_Trailer");

                    b.ToTable("Trailer");
                });

            modelBuilder.Entity("TQ.Geocoding.DataLoad.Models.Classes.ApplicationCrossReference", b =>
                {
                    b.HasOne("TQ.Geocoding.DataLoad.Models.Classes.BasicLandAndPropertyUnit", "BasicLandAndPropertyUnit")
                        .WithMany()
                        .HasForeignKey("Uprn")
                        .HasConstraintName("FK_ApplicationCrossReference_BLPU")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TQ.Geocoding.DataLoad.Models.Classes.Classification", b =>
                {
                    b.HasOne("TQ.Geocoding.DataLoad.Models.Classes.BasicLandAndPropertyUnit", "BasicLandAndPropertyUnit")
                        .WithMany()
                        .HasForeignKey("Uprn")
                        .HasConstraintName("FK_Classification_BLPU")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TQ.Geocoding.DataLoad.Models.Classes.DeliveryPointAddress", b =>
                {
                    b.HasOne("TQ.Geocoding.DataLoad.Models.Classes.BasicLandAndPropertyUnit", "BasicLandAndPropertyUnit")
                        .WithMany()
                        .HasForeignKey("Uprn")
                        .HasConstraintName("FK_DeliveryPointAddress_BLPU")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TQ.Geocoding.DataLoad.Models.Classes.LandAndPropertyIdentifier", b =>
                {
                    b.HasOne("TQ.Geocoding.DataLoad.Models.Classes.BasicLandAndPropertyUnit", "BasicLandAndPropertyUnit")
                        .WithMany()
                        .HasForeignKey("Uprn")
                        .HasConstraintName("FK_LandAndPropertyIdentifier_BLPU")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TQ.Geocoding.DataLoad.Models.Classes.Street", "Street")
                        .WithMany()
                        .HasForeignKey("Usrn")
                        .HasConstraintName("FK_LandAndPropertyIdentifier_Street")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TQ.Geocoding.DataLoad.Models.Classes.Organisation", b =>
                {
                    b.HasOne("TQ.Geocoding.DataLoad.Models.Classes.BasicLandAndPropertyUnit", "BasicLandAndPropertyUnit")
                        .WithMany()
                        .HasForeignKey("Uprn")
                        .HasConstraintName("FK_Organisation_BLPU")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TQ.Geocoding.DataLoad.Models.Classes.SearchableAddress", b =>
                {
                    b.HasOne("TQ.Geocoding.DataLoad.Models.Classes.BasicLandAndPropertyUnit", "BasicLandAndPropertyUnit")
                        .WithMany()
                        .HasForeignKey("Uprn")
                        .HasConstraintName("FK_SearchableAddress_BLPU")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TQ.Geocoding.DataLoad.Models.Classes.SearchableGeographic", b =>
                {
                    b.HasOne("TQ.Geocoding.DataLoad.Models.Classes.BasicLandAndPropertyUnit", "BasicLandAndPropertyUnit")
                        .WithMany()
                        .HasForeignKey("Uprn")
                        .HasConstraintName("FK_SearchableGeographic_BLPU")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TQ.Geocoding.DataLoad.Models.Classes.StreetDescriptor", b =>
                {
                    b.HasOne("TQ.Geocoding.DataLoad.Models.Classes.Street", "Street")
                        .WithMany()
                        .HasForeignKey("Usrn")
                        .HasConstraintName("FK_StreetDescriptor_Street")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
