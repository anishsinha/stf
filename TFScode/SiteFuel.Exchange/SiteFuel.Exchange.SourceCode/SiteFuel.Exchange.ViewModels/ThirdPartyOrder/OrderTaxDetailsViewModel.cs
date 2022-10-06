using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class OrderTaxDetailsViewModel : BaseViewModel
    {

        public OrderTaxDetailsViewModel()
        {
            Currency = Currency.USD;
        }

        public OrderTaxDetailsViewModel(Status status)
            : base(status)
        {
            Currency = Currency.USD;
        }

        public int Id { get; set; }

        //public int OrderId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMaximumLength), MinimumLength = 1)]
        [Display(Name = nameof(Resource.lblTaxDescription), ResourceType = typeof(Resource))]
        public string TaxDescription { get; set; }

        public string TaxPricingDescription { get; set; }

        public int TaxPricingTypeId { get; set; }

        public decimal TaxRate { get; set; }

        public int AddedBy { get; set; }

        public int AddedByCompanyId { get; set; }

        public int OtherFuelTypeId { get; set; }

        public Currency Currency { get; set; }

        public decimal ExchangeRate { get; set; }
    }
}
