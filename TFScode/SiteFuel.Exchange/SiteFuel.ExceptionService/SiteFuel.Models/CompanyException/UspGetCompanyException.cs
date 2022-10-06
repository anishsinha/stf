using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Models.CompanyException
{
    public class UspGetCompanyException
    {
        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public int? ExceptionApproverId { get; set; }
        public int? AutoApprovalDays { get; set; }
        public int? DelayInvoiceCreationTime { get; set; }
        public decimal? Threshold { get; set; }
        public bool IsActive { get; set; }
        public int ApproverId { get; set; }
        public string ApproverName { get; set; }
        public int ResolutionId { get; set; }
        public string ResolutionName { get; set; }
    }
}
