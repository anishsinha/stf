using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class InvoiceEditResponseViewModel : StatusViewModel
    {
        public int JobId { get; set; }
        public int OrderId { get; set; }
        public int InvoiceId { get; set; }
        public int DealCreatedInvoiceId { get; set; }
        public int InvoiceTypeId { get; set; }
        public int BuyerCompanyId { get; set; }
        public int SupplierCompanyId { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTimeOffset DropStartDate { get; set; }
        public DateTimeOffset DropEndDate { get; set; }
        public decimal OrderTotalDelivered { get; set; }
        public bool IsOrderAutoClosed { get; set; }
        public bool IsDtnUploaded { get; set; }
        public int? SplitLoadSequence { get; set; }
        public string SplitLoadChainId { get; set; }
        public int JobCompanyId { get; set; }
        public int InvoiceHeaderId { get; set; }
        public string TimeZoneName { get; set; }
        public int InvoiceNumberId { get; set; }
    }
}
