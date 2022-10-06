using SiteFuel.Exchange.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain
{
    public class ForcastingCalculationDomain : BaseDomain
    {
        public ForcastingCalculationDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public ForcastingCalculationDomain(BaseDomain domain) : base(domain)
        {
        }


    }
}
