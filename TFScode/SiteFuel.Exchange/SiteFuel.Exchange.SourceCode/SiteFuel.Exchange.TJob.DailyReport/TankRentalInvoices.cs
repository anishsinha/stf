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
    public class TankRentalInvoices
    {
        public TankRentalInvoices()
        {
            //Register Context
            ContextFactory.Register(new ApplicationContext());
        }

        public async Task<bool> ProcessTankRentalScheduleInvoices()
        {
            using (var tracer = new Tracer("TankRentalInvoices", "ProcessTankRentalScheduleInvoices"))
            {
                try
                {
                    var response = await new TankRentalInvoiceDomain().AutoAddTankRentalInvoiceCreateMessage();
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("TankRentalInvoices", "ProcessTankRentalScheduleInvoices", "Exception Details : ", ex);
                }
                return true;
            }
        }

    }
}

