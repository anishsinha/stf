using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class DeliveryScheduleMessageViewModel
    {
        public DeliveryScheduleMessageViewModel()
        {
            CurrentSchedules = new List<DeliveryScheduleDetail>();
            PreviousSchedules = new List<DeliveryScheduleDetail>();
        }

        public int RecipientUserId { get; set; }

        public string RecipientUserName { get; set; }

        public string CompanyUserName { get; set; }

        public string CompanyName { get; set; }

        public string PoNumber { get; set; }

        public string TargetUrl { get; set; }

        public IEnumerable<DeliveryScheduleDetail> CurrentSchedules { get; set; }

        public IEnumerable<DeliveryScheduleDetail> PreviousSchedules { get; set; }
    }
}
