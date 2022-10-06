using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class PaymentViewModel : StatusViewModel
    {
        public PaymentViewModel()
        {
            InstanceInitialize();
        }

        public PaymentViewModel(Status status)
            : base(status)
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            Card = new PaymentCardViewModel();
            State = new StateViewModel();
            Country = new CountryViewModel();
        }

        public int Id { get; set; }

        public int UserId { get; set; }

        public int CompanyId { get; set; }

        public string CardToken { get; set; }

        public string AddedBy { get; set; }

        public PaymentCardViewModel Card { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 1)]
        [Display(Name = nameof(Resource.lblAddress), ResourceType = typeof(Resource))]
        //[Remote("IsValidAddress", "Validation", AreaReference.UseRoot, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        public string Address { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [StringLength(128, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 1)]
        [Display(Name = nameof(Resource.lblCity), ResourceType = typeof(Resource))]
        public string City { get; set; }

        public StateViewModel State { get; set; }

        public CountryViewModel Country { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblZipcode), ResourceType = typeof(Resource))]        
        public string ZipCode { get; set; }
    }
}
