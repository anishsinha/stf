using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.ViewModels
{
    public class ResponseViewModel
    {
        public ResponseViewModel(Status status = Status.Failed)
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
    }
}
