using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueFill.Exchange.TJob.DiptestServices
{
    public class DiptestService
    {
        public DiptestService()
        {
            //Register Context
            ContextFactory.Register(new ApplicationContext());
        }
        public async Task<bool> ProcessIs360Data()
        {
            bool response = false;
            using (var tracer = new Tracer("DiptestService", "ProcessIs360Data"))
            {
                try
                {
                    var pedigreeDomain = new PedigreeDomain();
                    response = await pedigreeDomain.ProcessIS360();
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("DiptestService", "ProcessIs360Data", "ProcessIs360Data : ", ex);
                }
            }
            return response;
        }
    }
}
