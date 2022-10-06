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
    public class FreightCostInputViewModel
    {
        public int FreightRateRuleId { get; set; }

        public int OrderId { get; set; }

        public int? TerminalId { get; set; }

        public int? BulkPlantId { get; set; }

        public int SupplierId { get; set; }
        
        public decimal DeliveredQuantity { get; set; }

        public decimal Distance { get; set; }
    }
}
