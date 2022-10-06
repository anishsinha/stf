using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.Queue;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain.Processors
{
    public class EditBrokerInvoiceProcessor : IQueueMessageProcessor
    {
        public QueueProcessType ProcessorName => QueueProcessType.EditBrokerInvoice;

        public bool Process(QueueMessageViewModel queueMessage, out List<string> errorInfo, out string queueMessageJson)
        {
            errorInfo = new List<string>();
            queueMessageJson = queueMessage.JsonMessage;
            var input = JsonConvert.DeserializeObject<EditInvoiceProcessorModel>(queueMessageJson);
            if (input == null)
            {
                errorInfo.Add("Couldn't parse broker invoice edit json model. Contact Support.");
                throw new QueueMessageFatalException($"Couldn't parse broker invoice edit json model", errorInfo);
            }
            if (queueMessage.RetryCount < 2)
            {
                var domain = ContextFactory.Current.GetDomain<InvoiceEditDomain>();
                errorInfo.AddRange(Task.Run(() => domain.EditBrokeredInvoiceFromQueueService(input)).Result);
                if (errorInfo.Count > 0)
                    return false;
                else
                    return true;
            }
            else
            {
                errorInfo.Add("Edit Broker Invoice processing reached maximum retry count");
                throw new QueueMessageFatalException($"Queue message is been already tried for {queueMessage.RetryCount}", errorInfo);
            }
        }
    }
}
