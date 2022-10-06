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
using SiteFuel.Exchange.ViewModels.FreightOnlyOrder;

namespace SiteFuel.Exchange.Domain.Processors
{
    public class ConsolidationForDrCompletion : IQueueMessageProcessor
    {
        public QueueProcessType ProcessorName => QueueProcessType.ConsolidationForDrCompletion;

        public bool Process(QueueMessageViewModel queueMessage, out List<string> errorInfo, out string queueMessageJson)
        {
            errorInfo = new List<string>();
            queueMessageJson = queueMessage.JsonMessage;
            var bulkUploadMsg = JsonConvert.DeserializeObject<GroupDrInvoiceCreationViewModelForQueueService>(queueMessageJson);
            if (bulkUploadMsg == null)
            {
                errorInfo.Add("Couldn't parse Group DR invoice view model. Contact Support.");
                throw new QueueMessageFatalException($"Couldn't parse Group DR invoice view model", errorInfo);
            }
            if (queueMessage.RetryCount < 2)
            {
                var carrierDomain = ContextFactory.Current.GetDomain<ConsolidatedDdtDomain>();
                carrierDomain.ProcessConsolidationForGroupParentDr(bulkUploadMsg, errorInfo);
                return true;
            }
            else
            {
                errorInfo.Add("Group DR invoice processing reached maximum retry count");
                throw new QueueMessageFatalException($"Queue message is been already tried for {queueMessage.RetryCount}", errorInfo);
            }
        }

    }
}
