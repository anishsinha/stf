using Newtonsoft.Json;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class LocationResponseModel : StatusViewModel
    {
    }

    public class LocationViewModel
    {
        public LocationViewModel()
        {
        }

        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public string ThirdPartyLocationId { get; set; }
        public string OnsiteContactPerson { get; set; }
        public string OnsiteContactNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        [JsonIgnore]
        public int StateId { get; set; }
        public string StateCode { get; set; }
        public string State { get; set; }
        public string CountyName { get; set; }
        [JsonIgnore]
        public int CountryId { get; set; }
        public string CountryCode { get; set; }
        public string ZipCode { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        [JsonIgnore]
        public int StatusId { get; set; }
        public string Status { get; set; }
        public string BillingLocationName { get; set; }
        public string BillingLocationAddress { get; set; }
        public string BillingLocationCity { get; set; }
        [JsonIgnore]
        public int? BillingLocationStateId { get; set; }
        public string BillingLocationStateCode { get; set; }
        public string BillingLocationState { get; set; }
        public string BillingLocationZipCode { get; set; }
        [JsonIgnore]
        public int? BillingLocationCountryId { get; set; }
        public string BillingLocationPhoneNumber { get; set; }
        public DateTimeOffset LocationStartDate { get; set; }
        public DateTimeOffset? LocationEndDate { get; set; }
        public decimal Budget { get; set; }
        public bool IsRetailLocation { get; set; }
        public bool IsApprovalWorkflowEnabled { get; set; }
        public string SiteInstructions { get; set; }
        public string CreatedByUser { get; set; }
        public int LocationCompanyId { get; set; }
        public string CreatedByCompany { get; set; }
        public int? CarrierCompanyId { get; set; }
        public string CarrierCompany { get; set; }
        public bool IsSignatureEnabled { get; set; }
        public bool IsProFormaPoEnabled { get; set; }
        public bool IsResaleEnabled { get; set; }
        public bool IsMarineLocation { get; set; }
        [JsonIgnore]
        public int JobLocationTypeId { get; set; }
        [JsonProperty(PropertyName = "LocationType")]
        public string JobLocationType { get; set; }
        public int LocationManagedTypeId { get; set; }
        public string LocationManagedType { get; set; }
        public int? LocationInventoryManagedBy { get; set; }
        public bool IsAssetTracked { get; set; }
        public bool IsTaxExempted { get; set; }
        [JsonIgnore]
        public int Currency { get; set; }
        public string CurrencyFlag { get; set; }
        public string AccountingCompanyId { get; set; }
        public string BuyerCompany { get; set; }
        public int CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}
