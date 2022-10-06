using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Logger;
using System;
using System.Collections.Generic;
using SiteFuel.Exchange.Domain;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SiteFuel.Exchange.ViewModels;

namespace SiteFuel.Exchange.TJob.DsbLoadQueue
{
    public class ProcessSAPOrderStatus
    {
        public ProcessSAPOrderStatus()
        {
            //Register Context
            ContextFactory.Register(new ApplicationContext());
        }

        public async Task ProcessOrderStatus()
        {
            var watch = Stopwatch.StartNew();
            try
            {
                var domain = ContextFactory.Current.GetDomain<HeldDrQueueDomain>();
                var requests = await domain.GetUnProcessedOrderStatusRequest();

                foreach (var request in requests)
                {
                    await domain.UpdateHeldDrCreditCheckStatus(JsonConvert.DeserializeObject<SalesOrderStatusModel>(request.JsonRequest));
                    await domain.UpdateIsProcessedColumn(request.Id);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ProcessSAPOrderStatus", "ProcessOrderStatus", "Exception Details : ", ex);
            }
            watch.Stop();
            LogManager.Logger.WriteInfo("CJob.ProcessSAPOrderStatus", "ProcessOrderStatus", "End:TotalTime:" + watch.ElapsedMilliseconds);
        }
    }
}
