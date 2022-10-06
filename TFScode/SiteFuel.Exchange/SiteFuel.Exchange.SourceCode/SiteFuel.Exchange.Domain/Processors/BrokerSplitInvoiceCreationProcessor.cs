using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.Queue;
using SiteFuel.Exchange.ViewModels.ThirdPartyOrder;
using SiteFuel.Exchange.FileGenerator.DTN;

namespace SiteFuel.Exchange.Domain.Processors
{
    public class BrokerSplitInvoiceCreationProcessor : IQueueMessageProcessor
    {
        public QueueProcessType ProcessorName => QueueProcessType.BrokerSplitInvoiceCreation;

        public bool Process(QueueMessageViewModel queueMessage, out List<string> errorInfo, out string queueMessageJson)
        {
            errorInfo = new List<string>();
            queueMessageJson = queueMessage.JsonMessage;
            var input = JsonConvert.DeserializeObject<CreateBrokerSplitInvoiceQueueViewModel>(queueMessageJson);
            if (input == null)
            {
                errorInfo.Add("Couldn't parse Broker Split Invoice creation jsonMessage. Contact Support.");
                throw new QueueMessageFatalException($"Couldn't parse Broker Split Invoice creation jsonMessage", errorInfo);
            }
            if (queueMessage.RetryCount < 2)
            {
                var invoiceCreateDomain = ContextFactory.Current.GetDomain<InvoiceCreateDomain>();
                errorInfo.AddRange(Task.Run(() => invoiceCreateDomain.CreateBrokerSplitInvoiceFromQueueService(input)).Result);
                return true;
            }
            else
            {
                errorInfo.Add("Broker Split Invoice creation processing reached maximum retry count");
                throw new QueueMessageFatalException($"Queue message is been already tried for {queueMessage.RetryCount}", errorInfo);
            }
        }
    }
}
