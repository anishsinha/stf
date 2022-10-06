namespace SiteFuel.Exchange.DataAccess.Entities
{
    using SiteFuel.Exchange.Utilities;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class OrderAdditionalDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OrderId { get; set; }

        public int? CarrierId { get; set; }

        public int? BOLInvoicePreferenceId { get; set; }

        public decimal? Allowance { get; set; }

        public bool IsDriverToUpdateBOL { get; set; }

        public bool IsFuelSurcharge { get; set; }

        public bool IsFreightCost { get; set; }

        public int? SupplierSourceId { get; set; }

        public int? FuelSurchagePricingType { get; set; }

        [StringLength(100)]
        public string SupplierContract { get; set; }

        [StringLength(100)]
        public string LoadCode { get; set; }

        [ForeignKey("SupplierSourceId")]
        public virtual SupplierSource SupplierSource { get; set; }

        [ForeignKey("CarrierId")]
        public virtual Carrier Carrier { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }

        public string CustomAttribute { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }

        [StringLength(1024)]
        public string DRNotes { get; set; }

        public string FileDetails { get; set; }

        public int? PreferencesSettingId { get; set; }

        public LocationManagedType LocationManagedType { get; set; }

        public bool IsIncludePricingInExternalObj { get; set; }

        [StringLength(256)]
        public string Berth { get; set; }

        public DestinedForInternationalWaters InternationalWatersType { get; set; } = DestinedForInternationalWaters.Unknown;

        [ForeignKey("PreferencesSettingId")]
        public virtual OnboardingPreference OnboardingPreference { get; set; }

        public FreightPricingMethod FreightPricingMethod { get; set; } = FreightPricingMethod.Manual;

        public bool IsManualBDNConfirmationRequired { get; set; }

        public bool IsManualInvoiceConfirmationRequired { get; set; }

        public bool IsSupressPricingEnabled { get; set; }

        public bool IsPDITaxRequired { get; set; }

        public FreightRateRuleType? FreightRateRuleType { get; set; }

        public int? FreightRateRuleId { get; set; }

        public TableTypes? FreightRateTableType { get; set; }

        public int? FuelSurchargeTableId { get; set; }

        public TableTypes? FuelSurchargeTableType { get; set; }

        public int? AccessorialFeeId { get; set; }

        public TableTypes? AccessorialFeeTableType { get; set; }

    }
}
