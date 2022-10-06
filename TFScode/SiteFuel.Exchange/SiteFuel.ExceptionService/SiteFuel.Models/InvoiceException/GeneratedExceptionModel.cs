using SiteFuel.Models.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Models.InvoiceException
{
    public class GeneratedExceptionModel
    {
        public int Id { get; set; }
        public int? InvoiceId { get; set; }
        public int ExceptionTypeId { get; set; }
        public int OwnerCompanyId { get; set; }
        public int ApproverCompanyId { get; set; }
        public int BuyerCompanyId { get; set; }
        public int SupplierCompanyId { get; set; }
        public ExceptionStatus Status { get; set; }
        public int? ResolutionTypeId { get; set; }
        public DateTimeOffset GeneratedOn { get; set; }
        public DateTimeOffset? ResolvedOn { get; set; }
        public DateTimeOffset? AutoApprovedOn { get; set; }
        public GeneratedExceptionDetailModel GeneratedExceptionDetail { get; set; }
    }
}
