using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class TimeCardSettingViewModel : StatusViewModel
    {
        public TimeCardSettingViewModel()
        {
        }

        public TimeCardSettingViewModel(Status status)
            : base(status)
        {
        }

        public bool IsTimeCardEnabled { get; set; }
        public int UserId { get; set; }
        public int CompanyId { get; set; }
        
    }
}
