using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class StatementInvoiceDetails
    {
        public int InvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        public string InvoiceStatus { get; set; }
        public string DropDate { get; set; }
        public string Customer { get; set; }
        public string PoNumber { get; set; }
        public string TotalQuantityDropped { get; set; }
        public decimal TotalAmount { get; set; }
        public string FuelType { get; set; }
        public decimal Ppg { get; set; }
        public InvoiceStatus StatusId { get; set; }
    }
}
