namespace TrueFill.DemandCaptureDataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TFX_1_85 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Sale24Hours",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false, storeType: "date"),
                        SaleTankId = c.Int(nullable: false),
                        From0To1 = c.Decimal(nullable: false, precision: 18, scale: 8),
                        From1To2 = c.Decimal(nullable: false, precision: 18, scale: 8),
                        From2To3 = c.Decimal(nullable: false, precision: 18, scale: 8),
                        From3To4 = c.Decimal(nullable: false, precision: 18, scale: 8),
                        From4To5 = c.Decimal(nullable: false, precision: 18, scale: 8),
                        From5To6 = c.Decimal(nullable: false, precision: 18, scale: 8),
                        From6To7 = c.Decimal(nullable: false, precision: 18, scale: 8),
                        From7To8 = c.Decimal(nullable: false, precision: 18, scale: 8),
                        From8To9 = c.Decimal(nullable: false, precision: 18, scale: 8),
                        From9To10 = c.Decimal(nullable: false, precision: 18, scale: 8),
                        From10To11 = c.Decimal(nullable: false, precision: 18, scale: 8),
                        From11To12 = c.Decimal(nullable: false, precision: 18, scale: 8),
                        From12To13 = c.Decimal(nullable: false, precision: 18, scale: 8),
                        From13To14 = c.Decimal(nullable: false, precision: 18, scale: 8),
                        From14To15 = c.Decimal(nullable: false, precision: 18, scale: 8),
                        From15To16 = c.Decimal(nullable: false, precision: 18, scale: 8),
                        From16To17 = c.Decimal(nullable: false, precision: 18, scale: 8),
                        From17To18 = c.Decimal(nullable: false, precision: 18, scale: 8),
                        From18To19 = c.Decimal(nullable: false, precision: 18, scale: 8),
                        From19To20 = c.Decimal(nullable: false, precision: 18, scale: 8),
                        From20To21 = c.Decimal(nullable: false, precision: 18, scale: 8),
                        From21To22 = c.Decimal(nullable: false, precision: 18, scale: 8),
                        From22To23 = c.Decimal(nullable: false, precision: 18, scale: 8),
                        From23To0 = c.Decimal(nullable: false, precision: 18, scale: 8),
                        DayTotal = c.Decimal(nullable: false, precision: 38, scale: 8),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SaleTanks", t => t.SaleTankId, cascadeDelete: true)
                .Index(t => t.SaleTankId);
            
            CreateTable(
                "dbo.SaleTanks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SiteId = c.String(nullable: false, maxLength: 32),
                        TankId = c.String(nullable: false, maxLength: 32),
                        StorageId = c.String(nullable: false, maxLength: 32),
                        BandPeriod = c.Int(nullable: false),
                        DayStartOn = c.Time(nullable: false, precision: 7),
                        MaxFill = c.Decimal(precision: 18, scale: 8),
                        FillType = c.Int(),
                        FuelCapacity = c.Decimal(precision: 18, scale: 8),
                        Retain = c.Int(),
                        SaftyStock = c.Int(),
                        Runout = c.Int(),
                        InventoryUoM = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SaleBandWises",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false, storeType: "date"),
                        BandStartDate = c.DateTime(nullable: false, storeType: "date"),
                        BandStartTime = c.Time(nullable: false, precision: 7),
                        BandEndDate = c.DateTime(nullable: false, storeType: "date"),
                        BandEndTime = c.Time(nullable: false, precision: 7),
                        BandNumber = c.Int(nullable: false),
                        SaleTankId = c.Int(nullable: false),
                        TotalSale = c.Decimal(nullable: false, precision: 18, scale: 8),
                        AverageSale = c.Decimal(nullable: false, precision: 18, scale: 8),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SaleTanks", t => t.SaleTankId, cascadeDelete: true)
                .Index(t => t.SaleTankId);
            
            CreateTable(
                "dbo.SaleComsumptionRates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false, storeType: "date"),
                        BandStartDate = c.DateTime(nullable: false, storeType: "date"),
                        BandStartTime = c.Time(nullable: false, precision: 7),
                        BandEndDate = c.DateTime(nullable: false, storeType: "date"),
                        BandEndTime = c.Time(nullable: false, precision: 7),
                        BandNumber = c.Int(nullable: false),
                        SaleTankId = c.Int(nullable: false),
                        RetainHours = c.Decimal(nullable: false, precision: 18, scale: 8),
                        SaftyStockHours = c.Decimal(nullable: false, precision: 18, scale: 8),
                        RunoutHours = c.Decimal(nullable: false, precision: 18, scale: 8),
                        RemainingHours = c.Decimal(nullable: false, precision: 18, scale: 8),
                        UoM = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SaleTanks", t => t.SaleTankId, cascadeDelete: true)
                .Index(t => t.SaleTankId);
            
            CreateTable(
                "dbo.SaleMonthlyDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false, storeType: "date"),
                        BandStartDate = c.DateTime(nullable: false, storeType: "date"),
                        BandStartTime = c.Time(nullable: false, precision: 7),
                        BandEndDate = c.DateTime(nullable: false, storeType: "date"),
                        BandEndTime = c.Time(nullable: false, precision: 7),
                        BandNumber = c.Int(nullable: false),
                        SaleTankId = c.Int(nullable: false),
                        TotalSale = c.Decimal(nullable: false, precision: 18, scale: 8),
                        AverageSale = c.Decimal(nullable: false, precision: 18, scale: 8),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SaleTanks", t => t.SaleTankId, cascadeDelete: true)
                .Index(t => t.SaleTankId);
            
            CreateTable(
                "dbo.SaleWeeklyDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false, storeType: "date"),
                        BandStartDate = c.DateTime(nullable: false, storeType: "date"),
                        BandStartTime = c.Time(nullable: false, precision: 7),
                        BandEndDate = c.DateTime(nullable: false, storeType: "date"),
                        BandEndTime = c.Time(nullable: false, precision: 7),
                        BandNumber = c.Int(nullable: false),
                        DayId = c.Int(nullable: false),
                        WeekId = c.Int(nullable: false),
                        SaleTankId = c.Int(nullable: false),
                        TotalSale = c.Decimal(nullable: false, precision: 18, scale: 8),
                        AverageSale = c.Decimal(nullable: false, precision: 18, scale: 8),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SaleTanks", t => t.SaleTankId, cascadeDelete: true)
                .Index(t => t.SaleTankId);
            
            AlterColumn("dbo.Demands", "SiteId", c => c.String(maxLength: 64));
            AlterColumn("dbo.Demands", "TankId", c => c.String(maxLength: 64));
            AlterColumn("dbo.Demands", "StorageId", c => c.String(maxLength: 64));
            AlterColumn("dbo.TankDrops", "StartTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.TankDrops", "EndTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SaleWeeklyDatas", "SaleTankId", "dbo.SaleTanks");
            DropForeignKey("dbo.SaleMonthlyDatas", "SaleTankId", "dbo.SaleTanks");
            DropForeignKey("dbo.SaleComsumptionRates", "SaleTankId", "dbo.SaleTanks");
            DropForeignKey("dbo.SaleBandWises", "SaleTankId", "dbo.SaleTanks");
            DropForeignKey("dbo.Sale24Hours", "SaleTankId", "dbo.SaleTanks");
            DropIndex("dbo.SaleWeeklyDatas", new[] { "SaleTankId" });
            DropIndex("dbo.SaleMonthlyDatas", new[] { "SaleTankId" });
            DropIndex("dbo.SaleComsumptionRates", new[] { "SaleTankId" });
            DropIndex("dbo.SaleBandWises", new[] { "SaleTankId" });
            DropIndex("dbo.Sale24Hours", new[] { "SaleTankId" });
            AlterColumn("dbo.TankDrops", "EndTime", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.TankDrops", "StartTime", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.Demands", "StorageId", c => c.String(maxLength: 32));
            AlterColumn("dbo.Demands", "TankId", c => c.String(maxLength: 32));
            AlterColumn("dbo.Demands", "SiteId", c => c.String(maxLength: 32));
            DropTable("dbo.SaleWeeklyDatas");
            DropTable("dbo.SaleMonthlyDatas");
            DropTable("dbo.SaleComsumptionRates");
            DropTable("dbo.SaleBandWises");
            DropTable("dbo.SaleTanks");
            DropTable("dbo.Sale24Hours");
        }
    }
}
