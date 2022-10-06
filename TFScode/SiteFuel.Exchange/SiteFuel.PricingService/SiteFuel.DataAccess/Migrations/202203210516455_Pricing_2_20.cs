namespace SiteFuel.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pricing_2_20 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MstExternalTerminals", "TerminalOwner", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MstExternalTerminals", "TerminalOwner");
        }
    }
}
