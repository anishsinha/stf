namespace SiteFuel.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CompanyExceptions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OwnerCompanyId = c.Int(nullable: false),
                        ExceptionTypeId = c.Int(nullable: false),
                        ExceptionApproverId = c.Int(nullable: false),
                        IsAutoApprovalEnabled = c.Boolean(nullable: false),
                        AutoApprovalDays = c.Int(),
                        Threshold = c.Decimal(precision: 18, scale: 8),
                        CreatedBy = c.Int(nullable: false),
                        CreatedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdateBy = c.Int(nullable: false),
                        UpdatedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ExceptionApprovers", t => t.ExceptionApproverId, cascadeDelete: true)
                .ForeignKey("dbo.ExceptionTypes", t => t.ExceptionTypeId, cascadeDelete: true)
                .Index(t => t.ExceptionTypeId)
                .Index(t => t.ExceptionApproverId);
            
            CreateTable(
                "dbo.ExceptionApprovers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CreatedBy = c.Int(nullable: false),
                        CreatedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ExceptionTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CreatedBy = c.Int(nullable: false),
                        CreatedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CustomerExceptions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ExceptionTypeId = c.Int(nullable: false),
                        OwnerCompanyId = c.Int(nullable: false),
                        CustomerCompanyId = c.Int(nullable: false),
                        ApproverCompanyId = c.Int(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        CreatedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdateBy = c.Int(nullable: false),
                        UpdatedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ExceptionTypes", t => t.ExceptionTypeId, cascadeDelete: true)
                .Index(t => t.ExceptionTypeId);
            
            CreateTable(
                "dbo.ExceptionTypeXApprovers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ExceptionTypeId = c.Int(nullable: false),
                        ApproverId = c.Int(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        CreatedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ExceptionApprovers", t => t.ApproverId, cascadeDelete: true)
                .ForeignKey("dbo.ExceptionTypes", t => t.ExceptionTypeId, cascadeDelete: true)
                .Index(t => t.ExceptionTypeId)
                .Index(t => t.ApproverId);
            
            CreateTable(
                "dbo.GeneratedExceptionDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DropDate = c.DateTimeOffset(nullable: false, precision: 7),
                        BuyerCompanyName = c.String(),
                        SupplierCompanyName = c.String(),
                        PoNumber = c.String(),
                        InvoiceNumber = c.String(),
                        JobName = c.String(),
                        OrderedQuantity = c.Decimal(nullable: false, precision: 18, scale: 8),
                        BolQuantity = c.Decimal(nullable: false, precision: 18, scale: 8),
                        DeliveredQuantity = c.Decimal(nullable: false, precision: 18, scale: 8),
                        Tolerance = c.Decimal(nullable: false, precision: 18, scale: 8),
                        Varience = c.Decimal(nullable: false, precision: 18, scale: 8),
                        ScheduledLocation = c.String(),
                        DroppedLocation = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GeneratedExceptions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeId = c.Int(),
                        InvoiceId = c.Int(),
                        ExceptionTypeId = c.Int(nullable: false),
                        OwnerCompanyId = c.Int(nullable: false),
                        ApproverCompanyId = c.Int(nullable: false),
                        BuyerCompanyId = c.Int(nullable: false),
                        SupplierCompanyId = c.Int(nullable: false),
                        StatusId = c.Int(nullable: false),
                        ResolutionTypeId = c.Int(),
                        GeneratedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        ResolvedOn = c.DateTimeOffset(precision: 7),
                        AutoApprovedOn = c.DateTimeOffset(precision: 7),
                        ExceptionDetailId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ExceptionTypes", t => t.ExceptionTypeId, cascadeDelete: true)
                .ForeignKey("dbo.GeneratedExceptionDetails", t => t.ExceptionDetailId)
                .Index(t => t.ExceptionTypeId)
                .Index(t => t.ExceptionDetailId);
            
            CreateTable(
                "dbo.ResolutionTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ExceptionTypeId = c.Int(nullable: false),
                        Name = c.String(),
                        CreatedBy = c.Int(nullable: false),
                        CreatedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ExceptionTypes", t => t.ExceptionTypeId, cascadeDelete: true)
                .Index(t => t.ExceptionTypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ResolutionTypes", "ExceptionTypeId", "dbo.ExceptionTypes");
            DropForeignKey("dbo.GeneratedExceptions", "ExceptionDetailId", "dbo.GeneratedExceptionDetails");
            DropForeignKey("dbo.GeneratedExceptions", "ExceptionTypeId", "dbo.ExceptionTypes");
            DropForeignKey("dbo.ExceptionTypeXApprovers", "ExceptionTypeId", "dbo.ExceptionTypes");
            DropForeignKey("dbo.ExceptionTypeXApprovers", "ApproverId", "dbo.ExceptionApprovers");
            DropForeignKey("dbo.CustomerExceptions", "ExceptionTypeId", "dbo.ExceptionTypes");
            DropForeignKey("dbo.CompanyExceptions", "ExceptionTypeId", "dbo.ExceptionTypes");
            DropForeignKey("dbo.CompanyExceptions", "ExceptionApproverId", "dbo.ExceptionApprovers");
            DropIndex("dbo.ResolutionTypes", new[] { "ExceptionTypeId" });
            DropIndex("dbo.GeneratedExceptions", new[] { "ExceptionDetailId" });
            DropIndex("dbo.GeneratedExceptions", new[] { "ExceptionTypeId" });
            DropIndex("dbo.ExceptionTypeXApprovers", new[] { "ApproverId" });
            DropIndex("dbo.ExceptionTypeXApprovers", new[] { "ExceptionTypeId" });
            DropIndex("dbo.CustomerExceptions", new[] { "ExceptionTypeId" });
            DropIndex("dbo.CompanyExceptions", new[] { "ExceptionApproverId" });
            DropIndex("dbo.CompanyExceptions", new[] { "ExceptionTypeId" });
            DropTable("dbo.ResolutionTypes");
            DropTable("dbo.GeneratedExceptions");
            DropTable("dbo.GeneratedExceptionDetails");
            DropTable("dbo.ExceptionTypeXApprovers");
            DropTable("dbo.CustomerExceptions");
            DropTable("dbo.ExceptionTypes");
            DropTable("dbo.ExceptionApprovers");
            DropTable("dbo.CompanyExceptions");
        }
    }
}
