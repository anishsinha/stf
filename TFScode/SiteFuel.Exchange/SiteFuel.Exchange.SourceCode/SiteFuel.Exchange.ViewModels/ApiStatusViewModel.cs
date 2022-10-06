using Newtonsoft.Json;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class ApiStatusViewModel
    {
        public ApiStatusViewModel(Status status = Status.Failed)
        {
            if (status == Status.Success)
            {
                StatusCode = Status.Success;
                StatusMessage = Resource.errMessageSuccess;
            }
            else
            {
                StatusCode = Status.Failed;
                StatusMessage = Resource.errMessageFailed;
            }
        }

        public Status StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public int FailedStatusCode { get; set; }
        public int MessageCode { get; set; }
        public Object ResponseData { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
