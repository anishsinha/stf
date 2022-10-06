using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueFill.Exchange.TJob.LFVServices
{
    public class LFVService
    {
        public LFVService()
        {
            //Register Context
            ContextFactory.Register(new ApplicationContext());
        }
        public async Task<bool> ProcessLiftFileValidate()
        {
            bool response = false;
            using (var tracer = new Tracer("LFVService", "ProcessLiftFileValidate"))
            {
                try
                {
                    var lfvDomain = new LFVDomain();
                    response = await lfvDomain.ProcessMatchPendingLfvRecords();
                    response = await lfvDomain.ProcessFailedPostApiCalls();
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("LFVService", "ProcessLiftFileValidate", "Lift File Details : ", ex);
                }
            }
            return response;
        }
    }
}
