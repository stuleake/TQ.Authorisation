using System;
using Microsoft.EntityFrameworkCore;
using TQ.Geocoding.DataLoad.Models.Classes;
using TQ.Geocoding.DataLoad.Models.Enums;

namespace TQ.Geocoding.DataLoad.Models.Context
{
    public class GeocodingDataLoadContext : DbContext
    {
        public GeocodingDataLoadContext(DbContextOptions<GeocodingDataLoadContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationCrossReference> ApplicationCrossReference { get; set; }
        public DbSet<BasicLandAndPropertyUnit> BasicLandAndPropertyUnit { get; set; }
        public DbSet<Classification> Classification { get; set; }
        public DbSet<DeliveryPointAddress> DeliveryPointAddress { get; set; }
        public DbSet<Header> Header { get; set; }
        public DbSet<LandPropertyIdentifier> LandPropertyIdentifier { get; set; }
        public DbSet<Metadata> Metadata { get; set; }
        public DbSet<Organisation> Organisation { get; set; }
        public DbSet<SearchableAddress> SearchableAddress { get; set; }
        public DbSet<SearchableGeographic> SearchableGeographic { get; set; }
        public DbSet<Staging> Staging { get; set; }
        public DbSet<StagingFilter> StagingFilter { get; set; }
        public DbSet<StagingFilterUprn> StagingFilterUprn { get; set; }
        public DbSet<Street> Street { get; set; }
        public DbSet<StreetDescriptor> StreetDescriptor { get; set; }
        public DbSet<Trailer> Trailer { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationCrossReference>()
                .HasKey(x => x.XrefKey)
                .HasName("PK_ApplicationCrossReference");

            modelBuilder.Entity<ApplicationCrossReference>()
                .HasOne(x => x.BasicLandAndPropertyUnit)
                .WithMany()
                .HasForeignKey(x => x.Uprn)
                .HasConstraintName("FK_ApplicationCrossReference_BLPU");

            modelBuilder.Entity<BasicLandAndPropertyUnit>()
                .HasKey(x => x.Uprn)
                .HasName("PK_BasicLandAndPropertyUnit");

            modelBuilder.Entity<BasicLandAndPropertyUnit>()
                .Property(x => x.Uprn)
                .ValueGeneratedNever();

            modelBuilder.Entity<Classification>()
                .HasKey(x => x.ClassificationKey)
                .HasName("PK_Classification");

            modelBuilder.Entity<Classification>()
                .HasOne(x => x.BasicLandAndPropertyUnit)
                .WithMany()
                .HasForeignKey(x => x.Uprn)
                .HasConstraintName("FK_Classification_BLPU");

            modelBuilder.Entity<DeliveryPointAddress>()
                .HasKey(x => x.Udprn)
                .HasName("PK_DeliveryPointAddress");

            modelBuilder.Entity<DeliveryPointAddress>()
                .HasOne(x => x.BasicLandAndPropertyUnit)
                .WithMany()
                .HasForeignKey(x => x.Uprn)
                .HasConstraintName("FK_DeliveryPointAddress_BLPU");

            modelBuilder.Entity<DeliveryPointAddress>()
                .Property(x => x.Udprn)
                .ValueGeneratedNever();

            modelBuilder.Entity<Header>()
                .HasKey(x => x.HeaderId)
                .HasName("PK_Header");

            modelBuilder.Entity<LandPropertyIdentifier>()
                .HasKey(x => x.LpiKey)
                .HasName("PK_LandPropertyIdentifier");

            modelBuilder.Entity<LandPropertyIdentifier>()
                .HasOne(x => x.BasicLandAndPropertyUnit)
                .WithMany()
                .HasForeignKey(x => x.Uprn)
                .HasConstraintName("FK_LandPropertyIdentifier_BLPU");

            modelBuilder.Entity<LandPropertyIdentifier>()
                .HasOne(x => x.Street)
                .WithMany()
                .HasForeignKey(x => x.Usrn)
                .HasConstraintName("FK_LandPropertyIdentifier_Street");

            modelBuilder.Entity<Metadata>()
                .HasKey(x => x.MetadataId)
                .HasName("PK_Metadata");

            modelBuilder.Entity<Organisation>()
                .HasKey(x => x.OrgKey)
                .HasName("PK_Organisation");

            modelBuilder.Entity<Organisation>()
                .HasOne(x => x.BasicLandAndPropertyUnit)
                .WithMany()
                .HasForeignKey(x => x.Uprn)
                .HasConstraintName("FK_Organisation_BLPU");

            modelBuilder.Entity<SearchableAddress>()
            .HasKey(x => x.SearchableAddressId)
            .HasName("PK_SearchableAddress");

            modelBuilder.Entity<SearchableAddress>()
                .HasOne(x => x.BasicLandAndPropertyUnit)
                .WithMany()
                .HasForeignKey(x => x.Uprn)
                .HasConstraintName("FK_SearchableAddress_BLPU");

            modelBuilder.Entity<SearchableAddress>()
                .HasIndex(x => new
                {
                    x.Postcode
                })
                .HasName("UX_SearchableAddress_Postcode");

            modelBuilder.Entity<SearchableAddress>()
                .HasIndex(x => new
                {
                    x.PostcodeNoSpaces
                })
                .HasName("IX_SearchableAddress_PostcodeNoSpaces");

            modelBuilder.Entity<SearchableAddress>()
                .HasIndex(x => new
                {
                    x.SingleLineAddress
                })
                .HasName("IX_SearchableAddress_SingleLineAddress");

            modelBuilder.Entity<SearchableAddress>()
                .HasIndex(x => new
                {
                    x.WelshSingleLineAddress
                })
                .HasName("IX_SearchableAddress_WelshSingleLineAddress");

            modelBuilder.Entity<SearchableGeographic>()
                .HasKey(x => x.SearchableGeographicId)
                .HasName("PK_SearchableGeographic");

            modelBuilder.Entity<SearchableGeographic>()
                .HasOne(x => x.BasicLandAndPropertyUnit)
                .WithMany()
                .HasForeignKey(x => x.Uprn)
                .HasConstraintName("FK_SearchableGeographic_BLPU");

            // textsearchable, textsearchable_addressline, adddressline
            modelBuilder.Entity<SearchableGeographic>()
                    .HasIndex(x => new
                    {
                        x.SaoText,
                        x.SaoStartNumber,
                        x.SaoStartSuffix,
                        x.SaoEndNumber,
                        x.SaoEndSuffix,
                        x.PaoText,
                        x.PaoStartNumber,
                        x.PaoStartSuffix,
                        x.PaoEndNumber,
                        x.PaoEndSuffix,
                        x.StreetDescription,
                        x.Locality,
                        x.TownName,
                        x.AdministrativeArea,
                        x.PostcodeLocator
                    })
                    .HasName("UX_SearchableGeographic_Text");

            modelBuilder.Entity<Staging>()
                .HasKey(x => x.StagingRecordId)
                .HasName("PK_Staging");

            modelBuilder.Entity<Staging>()
                .Property(x => x.StagingRecordId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Staging>()
                .HasIndex(x => new
                {
                    x.RecordIdentifier,
                    x.StagingRecordId
                })
                .HasName("IX_Staging_RecordIdentifier")
                .IsUnique(true);

            modelBuilder.Entity<StagingFilter>()
                .HasKey(x => x.StagingFilterId)
                .HasName("PK_StagingFilter");

            modelBuilder.Entity<StagingFilter>()
                .Property(x => x.StagingFilterId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<StagingFilterUprn>()
                .HasKey(x => x.FilterUprn)
                .HasName("PK_StagingFilterUprn");

            modelBuilder.Entity<Street>()
                .HasKey(x => x.Usrn)
                .HasName("PK_Street");

            modelBuilder.Entity<Street>()
                .Property(x => x.Usrn)
                .ValueGeneratedNever();

            modelBuilder.Entity<StreetDescriptor>()
                .HasOne(x => x.Street)
                .WithMany()
                .HasForeignKey(x => x.Usrn)
                .HasConstraintName("FK_StreetDescriptor_Street");

            modelBuilder.Entity<StreetDescriptor>()
                .HasKey(x => new { x.Usrn, x.Language })
                .HasName("PK_StreetDescriptor");

            modelBuilder.Entity<Trailer>()
                .HasKey(x => x.TrailerId)
                .HasName("PK_Trailer");
        }
    }
}
