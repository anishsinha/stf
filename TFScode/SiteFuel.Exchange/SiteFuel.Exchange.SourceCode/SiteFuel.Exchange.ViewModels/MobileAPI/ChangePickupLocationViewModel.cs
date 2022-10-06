using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class ChangePickupLocationViewModel
    {    
        public int? TerminalId { get; set; }

        public string BulkplantName { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string StateCode { get; set; }

        public string ZipCode { get; set; }

        public string CountryCode { get; set; }

        public int UserId { get; set; }

        public List<PickupLocationOrderDetailRequestModel> Orders { get; set; }
    }

    public class PickupLocationOrderDetailRequestModel
    {
        public int OrderId { get; set; }
        public int? DeliveryScheduleId { get; set; }
        public int? TrackableScheduleId { get; set; }
    }

    public class PickLocationOrderDetailResponseModel
    {
        public int PickupLocationId { get; set; }
        public int OrderId { get; set; }
        public int? TerminalId { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}
