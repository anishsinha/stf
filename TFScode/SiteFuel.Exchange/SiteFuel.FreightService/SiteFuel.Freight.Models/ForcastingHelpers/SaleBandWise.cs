using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels.ForcastingHelpers
{
    public class SaleBandWise : ISaleBandWise
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime StartTime { get; set; }
        public string SlabName { get; set; }
        public decimal Week1 { get; set; }
        public decimal Week2 { get; set; }
        public decimal Week3 { get; set; }
        public decimal Week4 { get; set; }
    }
}
