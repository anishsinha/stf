using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class AccessorialFeeViewModel : StatusViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Version { get; set; }

        public FreightTableStatus Status { get; set; } = FreightTableStatus.Unknown;

        public DateTimeOffset StartDate { get; set; } = DateTimeOffset.Now;

        public DateTimeOffset? EndDate { get; set; }

        public TableTypes TableType { get; set; }

        public List<int> CustomerIds { get; set; } = new List<int>();

        public List<int> CarrierIds { get; set; } = new List<int>();

        public List<int> SourceRegionIds { get; set; } = new List<int>();

        public List<DropdownDisplayExtended> TerminalsAndBulkPlants { get; set; } = new List<DropdownDisplayExtended>();

        public List<FeesViewModel> Fees { get; set; } = new List<FeesViewModel>();

        public Country CountryId { get; set; }
    }
}
