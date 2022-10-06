using SiteFuel.Exchange.Core.StringResources;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class DeleteTpoCompanyViewModel : StatusViewModel
    {
        public string OrderIds { get; set; }

        public string JobIds { get; set; }

        public string AssetTankIds { get; set; }

        public string DeliveryScheduleIds { get; set; }

        public string TrackableScheduleIds { get; set; }
    }
}
