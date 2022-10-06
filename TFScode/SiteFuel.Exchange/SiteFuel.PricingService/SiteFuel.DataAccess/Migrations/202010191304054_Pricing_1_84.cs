namespace SiteFuel.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pricing_1_84 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CumulationDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RequestPriceDetailId = c.Int(nullable: false),
                        StartDate = c.DateTimeOffset(nullable: false, precision: 7),
                        EndDate = c.DateTimeOffset(nullable: false, precision: 7),
                        CumulatedQuantity = c.Decimal(nullable: false, precision: 18, scale: 8),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RequestPriceDetails", t => t.RequestPriceDetailId, cascadeDelete: true)
                .Index(t => t.RequestPriceDetailId);
            
            CreateTable(
                "dbo.PricingDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RequestPriceDetailId = c.Int(nullable: false),
                        PricingCodeId = c.Int(nullable: false),
                        RackAvgTypeId = c.Int(),
                        PricePerGallon = c.Decimal(nullable: false, precision: 18, scale: 8),
                        SupplierCost = c.Decimal(precision: 18, scale: 8),
                        SupplierCostTypeId = c.Int(),
                        MarginTypeId = c.Int(),
                        Margin = c.Decimal(nullable: false, precision: 18, scale: 8),
                        BasePrice = c.Decimal(nullable: false, precision: 18, scale: 8),
                        BaseSupplierCost = c.Decimal(precision: 18, scale: 8),
                        MinQuantity = c.Decimal(precision: 18, scale: 8),
                        MaxQuantity = c.Decimal(precision: 18, scale: 8),
                        CityRackTerminalId = c.Int(),
                        TerminalId = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        FuelTypeId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RequestPriceDetails", t => t.RequestPriceDetailId, cascadeDelete: true)
                .Index(t => t.RequestPriceDetailId);
            
            AddColumn("dbo.RequestPriceDetails", "TierTypeId", c => c.Int());
            AddColumn("dbo.RequestPriceDetails", "PricingTypeId", c => c.Int());
            AddColumn("dbo.RequestPriceDetails", "CumulationTypeId", c => c.Int());
            AddColumn("dbo.RequestPriceDetails", "CumulationResetDay", c => c.Int());
            AddColumn("dbo.RequestPriceDetails", "CumulationResetDate", c => c.DateTimeOffset(precision: 7));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PricingDetails", "RequestPriceDetailId", "dbo.RequestPriceDetails");
            DropForeignKey("dbo.CumulationDetails", "RequestPriceDetailId", "dbo.RequestPriceDetails");
            DropIndex("dbo.PricingDetails", new[] { "RequestPriceDetailId" });
            DropIndex("dbo.CumulationDetails", new[] { "RequestPriceDetailId" });
            DropColumn("dbo.RequestPriceDetails", "CumulationResetDate");
            DropColumn("dbo.RequestPriceDetails", "CumulationResetDay");
            DropColumn("dbo.RequestPriceDetails", "CumulationTypeId");
            DropColumn("dbo.RequestPriceDetails", "PricingTypeId");
            DropColumn("dbo.RequestPriceDetails", "TierTypeId");
            DropTable("dbo.PricingDetails");
            DropTable("dbo.CumulationDetails");
        }
    }
}
