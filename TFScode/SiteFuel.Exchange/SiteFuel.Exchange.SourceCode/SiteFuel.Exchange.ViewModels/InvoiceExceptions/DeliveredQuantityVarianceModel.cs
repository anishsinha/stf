using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.InvoiceExceptions
{
    public class DeliveredQuantityVarianceModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Customer { get; set; }
        public string Vendor { get; set; }
        public string PoNumber { get; set; }
        public string InvoiceNumber { get; set; }
        public string JobName { get; set; }
        public string DropDate { get; set; }
        public string DropTime { get; set; }
        public decimal OrderedQuantity { get; set; }
        public decimal BolQuantity { get; set; }
        public decimal DeliveredQuantity { get; set; }
        public decimal Tolerance { get; set; }
        public decimal Varience { get; set; }
        public decimal PricePerGallon { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public string AutoApprove { get; set; }
        public string ApprovedDate { get; set; }

        public string DriverName { get; set; }
        public string CarrierName { get; set; }
        public string ResolvedOn { get; set; }
        public string UOM { get; set; }
    }
}
