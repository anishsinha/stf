using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class DdtWatingActionDetailModel
    {
        public int DdtId { get; set; }
        public int RequestPriceDetailId { get; set; }
        public string ProductCode { get; set; }
        public int ProductTypeId { get; set; }
        public int CreatedBy { get; set; }
        public int AcceptedCompanyId { get; set; }
        public int JobId { get; set; }
        public string BuyerCompanyName { get; set; }
        public string SupplierCompanyName { get; set; }
        public int BuyerCompanyId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayInvoiceNumber { get; set; }
        public int InvoiceHeaderId { get; set; }
        public string TimeZoneName { get; set; }
        public int OrderId { get; set; }
    }
}
