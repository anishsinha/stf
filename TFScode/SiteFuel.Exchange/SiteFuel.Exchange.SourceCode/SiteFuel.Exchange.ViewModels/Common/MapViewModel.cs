using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class MapViewModel : BaseViewModel
    {
        public MapViewModel()
        {
        }

        public MapViewModel(Status status)
            : base(status)
        {
        }

        public int JobId { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public string ZipCode { get; set; }

        public List<ContactPersonViewModel> ContactPersons { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public string PoNumber { get; set; }

        public int OrderId { get; set; }

        public string SupplierName { get; set; }
    }
}
