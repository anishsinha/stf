using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Models
{
    public class ProductRequestModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int ProductTypeId { get; set; }

        public int ProductDisplayGroupId { get; set; }

        public int PricingSourceId { get; set; }

        public int? MappedParentId { get; set; }

        public string ProductCode { get; set; }

        public bool IsActive { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        public int? AxxisProductId { get; set; }

        public int? OpisProductId { get; set; }

        public int? PlattsProductId { get; set; }

        public int? CompanyId { get; set; }
        public bool IsDeleted { get; set; }
        public int? TfxProductId { get; set; }
        public string DisplayName { get; set; } // stores displayname in mstproducts
    }

    public class PickupLocationDetailViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public string ControlNumber { get; set; }
        public string Address { get; set; }
        public string StateCode { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string ZipCode { get; set; }
        public int StateId { get; set; }
        public int CountryId { get; set; }
        public string CountryCode { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public string TerminalOwner { get; set; }
        public int UpdatedBy { get; set; }
    }
}
