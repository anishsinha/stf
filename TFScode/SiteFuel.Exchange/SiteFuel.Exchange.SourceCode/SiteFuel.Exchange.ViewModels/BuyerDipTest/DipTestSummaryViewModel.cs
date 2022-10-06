using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class DipTestSummaryViewModel
    {
        public string Customer { get; set; }
        public string Location { get; set; }
        public string SiteId { get; set; }
        public string TankName { get; set; }
        public string Capacity { get; set; }
        public UoM CapacityUom { get; set; }
        public string LastInventory { get; set; }
        public TankScaleMeasurement DipTestUom { get; set; }
        public string LastUpdatedOn { get; set; }
        public DipTestMethod DiptestMethod { get; set; }
        public string DisplayDiptestMethod { get; set; }
        public string ContactPerson { get; set; }
        public int LocationId { get; set; }
    }

    public class DipTestRequestModel
    {
        public int CompanyId { get; set; }
        public CompanyType CompanyTypeId { get; set; }
        public int JobId { get; set; }
    }
}
