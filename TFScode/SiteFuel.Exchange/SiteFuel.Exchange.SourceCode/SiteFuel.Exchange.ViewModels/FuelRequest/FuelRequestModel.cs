using Newtonsoft.Json;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class FuelRequestResponseModel : StatusViewModel
    {
    }

    public class FuelRequestModel
    {
        public int FuelRequestId { get; set; }
        public string RequestNumber { get; set; }
        [JsonIgnore]
        public int LocationId { get; set; }
        public string Location { get; set; }
        public string LocationAddress { get; set; }
        [JsonIgnore]
        public OrderType OrderTypeId { get; set; }
        public string OrderType { get; set; }
        [JsonIgnore]
        public int? FuelTypeId { get; set; }
        public string FuelType { get; set; }
        public string NonStandardFuelType { get; set; }
        public string NonStandardFuelDescription { get; set; }
        [JsonIgnore]
        public TruckLoadTypes? TruckLoadTypeId { get; set; }
        public string TruckLoadType { get; set; }
        public decimal? MinQuantity { get; set; }
        public decimal Quantity { get; set; }
        [JsonIgnore]
        public QuantityType? QuantityTypeId { get; set; }
        [JsonIgnore]
        public PricingType? PricingTypeId { get; set; }
        public string QuantityType { get; set; }
        [JsonIgnore]
        public string DisplayPrice { get; set; }
        [JsonIgnore]
        public string PricingCode { get; set; }
        [JsonIgnore]
        public string DisplayPriceCode { get; set; }
        [JsonIgnore]
        public int PricingCodeId { get; set; }
        [JsonIgnore]
        public int RequestPriceDetailId { get; set; }
        [JsonIgnore]
        public PricingSource? PricingSourceId { get; set; }
        [JsonIgnore]
        public string PricingSource { get; set; }
        [JsonIgnore]
        public FreightOnBoardTypes? FreightOnBoardTypeId { get; set; }
        public string FOB { get; set; }
        [JsonIgnore]
        public int? TerminalId { get; set; }
        public string TerminalName { get; set; }
        [JsonIgnore]
        public int? CityGroupTerminalId { get; set; }
        [JsonProperty(PropertyName = "CityRackTerminalName")]
        public string CityGroupTerminalName { get; set; }
        [JsonIgnore]
        public DeliveryType? DeliveryTypeId { get; set; }
        [JsonProperty(PropertyName = "OrderRequestType")]
        public string DeliveryType { get; set; }
        public DateTimeOffset DeliveryStartDate { get; set; }
        public DateTimeOffset? DeliveryEndDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int EstimatedQtyPerDelivery { get; set; }
        public decimal OveragePercentage { get; set; }
        public string SpecialInstruction { get; set; }
        public bool IsPrePostDipRequired { get; set; }
        [JsonIgnore]
        public PaymentTerms? PaymentTermId { get; set; }
        public string PaymentTerm { get; set; }
        [JsonIgnore]
        public PaymentMethods? PaymentMethodId { get; set; }
        public string PaymentMethod { get; set; }
        [JsonIgnore]
        public FuelRequestType FuelRequestTypeId { get; set; }
        public string FuelRequestType { get; set; }
        [JsonIgnore]
        public int StatusId { get; set; }
        public string Status { get; set; }
        [JsonIgnore]
        public bool IsMarine { get; set; }
        public bool IsCounterOfferAvailable { get; set; }
        [JsonProperty(PropertyName = "CounterOfferId")]
        public int? CounterOfferFuelRequestId { get; set; }
        public int? ParentId { get; set; }
        [JsonIgnore]
        public Currency Currency { get; set; }
        [JsonProperty(PropertyName = "Currency")]
        public string CurrencyFlag { get; set; }
        [JsonIgnore]
        public UoM UoM { get; set; }
        [JsonProperty(PropertyName = "UoM")]
        public string UoMFlag { get; set; }
        [JsonIgnore]
        public int? AcceptedCompanyId { get; set; }
        public string AcceptedByCompany { get; set; }
        [JsonIgnore]
        public int? FRAcceptedByUserId { get; set; }
        [JsonIgnore]
        public int CreatedBy { get; set; }
        public string CreatedByUser { get; set; }
        public string CreatedByCompany { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        [JsonIgnore]
        public int UpdatedBy { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public bool IsActive { get; set; }

        public List<PricingModel> Pricing { get; set; }
    }

    public class PricingModel
    {
        public int RequestPriceDetailId { get; set; }

        public string PricingSource { get; set; }

        public string PricingCode { get; set; }

        public string DisplayPricingCode { get; set; }

        public decimal Pricing { get; set; }

        public string DisplayPrice { get; set; }

        public decimal? MinQuantity { get; set; }

        public decimal? MaxQuantity { get; set; }
    }
}
