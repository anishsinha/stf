using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels.ForcastingHelpers
{
    public interface ISaleBandWise
    {
        int Id { get; set; }
        DateTime Date { get; set; }
        DateTime StartTime { get; set; }
        string SlabName { get; set; }
        decimal Week1 { get; set; }
        decimal Week2 { get; set; }
        decimal Week3 { get; set; }
        decimal Week4 { get; set; }
    }
}
