using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.InvoiceExceptions
{
    public class ExceptionRaised
    {
        public int? InvoiceId { get; set; }
        public int ExceptionId { get; set; }
        public int ExceptionTypeId { get; set; }
        public DateTimeOffset RaisedOn { get; set; }
        public int ApproverCompanyId { get; set; }
        public int StatusId { get; set; }
        public bool IsActive { get; set; }
        public int OrderId { get;set; }
    }
}
