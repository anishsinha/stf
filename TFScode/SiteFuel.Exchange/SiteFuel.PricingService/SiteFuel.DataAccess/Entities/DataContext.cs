using SiteFuel.DataAccess.Entities;
using System.Data.Entity;

namespace SiteFuel.DataAccess.Entities
{
    public class DataContext : DbContext
    {
        public DataContext() : base("PricingDatabaseConnection")
        {

        }
        public virtual DbSet<ExternalPricingAxxis> ExternalPricingAxxis { get; set; }
        public virtual DbSet<ExternalPricingOpis> ExternalPricingOpis { get; set; }
        public virtual DbSet<ExternalPricingPlatts> ExternalPricingPlatts { get; set; }
        public virtual DbSet<MstExternalProduct> MstExternalProducts { get; set; }
        public virtual DbSet<MstExternalTerminal> MstExternalTerminals { get; set; }
        public virtual DbSet<MstProductMapping> MstProductMappings { get; set; }
        public virtual DbSet<MstProduct> MstProducts { get; set; }
        public virtual DbSet<MstProductType> MstProductTypes { get; set; }
        public virtual DbSet<MstPricingSource> MstPricingSources { get; set; }
        public virtual DbSet<MstState> MstStates { get; set; }
        public virtual DbSet<MstCountry> MstCountries { get; set; }
        public virtual DbSet<MstPricingConfig> MstPricingConfig { get; set; }
        public virtual DbSet<MstLookup> MstLookup { get; set; }
        public virtual DbSet<MstLookupType> MstLookupType { get; set; }
        public virtual DbSet<MstPricingCode> MstPricingCodes { get; set; }
        public virtual DbSet<RequestPriceDetail> RequestPriceDetails { get; set; }
        public virtual DbSet<PricingDetail> PricingDetails { get; set; }
        public virtual DbSet<CumulationDetail> CumulationDetails { get; set; }
        public virtual DbSet<CurrencyRate> CurrencyRates { get; set; }
        public virtual DbSet<MstTfxProduct> MstTfxProducts { get; set; }
        public virtual DbSet<MstOPISProduct> MstOPISProducts { get; set; }
        public virtual DbSet<MstClearDyedProductMapping> MstClearDyedProductMapping { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExternalPricingAxxis>()
                    .Property(e => e.AvgPrice)
                    .HasPrecision(18, 8);

            modelBuilder.Entity<MstExternalProduct>()
                    .HasMany(e => e.ExternalPricingData)
                    .WithRequired(e => e.MstExternalProduct)
                    .HasForeignKey(e => e.ProductId)
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstExternalTerminal>()
                    .HasMany(e => e.ExternalPricingData)
                    .WithRequired(e => e.MstExternalTerminal)
                    .HasForeignKey(e => e.TerminalId)
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<ExternalPricingAxxis>()
                    .Property(e => e.LowPrice)
                    .HasPrecision(18, 8);

            modelBuilder.Entity<ExternalPricingAxxis>()
                    .Property(e => e.HighPrice)
                    .HasPrecision(18, 8);

            modelBuilder.Entity<ExternalPricingOpis>()
                    .Property(e => e.Price)
                    .HasPrecision(18, 8);

            modelBuilder.Entity<ExternalPricingPlatts>()
                    .Property(e => e.Price)
                    .HasPrecision(18, 8);

            modelBuilder.Entity<MstState>()
                    .HasMany(e => e.MstExternalTerminals)
                    .WithRequired(e => e.MstState)
                    .HasForeignKey(e => e.StateId)
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstExternalProduct>()
                    .HasMany(e => e.MstProductMappings)
                    .WithRequired(e => e.MstExternalProduct)
                    .HasForeignKey(e => e.ExternalProductId)
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstExternalTerminal>()
                    .HasMany(e => e.MstProductMappings)
                    .WithRequired(e => e.MstExternalTerminal)
                    .HasForeignKey(e => e.ExternalTerminalId)
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstProduct>()
                    .HasMany(e => e.MstProductMappings)
                    .WithRequired(e => e.MstProduct)
                    .HasForeignKey(e => e.ProductId)
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstProductType>()
                    .HasMany(e => e.MstProducts)
                    .WithRequired(e => e.MstProductType)
                    .HasForeignKey(e => e.ProductTypeId)
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstCountry>()
                    .HasMany(e => e.MstStates)
                    .WithRequired(e => e.MstCountry)
                    .HasForeignKey(e => e.CountryId)
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<CurrencyRate>()
                    .Property(e => e.ExchangeRate)
                    .HasPrecision(18, 8);

            modelBuilder.Entity<PricingDetail>()
                    .Property(e => e.PricePerGallon)
                    .HasPrecision(18, 8);

            modelBuilder.Entity<PricingDetail>()
                    .Property(e => e.Margin)
                    .HasPrecision(18, 8);

            modelBuilder.Entity<PricingDetail>()
                    .Property(e => e.SupplierCost)
                    .HasPrecision(18, 8);

            modelBuilder.Entity<PricingDetail>()
                    .Property(e => e.BasePrice)
                   .HasPrecision(18, 8);

            modelBuilder.Entity<PricingDetail>()
                    .Property(e => e.BaseSupplierCost)
                    .HasPrecision(18, 8);

            modelBuilder.Entity<PricingDetail>()
                    .Property(e => e.MinQuantity)
                    .HasPrecision(18, 8);

            modelBuilder.Entity<PricingDetail>()
                    .Property(e => e.MaxQuantity)
                    .HasPrecision(18, 8);

            modelBuilder.Entity<CumulationDetail>()
                    .Property(e => e.CumulatedQuantity)
                    .HasPrecision(18, 8);

            modelBuilder.Entity<MstExternalTerminal>()
                    .Property(e => e.Latitude)
                    .HasPrecision(18, 8);

            modelBuilder.Entity<MstExternalTerminal>()
                    .Property(e => e.Longitude)
                    .HasPrecision(18, 8);
        }
    }
}
