using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiteFuel.Exchange.Core;

namespace SiteFuel.Exchange.ViewModels
{
    public class EnabledExceptionViewModel
    {
        public int ExceptionTypeId { get; set; }
        public int OwnerCompanyId { get; set; }
        public int ApproverCompanyId { get; set; }
        public decimal Threshold { get; set; }
        public int? AutoApprovalDays { get; set; }
    }
}
