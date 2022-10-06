using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class FuelGroupViewModel : StatusViewModel
    {
        public int Id { get; set; }

        public string GroupName { get; set; }

        public int? AssignedCompanyId { get; set; }

        public FreightTableStatus FreightTableStatus { get; set; } = FreightTableStatus.Unknown;

        public FuelGroupType FuelGroupType { get; set; } = FuelGroupType.Unknown;

        public TableTypes TableType { get; set; }

        public List<int> FuelTypeIds { get; set; } = new List<int>();

        public List<int> ProductTypeIds { get; set; } = new List<int>();
    }
}
