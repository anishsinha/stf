using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels.ForcastingHelpers
{
    public interface ISalesCalculatorDaily : ISalesCalculator
    {
        ISale24HourModel Calculate(SaleTankModel tank);
    }
}
