namespace TrueFill.DemandCaptureDataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TFX_1_89 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Sales", "IX_Sales_SiteId_TankId_StorageId");
            DropIndex("dbo.TankDrops", "IX_TankDrop_SiteId_TankId_StorageId");
            AlterColumn("dbo.SaleTanks", "SiteId", c => c.String(nullable: false, maxLength: 64));
            AlterColumn("dbo.SaleTanks", "TankId", c => c.String(nullable: false, maxLength: 64));
            AlterColumn("dbo.SaleTanks", "StorageId", c => c.String(nullable: false, maxLength: 64));
            AlterColumn("dbo.Sales", "SiteId", c => c.String(maxLength: 64));
            AlterColumn("dbo.Sales", "TankId", c => c.String(maxLength: 64));
            AlterColumn("dbo.Sales", "StorageId", c => c.String(maxLength: 64));
            AlterColumn("dbo.TankDrops", "SiteId", c => c.String(maxLength: 64));
            AlterColumn("dbo.TankDrops", "TankId", c => c.String(maxLength: 64));
            AlterColumn("dbo.TankDrops", "StorageId", c => c.String(maxLength: 64));
            CreateIndex("dbo.Sales", new[] { "SiteId", "TankId", "StorageId" }, name: "IX_Sales_SiteId_TankId_StorageId");
            CreateIndex("dbo.TankDrops", new[] { "SiteId", "TankId", "StorageId" }, name: "IX_TankDrop_SiteId_TankId_StorageId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.TankDrops", "IX_TankDrop_SiteId_TankId_StorageId");
            DropIndex("dbo.Sales", "IX_Sales_SiteId_TankId_StorageId");
            AlterColumn("dbo.TankDrops", "StorageId", c => c.String(maxLength: 32));
            AlterColumn("dbo.TankDrops", "TankId", c => c.String(maxLength: 32));
            AlterColumn("dbo.TankDrops", "SiteId", c => c.String(maxLength: 32));
            AlterColumn("dbo.Sales", "StorageId", c => c.String(maxLength: 32));
            AlterColumn("dbo.Sales", "TankId", c => c.String(maxLength: 32));
            AlterColumn("dbo.Sales", "SiteId", c => c.String(maxLength: 32));
            AlterColumn("dbo.SaleTanks", "StorageId", c => c.String(nullable: false, maxLength: 32));
            AlterColumn("dbo.SaleTanks", "TankId", c => c.String(nullable: false, maxLength: 32));
            AlterColumn("dbo.SaleTanks", "SiteId", c => c.String(nullable: false, maxLength: 32));
            CreateIndex("dbo.TankDrops", new[] { "SiteId", "TankId", "StorageId" }, name: "IX_TankDrop_SiteId_TankId_StorageId");
            CreateIndex("dbo.Sales", new[] { "SiteId", "TankId", "StorageId" }, name: "IX_Sales_SiteId_TankId_StorageId");
        }
    }
}
