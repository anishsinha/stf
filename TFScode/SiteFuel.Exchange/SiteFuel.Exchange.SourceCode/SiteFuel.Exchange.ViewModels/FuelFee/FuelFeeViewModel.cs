using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class FuelFeeViewModel
    {
        private decimal _fee = 0;
        public FuelFeeViewModel()
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            FeeSubQuantity = 1;
            FeeByQuantities = new List<DeliveryFeeByQuantityViewModel>();
        }

        public int Id { get; set; }

        public int FeeTypeId { get; set; }

        public int FeeSubTypeId { get; set; }

        public decimal? MinimumGallons { get; set; }

        public decimal Fee {
            get
            {
                return Math.Round(_fee, ApplicationConstants.InvoiceFeeUnitPriceDecimalDisplay);
            }
            set
            { _fee = value; } }

        public string FeeDetails { get; set; }

        public int? MarginTypeId { get; set; }

        public decimal Margin { get; set; }

        public bool IncludeInPPG { get; set; }

        public int? InvoiceId { get; set; }

        public decimal? FeeSubQuantity { get; set; }

        public decimal? TotalFee { get; set; }

        public int? OtherFeeTypeId { get; set; }

        public int? FeeConstraintTypeId { get; set; }

        public DateTimeOffset? SpecialDate { get; set; }

        public Currency Currency { get; set; }

        public UoM UoM { get; set; }

        public int? OfferPricingId { get; set; }

        public int? DiscountLineItemId { get; set; }

        public int? WaiveOffTime { get; set; }

        public DateTimeOffset? StartTime { get; set; }

        public DateTimeOffset? EndTime { get; set; }

        public List<DeliveryFeeByQuantityViewModel> FeeByQuantities { get; set; }

        public bool AddToCommonFees { get; set; }

        public FeeTaxDetails FeeTaxDetails { get; set; }

        public int OrderId { get; set; }

        public bool IsMarineLocation { get; set; }
    }
}
