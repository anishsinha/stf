using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class EnrouteDeliveryViewModel : StatusViewModel
    {
        public EnrouteDeliveryViewModel()
        {
            Message = new MessageViewModel();
        }

        public EnrouteDeliveryViewModel(Status status)
            : base(status)
        {
            Message = new MessageViewModel();
        }
        private int? _orderId = null;
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public int UserId { get; set; }

        public string FCMAppId { get; set; }

        public int? OrderId { get { return _orderId > 0 ? _orderId : (int?)null; } set { _orderId = value; } }

        public int? DeliveryScheduleId { get; set; }

        public int? TrackableScheduleId { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public int StatusId { get; set; }

        public MessageViewModel Message { get; set; }
    }
}
