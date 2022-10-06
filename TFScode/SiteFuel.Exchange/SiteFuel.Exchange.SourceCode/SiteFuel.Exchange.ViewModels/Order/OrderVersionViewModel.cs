using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class OrderVersionViewModel : BaseViewModel
    {
        public OrderVersionViewModel()
        {
            InstanceInitialize();
        }

        public OrderVersionViewModel(Status status) : base(status)
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            DeliverySchedules = new List<DeliveryScheduleViewModel>();
            OnsiteDeliveryRequests = new List<DeliveryScheduleViewModel>();
        }

        public int Id { get; set; }

        public int CreatedBy { get; set; }

        public int Version { get; set; }

        public string AdditionalNotes { get; set; }

        public List<DeliveryScheduleViewModel> DeliverySchedules { get; set; }

        public List<DeliveryScheduleViewModel> OnsiteDeliveryRequests { get; set; }
    }
}
