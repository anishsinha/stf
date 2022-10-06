using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class UserNotificationSettingViewModel : StatusViewModel
    {
        public UserNotificationSettingViewModel()
        {
           
        }

        public UserNotificationSettingViewModel(Status status)
            : base(status)
        {
            
        }

        public int UserId { get; set; }

        public int EventTypeId { get; set; }

        [Display(Name = nameof(Resource.lblEventTypes), ResourceType = typeof(Resource))]
        public string EventTypeName { get; set; }

        [Display(Name = nameof(Resource.lblEmail), ResourceType = typeof(Resource))]
        public bool IsEmail { get; set; }

        [Display(Name = nameof(Resource.lblSMS), ResourceType = typeof(Resource))]
        public bool IsSMS { get; set; }

        [Display(Name = nameof(Resource.lblInApp), ResourceType = typeof(Resource))]
        public bool IsInApp { get; set; }

        public bool IsEmailEnabled { get; set; }

        public bool IsSmsEnabled { get; set; }
    }
}
