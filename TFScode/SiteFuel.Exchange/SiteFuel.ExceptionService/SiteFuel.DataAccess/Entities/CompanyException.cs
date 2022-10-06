using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.DataAccess.Entities
{
    public class CompanyException
    {
        public int Id { get; set; }
        public int OwnerCompanyId { get; set; }
        public int ExceptionTypeId { get; set; }
        public int ExceptionApproverId { get; set; }
        public bool IsAutoApprovalEnabled { get; set; }
        public int? AutoApprovalDays { get; set; }
        public int? DelayInvoiceCreationTime { get; set; }
        public decimal? Threshold { get; set; }
        public int CreatedBy { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public int UpdateBy { get; set; }
        public DateTimeOffset UpdatedOn { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        [ForeignKey("ExceptionTypeId")]
        public virtual ExceptionType ExceptionType { get; set; }
        [ForeignKey("ExceptionApproverId")]
        public virtual ExceptionApprover ExceptionApprover { get; set; }
    }
}
