using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class UspGenerateStatement
    {
        public int Id { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public int FrequencyTypeId { get; set; }
        public int PaymentTermId { get; set; }
        public string TimeZoneName { get; set; }
        public int PaymentNetDays { get; set; }
        public decimal DroppedGallons { get; set; }
        public decimal BasicAmount { get; set; }
        public decimal TotalFeeAmount { get; set; }
        public decimal TotalTaxAmount { get; set; }
        public decimal TotalDiscountAmount { get; set; }
        public int CreatedBy { get; set; }
        public int InvoiceId { get; set; }
        public Currency Currency { get; set; }
        public UoM UoM { get; set; }
        public decimal ExchangeRate { get; set; }
        public int CreatedByCompanyId { get;set;}
    }
}
