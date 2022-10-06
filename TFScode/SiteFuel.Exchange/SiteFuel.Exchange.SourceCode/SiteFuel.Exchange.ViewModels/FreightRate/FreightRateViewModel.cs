using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class FreightRateViewModel : StatusViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public FreightTableStatus Status { get; set; } = FreightTableStatus.Unknown;

        public FreightRateRuleType FreightRateRuleType { get; set; } = FreightRateRuleType.Unknown;

        public FuelGroupType FuelGroupType { get; set; } = FuelGroupType.Unknown;

        public int MixLoadMinValue { get; set; }

        public FreightRateCalcPreferenceType FreightRateCalcPreferenceType { get; set; } = FreightRateCalcPreferenceType.Unknown;
        
        public int? FreightRateCalcPrefFuelGroupId { get; set; }

        public DateTimeOffset StartDate { get; set; } = DateTimeOffset.Now;

        public DateTimeOffset? EndDate { get; set; }

        public TableTypes TableType { get; set; }

        public List<int> CustomerIds { get; set; } = new List<int>();

        public List<int> CarrierIds { get; set; } = new List<int>();

        public List<int> SourceRegionIds { get; set; } = new List<int>();

        public List<int> JobIds { get; set; } = new List<int>();

        public List<int> FuelGroupIds { get; set; } = new List<int>();

        public List<DropdownDisplayExtended> TerminalsAndBulkPlants { get; set; } = new List<DropdownDisplayExtended>();

        public List<FreightRateFuelGroupViewModel> FreightRateFuelGroups { get; set; } = new List<FreightRateFuelGroupViewModel>();

        public List<FreightRateRouteTableViewModel> FreightRateRouteTables { get; set; } = new List<FreightRateRouteTableViewModel>();

        public List<FreightRateRangeTableViewModel> FreightRateRangeTables { get; set; } = new List<FreightRateRangeTableViewModel>();

        public List<FreightRatePtoPTableViewModel> FreightRatePtoPTables { get; set; } = new List<FreightRatePtoPTableViewModel>();
    }
}
