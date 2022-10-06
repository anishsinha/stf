using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Models.InvoiceException
{
    public class AutoApprovalExceptionModel
    {
        public int ExceptionId { get; set; }
        public int ExceptionTypeId { get; set; }
        public int OwnerCompanyId { get; set; }
        public int BuyerCompanyId { get; set; }
        public int SupplierCompanyId { get; set; }
        public decimal BolQuantity { get; set; }
        public decimal DeliveredQuantity { get; set; }
        public decimal Varience { get; set; }
        public int StatusId { get; set; }
        public DateTimeOffset GeneratedOn { get; set; }
        public DateTime AutoApprovalDate { get; set; }
    }
}
