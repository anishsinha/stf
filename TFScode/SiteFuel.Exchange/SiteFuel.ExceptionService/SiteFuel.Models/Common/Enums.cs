using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Models.Common.Enums
{
    public enum ExceptionApprover
    {
        Unknown = 0,
        Self = 1,
        Buyer = 2,
        Supplier = 3,
        CarrierOrSupplier = 4
    }

    public enum ExceptionType
    {
        Unknown = 0,
        DeliveredQuantityVariance = 1,
        DuplicateInvoice = 2,
        InvoiceApiException = 3,
        UnknownDeliveries = 4,
        MissingDeliveries = 5,
        UnmatchedDeliveryLocation = 4,
        OverFilledAsset = 5,
        UnassignedAsset = 6,
    }

    public enum ExceptionStatus
    {
        Unknown = 0,
        Raised = 1,
        Resolved = 2,
        AutoApproved = 3,
        Deleted = 4
    }

    public enum ExceptionResolution
    {
        Unknown = 0,
        ApproveBOLQuantity = 1,
        ApproveDroppedQuantity = 2,
        ApproveBOLMinusNetVariance = 3,
        ApproveDropTicket = 4,
        DiscardDropTicket = 5,
        CreateManualInvoice = 6,
        DiscardException = 7
    }
}
