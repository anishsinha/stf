using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels.ForcastingHelpers
{
    public interface ISalesCalculatorMonthly : ISalesCalculator
    {
        List<ISale24HourModel> Calculate(SaleTankModel tank);
    }
}
