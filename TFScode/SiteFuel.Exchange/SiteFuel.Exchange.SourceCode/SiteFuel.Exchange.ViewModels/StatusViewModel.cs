using Newtonsoft.Json;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class StatusViewModel
    {
        public StatusViewModel(Status status = Status.Failed)
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

        public PageDisplayMode DisplayMode { get; set; }

        public Status StatusCode { get; set; }

        public string StatusMessage { get; set; }

        public int FailedStatusCode { get; set; }
        public int MessageCode { get; set; }
        public int EntityId { get; set; }
        public int EntityHeaderId { get; set; }
        public int CustomerCompanyId { get; set; }
        public int CustomerId { get; set; }
        public List<string> EntityIds { get; set; }
        public Dictionary<string, string> EntityParentIds { get; set; }

        public string EntityNumber { get; set; }

        public Object ResponseData { get; set; }
        public int IsForecatingAccountLevel { get; set; } = 0;
        public int OttoNotificationCount { get; set; }
        public List<DsbLoadQueueSuccess> DsbLoadQueueSuccess { get; set; } = new List<DsbLoadQueueSuccess>();
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
