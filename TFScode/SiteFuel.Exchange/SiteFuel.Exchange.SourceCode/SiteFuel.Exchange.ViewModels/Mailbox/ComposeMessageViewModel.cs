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
    public class ComposeMessageViewModel : ResponseViewModel
    {
        public ComposeMessageViewModel()
            : base(Status.Failed)
        {
            Recipients = new List<int>();
            RecipientCompanies = new List<int>();
        }

        public ComposeMessageViewModel(Status status)
            : base(status)
        {
            Recipients = new List<int>();
            RecipientCompanies = new List<int>();
        }

        public int Id { get; set; }

        public AppMessageComposeType ComposeType { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblType), ResourceType = typeof(Resource))]
        public AppMessageQueryType Type { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblNumber), ResourceType = typeof(Resource))]
        public int Number { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblRecipients), ResourceType = typeof(Resource))]
        public List<int> Recipients { get; set; }

        public List<int> RecipientCompanies { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblSubject), ResourceType = typeof(Resource))]
        public string Subject { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblMessages), ResourceType = typeof(Resource))]
        public string Message { get; set; }

        public string PlainTextMessage { get; set; }

        public bool IsDraft { get; set; }

        public DateTimeOffset TimeStamp { get; set; }
    }
}
