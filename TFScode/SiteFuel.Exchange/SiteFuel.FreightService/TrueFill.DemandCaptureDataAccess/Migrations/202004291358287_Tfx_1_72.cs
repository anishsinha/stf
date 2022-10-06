namespace TrueFill.DemandCaptureDataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tfx_1_72 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Demands", "DipTestValue", c => c.Single(nullable: false));
            AddColumn("dbo.Demands", "DipTestUoM", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Demands", "DipTestUoM");
            DropColumn("dbo.Demands", "DipTestValue");
        }
    }
}
