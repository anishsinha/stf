using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SiteFuel.Exchange.ViewModels
{
    public class PDIFuelOrders
    {
        public PDIFuelOrder PDIFuelOrder { get; set; } = new PDIFuelOrder();
    }

    public class PDIFuelOrder
    {

        public string OrderNo { get; set; }
        public int DestinationType { get; set; }
        public string CustomerID { get; set; }
        public string CustomerLocationID { get; set; }
        public string SiteID { get; set; }
        public string PurchaseOrderNo { get; set; }
        public string AlternateReferenceNo { get; set; }        
        public DateTime DeliveryDateTime { get; set; }
        public string BusinessDate { get; set; }
        public string CarrierID { get; set; }
        public string DriverID { get; set; }
        public DateTime LiftDateTime { get; set; }
        public string DeliveryNotes { get; set; }
        public string DriverName { get; set; }
        public List<FuelDetail> FuelDetails { get; set; } = new List<FuelDetail>();
    }

    public class FuelDetail
    {
        public int OrderLineItemNo { get; set; }
        public string OrderedProductID { get; set; }
        public string PurchasedProductID { get; set; }
        public decimal OrderedQuantity { get; set; }
        public decimal DeliveredGrossQuantity { get; set; }
        public decimal DeliveredNetQuantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal FreightUnitPrice { get; set; }
        public decimal EstimatedPerUnitTaxAmount { get; set; }
        public int UnitPriceIncludesTaxes { get; set; }
        public int UnitPriceIncludesFreight { get; set; }

        public LoadDetail LoadDetail { get; set; } = new LoadDetail();
    }

    public class LoadDetail
    {
        public DateTime LiftDateTime { get; set; }
        public string LoadProductID { get; set; }
        //public string PurchasedProductID { get; set; }
        public decimal LoadQuantity { get; set; }
        public decimal LiftGrossQuantity { get; set; }
        public decimal LiftNetQuantity { get; set; }
        public int OriginType { get; set; }
        public string OriginVendorID { get; set; }
        public string OriginTerminalID { get; set; }
        public string OriginSiteID { get; set; }
        public string OriginTankNo { get; set; }
        public string BOLNo { get; set; }
        public decimal UnitCost { get; set; }
        public decimal FreightUnitCost { get; set; }
    }

    public class UspDeliveryDetails
    {
        public int InvoiceId { get; set; }
        public string VendorId { get; set; }
        public string CustomerId { get; set; }
        public string DisplayInvoiceNumber { get; set; }
        public string JobName { get; set; }
        public string SiteId { get; set; }
        public DateTimeOffset DropEndDate { get; set; }
        public DateTimeOffset BusinessDate { get; set; }
        public string Carrier { get; set; }
        public string DriverName { get; set; }
        public string TargetDriverValue { get; set; }
        public string PoNumber { get; set; }
        public DateTimeOffset? LiftDate { get; set; }
        public string Notes { get; set; }
        public decimal? OrderedQuantity { get; set; }
        public decimal DroppedGallons { get; set; }
        public decimal? NetQuantity { get; set; }
        public decimal? GrossQuantity { get; set; }
        public int PickupLocation { get; set; }
        public string SiteName { get; set; }
        public string BolNumber { get; set; }
        public bool DestinationType { get; set; }
        public string FuelType { get; set; }
        public int QuantityIndicatorTypeId { get; set; }
        public int BOLInvoicePreferenceId { get; set; }
        public int OrderId { get; set; }
        public int RequestPriceDetailId { get; set; }
        public bool IsMarineLocation { get; set; }
        public bool IsPDITaxRequired { get; set; }
        public bool IsIncludePricing { get; set; }
        public decimal? ConvertedPricing { get; set; }
        public decimal PricePerGallon { get; set; }
        public int PricingTypeId { get; set; }
        public int UoM { get; set; }
        public int InvoiceTypeId { get; set; }
        public string OriginalInvoiceNumber { get; set; }
        public int FRFuelTypeId { get; set; }
        public int BOLFuelTypeId { get; set; }
        public string StaticOriginVendorId { get; set; }
    }

    public class PDIAddFuelOrderResponse
    {
      public PDIResponseOrder Order { get; set; } = new PDIResponseOrder();
    }
    public class PDIUpdateFuelOrderResponse
    {
        public PDIResponseOrder Order { get; set; } = new PDIResponseOrder();
    }
    public class PDIResponseOrder
    {
        public string OrderNo { get; set; }
        public string PurchaseOrderNo { get; set; }
        public string AlternateReferenceNo { get; set; }
        public string DestinationType { get; set; }
        public string CustomerID { get; set; }
        public string CustomerLocationID { get; set; }
        public string SiteID { get; set; }
        public string OrderStatus { get; set; }
        public string Result { get; set; }
        public List<PDIExceptionMessage> PDIExceptionMessage = new List<PDIExceptionMessage>();
    }

    public class PDIExceptionMessage
    {

        public string Type { get; set; }
        public string Description { get; set; }
        public string AdditionalData { get; set; }
    }

}
