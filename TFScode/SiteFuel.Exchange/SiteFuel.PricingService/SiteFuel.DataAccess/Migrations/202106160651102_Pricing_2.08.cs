namespace SiteFuel.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PricingDetails", "ParameterJson", c => c.String());
            AlterColumn("dbo.CumulationDetails", "RequestPriceDetailId", c => c.Int(nullable: false));
            AlterColumn("dbo.PricingDetails", "RequestPriceDetailId", c => c.Int(nullable: false));
            AlterColumn("dbo.PricingDetails", "PricingCodeId", c => c.Int(nullable: false));
            AlterColumn("dbo.MstPricingCodes", "PricingSourceId", c => c.Int(nullable: false));
            AlterColumn("dbo.ExternalPricingAxxis", "TerminalId", c => c.Int(nullable: false));
            AlterColumn("dbo.ExternalPricingAxxis", "ProductId", c => c.Int(nullable: false));
            AlterColumn("dbo.MstProductMappings", "ExternalTerminalId", c => c.Int(nullable: false));
            AlterColumn("dbo.MstProductMappings", "ExternalProductId", c => c.Int(nullable: false));
            AlterColumn("dbo.MstProductMappings", "ProductId", c => c.Int(nullable: false));
            AlterColumn("dbo.MstExternalTerminals", "StateId", c => c.Int(nullable: false));
            AlterColumn("dbo.MstExternalTerminals", "PricingSourceId", c => c.Int(nullable: false));
            AlterColumn("dbo.MstStates", "CountryId", c => c.Int(nullable: false));
            AlterColumn("dbo.MstProducts", "ProductTypeId", c => c.Int(nullable: false));
            AlterColumn("dbo.MstProducts", "PricingSourceId", c => c.Int(nullable: false));
            AlterColumn("dbo.MstProducts", "TfxProductId", c => c.Int());
            AlterColumn("dbo.MstTfxProducts", "ProductTypeId", c => c.Int(nullable: false));
            AlterColumn("dbo.ExternalPricingOpis", "TerminalId", c => c.Int(nullable: false));
            AlterColumn("dbo.ExternalPricingOpis", "ProductId", c => c.Int(nullable: false));
            AlterColumn("dbo.ExternalPricingPlatts", "TerminalId", c => c.Int(nullable: false));
            AlterColumn("dbo.ExternalPricingPlatts", "ProductId", c => c.Int(nullable: false));
            AlterColumn("dbo.MstClearDyedProductMappings", "ClearProductId", c => c.Int(nullable: false));
            AlterColumn("dbo.MstClearDyedProductMappings", "DyedProductId", c => c.Int(nullable: false));
            AlterColumn("dbo.MstClearDyedProductMappings", "ClearExternalProductId", c => c.Int(nullable: false));
            AlterColumn("dbo.MstClearDyedProductMappings", "DyedExternalProductId", c => c.Int(nullable: false));
            AlterColumn("dbo.MstLookupTypes", "LookupId", c => c.Int(nullable: false));
            AlterColumn("dbo.MstOPISProducts", "PricingSourceId", c => c.Int(nullable: false));
            AlterColumn("dbo.MstOPISProducts", "MstProductId", c => c.Int());
            AlterColumn("dbo.MstOPISProducts", "TfxProductId", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MstOPISProducts", "TfxProductId", c => c.Int());
            AlterColumn("dbo.MstOPISProducts", "MstProductId", c => c.Int());
            AlterColumn("dbo.MstOPISProducts", "PricingSourceId", c => c.Int(nullable: false));
            AlterColumn("dbo.MstLookupTypes", "LookupId", c => c.Int(nullable: false));
            AlterColumn("dbo.MstClearDyedProductMappings", "DyedExternalProductId", c => c.Int(nullable: false));
            AlterColumn("dbo.MstClearDyedProductMappings", "ClearExternalProductId", c => c.Int(nullable: false));
            AlterColumn("dbo.MstClearDyedProductMappings", "DyedProductId", c => c.Int(nullable: false));
            AlterColumn("dbo.MstClearDyedProductMappings", "ClearProductId", c => c.Int(nullable: false));
            AlterColumn("dbo.ExternalPricingPlatts", "ProductId", c => c.Int(nullable: false));
            AlterColumn("dbo.ExternalPricingPlatts", "TerminalId", c => c.Int(nullable: false));
            AlterColumn("dbo.ExternalPricingOpis", "ProductId", c => c.Int(nullable: false));
            AlterColumn("dbo.ExternalPricingOpis", "TerminalId", c => c.Int(nullable: false));
            AlterColumn("dbo.MstTfxProducts", "ProductTypeId", c => c.Int(nullable: false));
            AlterColumn("dbo.MstProducts", "TfxProductId", c => c.Int());
            AlterColumn("dbo.MstProducts", "PricingSourceId", c => c.Int(nullable: false));
            AlterColumn("dbo.MstProducts", "ProductTypeId", c => c.Int(nullable: false));
            AlterColumn("dbo.MstStates", "CountryId", c => c.Int(nullable: false));
            AlterColumn("dbo.MstExternalTerminals", "PricingSourceId", c => c.Int(nullable: false));
            AlterColumn("dbo.MstExternalTerminals", "StateId", c => c.Int(nullable: false));
            AlterColumn("dbo.MstProductMappings", "ProductId", c => c.Int(nullable: false));
            AlterColumn("dbo.MstProductMappings", "ExternalProductId", c => c.Int(nullable: false));
            AlterColumn("dbo.MstProductMappings", "ExternalTerminalId", c => c.Int(nullable: false));
            AlterColumn("dbo.ExternalPricingAxxis", "ProductId", c => c.Int(nullable: false));
            AlterColumn("dbo.ExternalPricingAxxis", "TerminalId", c => c.Int(nullable: false));
            AlterColumn("dbo.MstPricingCodes", "PricingSourceId", c => c.Int(nullable: false));
            AlterColumn("dbo.PricingDetails", "PricingCodeId", c => c.Int(nullable: false));
            AlterColumn("dbo.PricingDetails", "RequestPriceDetailId", c => c.Int(nullable: false));
            AlterColumn("dbo.CumulationDetails", "RequestPriceDetailId", c => c.Int(nullable: false));
            DropColumn("dbo.PricingDetails", "ParameterJson");
        }
    }
}
