using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class AppSettingRequestViewModel
    {
        public AppSettingRequestViewModel()
        {
            Parameters = new List<KeyValuePair<string, string>>();
        }

        public List<KeyValuePair<string, string>> Parameters { get; set; }
    }
}
