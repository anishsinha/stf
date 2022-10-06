using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class RegionDetailViewModel
    {
        public string Id { get; set; }
        public List<DriverAdditionalDetailsViewModel> Drivers { get; set; }
        public List<TrailerModel> Trailers { get; set; }
        public int ShiftSlotTime { get; set; }
        public int CreatedByCompanyId { get; set; }
    }
}
