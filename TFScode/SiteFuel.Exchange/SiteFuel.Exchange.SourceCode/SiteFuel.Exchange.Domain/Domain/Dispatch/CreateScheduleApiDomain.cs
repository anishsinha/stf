using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.MobileAPI.TPD;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace SiteFuel.Exchange.Domain
{
    public class CreateScheduleApiDomain : ScheduleBuilderDomain
    {
        public CreateScheduleApiDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public CreateScheduleApiDomain(BaseDomain domain) : base(domain)
        {
        }

        public async Task CreateSchedule(ProcessDSBCreation inputModel, ApiResponseViewModel apiResponse)
        {
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    CreateDeliveryGroupModel additionalDetails = new CreateDeliveryGroupModel();
                    additionalDetails.Terminals = inputModel.Terminals;
                    additionalDetails.OrderIds = inputModel.ScheduleDetails.Select(t => t.OrderId).Distinct().ToList();
                    inputModel.ScheduleDetails.GroupBy(t => t.OrderId).Select(t => t.FirstOrDefault()).ToList().ForEach(t => additionalDetails.Orders.Add(new OrderDetailModel() { TimeZoneName = t.TimeZoneName, Currency = t.Currency, Id = t.OrderId }));
                    var deliveryRequests = CreateDeliveryRequest(inputModel);
                    var scheduleBuilder = await GetScheduleBuilderInputModel(inputModel, deliveryRequests);
                    if (scheduleBuilder.StatusCode == Status.Failed)
                    {
                        if (scheduleBuilder.StatusMessage == Resource.errMsgProcessRequestFailed)
                        {
                            apiResponse.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeTF01, Message = Resource.errMsgProcessRequestFailed });
                        }
                        else
                        {
                            apiResponse.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeRS02, Message = scheduleBuilder.StatusMessage });
                        }
                        return;
                    }
                    UserContext userContext = new UserContext() { Id = inputModel.UserId, Name = inputModel.UserName, CompanyId = inputModel.CarrierCompanyId, CompanyName = inputModel.CarrierCompanyName };
                    await PublishLoads(scheduleBuilder, userContext, additionalDetails, inputModel);
                    await Context.CommitAsync();
                    scheduleBuilder = await ApiPostCall<DSBSaveModel>(ApplicationConstants.UrlCreateSchedules, scheduleBuilder);
                    if (scheduleBuilder.StatusCode == Status.Success)
                    {
                        transaction.Commit();
                        var trackableScheduleIds = additionalDetails.GroupChanges.Where(t1 => t1.TrackableScheduleId > 0 && (t1.ScheduleStatus == (int)DeliveryScheduleStatus.New
                                                                                                || t1.ScheduleStatus == (int)DeliveryScheduleStatus.Accepted
                                                                                                || t1.ScheduleStatus == (int)DeliveryScheduleStatus.Modified)).Select(t1 => t1.TrackableScheduleId).ToList();
                        if (trackableScheduleIds.Any())
                        {
                            await new StoredProcedureDomain(this).InsertBrokerSchedules(trackableScheduleIds);
                        }
                        foreach (var apiSchedules in inputModel.ApiResponseModel.GroupBy(t => t.RequestCarrierOrderId))
                        {
                            var apiSchedule = apiSchedules.FirstOrDefault();
                            if (apiSchedule != null)
                            {
                                if (apiSchedule.RequestCarrierOrderId == apiSchedule.ResponseCarrierOrderId)
                                    apiResponse.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeRS03, Message = string.Format(Resource.successMsgScheduleCreateSuccess, apiSchedule.RequestCarrierOrderId), EntityId = apiSchedule.TrackableScheduleId.ToString() });
                                else
                                    apiResponse.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeRS03, Message = string.Format(Resource.successMsgScheduleMergeSuccess, apiSchedule.RequestCarrierOrderId, apiSchedule.ResponseCarrierOrderId), EntityId = apiSchedule.TrackableScheduleId.ToString() });
                            }
                            apiResponse.Status = Status.Success;
                        }
                    }
                    else
                    {
                        transaction.Rollback();
                        apiResponse.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeTF01, Message = Resource.errMsgProcessRequestFailed });
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("ScheduleApiDomain", "CreateSchedule", ex.Message, ex);
                    apiResponse.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeTF01, Message = Resource.errMsgProcessRequestFailed });
                }
            }
            return;
        }


        public List<DeliveryRequestViewModel> CreateDeliveryRequest(ProcessDSBCreation inputModel)
        {
            var deliveryRequestsModel = new List<DeliveryRequestViewModel>();
            List<int> jobIds = inputModel.ScheduleDetails.Select(t => t.JobId).Distinct().ToList();
            List<string> tankIds = inputModel.ScheduleDetails.Where(t => t.TankId != null && t.TankId.Trim() != "").Select(t => t.TankId).Distinct().ToList();
            List<string> storageIds = inputModel.ScheduleDetails.Where(t => t.StorageId != null && t.StorageId.Trim() != "").Select(t => t.StorageId).Distinct().ToList();
            var tanks = Context.DataContext.AssetAdditionalDetails.Where(t => tankIds.Any() && tankIds.Contains(t.VehicleId) && storageIds.Contains(t.Vendor) && t.Asset.JobXAssets.Any(t1 => t1.RemovedBy == null && jobIds.Contains(t1.JobId))).
                    Select(t => new { t.VehicleId, t.Vendor, t.Asset.FuelType, t.Asset.MstProductType.Name, jobAssets = t.Asset.JobXAssets.Where(t1 => t1.RemovedBy == null).ToList() }).ToList();
            foreach (var schedule in inputModel.ScheduleDetails)
            {
                
                var existingDr = deliveryRequestsModel.FirstOrDefault(t => t.OrderId == schedule.OrderId && t.PickupLocationType == schedule.PickupLocationType
                                                                                            && ((t.PickupLocationType == PickupLocationType.BulkPlant && t.BulkPlant.Equals(schedule.BulkPlant))
                                                                                            || (t.PickupLocationType == PickupLocationType.Terminal && t.Terminal.Id == schedule.Terminal.Id)));
                if (existingDr != null)
                {
                    existingDr.RequiredQuantity += schedule.RequiredQuantity;
                    inputModel.ApiResponseModel.Add(new ScheduleApiResponse() { RequestCarrierOrderId = schedule.CarrierOrderID, ResponseCarrierOrderId = existingDr.CarrierOrderId });
                }
                else
                {
                    var request = new DeliveryRequestViewModel
                    {
                        ExternalRefId = inputModel.ExternalRefID,
                        BadgeNo1 = schedule.BadgeNumber,
                        DispactherNote = schedule.DispatcherNote,
                        CarrierOrderId = schedule.CarrierOrderID,
                        JobId = schedule.JobId,
                        OrderId = schedule.OrderId,
                        TankId = schedule.TankId,
                        StorageId = schedule.StorageId,
                        Priority = DeliveryReqPriority.MustGo,
                        RequiredQuantity = schedule.RequiredQuantity,
                        SiteId = schedule.SiteId,
                        AssignedToRegionId = inputModel.CarrierRegionId,
                        CreatedByRegionId = inputModel.SupplierRegionId,
                        AssignedToCompanyId = inputModel.CarrierCompanyId,
                        CreatedByCompanyId = schedule.SupplierCompanyId,
                        Status = 2,
                        SupplierCompanyId = schedule.SupplierCompanyId,
                        UoM = (int)schedule.UoM,
                        PickupLocationType = schedule.PickupLocationType,
                        BulkPlant = schedule.BulkPlant,
                        Terminal = schedule.Terminal,
                        ProductType = schedule.ProductTypeName,
                        ProductTypeId = schedule.ProductTypeId,
                        CustomerCompany = schedule.CustomerCompanyName,
                        JobName = schedule.JobName,
                        JobAddress = schedule.JobAddress,
                        JobCity = schedule.JobCity
                    };
                    deliveryRequestsModel.Add(request);
                }
            }
            return deliveryRequestsModel;
        }

        public async Task<DSBSaveModel> GetScheduleBuilderInputModel(ProcessDSBCreation inputModel, List<DeliveryRequestViewModel> deliveryRequests)
        {
            object scheduleData = new { CreateScheduleInput = inputModel, DeliveryRequests = deliveryRequests };
            var response = await ApiPostCall<DSBSaveModel>(ApplicationConstants.UrlGetCreateScheduleInput, scheduleData);
            inputModel.ApiResponseModel.AddRange(response.ApiResponseModel);
            return response;
        }

        //public void CreateLoads(ProcessDSBCreation inputModel, DSBSaveModel scheduleBuilder, DeliveryRequestsViewModel deliveryRequests)
        //{
        //    scheduleBuilder.UserId = inputModel.UserId;
        //    var newLoad = scheduleBuilder.Trips.FirstOrDefault(t => !t.DeliveryRequests.Any());
        //    foreach (var drs in deliveryRequests.DeliveryRequests.GroupBy(t => t.JobId))
        //    {
        //        var load = scheduleBuilder.Trips.FirstOrDefault(t => t.DeliveryRequests.Any(t1 => t1.JobId == drs.Key));
        //        if (load != null)
        //        {
        //            if (drs != null && drs.Any())
        //            {
        //                foreach (var dr in drs)
        //                {
        //                    var existingDr = load.DeliveryRequests.FirstOrDefault(t => t.OrderId == dr.OrderId && t.PickupLocationType == dr.PickupLocationType
        //                                                                                        && ((t.PickupLocationType == PickupLocationType.BulkPlant && t.BulkPlant.Equals(dr.BulkPlant))
        //                                                                                        || (t.PickupLocationType == PickupLocationType.Terminal && t.Terminal.Id == dr.Terminal.Id)));
        //                    if (existingDr != null)
        //                    {
        //                        existingDr.RequiredQuantity += dr.RequiredQuantity;
        //                    }
        //                    else
        //                    {
        //                        load.DeliveryRequests.Add(dr);
        //                    }
        //                }
        //                load.UpdatedByName = inputModel.UserName;
        //                load.DeliveryGroupStatus = DeliveryGroupStatus.Published;
        //                load.TripStatus = TripStatus.Modified;
        //            }
        //        }
        //        else
        //        {
        //            newLoad.DeliveryRequests.AddRange(drs);
        //            newLoad.DeliveryGroupStatus = DeliveryGroupStatus.Published;
        //            newLoad.UpdatedByName = inputModel.UserName;
        //            newLoad.TripStatus = TripStatus.Added;
        //            newLoad.StartTime = inputModel.StartTime;
        //            newLoad.EndTime = inputModel.EndTime;
        //            newLoad.Drivers.Add(inputModel.Drivers);
        //            newLoad.StartDate = inputModel.Date;
        //            newLoad.Carrier = inputModel.CarrierCompanyName;
        //            newLoad.UpdatedByName = inputModel.UserName;
        //        }
        //    }
        //}

        public async Task PublishLoads(DSBSaveModel scheduleBuilder, UserContext userContext, CreateDeliveryGroupModel additionalDetails, ProcessDSBCreation apiRequest)
        {

            int maxGroupId = 0;
            if (Context.DataContext.DeliverySchedules.Any())
            {
                maxGroupId = Context.DataContext.DeliverySchedules.Max(t => t.GroupId);
            }
            var modifiedTrackableSchedules = new List<DeliveryScheduleXTrackableSchedule>();
            var trackableScheduleIds = scheduleBuilder.Trips.SelectMany(t => t.DeliveryRequests).Where(t => (t.ScheduleStatus == (int)DeliveryScheduleStatus.New || t.ScheduleStatus == (int)DeliveryScheduleStatus.Modified) && t.TrackableScheduleId > 0).Select(t => t.TrackableScheduleId).ToList();
            if (trackableScheduleIds.Any())
            {
                modifiedTrackableSchedules = await Context.DataContext.DeliveryScheduleXTrackableSchedules.Where(t => trackableScheduleIds.Contains(t.Id)).ToListAsync();
            }
            DispatchDomain dispatchDomain = new DispatchDomain(this);
            foreach (var group in scheduleBuilder.Trips.Where(t => t.TripStatus == TripStatus.Added || t.TripStatus == TripStatus.Modified))
            {
                var driver = group.Drivers.FirstOrDefault();
                await GetDeliveryGroup(userContext, driver, group, additionalDetails);
                foreach (var deliveryRequest in group.DeliveryRequests.Where(t => t.PreviousStatus != (int)DeliveryReqStatus.ScheduleCreated || t.ScheduleStatus == (int)DeliveryScheduleStatus.New || t.ScheduleStatus == (int)DeliveryScheduleStatus.Modified))
                {
                    DeliverySchedule deliverySchedule = null;
                    DeliveryScheduleXTrackableSchedule trackableSchedule = null;
                    ++maxGroupId;
                    deliverySchedule = GetDeliverySchedule(userContext, maxGroupId, group, deliveryRequest, driver, deliverySchedule);
                    trackableSchedule = await GetTrackableSchedule(driver, group, deliveryRequest, scheduleBuilder, trackableSchedule);
                    deliverySchedule.DeliveryScheduleXTrackableSchedules.Add(trackableSchedule);
                    await SetCarrierAndSupplierSource(group.Carrier, group.SupplierSource, deliverySchedule, userContext, dispatchDomain);
                    Context.DataContext.DeliverySchedules.Add(deliverySchedule);
                    await Context.CommitAsync();
                    apiRequest.ApiResponseModel.Where(t => t.RequestCarrierOrderId == deliveryRequest.CarrierOrderId || t.ResponseCarrierOrderId == deliveryRequest.CarrierOrderId).ToList().ForEach(t => t.TrackableScheduleId = trackableSchedule.Id);
                    if (deliveryRequest.DeliveryScheduleId.HasValue)
                    {
                        deliverySchedule.StatusId = trackableSchedule.DeliveryScheduleStatusId = (int)DeliveryScheduleStatus.Modified;
                    }
                    var order = additionalDetails.Orders.FirstOrDefault(t => t.Id == deliveryRequest.OrderId.Value);
                    additionalDetails.GroupChanges.Add(new ScheduleNotificationModel() { ScheduleDate = trackableSchedule.Date, StartTime = trackableSchedule.StartTime, EndTime = trackableSchedule.EndTime, OrderId = trackableSchedule.OrderId ?? 0, GroupId = group.GroupId, ScheduleId = trackableSchedule.DeliveryScheduleId, ScheduleStatus = trackableSchedule.DeliveryScheduleStatusId, TrackableScheduleId = trackableSchedule.Id, DriverId = trackableSchedule.DriverId });
                    deliveryRequest.ScheduleStatus = (int)DeliveryScheduleStatus.Modified;
                    deliveryRequest.TrackScheduleStatus = trackableSchedule.DeliveryScheduleStatusId;
                    deliveryRequest.TrackScheduleStatusName = ((TrackableDeliveryScheduleStatus)trackableSchedule.DeliveryScheduleStatusId).GetDisplayName();
                    deliveryRequest.DeliveryScheduleId = deliverySchedule.Id;
                    deliveryRequest.TrackableScheduleId = trackableSchedule.Id;
                    deliveryRequest.DeliveryGroupId = group.GroupId;
                    var pickUpLocation = GetFuelDispatchLocation(userContext, additionalDetails.Terminals, group, deliveryRequest, order, trackableSchedule);
                    deliverySchedule.FuelDispatchLocations.Add(pickUpLocation);
                    if (pickUpLocation != null)
                    {
                        await dispatchDomain.SaveBulkPlantLocation(pickUpLocation, userContext.CompanyId);
                    }
                }
                await Context.CommitAsync();
            }
            CreateOrdersNewVersion(additionalDetails, userContext);
        }
    }
}
