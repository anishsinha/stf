using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class BuyerOrderStat
    {
        public string GallonsOrdered { get; set; }

        public decimal GallonsDelivered { get; set; }

        public string GallonsRemaining { get; set; }

        public decimal AvgGallonsPerDelivery { get; set; }

        public decimal TotalInvoicedAmount { get; set; }

        public decimal AvgPricePerGallon { get; set; }

        public bool IsInvoicesCreated { get; set; }
    }
}
