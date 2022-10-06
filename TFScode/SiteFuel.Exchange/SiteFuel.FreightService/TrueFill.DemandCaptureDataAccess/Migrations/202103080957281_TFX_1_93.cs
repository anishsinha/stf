namespace TrueFill.DemandCaptureDataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TFX_1_93 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DailySales",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SiteId = c.String(maxLength: 64),
                        TankId = c.String(maxLength: 64),
                        StorageId = c.String(maxLength: 64),
                        StartUllage = c.Single(nullable: false),
                        EndUllage = c.Single(nullable: false),
                        NetVolume = c.Single(nullable: false),
                        UoM = c.Int(nullable: false),
                        DroppedQuantity = c.Decimal(nullable: false, precision: 18, scale: 8),
                        CreatedDate = c.DateTime(nullable: false),
                        StartUllageDate = c.DateTime(nullable: false),
                        EndUllageDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => new { t.SiteId, t.TankId, t.StorageId }, name: "IX_Sales_SiteId_TankId_StorageId")
                .Index(t => t.CreatedDate, name: "IX_Sales_CreatedDate");
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.DailySales", "IX_Sales_CreatedDate");
            DropIndex("dbo.DailySales", "IX_Sales_SiteId_TankId_StorageId");
            DropTable("dbo.DailySales");
        }
    }
}
