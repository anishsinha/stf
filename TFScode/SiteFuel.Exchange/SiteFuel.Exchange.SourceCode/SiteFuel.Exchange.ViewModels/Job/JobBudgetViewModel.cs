using Foolproof;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.CustomAttributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class JobBudgetViewModel : BaseViewModel
    {
        public JobBudgetViewModel()
        {
            InstanceInitialize();
        }

        public JobBudgetViewModel(Status status) : base(status)
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            BudgetCalculationTypeId = (int)BudgetCalculationType.NoBudget;
            ExchangeRate = 1;
            Currency = Currency.USD;
            UoM = UoM.Gallons;
        }

        public int Id { get; set; }

        [Display(Name = nameof(Resource.lblBudgetCalculation), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public int BudgetCalculationTypeId { get; set; }

        public Nullable<int> BudgetTypeId { get; set; }

        [DataType(DataType.Currency, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        [Display(Name = nameof(Resource.lblBudgetCalculation), ResourceType = typeof(Resource))]
        [RequiredIf("BudgetCalculationTypeId", (int)BudgetCalculationType.Budget, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public decimal Budget { get; set; }

        [Display(Name = nameof(Resource.lblGallons), ResourceType = typeof(Resource))]
        [RequiredIf("BudgetCalculationTypeId", (int)BudgetCalculationType.Fuel, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public decimal Gallons { get; set; }

        [Display(Name = nameof(Resource.lblPricePerGallon), ResourceType = typeof(Resource))]
        [DataType(DataType.Currency, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        [RequiredIf("BudgetCalculationTypeId", (int)BudgetCalculationType.Fuel, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public decimal PricePerGallon { get; set; }

        [Display(Name = nameof(Resource.lblTrackBudget), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public bool IsBudgetTracked { get; set; }

        [RequiredIfTrue("IsBudgetTracked", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public bool IsHedgeAmountTracked { get; set; }

        [Display(Name = nameof(Resource.lblHedge), ResourceType = typeof(Resource))]
        [DataType(DataType.Currency, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        [RequiredIfTrue("IsHedgeAmountTracked", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public decimal HedgeAmount { get; set; }

        [Display(Name = nameof(Resource.lblSpot), ResourceType = typeof(Resource))]
        [DataType(DataType.Currency, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        [RequiredIfFalse("IsHedgeAmountTracked", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public decimal SpotAmount { get; set; }

        [Display(Name = nameof(Resource.lblExceededBudget), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public bool IsExceededBudget { get; set; }

        [Display(Name = nameof(Resource.lblTaxExempted), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public bool IsTaxExempted { get; set; }

        [Display(Name = nameof(Resource.lblDropPictureRequired), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public bool IsDropPictureRequired { get; set; }

        [Display(Name = nameof(Resource.lblEnableAssetTracking), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public bool IsAssetTracked { get; set; }

        [Display(Name = nameof(Resource.lblEnableAssetTracking), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public bool IsAssetDropStatusEnabled { get; set; }

        public Currency Currency { get; set; }

        public decimal ExchangeRate { get; set; }

        public UoM UoM { get; set; }
    }
}
