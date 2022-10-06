
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class JobDetailsWithOrders
    {
        public int OrderId { get; set; }
        public string PoNumber { get; set; }
        public string JobName { get; set; }
        public string DisplayJobID { get; set; }
        public int JobId { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int UoM { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string BadgeNo1 { get; set; }
        public string BadgeNo2 { get; set; }
        public string BadgeNo3 { get; set; }
        public int FuelTypeId { get; set; }
    }
}
