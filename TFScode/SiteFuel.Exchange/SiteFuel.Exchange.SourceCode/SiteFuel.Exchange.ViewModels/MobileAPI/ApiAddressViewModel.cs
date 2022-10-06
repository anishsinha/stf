
using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.ViewModels
{
    public class ApiAddressViewModel
    {
        public int Id { get; set; }

        public string Address { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public bool IsJobLocation { get; set; }
                 
        public DropAddressStatus Status { get; set; }
    }

    public class ApiDispatchAddressViewModel
    {
        public int UserId { get; set; }

        public int OrderId { get; set; }

        public int? DeliveryScheduleId { get; set; }

        public int? TrackableScheduleId { get; set; }

        public ApiAddressViewModel Address { get; set; }
    }
}

