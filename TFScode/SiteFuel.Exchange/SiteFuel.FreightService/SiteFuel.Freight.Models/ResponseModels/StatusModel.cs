using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels
{
    public class StatusModel
    {
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public int MessageCode { get; set; }
        public List<string> EntityIds { get; set; } = new List<string>();
        public Dictionary<string, string> EntityParentIds { get; set; } = new Dictionary<string, string>();
        public List<DsbLoadQueueSuccess> DsbLoadQueueSuccess { get; set; } = new List<DsbLoadQueueSuccess>();
        public StatusModel(Status status = Status.Failed)
        {
            if (status == Status.Success)
            {
                StatusCode = (int)Status.Success;
                StatusMessage = Resource.errMessageSuccess;
            }
            else
            {
                StatusCode = (int)Status.Failed;
                StatusMessage = Resource.errMessageFailed;
            }
        }

    }
    public class DsbLoadQueueSuccess
    {
        public string Id { get; set; }
        public string RegionId { get; set; }
        public string ShiftId { get; set; }
        public int ShiftIndex { get; set; }
        public int DriverRowIndex { get; set; }
    }
    public class LongStatusModel : StatusModel
    {
        public long Result { get; set; }
    }
}
