using SiteFuel.Exchange.Core.StringResources;
using System;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class USP_FuelRequestSummaryModel
    {
        public DateTimeOffset CreatedDate { get; set; }

        public DateTimeOffset? AcceptedDate { get; set; }

        public int FuelRequestId { get; set; }

        public string RequestNumber { get; set; }

        public string ZipCode { get; set; }

        public decimal Quantity { get; set; }

        public string FuelType { get; set; }

        public string CompanyName { get; set; }

        public decimal Price { get; set; }

        public string PricePerGallon { get; set; }

        public string Status { get; set; }

        public string DateCreated { get; set; }

        public string DateAccepted { get; set; }

        public int PricingTypeId { get; set; }

        //public int? RackAvgTypeId { get; set; }

        public string AccountType { get; set; }

        public int StatusId { get; set; }

        public DateTimeOffset? ExpirationDate { get; set; }

        public string TimeZoneName { get; set; }

        public DateTimeOffset StartDate { get; set; }

        public TimeSpan EndTime { get; set; }

        public int FuelRequestTypeId { get; set; }

        public string FuelRequestType { get; set; }

        public bool AboutToExpire { get; set; }

        public string GallonsOrdered { get; set; }

        public int DeliveryTypeId { get; set; }

        public string DeliveryType { get; set; }

        public int TotalCount { get; set; }
    }
}

