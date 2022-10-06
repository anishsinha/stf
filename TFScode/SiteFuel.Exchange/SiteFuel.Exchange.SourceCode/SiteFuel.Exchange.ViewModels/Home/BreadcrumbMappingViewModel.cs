using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.ViewModels
{
    public class BreadcrumbMappingViewModel 
    {
        public string key { get; set; }

        public string breadcrumb_title { get; set; }

        public string page_title { get; set; }

        public string[] parents { get; set; }        
    }
}
