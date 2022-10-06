namespace TrueFill.DemandCaptureDataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TFX_2_02 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.DailySales", "IX_Sales_SiteId_TankId_StorageId");
            DropIndex("dbo.Sales", "IX_Sales_SiteId_TankId_StorageId");
            DropIndex("dbo.TankDrops", "IX_TankDrop_SiteId_TankId_StorageId");
            AlterColumn("dbo.DailySales", "SiteId", c => c.String(maxLength: 256));
            AlterColumn("dbo.Demands", "SiteId", c => c.String(maxLength: 256));
            AlterColumn("dbo.SaleTanks", "SiteId", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.Sales", "SiteId", c => c.String(maxLength: 256));
            AlterColumn("dbo.TankDrops", "SiteId", c => c.String(maxLength: 256));
            CreateIndex("dbo.DailySales", new[] { "SiteId", "TankId", "StorageId" }, name: "IX_Sales_SiteId_TankId_StorageId");
            CreateIndex("dbo.Sales", new[] { "SiteId", "TankId", "StorageId" }, name: "IX_Sales_SiteId_TankId_StorageId");
            CreateIndex("dbo.TankDrops", new[] { "SiteId", "TankId", "StorageId" }, name: "IX_TankDrop_SiteId_TankId_StorageId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.TankDrops", "IX_TankDrop_SiteId_TankId_StorageId");
            DropIndex("dbo.Sales", "IX_Sales_SiteId_TankId_StorageId");
            DropIndex("dbo.DailySales", "IX_Sales_SiteId_TankId_StorageId");
            AlterColumn("dbo.TankDrops", "SiteId", c => c.String(maxLength: 64));
            AlterColumn("dbo.Sales", "SiteId", c => c.String(maxLength: 64));
            AlterColumn("dbo.SaleTanks", "SiteId", c => c.String(nullable: false, maxLength: 64));
            AlterColumn("dbo.Demands", "SiteId", c => c.String(maxLength: 64));
            AlterColumn("dbo.DailySales", "SiteId", c => c.String(maxLength: 64));
            CreateIndex("dbo.TankDrops", new[] { "SiteId", "TankId", "StorageId" }, name: "IX_TankDrop_SiteId_TankId_StorageId");
            CreateIndex("dbo.Sales", new[] { "SiteId", "TankId", "StorageId" }, name: "IX_Sales_SiteId_TankId_StorageId");
            CreateIndex("dbo.DailySales", new[] { "SiteId", "TankId", "StorageId" }, name: "IX_Sales_SiteId_TankId_StorageId");
        }
    }
}
