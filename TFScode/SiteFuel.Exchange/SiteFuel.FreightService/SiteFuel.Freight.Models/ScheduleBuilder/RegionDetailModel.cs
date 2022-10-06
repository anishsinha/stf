using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels.ScheduleBuilder
{
    public class RegionDetailModel : CommonFieldsModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<DriverAdditionalDetailsViewModel> Drivers { get; set; } = new List<DriverAdditionalDetailsViewModel>();
        public List<TrailerModel> Trailers { get; set; } = new List<TrailerModel>();
        public int ShiftSlotTime { get; set; }
        public int CreatedByCompanyId { get; set; }
    } 
}
