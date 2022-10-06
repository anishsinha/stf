using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels.ScheduleBuilder
{
    public class DrFilterPreferencesModel : FreightModels.StatusModel
    {
        public string Id { get; set; }
        public int UserId { get; set; }
        public int CompanyId { get; set; }
        public string RegionId { get; set; }
        public string FilterData { get; set; }
        public DateTimeOffset Date { get; set; }
    }
}
