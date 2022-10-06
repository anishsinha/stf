namespace SiteFuel.Exchange.DataAccess.Entities
{
    using SiteFuel.Exchange.Utilities;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class ExternalBrokerBuySellDetail
    {
        public ExternalBrokerBuySellDetail()
        {
            Currency = Currency.USD;
        }

        [Key]
        public int OrderId { get; set; }

        public int ExternalBrokerId { get; set; }

        public decimal BrokerMarkUp { get; set; }

        public decimal SupplierMarkUp { get; set; }

        public bool IsActive { get; set; }

        public Currency Currency { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }
    }
}
