using SiteFuel.Exchange.Utilities;
using System;

namespace SiteFuel.Exchange.ViewModels
{
    public class QuoteResponseViewModel : BaseViewModel
    {
        public QuoteResponseViewModel()
        {
            InstanceInitialize();
        }

        public QuoteResponseViewModel(Status status) : base(status)
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            QuoteRequest = new QuoteRequestDetailsViewModel();
            FuelDeliveryDetails = new FuelDeliveryDetailsViewModel();
            FuelPricing = new FuelPricingViewModel();
            IsOtherFuelTypeInFavorite = false;
            ExchangeRate = 1;
        }

        public int QuotationId { get; set; }

        public FuelPricingViewModel FuelPricing { get; set; }

        public string SupplierQuoteNumber { get; set; }

        public FuelDeliveryDetailsViewModel FuelDeliveryDetails { get; set; }

        public QuoteRequestDetailsViewModel QuoteRequest { get; set; }

        public ImageViewModel Image { get; set; }

        public string Notes { get; set; }

        public int CreatedBy { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public int FuelTypeId { get; set; }

        public bool IsOtherFuelTypeInFavorite { get; set; }

        public decimal ExchangeRate { get; set; }
    }
}
