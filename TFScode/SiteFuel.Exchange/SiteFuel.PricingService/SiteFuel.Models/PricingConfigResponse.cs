using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Models
{
    public class PricingConfigResponse : BaseResponseModel
    {
        public PricingConfigResponse()
        {
        }
        public PricingConfigResponse(Status status) : base(status)
        {
            Status = status;
        }

        public List<PricingConfig> Configs { get; set; }
    }

    public class PricingConfig
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
