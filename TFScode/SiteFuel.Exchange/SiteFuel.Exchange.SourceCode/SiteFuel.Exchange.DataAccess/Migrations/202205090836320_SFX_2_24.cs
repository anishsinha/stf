namespace SiteFuel.Exchange.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SFX_2_24 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LoadOptimizationUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(nullable: false),
                        DistributedUsers = c.String(),
                        CreatedDate = c.DateTimeOffset(nullable: false, precision: 7),
                        CreatedBy = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.MstProductTypes", "ProductCode", c => c.String(maxLength: 256));
            AddColumn("dbo.OnboardingPreferences", "IsLoadOptimization", c => c.Boolean(nullable: false));
            AddColumn("dbo.OrderDetailVersions", "EditPropertyType", c => c.Int());
            AddColumn("dbo.OrderDetailVersions", "JsonOrderHistory", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderDetailVersions", "JsonOrderHistory");
            DropColumn("dbo.OrderDetailVersions", "EditPropertyType");
            DropColumn("dbo.OnboardingPreferences", "IsLoadOptimization");
            DropColumn("dbo.MstProductTypes", "ProductCode");
            DropTable("dbo.LoadOptimizationUsers");
        }
    }
}
