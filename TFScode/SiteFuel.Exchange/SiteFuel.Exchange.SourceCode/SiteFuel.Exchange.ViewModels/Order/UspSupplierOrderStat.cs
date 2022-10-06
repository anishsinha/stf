using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class UspSupplierOrderStat
    {
        public decimal GallonsOrdered { get; set; }

        public decimal GallonsDelivered { get; set; }

        public decimal AvgGallonsPerDelivery { get; set; }

        public decimal AvgPricePerGallon { get; set; }

        public Currency Currency { get; set; }

        public UoM UoM { get; set; }

        public string DisplayCurrency { get { return Currency.ToString(); } }

        public string DisplayUoM { get { return UoM.ToString(); } }
    }
}
