namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            AssetDrops = new HashSet<AssetDrop>();
            DeliveryScheduleXTrackableSchedules = new HashSet<DeliveryScheduleXTrackableSchedule>();
            EnrouteDeliveryHistories = new HashSet<EnrouteDeliveryHistory>();
            Invoices = new HashSet<Invoice>();
            Orders1 = new HashSet<Order>();
            OrderDeliverySchedules = new HashSet<OrderVersionXDeliverySchedule>();
            OrderDetailVersions = new HashSet<OrderDetailVersion>();
            OrderXDrivers = new HashSet<OrderXDriver>();
            OrderXStatuses = new HashSet<OrderXStatus>();
            Spills = new HashSet<Spill>();
            AppLocations = new HashSet<AppLocation>();
            TaxExemptLicenses = new HashSet<TaxExemptLicens>();
            ExternalDropDetails = new HashSet<ExternalDropDetail>();
            BillingScheduleXCustomerOrders = new HashSet<BillingScheduleXCustomerOrder>();
            FuelDispatchLocations = new HashSet<FuelDispatchLocation>();
            OrderGroupXOrders = new HashSet<OrderGroupXOrder>();
            OrderBadgeDetails = new HashSet<OrderBadgeDetail>();
            TermOrderGroupHistories = new HashSet<TermOrderGroupHistory>();
            ScheduleXBrokerOrderDetails = new HashSet<ScheduleXBrokerOrderDetail>();
            OrderXUsers = new HashSet<User>();
        }

        public int Id { get; set; }

        [StringLength(256)]
        public string Name { get; set; }
        public int FuelRequestId { get; set; }

        public int AcceptedCompanyId { get; set; }

        public int AcceptedBy { get; set; }

        public DateTimeOffset AcceptedDate { get; set; }

        public int DefaultInvoiceType { get; set; }

        public Nullable<int> TerminalId { get; set; }

        [StringLength(275)]
        public string PoNumber { get; set; }

        [StringLength(275)]
        public string TfxPoNumber { get; set; }

        public Nullable<int> ParentId { get; set; }

        public bool IsActive { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        public int BuyerCompanyId { get; set; }

        public bool IsEndSupplier { get; set; }

        public bool IsProFormaPo { get; set; }

        public bool SignatureEnabled { get; set; }

        public int? ExternalMeterServiceId { get; set; }

        public int? ExternalBrokerId { get; set; }

        public int? CityGroupTerminalId { get; set; }

        public bool IsFTL { get; set; }

        public int? LeadRequestId { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderGroupXOrder> OrderGroupXOrders { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AssetDrop> AssetDrops { get; set; }

        public virtual OrderAdditionalDetail OrderAdditionalDetail { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderBadgeDetail> OrderBadgeDetails { get; set; }

        [ForeignKey("AcceptedCompanyId")]
        public virtual Company Company { get; set; }

        [ForeignKey("BuyerCompanyId")]
        public virtual Company BuyerCompany { get; set; }

        [ForeignKey("ExternalBrokerId")]
        public virtual ExternalBroker ExternalBroker { get; set; }

        public virtual ExternalBrokerOrderDetail ExternalBrokerOrderDetail { get; set; }

        public virtual ExternalBrokerBuySellDetail ExternalBrokerBuySellDetail { get; set; }

        public virtual FuelRequest FuelRequest { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Invoice> Invoices { get; set; }

        public virtual MstExternalTerminal MstExternalTerminal { get; set; }

        public virtual MstInvoiceType MstInvoiceType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders1 { get; set; }

        public virtual Order Order1 { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<OrderVersionXDeliverySchedule> OrderDeliverySchedules { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetailVersion> OrderDetailVersions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FuelDispatchLocation> FuelDispatchLocations { get; set; }

        public virtual OrderXCancelationReason OrderXCancelationReason { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderXDriver> OrderXDrivers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderXStatus> OrderXStatuses { get; set; }

        public virtual OrderXTogglePricingDetail OrderXTogglePricingDetail { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Spill> Spills { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AppLocation> AppLocations { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EnrouteDeliveryHistory> EnrouteDeliveryHistories { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliveryScheduleXTrackableSchedule> DeliveryScheduleXTrackableSchedules { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaxExemptLicens> TaxExemptLicenses { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [ForeignKey("ExternalMeterServiceId")]
        public virtual MstExternalMeterService MstExternalMeterService { get; set; }

        [ForeignKey("LeadRequestId")]
        public virtual LeadRequest LeadRequest { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExternalDropDetail> ExternalDropDetails { get; set; }

        public decimal? BrokeredMaxQuantity { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderTaxDetail> OrderTaxDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BillingScheduleXCustomerOrder> BillingScheduleXCustomerOrders { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TermOrderGroupHistory> TermOrderGroupHistories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ScheduleXBrokerOrderDetail> ScheduleXBrokerOrderDetails { get; set; }
        public virtual ICollection<User> OrderXUsers { get; set; }
    }
}
