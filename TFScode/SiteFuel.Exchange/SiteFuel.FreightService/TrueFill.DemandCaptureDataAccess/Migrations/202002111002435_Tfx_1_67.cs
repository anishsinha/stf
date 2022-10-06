namespace TrueFill.DemandCaptureDataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tfx_1_67 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SourceFiles",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FileName = c.String(),
                        Uid = c.Long(nullable: false),
                        CreationDate = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Demands", "SupplierId", c => c.Int(nullable: false));
            AddColumn("dbo.Demands", "SourceFileId", c => c.Long());
            AlterColumn("dbo.Demands", "CaptureTime", c => c.DateTime(nullable: false));
            CreateIndex("dbo.Demands", "SourceFileId");
            AddForeignKey("dbo.Demands", "SourceFileId", "dbo.SourceFiles", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Demands", "SourceFileId", "dbo.SourceFiles");
            DropIndex("dbo.Demands", new[] { "SourceFileId" });
            AlterColumn("dbo.Demands", "CaptureTime", c => c.DateTimeOffset(nullable: false, precision: 7));
            DropColumn("dbo.Demands", "SourceFileId");
            DropColumn("dbo.Demands", "SupplierId");
            DropTable("dbo.SourceFiles");
        }
    }
}
