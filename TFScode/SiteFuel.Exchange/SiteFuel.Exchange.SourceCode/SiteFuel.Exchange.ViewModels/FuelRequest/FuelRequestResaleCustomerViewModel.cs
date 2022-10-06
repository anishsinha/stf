using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Foolproof;
using SiteFuel.Exchange.ViewModels.CustomAttributes;

namespace SiteFuel.Exchange.ViewModels
{
    public class FuelRequestResaleCustomerViewModel : BaseViewModel
    {
        public FuelRequestResaleCustomerViewModel()
        {
           
        }

        public FuelRequestResaleCustomerViewModel(Status status) 
            : base(status)
        {
        }

        [EmailAddressEx(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        public string Email { get; set; }

        [Display(Name = nameof(Resource.lblCustomerName), ResourceType = typeof(Resource))]
        public string Name { get; set; }
    }
}
