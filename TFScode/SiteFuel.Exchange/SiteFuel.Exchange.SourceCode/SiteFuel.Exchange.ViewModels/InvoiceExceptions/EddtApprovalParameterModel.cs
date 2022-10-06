using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.InvoiceExceptions
{
    public class EddtApprovalParameterModel
    {
        public int InvoiceId { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public string SplitLoadChainId { get; set; }
        public decimal ApprovedQuantity { get; set; }
        public int ExceptionTypeId { get; set; }
    }
}
