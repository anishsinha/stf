namespace SiteFuel.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pricing_1_93 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.MstExternalTerminals", "Latitude", c => c.Decimal(nullable: false, precision: 18, scale: 8));
            AlterColumn("dbo.MstExternalTerminals", "Longitude", c => c.Decimal(nullable: false, precision: 18, scale: 8));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MstExternalTerminals", "Longitude", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.MstExternalTerminals", "Latitude", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
