using Foolproof;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class TankDetailsViewModel : StatusViewModel
    {
        public int TankDetailId { get; set; }

        public int BillingFrequencyId { get; set; }

        public decimal Size { get; set; }

        [Display(Name = nameof(Resource.lblDescription), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [StringLength(512)]
        public string Description { get; set; }

        [Range(typeof(Decimal), "0", ApplicationConstants.DecimalMaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        public decimal RentalFee { get; set; }

        public FeeTaxDetails FeeTaxDetails { get; set; }

        [Display(Name = nameof(Resource.lblStartDate), ResourceType = typeof(Resource))]
        [LessThanOrEqualTo("EndDate", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageLessThan), PassOnNull = true)]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public DateTimeOffset StartDate { get; set; }

        [Display(Name = nameof(Resource.lblEndDate), ResourceType = typeof(Resource))]
        [GreaterThanOrEqualTo("StartDate", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageGreaterThan), PassOnNull = true)]
        [RequiredIf("IsToBeIncludedInInvoice", true, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public DateTimeOffset? EndDate { get; set; }

        public Currency Currency { get; set; }

        public UoM UoM { get; set; }

        public bool IsToBeIncludedInInvoice { get; set; }

        public DateTimeOffset? IntervalStartDate { get; set; }
        public DateTimeOffset? IntervalEndDate { get; set; }
        public decimal IntervalDays { get; set; }
        public TankDetailsViewModel Clone()
        {
            var thisObject = (TankDetailsViewModel)this.MemberwiseClone();
            return thisObject;
        }
    }

    public class TankFeeAndTax
    {
        public decimal TotalFee { get; set; }
        public decimal TotalTax { get; set; }
    }
}