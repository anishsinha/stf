namespace SiteFuel.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tfx_exception_164 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GeneratedExceptionDetails", "PricePerGallon", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.GeneratedExceptionDetails", "UserName", c => c.String());
            AddColumn("dbo.GeneratedExceptionDetails", "ParameterJson", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.GeneratedExceptionDetails", "ParameterJson");
            DropColumn("dbo.GeneratedExceptionDetails", "UserName");
            DropColumn("dbo.GeneratedExceptionDetails", "PricePerGallon");
        }
    }
}
