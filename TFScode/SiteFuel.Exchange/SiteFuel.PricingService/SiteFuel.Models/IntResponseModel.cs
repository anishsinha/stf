using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Models
{
    public class IntResponseModel : BaseResponseModel
    {
        public IntResponseModel()
        {
        }
        public IntResponseModel(Status status) : base(status)
        {
            Status = status;
        }
        public int Result { get; set; }

        public List<int> ListResult { get; set; }
    }

    public class CustomResponseModel : BaseResponseModel
    {
        public int Result { get; set; }
        public string CustomString1 { get; set; }
        public string CustomString2 { get; set; }
    }
}
