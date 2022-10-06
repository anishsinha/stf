using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class BuyerAppFuelRequestViewModel
    {
        public BuyerAppFuelRequestViewModel()
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            IsPublicRequest = true;
            FuelRequestFee = new FuelRequestFeeViewModel();
            DeliveryTypeId = (int)DeliveryType.OneTimeDelivery;
            StartDate = DateTimeOffset.Now;
            StartTime = Convert.ToDateTime("08:00").ToShortTimeString();
            EndTime = Convert.ToDateTime("17:00").ToShortTimeString();
            SpecialInstructions = new List<SpecialInstructionViewModel>();
            DeliverySchedules = new List<DeliveryScheduleViewModel>();
            PaymentTermId = (int)PaymentTerms.NetDays;
            FuelFees = new FuelFeesViewModel();
            PricingSourceId = (int)PricingSource.Axxis;

            //Yet Not in Screen
            IsOverageAllowed = false;
            OrderTypeId = (int)OrderType.Spot;
            CloseOrderId = (int)CloseOrderType.OnCompleted;
            SupplierQualifications = new List<int>();
        }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageGreaterThanZero))]
        public int UserId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageGreaterThanZero))]
        public int CompanyId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageGreaterThanZero))]
        public int JobId { get; set; }

        public bool IsPublicRequest { get; set; }

        public int? PrivateSupplierListId { get; set; }

        public string ExternalPoNumber { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageGreaterThanZero))]
        public int FuelTypeId { get; set; }

        public int QuantityTypeId { get; set; }

        public decimal MinimumQuantity { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageGreaterThanZero))]
        public decimal MaximumQuantity { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageGreaterThanZero))]
        public int PricingTypeId { get; set; }

        public Nullable<int> RackAvgTypeId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Range(0.00001, double.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageGreaterThanZero))]
        public decimal PricePerGallon { get; set; }

        public FuelRequestFeeViewModel FuelRequestFee { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageGreaterThanZero))]
        public int DeliveryTypeId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public DateTimeOffset StartDate { get; set; }

        public Nullable<DateTimeOffset> EndDate { get; set; }

        public Nullable<DateTimeOffset> ExpirationDate { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string StartTime { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string EndTime { get; set; }

        public List<SpecialInstructionViewModel> SpecialInstructions { get; set; }

        public List<DeliveryScheduleViewModel> DeliverySchedules { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageGreaterThanZero))]
        public int PaymentTermId { get; set; }

        public int NetDays { get; set; }

        public bool IsCityRackTerminal { get; set; }

        //Yet Not in Screen
        public bool IsOverageAllowed { get; set; }

        public decimal OverageAllowedPercent { get; set; }

        public int OrderTypeId { get; set; }

        public int CloseOrderId { get; set; }

        public Nullable<int> OrderClosingThreshold { get; set; }

        public List<int> SupplierQualifications { get; set; }

        public int TPOSupplierId { get; set; }

        public FuelFeesViewModel FuelFees { get; set; }

        public int PricingSourceId { get; set; }

        public int? FeedTypeId { get; set; }

        public int? PricingQuantityIndicatorTypeId { get; set; }

        public int? FuelClassId { get; set; }

        public int? TruckLoadTypeId { get; set; }

        public int? FobTypeId { get; set; }

        public int? FrQuantityIndicatorTypeId { get; set; }  // only with FTL (Net/Gross)
    }
}
