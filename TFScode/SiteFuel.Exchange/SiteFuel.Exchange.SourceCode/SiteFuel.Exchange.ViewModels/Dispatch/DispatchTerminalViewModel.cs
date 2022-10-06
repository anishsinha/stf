using Foolproof;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.CustomAttributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class DispatchTerminalViewModel
    {
        [Display(Name = nameof(Resource.lblTerminal), ResourceType = typeof(Resource))]
        [RequiredIfFalse("IsNewLocation", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public int? TerminalId { get; set; }

        public string TerminalName { get; set; }

        public bool IsNewLocation { get; set; }

        public DispatchAddressViewModel AddressDetails { get; set; } = new DispatchAddressViewModel();

        public Currency Currency { get; set; } = Currency.USD;
    }

    public class CarrierViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CreatedBy { get; set; }

        public int CompanyId { get; set; }
    }    
}
