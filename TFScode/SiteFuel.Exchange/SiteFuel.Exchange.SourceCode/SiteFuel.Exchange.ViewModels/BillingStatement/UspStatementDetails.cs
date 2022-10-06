using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.BillingStatement
{
    public class UspStatementDetails
    {
        public int Id { get; set; }
        public string BillingStatementId { get; set; }
        public string StatementNumber { get; set; }
        public DateTimeOffset StatementDate { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public DateTimeOffset DueDate { get; set; }
        public int PaymentTermId { get; set; }
        public bool IsActive { get; set; }
        public int PaymentNetDays { get; set; }
        public int VersionNumber{ get; set; }
        public int CustomerId { get; set; }
        public string CustomerCompany { get; set; }
        public string CompanyAddress { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public string ZipCode { get; set; }
        public string ContactPerson { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public decimal TotalStatementValue { get; set; }
        public string SupplierCompany { get; set; }
        public string SupplierAddress { get; set; }
        public string SupplierCity { get; set; }
        public string SupplierStateCode { get; set; }
        public string SupplierZipCode { get; set; }
        public string SupplierPhoneNumber { get; set; }
        public byte[] CompanyLogo { get; set; }
        public Currency currency { get; set; }
    }
}
