using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
   public class FuelRetainViewModel
    {
        public int DriverId { get; set; }
        public List<TrailerFuelRetainViewModel> TrailerFuelRetain { get; set; } = new List<TrailerFuelRetainViewModel>();
        public List<PreLoadBolViewModel> BolDetails { get; set; } = new List<PreLoadBolViewModel>();
    }
}
