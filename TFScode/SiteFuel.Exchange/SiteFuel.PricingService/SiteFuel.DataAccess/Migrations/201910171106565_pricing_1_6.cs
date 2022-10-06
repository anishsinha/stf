namespace SiteFuel.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pricing_1_6 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CurrencyRates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FromCurrency = c.String(nullable: false, maxLength: 3),
                        ExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 8),
                        ToCurrency = c.String(nullable: false, maxLength: 3),
                        CreatedDate = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MstLookups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Key = c.String(nullable: false, maxLength: 256),
                        Description = c.String(maxLength: 500),
                        IsActive = c.Boolean(nullable: false),
                        UpdatedBy = c.Int(nullable: false),
                        UpdatedDate = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MstLookupTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                        Value = c.Int(nullable: false),
                        Description = c.String(maxLength: 500),
                        LookupId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        UpdatedBy = c.Int(nullable: false),
                        UpdatedDate = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MstLookups", t => t.LookupId, cascadeDelete: true)
                .Index(t => t.LookupId);
            
            CreateTable(
                "dbo.MstPricingCodes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 256),
                        PricingSourceId = c.Int(nullable: false),
                        PricingTypeId = c.Int(nullable: false),
                        RackTypeId = c.Int(nullable: false),
                        FeedTypeId = c.Int(nullable: false),
                        QuantityIndicatorId = c.Int(nullable: false),
                        FuelClassTypeId = c.Int(nullable: false),
                        WeekendPricingTypeId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MstPricingSources", t => t.PricingSourceId, cascadeDelete: true)
                .Index(t => t.PricingSourceId);
            
            CreateTable(
                "dbo.RequestPriceDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PricingCodeId = c.Int(nullable: false),
                        RackAvgTypeId = c.Int(),
                        PricePerGallon = c.Decimal(nullable: false, precision: 18, scale: 8),
                        SupplierCost = c.Decimal(precision: 18, scale: 8),
                        SupplierCostTypeId = c.Int(),
                        MarginTypeId = c.Int(),
                        Margin = c.Decimal(nullable: false, precision: 18, scale: 8),
                        BasePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BaseSupplierCost = c.Decimal(precision: 18, scale: 2),
                        Currency = c.Int(nullable: false),
                        ExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UoM = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MstPricingCodes", t => t.PricingCodeId, cascadeDelete: true)
                .Index(t => t.PricingCodeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RequestPriceDetails", "PricingCodeId", "dbo.MstPricingCodes");
            DropForeignKey("dbo.MstPricingCodes", "PricingSourceId", "dbo.MstPricingSources");
            DropForeignKey("dbo.MstLookupTypes", "LookupId", "dbo.MstLookups");
            DropIndex("dbo.RequestPriceDetails", new[] { "PricingCodeId" });
            DropIndex("dbo.MstPricingCodes", new[] { "PricingSourceId" });
            DropIndex("dbo.MstLookupTypes", new[] { "LookupId" });
            DropTable("dbo.RequestPriceDetails");
            DropTable("dbo.MstPricingCodes");
            DropTable("dbo.MstLookupTypes");
            DropTable("dbo.MstLookups");
            DropTable("dbo.CurrencyRates");
        }
    }
}
