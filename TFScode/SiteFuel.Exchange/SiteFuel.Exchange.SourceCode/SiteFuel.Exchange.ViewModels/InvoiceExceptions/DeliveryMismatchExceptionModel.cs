using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.InvoiceExceptions
{
    public class DeliveryMismatchExceptionModel : JobTankDetailsViewModel
    {
        public string ExceptionAdditionalDetail { get; set; }
    }
}
