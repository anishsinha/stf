using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.InvoiceExceptions
{
    public class InvoiceExceptionResponseModel : BaseViewModel
    {
        public bool IsExceptionsEnabled { get; set; }
        public bool IsExceptionsRaised { get; set; }
        public List<ExceptionRaised> Exceptions { get; set; } = new List<ExceptionRaised>();
    }
}
