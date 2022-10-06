using System.Collections.Generic;
using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.MobileAPI.TPD;
using SiteFuel.Exchange.ViewModels.Queue;

namespace SiteFuel.Exchange.Domain.Processors
{
    public class BrokerInvoiceImageProcessor : IQueueMessageProcessor
    {
        public QueueProcessType ProcessorName => QueueProcessType.BrokerInvoiceImageUpload;

        public bool Process(QueueMessageViewModel queueMessage, out List<string> errorInfo, out string queueMessageJson)
        {
            errorInfo = new List<string>();
            queueMessageJson = queueMessage.JsonMessage;
            var input = JsonConvert.DeserializeObject<BrokerImageUpdateProcessorViewModel>(queueMessageJson);
            if (input == null)
            {
                errorInfo.Add("Couldn't parse file jsonMessage. Contact Support.");
                throw new QueueMessageFatalException($"Couldn't parse file jsonMessage", errorInfo);
            }
            if (queueMessage.RetryCount < 2)
            {
                var invoiceTPDDomain = ContextFactory.Current.GetDomain<InvoiceTPDDomain>();
                invoiceTPDDomain.ProcessBrokerImagesUpload(input, errorInfo);
                return true;
            }
            else
            {
                errorInfo.Add("file processing reached maximum retry count");
                throw new QueueMessageFatalException($"Queue message is been already tried for {queueMessage.RetryCount}", errorInfo);
            }
        }
    }
}
