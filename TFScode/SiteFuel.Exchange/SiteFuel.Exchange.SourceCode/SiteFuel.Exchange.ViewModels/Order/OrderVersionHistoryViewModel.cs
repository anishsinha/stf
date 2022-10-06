using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class OrderVersionHistoryViewModel : StatusViewModel
    {
        public OrderVersionHistoryViewModel()
        {
            InstanceInitialize();
        }

        public OrderVersionHistoryViewModel(Status status)
            : base(status)
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            DeliverySchedules = new List<DeliveryScheduleViewModel>();
        }

        public string CreatedUser { get; set; }

        public string CreatedDate { get; set; }

        public string AcceptedUser { get; set; }

        public string AcceptedDate { get; set; }

        public int Version { get; set; }

        public IList<DeliveryScheduleViewModel> DeliverySchedules { get; set; }
    }
}
