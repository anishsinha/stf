using Foolproof;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.FilldService;
using SiteFuel.Exchange.ViewModels.Forcasting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace SiteFuel.Exchange.ViewModels
{
    public partial class OnboardingPreferenceViewModel : StatusViewModel
    {
        public OnboardingPreferenceViewModel()
        {
        }

        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int UserId { get; set; }
        public bool IsBuyerReceiptEnabled { get; set; }
        public bool IsExceptionEnabled { get; set; }
        public bool IsUnscheduledDeliveryAllowed { get; set; } = true;
        public bool IsUnscheduledPickupAllowed { get; set; } = true;
        public OnboardingPreferencePricingMethod PreferencePricingMethod { get; set; } = OnboardingPreferencePricingMethod.Multiple;
        public bool IsCreditCheckEnabled { get; set; }
        public CreditCheckTypes CreditCheckType { get; set; } = CreditCheckTypes.None;
        public bool IsFtl { get; set; } = true;
        public FreightOnBoardTypes FreightOnBoardType { get; set; } = FreightOnBoardTypes.Destination;
        public bool IsBuySellEnabled { get; set; }
        public bool IsDropTicketImageRequired { get; set; }
        public bool IsThirdPartyHardwareUsed { get; set; }

        public DipTestMethod DipTestMethod { get; set; }
        public LocationManagedType LocationManagedType { get; set; } = LocationManagedType.NotSpecified;
        public bool IsSuppressOrderNotifications { get; set; }
        public bool IsCustomerInvitationEnabled { get; set; }
        public bool IsSupressOrderPricing { get; set; }
        public bool IsFreightOnlyOrderEnabled { get; set; }
        public bool AllowChatWithDrivers { get; set; }
        public int CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsByPassPreferencesSetting { get; set; } = true;
        public List<BuyerXOnboardingPreferenceViewModel> BuyersToSendReceipts { get; set; }
        public bool IsDriverProdutDisplayEnable { get; set; }
        public bool IsCustomUnScheduleDelivery { get; set; }

        public string ShiftStartTime { get; set; } = "08:00 AM";

        public string ShiftEndTime { get; set; } = "05:00 PM";
        public bool IsBrandMyWebsite { get; set; }
        
        public string BackgroundColor { get; set; }
        public string ForegroundColor { get; set; }
        public string IconColor { get; set; }
        public string FontColor { get; set; }
        public string HeaderColor { get; set; }
        public string ButtonColor { get; set; }
        [Display(Name = nameof(Resource.lblUploadLogo), ResourceType = typeof(Resource))]
        public string ImageFilePath { get; set; }
        public string hdnImageFilePath { get; set; }

        [Display(Name = nameof(Resource.lblUploadFavicon), ResourceType = typeof(Resource))]
        public string FaviconFilePath { get; set; }
        public string hdnfaviconFilePath { get; set; }

        [Display(Name = nameof(Resource.lblUploadCarrierOnboarding), ResourceType = typeof(Resource))]
        public string CarrierOnboardingImageFilePath { get; set; }
        public string hdnCarrierOnboardingImageFilePath { get; set; }

        [Display(Name = nameof(Resource.lblURLName), ResourceType = typeof(Resource))]
        public string URLName { get; set; }

        public bool IsRemoved { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Range(0, Int32.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        public int NotificationPeriod { get; set; } = ApplicationConstants.DefaultNotificationPeriod;

        [Display(Name = nameof(Resource.headingDeliveryDays), ResourceType = typeof(Resource))]
        public List<int> DeliveryDays { get; set; }
        public string DeliveryDaysInString { get; set; } = "1,2,3,4,5,6,7" ;
        public List<int> DeliveryDaysInList { get; set; }

        [Display(Name = nameof(Resource.lblUploadBackgroundImage), ResourceType = typeof(Resource))]
        public string BackgroundImageFilePath { get; set; }
        public string hdnBackgroundImageFilePath { get; set; }
        public bool IsBadgeMandatory { get; set; }
        [Display(Name = nameof(Resource.lblEbolWorkflow), ResourceType = typeof(Resource))]
        public bool IsEbolWorkflowEnabled { get; set; }
        public bool IsLiftFileValidationEnabled { get; set; }
        public bool IsShowProductDescriptionOnInvoice { get; set; }
        public LfvParameterViewModel LfvInputParameter { get; set; } = new LfvParameterViewModel();
        public LfvParameterViewModel LfvOutputParameter { get; set; } = new LfvParameterViewModel();
        public bool IsProductSequencingEnabled { get; set; }
        public ProductSequenceViewModel ProductSequencing { get; set; } = new ProductSequenceViewModel();
        public string ProductSequencingJson { get; set; }
        public ForcastingPreferenceViewModel ForcastingPreference { get; set; } = new ForcastingPreferenceViewModel();
        public List<int> LoadQueueAttributes { get; set; }
        public List<int> DRQueueAttributes { get; set; }
        public string LoadQueueAttributesValue { get; set; }
        public string DRQueueAttributesValue { get; set; }
        public FilldSettingsViewModel FilldConfigurations { get; set; } = new FilldSettingsViewModel();
        public int RetainThreshold { get; set; }
        public int UOM { get; set; }         
        public string RetainThresholdValueConvertion { get; set; }
        public bool IsDSBDriverSchedule { get; set; }

        public bool IsSendInventoryExportEmail { get; set; }
        public bool IsCarrierTileEmailNotification { get; set; }
        public List<int> CarrierUsers { get; set; } = new List<int>();
        public int? SelectedIdentityProvider { get; set; }
        public bool IsSelectedIdentityProvider { get; set; }
        public FreightPricingMethod FreightPricingMethod { get; set; } = FreightPricingMethod.Manual;
        public bool IsThirdPartyInvitationEnabled { get; set; }
        public PaymentDueDateType PaymentDueDateType { get; set; } = PaymentDueDateType.InvoiceCreationDate;

        public bool IsAdditiveBlendingEnabled { get; set; }
        public bool IsReasonCodesEnabled { get; set; }
        public bool IsLoadOptimization { get; set; }
        public List<int> LoadOptimizationUsers { get; set; }
    }

    public class LfvParameterViewModel
    {
        public bool IsTerminalCodeReq { get; set; } = true;
        public bool IsBolReq { get; set; } = true;
        public bool IsCINReq { get; set; }
        public bool IsCarrierNameReq { get; set; }
        public bool IsLoadDateReq { get; set; }
        public bool IsTermItemCodeReq { get; set; } = true;
        public bool IsCorrectedQtyRes { get; set; }
        public bool IsGrossReq { get; set; }
        public bool IsNeedToTruncateLeadingZeros { get; set; }

        public int DaysToContinueMatchProcess { get; set; } = ApplicationConstants.DefaultNoMatchRecordDays;

        public bool IsIgnoreSelfHauling { get; set; }

        public bool IsReplacePoWithAccoutingId { get; set; }

        public bool IsIgnoreWholesaleBadge { get; set; }

        public bool IsIgnoreNonRegisteredCarriers { get; set; }

        public bool IsIgnoreQuebecBillingBadges { get; set; }

    }
}
