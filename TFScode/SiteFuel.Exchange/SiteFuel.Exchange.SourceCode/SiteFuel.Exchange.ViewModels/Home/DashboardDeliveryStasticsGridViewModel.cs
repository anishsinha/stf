using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.ViewModels
{
    public class DashboardDeliveryStatisticsGridViewModel : StatusViewModel
    {
        public DashboardDeliveryStatisticsGridViewModel()
        {
        }

        public DashboardDeliveryStatisticsGridViewModel(Status status)
            : base(status)
        {
        }

        public int GroupingId { get; set; }

        public string GroupingName { get; set; }

        public string TotalDeliveries { get; set; }

        public int OnTimeDeliveryPercentage { get; set; }

        public int LateDeliveryPercentage { get; set; }

        public string AverageTimeDelay { get; set; }

        public string TotalQuantityDelivered { get; set; }

        public string AverageQuantityPerDelivery { get; set; }
    }
}