using SiteFuel.Exchange.Core;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels.InvoiceExceptions
{
    public class AssignOrderMissingDeliveryModel
    {
        public int OldOrderId { get; set; }

        public int NewOrderId { get; set; }

        public int InvoiceId { get; set; }

        public bool IsReassignDifferentJob { get; set; }
    }
}
