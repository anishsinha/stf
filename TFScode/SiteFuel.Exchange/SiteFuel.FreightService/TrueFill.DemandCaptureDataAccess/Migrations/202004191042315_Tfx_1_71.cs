namespace TrueFill.DemandCaptureDataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tfx_1_71 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Demands", "SiteId", c => c.String(maxLength: 32));
            AlterColumn("dbo.Demands", "TankId", c => c.String(maxLength: 32));
            AlterColumn("dbo.Demands", "StorageId", c => c.String(maxLength: 32));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Demands", "StorageId", c => c.String());
            AlterColumn("dbo.Demands", "TankId", c => c.String());
            AlterColumn("dbo.Demands", "SiteId", c => c.String());
        }
    }
}
