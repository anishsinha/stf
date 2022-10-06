using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels.ForcastingHelpers
{
    public interface ISalesCalculator
    {
        DateTime Date { get; set; }
        ForecastingSettings ForecastingSettings { get; set; }
        List<IDemandModel> Demands { get; set; }
        List<ITankDropModel> TankDrops { get; set; }
    }
}
