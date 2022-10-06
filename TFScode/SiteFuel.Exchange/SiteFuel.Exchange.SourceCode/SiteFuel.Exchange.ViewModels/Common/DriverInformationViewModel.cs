using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace SiteFuel.Exchange.ViewModels
{
    public class DriverInformationViewModel
    {
        public HttpPostedFileBase ProfilePhoto { get; set; }
        public string ProfilePhotoUrl { get; set; }
        [Display(Name = nameof(Resource.lblLicenseNumber), ResourceType = typeof(Resource))]
        public string LicenseNumber { get; set; }
        [Display(Name = nameof(Resource.lblShiftInformation), ResourceType = typeof(Resource))]
        public List<string> ShiftId { get; set; }
        public string ShiftName { get; set; }

       
        [Display(Name = nameof(Resource.lblTrailerType), ResourceType = typeof(Resource))]
        public List<TrailerTypeStatus> TrailerType { get; set; }

        [Display(Name = nameof(Resource.lblCompanyName), ResourceType = typeof(Resource))]
        public string CompanyName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblLicenseExpiryDate), ResourceType = typeof(Resource))]
        public string ExpiryDate { get; set; }

        [Display(Name = nameof(Resource.lblLicenseType), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string LicenseTypeId { get; set; }
        [Display(Name = nameof(Resource.lblAssignedRegion), ResourceType = typeof(Resource))]
        public List<string> Regions { get; set; }
        [Display(Name = nameof(Resource.lblShiftPreference), ResourceType = typeof(Resource))]
        public List<TerminalCardNumberViewModel> CardNumbers { get; set; } = new List<TerminalCardNumberViewModel>();

        public bool IsFilldAuthorized { get; set; }

    }
}
