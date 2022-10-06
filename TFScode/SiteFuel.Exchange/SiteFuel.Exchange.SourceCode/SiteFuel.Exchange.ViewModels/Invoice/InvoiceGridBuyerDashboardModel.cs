using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class InvoiceGridBuyerDashboardModel
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; }
        public string Supplier { get; set; }
        public string PoNumber { get; set; }
        public string DropDate { get; set; }
        public string DropTime { get; set; }
        public string Status { get; set; }
        public bool IsSupressOrderPricing { get; set; }
    }
}
