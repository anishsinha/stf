using Foolproof;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class FreightCostQuantityBasedOutputViewModel
    {
        public decimal StartQuantity { get; set; }

        public decimal? EndQuantity { get; set; }

        public decimal FuelCost { get; set; } 
    }
}
