using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class ApiNearestJobByCustomerModel
    {
        public int CustomerId { get; set; }

        public int SupplierId { get; set; }

        public List<int> FuelTypeIds { get; set; } = new List<int>();

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public double Radius { get; set; } = 100;

        public int UserId { get; set; }
    }
}
