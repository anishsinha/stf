namespace SiteFuel.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pricing_1_85 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RequestPriceDetails", "PricingCodeId", "dbo.MstPricingCodes");
            DropIndex("dbo.RequestPriceDetails", new[] { "PricingCodeId" });
            AddColumn("dbo.PricingDetails", "FuelTypeId", c => c.Int());
            CreateIndex("dbo.PricingDetails", "PricingCodeId");
            AddForeignKey("dbo.PricingDetails", "PricingCodeId", "dbo.MstPricingCodes", "Id", cascadeDelete: true);
            DropColumn("dbo.RequestPriceDetails", "PricingCodeId");
            DropColumn("dbo.RequestPriceDetails", "RackAvgTypeId");
            DropColumn("dbo.RequestPriceDetails", "PricePerGallon");
            DropColumn("dbo.RequestPriceDetails", "SupplierCost");
            DropColumn("dbo.RequestPriceDetails", "SupplierCostTypeId");
            DropColumn("dbo.RequestPriceDetails", "MarginTypeId");
            DropColumn("dbo.RequestPriceDetails", "Margin");
            DropColumn("dbo.RequestPriceDetails", "BasePrice");
            DropColumn("dbo.RequestPriceDetails", "BaseSupplierCost");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RequestPriceDetails", "BaseSupplierCost", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.RequestPriceDetails", "BasePrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.RequestPriceDetails", "Margin", c => c.Decimal(nullable: false, precision: 18, scale: 8));
            AddColumn("dbo.RequestPriceDetails", "MarginTypeId", c => c.Int());
            AddColumn("dbo.RequestPriceDetails", "SupplierCostTypeId", c => c.Int());
            AddColumn("dbo.RequestPriceDetails", "SupplierCost", c => c.Decimal(precision: 18, scale: 8));
            AddColumn("dbo.RequestPriceDetails", "PricePerGallon", c => c.Decimal(nullable: false, precision: 18, scale: 8));
            AddColumn("dbo.RequestPriceDetails", "RackAvgTypeId", c => c.Int());
            AddColumn("dbo.RequestPriceDetails", "PricingCodeId", c => c.Int(nullable: false));
            DropForeignKey("dbo.PricingDetails", "PricingCodeId", "dbo.MstPricingCodes");
            DropIndex("dbo.PricingDetails", new[] { "PricingCodeId" });
            DropColumn("dbo.PricingDetails", "FuelTypeId");
            CreateIndex("dbo.RequestPriceDetails", "PricingCodeId");
            AddForeignKey("dbo.RequestPriceDetails", "PricingCodeId", "dbo.MstPricingCodes", "Id", cascadeDelete: true);
        }
    }
}
