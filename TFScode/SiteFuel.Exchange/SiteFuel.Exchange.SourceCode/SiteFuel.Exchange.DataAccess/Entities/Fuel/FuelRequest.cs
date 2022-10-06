namespace SiteFuel.Exchange.DataAccess.Entities
{
    using SiteFuel.Exchange.Utilities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class FuelRequest
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FuelRequest()
        {
            AssetDropRequests = new HashSet<AssetDropRequest>();
            CounterOffers = new HashSet<CounterOffer>();
            FuelRequests1 = new HashSet<FuelRequest>();
            FuelRequestXStatuses = new HashSet<FuelRequestXStatus>();
            Orders = new HashSet<Order>();
            DeliverySchedules = new HashSet<DeliverySchedule>();
            DifferentFuelPrices = new HashSet<DifferentFuelPrice>();
            FuelRequestFees = new HashSet<FuelFee>();
            PaymentDiscounts = new HashSet<PaymentDiscount>();
            PrivateSupplierLists = new HashSet<PrivateSupplierList>();
            ResaleCustomers = new HashSet<ResaleCustomer>();
            Resales = new HashSet<Resale>();
            SpecialInstructions = new HashSet<SpecialInstruction>();
            MstSupplierQualifications = new HashSet<MstSupplierQualification>();
            Users = new HashSet<User>();
            FuelRequestCurrentCosts = new HashSet<FuelRequestCurrentCost>();
            Quotations = new HashSet<Quotation>();
            Currency = Currency.USD;
            UoM = UoM.Gallons;
            ExchangeRate = 1;
        }

        public int Id { get; set; }

        public int FuelTypeId { get; set; }

        public int QuantityTypeId { get; set; }

        public decimal MinQuantity { get; set; }

        public decimal MaxQuantity { get; set; }

        public int PricingTypeId { get; set; }

        public bool IsOverageAllowed { get; set; }

        public decimal OverageAllowedAmount { get; set; }

        public int PaymentTermId { get; set; }

        public int NetDays { get; set; }

        public int OrderTypeId { get; set; }

        public decimal HedgeDroppedGallons { get; set; }

        public decimal HedgeDroppedAmount { get; set; }

        public decimal SpotDroppedGallons { get; set; }

        public decimal SpotDroppedAmount { get; set; }

        public bool IsPublicRequest { get; set; }

        public bool IsPaymentDiscounted { get; set; }

        public bool IsFuelAlreadyResubmitted { get; set; }

        public int CreatedBy { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public Nullable<DateTimeOffset> ExpirationDate { get; set; }

        public Nullable<int> OrderClosingThreshold { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [StringLength(11)]
        public string RequestNumber { get; set; }

        [StringLength(256)]
        public string ExternalPoNumber { get; set; }

        public Nullable<int> TerminalId { get; set; }

        public int FuelRequestTypeId { get; set; }

        public Nullable<int> ParentId { get; set; }

        public string Comment { get; set; }

        public bool IsActive { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        public int EstimateGallonsPerDelivery { get; set; }

        public decimal CreationTimeRackPPG { get; set; }

        public int ProductDisplayGroupId { get; set; }

        [StringLength(1024)]
        public string FuelDescription { get; set; }

        public int JobId { get; set; }

        public int? CityGroupTerminalId { get; set; }

        public decimal BasePrice { get; set; }

        public decimal BaseSpotDroppedQuantity { get; set; }

        public decimal BaseHedgeDroppedQuantity { get; set; }

        public decimal BaseSpotDroppedAmount { get; set; }

        public decimal BaseHedgeDroppedAmount { get; set; }

        public Currency Currency { get; set; }

        public decimal ExchangeRate { get; set; }

        public UoM UoM { get; set; }

        public Nullable<int> OfferPricingId { get; set; }

        public int? FreightOnBoardTypeId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AssetDropRequest> AssetDropRequests { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CounterOffer> CounterOffers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FuelRequest> FuelRequests1 { get; set; }

        public virtual FuelRequest FuelRequest1 { get; set; }

        public virtual MstExternalTerminal MstExternalTerminal { get; set; }

        public virtual MstFuelRequestType MstFuelRequestType { get; set; }

        public virtual MstOrderType MstOrderType { get; set; }

        public virtual MstPaymentTerm MstPaymentTerm { get; set; }

        public virtual MstPricingType MstPricingType { get; set; }

        public virtual MstProduct MstProduct { get; set; }

        public virtual MstQuantityType MstQuantityType { get; set; }

        //public virtual MstRackAvgPricingType MstRackAvgPricingType { get; set; }

        public virtual User User { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Quotation> Quotations { get; set; }

        public virtual FuelRequestDetail FuelRequestDetail { get; set; }

        public virtual FuelRequestPricingDetail FuelRequestPricingDetail { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FuelRequestXStatus> FuelRequestXStatuses { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliverySchedule> DeliverySchedules { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DifferentFuelPrice> DifferentFuelPrices { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FuelFee> FuelRequestFees { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PaymentDiscount> PaymentDiscounts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PrivateSupplierList> PrivateSupplierLists { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ResaleCustomer> ResaleCustomers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Resale> Resales { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SpecialInstruction> SpecialInstructions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MstSupplierQualification> MstSupplierQualifications { get; set; }

        [ForeignKey("JobId")]
        public virtual Job Job { get; set; }

        [ForeignKey("OfferPricingId")]
        public virtual OfferPricing OfferPricing { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User> Users { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FuelRequestCurrentCost> FuelRequestCurrentCosts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FuelRequestTankRentalFrequency> TankRentals { get; set; }
    }
}
