using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.InvoiceExceptions
{
    public class ExceptionApprovalResponseModel : DeliveredQuantityVarianceModel
    {
        public int InvoiceId { get; set; }

        public int InvoiceHeaderId { get; set; }

        public string ExceptionAdditionalDetail { get; set; }

        public InvoiceExceptionModel OrigionalInvoice { get; set; }

        public JobTankDetailsViewModel TankInfo { get; set; }
    }
}
