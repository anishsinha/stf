using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.Queue
{
    public class QueueMessageResult
    {
        public int Id { get; set; }
        public int QueueMessageId { get; set; }
        public string ErrorInfo { get; set; }
        public int RetryCount { get; set; }
    }
}
