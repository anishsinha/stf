using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    public class InvoiceException
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public int GeneratedExceptionId { get; set; }
        public int ExceptionTypeId { get; set; }
        public DateTimeOffset RaisedOn { get; set; }
        public virtual Invoice Invoice { get; set; }
        public bool IsActive { get; set; }
        public int StatusId { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
    }
}
