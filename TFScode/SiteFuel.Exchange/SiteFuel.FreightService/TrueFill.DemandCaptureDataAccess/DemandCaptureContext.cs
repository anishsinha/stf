using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueFill.DemandCaptureDataAccess.Entities;

namespace TrueFill.DemandCaptureDataAccess
{
    public class DemandCaptureContext : DbContext
    {
        public DemandCaptureContext() : base(ConfigurationManager.ConnectionStrings["DemandCaptureConnectionString"].ConnectionString)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DemandCaptureContext, Migrations.Configuration>());
        }
        


        public virtual DbSet<Demand> Demands { get; set; }
        public virtual DbSet<Sale> Sales { get; set; }
        public virtual DbSet<TankDrop> TankDrops { get; set; }
        public virtual DbSet<SourceFile> SourceFiles { get; set; }
        public virtual DbSet<SaleTank> SaleTanks { get; set; }
        public virtual DbSet<Sale24Hours> Sale24Hours { get; set; }
        public virtual DbSet<SaleBandWise> SaleBandWise { get; set; }
        public virtual DbSet<SaleWeeklyData> SaleWeeklyData { get; set; }
        public virtual DbSet<SaleMonthlyData> SaleMonthlyData { get; set; }
        public virtual DbSet<SaleComsumptionRate> SaleComsumptionRates { get; set; }
        public virtual DbSet<DailySale> DailySales { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sale>()
                .HasIndex(t => new { t.SiteId, t.TankId, t.StorageId })
                .HasName("IX_Sales_SiteId_TankId_StorageId");

            modelBuilder.Entity<Sale>()
                .HasIndex(t => t.CreatedDate)
                .HasName("IX_Sales_CreatedDate");
            
            modelBuilder.Entity<DailySale>()
                .HasIndex(t => new { t.SiteId, t.TankId, t.StorageId })
                .HasName("IX_Sales_SiteId_TankId_StorageId");

            modelBuilder.Entity<DailySale>()
                .HasIndex(t => t.CreatedDate)
                .HasName("IX_Sales_CreatedDate");
            
            modelBuilder.Entity<DailySale>()
                .Property(t => t.DroppedQuantity)
                .HasPrecision(18, 8);

            modelBuilder.Entity<TankDrop>()
                .HasIndex(t => new { t.SiteId, t.TankId, t.StorageId })
                .HasName("IX_TankDrop_SiteId_TankId_StorageId");

            modelBuilder.Entity<TankDrop>()
                .HasIndex(t => t.AssetDropId)
                .HasName("IX_TankDrop_AssetDropId");

            modelBuilder.Entity<Sale24Hours>()
                .Property(t => t.From0To1)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Sale24Hours>()
                .Property(t => t.From1To2)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Sale24Hours>()
                .Property(t => t.From2To3)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Sale24Hours>()
                .Property(t => t.From3To4)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Sale24Hours>()
                .Property(t => t.From4To5)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Sale24Hours>()
                .Property(t => t.From5To6)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Sale24Hours>()
                .Property(t => t.From6To7)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Sale24Hours>()
                .Property(t => t.From7To8)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Sale24Hours>()
                .Property(t => t.From8To9)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Sale24Hours>()
                .Property(t => t.From9To10)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Sale24Hours>()
                .Property(t => t.From10To11)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Sale24Hours>()
                .Property(t => t.From11To12)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Sale24Hours>()
                .Property(t => t.From12To13)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Sale24Hours>()
                .Property(t => t.From13To14)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Sale24Hours>()
                .Property(t => t.From14To15)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Sale24Hours>()
                .Property(t => t.From15To16)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Sale24Hours>()
                .Property(t => t.From16To17)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Sale24Hours>()
                .Property(t => t.From17To18)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Sale24Hours>()
                .Property(t => t.From18To19)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Sale24Hours>()
                .Property(t => t.From19To20)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Sale24Hours>()
                .Property(t => t.From20To21)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Sale24Hours>()
                .Property(t => t.From21To22)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Sale24Hours>()
                .Property(t => t.From22To23)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Sale24Hours>()
                .Property(t => t.From23To0)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Sale24Hours>()
                .Property(t => t.DayTotal)
                .HasPrecision(38, 8);

            modelBuilder.Entity<SaleBandWise>()
                .Property(t => t.TotalSale)
                .HasPrecision(18, 8);

            modelBuilder.Entity<SaleBandWise>()
                .Property(t => t.AverageSale)
                .HasPrecision(18, 8);

            modelBuilder.Entity<SaleComsumptionRate>()
                .Property(t => t.RetainHours)
                .HasPrecision(18, 8);

            modelBuilder.Entity<SaleComsumptionRate>()
                .Property(t => t.SaftyStockHours)
                .HasPrecision(18, 8);

            modelBuilder.Entity<SaleComsumptionRate>()
                .Property(t => t.RunoutHours)
                .HasPrecision(18, 8);

            modelBuilder.Entity<SaleComsumptionRate>()
                .Property(t => t.RemainingHours)
                .HasPrecision(18, 8);

            modelBuilder.Entity<SaleWeeklyData>()
                .Property(t => t.TotalSale)
                .HasPrecision(18, 8);

            modelBuilder.Entity<SaleWeeklyData>()
                .Property(t => t.AverageSale)
                .HasPrecision(18, 8);

            modelBuilder.Entity<SaleMonthlyData>()
                .Property(t => t.TotalSale)
                .HasPrecision(18, 8);

            modelBuilder.Entity<SaleMonthlyData>()
                .Property(t => t.AverageSale)
                .HasPrecision(18, 8);

            modelBuilder.Entity<SaleTank>()
                .Property(t => t.MaxFill)
                .HasPrecision(18, 8);

            modelBuilder.Entity<SaleTank>()
                .Property(t => t.FuelCapacity)
                .HasPrecision(18, 8);
        }
    }
}
