using SiteFuel.Exchange.Core.StringResources;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class TerminalCardNumberViewModel
    {
        public string CollectionHtmlPrefix { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblTerminal), ResourceType = typeof(Resource))]
        public int? TerminalId { get; set; }
        public string TerminalName { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblCardNumber), ResourceType = typeof(Resource))]
        public string CardNumber { get; set; }
    }
}