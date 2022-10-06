using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain.Processors
{
    public interface IQueueMessageProcessor
    {
        QueueProcessType ProcessorName { get; }

        bool Process(QueueMessageViewModel queueMessage, out List<string> errorInfo, out string queueMessageJson);
    }
}
