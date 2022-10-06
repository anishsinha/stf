using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class DashboardDeliveryStatisticsViewModel : StatusViewModel
    {
        public DashboardDeliveryStatisticsViewModel()
        {
            
        }

        public DashboardDeliveryStatisticsViewModel(Status status)
            : base(status)
        {
           
        }

        public int SelectedJobId { get; set; }

        public string GroupIds { get; set; }

        public List<DashboardDeliveryStatisticsGridViewModel> Deliveries { get; set; } = new List<DashboardDeliveryStatisticsGridViewModel>();

        public string GlobalTotalDeliveries { get; set; } = ApplicationConstants.Zero;

        public decimal GlobalOnTimeDeliveryPercentage { get; set; }

        public decimal GlobalLateDeliveryPercentage { get; set; }

        public string GlobalTotalOnTimeDeliveries { get; set; } = ApplicationConstants.Zero;

        public string GlobalTotalLateDeliveries { get; set; } = ApplicationConstants.Zero;

        public string GlobalAverageDeliveryTime { get; set; } = Resource.lblHyphen;

        public bool IsCollapsed { get; set; }
    }
}
