using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.Web.Controllers;
using SiteFuel.Exchange.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Areas.Supplier.Controllers
{
    public class RegionController : BaseController
    {
        [HttpGet]
        public ActionResult View(int id = 0)
        {
            return View("View", id == 0 ? CurrentUser.CompanyId : id);
        }

        [HttpPost]
        public async Task<JsonResult> Create(RegionViewModel viewModel)
        {
            viewModel.CreatedBy = UserContext.Id;
            viewModel.CompanyId = UserContext.CompanyId;

            if (viewModel.Shifts != null && viewModel.Shifts.Any())
            {
                viewModel.Shifts.ForEach(t =>
                {
                    t.CreatedBy = viewModel.CreatedBy;
                    t.CompanyId = viewModel.CompanyId;
                });
            }

            if (viewModel.Drivers != null && viewModel.Drivers.Any())
            {
                foreach (var driver in viewModel.Drivers)
                {
                    if ((driver.Name.Contains(Resource.lblDriverEmailVerfied)))
                    {
                        var driverName = driver.Name.Replace(Resource.lblDriverEmailVerfied, "");
                        driver.Name = driverName;
                    }
                    else if ((driver.Name.Contains(Resource.lblDriverInvited)))
                    {
                        var driverName = driver.Name.Replace(Resource.lblDriverInvited, "");
                        driver.Name = driverName;
                    }
                }
            }
            var fsDomain = ContextFactory.Current.GetDomain<FreightServiceDomain>();
            var response = await fsDomain.CreateRegion(viewModel);
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public async Task<JsonResult> Update(RegionViewModel viewModel)
        {
            if (viewModel.Drivers != null && viewModel.Drivers.Any())
            {
                foreach (var driver in viewModel.Drivers)
                {
                    if ((driver.Name.Contains(Resource.lblDriverEmailVerfied)))
                    {
                        var driverName = driver.Name.Replace(Resource.lblDriverEmailVerfied, "");
                        driver.Name = driverName;
                    }
                    else if ((driver.Name.Contains(Resource.lblDriverInvited)))
                    {
                        var driverName = driver.Name.Replace(Resource.lblDriverInvited, "");
                        driver.Name = driverName;
                    }
                }
            }
            viewModel.CreatedBy = UserContext.Id;
            viewModel.CompanyId = UserContext.CompanyId;

            var fsDomain = ContextFactory.Current.GetDomain<FreightServiceDomain>();
            var response = await fsDomain.UpdateRegion(viewModel);
            if (response.ScheduleBuilderDetails.Any())
            {
                await ResetDriverScheduleBuilderDriverView(response.ScheduleBuilderDetails);
            }
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public async Task<JsonResult> Delete(string id)
        {
            var fsDomain = ContextFactory.Current.GetDomain<FreightServiceDomain>();
            var response = await fsDomain.DeleteRegion(id, UserContext.Id);
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetRegions()
        {
            var fsDomain = ContextFactory.Current.GetDomain<FreightServiceDomain>();
            var response = await fsDomain.GetRegions(UserContext);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetSourceRegions()
        {
            var response = await ContextFactory.Current.GetDomain<RegionDomain>().GetSourceRegion(UserContext);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> CreateSourceRegion(SourceRegionViewModel viewModel)
        {
            var response = await ContextFactory.Current.GetDomain<RegionDomain>().CreateSourceRegion(UserContext, viewModel);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> UpdateSourceRegion(SourceRegionViewModel viewModel)
        {
            var response = await ContextFactory.Current.GetDomain<RegionDomain>().UpdateSourceRegion(UserContext, viewModel);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> DeleteSourceRegion(int id)
        {
            var response = await ContextFactory.Current.GetDomain<RegionDomain>().DeleteSourceRegion(UserContext.Id, id);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetJobs()
        {
            var response = await ContextFactory.Current.GetDomain<RegionDomain>().GetJobsForCarrierAsync(UserContext);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetDrivers()
        {
            var response = await ContextFactory.Current.GetDomain<RegionDomain>().GetDriversForCarrierAsync(UserContext);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> GetRegionDrivers(string regionID)
        {
            List<DropdownDisplayExtendedItem> response = new List<DropdownDisplayExtendedItem>();
            var fsDomain = ContextFactory.Current.GetDomain<FreightServiceDomain>();
            var driverdetails = await fsDomain.GetDriverDetailsByCompanyId(UserContext.CompanyId, UserContext.Id, regionID);
            if (driverdetails != null)
            {
                response = await ContextFactory.Current.GetDomain<RegionDomain>().GetDriversForRegionCarrierAsync(UserContext, driverdetails.Distinct().ToList());
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> GetDispatchers()
        {
            var response = await ContextFactory.Current.GetDomain<RegionDomain>().GetDispatchersForCarrierAsync(UserContext);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetTrailers()
        {
            var response = await ContextFactory.Current.GetDomain<RegionDomain>().GetTruckAndTrailersForCarrierAsync(UserContext);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetCompanyShifts()
        {
            var response = await ContextFactory.Current.GetDomain<RegionDomain>().GetCompanyShifts(UserContext.CompanyId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public async Task<JsonResult> AddDriverSchedule(DriverScheduleMappingViewModel viewModel)
        {
            var scheduleInfo = viewModel.ScheduleList;
            IntializeRepeatSchedule(scheduleInfo);
            viewModel.CreatedBy = UserContext.Id;
            //viewModel.CompanyId = UserContext.CompanyId;
            var fsDomain = ContextFactory.Current.GetDomain<FreightServiceDomain>();
            var response = await fsDomain.AddDriverSchedule(viewModel);
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        public async Task<JsonResult> UpdateDriverSchedule(List<DriverScheduleMappingViewModel> model, string SelectedDate)
        {
            foreach (var res in model)
            {
                var scheduleInfo = res.ScheduleList;
                IntializeRepeatSchedule(scheduleInfo);
                res.UpdatedBy = UserContext.Id;
                res.UpdatedOn = DateTime.Today;
                res.CompanyId = UserContext.CompanyId;
                res.SelectedDate = SelectedDate;
            }

            //viewModel.CompanyId = UserContext.CompanyId;          
            var fsDomain = ContextFactory.Current.GetDomain<FreightServiceDomain>();
            var response = await fsDomain.UpdateDriverSchedule(model);
            if (response.DsbScheduleBuilderInfo.Any())
            {
                await ResetDriverScheduleBuilderDriverView(response.DsbScheduleBuilderInfo);
            }
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        public async Task<JsonResult> DeleteDriverSchedules(List<DriverScheduleMappingViewModel> driverScheduleMappingViewModels, string SelectedDate)
        {
            foreach (var model in driverScheduleMappingViewModels)
            {
                var scheduleInfo = model.ScheduleList;
                IntializeRepeatSchedule(scheduleInfo);
                model.CreatedBy = UserContext.Id;
                model.CompanyId = UserContext.CompanyId;
                model.SelectedDate = SelectedDate;
            }

            var fsDomain = ContextFactory.Current.GetDomain<FreightServiceDomain>();
            var response = await fsDomain.DeleteDriverSchedules(driverScheduleMappingViewModels);
            if (response.DsbScheduleBuilderInfo.Any())
            {
                await ResetDriverScheduleBuilderDriverView(response.DsbScheduleBuilderInfo);
            }
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        /// <summary>
        /// used for convert repeat day UTC list to current time zone.
        /// </summary>
        /// <param name="scheduleInfo"></param>

        private static void IntializeRepeatSchedule(List<DriverScheduleViewModel> scheduleInfo)
        {
            foreach (var scheduleItem in scheduleInfo)
            {
                List<DateTimeOffset> repeatDayList = new List<DateTimeOffset>();
                if (scheduleItem.RepeatDayStringList != null && scheduleItem.RepeatDayStringList.Any() && scheduleItem.TypeId != 2)
                {
                    foreach (var item in scheduleItem.RepeatDayStringList)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            DateTime dateFilter = DateTimeOffset.Now.Date;
                            if (!string.IsNullOrWhiteSpace(item))
                            {
                                dateFilter = Convert.ToDateTime(item).Date;
                            }
                            repeatDayList.Add(dateFilter);
                        }
                    }
                    scheduleItem.RepeatDayList = repeatDayList.Select(t => t.DateTime).ToList();
                }
                else
                {
                    scheduleItem.RepeatDayList = GetDatesBetween(scheduleItem.StartDate.Date, scheduleItem.EndDate.Date);
                }
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetRegionsDdl()
        {
            var fsDomain = ContextFactory.Current.GetDomain<FreightServiceDomain>();
            var response = await fsDomain.GetRegionsDdl(UserContext.CompanyId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetCarriers()
        {
            var response = CommonHelperMethods.GetCarriers(CurrentUser.CompanyId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> AddRegionSchedule(RegionScheduleViewModel viewModel)
        {
            var response = new StatusViewModel();

            viewModel.CreatedBy = UserContext.Id;
            viewModel.CreatedOn = System.DateTime.Today;
            var fsDomain = ContextFactory.Current.GetDomain<FreightServiceDomain>();
            response = await fsDomain.AddRegionSchedule(viewModel);
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetResionShiftSchedulesDetails(string regionId, string routeId)
        {
            var response = new List<RegionScheduleViewModel>();
            var fsDomain = ContextFactory.Current.GetDomain<FreightServiceDomain>();
            response = await fsDomain.GetResionShiftSchedulesDetails(regionId, routeId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> GetTerminalsAndBulkPlantsByRegion(SourceRegionRequestModel inputModel)
        {
            var regionDomain = ContextFactory.Current.GetDomain<RegionDomain>();
            var response = await regionDomain.GetTerminalsAndBulkPlantsByRegion(UserContext.CompanyId, inputModel);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #region  ResetDriverScheduleBuilderDriverView
        private async Task<StatusViewModel> ResetDriverScheduleBuilderDriverView(List<ScheduleBuilderViewModel> scheduleBuilders)
        {
            List<int> trackCompleted = new List<int> { 7, 8, 9, 10, 22 };
            List<int> enrouteInProgress = new List<int> { 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 };
            StatusViewModel statusViewModel = new StatusViewModel();
            var scheduleBuilderDomain = ContextFactory.Current.GetDomain<ScheduleBuilderDomain>();
            //update the dsb schedule builder model with draft version.
            List<int> DeliveryGroupIds = new List<int>();
            foreach (var scItem in scheduleBuilders)
            {
                bool publishedTrip = false;
                bool isModification = true;
                if (scItem.Trips.Any(x => x.DeliveryGroupStatus == DeliveryGroupStatus.Published || x.DeliveryGroupPrevStatus == DeliveryGroupStatus.Published || x.DeliveryRequests.Any(dr => dr.Status == (int)DeliveryReqStatus.ScheduleCreated)))
                {
                    publishedTrip = true;
                }
                if (scItem.Trips.Any(x => x.DeliveryRequests.Any(dr => trackCompleted.Contains(dr.TrackScheduleStatus))))
                {
                    isModification = false;
                }
                else if (scItem.Trips.Any(x => x.DeliveryRequests.Any(dr => enrouteInProgress.Contains(dr.TrackScheduleStatus))))
                {
                    isModification = false;
                }
                //get DeliveryRequest group Id 
                var deliveryGroupInfo = scItem.Trips.SelectMany(x => x.DeliveryRequests.Select(top => top.DeliveryGroupId)).Distinct().ToList();
                if (isModification)
                {
                    foreach (var item in scItem.Trips)
                    {
                        item.Drivers = new List<DriverAdditionalDetailsViewModel>();
                        item.IsIncludeAllRegionDriver = false;
                        //item.Trailers = new List<TrailerModel>(); - as per discussion we are removed trailer. meeting id : fyq-qinf-bmx
                        item.GroupId = 0;
                        item.TripStatus = TripStatus.Modified;
                        item.DeliveryGroupStatus = DeliveryGroupStatus.Draft;
                        item.DeliveryGroupPrevStatus = DeliveryGroupStatus.None;
                        item.DeliveryRequests.ForEach(t => { t.ScheduleStatus = 14; t.Status = (int)DeliveryReqStatus.Draft; t.DeliveryGroupId = null; t.DeliveryScheduleId = null; t.TrackableScheduleId = null; t.TrackScheduleStatus = 0; });
                    }
                    foreach (var item in deliveryGroupInfo)
                    {
                        if (item != null)
                        {
                            DeliveryGroupIds.Add(Convert.ToInt32(item.Value));
                        }
                    }
                    UserContext userContext = new UserContext();
                    userContext.Id = UserContext.Id;
                    userContext.CompanyId = scItem.CompanyId;
                    statusViewModel = await scheduleBuilderDomain.ResetDsbScheduleBuilderDriver(DeliveryGroupIds, scItem, publishedTrip, userContext);
                }
                else
                {
                    //Not Remove Driver from schedule as there are some completed drops or driver on the way.
                    statusViewModel.StatusCode = Status.Warning;
                }

            }
            return statusViewModel;
        }

        #endregion

        public static List<DateTime> GetDatesBetween(DateTime startDate, DateTime endDate)
        {
            List<DateTime> allDates = new List<DateTime>();
            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                if ((date.DayOfWeek != DayOfWeek.Saturday) && (date.DayOfWeek != DayOfWeek.Sunday))
                {
                    allDates.Add(date);
                }
            }
            return allDates;

        }
        [HttpGet]
        public async Task<JsonResult> GetMstFuelProducts()
        {
            var response = await ContextFactory.Current.GetDomain<MasterDomain>().GetMstFuelProducts();
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> IsPublishedDR(string productIds, string fuelTypeIds)
        {    
            var fsDomain = ContextFactory.Current.GetDomain<FreightServiceDomain>();
            var response = await fsDomain.IsPublishedDR(UserContext.CompanyId, productIds, fuelTypeIds);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }

}