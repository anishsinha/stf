using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Foolproof;

namespace SiteFuel.Exchange.ViewModels
{
    public class AdditionalFeeViewModel : StatusViewModel
    {
        public AdditionalFeeViewModel()
        {
            InstanceInitialize();
        }

        public AdditionalFeeViewModel(Status status) 
            : base(status)
        {
            InstanceInitialize(status);
        }

        private void InstanceInitialize(Status status = Status.Failed)
        {
            FeeSubTypeId = (int)FeeSubType.FlatFee;
        }

        public int FeeTypeId { get; set; }

        [Display(Name = nameof(Resource.lblAdditionalFee), ResourceType = typeof(Resource))]
        public int FeeSubTypeId { get; set; }

        public string FeeSubTypeName { get; set; }

        [Display(Name = nameof(Resource.lblDescription), ResourceType = typeof(Resource))]
        public string FeeDetails { get; set; }

        [RequiredIf("FeeSubTypeId", (int)FeeSubType.FlatFee, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public decimal Fee { get; set; }

        public bool IncludeInPPG { get; set; }
    }
}
