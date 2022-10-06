using SiteFuel.Exchange.Core;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels.InvoiceExceptions
{
    public class ApprovalExceptionRequestModel : DataTableAjaxPostModel
    {
        public string ExceptionTypes { get; set; } // 1 - DeliveredQuantityVariance | 2 - DuplicateInvoice
    }
}
