using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class DigitalDropTicketApprovalNewsfeedModel
    {
        public int InvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        public int OrderId { get; set; }
        public string PoNumber { get; set; }
        public int BuyerCompanyId { get; set; }
        public int SupplierCompanyId { get; set; }
        public int ApprovalUserId { get; set; }
        public string TimeZoneName { get; set; }
        public bool IsBrokeredOrder { get; set; }
        public int SupplierPreferredInvoiceTypeId { get; set; }
        public int CreatedBy { get; set; }
        public int ApprovalUserCompanyId { get; set; }
        public string ApprovalUserCompany { get; set; }
        public int JobId { get; set; }
        public string UserName { get; set; }
        public string SupplierCompanyName { get; set; }
        public string ApprovalUserName { get; set; }
        public int InvoiceTypeId { get; set; }
        public int InvoiceHeaderId { get; set; }
    }
}
