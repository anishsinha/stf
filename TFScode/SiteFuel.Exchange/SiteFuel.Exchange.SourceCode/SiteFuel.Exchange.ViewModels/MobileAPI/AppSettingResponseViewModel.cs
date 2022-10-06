using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class AppSettingResponseViewModel : StatusViewModel
    {
        public AppSettingResponseViewModel(Status status = Status.Failed)
        {
            AppSettings = new List<KeyValuePair<string, string>>();
        }

        public List<KeyValuePair<string, string>> AppSettings { get; set; }
    }
}
