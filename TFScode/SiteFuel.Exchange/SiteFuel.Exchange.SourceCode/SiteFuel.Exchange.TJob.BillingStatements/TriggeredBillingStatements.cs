using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.TJob.BillingStatements
{
    public class TriggeredBillingStatements
    {
        public TriggeredBillingStatements()
        {
            //Register Context
            ContextFactory.Register(new ApplicationContext());
        }

        public async Task<bool> ProcessStatementGeneration()
        {
            using (var tracer = new Tracer("TriggeredBillingStatements", "ProcessStatementGeneration"))
            {
                try
                {
                    var billingScheduleDomain = ContextFactory.Current.GetDomain<BillingScheduleDomain>();
                    List<int> billingScheduleIds = billingScheduleDomain.GetBillingSchedules();

                    foreach (var billingSchedule in billingScheduleIds)
                    {
                        await billingScheduleDomain.ProcessBillingSchedulesAsync(billingSchedule);
                        await billingScheduleDomain.ProcessBillingSchedulesForEditedInvoiceAsync(billingSchedule);
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("TriggeredBillingStatements", "ProcessStatementGeneration", ex.Message, ex);
                }
                return true;
            }
        }
    }
}
