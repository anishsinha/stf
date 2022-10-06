using SiteFuel.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Models.CompanyException
{
    public class CompanyExceptionModel
    {
        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public int? ApproverId { get; set; }
        public int? AutoApprovalDays { get; set; }
        public int? DelayInvoiceCreationTime { get; set; }
        public decimal? Threshold { get; set; }
        public bool IsActive { get; set; }
        public List<ListItem> Approvers { get; set; }
        public List<ListItem> Resolutions { get; set; }
    }
}
