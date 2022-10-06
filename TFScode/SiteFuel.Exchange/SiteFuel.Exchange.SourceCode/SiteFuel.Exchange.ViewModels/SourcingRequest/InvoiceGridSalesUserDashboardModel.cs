using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class InvoiceGridSalesUserDashboardModel
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; }
        public string SourcingRequest { get; set; }
        public string PoNumber { get; set; }
        public string DropDate { get; set; }
        public string Status { get; set; }
    }
}
