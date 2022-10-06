using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class MessagesCountViewModel : StatusViewModel
    {
        public MessagesCountViewModel()
        {
            InstanceInitialize();
        }

        public MessagesCountViewModel(Status status)
            : base(status)
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            LastFiveUnreadMessageModel = new List<DispalyMessageViewModel>();
        }

        public int TotalUnreadMessagesCount { get; set; }

        public List<DispalyMessageViewModel> LastFiveUnreadMessageModel { get; set; }       
    }
}
