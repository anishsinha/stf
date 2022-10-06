namespace SiteFuel.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pricing_2_13 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MstProducts", "CompanyId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MstProducts", "CompanyId");
        }
    }
}
