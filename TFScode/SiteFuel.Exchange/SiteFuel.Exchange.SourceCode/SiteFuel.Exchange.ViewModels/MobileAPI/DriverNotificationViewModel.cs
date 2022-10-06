using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.CustomAttributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class DriverNotificationViewModel 
    {
        public DriverNotificationViewModel()
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            DriverIds = new List<int>();
            Message = new MessageViewModel();
        }

        public MessageViewModel Message { get; set; }

        public List<int> DriverIds { get; set; }
    }
}
