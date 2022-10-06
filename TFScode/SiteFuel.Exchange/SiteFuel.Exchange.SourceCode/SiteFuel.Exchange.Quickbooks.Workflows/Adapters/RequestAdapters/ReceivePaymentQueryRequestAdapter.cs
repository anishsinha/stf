using SiteFuel.Exchange.Quickbooks.Models;
using SiteFuel.Exchange.Quickbooks.SharedEnums;
using SiteFuel.Exchange.Quickbooks.Workflows.Extensions;
using SiteFuel.Exchange.Quickbooks.Workflows.Mappers;
using SiteFuel.Exchange.Quickbooks.Workflows.Models;

namespace SiteFuel.Exchange.Quickbooks.Workflows.Adapters.RequestAdapters
{
    public class ReceivePaymentQueryRequestAdapter : BaseRequestAdapter<PaymentViewModel>
    {
        public override AdapterResponse Convert(PaymentViewModel inputViewModel)
        {
            var receivePaymentDateRange = new ModifiedDateRangeFilter
            {
                FromModifiedDate = inputViewModel.FromModifiedDate.ToDateString(),
                ToModifiedDate = inputViewModel.ToModifiedDate.ToDateString(),
            };

            ReceivePaymentQueryRq receivePaymentQueryRq = new ReceivePaymentQueryRq
            {
                ModifiedDateRangeFilter = receivePaymentDateRange
            };

            return new AdapterResponse
            {
                QbXmlObject = receivePaymentQueryRq,
                QbXmlType = QbXmlType.ReceivePaymentQuery,
                Status = QbXmlStatus.ReadyToQueue,
                EntityType = QbEntityType.ReceivePayment.ToString()
            };
        }
    }
}
