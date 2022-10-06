using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.TJob.DailyReports
{
    public class ExceptionAutoApproval
    {
        public ExceptionAutoApproval()
        {
            //Register Context
            ContextFactory.Register(new ApplicationContext());
        }

        public async Task<bool> ProcessExceptionsForAutoApproval()
        {
            using (var tracer = new Tracer("ExceptionAutoApproval", "ProcessExceptionsForAutoApproval"))
            {
                try
                {
                    var response = await new ExceptionDomain().AutoApproveExceptions();

                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("ExceptionAutoApproval", "ProcessExceptionsForAutoApproval", "Exception Details : ", ex);
                }
                return true;
            }
        }

        public async Task<bool> ProcessExceptionsForAutoReject()
        {
            using (var tracer = new Tracer("ExceptionAutoApproval", "ProcessExceptionsForAutoReject"))
            {
                try
                {
                    var unassignedDdtResponse = await new ExceptionDomain().AutoDiscardUnAssignDDT();
                    var unknownDeliveriesResponse = await new ExceptionDomain().AutoDiscardUnknownDeliveries();
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("ExceptionAutoApproval", "ProcessExceptionsForAutoReject", "Exception Details : ", ex);
                }
                return true;
            }
        }
    }
}

