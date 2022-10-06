using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.Queue;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain.Processors
{
    public class ConsolidatedMobileInvoiceCreationProcessor : IQueueMessageProcessor
    {
        public QueueProcessType ProcessorName => QueueProcessType.CreateMobileInvoiceUsingJsonFile;

        public bool Process(QueueMessageViewModel queueMessage, out List<string> errorInfo, out string queueMessageJson)
        {
            errorInfo = new List<string>();
            queueMessageJson = queueMessage.JsonMessage;
            var input = JsonConvert.DeserializeObject<InvoiceBulkUploadProcessorReqViewModel>(queueMessageJson);
            if (input == null)
            {
                errorInfo.Add("Couldn't parse mobile invoice creation jsonMessage. Contact Support.");
                throw new QueueMessageFatalException($"Couldn't parse mobile invoice creation jsonMessage", errorInfo);
            }
            if (queueMessage.RetryCount < 2)
            {
                var domain = ContextFactory.Current.GetDomain<ConsolidatedInvoiceDomain>();
                errorInfo.AddRange(Task.Run(() => domain.CreateMobileInvoiceFromQueueService(input)).Result);
                if (errorInfo.Count > 0)
                    return false;
                else
                    return true;
            }
            else
            {
                errorInfo.Add("Mobile invoice creation processing reached maximum retry count");
                throw new QueueMessageFatalException($"Queue message is been already tried for {queueMessage.RetryCount}, QueueMessageId:" + queueMessage.MessageId, errorInfo);
            }
        }
    }
}
