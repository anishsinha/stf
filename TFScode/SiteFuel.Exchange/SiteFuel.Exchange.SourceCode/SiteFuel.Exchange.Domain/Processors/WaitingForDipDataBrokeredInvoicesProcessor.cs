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


namespace SiteFuel.Exchange.Domain.Processors
{
    public class WaitingForDipDataBrokeredInvoicesProcessor: IQueueMessageProcessor
    {

        public QueueProcessType ProcessorName => QueueProcessType.ConvertBrokeredInvoiceForDipData;

        public bool Process(QueueMessageViewModel queueMessage, out List<string> errorInfo, out string queueMessageJson)
        {
            errorInfo = new List<string>();
            queueMessageJson = queueMessage.JsonMessage;
            var bulkUploadMsg = JsonConvert.DeserializeObject<ConvertBrokeredInvoiceForDipDataQueueMessage>(queueMessageJson);
            if (bulkUploadMsg == null)
            {
                errorInfo.Add("Couldn't parse bulkUploadMsg. Contact Support.");
                throw new QueueMessageFatalException($"Couldn't parse bulkUploadMsg to ConvertBrokeredInvoiceForDipData", errorInfo);
            }
            if (queueMessage.RetryCount < 2)
            {
                var exceptionDomain = ContextFactory.Current.GetDomain<ExceptionDomain>();
                exceptionDomain.ProcessConvertBrokeredInvoiceForDipData(bulkUploadMsg, errorInfo);
                return true;
            }
            else
            {
                errorInfo.Add("ConvertBrokeredInvoiceForDipData processing reached maximum retry count");
                throw new QueueMessageFatalException($"Queue message is been already tried for {queueMessage.RetryCount}", errorInfo);
            }
        }
    }
}
