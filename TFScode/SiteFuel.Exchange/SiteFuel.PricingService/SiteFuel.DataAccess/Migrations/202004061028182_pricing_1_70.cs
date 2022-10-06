namespace SiteFuel.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pricing_1_70 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MstOPISProducts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                        PricingSourceId = c.Int(nullable: false),
                        MstProductId = c.Int(),
                        ProductCode = c.String(maxLength: 32),
                        IsActive = c.Boolean(nullable: false),
                        UpdatedBy = c.Int(nullable: false),
                        UpdatedDate = c.DateTimeOffset(nullable: false, precision: 7),
                        TfxProductId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MstPricingSources", t => t.PricingSourceId, cascadeDelete: true)
                .ForeignKey("dbo.MstProducts", t => t.MstProductId)
                .ForeignKey("dbo.MstTfxProducts", t => t.TfxProductId)
                .Index(t => t.PricingSourceId)
                .Index(t => t.MstProductId)
                .Index(t => t.TfxProductId);
            
            AddColumn("dbo.ExternalPricingOpis", "IsActualOPIS", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MstOPISProducts", "TfxProductId", "dbo.MstTfxProducts");
            DropForeignKey("dbo.MstOPISProducts", "MstProductId", "dbo.MstProducts");
            DropForeignKey("dbo.MstOPISProducts", "PricingSourceId", "dbo.MstPricingSources");
            DropIndex("dbo.MstOPISProducts", new[] { "TfxProductId" });
            DropIndex("dbo.MstOPISProducts", new[] { "MstProductId" });
            DropIndex("dbo.MstOPISProducts", new[] { "PricingSourceId" });
            DropColumn("dbo.ExternalPricingOpis", "IsActualOPIS");
            DropTable("dbo.MstOPISProducts");
        }
    }
}
