using SiteFuel.Exchange.Utilities;
using System;

namespace SiteFuel.Models
{
    public class BooleanResponseModel : BaseResponseModel
    {
        public BooleanResponseModel()
        {
        }
        public BooleanResponseModel(Status status) : base(status)
        {
            Status = status;
        }
        public bool Result { get; set; }
    }
}
