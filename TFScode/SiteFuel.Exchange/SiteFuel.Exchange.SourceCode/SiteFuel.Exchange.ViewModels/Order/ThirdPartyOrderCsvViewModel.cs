using FileHelpers;

namespace SiteFuel.Exchange.ViewModels
{
    [DelimitedRecord(",")]
    public class ThirdPartyOrderCsvViewModel
    {
        [FieldQuoted]
        public string CompanyName { get; set; }

        [FieldQuoted]
        public string ContactPersonName { get; set; }

        [FieldQuoted]
        public string ContactPersonEmail { get; set; }

        [FieldQuoted]
        public string ConactPersonPhone { get; set; }

        [FieldQuoted]
        public string JobName { get; set; }

        [FieldQuoted]
        public string DisplayJobID { get; set; }

        [FieldQuoted]
        public string Address { get; set; }

        [FieldQuoted]
        public string City { get; set; }

        [FieldQuoted]
        public string State { get; set; }

        [FieldQuoted]
        public string CountryCode { get; set; }

        [FieldQuoted]
        public string Zip { get; set; }

        [FieldQuoted]
        public string StandardFuelType { get; set; }

        [FieldQuoted]
        public string NonStandardFuelType { get; set; }

        [FieldQuoted]
        public string NonStandardFuelDescription { get; set; }

        [FieldQuoted]
        public string MinQuantity { get; set; }

        [FieldQuoted]
        public string MaxQuantity { get; set; }

        [FieldQuoted]
        public string PricePerGallon { get; set; }

        [FieldQuoted]
        public string CityRackTerminal { get; set; }

        [FieldQuoted]
        public string DeliveryDate { get; set; }

        [FieldQuoted]
        public string DeliveryEndDate { get; set; }

        [FieldQuoted]
        public string DeliveryStartTime { get; set; }

        [FieldQuoted]
        public string DeliveryEndTime { get; set; }

        [FieldQuoted]
        public string DeliveryType { get; set; }

        [FieldQuoted]
        public string DriverFirstName { get; set; }

        [FieldQuoted]
        public string DriverLastName { get; set; }

        [FieldQuoted]
        public string DriverEmail { get; set; }

        [FieldQuoted]
        public string CommonFees { get; set; }

        [FieldQuoted]
        public string OtherFees { get; set; }

        [FieldQuoted]
        public string ExternalPONumber { get; set; }

        [FieldQuoted]
        public string OnsiteContactEmail { get; set; }

        [FieldQuoted]
        public string OnsiteContactName { get; set; }

        [FieldQuoted]
        public string OnsiteContactPhone { get; set; }

        [FieldQuoted]
        public string DeliverySchedulesDate { get; set; }

        [FieldQuoted]
        public string DeliverySchedulesStartTime { get; set; }

        [FieldQuoted]
        public string DeliverySchedulesEndTime { get; set; }

        [FieldQuoted]
        public string DeliverySchedulesQuantity { get; set; }

        [FieldQuoted]
        public string AssetName { get; set; }

        [FieldQuoted]
        public string CatClass { get; set; }

        [FieldQuoted]
        public string Make { get; set; }

        [FieldQuoted]
        public string Model { get; set; }

        [FieldQuoted]
        public string Year { get; set; }

        [FieldQuoted]
        public string IsBuyAndSellOrder { get; set; }

        [FieldQuoted]
        public string BrokeredCustomer { get; set; }

        [FieldQuoted]
        public string IsThirdPartyHardwareUsed { get; set; }

        [FieldQuoted]
        public string InvoicePreference { get; set; }

        [FieldQuoted]
        public string VendorId { get; set; }

        [FieldQuoted]
        public string CustomerNumber { get; set; }

        [FieldQuoted]
        public string ShipTo { get; set; }

        [FieldQuoted]
        public string Source { get; set; }

        [FieldQuoted]
        public string ProductCode { get; set; }

        [FieldQuoted]
        public string FreightFee { get; set; }

        [FieldQuoted]
        public string BrokeredOrderAdditionalFees { get; set; }

        [FieldQuoted]
        public string BrokerMarkup { get; set; }

        [FieldQuoted]
        public string SupplierMarkup { get; set; }

        [FieldQuoted]
        public string OrderTaxes { get; set; }
    }
}
