using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class OnBoardingFleetInformation
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public FleetType TrailerServiceType { get; set; }
        public int Capacity { get; set; }
        public bool TrailerHasPump { get; set; }
        public bool IsTrailerMetered { get; set; }
        public int Count { get; set; }
        public bool IsPackagedGoods { get; set; }
      
    }
    public class OnBoardingFleetModel
    {
        public List<OnBoardingFleetInformation> FuelAssets { get; set; }
        public List<OnBoardingFleetInformation> DefAssets { get; set; }
    }
}
