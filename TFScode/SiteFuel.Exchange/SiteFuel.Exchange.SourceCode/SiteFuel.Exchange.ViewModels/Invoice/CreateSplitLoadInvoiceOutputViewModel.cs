using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class CreateSplitLoadInvoiceOutputViewModel : InvoiceCreateResponseViewModel
    {
        public bool IsApprovalWorkflowEnabledForJob { get; set; }
        public int? ApprovalUserId { get; set; }
        public string ApprovalUserName { get; set; }
        public int DeliveryTypeId { get; set; }
        public int? ApprovalUserOnboardedType { get; set; }
        public decimal DroppedGallons { get; set; }
        public UoM UoM { get; set; }
        public string TimeZoneName { get; set; }
        public bool IsBrokeredOrder { get; set; }
        public int? SupplierPreferredInvoiceTypeId { get; set; }
        public int JobCompanyId { get; set; }
        public string JobCompanyName { get; set; }
        public string SupplierCompanyName { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}
