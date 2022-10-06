using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class SupplierDetailViewModel
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int AddressId { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNumber { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public string StateCode { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public bool IsStateWideService { get; set; }
        public int Radius { get; set; }
        public bool IsDefault { get; set; }
        public string FuelTypesServe { get; set; }
        public int SupplierType { get; set; }
    }
}
