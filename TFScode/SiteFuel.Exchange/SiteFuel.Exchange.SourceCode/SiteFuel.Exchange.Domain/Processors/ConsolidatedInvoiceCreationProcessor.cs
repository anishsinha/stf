using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.Queue;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain.Processors
{
    public class ConsolidatedInvoiceCreationProcessor : IQueueMessageProcessor
    {
        public QueueProcessType ProcessorName => QueueProcessType.CreateInvoiceUsingJsonFile;

        public bool Process(QueueMessageViewModel queueMessage, out List<string> errorInfo, out string queueMessageJson)
        {
            Thread.Sleep(300);
            errorInfo = new List<string>();
            queueMessageJson = queueMessage.JsonMessage;
            var input = JsonConvert.DeserializeObject<InvoiceBulkUploadProcessorReqViewModel>(queueMessageJson);
            if (input == null)
            {
                errorInfo.Add("Couldn't parse Invoice creation jsonMessage. Contact Support.");
                throw new QueueMessageFatalException($"Couldn't parse Invoice creation jsonMessage", errorInfo);
            }
            if (queueMessage.RetryCount < 2)
            {
                var domain = ContextFactory.Current.GetDomain<ConsolidatedInvoiceDomain>();
                errorInfo.AddRange(Task.Run(() => domain.CreateManualInvoiceFromQueueService(input)).Result);
                if (errorInfo.Count > 0)
                    return false;
                else
                    return true;
            }
            else
            {
                errorInfo.Add("Invoice creation processing reached maximum retry count");
                throw new QueueMessageFatalException($"Queue message is been already tried for {queueMessage.RetryCount}", errorInfo);
            }
        }
    }
}
