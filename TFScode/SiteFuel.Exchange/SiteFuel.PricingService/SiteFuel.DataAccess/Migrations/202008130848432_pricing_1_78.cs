namespace SiteFuel.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pricing_1_78 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MstClearDyedProductMappings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClearProductId = c.Int(nullable: false),
                        DyedProductId = c.Int(nullable: false),
                        ClearExternalProductId = c.Int(nullable: false),
                        DyedExternalProductId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        UpdatedBy = c.Int(nullable: false),
                        UpdatedDate = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MstExternalProducts", t => t.DyedExternalProductId, cascadeDelete: true)
                .ForeignKey("dbo.MstProducts", t => t.ClearProductId, cascadeDelete: true)
                .ForeignKey("dbo.MstExternalProducts", t => t.ClearExternalProductId, cascadeDelete: true)
                .ForeignKey("dbo.MstProducts", t => t.DyedProductId, cascadeDelete: true)
                .Index(t => t.ClearProductId)
                .Index(t => t.DyedProductId)
                .Index(t => t.ClearExternalProductId)
                .Index(t => t.DyedExternalProductId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MstClearDyedProductMappings", "DyedProductId", "dbo.MstProducts");
            DropForeignKey("dbo.MstClearDyedProductMappings", "ClearExternalProductId", "dbo.MstExternalProducts");
            DropForeignKey("dbo.MstClearDyedProductMappings", "ClearProductId", "dbo.MstProducts");
            DropForeignKey("dbo.MstClearDyedProductMappings", "DyedExternalProductId", "dbo.MstExternalProducts");
            DropIndex("dbo.MstClearDyedProductMappings", new[] { "DyedExternalProductId" });
            DropIndex("dbo.MstClearDyedProductMappings", new[] { "ClearExternalProductId" });
            DropIndex("dbo.MstClearDyedProductMappings", new[] { "DyedProductId" });
            DropIndex("dbo.MstClearDyedProductMappings", new[] { "ClearProductId" });
            DropTable("dbo.MstClearDyedProductMappings");
        }
    }
}
