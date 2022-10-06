using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Foolproof;

namespace SiteFuel.Exchange.ViewModels
{
    public class BrokeredOrderFeeViewModel : StatusViewModel
    {
        public BrokeredOrderFeeViewModel()
        {
            FeeTypeId = (int)FeeType.OtherFee;
        }

        public BrokeredOrderFeeViewModel(Status status) 
            : base(status)
        {
        }

        public int FeeTypeId { get; set; }

        public int FeeSubTypeId { get; set; }

        public string FeeSubTypeName { get; set; }

        [Display(Name = nameof(Resource.lblDescription), ResourceType = typeof(Resource))]
        public string FeeDetails { get; set; }

        public decimal Fee { get; set; }
    }
}
