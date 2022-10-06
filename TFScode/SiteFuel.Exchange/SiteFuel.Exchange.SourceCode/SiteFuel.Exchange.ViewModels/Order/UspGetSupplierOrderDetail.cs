using SiteFuel.Exchange.Utilities;
using System;

namespace SiteFuel.Exchange.ViewModels
{
    public class UspGetSupplierOrderDetail
    {
        public int BuyerCompanyId { get; set; }
        public int Id { get; set; }
        public int FuelRequestId { get; set; }
        public int JobId { get; set; }
        public string DisplayJobID { get; set; }
        public int StateId { get; set; }
        public string PoNumber { get; set; }
        public int TerminalId { get; set; }
        public int? BulkPlantId { get; set; }
        public int CityGroupTerminalId { get; set; }
        public string TerminalName { get; set; }
        public string FuelType { get; set; }
        public bool IsTaxExempted { get; set; }
        public bool IsProFormaPo { get; set; }
        public int JobCompanyId { get; set; }
        public decimal GallonsOrdered { get; set; }
        public DateTimeOffset? JobEndDate { get; set; }
        public bool IsEndSupplier { get; set; }
        public bool IsDefaultInvoiceTypeManual { get; set; }
        public int EstimatedGallonsPerDelivery { get; set; }
        public int FuelRequestTypeId { get; set; }
        public int TypeOfFuel { get; set; }
        public int ProductTypeId { get; set; }
        public string ProductDescription { get; set; }
        public int FuelTypeId { get; set; }
        public int? TfxFuelTypeId { get; set; }
        public int PricingTypeId { get; set; }
        public int? OrderClosingThreshold { get; set; }
        public string JobName { get; set; }
        public int LocationType { get; set; }
        public string Address { get; set; }
        public string StateCode { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public int CountryId { get; set; }
        public string CountryCode { get; set; }
        public Currency Currency { get; set; }
        public bool IsFTL { get; set; }
        public bool IsRetailJob { get; set; }
        public bool IsCustomerContactExists { get; set; }
        public decimal? BrokeredMaxQuantity { get; set; }
        public UoM UoM { get; set; }
        public int? ExternalBrokerId { get; set; }
        public string TimeZoneName { get; set; }
        public int SupplierCostTypeId { get; set; }
        public bool CanCreateInvoice { get; set; }
        public int StatusId { get; set; }
        public int? DriverId { get; set; }
        public string DriverName { get; set; }
        public int StatusUpdatedBy { get; set; }
        public int QuantityTypeId { get; set; }
        public int? FreightOnboardTypeId { get; set; }
        public int? PricingQuantityIndicatorTypeId { get; set; }
        public string CityGroupTerminalName { get; set; }
        public int? PricingSourceId { get; set; }
        public int? FeedTypeId { get; set; }
        public int? PricingSourceQuantityIndicatorTypeId { get; set; }
        public int? WeekendDropPricingDay { get; set; }
        public int? FuelClassTypeId { get; set; }
        public int FrPaymentTermId { get; set; }
        public int FrNetDays { get; set; }
        public decimal? Allowance { get; set; }
        public int? BolInvoicePreferenceId { get; set; }
        public bool? IsDriverToUpdateBol { get; set; }
        public bool IsBolImageRequired { get; set; }
        public bool IsDropImageRequired { get; set; }
        public bool? IsFuelSurcharge { get; set; }
        public bool? IsFreightCost { get; set; }
        public FuelSurchagePricingType? FuelSurchagePricingType { get; set; }
        public int? CarrierId { get; set; }
        public string LoadCode { get; set; }
        public string Notes { get; set; }
        public string SupplierContract { get; set; }
        public string CarrierName { get; set; }
        public int? SupplierSourceId { get; set; }
        public string SupplierSourceName { get; set; }
        public int? AdditionalDetailId { get; set; }
        public int DeliveryTypeId { get; set; }
        public int? OrderXTogglePricingDetailId { get; set; }
        public bool? IsHidePricingEnabledForBuyer { get; set; }
        public bool? IsHidePricingEnabledForSupplier { get; set; }
        public int PaymentTermId { get; set; }
        public int NetDays { get; set; }
        public PaymentMethods PaymentMethod { get; set; }
        public int? FrPricingDetailId { get; set; }
        public int? ExternalBrokerBuySellDetailId { get; set; }
        public int? BuySellBrokerId { get; set; }
        public int? InvoicePreferenceId { get; set; }
        public int? ExternalBrokerOrderDetailId { get; set; }
        public string PaymentTermName { get; set; }
        public decimal? BrokerMarkUp { get; set; }
        public decimal? SupplierCost { get; set; }
        public decimal? SupplierMarkUp { get; set; }
        public int? ThirdPartyNozzleId { get; set; }
        public string VendorId { get; set; }
        public string CustomerNumber { get; set; }
        public string ShipTo { get; set; }
        public string Source { get; set; }
        public string ProductCode { get; set; }
        public decimal MinQuantity { get; set; }
        public decimal MaxQuantity { get; set; }
        public int AssignedAssetCount { get; set; }
        public bool IsAssetHistoryAvailable { get; set; }
        public bool AnyInvoiceExists { get; set; }
        public int ScheduleId { get; set; }
        public string ScheduleName { get; set; }
        public decimal FuelDeliveredPercentage { get; set; }
        public decimal? OrderTotalAmount { get; set; }
        public string DisplayPricePerGallon { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int? PoContactId { get; set; }
        public string CustomAttribute { get; set; }
        public string DeliveryTypeName { get; set; }
        public decimal PricePerGallon { get; set; }
        public int? RackAvgTypeId { get; set; }
        public decimal? GlobalFuelCost { get; set; }
        public int CustomerId { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public string CustomerCompany { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string SupplierEmail { get; set; }
        public string SupplierPhoneNumber { get; set; }
        public string SupplierCompany { get; set; }
        public string SupplierName { get; set; }
        public string ExternalBrokerCompany { get; set; }
        public bool IsBrokerVisible { get; set; }
        public int? ParentOrderId { get; set; }
        public string FileDetails { get; set; }
        public bool IsSignatureEnabled { get; set; }
        public int RequestPriceDetailId { get; set; }
        public int AcceptedCompanyId { get; set; }
        public bool IsActive { get; set; }
        public bool FrIsDriverToUpdateBOL { get; set; }
        public int TruckLoadTypeId { get; set; }
        public string PricingCodeDescription { get; set; }
        public string PickupAddress { get; set; }
        public string PickupCity { get; set; }
        public string PickupStateCode { get; set; }
        public string PickupZipCode { get; set; }
        public string SiteName { get; set; }
        public string BulkPlantName { get; set; }
        public string GroupPoNumber { get; set; }
        public int? OrderGroupId { get; set; }

        public bool IsBillToEnabled { get; set; }
        public int? BillingAddressId { get; set; }
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
        public string SiteInstructions { get; set; }
        public int? PreferencesSettingId { get; set; }
        public string CarrierCompanyName { get; set; }

        public string MyProductId { get; set; }
        public string OrderName { get; set; }
        public OrderEnforcement OrderEnforcementId { get; set; }
        public bool IsPrePostDipRequired { get; set; }
        public bool IsDispatchRetainedByCustomer { get; set; }
        public string DriverProductId { get; set; }

        public bool IsMarineLocation { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get;set;}
        public bool IsIncludePricing { get; set; }
        public bool IsPdiTaxRequired { get; set; }
        public int? LeadRequestId { get; set; }

        public string Berth { get; set; }
        public int? VessleId { get; set; }
        public string IMONumber { get; set; }
        public string Flag { get; set; }
        public int? JobXAssetId { get; set; }

        public string Vessel { get; set; }

        public bool IsBDNConfirmationRequired { get; set; }

        public bool IsInvoiceConfirmationRequired { get; set; }

        public bool IsSupressOrderPricing { get; set; }

        public FreightPricingMethod FreightPricingMethod { get; set; }

        public FreightRateRuleType? FreightRateRuleType { get; set; }

        public TableTypes? FreightRateTableType { get; set; }

        public int? FreightRateRuleId { get; set; }

        public TableTypes? FuelSurchargeTableType { get; set; }

        public int? FuelSurchargeTableId { get; set; }

        public TableTypes? AccessorialFeeTableType { get; set; }
        public int? AccessorialFeeId { get; set; }
        public int? CreditCheckTypeId { get; set; }
    }
}
