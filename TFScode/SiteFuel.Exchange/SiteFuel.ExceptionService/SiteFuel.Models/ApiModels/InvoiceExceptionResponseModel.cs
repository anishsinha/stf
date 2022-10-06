using SiteFuel.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Models.ApiModels
{
    public class InvoiceExceptionResponseModel : BaseModel
    {
        public bool IsExceptionsEnabled { get; set; }
        public bool IsExceptionsRaised { get; set; }
        public List<ExceptionRaised> Exceptions { get; set; } = new List<ExceptionRaised>();
    }

    public class ExceptionRaised
    {
        public int? InvoiceId { get; set; }
        public int ExceptionId { get; set; }
        public int ExceptionTypeId { get; set; }
        public DateTimeOffset RaisedOn { get; set; }
        public int ApproverCompanyId { get; set; }
        public int StatusId { get; set; }
        public bool IsActive { get; set; }
        public int OrderId { get; set; }
    }
}
