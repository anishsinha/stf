using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Models
{
    public class PriceUpdatedDateResponseModel : BaseResponseModel
    {
        public PriceUpdatedDateResponseModel()
        {
        }
        public PriceUpdatedDateResponseModel(Status status) : base(status)
        {
            Status = status;
        }
        public DateTimeOffset PriceLastUpdatedDate { get; set; }
    }
}
