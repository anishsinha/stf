using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Models.InvoiceException
{
    public class UspMyApprovalException
    {
        public string BuyerCompanyName { get; set; }
        public string PoNumber { get; set; }
        public string InvoiceNumber { get; set; }
        public string JobName { get; set; }
        public string DropDate { get; set; }
        public decimal OrderedQuantity { get; set; }
        public decimal BolQuantity { get; set; }
        public decimal DeliveredQuantity { get; set; }
        public decimal Tolerance { get; set; }
        public decimal Varience { get; set; }
        public int StatusId { get; set; }
        public string AutoApprove { get; set; }
    }
}
