namespace SiteFuel.Exchange.DataAccess.Entities
{
    using SiteFuel.Exchange.Utilities;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class FuelRequestDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int FuelRequestId { get; set; }

        public int DeliveryTypeId { get; set; }

        public DateTimeOffset StartDate { get; set; }

        public Nullable<System.DateTimeOffset> EndDate { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public PaymentMethods PaymentMethod { get; set; }

        public virtual FuelRequest FuelRequest { get; set; }

        public virtual MstDeliveryType MstDeliveryType { get; set; }

        public Nullable<int> PoContactId { get; set; }

        [ForeignKey("PoContactId")]
        public virtual User PoContactUser { get; set; }

        public string CustomAttribute { get; set; }

        public bool IsDropImageRequired { get; set; }

        public bool IsBolImageRequired { get; set; }

        public bool IsDriverToUpdateBOL { get; set; }

        public bool IsDispatchRetainedByCustomer { get; set; }

        public int? PricingQuantityIndicatorTypeId { get; set; }

        public int TruckLoadTypeId { get; set; }
        public OrderEnforcement OrderEnforcementId { get; set; }
        public bool IsPrePostDipRequired { get; set; }
    }
}
