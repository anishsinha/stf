using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class DiscountViewModel
    {
        public DiscountViewModel()
        {
            DiscountLineItems = new List<DiscountLineItemViewModel>();
            CreatedDate = DateTimeOffset.Now;
            DealStatus = (int)Utilities.DealStatus.Pending;
        }

        [Display(Name = nameof(Resource.lblDealName), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Remote("IsValidDealName", "Validation", AreaReference.UseRoot, AdditionalFields = "InvoiceId", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageAlreadyExist))]
        public string DealName { get; set; }

        public List<DiscountLineItemViewModel> DiscountLineItems  { get; set; }

        [Display(Name = nameof(Resource.lblNotes), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string Notes { get; set; }

        public int OrderId { get; set; }

        public int InvoiceId { get; set; }

        public int DealStatus { get; set; }

        public int CreatedBy { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public int CreatedCompanyId { get; set; }
        public int? StatusChangedBy { get; set; }
        public int? StatusChangedCompanyId { get; set; }
        public DateTimeOffset? StatusChangedDate { get; set; }
    }
}
