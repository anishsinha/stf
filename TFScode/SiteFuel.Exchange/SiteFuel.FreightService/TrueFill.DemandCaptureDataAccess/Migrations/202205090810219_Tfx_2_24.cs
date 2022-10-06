namespace TrueFill.DemandCaptureDataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tfx_2_24 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Demands", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Demands", "IsActive");
        }
    }
}
