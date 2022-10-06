using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
   public class RecurringScheduleGroupInfo
    {
        public string RegionId { get; set; }
        public string Date { get; set; }
        public int CompanyId { get; set; }
        public int UserId { get; set; }
        public string ScheduleBuilderId { get; set; }
        public List<RecurringSchedulesDetails> RecurringSchedulesDetails { get; set; } = new List<RecurringSchedulesDetails>();
    }
}
