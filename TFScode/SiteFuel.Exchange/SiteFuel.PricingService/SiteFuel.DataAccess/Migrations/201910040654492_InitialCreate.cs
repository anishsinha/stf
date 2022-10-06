namespace SiteFuel.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DebugLog",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        MachineName = c.String(maxLength: 200),
                        SiteName = c.String(nullable: false, maxLength: 200),
                        LogDateTime = c.DateTime(nullable: false),
                        Level = c.String(nullable: false, maxLength: 5),
                        UserName = c.String(maxLength: 200),
                        Message = c.String(nullable: false),
                        Logger = c.String(maxLength: 300),
                        Properties = c.String(),
                        ServerName = c.String(maxLength: 200),
                        Port = c.String(maxLength: 100),
                        Url = c.String(maxLength: 2000),
                        Https = c.Boolean(),
                        ServerAddress = c.String(maxLength: 100),
                        RemoteAddress = c.String(maxLength: 100),
                        Callsite = c.String(maxLength: 300),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ExceptionLog",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        MachineName = c.String(maxLength: 200),
                        SiteName = c.String(nullable: false, maxLength: 200),
                        LogDateTime = c.DateTime(nullable: false),
                        Level = c.String(nullable: false, maxLength: 5),
                        UserName = c.String(maxLength: 200),
                        Message = c.String(nullable: false),
                        Logger = c.String(maxLength: 300),
                        Properties = c.String(),
                        ServerName = c.String(maxLength: 200),
                        Port = c.String(maxLength: 100),
                        Url = c.String(maxLength: 2000),
                        Https = c.Boolean(),
                        ServerAddress = c.String(maxLength: 100),
                        RemoteAddress = c.String(maxLength: 100),
                        Callsite = c.String(maxLength: 300),
                        Exception = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ExternalPricingData",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TerminalId = c.Int(nullable: false),
                        TerminalAbbreviation = c.String(nullable: false, maxLength: 16),
                        ProductId = c.Int(nullable: false),
                        ProductCode = c.String(nullable: false, maxLength: 32),
                        AvgPrice = c.Decimal(nullable: false, precision: 18, scale: 8),
                        LowPrice = c.Decimal(nullable: false, precision: 18, scale: 8),
                        HighPrice = c.Decimal(nullable: false, precision: 18, scale: 8),
                        EffectiveDate = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedDate = c.DateTimeOffset(nullable: false, precision: 7),
                        Currency = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MstExternalProducts", t => t.ProductId)
                .ForeignKey("dbo.MstExternalTerminals", t => t.TerminalId)
                .Index(t => t.TerminalId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.MstExternalProducts",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Code = c.String(nullable: false, maxLength: 32),
                        Name = c.String(nullable: false, maxLength: 256),
                        IsActive = c.Boolean(nullable: false),
                        UpdatedBy = c.Int(nullable: false),
                        UpdatedDate = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MstProductMappings",
                c => new
                    {
                        ExternalTerminalId = c.Int(nullable: false),
                        ExternalProductId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        UpdatedBy = c.Int(nullable: false),
                        UpdatedDate = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => new { t.ExternalTerminalId, t.ExternalProductId, t.ProductId })
                .ForeignKey("dbo.MstExternalTerminals", t => t.ExternalTerminalId)
                .ForeignKey("dbo.MstProducts", t => t.ProductId)
                .ForeignKey("dbo.MstExternalProducts", t => t.ExternalProductId)
                .Index(t => t.ExternalTerminalId)
                .Index(t => t.ExternalProductId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.MstExternalTerminals",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        ControlNumber = c.String(nullable: false, maxLength: 16),
                        Name = c.String(nullable: false, maxLength: 256),
                        Code = c.String(nullable: false, maxLength: 32),
                        Abbreviation = c.String(nullable: false, maxLength: 16),
                        Address = c.String(nullable: false),
                        City = c.String(nullable: false, maxLength: 128),
                        StateCode = c.String(nullable: false, maxLength: 32),
                        StateId = c.Int(nullable: false),
                        ZipCode = c.String(nullable: false, maxLength: 32),
                        CountryCode = c.String(nullable: false, maxLength: 32),
                        Latitude = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Longitude = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsActive = c.Boolean(nullable: false),
                        UpdatedBy = c.Int(nullable: false),
                        UpdatedDate = c.DateTimeOffset(nullable: false, precision: 7),
                        CountyName = c.String(nullable: false, maxLength: 64),
                        Currency = c.Int(nullable: false),
                        PricingSourceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MstPricingSources", t => t.PricingSourceId, cascadeDelete: true)
                .ForeignKey("dbo.MstStates", t => t.StateId)
                .Index(t => t.StateId)
                .Index(t => t.PricingSourceId);
            
            CreateTable(
                "dbo.MstPricingSources",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 64),
                        IsActive = c.Boolean(nullable: false),
                        UpdatedBy = c.Int(nullable: false),
                        UpdatedDate = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MstStates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 32),
                        Name = c.String(nullable: false, maxLength: 256),
                        CountryId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        UpdatedBy = c.Int(nullable: false),
                        UpdatedDate = c.DateTimeOffset(nullable: false, precision: 7),
                        QuantityIndicatorTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MstProducts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                        ProductTypeId = c.Int(nullable: false),
                        ProductDisplayGroupId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        UpdatedBy = c.Int(nullable: false),
                        UpdatedDate = c.DateTimeOffset(nullable: false, precision: 7),
                        PricingSourceId = c.Int(nullable: false),
                        MappedParentId = c.Int(),
                        ProductCode = c.String(maxLength: 32),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MstPricingSources", t => t.PricingSourceId, cascadeDelete: true)
                .ForeignKey("dbo.MstProductTypes", t => t.ProductTypeId)
                .Index(t => t.ProductTypeId)
                .Index(t => t.PricingSourceId);
            
            CreateTable(
                "dbo.MstProductTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                        IsActive = c.Boolean(nullable: false),
                        UpdatedBy = c.Int(nullable: false),
                        UpdatedDate = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ExternalPricingOpis",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TerminalId = c.Int(nullable: false),
                        TerminalAbbreviation = c.String(maxLength: 16),
                        ProductId = c.Int(nullable: false),
                        Symbol = c.String(maxLength: 32),
                        LoadDate = c.DateTimeOffset(nullable: false, precision: 7),
                        FeedTypeId = c.Int(nullable: false),
                        ReportedDate = c.DateTimeOffset(nullable: false, precision: 7),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 8),
                        Unit = c.Int(nullable: false),
                        Currency = c.Int(nullable: false),
                        SupplierNumber = c.Int(nullable: false),
                        Supplier = c.String(maxLength: 128),
                        RackTypeId = c.Int(nullable: false),
                        SupplierBrand = c.String(maxLength: 128),
                        SupplierBrandId = c.Int(nullable: false),
                        PriceType = c.String(maxLength: 64),
                        PriceTypeId = c.Int(nullable: false),
                        LiftPoint = c.String(maxLength: 256),
                        ExternalProductId = c.Int(nullable: false),
                        SourceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MstExternalTerminals", t => t.TerminalId, cascadeDelete: false)
                .ForeignKey("dbo.MstProducts", t => t.ProductId, cascadeDelete: false)
                .Index(t => t.TerminalId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.ExternalPricingPlatts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TerminalId = c.Int(nullable: false),
                        TerminalAbbreviation = c.String(maxLength: 16),
                        ProductId = c.Int(nullable: false),
                        Symbol = c.String(maxLength: 32),
                        LoadDate = c.DateTimeOffset(nullable: false, precision: 7),
                        ReportedDate = c.DateTimeOffset(nullable: false, precision: 7),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 8),
                        Unit = c.Int(nullable: false),
                        Currency = c.Int(nullable: false),
                        SupplierNumber = c.Int(nullable: false),
                        Supplier = c.String(maxLength: 128),
                        LiftPoint = c.String(maxLength: 256),
                        ExternalProductId = c.Int(nullable: false),
                        SourceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MstExternalTerminals", t => t.TerminalId, cascadeDelete: false)
                .ForeignKey("dbo.MstProducts", t => t.ProductId, cascadeDelete: false)
                .Index(t => t.TerminalId)
                .Index(t => t.ProductId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ExternalPricingPlatts", "ProductId", "dbo.MstProducts");
            DropForeignKey("dbo.ExternalPricingPlatts", "TerminalId", "dbo.MstExternalTerminals");
            DropForeignKey("dbo.ExternalPricingOpis", "ProductId", "dbo.MstProducts");
            DropForeignKey("dbo.ExternalPricingOpis", "TerminalId", "dbo.MstExternalTerminals");
            DropForeignKey("dbo.MstProductMappings", "ExternalProductId", "dbo.MstExternalProducts");
            DropForeignKey("dbo.MstProducts", "ProductTypeId", "dbo.MstProductTypes");
            DropForeignKey("dbo.MstProductMappings", "ProductId", "dbo.MstProducts");
            DropForeignKey("dbo.MstProducts", "PricingSourceId", "dbo.MstPricingSources");
            DropForeignKey("dbo.MstExternalTerminals", "StateId", "dbo.MstStates");
            DropForeignKey("dbo.MstProductMappings", "ExternalTerminalId", "dbo.MstExternalTerminals");
            DropForeignKey("dbo.MstExternalTerminals", "PricingSourceId", "dbo.MstPricingSources");
            DropForeignKey("dbo.ExternalPricingData", "TerminalId", "dbo.MstExternalTerminals");
            DropForeignKey("dbo.ExternalPricingData", "ProductId", "dbo.MstExternalProducts");
            DropIndex("dbo.ExternalPricingPlatts", new[] { "ProductId" });
            DropIndex("dbo.ExternalPricingPlatts", new[] { "TerminalId" });
            DropIndex("dbo.ExternalPricingOpis", new[] { "ProductId" });
            DropIndex("dbo.ExternalPricingOpis", new[] { "TerminalId" });
            DropIndex("dbo.MstProducts", new[] { "PricingSourceId" });
            DropIndex("dbo.MstProducts", new[] { "ProductTypeId" });
            DropIndex("dbo.MstExternalTerminals", new[] { "PricingSourceId" });
            DropIndex("dbo.MstExternalTerminals", new[] { "StateId" });
            DropIndex("dbo.MstProductMappings", new[] { "ProductId" });
            DropIndex("dbo.MstProductMappings", new[] { "ExternalProductId" });
            DropIndex("dbo.MstProductMappings", new[] { "ExternalTerminalId" });
            DropIndex("dbo.ExternalPricingData", new[] { "ProductId" });
            DropIndex("dbo.ExternalPricingData", new[] { "TerminalId" });
            DropTable("dbo.ExternalPricingPlatts");
            DropTable("dbo.ExternalPricingOpis");
            DropTable("dbo.MstProductTypes");
            DropTable("dbo.MstProducts");
            DropTable("dbo.MstStates");
            DropTable("dbo.MstPricingSources");
            DropTable("dbo.MstExternalTerminals");
            DropTable("dbo.MstProductMappings");
            DropTable("dbo.MstExternalProducts");
            DropTable("dbo.ExternalPricingData");
            DropTable("dbo.ExceptionLog");
            DropTable("dbo.DebugLog");
        }
    }
}
