using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain
{
    public class CalendarDomain : BaseDomain
    {
        public CalendarDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public CalendarDomain(BaseDomain domain) : base(domain)
        {
        }

        public async Task<List<UspSupplierOrderCalendarEvent>> GetSupplierOrderCalendarEvents(Usp_CalenderEventViewModel calEventData, int timeout = 30)
        {
            var response = new List<UspSupplierOrderCalendarEvent>();
            try
            {
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetSupplierOrderCalenderEvents", calEventData);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspSupplierOrderCalendarEvent>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CalendarDomain", "GetSupplierOrderCalendarEvents", ex.Message, ex);
            }
            return response;
        }

        public List<UspSupplierOrderCalendarEvent> GetCalendarFutureSchedules(Usp_CalenderEventViewModel calEventData, int timeout = 30)
        {
            var response = new List<UspSupplierOrderCalendarEvent>();
            try
            {
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetSupplierCalendarFutureSchedules", calEventData);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = Context.DataContext.Database.SqlQuery<UspSupplierOrderCalendarEvent>(input.Query, input.Params.ToArray()).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CalendarDomain", "GetCalendarFutureSchedulesAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<CalenderViewModel>> GetSupplierCalenderInvoicesAsync(Usp_CalenderEventViewModel calEventData, int timeout = 30)
        {
            var response = new List<CalenderViewModel>();
            try
            {
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetSupplierCalendarInvoices", calEventData);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<CalenderViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CalendarDomain", "GetSupplierCalenderInvoicesAsync", ex.Message, ex);
            }

            return response;
        }
    }
}
