using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class SystemOrderAutoClosedNewsfeedViewModel
    {
        public int OrderId { get; set; }
        public string PoNumber { get; set; }
        public int BuyerCompanyId { get; set; }
        public int SupplierCompanyId { get; set; }
        public string TimeZoneName { get; set; }
        public decimal TotalDelivered { get; set; }
        public int JobId { get; set; }
        public int JobCompanyId { get; set; }
        public UoM UoM { get; set; }
    }
}
