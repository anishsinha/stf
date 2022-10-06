using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiteFuel.Exchange.Utilities;
namespace SiteFuel.Exchange.ViewModels
{
    public class AcceptOfferMobileViewModel
    {
        public AcceptOfferMobileViewModel()
        {
            DeliverySchedules = new List<DeliveryScheduleViewModel>();
        }
        public int BuyerId { get; set; }
        public int JobId { get; set; }
        public string EncryptedRequest { get; set; }
        public string Address { get; set; }
        public int DeliveryTypeId { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public Nullable<DateTimeOffset> EndDate { get; set; }
        public Nullable<DateTimeOffset> ExpirationDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public List<DeliveryScheduleViewModel> DeliverySchedules { get; set; }
        //FTL support
        public TruckLoadTypes TruckLoadTypes { get; set; }

        public int PricingTypeId { get; set; }

        public PricingSource PricingSourceId { get; set; }

        public PricingSourceFeedTypes FeedTypeId { get; set; }

        public QuantityIndicatorTypes QuantityIndicatorTypes { get; set; }

        public FuelClassTypes FuelClassTypes { get; set; }

        public int? CityGroupTerminalId { get; set; }
    }
}
