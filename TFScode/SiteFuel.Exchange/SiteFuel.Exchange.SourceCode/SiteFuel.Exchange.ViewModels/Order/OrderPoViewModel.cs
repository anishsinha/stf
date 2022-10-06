using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.Invoice.Pdf;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class OrderPoViewModel : BaseViewModel
    {
        public OrderPoViewModel()
        {
            InstanceInitialize();
        }

        public OrderPoViewModel(Status status)
            : base(status)
        {
            InstanceInitialize(status);
        }

        private void InstanceInitialize(Status status = Status.Failed)
        {
            SupplierLocation = new AddressViewModel(status);
            BuyerLocation = new AddressViewModel(status);
            ShippingLocation = new AddressViewModel(status);
            DeliveryDetails = new FuelDeliveryDetailsViewModel(status);
            CompanyLogo = new ImageViewModel(status);
            PoContact = new ContactPersonViewModel(status);
            DifferentFuelPrices = new List<DifferentFuelPriceViewModel>();
            AdditionalFee = new List<AdditionalFeeViewModel>();
            FuelRequestFee = new FuelRequestFeeViewModel(status);
            DeliveryFeeByQuantity = new List<DeliveryFeeByQuantityViewModel>();
            SpecialInstructions = new List<SpecialInstructionViewModel>();
            InvoiceDetails = new List<OrderDropDetailsViewModel>();
            FuelFees = new FuelFeesViewModel();
        }

        public int OrderId { get; set; }

        public int FuelRequestId { get; set; }

        public string PoNumber { get; set; }

        public string JobName { get; set; }

        public string DisplayJobID { get; set; }

        public int ScheduleId { get; set; }

        public string ScheduleName { get; set; }

        public string SupplierCompanyName { get; set; }

        public string BuyerCompanyName { get; set; }

        public string PhoneNumber { get; set; }

        public string DateOrdered { get; set; }

        public string CustomerId { get; set; }

        public string PaymentTerm { get; set; }

        public string OrderType { get; set; }

        public string FuelName { get; set; }

        public decimal GallonsOrdered { get; set; }

        public string OrderTotalAmount { get; set; }

        public string PricePerGallon { get; set; }

        public int QuantityTypeId { get; set; }

        public int TypeOfFuel { get; set; }

        public string PrductDescription { get; set; }

        public bool IsAssetTrackingEnabled { get; set; }

        public int RequestPriceDetailId { get; set; }

        public decimal CreationTimeRackPPG { get; set; }

        public AddressViewModel SupplierLocation { get; set; }

        public AddressViewModel BuyerLocation { get; set; }

        public AddressViewModel ShippingLocation { get; set; }

        public ContactPersonViewModel PoContact { get; set; }

        public FuelDeliveryDetailsViewModel DeliveryDetails { get; set; }

        public List<SpecialInstructionViewModel> SpecialInstructions { get; set; }

        public IList<DifferentFuelPriceViewModel> DifferentFuelPrices { get; set; }

        public FuelRequestFeeViewModel FuelRequestFee { get; set; }

        public IList<AdditionalFeeViewModel> AdditionalFee { get; set; }

        public ImageViewModel CompanyLogo { get; set; }

        public IList<DeliveryFeeByQuantityViewModel> DeliveryFeeByQuantity { get; set; }

        public List<OrderDropDetailsViewModel> InvoiceDetails { get;set ; }

        public FuelFeesViewModel FuelFees { get; set; }

        public decimal QbRate { get; set; }

        public DateTimeOffset QbTxnDate { get; set; }

        public Currency Currency { get; set; }

        public UoM UoM { get; set; }

        public bool IsBillToEnabled { get; set; }
        public string BillToName { get; set; }
        public string BillToAddress { get; set; }
        public string BillToAddressLine2 { get; set; }
        public string BillToAddressLine3 { get; set; }
        public string BillToCity { get; set; }
        public string BillToZipCode { get; set; }
        public int? BillToStateId { get; set; }
        public string BillToStateCode { get; set; }
        public int? BillToCountryId { get; set; }
        public string BillToCountryCode { get; set; }
        public string BillToCounty { get; set; }
        public string BillToStateName { get; set; }
        public string BillToCountryName { get; set; }
        public int AcceptedCompanyId { get; set; }
        public string PdfFooterJson { get; set; }
        public PdfFooterModel PdfFooter { get; set; }
        public string Berth { get; set; }
        public string IMONumber { get; set; }
        public string Vessel { get; set; }
        public bool IsShowProductDescriptionOnInvoice { get; set; }
        public string SuperAdminProductDescription { get; set; }
        public string BDRNumber { get; set; }
    }
}
