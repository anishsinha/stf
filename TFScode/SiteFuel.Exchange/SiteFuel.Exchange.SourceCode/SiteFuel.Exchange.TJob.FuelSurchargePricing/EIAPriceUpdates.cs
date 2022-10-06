using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using System;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.TJob.FuelSurchargePricing
{
    public class EIAPriceUpdates
    {
        public EIAPriceUpdates()
        {
            //Register Context
            ContextFactory.Register(new ApplicationContext());
        }

        public async Task<bool> GetEaiPriceUpdates()
        {
            using (var tracer = new Tracer("EaiPriceUpdates", "GetEaiPriceUpdates"))
            {
                try
                {
                    var response = await new EIAPriceUpdateDomain().GetEIAPriceUpdates();
                    return response;
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("EaiPriceUpdates", "GetEaiPriceUpdates", "Exception Details : ", ex);
                }
                return true;
            }
        }

    }
}

