namespace SiteFuel.Exchange.DataAccess.Entities
{
    using SiteFuel.Exchange.Utilities;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class OrderTaxDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OrderTaxDetail()
        {
            Currency = Currency.USD;
            ExchangeRate = 1;
        }

        public int Id { get; set; }

        public int OrderId { get; set; }

        public int TaxPricingTypeId { get; set; }

        public string TaxDescription { get; set; }

        public decimal TaxRate { get; set; }

        public bool IsActive { get; set; }

        public int AddedBy { get; set; }

        public DateTimeOffset AddedDate { get; set; }

        public int AddedByCompanyId { get; set; }

        public int OtherFuelTypeId { get; set; }

        public decimal BaseTaxRate { get; set; }

        public Currency Currency { get; set; }

        public decimal ExchangeRate { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }
    }
}
