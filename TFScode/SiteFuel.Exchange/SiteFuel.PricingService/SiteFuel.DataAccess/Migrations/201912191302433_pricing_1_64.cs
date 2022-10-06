namespace SiteFuel.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pricing_1_64 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MstTfxProducts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                        ProductDisplayGroupId = c.Int(nullable: false),
                        ProductCode = c.String(maxLength: 32),
                        IsActive = c.Boolean(nullable: false),
                        UpdatedBy = c.Int(nullable: false),
                        UpdatedDate = c.DateTimeOffset(nullable: false, precision: 7),
                        ProductTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MstProductTypes", t => t.ProductTypeId, cascadeDelete: true)
                .Index(t => t.ProductTypeId);
            
            AddColumn("dbo.MstProducts", "TfxProductId", c => c.Int());
            AddColumn("dbo.MstProducts", "DisplayName", c => c.String(maxLength: 256));
            CreateIndex("dbo.MstProducts", "TfxProductId");
            AddForeignKey("dbo.MstProducts", "TfxProductId", "dbo.MstTfxProducts", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MstTfxProducts", "ProductTypeId", "dbo.MstProductTypes");
            DropForeignKey("dbo.MstProducts", "TfxProductId", "dbo.MstTfxProducts");
            DropIndex("dbo.MstTfxProducts", new[] { "ProductTypeId" });
            DropIndex("dbo.MstProducts", new[] { "TfxProductId" });
            DropColumn("dbo.MstProducts", "DisplayName");
            DropColumn("dbo.MstProducts", "TfxProductId");
            DropTable("dbo.MstTfxProducts");
        }
    }
}
