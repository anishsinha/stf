using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class TimeCardInputRequestViewModel
    {
        public int UserId { get; set; }
        public int ActionId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public long TimeStamp { get; set; }
        public string UserTimeZone { get; set; }
    }

    public class TimeCardOutputRequestViewModel : StatusViewModel
    {
        public TimeCardOutputRequestViewModel()
        {
            //IsTimeCardEnabled = true;
        }

        public int CurrentActionId { get; set; }
        public string CurrentActionName { get; set; }
        public string ActionDateTime { get; set; }
        public long UtcActionDateTime { get; set; }
        public bool IsTimeCardEnabled { get; set; }
        public List<TimeCardActionSummary> Summary { get; set; }
    }

    public class TimeCardActionSummary
    {
        public int Id { get; set; }
        public string ActionName { get; set; }
        public int ActionId { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Duration { get; set; }
        public string ActionDate { get; set; }
        public long UtcStartTime { get; set; }
        public long UtcEndTime { get; set; }
        public long UtcActionDate { get; set; }
        public string UserLocation { get; set; }
        public string Distance { get; set; }
        public string TotalShiftTime { get; set; }
    }
}
