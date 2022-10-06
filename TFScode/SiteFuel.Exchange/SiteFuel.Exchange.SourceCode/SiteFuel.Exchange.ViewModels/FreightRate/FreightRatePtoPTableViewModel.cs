using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class FreightRatePtoPTableViewModel
    {
        public int? TerminalId { get; set; }

        public int? BulkPlantId { get; set; }

        public int JobId { get; set; }

        public string TerminalAndBulkPlantName { get; set; }

        public string LocationName { get; set; }

        public string LocationAddress { get; set; }

        public string LaneID { get; set; }

        public decimal AssumedMiles { get; set; }

        public bool IsLaneRequired { get; set; }

        public decimal RateValue { get; set; }

        public string FuelGroupName { get; set; }

        public int FuelGroupId { get; set; }
    }
}
