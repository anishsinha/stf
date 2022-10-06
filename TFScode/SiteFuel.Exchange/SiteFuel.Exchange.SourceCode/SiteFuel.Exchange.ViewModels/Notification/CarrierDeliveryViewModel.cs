using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.Notification
{
    public class CarrierDeliveryNotificationViewModel : BaseNotificationViewModel
    {
        public List<CarrierDeliveryViewModel> CarrierDeliveryViewModel { get; set; } = new List<CarrierDeliveryViewModel>();
    }
}
