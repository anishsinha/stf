using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Foolproof;
using Newtonsoft.Json;

namespace SiteFuel.Exchange.ViewModels
{
    public class FuelDeliveryDetailsViewModel : StatusViewModel
    {
        public FuelDeliveryDetailsViewModel()
        {
            InstanceInitialize();
        }

        public FuelDeliveryDetailsViewModel(Status status) : base(status)
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            FuelRequestFee = new FuelRequestFeeViewModel();
            DeliveryTypeId = (int)DeliveryType.OneTimeDelivery;
            StartDate = DateTimeOffset.Now;
            StartTime = Convert.ToDateTime("08:00").ToShortTimeString();
            EndTime = Convert.ToDateTime("17:00").ToShortTimeString();
            SpecialInstructions = new List<SpecialInstructionViewModel>();
            DeliverySchedules = new List<DeliveryScheduleViewModel>();
            PendingRequest = new List<DeliveryScheduleViewModel>();
            DeliveryHistory = new List<DeliveryHistoryViewModel>();
            FuelFees = new FuelFeesViewModel();
            OrderEnforcementId = (OrderEnforcement)OrderEnforcement.EnforceOrderLevelValues;
            IsPrePostDipRequired = false;
        }

        public int FuelRequestId { get; set; }

        public FuelRequestFeeViewModel FuelRequestFee { get; set; }

        [Display(Name = nameof(Resource.lblDeliveryType), ResourceType = typeof(Resource))]
        public int DeliveryTypeId { get; set; }

        [Display(Name = nameof(Resource.lblType), ResourceType = typeof(Resource))]
        public SingleDeliverySubTypes SingleDeliverySubTypes { get; set; }

        public string DeliveryTypeName { get; set; }

        public int? PoContactId { get; set; }

        [LessThanOrEqualTo("EndDate", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageLessThan), PassOnNull = true)]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblStartDate), ResourceType = typeof(Resource))]
        public DateTimeOffset StartDate { get; set; }

        public bool IsOrderEndDateRequired { get; set; }

        public bool IsDispatchRetainedByCustomer { get; set; }

        [RequiredIf("IsOrderEndDateRequired", true, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [GreaterThanOrEqualTo("StartDate", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageGreaterThan), PassOnNull = true)]
        [Display(Name = nameof(Resource.lblOrderEndDate), ResourceType = typeof(Resource))]
        public Nullable<DateTimeOffset> EndDate { get; set; }

        [LessThan("StartDate", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageLessThan), PassOnNull = true)]
        [Display(Name = nameof(Resource.lblFuelRequestExpireDate), ResourceType = typeof(Resource))]
        public Nullable<DateTimeOffset> ExpirationDate { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string StartTime { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string EndTime { get; set; }

        [Display(Name = nameof(Resource.lblPaymentMethod), ResourceType = typeof(Resource))]
        public PaymentMethods PaymentMethods { get; set; }

        public int OrderVersion { get; set; }

        public List<SpecialInstructionViewModel> SpecialInstructions { get; set; }

        public SpecialInstructionAttachmentViewModel SpecialInstructionFiles { get; set; } = new SpecialInstructionAttachmentViewModel();

        public List<DeliveryScheduleViewModel> DeliverySchedules { get; set; }

        public List<DeliveryScheduleViewModel> PendingRequest { get; set; }

        public List<DeliveryHistoryViewModel> DeliveryHistory { get; set; }

        public FuelFeesViewModel FuelFees { get; set; }

        public bool IsOrderView { get; set; }

        [Display(Name = nameof(Resource.lblNetGross), ResourceType = typeof(Resource))]
        public int? PricingQuantityIndicatorTypeId { get; set; }

        [Display(Name = nameof(Resource.lblType), ResourceType = typeof(Resource))]
        public TruckLoadTypes TruckLoadTypes { get; set; } = TruckLoadTypes.LessTruckLoad;

        public string CustomAttribute { get; set; }

        // this is needed to be initialised as we need initialised object when tpo, fr, etc
        public FuelRequestCustomAttributeViewModel CustomAttributeViewModel { get; set; } = new FuelRequestCustomAttributeViewModel();

        public bool IsBolImageRequired { get; set; }
        public bool IsDropImageRequired { get; set; }

        public bool IsDriverToUpdateBOL { get; set; }
        public string SiteInstructions { get; set; }
        public OrderEnforcement OrderEnforcementId { get; set; }
        public bool IsPrePostDipRequired { get; set; }

        public bool IsMarineLocation { get; set; }
    }

    public sealed class FuelRequestCustomAttributeViewModel
    {
        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 0)]
        [Display(Name = nameof(Resource.lblWBSNumber), ResourceType = typeof(Resource))]
        public string WBSNumber { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
