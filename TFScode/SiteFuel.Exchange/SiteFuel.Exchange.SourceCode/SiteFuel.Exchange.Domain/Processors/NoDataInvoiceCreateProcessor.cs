using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.Queue;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain.Processors
{
    public class NoDataInvoiceCreateProcessor : IQueueMessageProcessor
    {
        public QueueProcessType ProcessorName => QueueProcessType.CreateInvoiceForNoData;

        public bool Process(QueueMessageViewModel queueMessage, out List<string> errorInfo, out string queueMessageJson)
        {
            errorInfo = new List<string>();
            queueMessageJson = queueMessage.JsonMessage;
            var input = JsonConvert.DeserializeObject<NoDataProcessorModel>(queueMessageJson);
            if (input == null)
            {
                errorInfo.Add("Couldn't parse no data broker invoice create json model. Contact Support.");
                throw new QueueMessageFatalException($"Couldn't parse no data broker invoice create json model", errorInfo);
            }
            if (queueMessage.RetryCount < 2)
            {
                var domain = ContextFactory.Current.GetDomain<ExceptionDomain>();
                errorInfo.AddRange(Task.Run(() => domain.ProcessNoDataInvoiceCreate(input)).Result);
                if (errorInfo.Count > 0)
                    return false;
                else
                    return true;
            }
            else
            {
                errorInfo.Add("no data broker invoice create processing reached maximum retry count");
                throw new QueueMessageFatalException($"Queue message is been already tried for {queueMessage.RetryCount}", errorInfo);
            }
        }
    }
}
