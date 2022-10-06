using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.Invoice.Pdf;
using System.Collections.Generic;
using System;

namespace SiteFuel.Exchange.ViewModels
{
    public class ConsolidatedInvoicePdfViewModel : BaseCultureViewModel
    {
        public ConsolidatedInvoicePdfViewModel()
        {
            
        }

        public ConsolidatedInvoicePdfViewModel(Status status) : base(status)
        {
           
        }

        public int AssetNotAvailableCount { get; set; }
        public int NoFuelNeededAssetCount { get; set; }
        public PdfHeaderViewModel InvoicePdfHeaderDetail { get; set; } = new PdfHeaderViewModel();
        public List<ConsolidatedInvoiceViewModel> Invoices { get; set; } = new List<ConsolidatedInvoiceViewModel>();
        public List<DispatchLocationViewModel> PickupLocations { get; set; } = new List<DispatchLocationViewModel>();
        public List<BolDetailViewModel> LiftDetails { get; set; }
        public List<BolDetailViewModel> BolDetails { get; set; }
        public FuelFeesViewModel FuelFees { get; set; } = new FuelFeesViewModel();
        public InvoiceTaxDetailsViewModel TaxDetail { get; set; }
        public List<InvoiceXSpecialInstructionViewModel> SpecialInstructions { get; set; }
        public List<List<AssetDropViewModel>> Assets { get; set; }
        public List<string> PoNumbers { get; set; }
        public List<string> DropTicketNumbers { get; set; }
        public PdfFooterModel InvoiceFooter { get; set; }
    }

    public class BDRPdfViewModel : BaseCultureViewModel
    {
        public string CalculatedAPIGravity { get; set; }
        public PdfHeaderViewModel InvoicePdfHeaderDetail { get; set; } = new PdfHeaderViewModel();
        public List<ConsolidatedInvoiceViewModel> Invoices { get; set; } = new List<ConsolidatedInvoiceViewModel>();
        public List<DispatchLocationViewModel> PickupLocations { get; set; } = new List<DispatchLocationViewModel>();
        public List<string> PoNumbers { get; set; }
        public List<string> DropTicketNumbers { get; set; }
        public BDRDetailsModel BDRDetailsModel { get; set; } = new BDRDetailsModel();
        public List<ImageViewModel> BDNImages { get; set; } = new List<ImageViewModel>();
        public bool IsFromDownloadPdf { get; set; }

        public PdfFooterModel InvoiceFooter { get; set; }
    }

    public class MarineTaxAffidavitPdfViewModel
    {        
        public int InvoiceHeaderId { get; set; }

        public DateTimeOffset DisplayDropEndDate { get; set; }

        public List<ImageViewModel> TaxAffidavitImages { get; set; } = new List<ImageViewModel>();
        

        public string BDRNumber { get; set; }

        public string SupplierCompanyName { get; set; }
        public string SupplierAddress { get; set; }
        public string SupplierAddressCity  { get; set; }

        public string SupplierAddressStateCode { get; set; }
        public string SupplierAddressZipCode { get; set; }
        public string SupplierPhoneNumber { get; set; }
        public int CompanyLogoId { get; set; }

        public bool IsFromDownloadPdf { get; set; }

        //public bool IsInlandWaterCandleConditionAccepted { get; set; }

        //public bool IsSalesTaxAccordanceAccepted { get; set; }

        //public bool IsDieselFuel { get; set; }

        //public bool IsLubeOil { get; set; }

        //public bool IsOtherChemicalOrFilters { get; set; }
        public string Vessel { get; set; }
        public string IMONumber { get; set; }

        public string Flag { get; set; }

    }

    public class MarineBDNPdfViewModel
    {
        public int InvoiceHeaderId { get; set; }

        public DateTimeOffset DisplayDropEndDate { get; set; }
        public List<ImageViewModel> BDNImages { get; set; } = new List<ImageViewModel>();
        public string BDRNumber { get; set; }
        public string SupplierCompanyName { get; set; }
        public string SupplierAddress { get; set; }
        public string SupplierAddressCity { get; set; }

        public string SupplierAddressStateCode { get; set; }
        public string SupplierAddressZipCode { get; set; }
        public string SupplierPhoneNumber { get; set; }
        public int CompanyLogoId { get; set; }
        public bool IsFromDownloadPdf { get; set; }       
    }

    public class MarineCGInspectionDocumentPdfViewModel
    {
        // drop location
        public string SiteName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public string ZipCode { get; set; }
        public DateTimeOffset? DisplayDropEndDate { get; set; }
        public DateTimeOffset? DisplayDropStartDate { get; set; }
        public DateTimeOffset? DisplayLiftDate { get; set; }
        public int InvoiceHeaderId { get; set; }
        public List<ImageViewModel> CGInspectionImages { get; set; } = new List<ImageViewModel>();
        public bool IsFromDownloadPdf { get; set; }
        public int OrderId { get; set; }
        public List<AssetDropViewModel> AssetDrops { get; set; } = new List<AssetDropViewModel>();

    }

    public class MarineInspectionRequestVoucherViewModel
    {
        public int InvoiceId { get; set; }
        public int OrderId { get; set; }

        public string DriverFirstName { get; set; }
        public string DriverLastName { get; set; }
        public int? DriverId { get; set; }
        public DateTimeOffset? DisplayDropEndDate { get; set; }
        public List<AssetDropViewModel> AssetDrops { get; set; } = new List<AssetDropViewModel>();
        public string TruckName { get; set; }
        public List<ImageViewModel> InspRequestVoucherImages { get; set; } = new List<ImageViewModel>();
        public int InvoiceHeaderId { get; set; }
        public bool IsFromDownloadPdf { get; set; }
    }
}

