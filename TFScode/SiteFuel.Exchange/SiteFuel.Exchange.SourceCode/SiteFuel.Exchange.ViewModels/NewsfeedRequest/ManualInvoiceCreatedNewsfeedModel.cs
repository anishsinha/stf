using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class ManualInvoiceCreatedNewsfeedModel
    {
        public int OrderId { get; set; }
        public int InvoiceTypeId { get; set; }
        public int BuyerCompanyId { get; set; }
        public int SupplierCompanyId { get; set; }
        public int InvoiceId { get; set; }
        public string TimeZoneName { get; set; }
        public string InvoiceNumber { get; set; }
        public int DeliveryTypeId { get; set; }
        public DateTimeOffset OrderCloseDate { get; set; }
        public string PoNumber { get; set; }
        public int JobId { get; set; }
        public decimal DropPercentage { get; set; }
        public WaitingAction WaitingFor { get; set; }
        public string OriginalInvoiceNumber { get; set; }
        public int InvoiceHeaderId { get; set; }
    }
}
