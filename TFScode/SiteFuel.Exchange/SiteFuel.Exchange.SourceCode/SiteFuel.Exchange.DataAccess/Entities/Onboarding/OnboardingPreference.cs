namespace SiteFuel.Exchange.DataAccess.Entities
{
    using SiteFuel.Exchange.Utilities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class OnboardingPreference
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OnboardingPreference()
        {
            BuyerXOnboardingPreferences = new HashSet<BuyerXOnboardingPreference>();
            LiftFileValidationParameters = new HashSet<LiftFileValidationParameter>();
        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public bool IsBuyerReceiptEnabled { get; set; }
        public int CompanyId { get; set; }
        public int UserId { get; set; }
        public bool IsExceptionEnabled { get; set; }
        public bool IsUnscheduledDeliveryAllowed { get; set; }
        public bool IsUnscheduledPickupAllowed { get; set; }
        public OnboardingPreferencePricingMethod PreferencePricingMethod { get; set; }
        public TruckLoadTypes DeliveryType { get; set; }
        public FreightOnBoardTypes FreightOnBoardType { get; set; }
        public bool IsBuySellEnabled { get; set; }
        public CreditCheckTypes CreditCheckType { get; set; } = CreditCheckTypes.None;
        public bool IsDropTicketImageRequired { get; set; }
        public bool IsThirdPartyHardwareUsed { get; set; }
        public bool IsSuppressOrderNotifications { get; set; }
        public bool IsCustomerInvitationEnabled { get; set; }
        public bool IsSupressOrderPricing { get; set; }
        public bool IsFreightOnlyOrderEnabled { get; set; }
        public int CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDriverProdutDisplayEnable { get; set; }
        public bool IsCustomUnScheduleDelivery { get; set; }
        public TimeSpan ShiftStartTime { get; set; }
        public TimeSpan ShiftEndTime { get; set; }

        public bool IsBrandMyWebsite { get; set; }

        public string ImageFilePath { get; set; }

        public string URLName { get; set; }

        [StringLength(64)]
        public string BackgroundColor { get; set; }
        [StringLength(64)]
        public string ForegroundColor { get; set; }
        [StringLength(64)]
        public string IconColor { get; set; }
        [StringLength(64)]
        public string FontColor { get; set; }
        [StringLength(64)]
        public string HeaderColor { get; set; }
        [StringLength(64)]
        public string ButtonColor { get; set; }
        public int? NotificationPeriod { get; set; }
        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }
        public string DeliveryDays { get; set; }
       
        public DipTestMethod DipTestMethod { get; set; }
        public LocationManagedType LocationManagedType { get; set; }

        
        public string BackgroundImageFilePath { get; set; }
        public bool IsBadgeMandatory { get; set; }
        public bool IsShowProductDescriptionOnInvoice { get; set; }
        public bool AllowCustomerDriverChat { get; set; }

        public bool IsLiftFileValidationEnabled { get; set; }
        public bool IsEbolWorkflowEnabled { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BuyerXOnboardingPreference> BuyerXOnboardingPreferences { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LiftFileValidationParameter> LiftFileValidationParameters { get; set; }

        public bool IsSequencingEnabled { get; set; }
        public string LoadQueueAttributes { get; set; }
        public string DRQueueAttributes { get; set; }
        public int RetainThreshold { get; set; }
        public int UOM { get; set; }
        public bool IsDSBDriverSchedule { get; set; }

        public string FaviconImageFilePath { get; set; }

        public bool IssendInventoryExportEmail { get; set; }

        public bool IsCarrierTileEmailNotification { get; set; }
        public FreightPricingMethod FreightPricingMethod { get; set; }
        public bool IsThirdPartyInvitationEnabled { get; set; }
        public PaymentDueDateType PaymentDueDateType { get; set; }

        public bool IsAdditiveBlendingEnabled { get; set; }

        public bool IsAssignReasonCodeEnabled { get; set; }

        public string CarrierOnboardingImageFilePath { get; set; }
        public bool IsLoadOptimization { get; set; }
    }
}
