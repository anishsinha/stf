using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class QuoteDetailsViewModel : BaseCultureViewModel
    {
        public QuoteDetailsViewModel()
        {
            InstanceInitialize();
        }

        public QuoteDetailsViewModel(Status status) : base(status)
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            QuoteRequest = new QuoteRequestDetailsViewModel();
            FuelRequestFee = new FuelRequestFeeViewModel();
            FuelPricing = new FuelPricingViewModel();
            Documents = new List<DocumentViewModel>();
            FuelDeliveryDetails = new FuelDeliveryDetailsViewModel();
        }
        public int Id { get; set; }

        public FuelPricingViewModel FuelPricing { get; set; }

        public string QuoteNumber { get; set; }

        public string SupplierQuoteNumber { get; set; }

        public string FuelType { get; set; }

        public string Quantity { get; set; }

        public FuelRequestFeeViewModel FuelRequestFee { get; set; }

        public QuoteRequestDetailsViewModel QuoteRequest { get; set; }

        public string Notes { get; set; }

        public List<DocumentViewModel> Documents { get; set; }

        public string QuotationCreatedBy { get; set; }

        public string QuotationCompany { get; set; }

        public string QuoteRequestCreatedBy { get; set; }

        public string QuoteRequestCompany { get; set; }

        public string CreatedDate { get; set; }

        public bool IsTncIncluded { get; set; }

        public int QuotationStatusId { get; set; }

        public string QuotationStatus { get; set; }

        public string PoNumber { get; set; }

        public bool IsOrderActive { get; set; }

        public int OrderId { get; set; }

        public FuelDeliveryDetailsViewModel FuelDeliveryDetails { get; set; }
    }
}
