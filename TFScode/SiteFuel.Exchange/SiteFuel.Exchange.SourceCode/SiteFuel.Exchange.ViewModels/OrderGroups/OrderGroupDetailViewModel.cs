using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class OrderGroupDetailViewModel
    {
        public int GroupId { get; set; }
        public int OrderId { get; set; }
        public string TfxPoNumber { get; set; }
        public string FuelType { get; set; }
        public string DisplayPrice { get; set; }
        public decimal BlendPercentage { get; set; }
        public string Quantity { get; set; }
    }
}
