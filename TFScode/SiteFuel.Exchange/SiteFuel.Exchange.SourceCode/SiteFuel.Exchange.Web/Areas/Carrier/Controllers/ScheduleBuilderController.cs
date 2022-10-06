using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.DispatchScheduler;
using SiteFuel.Exchange.ViewModels.OttoSchedule;
using SiteFuel.Exchange.Web.Areas.Dispatcher.Models;
using SiteFuel.Exchange.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Areas.Carrier.Controllers
{
    public class ScheduleBuilderController : BaseController
    {
        // GET: Carrier/ScheduleBuilder
        static int defaultScheduleBuilderView = 2;
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DeliveryRequests(string regionId)
        {
            return View();
        }

        public async Task<JsonResult> GetRegions()
        {

            using (var tracer = new Tracer("ScheduleBuilderController", $"GetRegions(userId:{CurrentUser.Id})"))
            {
                var response = await ContextFactory.Current.GetDomain<ScheduleBuilderDomain>().GetRegions(CurrentUser.Id);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetRegionsOfCompany(int companyId)
        {
            using (var tracer = new Tracer("ScheduleBuilderController", $"GetRegions(userId:{CurrentUser.Id})"))
            {
                var fsDomain = ContextFactory.Current.GetDomain<FreightServiceDomain>();
                var response = await fsDomain.GetRegionsDdl(companyId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> GetRegionDetails(string regionId)
        {
            var response = await ContextFactory.Current.GetDomain<ScheduleBuilderDomain>().GetRegionDetails(regionId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> IsFilldCompatibleOrder(List<int> orderIds)
        {
            using (var tracer = new Tracer("ScheduleBuilderController", $"IsFilldCompatibleOrder(orderID:)"))
            {
                var response = await ContextFactory.Current.GetDomain<ScheduleBuilderDomain>().IsFilldCompatibleOrder(orderIds);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<JsonResult> GetOrders(int jobId, int productTypeId, string startDate = null, int carrierStatus = -1)
        {
            using (var tracer = new Tracer("ScheduleBuilderController", $"GetOrders(jobId:{jobId} ,productTypeId:{productTypeId} ,startDate:{startDate},carrierStatus:{carrierStatus})"))
            {
                DateTimeOffset? loadDate = null;
                if (!string.IsNullOrWhiteSpace(startDate))
                {
                    loadDate = Convert.ToDateTime(startDate);
                }
                var response = await ContextFactory.Current.GetDomain<ScheduleBuilderDomain>().GetOrderDetails(jobId, productTypeId, loadDate, UserContext, carrierStatus);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> GetOrderDetailsForEditDeliveryGroup(int jobId, int productTypeId, string startDate = null, int carrierStatus = -1, bool isBlendReq = false)
        {
            using (var tracer = new Tracer("ScheduleBuilderController", $"GetOrderDetailsForEditDeliveryGroup(jobId:{jobId} ,productTypeId:{productTypeId} ,startDate:{startDate},carrierStatus:{carrierStatus})"))
            {
                DateTimeOffset? loadDate = null;
                if (!string.IsNullOrWhiteSpace(startDate))
                {
                    loadDate = Convert.ToDateTime(startDate);
                }
                var response = await ContextFactory.Current.GetDomain<ScheduleBuilderDomain>().GetOrderDetailsForEditDeliveryGroup(jobId, productTypeId, loadDate, UserContext, carrierStatus, isBlendReq);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public async Task<JsonResult> GetJobWithOrders(string regionId, int tfxProductId, int productTypeId, int? terminalId, int? bulkplantId, string startDate = null)
        {
            DateTimeOffset? loadDate = null;
            if (!string.IsNullOrWhiteSpace(startDate))
            {
                loadDate = Convert.ToDateTime(startDate);
            }
            var response = await ContextFactory.Current.GetDomain<ScheduleBuilderDomain>().GetJobWithOrders(regionId, tfxProductId, productTypeId, terminalId, bulkplantId, loadDate, UserContext);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetSheduleBuilder(string regionId, string date = "", string scheduleBuilderId = "", int sbView = 1, int sbDsbView = 2)
        {
            var startTime = DateTime.Now;
            var requestModelJson = $"{{regionId:{regionId}, date: {date}, scheduleBuilderId: {scheduleBuilderId}, sbView:{sbView}}}";

            var apiResponse = await ContextFactory.Current.GetDomain<ScheduleBuilderDomain>().GetScheduleBuilderData(regionId, date, scheduleBuilderId, sbView, sbDsbView == 1 ? defaultScheduleBuilderView : sbDsbView, UserContext);
            if (apiResponse != null)
            {
                if (CurrentUser.CompanyTypeId == CompanyType.BuyerSupplierAndCarrier || CurrentUser.CompanyTypeId == CompanyType.SupplierAndCarrier || CurrentUser.CompanyTypeId == CompanyType.Carrier)
                {
                    apiResponse.Trips.FindAll(t => t.TripId == null).ForEach(t => t.Carrier = CurrentUser.CompanyName);
                }
                if (sbView == (int)ScheduleBuilderView.None)
                {
                    sbView = apiResponse.ObjectFilter;
                }
                if (sbView == (int)ScheduleBuilderView.Driver)
                {
                    var response = ScheduleBuilderConverter.ConvertToDriverViewModel(apiResponse);
                    response.ObjectFilter = sbView;
                    response.StatusCode = apiResponse.StatusCode;
                    response.Status = DSBMethod.None;
                    var dsbScheduleLoadQueueDetails = await ContextFactory.Current.GetDomain<ScheduleBuilderDomain>().GetScheduleLoadQueueStatus(response.Id, response.Date, response.RegionId, UserContext.CompanyId, UserContext.Id);
                    IntializeLoadQueueColumnStatus(response, dsbScheduleLoadQueueDetails);
                    var responseModelJson = JsonConvert.SerializeObject(response);
                    LogManager.Logger.WriteAPIInfo(CurrentUser.UserName, "ScheduleBuilderController", "GetSheduleBuilder", requestModelJson, responseModelJson, 0, "Azure-AppService", startTime, DateTime.Now);
                    return new JsonResult
                    {
                        Data = response,

                        MaxJsonLength = int.MaxValue,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
                else
                {
                    var response = ScheduleBuilderConverter.ConvertToTrailerViewModel(apiResponse);
                    response.ObjectFilter = sbView;
                    response.StatusCode = apiResponse.StatusCode;

                    var responseModelJson = JsonConvert.SerializeObject(response);
                    LogManager.Logger.WriteAPIInfo(CurrentUser.UserName, "ScheduleBuilderController", "GetSheduleBuilder", requestModelJson, responseModelJson, 0, "Azure-AppService", startTime, DateTime.Now);

                    return Json(response, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new StatusViewModel() { StatusMessage = Resource.valMessageServiceNotResponded }, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetSheduleCalendarData(string regionId, string date = "")
        {
            var response = await ContextFactory.Current.GetDomain<ScheduleBuilderDomain>().GetScheduleCalendarData(regionId, date, string.Empty, 1, 2, UserContext);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> SaveCalendarDeliveryRequest(CalendarScheduleViewModel inputModel)
        {
            var response = new StatusViewModel();
            using (var tracer = new Tracer("ScheduleBuilderController", "SaveCalendarDeliveryRequest"))
            {
                try
                {
                    response = await ContextFactory.Current.GetDomain<ScheduleBuilderDomain>().SaveCalendarDeliveryRequest(inputModel, UserContext);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("ScheduleBuilderController", "SaveCalendarDeliveryRequest", ex.Message, ex);
                }
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> SaveDriverView()
        {
            List<DsbLoadQueueViewModel> dsbLoadQueueViewModel = new List<DsbLoadQueueViewModel>();
            List<DsbLoadQueueStatusViewModel> dsbLoadQueueStatusModel = new List<DsbLoadQueueStatusViewModel>();
            var sbDriverViewModel = new DSBSaveModel();
            try
            {
                var startTime = DateTime.Now;
                sbDriverViewModel = await GetViewModel<DSBSaveModel>(Request.InputStream);
                sbDriverViewModel.CompanyId = UserContext.CompanyId;
                sbDriverViewModel.UserId = UserContext.Id;
                sbDriverViewModel.Trips.ForEach(t => t.UpdatedByName = CurrentUser.Name);
                sbDriverViewModel.Trips.ForEach(t => t.IsDsbLoadQueueBackgroundProcess = false);
                var requestModelJson = JsonConvert.SerializeObject(sbDriverViewModel);
                UpdateBadgeNumberToDeliveryRequest(sbDriverViewModel);
                var sbDomain = new ScheduleBuilderDomain();
                await sbDomain.UpdateBulkPlantAddressForCarribean(sbDriverViewModel);
                if (sbDriverViewModel != null)
                {
                    string language = Request.UserLanguages == null ? "en-us" : Request.UserLanguages[0];
                    IntializeDsbSaveModel(dsbLoadQueueViewModel, sbDriverViewModel, language);
                    //validate the dsb load queue model.
                    dsbLoadQueueStatusModel = await ContextFactory.Current.GetDomain<DsbLoadQueueDomain>().ValidateLoadQueue(dsbLoadQueueViewModel, UserContext);
                }
                if (!dsbLoadQueueStatusModel.Any())
                {
                    var apiResponse = await sbDomain.SaveScheduleBuilder(sbDriverViewModel, UserContext);
                    if (apiResponse != null)
                    {
                        if (apiResponse.StatusCode != Status.Failed)
                        {
                            foreach (var trip in apiResponse.Trips)
                            {
                                if (trip.IsDispatcherDragDropSequence == true && trip.IsDispatcherDragDropSequenceModified == true)
                                {
                                    int dropSequence = 1;
                                    trip.DeliveryRequests.ForEach(x =>
                                    {
                                        x.IsDispatcherDragDrop = true;
                                        x.DispatcherDragDropSequence = dropSequence;
                                        dropSequence = dropSequence + 1;
                                    });
                                }
                                ScheduleBuilderConverter.InitializePickupLocation(trip);
                                ScheduleBuilderConverter.InitializeBadgeNumberForDeliveryRequests(trip);
                            }
                        }
                        sbDriverViewModel = apiResponse;
                        sbDriverViewModel.Status = DSBMethod.None;
                    }
                    else
                    {
                        sbDriverViewModel.StatusCode = Status.Failed;
                        sbDriverViewModel.StatusMessage = Resource.valMessageServiceNotResponded;
                    }
                }
                else
                {
                    var statusMessage = dsbLoadQueueStatusModel.SelectMany(top => top.Messages).Select(top => top.StatusMessage).ToList();
                    sbDriverViewModel.StatusCode = Status.Failed;
                    string message = string.Empty;
                    foreach (var item in statusMessage)
                    {
                        if (string.IsNullOrEmpty(message))
                            message = message + item;
                        else
                            message = message + "</br>" + item;
                    }
                    sbDriverViewModel.StatusMessage = message;
                }
                var responseModelJson = JsonConvert.SerializeObject(sbDriverViewModel);
                LogManager.Logger.WriteAPIInfo(CurrentUser.UserName, "ScheduleBuilderController", "SaveDriverView", requestModelJson, responseModelJson, 0, "Azure-AppService", startTime, DateTime.Now);
            }
            catch (Exception ex)
            {
                sbDriverViewModel.StatusCode = Status.Failed;
                sbDriverViewModel.StatusMessage = Resource.valMessageErrorOccurred;
                LogManager.Logger.WriteException("ScheduleBuilderController", "SaveSheduleBuilder-DriverView", ex.Message, ex);
            }
            return Json(sbDriverViewModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> SaveTrailerView()
        {
            using (var tracer = new Tracer("ScheduleBuilderController", "SaveSheduleBuilder-TrailerView"))
            {
                var sbTrailerViewModel = new DSBSaveModel();
                try
                {
                    var startTime = DateTime.Now;
                    sbTrailerViewModel = await GetViewModel<DSBSaveModel>(Request.InputStream);
                    sbTrailerViewModel.CompanyId = UserContext.CompanyId;
                    sbTrailerViewModel.UserId = UserContext.Id;

                    var requestModelJson = JsonConvert.SerializeObject(sbTrailerViewModel);

                    UpdateBadgeNumberToDeliveryRequest(sbTrailerViewModel);
                    var domain = new ScheduleBuilderDomain();
                    await domain.UpdateBulkPlantAddressForCarribean(sbTrailerViewModel);
                    ScheduleBuilderViewModel apiResponse = null;// await ContextFactory.Current.GetDomain<ScheduleBuilderDomain>().SaveScheduleBuilder(scheduleBuilder, UserContext);
                    if (apiResponse != null)
                    {
                        if (UserContext.CompanyTypeId == CompanyType.Carrier || UserContext.CompanyTypeId == CompanyType.BuyerSupplierAndCarrier || UserContext.CompanyTypeId == CompanyType.SupplierAndCarrier)
                        {
                            apiResponse.Trips.FindAll(t => t.TripId == null).ForEach(t => t.Carrier = UserContext.CompanyName);
                        }
                        sbTrailerViewModel.StatusCode = apiResponse.StatusCode;
                        sbTrailerViewModel.StatusMessage = apiResponse.StatusMessage;
                    }
                    else
                    {
                        sbTrailerViewModel.StatusCode = Status.Failed;
                        sbTrailerViewModel.StatusMessage = Resource.valMessageServiceNotResponded;
                    }
                    var responseModelJson = JsonConvert.SerializeObject(sbTrailerViewModel);
                    LogManager.Logger.WriteAPIInfo(CurrentUser.UserName, "ScheduleBuilderController", "SaveTrailerView", requestModelJson, responseModelJson, 0, "Azure-AppService", startTime, DateTime.Now);
                }
                catch (Exception ex)
                {
                    sbTrailerViewModel.StatusCode = Status.Failed;
                    sbTrailerViewModel.StatusMessage = Resource.valMessageErrorOccurred;
                    LogManager.Logger.WriteException("ScheduleBuilderController", "SaveSheduleBuilder-TrailerView", ex.Message, ex);
                }
                return Json(sbTrailerViewModel, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> PublishDriverView()
        {
            List<DsbLoadQueueViewModel> dsbLoadQueueViewModel = new List<DsbLoadQueueViewModel>();
            List<DsbLoadQueueStatusViewModel> dsbLoadQueueStatusModel = new List<DsbLoadQueueStatusViewModel>();
            var sbDriverViewModel = new DSBSaveModel();
            try
            {
                string language = Request.UserLanguages == null ? "en-us" : Request.UserLanguages[0];
                var startTime = DateTime.Now;
                sbDriverViewModel = await GetViewModel<DSBSaveModel>(Request.InputStream);
                sbDriverViewModel.CompanyId = UserContext.CompanyId;
                sbDriverViewModel.UserId = UserContext.Id;
                sbDriverViewModel.Trips.ForEach(t => t.UpdatedByName = CurrentUser.Name);
                sbDriverViewModel.Trips.ForEach(t => t.IsDsbLoadQueueBackgroundProcess = false);
                var requestModelJson = JsonConvert.SerializeObject(sbDriverViewModel);
                var domain = new ScheduleBuilderDomain();
                await domain.UpdateBulkPlantAddressForCarribean(sbDriverViewModel);
                if (sbDriverViewModel != null)
                {
                    IntializeDsbSaveModel(dsbLoadQueueViewModel, sbDriverViewModel, language);
                    //validate the dsb load queue model.
                    dsbLoadQueueStatusModel = await ContextFactory.Current.GetDomain<DsbLoadQueueDomain>().ValidateLoadQueue(dsbLoadQueueViewModel, UserContext);
                }
                if (!dsbLoadQueueStatusModel.Any())
                {
                    var apiResponse = await domain.PublishScheduleBuilder(sbDriverViewModel, UserContext, language);
                    if (apiResponse != null)
                    {
                        if (apiResponse.StatusCode != Status.Failed)
                        {
                            foreach (var trip in apiResponse.Trips)
                            {
                                if (trip.IsDispatcherDragDropSequence == true && trip.IsDispatcherDragDropSequenceModified == true)
                                {
                                    int dropSequence = 1;
                                    trip.DeliveryRequests.ForEach(x =>
                                    {
                                        x.IsDispatcherDragDrop = true;
                                        x.DispatcherDragDropSequence = dropSequence;
                                        dropSequence = dropSequence + 1;
                                    });
                                }
                                ScheduleBuilderConverter.InitializePickupLocation(trip);
                                ScheduleBuilderConverter.InitializeBadgeNumberForDeliveryRequests(trip);
                            }
                        }
                        sbDriverViewModel = apiResponse;
                        sbDriverViewModel.Status = DSBMethod.None;
                    }
                    else
                    {
                        sbDriverViewModel.StatusCode = Status.Failed;
                        sbDriverViewModel.StatusMessage = Resource.valMessageServiceNotResponded;
                    }
                }
                else
                {
                    var statusMessage = dsbLoadQueueStatusModel.SelectMany(top => top.Messages).Select(top => top.StatusMessage).ToList();
                    sbDriverViewModel.StatusCode = Status.Failed;
                    foreach (var item in statusMessage)
                    {
                        sbDriverViewModel.StatusMessage = item;
                    }
                }
                var responseModelJson = JsonConvert.SerializeObject(sbDriverViewModel);
                LogManager.Logger.WriteAPIInfo(CurrentUser.UserName, "ScheduleBuilderController", "PublishDriverView", requestModelJson, responseModelJson, 0, "Azure-AppService", startTime, DateTime.Now);
            }
            catch (Exception ex)
            {
                sbDriverViewModel.StatusCode = Status.Failed;
                sbDriverViewModel.StatusMessage = Resource.valMessageErrorOccurred;
                LogManager.Logger.WriteException("ScheduleBuilderController", "PublishSheduleBuilder-DriverView", ex.Message, ex);
            }
            return Json(sbDriverViewModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> PublishTrailerView()
        {
            using (var tracer = new Tracer("ScheduleBuilderController", "PublishSheduleBuilder-TrailerView"))
            {
                var sbTrailerViewModel = new DSBSaveModel();
                try
                {
                    var startTime = DateTime.Now;
                    sbTrailerViewModel = await GetViewModel<DSBSaveModel>(Request.InputStream);
                    sbTrailerViewModel.CompanyId = UserContext.CompanyId;
                    sbTrailerViewModel.UserId = UserContext.Id;

                    var requestModelJson = JsonConvert.SerializeObject(sbTrailerViewModel);
                    UpdateBadgeNumberToDeliveryRequest(sbTrailerViewModel);
                    var domain = new ScheduleBuilderDomain();
                    await domain.UpdateBulkPlantAddressForCarribean(sbTrailerViewModel);
                    var apiResponse = await domain.PublishScheduleBuilder(sbTrailerViewModel, UserContext);
                    if (apiResponse != null)
                    {
                        if (UserContext.CompanyTypeId == CompanyType.Carrier || UserContext.CompanyTypeId == CompanyType.BuyerSupplierAndCarrier || UserContext.CompanyTypeId == CompanyType.SupplierAndCarrier)
                        {
                            apiResponse.Trips.FindAll(t => t.TripId == null).ForEach(t => t.Carrier = UserContext.CompanyName);
                        }
                        sbTrailerViewModel.StatusCode = apiResponse.StatusCode;
                        sbTrailerViewModel.StatusMessage = apiResponse.StatusMessage;
                    }
                    else
                    {
                        sbTrailerViewModel.StatusCode = Status.Failed;
                        sbTrailerViewModel.StatusMessage = Resource.valMessageServiceNotResponded;
                    }
                    var responseModelJson = JsonConvert.SerializeObject(sbTrailerViewModel);
                    LogManager.Logger.WriteAPIInfo(CurrentUser.UserName, "ScheduleBuilderController", "PublishTrailerView", requestModelJson, responseModelJson, 0, "Azure-AppService", startTime, DateTime.Now);
                }
                catch (Exception ex)
                {
                    sbTrailerViewModel.StatusCode = Status.Failed;
                    sbTrailerViewModel.StatusMessage = Resource.valMessageErrorOccurred;
                    LogManager.Logger.WriteException("ScheduleBuilderController", "PublishTrailerView-TrailerView", ex.Message, ex);
                }
                return Json(sbTrailerViewModel, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> DeleteLoadDriverView()
        {
            var driverViewModel = new DSBSaveModel();
            try
            {
                var startTime = DateTime.Now;
                driverViewModel = await GetViewModel<DSBSaveModel>(Request.InputStream);
                driverViewModel.UserId = UserContext.Id;
                driverViewModel.CompanyId = UserContext.CompanyId;
                driverViewModel.Trips.ForEach(t => t.UpdatedByName = CurrentUser.Name);
                var requestModelJson = JsonConvert.SerializeObject(driverViewModel);

                var apiResponse = await ContextFactory.Current.GetDomain<ScheduleBuilderDomain>().DeleteDeliveryGroup(driverViewModel, UserContext);
                driverViewModel = apiResponse;
                if (apiResponse.StatusCode != Status.Failed)
                {
                    driverViewModel.Trips.ForEach(t =>
                    {
                        t.DeliveryRequests.ForEach(t1 =>
                        {
                            t1.GetDeliveryReqClassName(t.DeliveryRequests);
                        });
                        if (t.DeliveryRequests.Any() && (t.DeliveryRequests.All(t1 => t1.StatusClassId == 4)))
                        {
                            t.IsEditable = false;
                        }
                        else
                        {
                            t.IsEditable = true;
                        }
                    });
                }
                driverViewModel.Status = DSBMethod.None;
                var responseModelJson = JsonConvert.SerializeObject(driverViewModel);
                LogManager.Logger.WriteAPIInfo(CurrentUser.UserName, "ScheduleBuilderController", "DeleteLoadDriverView", requestModelJson, responseModelJson, 0, "Azure-AppService", startTime, DateTime.Now);
            }
            catch (Exception ex)
            {
                driverViewModel.StatusCode = Status.Failed;
                driverViewModel.StatusMessage = Resource.valMessageErrorOccurred;
                LogManager.Logger.WriteException("ScheduleBuilderController", "DeleteDeliveryGroup-DriverView", ex.Message, ex);
            }
            return Json(driverViewModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> DeleteLoadTrailerView()
        {
            using (var tracer = new Tracer("ScheduleBuilderController", "DeleteDeliveryGroup-TrailerView"))
            {
                var trailerViewModel = new DSBSaveModel();
                try
                {
                    var startTime = DateTime.Now;
                    trailerViewModel = await GetViewModel<DSBSaveModel>(Request.InputStream);
                    trailerViewModel.UserId = UserContext.Id;
                    trailerViewModel.CompanyId = UserContext.CompanyId;

                    var requestModelJson = JsonConvert.SerializeObject(trailerViewModel);
                    var apiResponse = await ContextFactory.Current.GetDomain<ScheduleBuilderDomain>().DeleteDeliveryGroup(trailerViewModel, UserContext);
                    if (UserContext.CompanyTypeId == CompanyType.Carrier || UserContext.CompanyTypeId == CompanyType.BuyerSupplierAndCarrier || UserContext.CompanyTypeId == CompanyType.SupplierAndCarrier)
                    {
                        apiResponse.Trips.FindAll(t => t.TripId == null).ForEach(t => t.Carrier = UserContext.CompanyName);
                    }
                    trailerViewModel.StatusCode = apiResponse.StatusCode;
                    trailerViewModel.StatusMessage = apiResponse.StatusMessage;

                    var responseModelJson = JsonConvert.SerializeObject(trailerViewModel);
                    LogManager.Logger.WriteAPIInfo(CurrentUser.UserName, "ScheduleBuilderController", "DeleteLoadTrailerView", requestModelJson, responseModelJson, 0, "Azure-AppService", startTime, DateTime.Now);
                }
                catch (Exception ex)
                {
                    trailerViewModel.StatusCode = Status.Failed;
                    trailerViewModel.StatusMessage = Resource.valMessageErrorOccurred;
                    LogManager.Logger.WriteException("ScheduleBuilderController", "DeleteDeliveryGroup-TrailerView", ex.Message, ex);
                }
                return Json(trailerViewModel, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> AssignDriverAndTrailer()
        {
            var response = new DSBSaveModel();
            try
            {
                var startTime = DateTime.Now;
                var scheduleBuilder = await GetViewModel<DSBSaveModel>(Request.InputStream);
                var requestModelJson = JsonConvert.SerializeObject(scheduleBuilder);
                scheduleBuilder.Trips.ForEach(t => t.UpdatedByName = CurrentUser.Name);
                response = await ContextFactory.Current.GetDomain<ScheduleBuilderDomain>().AssignDriverAndTrailer(scheduleBuilder, UserContext);
                var responseModelJson = JsonConvert.SerializeObject(response);
                response.Status = DSBMethod.None;
                LogManager.Logger.WriteAPIInfo(CurrentUser.UserName, "ScheduleBuilderController", "AssignDriverAndTrailer", requestModelJson, responseModelJson, 0, "Azure-AppService", startTime, DateTime.Now);
            }
            catch (Exception ex)
            {
                response.StatusCode = Status.Failed;
                response.StatusMessage = Resource.valMessageErrorOccurred;
                LogManager.Logger.WriteException("ScheduleBuilderController", "AssignDriverAndTrailer", ex.Message, ex);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> GetCompanyDrivers(List<string> trailerId, string regionId, string selectedDate)
        {
            using (var tracer = new Tracer("ScheduleBuilderController", "GetCompanyDrivers"))
            {
                var response = await ContextFactory.Current.GetDomain<ScheduleBuilderDomain>().GetCompanyDrivers(UserContext.CompanyId, trailerId, regionId, selectedDate);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<StatusViewModel> NotificationToDriver(List<int> driverIds, string message)
        {
            var driverNotificationViewModel = new DriverNotificationViewModel();
            var messageViewModel = new MessageViewModel();
            messageViewModel.Body = message;
            messageViewModel.Title = Resource.lblDriverNotificationTitle;
            messageViewModel.NotificationCode = (int)NotificationCode.Start;

            driverNotificationViewModel.Message = messageViewModel;
            driverNotificationViewModel.DriverIds = driverIds;

            var response = await ContextFactory.Current.GetDomain<PushNotificationDomain>().NotificationToDriver(driverNotificationViewModel);
            return response;

        }

        /// <summary>
        /// get driver details  based on driverid
        /// </summary>
        /// <param name="driverId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> GetDriverDetails(int driverId)
        {
            var response = await ContextFactory.Current.GetDomain<AuthenticationDomain>().GetUserByIdAsync(driverId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// get driver details  based on driverid
        /// </summary>
        /// <param name="driverId"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetSendBirdAPPId()
        {
            var response = ContextFactory.Current.GetDomain<MasterDomain>().GetAppSettings(ApplicationConstants.SendbirdAppId.ToString());
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> GetScheduleStatus(List<int> trackableScheduleIds)
        {
            using (var tracer = new Tracer("ScheduleBuilderController", "GetScheduleStatus"))
            {
                var response = await ContextFactory.Current.GetDomain<ScheduleBuilderDomain>().GetScheduleStatus(trackableScheduleIds);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetCompanyUsers()
        {
            List<CompanyUserDetails> response = new List<CompanyUserDetails>();
            using (var tracer = new Tracer("ScheduleBuilderController", "GetCompanyUsers"))
            {
                try
                {
                    response = await ContextFactory.Current.GetDomain<AuthenticationDomain>().GetCompanyUsers(CurrentUser.CompanyId);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("ScheduleBuilderController", "GetCompanyUsers", ex.Message, ex);
                }
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> SendPushNotificationTODriver(string message, int driverId)
        {
            using (var tracer = new Tracer("ScheduleBuilderController", "SendPushNotificationTODriver"))
            {
                var response = await ContextFactory.Current.GetDomain<ScheduleBuilderDomain>().SendPushNotificationDriver(message, driverId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// GetRegionDispactherDetails
        /// </summary>
        /// <param name="driverId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> GetRegionDispactherDetails(string regionId, int driverId)
        {
            List<SendBirdCompanyUserViewModel> companyUserDetails = new List<SendBirdCompanyUserViewModel>();
            using (var tracer = new Tracer("ScheduleBuilderController", "GetRegionDispactherDetails"))
            {
                var response = await ContextFactory.Current.GetDomain<ScheduleBuilderDomain>().GetRegionDispactherDetails(driverId, CurrentUser.CompanyId, regionId);
                if (response.Any())
                {
                    foreach (var item in response)
                    {
                        var comresponse = await ContextFactory.Current.GetDomain<AuthenticationDomain>().GetUserDetails(item.dispactherIds, item.RegionID, item.RegionName, item.RegionDescription);
                        companyUserDetails.AddRange(comresponse);
                    }

                }
                return Json(companyUserDetails, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> GetOrdersForJobOfCustomerAndSupplier(int jobId, int customerId, string regionId, bool skipMarineConversion, int endSupplier = 0, string productsToExclude = "")
        {
            using (var tracer = new Tracer("ScheduleBuilderController", "GetOrdersForJobOfCustomerAndSupplier"))
            {
                var response = await ContextFactory.Current.GetDomain<ScheduleBuilderDomain>().GetOrdersForJobOfCustomerAndSupplier(UserContext, customerId, jobId, regionId, skipMarineConversion, endSupplier, productsToExclude);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetDeliveryReqDemands(GroupDeliveryRequests input)
        {
            using (var tracer = new Tracer("ScheduleBuilderController", "GetDeliveryReqDemands"))
            {
                var status = await ContextFactory.Current.GetDomain<ScheduleBuilderDomain>().GetDeliveryReqDemands(input, UserContext);
                return Json(new { Status = status, Data = input.DeliveryReqs }, JsonRequestBehavior.AllowGet);
            }
        }

        private async Task<T> GetViewModel<T>(Stream inputStream)
        {
            inputStream.Seek(0, SeekOrigin.Begin);
            T viewModel = default(T);
            using (var stream = new StreamReader(inputStream))
            {
                string json = await stream.ReadToEndAsync();
                try
                {
                    var settings = new JsonSerializerSettings
                    {
                        Formatting = Formatting.Indented,
                        DefaultValueHandling = DefaultValueHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    viewModel = JsonConvert.DeserializeObject<T>(json, settings);
                }
                catch
                {
                    viewModel = default(T);
                    throw;
                }
            }
            return viewModel;
        }

        public async Task<JsonResult> GetSelectedDateDriverScheduleByDriverId(int driverId, string selectedDate)
        {
            using (var tracer = new Tracer("ScheduleBuilderController", "GetSelectedDateDriverScheduleByDriverId"))
            {
                var response = await ContextFactory.Current.GetDomain<ScheduleBuilderDomain>().GetSelectedDateDriverScheduleByDriverId(driverId, selectedDate);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> ValidateTrailerJobCompatibility(List<TrailerModel> trailers, List<DeliveryRequestViewModel> deliveryRequests)
        {

            using (var tracer = new Tracer("ScheduleBuilderController", "ValidateTrailerJobCompatibility"))
            {

                List<DeliveryRequestViewModel> deliveryRequestsNotCompatible = new List<DeliveryRequestViewModel>();
                if (trailers != null && trailers.Any())
                {
                    deliveryRequestsNotCompatible = await ContextFactory.Current.GetDomain<ScheduleBuilderDomain>().ValidateTrailerJobCompatibility(trailers, deliveryRequests);
                }
                var trackableSchedules = deliveryRequests.Where(t => t.TrackableScheduleId != null && t.TrackableScheduleId > 0 && t.ScheduleStatus != 0).Select(t => t.TrackableScheduleId.Value).ToList();
                List<DeliveryReqStatusUpdateModel> trackableScheduleStatuses = await ContextFactory.Current.GetDomain<ScheduleBuilderDomain>().GetScheduleStatus(trackableSchedules);

                var response = new { deliveryRequestsNotCompatible, trackableScheduleStatuses };
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public async Task<JsonResult> ValidateTrailerJobCompatibilityForLoadQueue(List<TrailersDeliveryRequestViewModel> models)
        {

            using (var tracer = new Tracer("ScheduleBuilderController", "ValidateTrailerJobCompatibilityForLoadQueue"))
            {
                List<TrailerJobNonCompatibleDrs> deliveryRequestsNotCompatible = new List<TrailerJobNonCompatibleDrs>();

                deliveryRequestsNotCompatible = await ContextFactory.Current.GetDomain<ScheduleBuilderDomain>().ValidateTrailerJobCompatibilityForLoadQueue(models);

                var response = new { deliveryRequestsNotCompatible };
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// get driver details  based on driverid
        /// </summary>
        /// <param name="driverId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetDriversDetails(List<int> driverIds)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            var response = await ContextFactory.Current.GetDomain<AuthenticationDomain>().GetUserDetailsByIds(driverIds);
            if (response.Any())
            {
                responseMessage.StatusCode = HttpStatusCode.Found;
            }
            else
            {
                responseMessage.StatusCode = HttpStatusCode.NotFound;
            }
            responseMessage.Data = response;
            return Json(responseMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> CloneDrsForPreload(List<string> drIds)
        {
            using (var tracer = new Tracer("ScheduleBuilderController", "CloneDrsForPreload"))
            {
                var freightServiceDomain = ContextFactory.Current.GetDomain<FreightServiceDomain>();
                var response = await freightServiceDomain.CloneDrsForPreload(drIds, UserContext);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> CreatePreloadForAcrossTheDate(PreLoadDrViewModel viewModel)
        {
            using (var tracer = new Tracer("ScheduleBuilderController", "CreatePreloadForAcrossDate"))
            {
                viewModel.UserId = UserContext.Id;
                viewModel.CompanyId = UserContext.CompanyId;
                var scheduleBuilderDomain = ContextFactory.Current.GetDomain<ScheduleBuilderDomain>();
                var response = await scheduleBuilderDomain.CreatePreloadForAcrossTheDate(viewModel, UserContext);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        private static void UpdateBadgeNumberToDeliveryRequest(DSBSaveModel scheduleBuilder)
        {
            if (scheduleBuilder != null)
            {
                foreach (var tripItem in scheduleBuilder.Trips)
                {
                    if (tripItem.DeliveryRequests.Any(top => top.IsCommonBadge))
                    {
                        foreach (var deliveryReqitem in tripItem.DeliveryRequests)
                        {
                            if (deliveryReqitem.IsCommonBadge)
                            {
                                deliveryReqitem.BadgeNo1 = tripItem.BadgeNo1;
                                deliveryReqitem.BadgeNo2 = tripItem.BadgeNo2;
                                deliveryReqitem.BadgeNo3 = tripItem.BadgeNo3;
                                // deliveryReqitem.DispactherNote = tripItem.RouteInfo;
                                deliveryReqitem.IsCommonBadge = true;
                            }
                            else
                            {
                                tripItem.IsCommonBadge = false;
                            }
                        }
                    }
                    else
                    {
                        tripItem.RouteInfo = string.IsNullOrWhiteSpace(tripItem.RouteInfo) ? string.Empty : tripItem.RouteInfo;
                    }
                }
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetRecurringScheduleDetails(int jobId, string PoNumber, string JobSiteId, int productTypeId)
        {
            using (var tracer = new Tracer("ScheduleBuilderController", "GetRecurringScheduleDetails"))
            {
                var response = await ContextFactory.Current.GetDomain<ScheduleBuilderDomain>().GetRecurringScheduleDetails(jobId, PoNumber, JobSiteId, productTypeId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> UnAssignDriverFromShift(UnassignDriverViewModel removeDriver)
        {
            using (var tracer = new Tracer("ScheduleBuilderController", "UnAssignDriver"))
            {
                var response = new UnassignDriverViewModel();
                try
                {
                    var startTime = DateTime.Now;
                    var requestModelJson = JsonConvert.SerializeObject(removeDriver);
                    removeDriver.Updatedby = UserContext.Id;
                    removeDriver.updatedByName = UserContext.Name;
                    response = await ContextFactory.Current.GetDomain<ScheduleBuilderDomain>().UnAssignDriverFromShift(removeDriver);
                    var responseModelJson = JsonConvert.SerializeObject(response);
                    LogManager.Logger.WriteAPIInfo(CurrentUser.UserName, "ScheduleBuilderController", "UnAssignDriverFromShift", requestModelJson, responseModelJson, 0, "Azure-AppService", startTime, DateTime.Now);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("ScheduleBuilderController", "UnAssignDriverFromShift", ex.Message, ex);
                }
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<JsonResult> GetAssignCarrierDetails(string regionId, int jobId, int fuelTypeId)
        {
            AssignDrToCarrierModel assignCarrierDetails = new AssignDrToCarrierModel();
            using (var tracer = new Tracer("ScheduleBuilderController", $"GetAssignCarrierDetails(regionId:{regionId})"))
            {
                var regionCarrier = await ContextFactory.Current.GetDomain<ScheduleBuilderDomain>().GetRegionCarriers(regionId);
                assignCarrierDetails.CarrierDetails = regionCarrier;
                var orderDetails = await ContextFactory.Current.GetDomain<ScheduleBuilderDomain>().GetOrderDetails(jobId, fuelTypeId, null, UserContext, -1, assignCarrierDetails.CarrierDetails);
                orderDetails.ForEach(x => assignCarrierDetails.OrderDetails.Add(new DispatchOrderDetailsDropdown { Id = x.OrderId, Name = x.PoNumber, Code = x.IsDispatchRetainedByCustomer == true ? "1" : "0", DRNote = x.DRNote }));
                return Json(assignCarrierDetails, JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<JsonResult> GetSelectedCarriersByRegion(string regionId)
        {
            var carriers = new List<TfxCarrierDropdownDisplayItem>();
            using (var tracer = new Tracer("ScheduleBuilderController", $"GetCarriersByRegion(regionId:{regionId})"))
            {
                carriers = await ContextFactory.Current.GetDomain<ScheduleBuilderDomain>().GetRegionCarriers(regionId);
                return Json(carriers, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public async Task<JsonResult> GetOrdersByDeliveryRequests(List<DeliveryRequestViewModel> deliveryRequests)
        {
            var response = new List<OrderPartialDetailModel>();

            var orders = new List<OrderPickupDetailModel>();
            var finalResult = new List<DeliveryRequestViewModel>();
            var normalDeliveryRequests = deliveryRequests.Where(x => !x.IsBlendedRequest).ToList();
            normalDeliveryRequests.ForEach(x => finalResult.Add(x));
            var blendedDeliveryRequests = deliveryRequests.Where(x => x.IsBlendedRequest && x.IsBlendedDrParent && !x.IsAdditive).ToList();
            blendedDeliveryRequests.ForEach(x => finalResult.Add(x));
            foreach (var dr in finalResult)
            {
                var tempResult = await ContextFactory.Current.GetDomain<ScheduleBuilderDomain>().GetOrderDetails(dr.JobId, dr.ProductTypeId, null, UserContext);
                if (tempResult != null && tempResult.Any())
                {
                    tempResult.ForEach(i => { i.JobId = dr.JobId; i.ProductTypeId = dr.ProductTypeId; });
                    if (dr.IsBlendedRequest)
                    {
                        orders.AddRange(tempResult.Where(x => x.OrderId == dr.OrderId).ToList());
                    }
                    else
                    {
                        orders.AddRange(tempResult);
                    }
                }
            }

            orders.ForEach(x => response.Add(new OrderPartialDetailModel { Id = x.OrderId, Name = x.PoNumber, Code = x.IsDispatchRetainedByCustomer == true ? "1" : "0", JobId = x.JobId, FuelTypeId = x.ProductTypeId }));
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> GetSupplierChildOrders(int OrderId)
        {
            using (var tracer = new Tracer("ScheduleBuilderController", $"GetSupplierChildOrders(OrderId:{OrderId})"))
            {
                var getChildOrderInfo = await ContextFactory.Current.GetDomain<ScheduleBuilderDomain>().GetSupplierChildOrders(OrderId);
                return Json(getChildOrderInfo, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> BrokerDeliveryRequestToCarrier(DeliveryRequestBrokerInfoViewModel viewModel)
        {
            viewModel.CarrierInfo = JsonConvert.DeserializeObject<List<TfxCarrierDropdownDisplayItem>>(viewModel.CarrierInfoJson);
            viewModel.CarrierInfoJson = string.Empty;
            var startTime = DateTime.Now;
            var requestModelJson = JsonConvert.SerializeObject(viewModel);
            using (var tracer = new Tracer("ScheduleBuilderController", $"BrokerDeliveryRequestToCarrier"))
            {
                var response = await ContextFactory.Current.GetDomain<CarrierDomain>().BrokerDeliveryRequestToCarrier(UserContext, viewModel);
                var responseModelJson = JsonConvert.SerializeObject(response);
                LogManager.Logger.WriteAPIInfo(CurrentUser.UserName, "ScheduleBuilderController", "BrokerDeliveryRequestToCarrier", requestModelJson, responseModelJson, 0, "Azure-AppService", startTime, DateTime.Now);
                return Json(response, JsonRequestBehavior.DenyGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> BrokerDeliveryRequestsToCarriers(DeliveryRequestBrokerInfoViewModel viewModel)
        {
            viewModel.CarrierInfo = JsonConvert.DeserializeObject<List<TfxCarrierDropdownDisplayItem>>(viewModel.CarrierInfoJson);
            viewModel.CarrierInfoJson = string.Empty;
            var startTime = DateTime.Now;
            var requestModelJson = JsonConvert.SerializeObject(viewModel);
            using (var tracer = new Tracer("ScheduleBuilderController", $"BrokerDeliveryRequestsToCarriers"))
            {
                var response = await ContextFactory.Current.GetDomain<CarrierDomain>().BrokerDeliveryRequestsToCarriers(UserContext, viewModel);
                var responseModelJson = JsonConvert.SerializeObject(response);
                LogManager.Logger.WriteAPIInfo(CurrentUser.UserName, "ScheduleBuilderController", "BrokerDeliveryRequestsToCarriers", requestModelJson, responseModelJson, 0, "Azure-AppService", startTime, DateTime.Now);
                return Json(response, JsonRequestBehavior.DenyGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> RecallDrFromCarrier(DeliveryRequestViewModel viewModel)
        {
            var startTime = DateTime.Now;
            var requestModelJson = JsonConvert.SerializeObject(viewModel);
            using (var tracer = new Tracer("ScheduleBuilderController", $"RecallDrFromCarrier"))
            {
                var response = await ContextFactory.Current.GetDomain<CarrierDomain>().RecallDeliveryRequest(UserContext, viewModel);
                var responseModelJson = JsonConvert.SerializeObject(response);
                LogManager.Logger.WriteAPIInfo(CurrentUser.UserName, "ScheduleBuilderController", "BrokerDeliveryRequestToCarrier", requestModelJson, responseModelJson, 0, "Azure-AppService", startTime, DateTime.Now);
                return Json(response, JsonRequestBehavior.DenyGet);
            }
        }
        [HttpGet]
        public async Task<JsonResult> GetTrailerFuelRetainDetails(string trailerId)
        {
            using (var tracer = new Tracer("ScheduleBuilderController", $"GetTrailerFuelRetainDetails(OrderId:{trailerId})"))
            {
                var trailerFuelRetains = await ContextFactory.Current.GetDomain<ScheduleBuilderDomain>().GetTrailerFuelRetainDetails(trailerId);
                return Json(trailerFuelRetains, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public async Task<JsonResult> SaveTrailerFuelRetain(TrailerFuelRetainViewModel trailerFuelRetainViewModel)
        {
            using (var tracer = new Tracer("ScheduleBuilderController", "SaveTrailerFuelRetain"))
            {
                var trailerFuelRetainsViewModel = new List<TrailerFuelRetainViewModel> { trailerFuelRetainViewModel };
                var response = await ContextFactory.Current.GetDomain<ScheduleBuilderDomain>().SaveTrailerFuelRetain(trailerFuelRetainsViewModel);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetShifts(string regionId)
        {
            var fsDomain = ContextFactory.Current.GetDomain<FreightServiceDomain>();
            var response = await fsDomain.GetShifts(CurrentUser.CompanyId, regionId);
            response.ForEach(x => x.ShiftInfo = "Shift - " + x.StartTime + " - " + x.EndTime);
            foreach (var item in response)
            {
                TimeSpan tspan;
                tspan = DateTime.ParseExact(item.StartTime, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay;
                item.StartTimespan = tspan.Ticks;
            }
            return Json(response.OrderByDescending(top => top.Name), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public async Task<JsonResult> GetOttoNotifications(string regionId)
        {
            var fsDomain = ContextFactory.Current.GetDomain<FreightServiceDomain>();
            var response = await fsDomain.GetDsbNotification(regionId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public async Task<JsonResult> GetDsbNotificationCount(string regionId)
        {
            var statusMessage = new StatusViewModel();
            var fsDomain = ContextFactory.Current.GetDomain<FreightServiceDomain>();
            var response = await fsDomain.GetDsbNotificationCount(regionId);
            if (response > 0)
            {
                statusMessage.StatusCode = Status.Success;
                statusMessage.OttoNotificationCount = response;
            }
            else
            {
                statusMessage.StatusCode = Status.Failed;
                statusMessage.OttoNotificationCount = response;
            }
            return Json(statusMessage, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<JsonResult> UpdateDsbNotificationStatus(string Id)
        {
            var fsDomain = ContextFactory.Current.GetDomain<FreightServiceDomain>();
            var response = await fsDomain.UpdateDsbNotificationStatus(Id);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> ScheduleOttoDRs(OttoBuilder ottoBuilder)
        {
            var sbDomain = ContextFactory.Current.GetDomain<ScheduleBuilderDomain>();
            var response = await sbDomain.ScheduleOttoDRs(UserContext, ottoBuilder);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeliveryRequestsReport()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> SaveDsbLoadQueue()
        {
            List<DsbLoadQueueViewModel> loadQueueModel = new List<DsbLoadQueueViewModel>();
            var sbDriverViewModel = new List<DSBSaveModel>();
            string language = Request.UserLanguages == null ? "en-us" : Request.UserLanguages[0];
            sbDriverViewModel = await GetViewModel<List<DSBSaveModel>>(Request.InputStream);
            sbDriverViewModel.ForEach(x => { x.CompanyId = UserContext.CompanyId; x.UserId = UserContext.Id; x.Trips.ForEach(t => t.UpdatedByName = CurrentUser.Name); });
            foreach (var item in sbDriverViewModel)
            {
                item.Trips.ForEach(t => t.UpdatedByName = CurrentUser.Name);
                item.Trips.ForEach(t => t.IsDsbLoadQueueBackgroundProcess = true);
            }
            LoadQueueUpdateBadgeNumberToDeliveryRequest(sbDriverViewModel);
            if (sbDriverViewModel.Any())
            {
                IntializeDsbSaveModel(loadQueueModel, sbDriverViewModel, language);
            }
            LoadQueueStatusViewModel loadQueueStatusViewModel = new LoadQueueStatusViewModel();
            //validate the dsb load queue model.
            var validateLoadQueue = await ContextFactory.Current.GetDomain<DsbLoadQueueDomain>().ValidateLoadQueue(loadQueueModel, UserContext);
            if (validateLoadQueue.Any())
            {
                loadQueueStatusViewModel.LoadQueueErrorInfo = validateLoadQueue;
                RemoveLoadQueueErrorInQueueModel(loadQueueModel, loadQueueStatusViewModel);
            }
            //save the dsb load queue model.
            var loadQueueSaveResponse = await ContextFactory.Current.GetDomain<DsbLoadQueueDomain>().SaveDsbLoadQueue(loadQueueModel, UserContext);
            IntializeLoadQueueSuccess(loadQueueStatusViewModel, loadQueueSaveResponse);
            return Json(loadQueueStatusViewModel, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<JsonResult> GetShiftCompanyDrivers(string regionId, string otherRegion, string selectedDate, string shiftId)
        {
            using (var tracer = new Tracer("ScheduleBuilderController", "GetGridViewCompanyDrivers"))
            {
                var response = await ContextFactory.Current.GetDomain<ScheduleBuilderDomain>().GetCompanyDrivers(UserContext.CompanyId, regionId, otherRegion, selectedDate, shiftId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetDriversShiftsURL(string regionId, string SelectedDate)
        {
            var fsDomain = ContextFactory.Current.GetDomain<FreightServiceDomain>();
            var response = await fsDomain.GetDriversShifts(CurrentUser.CompanyId, regionId, SelectedDate);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> getSelectedDateDriverScheduleByDriverIdGridView(int driverId, string selectedDate, string shiftId)
        {
            using (var tracer = new Tracer("ScheduleBuilderController", "getSelectedDateDriverScheduleByDriverIdGridView"))
            {
                var response = await ContextFactory.Current.GetDomain<ScheduleBuilderDomain>().GetSelectedDateDriverScheduleByDriverId(driverId, selectedDate, shiftId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> Create(OnTheFlyLocationModel model)
        {
            using (var tracer = new Tracer("ScheduleBuilderController", "Create(viewModel)"))
            {


                //if (ModelState.IsValid)
                //{
                var response = await ContextFactory.Current.GetDomain<ThirdPartyOrderDomain>().CreateOnTheFlyLocation(UserContext, model);

                //if (thirdPartyOrderViewModel.FuelDetails.IsTierPricing && thirdPartyOrderViewModel.FuelDetails.TierPricing != null && thirdPartyOrderViewModel.FuelDetails.TierPricing.Pricings.Any())
                //{
                //    thirdPartyOrderViewModel.FuelDetails.TierPricing = thirdPartyOrderViewModel.FuelDetails.FuelPricing.TierPricing;
                //}                        
                //return Json(thirdPartyOrderViewModel);
                return Json(response);
                // }
                //return Json(model);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetTerminalsForMultipleProducts(int jobCountryId, int pricingCodeId, int fuelType, int companyCountryId, bool isSupressOrderPricing, decimal jobLatitude, decimal jobLongitude, string searchStringTeminal)
        {
            var response = await ContextFactory.Current.GetDomain<ExternalPricingDomain>().GetTerminalsForMultipleProducts(jobCountryId, pricingCodeId, new List<int>() { fuelType }, companyCountryId, isSupressOrderPricing, jobLatitude, jobLongitude, searchStringTeminal);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetPreferenceSettingForOnTheFlyLocation()
        {
            var response = await ContextFactory.Current.GetDomain<ScheduleBuilderDomain>().GetPreferenceSettingForOnTheFlyLocation(UserContext);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    
        private void IntializeDsbSaveModel(List<DsbLoadQueueViewModel> loadQueueModel, List<DSBSaveModel> sbDriverViewModel, string userLanguage)
        {
            foreach (var item in sbDriverViewModel)
            {
                var tripInfo = item.Trips.FirstOrDefault();
                if (tripInfo != null)
                {
                    DsbLoadQueueViewModel loadQueueViewModel = new DsbLoadQueueViewModel();
                    IntializeLoadScheduleDetails(item, tripInfo, loadQueueViewModel);
                    IntializeLoadTrailerDriverDetails(tripInfo, loadQueueViewModel);
                    IntializeLoadColumnDetails(userLanguage, item, tripInfo, loadQueueViewModel);
                    loadQueueModel.Add(loadQueueViewModel);
                }
            }
        }
        private void LoadQueueUpdateBadgeNumberToDeliveryRequest(List<DSBSaveModel> scheduleBuilder)
        {
            if (scheduleBuilder != null)
            {
                foreach (var item in scheduleBuilder)
                {
                    foreach (var tripItem in item.Trips)
                    {
                        if (tripItem.DeliveryRequests.Any(top => top.IsCommonBadge))
                        {
                            foreach (var deliveryReqitem in tripItem.DeliveryRequests)
                            {
                                if (deliveryReqitem.IsCommonBadge)
                                {
                                    deliveryReqitem.BadgeNo1 = tripItem.BadgeNo1;
                                    deliveryReqitem.BadgeNo2 = tripItem.BadgeNo2;
                                    deliveryReqitem.BadgeNo3 = tripItem.BadgeNo3;
                                    deliveryReqitem.DispactherNote = tripItem.RouteInfo;
                                    deliveryReqitem.IsCommonBadge = true;
                                }
                                else
                                {
                                    tripItem.IsCommonBadge = false;
                                }
                            }
                        }
                        else
                        {
                            tripItem.RouteInfo = string.Empty;
                        }
                    }
                }

            }
        }
        private void RemoveLoadQueueErrorInQueueModel(List<DsbLoadQueueViewModel> loadQueueModel, LoadQueueStatusViewModel loadQueueStatusViewModel)
        {
            foreach (var loadQueueErrorInfo in loadQueueStatusViewModel.LoadQueueErrorInfo)
            {
                var loadQueueErrorIndex = loadQueueModel.FindIndex(top => top.ShiftIndex == loadQueueErrorInfo.ShiftIndex && top.DriverColIndex == loadQueueErrorInfo.DriverColIndex && top.TfxUserId == loadQueueErrorInfo.TfxUserId && top.TfxCompanyId == loadQueueErrorInfo.TfxCompanyId);
                if (loadQueueErrorIndex >= 0)
                {
                    loadQueueModel.RemoveAt(loadQueueErrorIndex);
                }
            }
        }
        private void IntializeLoadQueueSuccess(LoadQueueStatusViewModel loadQueueStatusViewModel, LoadQueueStatusViewModel loadQueueSaveResponse)
        {
            if (loadQueueSaveResponse != null && loadQueueSaveResponse.StatusCode == (int)Status.Success)
            {
                loadQueueStatusViewModel.LoadQueueSuccessInfo = loadQueueSaveResponse.LoadQueueSuccessInfo;
                loadQueueStatusViewModel.LoadQueueErrorInfo.AddRange(loadQueueSaveResponse.LoadQueueErrorInfo);
                loadQueueStatusViewModel.StatusCode = loadQueueSaveResponse.StatusCode;
            }
        }

        private void IntializeLoadScheduleDetails(DSBSaveModel item, TripViewModel tripInfo, DsbLoadQueueViewModel loadQueueViewModel)
        {
            loadQueueViewModel.ScheduleBuilderId = item.Id;
            loadQueueViewModel.Date = item.Date;
            loadQueueViewModel.RegionId = item.RegionId;
            loadQueueViewModel.ShiftId = tripInfo.ShiftId;
            loadQueueViewModel.ShiftIndex = tripInfo.ShiftIndex.GetValueOrDefault();
            loadQueueViewModel.DriverColIndex = tripInfo.DriverRowIndex.GetValueOrDefault();
        }
        private void IntializeLoadColumnDetails(string userLanguage, DSBSaveModel item, TripViewModel tripInfo, DsbLoadQueueViewModel loadQueueViewModel)
        {
            var deliveryRequestDetails = tripInfo.DeliveryRequests.Select(top => top.Id).ToList();
            loadQueueViewModel.DeliveryRequestInfo = JsonConvert.SerializeObject(deliveryRequestDetails);
            loadQueueViewModel.DeliveryRequestDetails = deliveryRequestDetails;
            loadQueueViewModel.DriverColJsonInfo = JsonConvert.SerializeObject(item);
            loadQueueViewModel.TfxUserId = UserContext.Id;
            loadQueueViewModel.TfxUserName = UserContext.Name;
            loadQueueViewModel.TfxCompanyId = UserContext.CompanyId;
            loadQueueViewModel.CreatedBy = UserContext.Id;
            loadQueueViewModel.UserLanguage = userLanguage;
        }
        private void IntializeLoadTrailerDriverDetails(TripViewModel tripInfo, DsbLoadQueueViewModel loadQueueViewModel)
        {
            List<TrailerInfo> trailerDetails = new List<TrailerInfo>();
            foreach (var traileritem in tripInfo.Trailers)
            {
                trailerDetails.Add(new TrailerInfo { Id = traileritem.Id, TrailerId = traileritem.TrailerId });
            }
            loadQueueViewModel.TrailerInfo = JsonConvert.SerializeObject(trailerDetails);
            loadQueueViewModel.TrailerDetails = trailerDetails;
            if (tripInfo.Drivers != null && tripInfo.Drivers.Any())
            {
                loadQueueViewModel.TfxDriverId = tripInfo.Drivers.FirstOrDefault().Id;
                loadQueueViewModel.TfxDriverName = tripInfo.Drivers.FirstOrDefault().Name;
            }
        }
        private static void IntializeLoadQueueColumnStatus(SbDriverViewModel response, List<DataAccess.Entities.DsbLoadQueueDetails> dsbScheduleLoadQueueDetails)
        {
            if (dsbScheduleLoadQueueDetails.Any())
            {
                foreach (var shiftitem in response.Shifts)
                {
                    foreach (var scitem in shiftitem.Schedules)
                    {
                        var tripInfo = scitem.Trips.Select(top => top.DriverRowIndex).FirstOrDefault();
                        if (tripInfo != null)
                        {
                            var dsbColumnExists = dsbScheduleLoadQueueDetails.Where(top => top.ShiftId == shiftitem.Id && top.DriverColIndex == tripInfo.GetValueOrDefault()).OrderByDescending(top => top.Id).FirstOrDefault();
                            if (dsbColumnExists != null)
                            {
                                if (dsbColumnExists.ProcessStatus == (int)DsbLoadQueueStatus.New || dsbColumnExists.ProcessStatus == (int)DsbLoadQueueStatus.InProgress)
                                {
                                    scitem.IsLoadQueueColumnBlocked = true;

                                }
                                scitem.LoadQueueColumnStatus = dsbColumnExists.ProcessStatus;
                            }

                        }
                    }
                }
            }
        }
        private void IntializeDsbSaveModel(List<DsbLoadQueueViewModel> loadQueueModel, DSBSaveModel sbDriverViewModel, string userLanguage)
        {
            var tripInfo = sbDriverViewModel.Trips.FirstOrDefault();
            if (tripInfo != null)
            {
                DsbLoadQueueViewModel loadQueueViewModel = new DsbLoadQueueViewModel();
                IntializeLoadScheduleDetails(sbDriverViewModel, tripInfo, loadQueueViewModel);
                IntializeLoadTrailerDriverDetails(tripInfo, loadQueueViewModel);
                IntializeLoadColumnDetails(userLanguage, sbDriverViewModel, tripInfo, loadQueueViewModel);
                loadQueueModel.Add(loadQueueViewModel);
            }
        }
        [HttpGet]
        public async Task<JsonResult> GetDefaultTBDScheduleData()
        {
            var response = new DefaultTBDScheduleData();
            using (var tracer = new Tracer("ScheduleBuilderController", "GetDefaultTBDScheduleData"))
            {
                var FsDomain = ContextFactory.Current.GetDomain<FreightServiceDomain>();
                response.UoM = await FsDomain.getUoMByCompany(CurrentUser.CompanyId);
                response.MstProductTypes = await FsDomain.GetMstProducts();
                response.OtherProducts = await ContextFactory.Current.GetDomain<OrderDomain>().GetOtherProductsOfSupplier(CurrentUser.CompanyId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public async Task<JsonResult> GetTBDTerminals()
        {
            using (var tracer = new Tracer("BaseController", "GetTBDTerminals"))
            {
                var response = await ContextFactory.Current.GetDomain<ExternalPricingDomain>().GetTBDTerminalsForOrders();
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public async Task<JsonResult> CancelDriverSchedule()
        {
            List<DsbLoadQueueViewModel> dsbLoadQueueViewModel = new List<DsbLoadQueueViewModel>();
            List<DsbLoadQueueStatusViewModel> dsbLoadQueueStatusModel = new List<DsbLoadQueueStatusViewModel>();
            var sbDriverViewModel = new DSBSaveModel();
            try
            {
                string language = Request.UserLanguages == null ? "en-us" : Request.UserLanguages[0];
                var startTime = DateTime.Now;
                sbDriverViewModel = await GetViewModel<DSBSaveModel>(Request.InputStream);
                sbDriverViewModel.CompanyId = UserContext.CompanyId;
                sbDriverViewModel.UserId = UserContext.Id;
                sbDriverViewModel.Trips.ForEach(t => t.UpdatedByName = CurrentUser.Name);
                sbDriverViewModel.Trips.ForEach(t => t.IsDsbLoadQueueBackgroundProcess = false);
                var requestModelJson = JsonConvert.SerializeObject(sbDriverViewModel);
                if (sbDriverViewModel != null)
                {
                    IntializeDsbSaveModel(dsbLoadQueueViewModel, sbDriverViewModel, language);
                    //validate the dsb load queue model.
                    //dsbLoadQueueStatusModel = await ContextFactory.Current.GetDomain<DsbLoadQueueDomain>().ValidateLoadQueue(dsbLoadQueueViewModel, UserContext);
                }
                if (!dsbLoadQueueStatusModel.Any())
                {
                    var apiResponse = await ContextFactory.Current.GetDomain<ScheduleBuilderDomain>().CancelDriverScheduleBuilder(sbDriverViewModel, UserContext, language);
                    if (apiResponse != null)
                    {
                        sbDriverViewModel = apiResponse;
                        sbDriverViewModel.Status = DSBMethod.None;
                        var parentDRGroupId = new List<string>();
                        sbDriverViewModel.Trips.ForEach(t =>
                        {
                            if (t.DeliveryRequests.Any())
                            {
                                t.DeliveryRequests.ForEach(dr => { parentDRGroupId.Add(dr.GroupParentDRId); });
                            }
                        });
                        if (parentDRGroupId.Any())
                            parentDRGroupId = parentDRGroupId.Where(x => !string.IsNullOrEmpty(x)).Distinct().ToList();
                        var domain = ContextFactory.Current.GetDomain<ConsolidatedInvoiceDomain>();
                        parentDRGroupId.ForEach(t =>
                        {
                            domain.AddQueueMessageForDrCompletion(UserContext, t);
                        });
                    }
                    else
                    {
                        sbDriverViewModel.StatusCode = Status.Failed;
                        sbDriverViewModel.StatusMessage = Resource.valMessageServiceNotResponded;
                    }
                }
                else
                {
                    var statusMessage = dsbLoadQueueStatusModel.SelectMany(top => top.Messages).Select(top => top.StatusMessage).ToList();
                    sbDriverViewModel.StatusCode = Status.Failed;
                    foreach (var item in statusMessage)
                    {
                        sbDriverViewModel.StatusMessage = item;
                    }
                }
                var responseModelJson = JsonConvert.SerializeObject(sbDriverViewModel);
                LogManager.Logger.WriteAPIInfo(CurrentUser.UserName, "ScheduleBuilderController", "CancelDriverSchedule", requestModelJson, responseModelJson, 0, "Azure-AppService", startTime, DateTime.Now);
            }
            catch (Exception ex)
            {
                sbDriverViewModel.StatusCode = Status.Failed;
                sbDriverViewModel.StatusMessage = Resource.valMessageErrorOccurred;
                LogManager.Logger.WriteException("ScheduleBuilderController", "PublishSheduleBuilder-DriverView", ex.Message, ex);
            }
            return Json(sbDriverViewModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> CancelDeliverySchedule(List<CancelDeliverySchedule> cancelDeliverySchedules)
        {
            List<CancelDeliverySchedule> cancelDSCollection = new List<CancelDeliverySchedule>();
            if (cancelDeliverySchedules != null && cancelDeliverySchedules.Any())
            {
                cancelDSCollection = await ContextFactory.Current.GetDomain<ScheduleBuilderDomain>().CancelDeliverySchedule(UserContext, cancelDeliverySchedules);
            }
            return Json(cancelDSCollection, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<JsonResult> GetSubDRInfoCancelDS(CancelDSDeliveryScheduleInfo cancelDeliverySchedules)
        {
            List<CancelDSDeliveryScheduleViewModel> cancelDSCollection = new List<CancelDSDeliveryScheduleViewModel>();
            if (cancelDeliverySchedules != null && cancelDeliverySchedules.CancelDSDeliverySchedules.Any())
            {
                cancelDeliverySchedules.TfxCompanyId = UserContext.CompanyId;
                cancelDSCollection = await ContextFactory.Current.GetDomain<ScheduleBuilderDomain>().GetSubDRInfoCancelDS(cancelDeliverySchedules);
            }
            return Json(cancelDSCollection, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> getSubDrsStatus(List<string> groupDRParentIds)
        {
            var response = await ContextFactory.Current.GetDomain<ScheduleBuilderDomain>().getSubDrsStatusByParentId(groupDRParentIds);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetCreateDrSetting()
        {
            var response = await ContextFactory.Current.GetDomain<ScheduleBuilderDomain>().GetCreateDrSetting(UserContext.CompanyId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetAdditiveOrders(string regionId = null)
        {
            var response = await ContextFactory.Current.GetDomain<OrderDomain>().GetAdditiveOrders(CurrentUser.CompanyId, regionId);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<JsonResult> RemoveDeliverySchedule(ResetDeliveryGroupScheduleModel model)
        {
            model.CompanyId = UserContext.CompanyId;
            model.UserId = UserContext.Id;
            model.UpdatedByName = UserContext.UserName;
            var response = await ContextFactory.Current.GetDomain<CarrierDomain>().ResetDriverDeliveryRequest(model, UserContext);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

    }

}