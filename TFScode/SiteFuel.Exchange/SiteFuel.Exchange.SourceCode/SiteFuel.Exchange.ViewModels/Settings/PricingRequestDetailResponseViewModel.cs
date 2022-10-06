using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class PricingConfigApiResponse
    {
        public Status Status { get; set; }
        public string Message { get; set; }
        public List<PricingConfigViewModel> ConfigList { get; set; }
        public PricingConfigViewModel Config { get; set; }
    }

    public class PricingConfigViewModel
    {
        public int Id { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        public string Description { get; set; }

        public string UpdatedBy { get; set; }

        public string UpdatedDate { get; set; }

        public bool IsActive { get; set; }
    }
}
