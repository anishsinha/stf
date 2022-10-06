namespace TrueFill.DemandCaptureDataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TFX_1_70 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Demands", "IsProcessed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Demands", "IsProcessed");
        }
    }
}
