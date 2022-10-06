namespace SiteFuel.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _202004270735366_Tfx_1_76 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GeneratedExceptionDetails", "DriverName", c => c.String());
            AddColumn("dbo.GeneratedExceptionDetails", "CarrierName", c => c.String());
            AddColumn("dbo.GeneratedExceptionDetails", "DriverId", c => c.Int());
            AddColumn("dbo.GeneratedExceptionDetails", "UOM", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.GeneratedExceptionDetails", "UOM");
            DropColumn("dbo.GeneratedExceptionDetails", "DriverId");
            DropColumn("dbo.GeneratedExceptionDetails", "CarrierName");
            DropColumn("dbo.GeneratedExceptionDetails", "DriverName");
        }
    }
}
