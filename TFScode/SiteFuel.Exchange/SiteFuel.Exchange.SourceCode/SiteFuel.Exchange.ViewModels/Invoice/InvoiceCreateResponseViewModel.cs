using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class InvoiceCreateResponseViewModel : StatusViewModel
    {
        public int JobId { get; set; }
        public int OrderId { get; set; }
        public string PoNumber { get; set; }
        public int InvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        public int InvoiceTypeId { get; set; }
        public int? SupplierPrefferedInvoiceTypeId { get; set; }
        public int BuyerCompanyId { get; set; }
        public int SupplierCompanyId { get; set; }
        public DateTimeOffset DropStartDate { get; set; }
        public DateTimeOffset DropEndDate { get; set; }
        public decimal OrderTotalDelivered { get; set; }
        public bool IsOrderAutoClosed { get; set; }
        public int? DriverId { get; set; }
        public int OrderAcceptedBy { get; set; }
        public WaitingAction WaitingFor { get; set; }
        public decimal DropPercentPerDelivery { get; set; }
        public int UserId { get; set; }
        public bool IsDtnUploaded { get; set; }
        public int? SplitLoadSequence { get; set; }
        public int? BolDetailId { get; set; }
        public string SplitLoadChainId { get; set; }
        public string OriginalInvoiceNumber { get; set; }
        public int InvoiceHeaderId { get; set; }
    }
}
