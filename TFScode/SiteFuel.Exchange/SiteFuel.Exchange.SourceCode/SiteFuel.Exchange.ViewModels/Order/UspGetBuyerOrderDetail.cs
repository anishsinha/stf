using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class UspGetBuyerOrderDetail
    {
        public int Id { get; set; }
        public int FuelRequestId { get; set; }
        public string PoNumber { get; set; }
        public int AcceptedCompanyId { get; set; }
        public decimal? BrokeredMaxQuantity { get; set; }
        public bool IsProFormaPo { get; set; }
        public bool IsHidePricingEnabledForBuyer { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public int? DriverId { get; set; }
        public string DriverFirstName { get; set; }
        public string DriverLastName { get; set; }
        public string TerminalName { get; set; }
        public string CityGroupTerminalName { get; set; }
        public double Distance { get; set; }
        public decimal OrderTotalAmount { get; set; }
        public string SupplierCompanyName { get; set; }
        public string SupplerFirstName { get; set; }
        public string SupplerLastName { get; set; }
        public string SupplerEmail { get; set; }
        public string SupplerPhoneNumber { get; set; }
        public int BuyerCompanyId { get; set; }
        public int QuantityTypeId { get; set; }
        public decimal MinQuantity { get; set; }
        public decimal MaxQuantity { get; set; }
        public string ExternalPoNumber { get; set; }
        public int FuelTypeId { get; set; }
        public int EstimateGallonsPerDelivery { get; set; }
        public int? OrderClosingThreshold { get; set; }
        public string FuelDescription { get; set; }
        public int PricingTypeId { get; set; }
        public int? RackAvgTypeId { get; set; }
        public decimal PricePerGallon { get; set; }
        public decimal? SupplierCost { get; set; }
        public UoM UoM { get; set; }
        public Currency Currency { get; set; }
        public int DeliveryTypeId { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string FuelTypeName { get; set; }
        public int ProductDisplayGroupId { get; set; }
        public int JobId { get; set; }
        public string DisplayJobID { get; set; }
        public string JobName { get; set; }
        public int JobStateId { get; set; }
        public bool IsResaleEnabled { get; set; }
        public DateTimeOffset? JobEndDate { get; set; }
        public string JobAddress { get; set; }
        public string JobCity { get; set; }
        public string JobStateCode { get; set; }
        public int JobLocationType { get; set; }
        public string JobZipCode { get; set; }
        public bool IsTaxExempted { get; set; }
        public decimal InvoicedAmount { get; set; }
        public decimal DropTicketAmount { get; set; }
        public decimal DeliveredPercentage { get; set; }
        public int AssetsAssigned { get; set; }
        public string SpecialInstructions { get; set; }
        public string Qualifications { get; set; }
        public string CustomAttribute { get; set; }
        public int PricingSourceId { get; set; }
        public string FileDetails { get; set; }
        public string DisplayPricePerGallon { get; set; }
        public int RequestPriceDetailId { get; set; }
        public int PricingCodeId { get; set; }
        public string PricingCode { get; set; }
        public string PricingCodeDescription { get; set; }
        public int PricingQuantityIndicatorTypeId { get; set; }
        public string SiteInstructions { get; set; }
        public string AccountingCompanyId { get; set; }
        public string SupplierAssignedProductName { get; set; }
        public OrderEnforcement OrderEnforcementId { get; set; }
        public bool IsPrePostDipRequired { get; set; }
        public int CountryId { get; set; }
        public int CompanyCountryId { get; set; }
    }
}
