using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.Queue;
using SiteFuel.Exchange.ViewModels.TankRental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain.Processors
{
    public class TankRentalInvoiceProcessor : IQueueMessageProcessor
    {
        public QueueProcessType ProcessorName => QueueProcessType.TankRentalInvoice;

        public bool Process(QueueMessageViewModel queueMessage, out List<string> errorInfo, out string queueMessageJson)
        {
            var result = false;
            errorInfo = new List<string>();
            queueMessageJson = queueMessage.JsonMessage;
            var tankRentalQueueMessage = JsonConvert.DeserializeObject<TankRentalQueueMessage>(queueMessageJson);
            if (tankRentalQueueMessage == null)
            {
                errorInfo.Add("Couldn't parse Tank Rental Invoice jsonMessage. Contact Support.");
                throw new QueueMessageFatalException($"Couldn't parse Tank Rental Invoice jsonMessage to TankRentalQueueMessage", errorInfo);
            }
            if (queueMessage.RetryCount < 2)
            {
                var domain = ContextFactory.Current.GetDomain<TankRentalInvoiceDomain>();
                result = Task.Run(() => domain.AutoGenerateTankRentalInvoice1(tankRentalQueueMessage)).Result;
                return result;
            }
            else
            {
                errorInfo.Add("Tank rental invoice creation processing reached maximum retry count");
                throw new QueueMessageFatalException($"Queue message is been already tried for {queueMessage.RetryCount}", errorInfo);
            }
        }
    }
}
