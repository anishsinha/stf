using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class MessageViewModel
    {
        public MessageViewModel()
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            Sound = Resource.lblDefault;
            Notify = true;
        }

        public string Body { get; set; }

        public string Title { get; set; }

        public string Sound { get; set; }

        public bool Notify { get; set; }

        public int NotificationCode { get; set; }

        public int MessageCode { get; set; }

        public List<string> FCMAppIds { get; set; }
        public object Data { get; set; }
    }
}
