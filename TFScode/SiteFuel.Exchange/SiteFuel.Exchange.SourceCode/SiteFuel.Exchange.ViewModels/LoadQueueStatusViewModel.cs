using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class LoadQueueStatusViewModel
    {
        public LoadQueueStatusViewModel(Status status = Status.Failed)
        {
            if (status == Status.Success)
            {
                StatusCode = Status.Success;
            }
            else
            {
                StatusCode = Status.Failed;
            }
        }
        public Status StatusCode { get; set; }
        public List<DsbLoadQueueStatusViewModel> LoadQueueSuccessInfo { get; set; } = new List<DsbLoadQueueStatusViewModel>();
        public List<DsbLoadQueueStatusViewModel> LoadQueueErrorInfo { get; set; } = new List<DsbLoadQueueStatusViewModel>();
    }
}
