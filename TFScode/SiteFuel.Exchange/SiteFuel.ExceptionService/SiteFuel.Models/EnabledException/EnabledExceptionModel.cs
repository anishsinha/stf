using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Models.EnabledException
{
    public class EnabledExceptionModel
    {
        public int Id { get; set; }
        public int OwnerCompanyId { get; set; }
        public int ExceptionTypeId { get; set; }
        public string ExceptionTypeName { get; set; }
        public int ExceptionApproverId { get; set; }
        public int ExceptionApproverName { get; set; }
        public bool IsAutoApprovalEnabled { get; set; }
        public int? AutoApprovalDays { get; set; }
        public decimal? Threshold { get; set; }
        public int CreatedBy { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public DateTimeOffset UpdatedOn { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
