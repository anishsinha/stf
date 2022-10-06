namespace TrueFill.DemandCaptureDataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TFX_1_78 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Sales",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SiteId = c.String(maxLength: 32),
                        TankId = c.String(maxLength: 32),
                        StorageId = c.String(maxLength: 32),
                        StartUllage = c.Single(nullable: false),
                        EndUllage = c.Single(nullable: false),
                        NetVolume = c.Single(nullable: false),
                        UoM = c.Int(nullable: false),
                        CreatedDate = c.DateTimeOffset(nullable: false, precision: 7),
                        StartUllageDate = c.DateTimeOffset(nullable: false, precision: 7),
                        EndUllageDate = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => new { t.SiteId, t.TankId, t.StorageId }, name: "IX_Sales_SiteId_TankId_StorageId")
                .Index(t => t.CreatedDate, name: "IX_Sales_CreatedDate");
            
            CreateTable(
                "dbo.TankDrops",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SiteId = c.String(maxLength: 32),
                        TankId = c.String(maxLength: 32),
                        StorageId = c.String(maxLength: 32),
                        AssetDropId = c.Int(nullable: false),
                        DroppedQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StartTime = c.DateTimeOffset(nullable: false, precision: 7),
                        EndTime = c.DateTimeOffset(nullable: false, precision: 7),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => new { t.SiteId, t.TankId, t.StorageId }, name: "IX_TankDrop_SiteId_TankId_StorageId")
                .Index(t => t.AssetDropId, name: "IX_TankDrop_AssetDropId");
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.TankDrops", "IX_TankDrop_AssetDropId");
            DropIndex("dbo.TankDrops", "IX_TankDrop_SiteId_TankId_StorageId");
            DropIndex("dbo.Sales", "IX_Sales_CreatedDate");
            DropIndex("dbo.Sales", "IX_Sales_SiteId_TankId_StorageId");
            DropTable("dbo.TankDrops");
            DropTable("dbo.Sales");
        }
    }
}
