using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.Job
{
    public class JobBuyerDashboardViewModel
    {
        public string CustomerName { get; set; }
        public int CustomerId { get; set; }
        public int CountryId { get; set; }
        public string CountryCode { get; set; }
        public int JobID { get; set; }
        public string JobName { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int? CityId { get; set; }
        public string State { get; set; }
        public int? StateID { get; set; }
        public string ZipCode { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactPhoneNumber { get; set; }
        public bool IsGeocodeUsed { get; set; }
        public string CountyName { get; set; }
        public string DistanceCovered { get; set; }
        public List<JobDRDetailsModel> jobDeliveryRequests { get; set; } = new List<JobDRDetailsModel>();
    }
}
