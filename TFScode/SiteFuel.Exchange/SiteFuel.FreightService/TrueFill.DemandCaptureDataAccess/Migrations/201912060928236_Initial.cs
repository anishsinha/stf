namespace TrueFill.DemandCaptureDataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Demands",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SiteId = c.String(),
                        TankId = c.String(),
                        StorageId = c.String(),
                        Level = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Ullage = c.Single(nullable: false),
                        GrossVolume = c.Single(nullable: false),
                        NetVolume = c.Single(nullable: false),
                        WaterNetLevel = c.Single(nullable: false),
                        WaterGrossLevel = c.Single(nullable: false),
                        CaptureTime = c.DateTimeOffset(nullable: false, precision: 7),
                        ProductName = c.String(),
                        DataSourceTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Demands");
        }
    }
}
