using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.ViewModels
{
    public class Usp_DeliveryStatisticsViewModel : StatusViewModel
    {
        public Usp_DeliveryStatisticsViewModel()
        {
           
        }

        public Usp_DeliveryStatisticsViewModel(Status status)
            : base(status)
        {
           
        }

        public int GroupingId { get; set; }

        public string GroupingName { get; set; }

        public decimal TotalDeliveries { get; set; }

        public decimal TotalOnTimeDeliveries { get; set; }

        public decimal TotalLateDeliveries { get; set; }

        public int OnTimeDeliveryPercentage { get; set; }

        public int LateDeliveryPercentage { get; set; }

        public string AverageTimeDelay { get; set; }

        public long AverageDeliveryTime { get; set; }

        public decimal TotalQuantityDelivered { get; set; }

        public decimal AverageQuantityPerDelivery { get; set; }

        public decimal GlobalTotalDeliveries { get; set; }

        public int GlobalOnTimeDeliveryPercentage { get; set; }

        public int GlobalLateDeliveryPercentage { get; set; }

        public decimal GlobalTotalOnTimeDeliveries { get; set; }

        public decimal GlobalTotalLateDeliveries { get; set; }
    }
}
