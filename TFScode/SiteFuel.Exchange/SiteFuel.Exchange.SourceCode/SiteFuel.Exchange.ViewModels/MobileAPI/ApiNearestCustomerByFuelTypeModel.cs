using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class ApiNearestCustomerByFuelTypeModel
    {
        public int SupplierCompanyId { get; set; }

        public List<int> FuelTypeIds { get; set; } = new List<int>();
    }
}
