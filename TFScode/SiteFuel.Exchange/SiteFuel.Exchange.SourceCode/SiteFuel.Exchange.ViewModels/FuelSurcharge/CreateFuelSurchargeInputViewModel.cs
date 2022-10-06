using Foolproof;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class CreateFuelSurchargeInputViewModel : BaseViewModel
    {
        public CreateFuelSurchargeInputViewModel()
        {
            BuyerCompanyIds = new List<int>();
            SurchargeTable = new List<FuelSurchargeTableViewModel>();
        }
        public int Id { get; set; }

        [Display(Name = nameof(Resource.lblTableType), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public TableTypes TableType { get; set; } = TableTypes.Master;

        [Display(Name = nameof(Resource.lblBuyerCompanyName), ResourceType = typeof(Resource))]
        [RequiredIf("TableType", (int)TableTypes.CustomerSpecific, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public List<int> BuyerCompanyIds { get; set; }

        [Display(Name = nameof(Resource.lblProductType), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public SurchargeProductTypes ProductType { get; set; } = SurchargeProductTypes.Gasoline;

        [Display(Name = nameof(Resource.lblStartDate), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public DateTimeOffset StartDate { get; set; }

        [Display(Name = nameof(Resource.lblEndDate), ResourceType = typeof(Resource))]
        [GreaterThanOrEqualTo("StartDate", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageGreaterThan), PassOnNull = true)]
        public DateTimeOffset? EndDate { get; set; }

        [Display(Name = nameof(Resource.lblPriceRangeStartValue), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [LessThanOrEqualTo("PriceRangeEndValue", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageGreaterThan), PassOnNull = true)]
        public decimal PriceRangeStartValue { get; set; }

        [Display(Name = nameof(Resource.lblPriceRangeEndValue), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Range(typeof(Decimal), ApplicationConstants.DecimalMinValue, ApplicationConstants.DecimalMaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        [GreaterThanOrEqualTo("PriceRangeStartValue", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageGreaterThan), PassOnNull = true)]
        public decimal PriceRangeEndValue { get; set; }

        [Display(Name = nameof(Resource.lblPriceRangeInterval), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Range(typeof(Decimal), "0", ApplicationConstants.DecimalMaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        public decimal PriceRangeInterval { get; set; }

        [Display(Name = nameof(Resource.lblFuelSurchargeStartPercentage), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Range(typeof(Decimal), ApplicationConstants.DecimalMinValue, ApplicationConstants.DecimalMaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        public decimal FuelSurchargeStartPercentage { get; set; }

        [Display(Name = nameof(Resource.lblSurchargeInterval), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Range(typeof(Decimal), ApplicationConstants.DecimalMinValue, ApplicationConstants.DecimalMaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        public decimal SurchargeInterval { get; set; }

        public List<FuelSurchargeTableViewModel> SurchargeTable { get; set; }
    }
}
