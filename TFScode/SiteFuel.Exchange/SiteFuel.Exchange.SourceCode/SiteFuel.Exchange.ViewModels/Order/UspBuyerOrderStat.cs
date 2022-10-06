using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class UspBuyerOrderStat
    {
        public decimal GallonsOrdered { get; set; }

        public decimal GallonsDelivered { get; set; }

        public decimal GallonsRemaining { get; set; }

        public decimal AvgGallonsPerDelivery { get; set; }

        public decimal TotalInvoicedAmount { get; set; }

        public decimal AvgPricePerGallon { get; set; }
    }
}
