namespace SiteFuel.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pricing_1_83 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MstCountries", "UoD", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MstCountries", "UoD");
        }
    }
}
