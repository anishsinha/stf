using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Models
{
    public class PricingConfigResponseModel : BaseResponseModel
    {
        public PricingConfigResponseModel()
        {
        }
        public PricingConfigResponseModel(Status status) : base(status)
        {
            Status = status;
        }

        public List<PricingConfigModel> ConfigList { get; set; }

        public PricingConfigModel Config { get; set; }
    }

    public class PricingConfigModel
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
