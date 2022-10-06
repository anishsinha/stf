using Newtonsoft.Json;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class OrderAPIResponseModel : StatusViewModel
    {
    }

    public class BuyerOrderModel : OrderModel
    {

    }

    public class SupplierOrderModel : OrderModel
    {
        public int BrokerFuelRequestId { get; set; }
        
    }

    public class OrderModel
    {
        public int OrderId { get; set; }
        public int FuelRequestId { get; set; }
        public string OrderName { get; set; }
        public string RequestDeliveryType { get; set; }
        public string PoNumber { get; set; }
        public int FuelTypeId { get; set; }
        public string FuelType { get; set; }
        public string Pricing { get; set; }
        [JsonIgnore]
        public OrderType? OrderTypeId { get; set; }
        public string OrderType { get; set; }
        [JsonIgnore]
        public QuantityType? QuantityTypeId { get; set; }
        public string QuantityType { get; set; }
        public decimal? MinQuantity { get; set; }
        public decimal? Quantity { get; set; }
        [JsonIgnore]
        public DeliveryType? DeliveryTypeId { get; set; }
        [JsonProperty(PropertyName = "OrderRequestType")]
        public string DeliveryType { get; set; }
        [JsonIgnore]
        public int StatusId { get; set; }
        public string Status { get; set; }
        [JsonIgnore]
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public string LocationAddress { get; set; }
        public bool IsProFormaPo { get; set; }
        public bool IsEndSupplier { get; set; }
        [JsonIgnore]
        public InvoiceType DefaultInvoiceType { get; set; }
        [JsonIgnore]
        public int AcceptedCompanyId { get; set; }
        public string Supplier { get; set; }
        [JsonIgnore]
        public int BuyerCompanyId { get; set; }
        public string Buyer { get; set; }
        [JsonIgnore]
        public int AcceptedByUserId { get; set; }
        public string AcceptedByUser { get; set; }
        public DateTimeOffset OrderAcceptedDate { get; set; }
        [JsonIgnore]
        public int? TerminalId { get; set; }
        public string TerminalName { get; set; }
        [JsonIgnore]
        public int? CityGroupTerminalId { get; set; }
        public string TerminalControlNumber { get; set; }
        public string Notes { get; set; }
        public int? PreferencesSettingId { get; set; }
        public LocationManagedType? LocationManagedTypeId { get; set; }
        public string LocationManagedType { get; set; }
        public DateTimeOffset DeliveryStartDate { get; set; }
        [JsonIgnore]
        public int? ParentId { get; set; }
        public int UpdatedBy { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
