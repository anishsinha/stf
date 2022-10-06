namespace TrueFill.DemandCaptureDataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TFX_1_96 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SourceFiles", "DataSourceType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SourceFiles", "DataSourceType");
        }
    }
}
