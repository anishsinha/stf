namespace SiteFuel.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SFX_2_24 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MstProductTypes", "ProductCode", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MstProductTypes", "ProductCode");
        }
    }
}
