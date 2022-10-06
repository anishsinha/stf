using Foolproof;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class SupplierProfileViewModel : StatusViewModel
    {
        public SupplierProfileViewModel()
        {
            ServingStates = new List<int>();
            SupplierQualifications = new List<int>();
            IsStateWideService = true;
            Radius = 0;
            IsLocationOwned = true;
            IsHedgeOrderAllowed = true;
            IsOverWaterRefuelingAllowed = true;
        }

        public SupplierProfileViewModel(Status status) 
            : base(status)
        {
            ServingStates = new List<int>();
            SupplierQualifications = new List<int>();
            IsStateWideService = true;
            Radius = 0;
            IsLocationOwned = true;
            IsHedgeOrderAllowed = true;
            IsOverWaterRefuelingAllowed = true;
        }

        public int AddressId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblServingStates), ResourceType = typeof(Resource))]
        public List<int> ServingStates { get; set; }

        [Display(Name = nameof(Resource.lblSupplierQualifications), ResourceType = typeof(Resource))]
        public List<int> SupplierQualifications { get; set; }

        public bool IsStateWideService { get; set; }

        [RequiredIfFalse("IsStateWideService", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public int Radius { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblIsLocationOwned), ResourceType = typeof(Resource))]
        public bool IsLocationOwned { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblHedgeOrderAllowed), ResourceType = typeof(Resource))]
        public bool IsHedgeOrderAllowed { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblWaterRefuelingAllowed), ResourceType = typeof(Resource))]
        public bool IsOverWaterRefuelingAllowed { get; set; }
    }
}
