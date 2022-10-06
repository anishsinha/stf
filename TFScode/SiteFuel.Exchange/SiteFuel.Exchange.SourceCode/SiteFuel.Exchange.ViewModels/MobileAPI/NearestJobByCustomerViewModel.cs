using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class NearestJobByCustomerViewModel : StatusViewModel
    {
        public int JobId { get; set; }

        public string JobName { get; set; }

        public string UoM { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string StateCode { get; set; }

        public string ZipCode { get; set; }

        public string CountryCode { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public int OrderId { get; set; }

        public string PoNumber { get; set; }

        public int? TerminalId { get; set; }

        public int FuelTypeId { get; set; }

        public string FuelType { get; set; }

        public int ProductTypeId { get; set; }

        public string ProductType { get; set; }

        public int AssetCount { get; set; }

        public bool IsDriverToUpdateBOL { get; set; }

        public bool IsDropImageRequired { get; set; }

        public bool IsBOLImageRequired { get; set; }

        public bool IsSignatureRequired { get; set; }

        public double Distance { get; set; }

        public bool IsPrePostDipRequired { get; set; }
    }
}
