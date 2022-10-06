using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Models
{
    public class BaseResponseModel
    {
        public BaseResponseModel()
        {
        }
        public BaseResponseModel(Status status)
        {
            Status = status;
        }
        public Status Status { get; set; }
        public string Message { get; set; }
    }
}
