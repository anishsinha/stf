namespace SiteFuel.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tfx_1_78 : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.CustomerExceptions", "CustomerCompanyId", "EnabledForCompanyId");
            AddColumn("dbo.CompanyExceptions", "DelayInvoiceCreationTime", c => c.Int());
        }
        
        public override void Down()
        {
            RenameColumn("dbo.CustomerExceptions", "EnabledForCompanyId", "CustomerCompanyId");
            DropColumn("dbo.CompanyExceptions", "DelayInvoiceCreationTime");
        }
    }
}
