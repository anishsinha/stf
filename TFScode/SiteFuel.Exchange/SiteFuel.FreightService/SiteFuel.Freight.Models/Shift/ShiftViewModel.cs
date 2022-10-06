using SiteFuel.Exchange.Utilities;
using System;

namespace SiteFuel.FreightModels
{
    public class ShiftViewModel : CommonFieldsModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int CompanyId { get; set; }
        public string RegionId { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }

        public string RegionName { get; set; }
       
    }
}