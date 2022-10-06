using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class UspOrderPdfDetails
    {
        public string ScheduleName { get; set; }
        public string PoContactName { get; set; }
        public string PoContactEmail { get; set; }
        public string PoContactNumber { get; set; }
        public int OrderId { get; set; }
        public int FuelRequestId { get; set; }
        public string SupplierCompanyName { get; set; }
        public string SupplierAddress { get; set; }
        public string SupplierAddressLine2 { get; set; }
        public string SupplierAddressLine3 { get; set; }
        public string SupplierCity { get; set; }
        public string SupplierZipCode { get; set; }
        public string SupplierStateCode { get; set; }
        public byte[] Image { get; set; }

        public string CompanyLogoURL { get; set; }
        public string PhoneNumber { get; set; }
        public string JobName { get; set; }
        public string DisplayJobID { get; set; }
        public DateTimeOffset AcceptedDate { get; set; }
        public int AcceptedCompanyId { get; set; }
        public int BuyerCompanyId { get; set; }
        public string BuyerCompanyName { get; set; }
        public string BuyerAddress { get; set; }
        public string BuyerAddressLine2 { get; set; }
        public string BuyerAddressLine3 { get; set; }
        public string BuyerCity { get; set; }
        public string BuyerZipCode { get; set; }
        public string BuyerStateCode { get; set; }
        public string BuyerCountryName { get; set; }
        public string BuyerStateName { get; set; }
        public string BuyerCountyName { get; set; }
        public string JobAddress { get; set; }
        public string JobAddressLine2 { get; set; }
        public string JobAddressLine3 { get; set; }
        public string JobCity { get; set; }
        public string JobZipCode { get; set; }
        public string JobStateCode { get; set; }
        public decimal GallonsOrdered { get; set; }
        public int QuantityTypeId { get; set; }
        public string FuelDescription { get; set; }
        public Currency Currency { get; set; }
        public UoM UoM { get; set; }
        public string FuelName { get; set; }
        public int ProductDisplayGroupId { get; set; }
        public string OrderType { get; set; }
        public bool IsAssetTracked { get; set; }
        public string PoNumber { get; set; }
        public int PaymentTermId { get; set; }
        public int NetDays { get; set; }
        public string PaymentTerm { get; set; }
        public int PoContactId { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string CustomAttribute { get; set; }
        public string PricePerGallon { get; set; }
        public decimal? OrderAmount { get; set; }
        public decimal QbRequestPricePerGallon {get;set;}
        public int PoContactCompanyId { get; set; }
        public string PoContactCompanyName { get; set; }
        public string SupplierCountryCode { get; set; }
        public string BuyerCountryCode { get; set; }
        public string JobCountryCode { get; set; }
        public decimal CreationTimeRackPPG { get; set; }
        public int RequestPriceDetailId { get; set; }
        public int FuelTypeId { get; set; }
        public int StateId { get; set; }

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
        public string PdfFooterJson { get; set; }
        public string Berth { get; set; }
        public string IMONumber { get; set; }
        public string Vessel { get; set; }
        public bool IsShowProductDescriptionOnInvoice { get; set; }
        public string SuperAdminProductDescription { get; set; }
        public string BDRNumber { get; set; }
    }
}
