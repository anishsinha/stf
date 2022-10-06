using SiteFuel.Exchange.Utilities;
using System;

namespace SiteFuel.Exchange.ViewModels
{
    public class UspFuelGroupSummary
    {
        public int Id { get; set; }

        public string GroupName { get; set; }

        public TableTypes TableType { get; set; }

        public FuelGroupType FuelGroupType { get; set; }

        public string Company { get; set; }

        public string ProductType { get; set; }

        public FreightTableStatus StatusId { get; set; }
    }
}
