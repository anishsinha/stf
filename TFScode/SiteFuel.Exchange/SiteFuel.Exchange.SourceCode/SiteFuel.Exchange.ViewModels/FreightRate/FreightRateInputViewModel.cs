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
    public class FreightRateInputViewModel
    {
        public FreightRateInputViewModel()
        {
            FreightRateRuleType = 1;
            TableType = 1;
            SourceRegionIds = new List<int>();
        }

        public int FreightRateRuleType { get; set; } 

        public int TableType { get; set; }

        public int? OrderId { get; set; }

        public int SupplierId { get; set; }
        public int? FuelTypeId { get; set; }

        public int? CustomerId { get; set; }

        public int? TerminalId { get; set; }

        public int? BulkPlantId { get; set; }

        public List<int> SourceRegionIds { get; set; }

        public List<int> SelectedTerminals { get; set; }

        public List<int> SelectedBulkPlants { get; set; }
    }
}
