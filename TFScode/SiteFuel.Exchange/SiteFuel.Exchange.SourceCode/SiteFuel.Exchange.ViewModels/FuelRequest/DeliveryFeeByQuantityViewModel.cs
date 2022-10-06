using Foolproof;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class DeliveryFeeByQuantityViewModel : StatusViewModel
    {
        private decimal _fee = 0;
        public DeliveryFeeByQuantityViewModel()
        {
            InstanceInitialize();
        }

        public DeliveryFeeByQuantityViewModel(Status status) : base(status)
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            Fee = 0;
            FeeTypeId = (int)FeeType.DeliveryFee;
            FeeSubTypeId = (int)FeeSubType.ByQuantity;
        }

        public int Id { get; set; }

        [Display(Name = nameof(Resource.lblFrom), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public decimal MinQuantity { get; set; }

        [Display(Name = nameof(Resource.lblUpto), ResourceType = typeof(Resource))]
        [GreaterThan("MinQuantity", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageGreaterThan))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public decimal? MaxQuantity { get; set; }

        [RegularExpression(@"^((\d+)|(\d+(\.\d{1,8})))$", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public decimal Fee { get { return Math.Round(_fee, ApplicationConstants.InvoiceFeeUnitPriceDecimalDisplay); } set { _fee = value; } }

        public int FeeTypeId { get; set; }

        public int FeeSubTypeId { get; set; }

        public string CollectionHtmlPrefix { get; set; }

        public Currency Currency { get; set; }

        public UoM UoM { get; set; }

        public int? MarginTypeId { get; set; }

        public decimal Margin { get; set; }
        public int? FuelFeesId { get; set; }

        public override string ToString()
        {
            return $"${Fee}";

        }
    }
}
