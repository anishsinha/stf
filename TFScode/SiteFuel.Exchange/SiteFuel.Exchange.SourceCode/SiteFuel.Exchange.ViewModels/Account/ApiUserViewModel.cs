using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class ApiUserViewModel
    {
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        [StringLength(256)]
        public string Email { get; set; }
        [Required]
        [StringLength(256)]
        public string UserName { get; set; }
        public int? CompanyId { get; set; }
        public bool IsActive { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [StringLength(100, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 8)]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])[\w~@#$%^&*+=`|{}:;!.?\()\[\]-]{8,}$", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valPasswordValidate))]
        [Display(Name = nameof(Resource.lblPassword), ResourceType = typeof(Resource))]
        public string PlainPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }


    }
}
