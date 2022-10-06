using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.DataAccess.Entities
{
    public class GeneratedException
    {
        public int Id { get; set; }
        public int? TypeId { get; set; }
        public int? InvoiceId { get; set; }
        public int ExceptionTypeId { get; set; }
        public int OwnerCompanyId { get; set; }
        public int ApproverCompanyId { get; set; }
        public int BuyerCompanyId { get; set; }
        public int SupplierCompanyId { get; set; }
        public int StatusId { get; set; }
        public int? ResolutionTypeId { get; set; }
        public DateTimeOffset GeneratedOn { get; set; }
        public DateTimeOffset? ResolvedOn { get; set; }
        public DateTimeOffset? AutoApprovedOn { get; set; }
        public int? ExceptionDetailId { get; set; }
        [ForeignKey("ExceptionTypeId")]
        public virtual ExceptionType ExceptionType { get; set; }
        [ForeignKey("ExceptionDetailId")]
        public virtual GeneratedExceptionDetail GeneratedExceptionDetail { get; set; }
    }
}
