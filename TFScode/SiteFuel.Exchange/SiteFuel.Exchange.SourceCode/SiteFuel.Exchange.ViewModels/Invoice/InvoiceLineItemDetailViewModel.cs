using SiteFuel.Exchange.Utilities;
using System;

namespace SiteFuel.Exchange.ViewModels
{
    public class InvoiceLineItemDetailViewModel
    {
        public decimal DroppedGallons { get; set; }

        public string TimeZoneName { get; set; }

        public DateTimeOffset DropStartDate { get; set; }

        public DateTimeOffset DropEndDate { get; set; }

        public int AssetCount { get; set; }

        public string DriverName { get; set; }

        public string FuelType { get; set; }

        public int OrderId { get; set; }

        public int StatusId { get; set; }

        public int TaxInvoiceId { get; set; }

        public int FeesInvoiceId { get; set; }

        public UoM UoM { get; set; }

        public string PoNumber { get; set; }

        public decimal? Quantity { get; set; }

        public int? QuantityTypeId { get; set; }

        public int? OrderTypeId { get; set; }

        public int InvoiceTypeId { get; set; }

        public bool? IsPublicRequest { get; set; }

        public bool IsMarineLocation { get; set; }

        public decimal? ConvertedQuantity { get; set; }

        public int JobCountryId { get; set; }
    }
}