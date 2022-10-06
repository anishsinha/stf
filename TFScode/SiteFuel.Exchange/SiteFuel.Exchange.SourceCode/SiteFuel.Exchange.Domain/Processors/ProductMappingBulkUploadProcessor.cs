using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.Queue;

namespace SiteFuel.Exchange.Domain.Processors
{
    public class ProductMappingBulkUploadProcessor : IQueueMessageProcessor
    {
        public QueueProcessType ProcessorName => QueueProcessType.ProductMappingBulkUpload;

        public bool Process(QueueMessageViewModel queueMessage, out List<string> errorInfo, out string queueMessageJson)
        {
            errorInfo = new List<string>();
            queueMessageJson = queueMessage.JsonMessage;
            var input = JsonConvert.DeserializeObject<ProductMappingBulkUploadModel>(queueMessageJson);
            if (input == null)
            {
                errorInfo.Add("Couldn't parse file jsonMessage. Contact Support.");
                throw new QueueMessageFatalException($"Couldn't parse file jsonMessage", errorInfo);
            }
            if (queueMessage.RetryCount < 2)
            {
                var productMappingDomain = ContextFactory.Current.GetDomain<ProductMappingDomain>();
                var result = Task.Run(() => productMappingDomain.ProcessProductMappingBulkUploadFile(input)).Result;
                errorInfo.AddRange(result);
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
