using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.InvoiceExceptions
{
    public class ManageCustomerExceptionModel : StatusViewModel
    {
        public int UserId { get; set; }
        public int OwnerCompanyId { get; set; }
        public int EnabledForCompanyId { get; set; }
        public List<CustomerExceptionModel> Exceptions { get; set; }
    }

    public class CustomerExceptionModel
    {
        public int ExceptionTypeId { get; set; }
        public string ExceptionTypeName { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsActive { get; set; }
        public List<ListItem> Resolutions { get; set; } = new List<ListItem>();
    }
}
