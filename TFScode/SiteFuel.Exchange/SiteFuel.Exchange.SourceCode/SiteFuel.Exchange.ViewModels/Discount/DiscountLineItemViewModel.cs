using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class DiscountLineItemViewModel
    {
        public DiscountLineItemViewModel()
        {
            Currency = Currency.USD;
        }

        public int Id { get; set; }

        [Display(Name = nameof(Resource.lblDiscountOn), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public int FeeTypeId { get; set; }

        [Display(Name = nameof(Resource.lblRebateOption), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public int FeeSubTypeId { get; set; }

        [Display(Name = nameof(Resource.lblAmountOrPercent), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public decimal Amount { get; set; }

        public string FeeDetails { get; set; }
        
        public Currency Currency { get; set; }

        public int InvoiceId { get; set; }

        public string FeeTypeName { get; set; }

        public string FeeSubTypeName { get; set; }

        public decimal TotalFee { get; set; }

        public int? OtherFeeTypeId { get; set; }
    }
}
