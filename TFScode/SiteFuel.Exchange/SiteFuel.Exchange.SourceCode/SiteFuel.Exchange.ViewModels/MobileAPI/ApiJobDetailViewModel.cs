using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class ApiJobDetailViewModel
    {
        public int JobId { get; set; }

        public string JobName { get; set; }

        public string UoM { get; set; }

        public string SiteImageFilePath { get; set; }

        public string AdditionalImageFilePath { get; set; }

        public string AdditionalImageDescription { get; set; }

        public string DisplayJobId { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string StateCode { get; set; }

        public string ZipCode { get; set; }

        public string CountryCode { get; set; }

        public double Distance { get; set; }

        public int AssetCount { get; set; }

        public List<ApiOrderDetailsForJobViewModel> Orders { get; set; }
    }
}
