using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.DataAccess.Entities
{
    public partial class ExceptionDataContext : DbContext
    {
        public ExceptionDataContext() : base("name=ExceptionDatabaseConnection")
        {
        }
        public ExceptionDataContext(string connectionString) : base(connectionString)
        {
        }

        public virtual DbSet<CustomerException> CustomerExceptions { get; set; }
        public virtual DbSet<CompanyException> CompanyExceptions { get; set; }
        public virtual DbSet<ExceptionApprover> ExceptionApprovers { get; set; }
        public virtual DbSet<ExceptionType> ExceptionTypes { get; set; }
        public virtual DbSet<ExceptionTypeXApprover> ExceptionTypeXApprovers { get; set; }
        public virtual DbSet<GeneratedException> GeneratedExceptions { get; set; }
        public virtual DbSet<GeneratedExceptionDetail> GeneratedExceptionDetails { get; set; }
        public virtual DbSet<ResolutionType> ResolutionTypes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CompanyException>()
                .Property(e => e.Threshold)
                .HasPrecision(18, 8);

            modelBuilder.Entity<GeneratedExceptionDetail>()
                .Property(e => e.OrderedQuantity)
                .HasPrecision(18, 8);

            modelBuilder.Entity<GeneratedExceptionDetail>()
                .Property(e => e.BolQuantity)
                .HasPrecision(18, 8);

            modelBuilder.Entity<GeneratedExceptionDetail>()
                .Property(e => e.DeliveredQuantity)
                .HasPrecision(18, 8);

            modelBuilder.Entity<GeneratedExceptionDetail>()
                .Property(e => e.Tolerance)
                .HasPrecision(18, 8);

            modelBuilder.Entity<GeneratedExceptionDetail>()
                .Property(e => e.Varience)
                .HasPrecision(18, 8);
        }
    }
}
