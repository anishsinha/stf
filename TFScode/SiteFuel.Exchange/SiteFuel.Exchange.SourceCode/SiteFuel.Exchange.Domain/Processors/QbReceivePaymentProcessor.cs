using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.Payments;
using SiteFuel.Exchange.ViewModels.Queue;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain.Processors
{
    public class QbReceivePaymentProcessor : IQueueMessageProcessor
    {
        public QueueProcessType ProcessorName => QueueProcessType.ReceivePayment;

        public bool Process(QueueMessageViewModel queueMessage, out List<string> errorInfo, out string queueMessageJson)
        {
            errorInfo = new List<string>();
            queueMessageJson = queueMessage.JsonMessage;
            var input = JsonConvert.DeserializeObject<ReceivePaymentQueueMessage>(queueMessageJson);
            if (input == null)
            {
                errorInfo.Add("Couldn't parse receive payment QB response jsonMessage. Contact Support.");
                throw new QueueMessageFatalException($"Couldn't receive payment QB response jsonMessage", errorInfo);
            }
            if (queueMessage.RetryCount < 2)
            {
                var domain = ContextFactory.Current.GetDomain<QbDomain>();
                var result = Task.Run(() => domain.ProcessQbInvoicePayments(input)).Result;
                errorInfo.Add(result.StatusMessage);
                return true;
            }
            else
            {
                errorInfo.Add("Receive payment QB response processing reached maximum retry count");
                throw new QueueMessageFatalException($"Queue message is been already tried for {queueMessage.RetryCount}", errorInfo);
            }
        }
    }
}
