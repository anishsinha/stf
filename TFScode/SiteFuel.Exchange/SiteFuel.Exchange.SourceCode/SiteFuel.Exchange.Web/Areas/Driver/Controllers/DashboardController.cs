using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.Web.Attributes;
using SiteFuel.Exchange.Web.Controllers;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Linq;
using SiteFuel.Exchange.Core.Logger;
using System;

namespace SiteFuel.Exchange.Web.Areas.Driver.Controllers
{
    public class DashboardController : BaseController
    {
        // GET: Driver/Dashboard
        public ActionResult Index()
        {          
            return View("Index");
        }

        public async Task<JsonResult> GetCalenderData(int month = 0, int year = 0)
        {
            using (var tracer = new Tracer("DashboardController", "GetCalenderData"))
            {
                var response = await ContextFactory.Current.GetDomain<DashboardDomain>().GetDriverCalendarDataAsync(CurrentUser.CompanyId, CurrentUser.Id, month, year, 1);
                foreach (var data in response)
                {
                    data.url = Url.Action("Details", "Order", new { area = "Driver", id = data.id });
                }

                return new JsonResult
                {
                    Data = response.ToArray(),
                    MaxJsonLength = int.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }


        [HttpGet]
        public async Task<ActionResult> RecentDropsHistoryGrid()
        {
            var dashboardDriverDropsHistoryViewModel = new DashboardDriverDropsHistoryViewModel();
            using (var tracer = new Tracer("DashboardController", "RecentDropsHistoryGrid"))
            {
                List<int> driverIds = new List<int>();
                driverIds.Add(CurrentUser.Id);

                // pass Currency - 0 and All - 0 to get all deliveries of USA and canada
                var response = await ContextFactory.Current.GetDomain<DispatchDomain>().GetCompletedDeliveriesForSupplierAsync(CurrentUser.CompanyId, driverIds, "", "", Currency.None, (int)Country.All); 
                dashboardDriverDropsHistoryViewModel.TotalMissedDrops = ContextFactory.Current.GetDomain<DashboardDomain>().GetMissedSchedulesCount(CurrentUser.Id);
                dashboardDriverDropsHistoryViewModel.TotalDrops = response.Count;
                dashboardDriverDropsHistoryViewModel.RecentDrops = response.Take(10).ToList();
                dashboardDriverDropsHistoryViewModel.TotalOnTimeDrops = response.Count(t => t.ScheduleStatusId == (int)DeliveryScheduleStatus.Completed || t.ScheduleStatusId == (int)DeliveryScheduleStatus.RescheduledCompleted);
                dashboardDriverDropsHistoryViewModel.TotalLateDrops = response.Count(t => t.ScheduleStatusId == (int)DeliveryScheduleStatus.CompletedLate);
                dashboardDriverDropsHistoryViewModel.TotalDiscontinuedDrops = response.Count(t => t.ScheduleStatusId == (int)DeliveryScheduleStatus.Discontinued);
                dashboardDriverDropsHistoryViewModel.TotalDropsWithOverage = response.Count(t => t.Overage > 0);
            }
            return Json(dashboardDriverDropsHistoryViewModel, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> TimeCardGrid(TimeCardView timeCardView = TimeCardView.All)
        {
            using (var tracer = new Tracer("DashboardController", "TimeCardGrid"))
            {
                if (timeCardView == TimeCardView.Day)
                {
                    var response = await ContextFactory.Current.GetDomain<TimeCardDomain>().GetTimeCardGridDataAsync(CurrentUser.CompanyId, CurrentUser.Id, null, null);
                    response = response.Where(t => DateTimeOffset.FromUnixTimeMilliseconds(t.UtcActionDate).Date == DateTimeOffset.UtcNow.AddDays(-1).Date).OrderBy(t => t.Id).ToList();
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    List<int> driverIds = new List<int>();
                    driverIds.Add(CurrentUser.Id);

                    var response = await ContextFactory.Current.GetDomain<TimeCardDomain>().GetTimeCardActionSummaryForAllDrivers(driverIds, CurrentUser.CompanyId, null, null);
                    response = response.OrderByDescending(t => Convert.ToDateTime(t.ActionDate)).Take(10).ToList();
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public async Task<JsonResult> GetMySchedulesAsync(string dtSelected)
        {
            using (var tracer = new Tracer("DashboardController", "GetMySchedulesAsync"))
            {
                var response = await ContextFactory.Current.GetDomain<DashboardDomain>().GetDriverDeliverySchedulesAsync(CurrentUser.CompanyId, CurrentUser.Id, dtSelected);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
    }
}