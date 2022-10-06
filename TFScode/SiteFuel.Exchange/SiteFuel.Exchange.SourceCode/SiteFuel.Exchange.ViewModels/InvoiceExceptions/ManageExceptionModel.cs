using Foolproof;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.InvoiceExceptions
{
    public class ManageExceptionModel : StatusViewModel
    {
        public int UserId { get; set; }
        public int OwnerCompanyId { get; set; }
        public List<CompanyExceptionModel> Exceptions { get; set; }
    }

    public class CompanyExceptionModel
    {
        public int TypeId { get; set; }
        public string TypeName { get; set; }
        [Display(Name = nameof(Resource.lblExceptionApprover), ResourceType = typeof(Resource))]
        [RequiredIfTrue("IsActive", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public int? ApproverId { get; set; }
        [Display(Name = nameof(Resource.lblBusinessDays), ResourceType = typeof(Resource))]
        [RequiredIfTrue("IsActive", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageGreaterThanZero))]
        public int? AutoApprovalDays { get; set; }
        public int? DelayInvoiceCreationTime { get; set; }
        [RequiredIfTrue("IsActive", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public decimal? Threshold { get; set; }
        public bool IsActive { get; set; }
        public List<ListItem> Approvers { get; set; }
        public List<ListItem> Resolutions { get; set; }
    }
}
