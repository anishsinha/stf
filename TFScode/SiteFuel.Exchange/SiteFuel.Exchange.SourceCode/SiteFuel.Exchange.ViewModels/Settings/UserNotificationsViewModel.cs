using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class UserNotificationsViewModel : StatusViewModel
    {
        public UserNotificationsViewModel()
        {
            InstanceInitialize();
        }

        public UserNotificationsViewModel(Status status)
            : base(status)
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            UserNotificationSettings = new List<UserNotificationSettingViewModel>();
        }

        public int UserId { get; set; }

        public List<UserNotificationSettingViewModel> UserNotificationSettings { get; set; }
    }
}
