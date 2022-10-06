using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.FreightModels;
using SiteFuel.FreightModels.Carrier;
using SiteFuel.FreightModels.DeliveryRequest;
using SiteFuel.FreightModels.ForcastingHelpers;
using SiteFuel.FreightModels.ScheduleBuilder;
using SiteFuel.FreightRepository.Helpers;
using SiteFuel.FreightRepository.Mappers;
using SiteFuel.MdbDataAccess.Collections;
using SiteFuel.MdbDataAccess.DbContext;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using TrueFill.DemandCaptureDataAccess;
using TrueFill.DemandCaptureDataAccess.Entities;
using TrueFill.ExchangeDataAccess.DataAccess;

namespace SiteFuel.FreightRepository
{
    public class DeliveryRequestRepository : IDeliveryRequestRepository
    {
        private readonly DemandCaptureContext context = new DemandCaptureContext();
        private readonly MdbContext mdbContext;
        public DeliveryRequestRepository()
        {
            if (mdbContext == null)
            {
                mdbContext = new MdbContext();
            }
        }
        public DeliveryRequestRepository(MdbContext _context)
        {
            mdbContext = _context;
        }

        public async Task<DeliveryRequestsViewModel> CreateDeliveryRequest(List<DeliveryRequestViewModel> model)
        {
            var response = new DeliveryRequestsViewModel();
            bool isSessionStarted = false;
            using (var session = await mdbContext.Client.StartSessionAsync())
            {
                try
                {
                    var requestsWithoutRegion = model.Where(t => ((t.ScheduleQuantityType == (int)ScheduleQuantityType.Quantity) && t.RequiredQuantity > 0 && !t.IsTBD) || t.CreatedByRegionId == null || t.CreatedByRegionId == "").Select(t => t).ToList();
                    bool isValidRegion = true;
                    if (model.Any(t => t.OrderId == null))
                    {
                        isValidRegion = ValidateRegion(requestsWithoutRegion, isValidRegion);
                        if (!isValidRegion)
                        {
                            response.StatusCode = (int)Status.Failed;
                            response.StatusMessage = Resource.valMessageJobNotAssignedToRegion;
                            return response;
                        }
                    }
                    AssignRouteInformation(model);
                    var deliveryRequests = model.ToEntity();
                    if (deliveryRequests.Count > 0)
                    {
                        deliveryRequests.ForEach(t =>
                        {
                            t.CarrierStatus = string.IsNullOrWhiteSpace(t.BrokeredParentId) ?
                            (int)BrokeredDrCarrierStatus.None : (int)BrokeredDrCarrierStatus.Pending;
                            if (t.ScheduleQuantityType != (int)ScheduleQuantityType.Quantity) { t.RequiredQuantity = 0; };
                        });
                        session.StartTransaction();
                        isSessionStarted = true;
                        await mdbContext.DeliveryRequests.InsertManyAsync(deliveryRequests);

                        // To mantain parent child relationship in brokered DRs.
                        var brokeredChildDrs = deliveryRequests.Where(t => !string.IsNullOrWhiteSpace(t.BrokeredParentId)).ToList();
                        foreach (var item in brokeredChildDrs)
                        {
                            var parentId = ObjectId.Parse(item.BrokeredParentId);
                            var brokeredOrderId = model.Where(t => t.BrokeredDrId == item.BrokeredParentId).Select(t => t.BrokeredOrderId).FirstOrDefault();
                            var filter = Builders<DeliveryRequest>.Filter.Where(t => t.Id == parentId);
                            var update = Builders<DeliveryRequest>.Update
                                        .Set(t => t.BrokeredChildId, item.Id.ToString())
                                        .Set(t => t.TfxOrderId, brokeredOrderId)
                                        .Set(t => t.CarrierStatus, (int)BrokeredDrCarrierStatus.Pending);
                            await mdbContext.DeliveryRequests.UpdateOneAsync(filter, update);
                        }
                        await session.CommitTransactionAsync();
                        model.Clear();
                        deliveryRequests.ForEach(t => model.Add(t.ToDeliveryRequestViewModel()));

                        foreach (var item in deliveryRequests)
                        {
                            response.EntityIds.Add(item.Id.ToString());
                            if (!string.IsNullOrEmpty(item.BrokeredParentId))
                                response.EntityParentIds.Add(item.BrokeredParentId.ToString(), item.Id.ToString());
                            if (item.NumOfSubDrs > 0)
                            {
                                SplitDeliveryRequestModel splitDRModel = new SplitDeliveryRequestModel() { ParentDRId = item.Id.ToString(), RequiredQtyDetails = new List<RequiredQtyDetails>() };
                                var requiredQuantity = (item.RequiredQuantity / item.NumOfSubDrs.Value).GetPreciseValue(4);
                                for (int i = 0; i < item.NumOfSubDrs; i++)
                                {
                                    splitDRModel.RequiredQtyDetails.Add(new RequiredQtyDetails() { RequiredQty = requiredQuantity });
                                }
                                await CreateSplitDeliveryRequests(splitDRModel);
                            }
                        }
                        var IsCarrierCompany = deliveryRequests.Any(x => x.TfxAssignedToCompanyId != x.TfxCreatedByCompanyId && x.TfxAssignedToCompanyId != x.TfxSupplierCompanyId);
                        response.DeliveryRequests = model;
                        response.StatusCode = (int)Status.Success;
                        response.StatusMessage = IsCarrierCompany ? Resource.msgDelReqrCreationSuccessForCarrier : Resource.msgDelReqCreationSuccess;
                    }
                    else
                    {
                        response.StatusCode = (int)Status.Warning;
                        response.StatusMessage = Resource.msgDelReqCreationFailed;
                    }
                }
                catch (Exception)
                {
                    if (isSessionStarted)
                    {
                        await session.AbortTransactionAsync();
                    }
                    response.StatusCode = (int)Status.Failed;
                    throw;
                }
            }
            return response;
        }

        public async Task<List<TBDRequestDetailModel>> GetTbdDeliveryRequestDetails(List<string> deliveryRequestIds)
        {
            var response = new List<TBDRequestDetailModel>();
            var objectIds = new List<ObjectId> { };
            deliveryRequestIds.Where(t => !string.IsNullOrWhiteSpace(t)).ToList().ForEach(t => objectIds.Add(ObjectId.Parse(t)));
            var deliveryRequests = await mdbContext.DeliveryRequests.Find(t => objectIds.Contains(t.Id) && t.IsActive && !t.IsDeleted).Project(t => new { t.Id, t.TfxFuelTypeId, t.FuelType, t.IsTBD, t.GroupParentDRId, t.TfxProductTypeId, t.TfxUoM }).ToListAsync();

            foreach (var dr in deliveryRequests)
            {
                TBDRequestDetailModel request = new TBDRequestDetailModel()
                {
                    DeliveryRequestId = dr.Id.ToString(),
                    FuelTypeId = dr.TfxFuelTypeId > 0 ? dr.TfxFuelTypeId.Value : 0,
                    FuelType = dr.FuelType,
                    ProductTypeId = dr.TfxProductTypeId,
                    UoM = dr.TfxUoM,
                    IsTBD = dr.IsTBD,
                    GroupedParentDrId = dr.GroupParentDRId != null ? dr.GroupParentDRId.ToString() : null
                };

                response.Add(request);
            }
            return response;
        }

        public StatusModel AssignRegionForDelRequest(List<DeliveryRequestViewModel> model)
        {
            var status = new StatusModel() { StatusCode = (int)Status.Success };
            var companyIds = model.Select(t => t.SupplierCompanyId.Value).ToList();
            var jobIds = model.Select(t => t.JobId).ToList();
            var carrierList = (from carrier in mdbContext.Carriers.AsQueryable()
                               join job in mdbContext.CarrierJobs.AsQueryable()
                                on carrier.TfxCarrierCompanyId equals job.TfxCarrierCompanyId into grp
                               where companyIds.Contains(carrier.TfxSupplierCompanyId) && carrier.IsActive && !carrier.IsDeleted
                               select new
                               {
                                   carrier.Id,
                                   carrier.TfxCarrierCompanyId,
                                   carrier.TfxSupplierCompanyId,
                                   Jobs = grp.Where(t => t.IsActive && jobIds.Contains(t.TfxJobId)).Select(t => new
                                   {
                                       t.TfxJobId,
                                       t.TfxSupplierCompanyId
                                   })
                               }).OrderByDescending(t => t.Id).ToList();
            carrierList = carrierList.Where(t => t.Jobs != null && t.Jobs.Any()).ToList();
            var carrierCompanies = carrierList.Select(t => t.TfxCarrierCompanyId).ToList();
            companyIds = companyIds.Union(carrierCompanies).ToList();
            var filter = Builders<Region>.Filter.And(
                        Builders<Region>.Filter.Where(x => x.IsActive && !x.IsDeleted && x.TfxJobs.Any(x1 => jobIds.Contains(x1.Id))),
                        Builders<Region>.Filter.In(x => x.TfxCompanyId, companyIds));

            var regions = mdbContext.Regions.Find(filter).Project(t => new { t.Id, t.TfxJobs, t.TfxCompanyId }).SortByDescending(t => t.Id).ToList();
            if (regions != null && regions.Any())
            {
                foreach (var dr in model)
                {
                    var carrier = carrierList.FirstOrDefault(t => t.TfxSupplierCompanyId == dr.SupplierCompanyId && t.Jobs.Any(t1 => t1.TfxJobId == dr.JobId && t1.TfxSupplierCompanyId == dr.SupplierCompanyId));
                    if (carrier != null)
                    {
                        var carrierRegion = regions.FirstOrDefault(t => t.TfxCompanyId == carrier.TfxCarrierCompanyId && t.TfxJobs.Any(t1 => t1.Id == dr.JobId));
                        if (carrierRegion != null)
                        {
                            dr.AssignedToCompanyId = carrier.TfxCarrierCompanyId;
                            dr.AssignedToRegionId = carrierRegion.Id.ToString();
                            dr.CreatedByRegionId = carrierRegion.Id.ToString();
                            dr.StatusCode = (int)Status.Success;
                        }
                        else
                        {
                            status.StatusMessage = Resource.errorMessageLocationNotAssignedToRegionByCarrier;
                            status.StatusCode = (int)Status.Failed;
                        }
                    }
                    else
                    {
                        var supplierRegion = regions.FirstOrDefault(t => t.TfxCompanyId == dr.SupplierCompanyId && t.TfxJobs.Any(t1 => t1.Id == dr.JobId));
                        if (supplierRegion != null)
                        {
                            dr.AssignedToRegionId = supplierRegion.Id.ToString();
                            dr.CreatedByRegionId = supplierRegion.Id.ToString();
                        }
                        else
                        {
                            status.StatusMessage = Resource.valMessageJobNotAssignedToRegion;
                            status.StatusCode = (int)Status.Failed;
                        }
                    }
                }
            }
            else
            {
                status.StatusMessage = Resource.valMessageJobNotAssignedToRegion;
                status.StatusCode = (int)Status.Failed;
            }
            return status;
        }

        public async Task<StatusModel> ReCreateDeliveryRequestAsync(ReCreateDeliveryRequestsViewModel model)
        {
            var response = new StatusModel();
            using (var session = await mdbContext.Client.StartSessionAsync())
            {
                bool isSessionStarted = false;
                try
                {
                    if (model.ExistingDrIds != null && model.ExistingDrIds.Count() > 0)
                    {
                        session.StartTransaction();
                        var objectIds = model.ExistingDrIds.Select(t => ObjectId.Parse(t)).ToList();
                        var filter = Builders<DeliveryRequest>.Filter.And(
                                        Builders<DeliveryRequest>.Filter.In(t => t.Id, objectIds),
                                        Builders<DeliveryRequest>.Filter.Where(t => t.IsActive && !t.IsDeleted),
                                        Builders<DeliveryRequest>.Filter.Where(t => t.Status == DeliveryReqStatus.None || t.Status == DeliveryReqStatus.Pending || t.Status == DeliveryReqStatus.Assigned)
                                    );
                        if (mdbContext.DeliveryRequests.Find(filter).CountDocuments() == objectIds.Count)
                        {
                            objectIds = model.ExistingDrIds.Where(t => !model.DeliveryRequests.Any(t1 => t1.Id == t)).Select(t => ObjectId.Parse(t)).ToList();
                            filter = Builders<DeliveryRequest>.Filter.And(
                                        Builders<DeliveryRequest>.Filter.In(t => t.Id, objectIds),
                                        Builders<DeliveryRequest>.Filter.Where(t => t.IsActive && !t.IsDeleted),
                                        Builders<DeliveryRequest>.Filter.Where(t => t.Status == DeliveryReqStatus.None || t.Status == DeliveryReqStatus.Pending || t.Status == DeliveryReqStatus.Assigned)
                                    );
                            var updateFields = Builders<DeliveryRequest>.Update
                                                .Set(t => t.IsActive, false)
                                                .Set(t => t.IsReCreated, true)
                                                .Set(t => t.IsDeleted, true);
                            await mdbContext.DeliveryRequests.UpdateManyAsync(filter, updateFields);
                        }
                        else
                        {
                            response.StatusMessage = Resource.valMessageDelReqInUse;
                            session.AbortTransaction();
                            return response;
                        }
                        foreach (var item in model.DeliveryRequests)
                        {
                            filter = Builders<DeliveryRequest>.Filter.And(
                                        Builders<DeliveryRequest>.Filter.Eq(t => t.Id, ObjectId.Parse(item.Id)),
                                        Builders<DeliveryRequest>.Filter.Where(t => t.IsActive && !t.IsDeleted),
                                        Builders<DeliveryRequest>.Filter.Where(t => t.Status == DeliveryReqStatus.None || t.Status == DeliveryReqStatus.Pending || t.Status == DeliveryReqStatus.Assigned));

                            var update = Builders<DeliveryRequest>.Update.Set(t => t.ScheduleQuantityType, item.ScheduleQuantityType)
                                                                         .Set(t => t.RequiredQuantity, item.RequiredQuantity)
                                                                         .Set(t => t.Priority, item.Priority);
                            await mdbContext.DeliveryRequests.UpdateOneAsync(filter, update);
                        }
                        session.CommitTransaction();
                    }
                    response.StatusCode = (int)Status.Success;
                }
                catch (Exception)
                {
                    if (isSessionStarted)
                    {
                        await session.AbortTransactionAsync();
                    }
                    response.StatusCode = (int)Status.Failed;
                    throw;
                }
            }
            return response;
        }

        private bool ValidateRegion(List<DeliveryRequestViewModel> requestsWithoutRegion, bool isValidRegion)
        {
            if (requestsWithoutRegion.Any())
            {
                var tfxJobIds = requestsWithoutRegion.Select(t => t.JobId).ToList();
                var companyIds = new List<int>();
                companyIds.AddRange(requestsWithoutRegion.Select(t => t.AssignedToCompanyId));
                companyIds.AddRange(requestsWithoutRegion.Select(t => t.CreatedByCompanyId));
                companyIds.AddRange(requestsWithoutRegion.Where(t => t.SupplierCompanyId.HasValue).Select(t => t.SupplierCompanyId.Value));

                var filter = Builders<Region>.Filter.And(
                        Builders<Region>.Filter.Where(x => x.IsActive && !x.IsDeleted && x.TfxJobs.Any(x1 => tfxJobIds.Contains(x1.Id))),
                        Builders<Region>.Filter.In(x => x.TfxCompanyId, companyIds)
                    );

                var regions = mdbContext.Regions.Find(filter).Project(t => new { t.Id, t.TfxJobs, t.TfxCompanyId }).SortByDescending(t => t.Id).ToList();

                foreach (var request in requestsWithoutRegion)
                {
                    var region = regions.FirstOrDefault(t => (t.TfxCompanyId == request.AssignedToCompanyId) && t.TfxJobs.Any(t1 => t1.Id == request.JobId));
                    if (region != null)
                    {
                        request.CreatedByRegionId = region.Id.ToString();
                        request.AssignedToRegionId = region.Id.ToString();
                    }
                    else
                    {
                        isValidRegion = false;
                        break;
                    }
                }
            }

            return isValidRegion;
        }
        public async Task<List<DeliveryRequestViewModel>> GetDeliveryRequests(int companyId, string regionId, string selectedDate = null)
        {
            var response = new List<DeliveryRequestViewModel>();
            if (!string.IsNullOrWhiteSpace(regionId) && !string.IsNullOrEmpty(regionId))
            {
                DateTime dateFilter = DateTimeOffset.Now.Date;
                if (!string.IsNullOrWhiteSpace(selectedDate))
                {
                    dateFilter = Convert.ToDateTime(selectedDate).Date;
                }

                List<DeliveryRequest> deliveryRequestsDetails = await CreateDeliveryRequestsFilters(companyId, regionId);

                var deliveryRequests = (from dr in deliveryRequestsDetails
                                        where (dr.DelReqSource != DRSource.OttoForecasting || (dr.DelReqSource == DRSource.OttoForecasting && dr.DeliveryWindowInfo != null && dr.DeliveryWindowInfo.StartDate == dateFilter))
                && (dr.DelReqSource != DRSource.Forecasting || (dr.DelReqSource == DRSource.Forecasting && dr.DeliveryWindowInfo != null && dr.DeliveryWindowInfo.StartDate <= dateFilter))
                                        select dr
                                        ).OrderBy(x => x.CurrentThreshold).ToList();

                var drModels = deliveryRequests.Select(t => t.ToDeliveryRequestViewModel()).ToList();
                response.GetBlendDRInfo(drModels, true);

                foreach (var dr in response)
                {
                    if (dr.ScheduleQuantityType != 0)
                    {
                        dr.ScheduleQuantityTypeText = GetSchduleQtyTypeText(dr.ScheduleQuantityType);
                    }
                }

                GetRecurringScheduleInformation(response);
                if (response.Any())
                {
                    GetJobCompatibilityInfo(response.Select(t => t.JobId).Distinct().ToList(), response, selectedDate);
                }

            }
            return response;
        }

        private async Task<List<DeliveryRequest>> CreateDeliveryRequestsFilters(int companyId, string regionId)
        {
            var deliveryRequestFilter = Builders<DeliveryRequest>.Filter.And(
            Builders<DeliveryRequest>.Filter.Eq(u => u.TfxAssignedToCompanyId, companyId),
            Builders<DeliveryRequest>.Filter.Eq(u => u.TfxAssignedToRegionId, regionId),
            Builders<DeliveryRequest>.Filter.Eq(u => u.IsActive, true),
            Builders<DeliveryRequest>.Filter.Ne(u => u.Status, DeliveryReqStatus.ScheduleCreated),
             Builders<DeliveryRequest>.Filter.Ne(u => u.Status, DeliveryReqStatus.Draft)
            );

            var deliveryRequestTypeFilter = Builders<DeliveryRequest>.Filter.And(
            Builders<DeliveryRequest>.Filter.Eq(u => u.DeliveryRequestType, 0) |
            Builders<DeliveryRequest>.Filter.Eq(u => u.DeliveryRequestType, 1));

            var deliveryRequestBrokeredFilter = Builders<DeliveryRequest>.Filter.And(
          Builders<DeliveryRequest>.Filter.Eq(u => u.CarrierStatus, (int)BrokeredDrCarrierStatus.None) |
          Builders<DeliveryRequest>.Filter.Eq(u => u.CarrierStatus, (int)BrokeredDrCarrierStatus.Recalled)
          );

            var deliveryRequestBrokeredFilter1 = Builders<DeliveryRequest>.Filter.And(
          Builders<DeliveryRequest>.Filter.Ne(u => u.BrokeredParentId, null) &
          Builders<DeliveryRequest>.Filter.Eq(u => u.CarrierStatus, (int)BrokeredDrCarrierStatus.Accepted)
          );

            var groupChildDRsFilter = Builders<DeliveryRequest>.Filter.And(
           Builders<DeliveryRequest>.Filter.Size(u => u.GroupChildDRs, 0));

            var deliveryRequestsDetails = await mdbContext.DeliveryRequests.Find(deliveryRequestFilter & (deliveryRequestBrokeredFilter | deliveryRequestBrokeredFilter1) & deliveryRequestTypeFilter & groupChildDRsFilter).ToListAsync();
            return deliveryRequestsDetails;
        }

        public List<DeliveryRequestViewModel> GetCalendarDeliveryRequests(int companyId, CalendarFilterModel inputModel)
        {
            var response = new List<DeliveryRequestViewModel>();
            if (companyId > 0)
            {
                DateTime dateFilter = DateTimeOffset.Now.Date;
                var priorityList = inputModel.Priorities.Select(t => (DeliveryReqPriority)t).ToList();

                var deliveryRequests = (from dr in mdbContext.DeliveryRequests.AsQueryable()
                                        where (dr.TfxAssignedToCompanyId == companyId)
                                                && dr.Status != DeliveryReqStatus.ScheduleCreated
                                                && dr.Status != DeliveryReqStatus.Draft && dr.IsActive
                                                && dr.IsFutureDR && dr.IsCalendarView //IsCalendarDR
                                                && dr.SelectedDate != null
                                                && dr.SelectedDate >= inputModel.FromDate.Date
                                                && dr.SelectedDate <= inputModel.ToDate.Date
                                                && (!inputModel.Customers.Any() || inputModel.Customers.Contains(dr.TfxCustomerCompany))
                                                && (!inputModel.Locations.Any() || inputModel.Locations.Contains(dr.TfxJobId))
                                                && (!inputModel.Priorities.Any() || priorityList.Contains(dr.Priority))
                                                && (!inputModel.LocationType || !inputModel.Vessels.Any() || inputModel.Vessels.Contains(dr.Vessel))
                                                &&
                                                (
                                                    (dr.CarrierStatus == (int)BrokeredDrCarrierStatus.None || dr.CarrierStatus == (int)BrokeredDrCarrierStatus.Recalled) // Supplier
                                                    ||
                                                    (dr.BrokeredParentId != null && dr.CarrierStatus == (int)BrokeredDrCarrierStatus.Accepted) // Carrier
                                                )
                                                && (dr.DeliveryRequestType == 0 || dr.DeliveryRequestType == 1)
                                                && (dr.DelReqSource != DRSource.OttoForecasting || (dr.DelReqSource == DRSource.OttoForecasting && dr.DeliveryWindowInfo != null && dr.DeliveryWindowInfo.StartDate == dateFilter))
                                                && dr.GroupChildDRs.Count() == 0
                                        select dr
                                        ).OrderBy(x => x.CurrentThreshold).ToList();

                var drModels = deliveryRequests.Select(t => t.ToDeliveryRequestViewModel()).ToList();
                response.GetBlendDRInfo(drModels, true);

                foreach (var dr in response)
                {
                    if (dr.ScheduleQuantityType != 0)
                    {
                        dr.ScheduleQuantityTypeText = GetSchduleQtyTypeText(dr.ScheduleQuantityType);
                    }
                }
            }
            return response;
        }

        public async Task<List<DeliveryRequestViewModel>> GetBrokeredDrRequestedToMe(int companyId, string regionId, string selectedDate = null)
        {
            var response = new List<DeliveryRequestViewModel>();
            if (!string.IsNullOrWhiteSpace(regionId))
            {
                ObjectId objRegionId = ObjectId.Empty;
                ObjectId.TryParse(regionId, out objRegionId);

                var deliveryRequests = await mdbContext.DeliveryRequests.Find(dr => dr.TfxAssignedToCompanyId == companyId &&
                                                                                   dr.TfxCreatedByCompanyId != companyId &&
                                                                                   dr.CarrierStatus == (int)BrokeredDrCarrierStatus.Pending &&
                                                                                   dr.CreatedRegionId != objRegionId &&
                                                                                   dr.TfxAssignedToRegionId == regionId &&
                                                                                   (dr.DeliveryRequestType == 0 || dr.DeliveryRequestType == 1) &&
                                                                                   dr.IsActive && !dr.IsDeleted
                                                                                   ).ToListAsync();

                if (deliveryRequests.Any())
                {
                    var odeliveryRequests = deliveryRequests.OrderBy(x => x.CurrentThreshold);
                    var drModels = odeliveryRequests.Select(t => t.ToDeliveryRequestViewModel()).ToList();
                    response.GetBlendDRInfo(drModels, true);
                    if (response.Any())
                    {
                        GetJobCompatibilityInfo(response.Select(t => t.JobId).Distinct().ToList(), response, selectedDate);
                        await AddCurrentInventoryDetails(response);
                    }
                }
            }
            return response;
        }

        private async Task GetDrCarrierInfo(List<DeliveryRequestViewModel> deliveryRequests)
        {
            deliveryRequests = deliveryRequests.Where(x => !string.IsNullOrEmpty(x.BrokeredDrId)).ToList();
            var drIds = deliveryRequests.Select(t => ObjectId.Parse(t.BrokeredDrId)).ToList();
            var carrierSequence = await mdbContext.DRCarrierSequences.Find(t => drIds.Contains(t.DeliveryRequestId) && t.IsActive && !t.IsDeleted).ToListAsync();
            foreach (var dr in deliveryRequests)
            {
                var drSequence = carrierSequence.FirstOrDefault(t => t.DeliveryRequestId.ToString() == dr.BrokeredDrId);
                if (drSequence != null)
                {
                    dr.CarrierRejected = drSequence.CarrierRejectInfo.OrderByDescending(t => t.RejectDate.Add(t.RejectTime)).Select(t => t.Name).FirstOrDefault();
                    var rejectedCarrierIds = drSequence.CarrierRejectInfo.Select(t => t.Id).ToList();
                    var carriers = drSequence.CarrierInfo.Where(t => !rejectedCarrierIds.Contains(t.Id)).OrderBy(t => t.SequenceNo).Select(t => new { t.Name, t.SequenceNo }).ToList();

                    dr.CurrentCarrier = carriers.Any() ? carriers[0].Name : String.Empty;
                    dr.UpcomingCarrier = carriers.Count > 1 ? carriers[1].Name : String.Empty;
                }
            }
        }
        public async Task AddCurrentInventoryDetails(List<DeliveryRequestViewModel> deliveryRequestViewModels)
        {
            if (deliveryRequestViewModels.Any())
            {
                var siteIds = new List<string>();
                var tankIdList = new List<string>();
                var storageList = new List<string>();

                var jobWithProductTypeIds = deliveryRequestViewModels.Select(t => new { t.JobId, t.ProductTypeId }).ToList();

                var jobIds = jobWithProductTypeIds.Select(t => t.JobId).Distinct().ToList();
                var jobwithTanks = await mdbContext.JobAdditionalDetails
                                         .Find(t => jobIds.Contains(t.TfxJobId))
                                         .Project(t => new { Tanks = t.Tanks.Select(t2 => new { t2.TfxProductTypeId, t2.StorageId, t2.StorageTypeId }).ToList(), t.TfxJobId, t.TfxDisplayJobId })
                                         .ToListAsync();
                if (jobwithTanks.Any())
                {
                    jobWithProductTypeIds.ForEach(t =>
                    {
                        jobwithTanks.Where(t1 => t1.TfxJobId == t.JobId).SelectMany(t2 => t2.Tanks).Where(t3 => t3.TfxProductTypeId == t.ProductTypeId)
                        .Select(t1 => new { t1.StorageId, t1.StorageTypeId }).ToList().ForEach(m => { storageList.Add(m.StorageId); tankIdList.Add(m.StorageTypeId); });
                    });
                    siteIds = jobwithTanks.Where(x => !string.IsNullOrEmpty(x.TfxDisplayJobId)).Select(y => y.TfxDisplayJobId).Distinct().ToList();
                    tankIdList = tankIdList.Distinct().ToList();
                    storageList = storageList.Distinct().ToList();

                    var respData = await GetTanksInventory(siteIds, tankIdList, storageList);
                    if (respData.SalesData != null && respData.SalesData.Count > 0)
                    {
                        foreach (var item in deliveryRequestViewModels)
                        {
                            var tankIdsandStoregeIds = jobwithTanks.Where(t => t.TfxJobId == item.JobId).SelectMany(t1 => t1.Tanks).Where(t2 => t2.TfxProductTypeId == item.ProductTypeId).ToList();
                            var tankData = respData.SalesData.Where(t => t.SiteId == item.SiteId && tankIdsandStoregeIds.Any(t1 => t1.StorageTypeId == t.TankId && t1.StorageId == t.StorageId)).ToList();
                            if (tankData != null)
                            {
                                var Inventory = tankData.Sum(x => x.InventoryData);
                                var Ullage = tankData.Sum(x => !string.IsNullOrEmpty(x.Ullage) ? Convert.ToDecimal(x.Ullage) : 0);
                                item.CurrentInventory = Inventory <= 0 ? Resource.lblHyphen : Inventory.ToString();
                                item.Ullage = Ullage > 0 ? Ullage.ToString() : "0";
                            }
                        }
                    }
                }
            }
        }
        public async Task<SalesInventoryResponseModel> GetTanksInventory(List<string> siteIds, List<string> tankIds, List<string> storageIds, int timeout = 30)
        {
            var result = new SalesInventoryResponseModel();
            try
            {
                //Create SiteIds Table
                DataTable SiteIds, TankIds, StorageIds;
                IntializeDemandsParameters(siteIds, tankIds, storageIds, out SiteIds, out TankIds, out StorageIds);
                var siteIdParam = new SqlParameter("@SiteList", SqlDbType.Structured);
                siteIdParam.Value = SiteIds;
                siteIdParam.TypeName = "dbo.DemandsSearchTypes";

                var tankIdsParam = new SqlParameter("@TankList", SqlDbType.Structured);
                tankIdsParam.Value = TankIds;
                tankIdsParam.TypeName = "dbo.DemandsSearchTypes";

                var storageIdParams = new SqlParameter("@StorageList", SqlDbType.Structured);
                storageIdParams.Value = StorageIds;
                storageIdParams.TypeName = "dbo.DemandsSearchTypes";

                context.Database.CommandTimeout = timeout;
                result.SalesData = await context.Database.SqlQuery<SalesInventoryData>("usp_GetTanksInventoryData @SiteList,@TankList,@StorageList", siteIdParam, tankIdsParam, storageIdParams).ToListAsync();

                result.StatusCode = (int)Status.Success;
                result.StatusMessage = Status.Success.ToString();
            }
            catch (Exception ex)
            {
                result.StatusCode = (int)Status.Failed;
                result.StatusMessage = Status.Failed.ToString();
                LogManager.Logger.WriteException("DeliveryRequestRepository", "GetTanksInventory", ex.Message, ex);
            }
            return result;
        }
        public async Task<List<DeliveryRequestViewModel>> GetBrokeredDrRequestedByMe(int companyId, string regionId, string selectedDate)
        {
            var response = new List<DeliveryRequestViewModel>();
            if (!string.IsNullOrWhiteSpace(regionId))
            {
                ObjectId objRegionId = ObjectId.Empty;
                ObjectId.TryParse(regionId, out objRegionId);

                var deliveryRequestFilter = Builders<DeliveryRequest>.Filter.And(
          Builders<DeliveryRequest>.Filter.Eq(u => u.TfxAssignedToCompanyId, companyId),
          Builders<DeliveryRequest>.Filter.Eq(u => u.TfxAssignedToRegionId, regionId),
           Builders<DeliveryRequest>.Filter.Eq(u => u.CreatedRegionId, objRegionId),
          Builders<DeliveryRequest>.Filter.Eq(u => u.IsActive, true),
           Builders<DeliveryRequest>.Filter.Eq(u => u.IsDeleted, false),
          Builders<DeliveryRequest>.Filter.Ne(u => u.CarrierStatus, (int)BrokeredDrCarrierStatus.None),
           Builders<DeliveryRequest>.Filter.Ne(u => u.CarrierStatus, (int)BrokeredDrCarrierStatus.Recalled),
           Builders<DeliveryRequest>.Filter.Ne(u => u.CarrierStatus, (int)BrokeredDrCarrierStatus.Delivered)
          );
                var deliveryRequests = mdbContext.DeliveryRequests.Find(deliveryRequestFilter).ToList();
                deliveryRequests = deliveryRequests.OrderBy(x => x.CurrentThreshold).ToList();
                var drModels = deliveryRequests.Select(t => t.ToDeliveryRequestViewModel()).ToList();
                response.GetBlendDRInfo(drModels, true);
                if (response.Any())
                {
                    GetJobCompatibilityInfo(response.Select(t => t.JobId).Distinct().ToList(), response, selectedDate);
                    await AddCurrentInventoryDetails(response);
                    await GetDrCarrierInfo(response);
                }
            }
            return response;
        }

        private void GetRecurringScheduleInformation(List<DeliveryRequestViewModel> response)
        {
            var recurringScheduleInfo = response.Where(top => top.isRecurringSchedule).ToList();
            var recurringfilterInfo = response.Where(top => top.isRecurringSchedule).Select(top => ObjectId.Parse(top.RecurringScheduleId)).ToList();
            if (recurringfilterInfo.Any())
            {
                var filterDef = new FilterDefinitionBuilder<RecurringSchedules>();
                var filter = filterDef.In(x => x.Id, recurringfilterInfo);
                var collectionRecurringInfo = mdbContext.RecurringSchedules.Find(filter).ToList();
                foreach (var item in recurringScheduleInfo)
                {
                    var scheduleInfo = collectionRecurringInfo.Where(top => top.Id == ObjectId.Parse(item.RecurringScheduleId)).FirstOrDefault();
                    if (scheduleInfo != null)
                    {
                        item.RecurringScheduleInfo += ((ScheduleTypes)scheduleInfo.ScheduleType).ToString();
                        if (scheduleInfo.ScheduleType == (int)ScheduleTypes.Weekly || scheduleInfo.ScheduleType == (int)ScheduleTypes.BiWeekly)
                        {
                            string weekname = string.Empty;
                            weekname = GetWeekInformation(scheduleInfo, weekname);
                            weekname = string.Join(",", weekname.Split(',').Select(s => s.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray());
                            item.RecurringScheduleInfo += ": " + weekname;
                        }
                        else
                        {
                            item.RecurringScheduleInfo += ": " + scheduleInfo.Date;
                        }
                        //item.ScheduleQuantityTypeText = GetSchduleQtyTypeText(scheduleInfo.ScheduleQuantityType);
                    }
                }
            }
        }

        private static string GetSchduleQtyTypeText(int ScheduleQuantityType)
        {
            string qtyText = string.Empty;
            var enumerationType = typeof(ScheduleQuantityType);
            var name = Enum.GetName(enumerationType, ScheduleQuantityType);
            var memberInfo = enumerationType.GetMember(name);
            if (memberInfo.Count() > 0)
            {
                var attributes = memberInfo[0].GetCustomAttributes(typeof(DisplayAttribute), false);
                if (attributes.Count() > 0)
                {
                    var description = ((DisplayAttribute)attributes[0]).Name;
                    qtyText = description;
                }
            }
            return qtyText;
        }

        private static string GetWeekInformation(RecurringSchedules scheduleInfo, string weekname)
        {
            foreach (var weekInfo in scheduleInfo.WeekDayId)
            {
                switch (weekInfo)
                {
                    case "1":
                        weekname += "MON,";
                        break;
                    case "2":
                        weekname += "TUE,";
                        break;
                    case "3":
                        weekname += "WED,";
                        break;
                    case "4":
                        weekname += "THU,";
                        break;
                    case "5":
                        weekname += "FRI,";
                        break;
                    case "6":
                        weekname += "SAT,";
                        break;
                    default:
                        weekname += "SUN,";
                        break;
                }

            }

            return weekname;
        }

        public async Task<DeliveryRequestViewModel> GetDeliveryRequestById(string deliveryReqId)
        {
            var response = new DeliveryRequestViewModel();
            ObjectId objDrId = ObjectId.Empty;
            if (!string.IsNullOrWhiteSpace(deliveryReqId))
            {
                objDrId = ObjectId.Parse(deliveryReqId);
            }
            var deliveryRequest = await mdbContext.DeliveryRequests.Find(t => t.Id == objDrId).FirstOrDefaultAsync();
            response = deliveryRequest.ToDeliveryRequestViewModel();
            return response;

        }

        public async Task<StatusModel> UpdateDeliveryRequest(List<DeliveryRequestViewModel> deliveryModel)
        {
            var response = new StatusModel();
            if (deliveryModel != null && deliveryModel.Any())
            {
                
                var objectIdsForDr = deliveryModel.Select(t => ObjectId.Parse(t.Id)).Distinct().ToList();
                var deliveryRequestObjs = await mdbContext.DeliveryRequests.Find(t => objectIdsForDr.Contains(t.Id)).Project(t => new { t.IsRecurringSchedule, t.RecurringScheduleId, t.Id, t.CreatedBy }).ToListAsync();
                foreach (var model in deliveryModel)
                {
                    TimeSpan? scheduleStartTime = new TimeSpan();
                    if (!string.IsNullOrWhiteSpace(model.ScheduleStartTime))
                        scheduleStartTime = Convert.ToDateTime(model.ScheduleStartTime).TimeOfDay;
                    else
                        scheduleStartTime = null;

                    TimeSpan? scheduleEndTime = new TimeSpan();
                    if (!string.IsNullOrWhiteSpace(model.ScheduleEndTime))
                        scheduleEndTime = Convert.ToDateTime(model.ScheduleEndTime).TimeOfDay;
                    else
                        scheduleEndTime = null;

                    var existingDr = deliveryRequestObjs.FirstOrDefault(t => t.Id.ToString() == model.Id);
                    if (existingDr != null)
                    {
                        if (model.IsDeleted)
                        {
                            var updateFields = Builders<DeliveryRequest>.Update
                                .Set(t => t.IsDeleted, true)
                                .Set(t => t.IsReCreated, false)
                                .Set(t => t.IsActive, false);

                            var filter = Builders<DeliveryRequest>.Filter.And(Builders<DeliveryRequest>.Filter.Where(x => x.Id == existingDr.Id));

                            await mdbContext.DeliveryRequests.UpdateOneAsync(filter, updateFields);
                            if (existingDr.IsRecurringSchedule && existingDr.RecurringScheduleId != ObjectId.Empty)
                            {
                                await DeleteRecurringScheduleIfExists(existingDr.RecurringScheduleId, existingDr.CreatedBy);
                            }
                        }
                        else
                        {
                            var sDate = DateTime.TryParse(model.SelectedDate, out DateTime selDate) ? selDate : (DateTime?)null;
                            var updateFields = Builders<DeliveryRequest>.Update
                                .Set(t => t.RequiredQuantity, model.RequiredQuantity)
                                .Set(t => t.QuantityInPercent, model.QuantityInPercent)
                                .Set(t => t.TfxCreatedByCompanyId, model.CreatedByCompanyId)
                                .Set(t => t.ScheduleQuantityType, model.ScheduleQuantityType)
                                .Set(t => t.SelectedDate, sDate)
                                .Set(t => t.ScheduleStartTime, scheduleStartTime)
                                .Set(t => t.ScheduleEndTime, scheduleEndTime)
                                .Set(t => t.Notes, model.Notes)
                                .Set(t => t.DeliveryLevelPO, model.DeliveryLevelPO)
                                .Set(t => t.Priority, model.Priority);

                            var filter = Builders<DeliveryRequest>.Filter.And(
                                    Builders<DeliveryRequest>.Filter.Where(x => x.Id == existingDr.Id)
                                );

                            await mdbContext.DeliveryRequests.UpdateOneAsync(filter, updateFields);
                        }
                    }
                }
                response.StatusMessage = deliveryModel.FirstOrDefault().IsDeleted ? Resource.msgDelReqDeleteSuccess : Resource.msgDelReqEditSuccess;
            }
            response.StatusCode = (int)Status.Success;
            return response;
        }



        private async Task DeleteRecurringScheduleIfExists(ObjectId recurringScheduleId, int createdBy)
        {
            var recurringDRCount = await mdbContext.DeliveryRequests.Find(t => t.RecurringScheduleId == recurringScheduleId).CountDocumentsAsync();
            if (recurringDRCount > 0)
            {
                //delete recurring schedules
                var recurringFilter = Builders<RecurringSchedules>.Filter.And(
                     Builders<RecurringSchedules>.Filter.Where(x => x.Id == recurringScheduleId));
                UpdateDefinition<RecurringSchedules> updateRecurringFields = null;
                updateRecurringFields = Builders<RecurringSchedules>.Update
                        .Set(t => t.IsActive, false)
                        .Set(t => t.IsDeleted, true)
                        .Set(t => t.UpdatedBy, createdBy)
                        .Set(t => t.UpdatedOn, DateTime.Now);
                await mdbContext.RecurringSchedules.UpdateManyAsync(recurringFilter, updateRecurringFields);
            }
        }

        public List<DeliveryRequestViewModel> GetDeliveryRequestsbyPriority(DeliveryReqPriority priority, int companyId)
        {
            var response = new List<DeliveryRequestViewModel>();
            if (priority != DeliveryReqPriority.None)
            {
                var deliveryRequestFilter = Builders<DeliveryRequest>.Filter.And(
           Builders<DeliveryRequest>.Filter.Eq(u => u.TfxAssignedToCompanyId, companyId),
           Builders<DeliveryRequest>.Filter.Eq(u => u.IsActive, true),
            Builders<DeliveryRequest>.Filter.Eq(u => u.Priority, priority),
           Builders<DeliveryRequest>.Filter.Ne(u => u.Status, DeliveryReqStatus.ScheduleCreated)

           );
                var deliveryRequestTypeFilter = Builders<DeliveryRequest>.Filter.And(
           Builders<DeliveryRequest>.Filter.Eq(u => u.DeliveryRequestType, 0) |
           Builders<DeliveryRequest>.Filter.Eq(u => u.DeliveryRequestType, 1));

                var deliveryRequests = mdbContext.DeliveryRequests.Find(deliveryRequestFilter & deliveryRequestTypeFilter).ToList();
                deliveryRequests.ForEach(dr => response.Add(dr.ToDeliveryRequestViewModel()));
            }
            else
            {
                var deliveryRequestFilter = Builders<DeliveryRequest>.Filter.And(
           Builders<DeliveryRequest>.Filter.Eq(u => u.TfxAssignedToCompanyId, companyId),
           Builders<DeliveryRequest>.Filter.Eq(u => u.IsActive, true),
           Builders<DeliveryRequest>.Filter.Ne(u => u.Status, DeliveryReqStatus.ScheduleCreated)

           );
                var deliveryRequestTypeFilter = Builders<DeliveryRequest>.Filter.And(
           Builders<DeliveryRequest>.Filter.Eq(u => u.DeliveryRequestType, 0) |
           Builders<DeliveryRequest>.Filter.Eq(u => u.DeliveryRequestType, 1));

                var deliveryRequests = mdbContext.DeliveryRequests.Find(deliveryRequestFilter & deliveryRequestTypeFilter).ToList();
                deliveryRequests.ForEach(dr => response.Add(dr.ToDeliveryRequestViewModel()));
            }
            return response;
        }

        public async Task ScheduleDeliveryRequest()
        {
            try
            {
                List<ForcastingServiceSetting> forcastingServiceSettings = new List<ForcastingServiceSetting>();
                var allTanks = await mdbContext.JobAdditionalDetails
                                                .Find(t => t.TfxDisplayJobId != null && t.Tanks != null && t.Tanks.Any() && t.IsActive)
                                                .Project(t1 => new JobAdditionalDetailsModel
                                                {
                                                    JobId = t1.TfxJobId,
                                                    TfxDisplayJobId = t1.TfxDisplayJobId,
                                                    IsAutoCreateDREnable = t1.IsAutoCreateDREnable,
                                                    TankDetails = t1.Tanks.Select(t2 => new TankDetailsModel
                                                    {
                                                        TankName = t2.TankName,
                                                        AssetId = t2.TfxAssetId,
                                                        StorageId = t2.StorageId,
                                                        TankId = t2.StorageTypeId,
                                                        FuelCapacity = t2.FuelCapacity,
                                                        MaxFill = t2.MaxFill,
                                                        FillType = (FillType)t2.FillType,
                                                        ProductTypeName = t2.TfxProductTypeName,
                                                        FuelTypeId = t2.TfxProductTypeId,
                                                        JobName = t1.TfxJobName,
                                                        JobId = t1.TfxJobId,
                                                        MinFill = t2.MinFill,
                                                        RunOutLevel = t2.RunOutLevel,
                                                        ThresholdDeliveryRequest = t2.ThresholdDeliveryRequest,
                                                        JobDisplayId = t1.TfxDisplayJobId,
                                                    }).ToList()
                                                })
                                                .ToListAsync();
                var allTfxJobIds = allTanks.Select(t => t.JobId).Distinct().ToList();

                var allcarrierJobs = await mdbContext.CarrierJobs.Find(t => allTfxJobIds.Contains(t.TfxJobId) && t.IsActive)
                                                                 .Project(t => new CarrierJobInfoModel
                                                                 {
                                                                     TfxJobId = t.TfxJobId,
                                                                     TfxSupplierCompanyId = t.TfxSupplierCompanyId,
                                                                     TfxCarrierCompanyId = t.TfxCarrierCompanyId
                                                                 }).ToListAsync();
                var allTankAssetIds = allTanks.SelectMany(t => t.TankDetails.Select(t1 => t1.AssetId)).Distinct().ToList();
                if (allTankAssetIds.Count > 0)
                {
                    forcastingServiceSettings = GetForcastingSetting(allTankAssetIds).Where(top => top.IsAutoDRCreation).ToList();
                }

                foreach (var item in allTanks)
                {
                    var itemStorageIds = item.TankDetails.Select(t1 => t1.StorageId).Distinct().ToList();
                    var itemTankIds = item.TankDetails.Select(t1 => t1.TankId).Distinct().ToList();
                    //var demandsData = context.Demands.Where(t => t.SiteId == item.TfxDisplayJobId
                    //                                                    && itemStorageIds.Contains(t.StorageId)
                    //                                                    && itemTankIds.Contains(t.TankId)
                    //                                                    && t.NetVolume > -1)
                    //                                        .GroupBy(t => new { t.SiteId, t.StorageId, t.TankId })
                    //                                        .Select(t => t.OrderByDescending(t1 => t1.CaptureTime).FirstOrDefault()).ToList();
                    var demandsData = await GetDemands(new List<string> { item.TfxDisplayJobId }, itemTankIds, itemStorageIds);

                    var allTanksDeliveryRequests = await mdbContext.DeliveryRequests.Find(t => t.TfxJobId == item.JobId
                                                                                        && itemStorageIds.Contains(t.StorageId)
                                                                                        && itemTankIds.Contains(t.StorageTypeId)
                                                                                        && string.IsNullOrEmpty(t.ParentId)
                                                                                        && t.IsActive && !t.IsDeleted && t.GroupChildDRs.Count() == 0
                                                                                        && (t.Status != DeliveryReqStatus.ScheduleCreated
                                                                                               ||
                                                                                               (t.TfxScheduleEnrouteStatus != (int)EnrouteDeliveryStatus.CompletedDrop
                                                                                                && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.Completed
                                                                                                && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.CompletedLate
                                                                                                && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.RescheduledCompleted
                                                                                                && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.Canceled
                                                                                                && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.RescheduledMissed
                                                                                                && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.Missed)
                                                                                            )
                                                                                        && t.Status != DeliveryReqStatus.Deleted
                                                                                        && (t.ScheduleQuantityType == 0 || t.ScheduleQuantityType == (int)ScheduleQuantityType.Quantity)
                                                                                        ).ToListAsync();


                    foreach (var tank in item.TankDetails)
                    {
                        var updatedDipTest = demandsData.FirstOrDefault(t => t.SiteId == item.TfxDisplayJobId
                                                                                && t.StorageId == tank.StorageId
                                                                                && t.TankId == tank.TankId);

                        if (updatedDipTest == null || updatedDipTest.NetVolume == -1 || updatedDipTest.IsProcessed)
                        {
                            //LogManager.Logger.WriteDebug("DeliveryRequestRepository", "ScheduleDeliveryRequest", $"Dip test not found or already processed for jobId:{tank.JobId}, siteId:{item.TfxDisplayJobId}, storagetypeId:{tank.TankId}, storageId:{tank.StorageId}");
                            continue;
                        }
                        var forcastingSetting = forcastingServiceSettings.Where(x => x.Id == tank.AssetId).FirstOrDefault();
                        if (forcastingSetting == null)
                        {
                            //auto dr creation using job level auto DR flag
                            await CreateDRUsingJobAutoDRSetting(item, tank, allTanksDeliveryRequests, allcarrierJobs, updatedDipTest);
                        }
                        else
                        {
                            //auto dr creation using forecasting level auto DR flag
                            await CreateDRUsingForecastingAutoDRSetting(item, tank, forcastingSetting, allTanksDeliveryRequests, allcarrierJobs, updatedDipTest);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryRequestRepository", "ScheduleDeliveryRequest", ex.Message, ex);
            }
        }

        private async Task CreateDRUsingJobAutoDRSetting(JobAdditionalDetailsModel job, TankDetailsModel tank, List<DeliveryRequest> allTanksDeliveryRequests, List<CarrierJobInfoModel> allcarrierJobs, Demand updatedDipTest)
        {
            // parentId flag to remove missed DR  
            var allDeliveryRequests = allTanksDeliveryRequests.Where(t => t.TfxJobId == tank.JobId
                                                                         && t.StorageId == tank.StorageId
                                                                         && t.StorageTypeId == tank.TankId).ToList();

            var netVolume = (updatedDipTest?.NetVolume) ?? 0;
            decimal.TryParse(netVolume.ToString(), out decimal currentVolume);
            if (tank.FuelCapacity == null || tank.FuelCapacity.Value <= 0)
            {
                return; // Do not create DR as capacity is not mentioned for the tank.
            }

            var current = (currentVolume / tank.FuelCapacity.Value) * 100;
            var currentInventory = (current < 0) ? 0 : (current > 100) ? 100 : current;
            var definedPrioirity = GetDeliveryRequestPriority(currentInventory, tank.MinFill, (int)tank.FillType, tank.RunOutLevel, tank.FuelCapacity, tank.ThresholdDeliveryRequest);
            var priority = definedPrioirity == DeliveryReqPriority.None ? DeliveryReqPriority.CouldGo : definedPrioirity;
            var tankMaxFill = tank.FuelCapacity.Value;
            if (tank.FillType == FillType.UoM)
            {
                tankMaxFill = tank.MaxFill ?? tank.FuelCapacity.Value;
            }
            else
            {
                var maxFill = tank.MaxFill ?? 100;
                tankMaxFill = (maxFill * tank.FuelCapacity.Value / 100);
            }

            var balanceRequiredQuantity = Convert.ToDecimal(updatedDipTest?.Ullage ?? 0); // - DS Created
            if (balanceRequiredQuantity <= 0)
            {
                balanceRequiredQuantity = (tankMaxFill - currentVolume); // - DS Created
            }

            var deliveryRequests = allDeliveryRequests.Where(t => t.Status != DeliveryReqStatus.ScheduleCreated && t.Status != DeliveryReqStatus.Draft).ToList();
            if (deliveryRequests != null && deliveryRequests.Any())
            {
                // update DR
                UpdateDefinition<DeliveryRequest> updateFields = Builders<DeliveryRequest>.Update.Set(t => t.Priority, priority)
                                                                   .Set(t => t.CurrentThreshold, currentInventory);
                var drIds = deliveryRequests.Select(t => t.Id);
                var updateResult = mdbContext.DeliveryRequests.UpdateMany(Builders<DeliveryRequest>.Filter.Where(x => drIds.Contains(x.Id)), updateFields);
                //LogManager.Logger.WriteDebug("DeliveryRequestRepository", "CreateDRUsingJobAutoDRSetting", $"Update Automate DR Priority to:{priority}, currentThreshold:{currentInventory}, StorageTypeId:{tank.TankId}, StorageId:{tank.StorageId}, JobId:{tank.JobId}");

                var totalDrQuantity = deliveryRequests.Sum(t => t.RequiredQuantity);
                balanceRequiredQuantity -= totalDrQuantity;

                //update last dr with balanceDr
                if (balanceRequiredQuantity != 0 && job.IsAutoCreateDREnable)
                {
                    var i = 0;
                    for (i = deliveryRequests.Count(); i > 0; i--)
                    {
                        var drToUpdate = deliveryRequests[i - 1];
                        if (drToUpdate != null && balanceRequiredQuantity != 0)
                        {
                            UpdateRouteInformation(drToUpdate);
                            var prevQuantity = drToUpdate.RequiredQuantity;
                            var newQuantity = drToUpdate.RequiredQuantity + balanceRequiredQuantity;
                            drToUpdate.AutoUpdatedOn = DateTimeOffset.Now;
                            if (newQuantity <= 0)
                            {
                                drToUpdate.AutoDRStatus = AutoDrStatus.Delete;
                                drToUpdate.IsActive = false;
                                drToUpdate.IsDeleted = true;
                                balanceRequiredQuantity = newQuantity;
                                mdbContext.DeliveryRequests.FindOneAndReplace(Builders<DeliveryRequest>.Filter.Where(x => x.Id == drToUpdate.Id), drToUpdate);
                                //LogManager.Logger.WriteDebug("DeliveryRequestRepository", "CreateDRUsingJobAutoDRSetting", $"InActivate DR because new ullage is less than already created DR quantity having:{drToUpdate.RequiredQuantity}, DR id:{drToUpdate.Id}");
                                continue;
                            }
                            else
                            {
                                drToUpdate.RequiredQuantity = newQuantity;
                                if (drToUpdate.AutoDRStatus == AutoDrStatus.None)//Manually created DR
                                {
                                    drToUpdate.AutoDRStatus = AutoDrStatus.Update;
                                }
                                else if (drToUpdate.AutoDRStatus == AutoDrStatus.Create)//Auto Created DR
                                {
                                    drToUpdate.AutoDRStatus = AutoDrStatus.CreateAndUpdate;
                                }
                                drToUpdate.Priority = priority;
                                drToUpdate.CurrentThreshold = currentInventory;
                                mdbContext.DeliveryRequests.FindOneAndReplace(Builders<DeliveryRequest>.Filter.Where(x => x.Id == drToUpdate.Id), drToUpdate);
                                //LogManager.Logger.WriteDebug("DeliveryRequestRepository", "CreateDRUsingJobAutoDRSetting", $"Update Automate DR Quantity to:{drToUpdate.RequiredQuantity}, prevDrQuantity:{prevQuantity}, DR id:{drToUpdate.Id}");
                                break; // balanceRequiredQuantity = 0;
                            }
                        }
                    }
                }
            }
            else if (balanceRequiredQuantity > 0 && job.IsAutoCreateDREnable)
            {
                if (definedPrioirity == DeliveryReqPriority.None)
                {
                    await SetProcessedDipValue(updatedDipTest);
                    //LogManager.Logger.WriteDebug("DeliveryRequestRepository", "CreateDRUsingJobAutoDRSetting", $"current inventory level:{currentInventory} for storageTypeid:{tank.TankId}. Hence no new DR will create for this Tank.");
                    return;
                }
                var DSCreated = allDeliveryRequests.Where(t => t.Status == DeliveryReqStatus.ScheduleCreated || t.Status == DeliveryReqStatus.Draft).ToList();
                var isAnyDSCreated = DSCreated != null && DSCreated.Any() && allDeliveryRequests.Any(t => t.TfxScheduleEnrouteStatus != (int)EnrouteDeliveryStatus.CompletedDrop
                                                   && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.Completed
                                                   && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.CompletedLate
                                                   && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.RescheduledCompleted
                                                   && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.Canceled
                                                   && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.RescheduledMissed
                                                   && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.Missed);

                if (isAnyDSCreated)
                {
                    var existingDs = DSCreated.Last();
                    //LogManager.Logger.WriteDebug("DeliveryRequestRepository", "CreateDRUsingJobAutoDRSetting", $"Delivery schedule exist for DRId:{existingDs.Id.ToString()}");
                    await SetProcessedDipValue(updatedDipTest);
                    return;
                }
                // create a new dr for bal Quantity
                List<DeliveryRequestViewModel> drToCreate = new List<DeliveryRequestViewModel>();
                DeliveryRequestViewModel dr = new DeliveryRequestViewModel()
                {
                    RequiredQuantity = balanceRequiredQuantity,
                    CurrentQuantity = currentVolume,
                    CurrentThreshold = currentInventory,
                    ProductTypeId = tank.FuelTypeId,
                    ProductType = tank.ProductTypeName,
                    StorageId = tank.StorageId,
                    TankId = tank.TankId,
                    JobId = tank.JobId,
                    JobName = tank.JobName,
                    Priority = priority,
                    TankMaxFill = tankMaxFill,
                    SiteId = job.TfxDisplayJobId,
                    CreatedBy = (int)SystemUser.System,
                    CreatedOn = DateTimeOffset.Now,
                    UpdatedOn = DateTimeOffset.Now,
                    IsActive = true,
                    IsDeleted = false,
                    DelReqSource = DRSource.Auto,
                    AutoDRStatus = AutoDrStatus.Create,
                    AutoCreatedOn = DateTimeOffset.Now,
                    ScheduleQuantityType = (int)ScheduleQuantityType.Quantity,
                    ScheduleQuantityTypeText = ScheduleQuantityType.Quantity.GetDisplayName()
                };

                //get supplier assigned to the job
                int supplierCompanyId;
                var carrierJob = allcarrierJobs.Where(t => t.TfxJobId == tank.JobId).FirstOrDefault();
                if (carrierJob != null)
                {
                    supplierCompanyId = carrierJob.TfxSupplierCompanyId;
                    dr.AssignedToCompanyId = carrierJob.TfxCarrierCompanyId;
                }
                else
                {
                    supplierCompanyId = mdbContext.Regions.Find(t => t.TfxJobs.Any(t1 => t1.Id == tank.JobId) && t.IsActive)
                                                      .Project(t => t.TfxCompanyId).FirstOrDefault();
                    dr.AssignedToCompanyId = supplierCompanyId;
                }

                if (supplierCompanyId == 0)
                {
                    //LogManager.Logger.WriteDebug("DeliveryRequestRepository", "CreateDRUsingJobAutoDRSetting", $"Supplier not found for jobId:{tank.JobId}, supplierCompanyId:{supplierCompanyId}, TfxProductTypeId:{tank.FuelTypeId}");
                    return;
                }

                await SetDRAdditionalDetails(dr, tank.JobId, supplierCompanyId, tank.FuelTypeId);
                dr.CreatedBy = dr.CreatedBy > 0 ? dr.CreatedBy : (int)SystemUser.System;

                drToCreate.Add(dr);
                var response = await CreateDeliveryRequest(drToCreate);

                DeliveryRequestAccess drExchangeAccess = new DeliveryRequestAccess();
                foreach (var drItem in response.EntityIds)
                {
                    var message = new TankDeliveryRequestMessageModel { EntityId = drItem };
                    var jsonMessage = new JavaScriptSerializer().Serialize(message);
                    drExchangeAccess.AddNotificationEvent((int)EventType.TankDeliveryRequestCreated, dr.CreatedBy, jsonMessage);
                }
                LogManager.Logger.WriteDebug("DeliveryRequestRepository", "CreateDRUsingJobAutoDRSetting", $"Automate DR Creation Status:{(Status)response.StatusCode} {response.StatusMessage}, for jobId:{tank.JobId}, supplierCompanyId:{supplierCompanyId}, StorageTYpeId:{tank.TankId}, storageId:{tank.StorageId}, Quantity:{balanceRequiredQuantity}");
            }
            await SetProcessedDipValue(updatedDipTest);
        }

        private async Task CreateDRUsingForecastingAutoDRSetting(JobAdditionalDetailsModel job, TankDetailsModel tank, ForcastingServiceSetting forecastingSettings, List<DeliveryRequest> allTanksDeliveryRequests, List<CarrierJobInfoModel> allcarrierJobs, Demand updatedDipTest)
        {
            // parentId flag to remove missed DR  
            var allDeliveryRequests = allTanksDeliveryRequests.Where(t => t.TfxDisplayJobId == job.TfxDisplayJobId
                                                                         && t.StorageId == tank.StorageId
                                                                         && t.StorageTypeId == tank.TankId).ToList();

            var netVolume = (updatedDipTest?.NetVolume) ?? 0;
            decimal.TryParse(netVolume.ToString(), out decimal currentVolume);
            if (tank.FuelCapacity == null || tank.FuelCapacity.Value <= 0)
            {
                return; // Do not create DR as capacity is not mentioned for the tank.
            }

            var current = (currentVolume / tank.FuelCapacity.Value) * 100;
            var currentInventory = (current < 0) ? 0 : (current > 100) ? 100 : current;
            var tankMaxFill = tank.FuelCapacity.Value;
            if (tank.FillType == FillType.UoM)
            {
                tankMaxFill = tank.MaxFill ?? tank.FuelCapacity.Value;
            }
            else
            {
                var maxFill = tank.MaxFill ?? 100;
                tankMaxFill = (maxFill * tank.FuelCapacity.Value / 100);
            }

            var balanceRequiredQuantity = Convert.ToDecimal(updatedDipTest?.Ullage ?? 0); // - DS Created
            if (balanceRequiredQuantity <= 0)
            {
                balanceRequiredQuantity = (tankMaxFill - currentVolume); // - DS Created
            }

            //get supplier assigned to the job
            int supplierCompanyId;
            var assignedToCompanyId = 0;
            var carrierJob = allcarrierJobs.Where(t => t.TfxJobId == tank.JobId).FirstOrDefault();
            if (carrierJob != null)
            {
                supplierCompanyId = carrierJob.TfxSupplierCompanyId;
                assignedToCompanyId = carrierJob.TfxCarrierCompanyId;
            }
            else
            {
                supplierCompanyId = mdbContext.Regions.Find(t => t.TfxJobs.Any(t1 => t1.Id == tank.JobId) && t.IsActive)
                                                  .Project(t => t.TfxCompanyId).FirstOrDefault();
                assignedToCompanyId = supplierCompanyId;
            }

            // Get tank retain, safty-stock and runout levels
            ForecastingRepository _forecastingRepository = new ForecastingRepository();
            var saleTank = await _forecastingRepository.GetSaleTank(job.TfxDisplayJobId, tank.TankId, tank.StorageId);
            if (saleTank == null)
            {
                return;
            }

            var monthStartDate = DateTime.Now.AddDays(-29).Date;
            var monthEndDate = DateTime.Now.AddDays(-1).Date;
            var monthlyDatas = await _forecastingRepository.GetSaleTankMonthlyData(monthStartDate, monthEndDate, saleTank.Id);

            TankLevelHelper tankLevelHelper = new TankLevelHelper();
            var tankDetails = tankLevelHelper.GetTankDetails(tank);
            var startTime = updatedDipTest?.CaptureTime ?? DateTime.Now;
            var thisTankLevels = tankLevelHelper.GetTankLevels(monthlyDatas, startTime, currentVolume, saleTank.BandPeriod, 168, tankDetails.Retain, tankDetails.SafetyStock, tankDetails.RunOutLevel);
            var thisTankLevelPriority = DeliveryReqPriority.None;
            DeliveryWindowInfoModel deliveryWindowInfo = null;
            if (thisTankLevels.Any())
            {
                var level = thisTankLevels.OrderBy(t => t.Type).First();
                if (forecastingSettings.InventoryPriorityType == 2)
                {
                    if (forecastingSettings.InventoryUOM == 2)
                    {
                        int defaulthours = 24;
                        forecastingSettings.RetainCouldGo = forecastingSettings.RetainCouldGo * defaulthours;
                        forecastingSettings.SafetyStockShouldGo = forecastingSettings.SafetyStockShouldGo * defaulthours;
                        forecastingSettings.RunoutLevelMustGo = forecastingSettings.RunoutLevelMustGo * 24;
                    }
                    if (level.Type == 1 && level.Hours <= forecastingSettings.RetainCouldGo)
                    {
                        thisTankLevelPriority = DeliveryReqPriority.CouldGo;
                    }
                    else if (level.Type == 2 && level.Hours <= forecastingSettings.SafetyStockShouldGo)
                    {
                        thisTankLevelPriority = DeliveryReqPriority.ShouldGo;
                    }
                    else if (level.Type == 3 && level.Hours <= forecastingSettings.RunoutLevelMustGo)
                    {
                        thisTankLevelPriority = DeliveryReqPriority.MustGo;
                    }
                }
                else
                {
                    if (forecastingSettings.InventoryUOM == 1)
                    {
                        if (level.Type == 1 && level.Quantity <= forecastingSettings.RetainCouldGo)
                        {
                            thisTankLevelPriority = DeliveryReqPriority.CouldGo;
                        }
                        else if (level.Type == 2 && level.Quantity <= forecastingSettings.SafetyStockShouldGo)
                        {
                            thisTankLevelPriority = DeliveryReqPriority.ShouldGo;
                        }
                        else if (level.Type == 3 && level.Quantity <= forecastingSettings.RunoutLevelMustGo)
                        {
                            thisTankLevelPriority = DeliveryReqPriority.MustGo;
                        }
                    }
                    else
                    {
                        if (tank.FuelCapacity != null && tank.FuelCapacity.HasValue && tank.FuelCapacity.Value > 0)
                        {
                            forecastingSettings.RetainCouldGo = Convert.ToInt32((tank.FuelCapacity.Value * forecastingSettings.RetainCouldGo) / 100);
                            forecastingSettings.SafetyStockShouldGo = Convert.ToInt32((tank.FuelCapacity.Value * forecastingSettings.SafetyStockShouldGo) / 100);
                            forecastingSettings.RetainCouldGo = Convert.ToInt32((tank.FuelCapacity.Value * forecastingSettings.RetainCouldGo) / 100);
                        }
                        if (level.Type == 1 && level.Quantity <= forecastingSettings.RetainCouldGo)
                        {
                            thisTankLevelPriority = DeliveryReqPriority.CouldGo;
                        }
                        else if (level.Type == 2 && level.Quantity <= forecastingSettings.SafetyStockShouldGo)
                        {
                            thisTankLevelPriority = DeliveryReqPriority.ShouldGo;
                        }
                        else if (level.Type == 3 && level.Quantity <= forecastingSettings.RunoutLevelMustGo)
                        {
                            thisTankLevelPriority = DeliveryReqPriority.MustGo;
                        }
                    }
                }
                if (thisTankLevelPriority != DeliveryReqPriority.None)
                {
                    deliveryWindowInfo = new DeliveryWindowInfoModel();
                    if (forecastingSettings.RetainTimeBufferUOM == (int)RateOfConsumsionUOM.Hours)
                    {
                        deliveryWindowInfo.RetainDate = DateTime.Now.AddHours(level.Hours);
                    }
                    else
                    {
                        deliveryWindowInfo.RetainDate = DateTime.Now.AddDays(level.Hours);
                    }
                    deliveryWindowInfo.RetainTime = deliveryWindowInfo.RetainDate.ToString(Resource.constFormat12HourTime);

                    var retainHoursStart = level.Hours - Math.Max(forecastingSettings.RetainTimeBuffer, forecastingSettings.StartBuffer);
                    deliveryWindowInfo.StartDate = DateTime.Now.AddHours(retainHoursStart);
                    deliveryWindowInfo.StartTime = deliveryWindowInfo.StartDate.ToString(Resource.constFormat12HourTime);

                    var retainHoursEnd = level.Hours + forecastingSettings.EndBuffer;
                    deliveryWindowInfo.EndDate = DateTime.Now.AddHours(retainHoursEnd);
                    deliveryWindowInfo.EndTime = deliveryWindowInfo.EndDate.ToString(Resource.constFormat12HourTime);
                }

                if (thisTankLevelPriority == DeliveryReqPriority.MustGo)
                {
                    AddNotificationForTankHitRunOutLevel(tank, currentInventory, updatedDipTest.Ullage, supplierCompanyId);
                }
            }
            if (thisTankLevelPriority == DeliveryReqPriority.None)
            {
                // Tank level not reached, so don't create/update DR
                await SetProcessedDipValue(updatedDipTest);
                return;
            }
            //filter the records based on DeliveryWindowInfo start date asc and start date grether than current date. and take the 1st records.
            var deliveryRequests = allDeliveryRequests.Where(t => t.Status != DeliveryReqStatus.ScheduleCreated && t.Status != DeliveryReqStatus.Draft
                                    && t.DeliveryWindowInfo != null && t.DeliveryWindowInfo.StartDate.Date >= DateTime.Now.Date)
                                    .OrderBy(top => top.DeliveryWindowInfo.StartDate.Date).Take(1).ToList();
            if (deliveryRequests != null && deliveryRequests.Any())
            {
                // update DR
                UpdateDefinition<DeliveryRequest> updateFields = Builders<DeliveryRequest>.Update.Set(t => t.Priority, thisTankLevelPriority)
                                                                   .Set(t => t.CurrentThreshold, currentInventory);
                var drIds = deliveryRequests.Select(t => t.Id);
                var updateResult = mdbContext.DeliveryRequests.UpdateMany(Builders<DeliveryRequest>.Filter.Where(x => drIds.Contains(x.Id)), updateFields);
                LogManager.Logger.WriteDebug("DeliveryRequestRepository", "CreateDRUsingForecastingAutoDRSetting", $"Update Automate DR Priority to:{thisTankLevelPriority}, currentThreshold:{currentInventory}, StorageTypeId:{tank.TankId}, StorageId:{tank.StorageId}, JobId:{tank.JobId}");

                var totalDrQuantity = deliveryRequests.Sum(t => t.RequiredQuantity);
                balanceRequiredQuantity -= totalDrQuantity;

                //update last dr with balanceDr
                if (balanceRequiredQuantity != 0 && forecastingSettings.IsAutoDRCreation)
                {
                    var i = 0;
                    for (i = deliveryRequests.Count(); i > 0; i--)
                    {
                        var drToUpdate = deliveryRequests[i - 1];
                        if (drToUpdate != null && balanceRequiredQuantity != 0)
                        {
                            UpdateRouteInformation(drToUpdate);
                            var prevQuantity = drToUpdate.RequiredQuantity;
                            var newQuantity = drToUpdate.RequiredQuantity + balanceRequiredQuantity;
                            drToUpdate.AutoUpdatedOn = DateTimeOffset.Now;
                            if (newQuantity <= 0)
                            {
                                drToUpdate.AutoDRStatus = AutoDrStatus.Delete;
                                drToUpdate.IsActive = false;
                                drToUpdate.IsDeleted = true;
                                balanceRequiredQuantity = newQuantity;
                                mdbContext.DeliveryRequests.FindOneAndReplace(Builders<DeliveryRequest>.Filter.Where(x => x.Id == drToUpdate.Id), drToUpdate);
                                LogManager.Logger.WriteDebug("DeliveryRequestRepository", "CreateDRUsingForecastingAutoDRSetting", $"InActivate DR because new ullage is less than already created DR quantity having:{drToUpdate.RequiredQuantity}, DR id:{drToUpdate.Id}");
                                continue;
                            }
                            else
                            {
                                drToUpdate.RequiredQuantity = newQuantity;
                                if (drToUpdate.AutoDRStatus == AutoDrStatus.None)//Manually created DR
                                {
                                    drToUpdate.AutoDRStatus = AutoDrStatus.Update;
                                }
                                else if (drToUpdate.AutoDRStatus == AutoDrStatus.Create)//Auto Created DR
                                {
                                    drToUpdate.AutoDRStatus = AutoDrStatus.CreateAndUpdate;
                                }
                                drToUpdate.Priority = thisTankLevelPriority;
                                drToUpdate.CurrentThreshold = currentInventory;
                                drToUpdate.DeliveryWindowInfo = deliveryWindowInfo?.ToEntity();
                                mdbContext.DeliveryRequests.FindOneAndReplace(Builders<DeliveryRequest>.Filter.Where(x => x.Id == drToUpdate.Id), drToUpdate);
                                LogManager.Logger.WriteDebug("DeliveryRequestRepository", "CreateDRUsingForecastingAutoDRSetting", $"Update Automate DR Quantity to:{drToUpdate.RequiredQuantity}, prevDrQuantity:{prevQuantity}, DR id:{drToUpdate.Id}");
                                break; // balanceRequiredQuantity = 0;
                            }
                        }
                    }
                }
            }
            else if (balanceRequiredQuantity > 0 && forecastingSettings.IsAutoDRCreation)
            {
                var DSCreated = allDeliveryRequests.Where(t => t.Status == DeliveryReqStatus.ScheduleCreated || t.Status == DeliveryReqStatus.Draft).ToList();
                var isAnyDSCreated = DSCreated != null && DSCreated.Any() && allDeliveryRequests.Any(t => t.TfxScheduleEnrouteStatus != (int)EnrouteDeliveryStatus.CompletedDrop
                                                   && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.Completed
                                                   && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.CompletedLate
                                                   && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.RescheduledCompleted
                                                   && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.Canceled
                                                   && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.RescheduledMissed
                                                   && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.Missed);

                if (isAnyDSCreated)
                {
                    var existingDs = DSCreated.Last();
                    LogManager.Logger.WriteDebug("DeliveryRequestRepository", "CreateDRUsingForecastingAutoDRSetting", $"Delivery schedule exist for DRId:{existingDs.Id.ToString()}");
                    await SetProcessedDipValue(updatedDipTest);
                    return;
                }
                // create a new dr for bal Quantity
                List<DeliveryRequestViewModel> drToCreate = new List<DeliveryRequestViewModel>();
                decimal averageLoadQty = 0;
                if (tank.FuelCapacity != null && tank.FuelCapacity.HasValue && tank.FuelCapacity.Value > 0)
                {
                    averageLoadQty = (tank.FuelCapacity.Value * forecastingSettings.AverageLoadQty) / 100;
                }
                DeliveryRequestViewModel dr = new DeliveryRequestViewModel()
                {
                    RequiredQuantity = forecastingSettings.InventoryPriorityType == 2 ? balanceRequiredQuantity : averageLoadQty,
                    CurrentQuantity = currentVolume,
                    CurrentThreshold = currentInventory,
                    ProductTypeId = tank.FuelTypeId,
                    ProductType = tank.ProductTypeName,
                    StorageId = tank.StorageId,
                    TankId = tank.TankId,
                    JobId = tank.JobId,
                    JobName = tank.JobName,
                    Priority = thisTankLevelPriority,
                    TankMaxFill = tankMaxFill,
                    SiteId = job.TfxDisplayJobId,
                    CreatedBy = (int)SystemUser.System,
                    CreatedOn = DateTimeOffset.Now,
                    UpdatedOn = DateTimeOffset.Now,
                    IsActive = true,
                    IsDeleted = false,
                    DelReqSource = forecastingSettings.InventoryPriorityType == 2 ? DRSource.Forecasting : DRSource.OttoForecasting,
                    AutoDRStatus = AutoDrStatus.Create,
                    AutoCreatedOn = DateTimeOffset.Now,
                    ScheduleQuantityType = (int)ScheduleQuantityType.Quantity,
                    ScheduleQuantityTypeText = ScheduleQuantityType.Quantity.GetDisplayName(),
                    DeliveryWindowInfo = deliveryWindowInfo
                };

                dr.AssignedToCompanyId = assignedToCompanyId;

                if (supplierCompanyId == 0)
                {
                    LogManager.Logger.WriteDebug("DeliveryRequestRepository", "CreateDRUsingForecastingAutoDRSetting", $"Supplier not found for jobId:{tank.JobId}, supplierCompanyId:{supplierCompanyId}, TfxProductTypeId:{tank.FuelTypeId}");
                    return;
                }

                await SetDRAdditionalDetails(dr, tank.JobId, supplierCompanyId, tank.FuelTypeId);
                dr.CreatedBy = dr.CreatedBy > 0 ? dr.CreatedBy : (int)SystemUser.System;
                drToCreate.Add(dr);
                var response = await CreateDeliveryRequest(drToCreate);

                DeliveryRequestAccess drExchangeAccess = new DeliveryRequestAccess();
                foreach (var drItem in response.EntityIds)
                {
                    var message = new TankDeliveryRequestMessageModel { EntityId = drItem };
                    var jsonMessage = new JavaScriptSerializer().Serialize(message);
                    var createdBy = drToCreate.FirstOrDefault().CreatedBy > 0 ? drToCreate.FirstOrDefault().CreatedBy : (int)SystemUser.System;
                    drExchangeAccess.AddNotificationEvent((int)EventType.TankDeliveryRequestCreated, createdBy, jsonMessage);
                }
                LogManager.Logger.WriteDebug("DeliveryRequestRepository", "CreateDRUsingForecastingAutoDRSetting", $"Automate DR Creation Status:{(Status)response.StatusCode} {response.StatusMessage}, for jobId:{tank.JobId}, supplierCompanyId:{supplierCompanyId}, StorageTYpeId:{tank.TankId}, storageId:{tank.StorageId}, Quantity:{balanceRequiredQuantity}");
            }
            await SetProcessedDipValue(updatedDipTest);
        }

        private void AddNotificationForTankHitRunOutLevel(TankDetailsModel tankDetails, decimal currentInventory, float ullage, int supplierCompanyId)
        {
            try
            {
                var message = new NotificationTankHitRunoutLevelModel
                {
                    Tank = tankDetails.TankName,
                    Location = tankDetails.JobName,
                    TankStorageId = tankDetails.StorageId,
                    CurrentInventory = currentInventory.GetPreciseValue(2).GetCommaSeperatedValue(),
                    Ullage = Convert.ToDecimal(ullage.GetPreciseValue(2)).GetCommaSeperatedValue(),
                    SupplierCompanyId = supplierCompanyId,
                };
                DeliveryRequestAccess drExchangeAccess = new DeliveryRequestAccess();
                var jsonMessage = new JavaScriptSerializer().Serialize(message);
                drExchangeAccess.AddNotificationEvent((int)EventType.TankHitRunOutLevel, (int)SystemUser.System, jsonMessage);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryRequestRepository", "AddNotificationForTankHitRunOutLevel", ex.Message, ex);
            }
        }

        public async Task<StatusModel> ProcessCarrierDeliveyForOttoAlerts()
        {
            var response = new StatusModel();
            try
            {
                var previousMonth = DateTime.Now.AddMonths(-1);
                var startDate = new DateTime(previousMonth.Year, previousMonth.Month, 1);
                var EndDate = new DateTime(previousMonth.Year, previousMonth.Month, DateTime.DaysInMonth(previousMonth.Year, previousMonth.Month), 23, 59, 59);
                var monthName = startDate.ToString("MMMM");

                //Actual
                var deliveryRequests_Actual = await mdbContext.DeliveryRequests.Find(t => (t.TfxScheduleStatus == (int)DeliveryScheduleStatus.Completed
                                || t.TfxScheduleStatus == (int)DeliveryScheduleStatus.CompletedLate) &&
                                t.IsActive && !t.IsDeleted && t.CreatedOn >= startDate && t.CreatedOn < EndDate).ToListAsync();

                List<CarrierExceedsDeliveryModel> carrierExceedsDelivery = new List<CarrierExceedsDeliveryModel>();
                Hashtable ht = new Hashtable();
                foreach (var item in deliveryRequests_Actual)
                {
                    var key = item.TfxAssignedToCompanyId + " " + item.TfxJobId + " " + item.TfxProductTypeId;
                    if (ht[key] == null)
                    {
                        ht.Add(key, item.TfxAssignedToCompanyId);
                        carrierExceedsDelivery.Add(new CarrierExceedsDeliveryModel()
                        {
                            TfxCarrierCompanyId = item.TfxAssignedToCompanyId,
                            TfxJobId = item.TfxJobId,
                            TfxProductId = item.TfxProductTypeId
                        });
                    }
                }

                foreach (var item in carrierExceedsDelivery)
                {
                    //Start of Otto
                    var deliveryRequests_Estimated = await mdbContext.DeliveryRequests.Find(t => t.DelReqSource == DRSource.OttoForecasting &&
                                (t.TfxScheduleStatus == (int)DeliveryScheduleStatus.Completed || t.TfxScheduleStatus == (int)DeliveryScheduleStatus.CompletedLate) &&
                               t.IsActive && !t.IsDeleted && t.CreatedOn >= startDate && t.CreatedOn <= EndDate
                               && t.TfxAssignedToCompanyId == item.TfxCarrierCompanyId && t.TfxJobId == item.TfxJobId
                               && t.TfxProductTypeId == item.TfxProductId).ToListAsync();

                    var estimatedQuantity = deliveryRequests_Estimated.Sum(t => t.RequiredQuantity);
                    var estimatedDelivery = deliveryRequests_Estimated.Count;
                    //End of Otto

                    //Start Actual
                    var actualValues = deliveryRequests_Actual.Where(t => t.TfxAssignedToCompanyId == item.TfxCarrierCompanyId
                            && t.TfxJobId == item.TfxJobId && t.TfxProductTypeId == item.TfxProductId).ToList();
                    var deliveryRequest_Actual = deliveryRequests_Estimated.FirstOrDefault();
                    var actualDelivery = actualValues.Count;
                    var actualQuantity = actualValues.Sum(t => t.RequiredQuantity);
                    //End Actual

                    if (deliveryRequest_Actual != null && actualDelivery > estimatedDelivery/*Otto*/ && estimatedDelivery > 0)
                    {
                        var message = new NotificationCarrierExceedsDeliveryModel
                        {
                            ActualDelivery = actualDelivery,
                            EstimatedDelivery = estimatedDelivery,
                            Product = deliveryRequest_Actual.TfxProductType,
                            Location = deliveryRequest_Actual.TfxJobName,
                            EstimatedQuantity = estimatedQuantity + " " + (UoM)deliveryRequest_Actual.TfxUoM,
                            ActualQuantity = actualQuantity + " " + (UoM)deliveryRequest_Actual.TfxUoM,
                            OrderId = deliveryRequest_Actual.TfxOrderId.Value,
                            CarrierCompanyId = deliveryRequest_Actual.TfxCreatedByCompanyId,
                            Month = monthName
                        };

                        DeliveryRequestAccess drExchangeAccess = new DeliveryRequestAccess();
                        var jsonMessage = new JavaScriptSerializer().Serialize(message);
                        drExchangeAccess.AddNotificationEvent((int)EventType.CarrierExceedsDelivery, (int)SystemUser.System, jsonMessage);
                    }
                }
                response.StatusCode = (int)Status.Success;
            }
            catch (Exception ex)
            {
                response.StatusCode = (int)Status.Failed;
                LogManager.Logger.WriteException("DeliveryRequestRepository", "ProcessCarrierDeliveyForOttoAlerts", ex.Message, ex);
            }
            return response;
        }

        private void UpdateRouteInformation(DeliveryRequest drToUpdate)
        {
            var routeInfo = (from routeItem in mdbContext.RouteInformations.AsQueryable()
                             where routeItem.IsActive && !routeItem.IsDeleted && routeItem.TfxJobs.Any(x => x.Id == drToUpdate.TfxJobId)
                             select new
                             {
                                 routeItem.Id,
                                 routeItem.Name,
                                 routeItem.TfxCompanyId,
                                 routeItem.TfxJobs,
                                 routeItem.RegionId
                             }).ToList();
            if (routeInfo.Count() > 0)
            {
                if (drToUpdate.TfxAssignedToCompanyId == drToUpdate.TfxCreatedByCompanyId)
                {
                    var routeData = routeInfo.Where(top => top.RegionId == drToUpdate.CreatedRegionId).FirstOrDefault();
                    if (routeData != null)
                    {
                        var jobInfo = routeData.TfxJobs.Where(top => top.Id == drToUpdate.TfxJobId).FirstOrDefault();
                        if (jobInfo != null)
                        {
                            RouteInfo routeInfoDetails = new RouteInfo();
                            routeInfoDetails.Id = routeData.Id.ToString();
                            routeInfoDetails.Name = routeData.Name;
                            routeInfoDetails.LocationSeqNo = jobInfo.SequenceNo;
                            drToUpdate.RouteInfo = routeInfoDetails;
                        }
                    }
                }
                else
                {
                    foreach (var routeInfoDetails in routeInfo)
                    {
                        var jobInfo = routeInfoDetails.TfxJobs.Where(top => top.Id == drToUpdate.TfxJobId).FirstOrDefault();
                        if (jobInfo != null)
                        {
                            RouteInfo routeInfoData = new RouteInfo();
                            routeInfoData.Id = routeInfoDetails.Id.ToString();
                            routeInfoData.Name = routeInfoDetails.Name;
                            routeInfoData.LocationSeqNo = jobInfo.SequenceNo;
                            drToUpdate.RouteInfo = routeInfoData;
                        }
                    }
                }
            }
        }

        private DeliveryReqPriority GetDeliveryRequestPriority(decimal currentInventory, decimal? minFill, int? fillType, decimal? runOutLevel, decimal? fuelCapacity, decimal? reOrderLevel)
        {
            //Make this Function Common which is present in Demand Capcture Domain
            var response = DeliveryReqPriority.None;
            try
            {
                decimal minFillPercent = 0;
                if (minFill.HasValue && fuelCapacity.HasValue && fillType == (int)FillType.UoM)
                {
                    minFillPercent = (minFill.Value * 100) / fuelCapacity.Value;
                }
                else if (minFill.HasValue && fillType == (int)FillType.Percent)
                {
                    minFillPercent = minFill.Value;
                }

                if (currentInventory <= minFillPercent)
                {
                    response = DeliveryReqPriority.MustGo;
                }
                else if (runOutLevel.HasValue && currentInventory <= runOutLevel)
                {
                    response = DeliveryReqPriority.ShouldGo;
                }
                else if (reOrderLevel.HasValue && currentInventory <= reOrderLevel)
                {
                    response = DeliveryReqPriority.CouldGo;
                }
            }
            catch (Exception ex)
            {
                Exchange.Logger.LogManager.Logger.WriteException("DeliveryRequestRepository", "GetDeliveryRequestPriority", ex.Message, ex);
            }
            return response;
        }

        private async Task SetProcessedDipValue(Demand updatedDipTest)
        {
            // set dip test processed
            updatedDipTest.IsProcessed = true;
            context.Entry(updatedDipTest).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        private async Task SetDRAdditionalDetails(DeliveryRequestViewModel dr, int tfxJobId, int supplierCompanyId, int tfxProductTypeId)
        {
            DeliveryRequestAccess drExchangeAccess = new DeliveryRequestAccess();
            var AdditionalData = await drExchangeAccess.GetAdditionalDeliveryRequestDetails(tfxJobId, supplierCompanyId, tfxProductTypeId);
            if (AdditionalData != null && AdditionalData.SupplierCompanyId.HasValue)
            {
                dr.JobAddress = AdditionalData.JobAddress;
                dr.JobCity = AdditionalData.JobCity;
                dr.JobName = AdditionalData.JobName;
                dr.UoM = AdditionalData.UoM;
                dr.SupplierCompanyId = AdditionalData.SupplierCompanyId;
                dr.CustomerCompany = AdditionalData.CustomerCompany;
                dr.CreatedByCompanyId = AdditionalData.SupplierCompanyId.Value; // need to discuss with sravanthi
                dr.CreatedBy = AdditionalData.CreatedBy;
                dr.UpdatedBy = AdditionalData.UpdatedBy;
                dr.JobTimeZoneOffset = AdditionalData.JobTimeZoneOffset;
            }
            else
            {
                LogManager.Logger.WriteException("DeliveryRequestRepository", "SetDRAdditionalDetails", $"Supplier not found for jobId:{tfxJobId}, supplierCompanyId:{supplierCompanyId}, TfxProductTypeId:{tfxProductTypeId}", new Exception());
            }
        }

        public async Task<StatusModel> UpdateDeliveryRequestStatus(List<DeliveryReqStatusUpdateModel> delReqs)
        {
            bool isSessionStarted = false;
            LogManager.Logger.WriteDebug("DeliveryRequestRepository", "UpdateDeliveryRequestStatus", JsonConvert.SerializeObject(delReqs));
            StatusModel status = new StatusModel();
            using (var session = await mdbContext.Client.StartSessionAsync())
            {
                try
                {
                    session.StartTransaction();
                    isSessionStarted = true;
                    foreach (var dr in delReqs)
                    {
                        var objectIdForDr = ObjectId.Parse(dr.DeliveryRequestId);
                        var existingDrs = await mdbContext.DeliveryRequests.Find(t => (t.Id == objectIdForDr || t.ParentId == dr.DeliveryRequestId) && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.Completed && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.CompletedLate && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.RescheduledCompleted && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.UnplannedDropCompleted && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.PreLoadBolCompleted).ToListAsync();
                        if (existingDrs != null && existingDrs.Any())
                        {
                            UpdateDefinition<DeliveryRequest> updateFields = null;
                            if (dr.ScheduleStatusId != 0)
                            {
                                updateFields = Builders<DeliveryRequest>.Update
                                    .Set(t => t.TfxScheduleStatus, dr.ScheduleStatusId)
                                    .Set(t => t.TfxScheduleStatusName, dr.ScheduleStatusName)
                                    .Set(t => t.UpdatedBy, dr.UserId);
                            }
                            else
                            {
                                updateFields = Builders<DeliveryRequest>.Update
                                   .Set(t => t.TfxScheduleEnrouteStatus, dr.ScheduleEnrouteStatusId)
                                   .Set(t => t.TfxScheduleStatusName, dr.ScheduleStatusName)
                                   .Set(t => t.UpdatedBy, dr.UserId);
                            }
                            var filter = Builders<DeliveryRequest>.Filter.And(
                                    Builders<DeliveryRequest>.Filter.Where(x => x.Id == objectIdForDr)
                                );

                            await mdbContext.DeliveryRequests.UpdateOneAsync(filter, updateFields);
                            LogManager.Logger.WriteDebug("DeliveryRequestRepository", "UpdateDeliveryRequestStatus", "updatedFor: " + objectIdForDr);

                            if (existingDrs.Any(t => t.ParentId == dr.DeliveryRequestId))
                            {
                                var missedDrfilter = Builders<DeliveryRequest>.Filter.And(Builders<DeliveryRequest>.Filter.Where(x => x.ParentId == dr.DeliveryRequestId));
                                var missedUpdateFields = updateFields.Set(t => t.IsActive, false).Set(t => t.IsDeleted, true);
                                await mdbContext.DeliveryRequests.UpdateOneAsync(missedDrfilter, missedUpdateFields);
                            }

                            if ((dr.OrderStatusId == (int)OrderStatus.Open || dr.OrderStatusId == 0) && (dr.ScheduleStatusId == (int)DeliveryScheduleStatus.Missed || dr.ScheduleStatusId == (int)DeliveryScheduleStatus.RescheduledMissed || dr.ScheduleStatusId == (int)DeliveryScheduleStatus.Diverted))
                            {
                                var existingDr = existingDrs.FirstOrDefault(t => t.Id == objectIdForDr);
                                if (existingDr != null)
                                {
                                    var newDeliveryRequest = existingDr.ToEntity();
                                    if ((existingDr.TfxTerminal == null || existingDr.TfxTerminal.Id == 0) && (existingDr.TfxBulkPlant == null || string.IsNullOrWhiteSpace(existingDr.TfxBulkPlant.SiteName)) && !string.IsNullOrWhiteSpace(existingDr.TripId) && !string.IsNullOrWhiteSpace(existingDr.ScheduleBuilderId))
                                    {
                                        var scheduleBuilderId = ObjectId.Parse(existingDr.ScheduleBuilderId);
                                        var tripId = ObjectId.Parse(existingDr.TripId);
                                        var dsb = mdbContext.ScheduleBuilders.Find(t => t.Id == scheduleBuilderId).Project(t => new { trip = t.Trips.Where(t1 => t1.TripId == tripId).FirstOrDefault() }).FirstOrDefault();
                                        if (dsb != null && dsb.trip != null)
                                        {
                                            if (dsb.trip.PickupLocationType == PickupLocationType.BulkPlant && dsb.trip.TfxBulkPlant != null)
                                            {
                                                newDeliveryRequest.TfxBulkPlant = dsb.trip.TfxBulkPlant;
                                            }
                                            else if (dsb.trip.PickupLocationType != PickupLocationType.BulkPlant && dsb.trip.TfxTerminal != null)
                                            {
                                                newDeliveryRequest.TfxTerminal = dsb.trip.TfxTerminal;
                                            }
                                        }
                                    }
                                    if (newDeliveryRequest.IsBlendedRequest)
                                    {
                                        var parentBlendDrIds = mdbContext.DeliveryRequests.Find(t => t.BlendedGroupId == existingDr.BlendedGroupId && t.IsActive && !t.IsDeleted).Project(t => t.Id.ToString()).ToList();
                                        var requestFilter = Builders<DeliveryRequest>.Filter.Where(x => parentBlendDrIds.Contains(x.ParentId));
                                        string blendGroupIdForMissedDr = mdbContext.DeliveryRequests.Find(requestFilter).Project(t => t.BlendedGroupId).FirstOrDefault();
                                        if (!string.IsNullOrWhiteSpace(blendGroupIdForMissedDr))
                                        {
                                            newDeliveryRequest.BlendedGroupId = blendGroupIdForMissedDr;
                                        }
                                        else
                                        {
                                            newDeliveryRequest.BlendedGroupId = Guid.NewGuid().ToString("n").Substring(0, 18);
                                        }
                                    }
                                    await mdbContext.DeliveryRequests.InsertOneAsync(newDeliveryRequest);
                                }
                            }
                            if (dr.ScheduleEnrouteStatusId == (int)EnrouteDeliveryStatus.ArrivedAtJob || dr.ScheduleStatusId == (int)DeliveryScheduleStatus.CompletedLate)
                            {
                                //update brokered parent status if dr is brokered by supplier/carrier.
                                await UpdateParentDeliveryCarrierStatus(dr.DeliveryRequestId);
                            }
                        }
                    }
                    await session.CommitTransactionAsync();
                }
                catch (Exception ex)
                {
                    if (isSessionStarted)
                    {
                        await session.AbortTransactionAsync();
                    }
                    status.StatusCode = (int)Status.Failed;
                    LogManager.Logger.WriteException("DeliveryRequestRepository", "UpdateDeliveryRequestStatus", JsonConvert.SerializeObject(delReqs), ex);
                    throw;
                }
            }
            status.StatusCode = (int)Status.Success;
            return status;
        }

        public async Task<StatusModel> UpdateDeliveryRequestStatusByTrackableScheduleId(List<DeliveryReqStatusUpdateModel> delReqs)
        {
            StatusModel status = new StatusModel();
            foreach (var dr in delReqs)
            {
                var existingDrs = await mdbContext.DeliveryRequests.Find(t => t.TfxTrackableScheduleId == dr.TrackableScheduleId && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.Completed && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.CompletedLate && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.RescheduledCompleted && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.UnplannedDropCompleted && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.PreLoadBolCompleted).ToListAsync();
                if (existingDrs != null && existingDrs.Any())
                {
                    UpdateDefinition<DeliveryRequest> updateFields = null;
                    updateFields = Builders<DeliveryRequest>.Update
                        .Set(t => t.TfxScheduleStatus, dr.ScheduleStatusId)
                        .Set(t => t.TfxScheduleStatusName, dr.ScheduleStatusName)
                        .Set(t => t.UpdatedBy, dr.UserId);

                    var filter = Builders<DeliveryRequest>.Filter.And(
                            Builders<DeliveryRequest>.Filter.Where(x => x.TfxTrackableScheduleId == dr.TrackableScheduleId)
                        );

                    await mdbContext.DeliveryRequests.UpdateManyAsync(filter, updateFields);
                }
            }
            status.StatusCode = (int)Status.Success;
            return status;

        }
        public async Task<StatusModel> UpdateDeliveryRequestCompartmentInfo(List<DeliveryRequestCompartmentInfoModel> delReqs)
        {
            StatusModel status = new StatusModel();
            foreach (var dr in delReqs)
            {
                var objectIdForDr = ObjectId.Parse(dr.DeliveryRequestId);
                var existingDrs = await mdbContext.DeliveryRequests.Find(t => t.Id == objectIdForDr || t.ParentId == dr.DeliveryRequestId).ToListAsync();
                if (existingDrs != null && existingDrs.Any())
                {
                    var compartmentInfo = new List<CompartmentsInfo>();
                    if (dr.Compartments != null && dr.Compartments.Count > 0)
                    {
                        compartmentInfo = dr.Compartments.ToCloneEntity();
                    }

                    UpdateDefinition<DeliveryRequest> updateFields = null;
                    updateFields = Builders<DeliveryRequest>.Update.Set(t => t.Compartments, compartmentInfo);

                    var filter = Builders<DeliveryRequest>.Filter.And(Builders<DeliveryRequest>.Filter.Where(x => x.Id == objectIdForDr));

                    await mdbContext.DeliveryRequests.UpdateOneAsync(filter, updateFields);
                }
            }
            status.StatusCode = (int)Status.Success;
            return status;

        }

        private void IntializeDemandsParameters(List<string> IdList, out DataTable Ids)
        {
            Ids = CreateDemandTable();
            foreach (var item in IdList.Distinct().ToList())
            {
                var row = Ids.NewRow();
                row["SearchVar"] = item;
                Ids.Rows.Add(row);
            }
        }


        public List<DemandCaptureChartViewModel> GetDemandCaptureChartData(string SiteId, int noOfDays, int tfxJobId, int companyId)
        {
            var response = new List<DemandCaptureChartViewModel>();
            DateTimeOffset startDate = DateTimeOffset.Now;
            DateTimeOffset endDate = DateTimeOffset.Now.AddDays(-noOfDays);
            if (!string.IsNullOrWhiteSpace(SiteId))
            {
                var tanksData = mdbContext.JobAdditionalDetails.Find(t => t.TfxJobId == tfxJobId && t.TfxDisplayJobId == SiteId)
                    .Project(t => t.Tanks.Select(t1 => new { t1.StorageId, t1.StorageTypeId })).FirstOrDefault();
                var storageIds = tanksData.Select(t => t.StorageId).ToList();
                var tankIds = tanksData.Select(t => t.StorageTypeId).ToList();

                if (storageIds != null && storageIds.Any())
                {

                    DataTable TankIds, StorageIds;
                    IntializeDemandsParameters(tankIds, out TankIds);
                    var tankIdsParam = new SqlParameter("@TankIds", SqlDbType.Structured);
                    tankIdsParam.Value = TankIds;
                    tankIdsParam.TypeName = "dbo.DemandsSearchTypes";

                    IntializeDemandsParameters(storageIds, out StorageIds);
                    var storageIdParams = new SqlParameter("@StorageIds", SqlDbType.Structured);
                    storageIdParams.Value = StorageIds;
                    storageIdParams.TypeName = "dbo.DemandsSearchTypes";

                    var siteIdParam = new SqlParameter("@SiteId", SqlDbType.NVarChar).Value = SiteId;
                    var companyIdParam = new SqlParameter("@CompanyId", SqlDbType.Int).Value = companyId;
                    var startDateParam = new SqlParameter("@StartDate", SqlDbType.DateTimeOffset).Value = startDate;
                    var endDateParam = new SqlParameter("@EndDate", SqlDbType.DateTimeOffset).Value = endDate;

                    context.Database.CommandTimeout = 30;
                    var data = context.Database.SqlQuery<DemandModel>("usp_GetDemandsBySiteIdTankIdsStorageIdsCaptureTime @CompanyId,@SiteId,@StartDate,@EndDate,@TankIds,@StorageIds", companyIdParam, startDateParam, endDateParam, siteIdParam, tankIdsParam, storageIdParams).ToList();


                    data.ForEach(dc => response.Add(new DemandCaptureChartViewModel() { TankId = dc.TankId, StorageId = dc.StorageId, CaptureTime = dc.CaptureTime.ToString(), NetVolume = dc.NetVolume, Ullage = dc.Ullage, ProductName = dc.ProductName }));
                    var storageId = response.Select(t => t.StorageId).ToList();
                    var storageTypeId = response.Select(t => t.TankId).ToList();
                    var jobs = mdbContext.JobAdditionalDetails.Find(t => t.TfxDisplayJobId == SiteId && t.Tanks.Any(t1 => storageId.Contains(t1.StorageId) && storageTypeId.Contains(t1.StorageTypeId))).Project(t => new { t.TfxDisplayJobId, t.Tanks }).ToList();
                    var tanks = jobs.SelectMany(t => t.Tanks).ToList();
                    foreach (var tank in response)
                    {
                        tank.TankName = tanks.Where(t => t.StorageId == tank.StorageId && t.StorageTypeId == tank.TankId).Select(t => t.TankName).FirstOrDefault();
                    }
                }
            }
            return response;
        }

        public List<DeliveryRequestDetail> GetDeliveryRequestDetailsByIds(List<string> DrIds)
        {
            var response = new List<DeliveryRequestDetail>();
            try
            {
                var DrObjectIds = DrIds.Select(t => ObjectId.Parse(t)).ToList();
                var list = from request in mdbContext.DeliveryRequests.AsQueryable()
                           join region in mdbContext.Regions.AsQueryable()
                           on request.CreatedRegionId equals region.Id into requestRegions
                           where DrObjectIds.Contains(request.Id)
                           select new
                           {
                               request.Id,
                               request.Priority,
                               RegionId = requestRegions.First().Id,
                               RegionName = requestRegions.First().Name,
                               States = requestRegions.First().TfxStates
                           };
                var listItems = list.ToList();
                foreach (var item in listItems)
                {
                    var dr = new DeliveryRequestDetail();
                    dr.Id = item.Id.ToString();
                    dr.LdPri = (int)item.Priority;
                    dr.RgId = item.RegionId.ToString();
                    dr.RgName = item.RegionName;
                    dr.States = item.States.Select(t => new FreightModels.DropdownDisplayItem
                    {
                        Id = t.Id,
                        Code = t.Code,
                        Name = t.Name
                    }).ToList();
                    response.Add(dr);
                }
            }
            catch (Exception ex)
            {
                Exchange.Logger.LogManager.Logger.WriteException("DeliveryRequestRepository", "GetDeliveryRequestDetailsByIds", ex.Message, ex);
            }
            return response;
        }

        public DeliveryRequestViewModel GetDeliveryRequestDetailsById(string deliveryRequestId)
        {
            var response = new DeliveryRequestViewModel();
            try
            {
                ObjectId objDeliveryReqId = ObjectId.Empty;
                ObjectId.TryParse(deliveryRequestId, out objDeliveryReqId);
                var deliveryRequest = mdbContext.DeliveryRequests.Find(x => x.Id == objDeliveryReqId).FirstOrDefault();

                response = deliveryRequest.ToDeliveryRequestViewModel();
            }
            catch (Exception ex)
            {
                Exchange.Logger.LogManager.Logger.WriteException("DeliveryRequestRepository", "GetDeliveryRequestDetailsById", ex.Message, ex);
            }
            return response;
        }

        public List<DipatchersRegionDetails> GetRegionDispactherDetails(int driverId, int companyId, string regionId)
        {
            List<DipatchersRegionDetails> dipatchersRegionDetails = new List<DipatchersRegionDetails>();
            if (string.IsNullOrEmpty(regionId))
            {
                var regionDetails = mdbContext.Regions.Find(region => region.TfxCompanyId == companyId && region.IsActive && region.IsDeleted == false && region.TfxDrivers.Any(top => top.Id == driverId)).Project(x => new { RegionID = x.Id.ToString(), RegionName = x.Name, RegionDescription = x.Description, dispactherIds = x.TfxDispatchers }).ToList();

                foreach (var item in regionDetails)
                {
                    dipatchersRegionDetails.Add(new DipatchersRegionDetails { dispactherIds = item.dispactherIds.Select(x => x.Id).ToList(), RegionDescription = item.RegionDescription, RegionName = item.RegionName, RegionID = item.RegionID });
                }
            }
            else
            {
                ObjectId objectId = new ObjectId(regionId);
                var regionDetails = mdbContext.Regions.Find(region => region.Id == objectId && region.TfxCompanyId == companyId && region.IsActive && region.IsDeleted == false && region.TfxDrivers.Any(top => top.Id == driverId)).Project(x => new { RegionID = x.Id.ToString(), RegionName = x.Name, RegionDescription = x.Description, dispactherIds = x.TfxDispatchers }).ToList();

                foreach (var item in regionDetails)
                {
                    dipatchersRegionDetails.Add(new DipatchersRegionDetails { dispactherIds = item.dispactherIds.Select(x => x.Id).ToList(), RegionDescription = item.RegionDescription, RegionName = item.RegionName, RegionID = item.RegionID });
                }
            }
            return dipatchersRegionDetails;
        }

        public async Task<List<DeliveryRequestViewModel>> CloneDrsForPreload(List<string> drIds)
        {
            var clonedDrs = new List<DeliveryRequestViewModel>();
            var drObjectIds = drIds.Select(t => ObjectId.Parse(t)).ToList();
            var filter = Builders<DeliveryRequest>.Filter.In(t => t.Id, drObjectIds);
            var drsForPreload = await mdbContext.DeliveryRequests.FindAsync(filter);
            var drsForPreloadList = drsForPreload.ToList();
            var entities = drsForPreloadList.Where(t => !t.IsBlendedRequest).Select(t => t.CloneEntity()).ToList();
            foreach (var blendDr in drsForPreloadList.Where(t => t.IsBlendedRequest).GroupBy(t => t.BlendedGroupId))
            {
                var blendDrs = blendDr.Select(t => t.CloneEntity()).ToList();
                var blendGroupId = Guid.NewGuid().ToString("n").Substring(0, 18);
                blendDrs.ForEach(t => t.BlendedGroupId = blendGroupId);
                entities.AddRange(blendDrs);
            }
            for (int idx = 0; idx < entities.Count; idx++)
            {
                entities[idx].PostLoadedFor = drsForPreloadList[idx].Id;
                entities[idx].PreLoadedFor = null;
            }
            await mdbContext.DeliveryRequests.InsertManyAsync(entities);
            var drModels = entities.Select(t => t.ToDeliveryRequestViewModel()).ToList();
            clonedDrs.GetBlendDRInfo(drModels, false);

            entities.ForEach(postloaded =>
            {
                var preloadedId = drsForPreloadList.Where(x => x.Id == postloaded.PostLoadedFor).Select(x => x.Id).FirstOrDefault();
                var updateFields = Builders<DeliveryRequest>.Update.Set(x => x.PreLoadedFor, postloaded.Id);
                var updateFilter = Builders<DeliveryRequest>.Filter.And(
                            Builders<DeliveryRequest>.Filter.Where(x => x.Id == preloadedId)
                        );
                mdbContext.DeliveryRequests.UpdateOne(updateFilter, updateFields);
            });
            return clonedDrs;
        }

        public async Task<StatusModel> CreateRecurringSchedules(List<DeliveryRequestViewModel> model)
        {
            var response = new StatusModel();
            using (var session = await mdbContext.Client.StartSessionAsync())
            {
                bool isSessionStarted = false;
                try
                {
                    List<ObjectId> deleteRecurringScheduleId = new List<ObjectId>();
                    Dictionary<ObjectId, decimal> updateRecurringScheduleId = new Dictionary<ObjectId, decimal>();
                    var requestsWithoutRegion = model.Where(t => t.CreatedByRegionId == null || t.CreatedByRegionId == "").Select(t => t).ToList();
                    bool isValidRegion = true;
                    if (model.Any(t => t.OrderId == null))
                    {
                        isValidRegion = ValidateRegion(requestsWithoutRegion, isValidRegion);
                        if (!isValidRegion)
                        {
                            response.StatusCode = (int)Status.Failed;
                            response.StatusMessage = Resource.valMessageJobNotAssignedToRegionRecurring;
                            return response;
                        }
                    }
                    //validate the recurring schdules details.
                    ValidateRecurringScheduleInfo(model, deleteRecurringScheduleId, updateRecurringScheduleId);
                    session.StartTransaction();
                    isSessionStarted = true;
                    //delete recurring schdules details if any changes in recurring schedule ..
                    await DeleteRecurringSchedules(model, deleteRecurringScheduleId);
                    //update recurring schedules details if schedule quantity type = quantity and quantity change detection found.
                    await UpdateRecurringSchedules(model, updateRecurringScheduleId);
                    AssignRouteInformation(model);
                    //create recurring schedules and delivery requests for that recurring schedules first time.
                    List<RecurringSchedules> recurringSchedules = await CreateRecurringDeliveryRequests(model);
                    await session.CommitTransactionAsync();
                    var IsCarrierCompany = recurringSchedules.Any(x => x.DeliveryRequests.Any(top => top.TfxAssignedToCompanyId != top.TfxCreatedByCompanyId && top.TfxAssignedToCompanyId != top.TfxSupplierCompanyId));
                    response.StatusCode = (int)Status.Success;
                    response.StatusMessage = IsCarrierCompany ? Resource.msgDelReqrCreationSuccessForCarrierRecurring : Resource.msgDelReqCreationSuccessRecurring;
                    return response;
                }
                catch (Exception)
                {
                    if (isSessionStarted)
                    {
                        await session.AbortTransactionAsync();
                    }
                    throw;
                }
            }
        }

        private void ValidateRecurringScheduleInfo(List<DeliveryRequestViewModel> model, List<ObjectId> deleteRecurringScheduleId, Dictionary<ObjectId, decimal> updateRecurringScheduleId)
        {
            foreach (var item in model)
            {
                foreach (var recurringSchdule in item.RecurringSchdule)
                {
                    if (!string.IsNullOrEmpty(recurringSchdule.Id))
                    {
                        ObjectId scheduleId = ObjectId.Empty;
                        ObjectId.TryParse(recurringSchdule.Id, out scheduleId);
                        var existingScheduleDetails = mdbContext.RecurringSchedules.Find(t => t.Id == scheduleId).FirstOrDefault();
                        if (existingScheduleDetails != null)
                        {
                            //check changes
                            bool needToupdateRecord = false;
                            needToupdateRecord = ValidateRecurringScheduleForChange(recurringSchdule, existingScheduleDetails, needToupdateRecord);
                            if (needToupdateRecord)
                            {
                                deleteRecurringScheduleId.Add(scheduleId);
                            }
                            //update the record if quantity change with same ScheduleQuantityType.
                            else if (recurringSchdule.ScheduleQuantityType == recurringSchdule.ScheduleQuantityType && recurringSchdule.RequiredQuantity != existingScheduleDetails.RequiredQuantity)
                            {
                                //need to update the quantity existing schedule.
                                updateRecurringScheduleId.Add(scheduleId, recurringSchdule.RequiredQuantity);
                                recurringSchdule.isIgnoreRecord = true;
                            }
                            else
                            {
                                recurringSchdule.isIgnoreRecord = true;
                            }
                        }
                    }
                    else
                    {
                        bool validateEmptyRecord = false;
                        validateEmptyRecord = ValidateEmptyRecurringSchedule(recurringSchdule, validateEmptyRecord);
                        if (validateEmptyRecord)
                        {
                            recurringSchdule.isIgnoreRecord = true;
                        }
                    }
                }
            }
        }

        private async Task UpdateRecurringSchedules(List<DeliveryRequestViewModel> model, Dictionary<ObjectId, decimal> updateRecurringScheduleId)
        {
            //update exisiting recurring schedule quantity due to quantity changes in original model.
            if (updateRecurringScheduleId.Count > 0)
            {
                foreach (var Keyitem in updateRecurringScheduleId)
                {
                    await UpdateRecurringScheduleInfo(model, Keyitem);
                    await UpdateDeliveryScheduleInfo(model, Keyitem);
                }
            }
        }

        private async Task UpdateRecurringScheduleInfo(List<DeliveryRequestViewModel> model, KeyValuePair<ObjectId, decimal> Keyitem)
        {
            var scheduleFilter = Builders<RecurringSchedules>.Filter.And(
                                 Builders<RecurringSchedules>.Filter.Where(x => Keyitem.Key == x.Id));
            UpdateDefinition<RecurringSchedules> updateFields = null;
            updateFields = Builders<RecurringSchedules>.Update
                    .Set(t => t.RequiredQuantity, Keyitem.Value)
                    .Set(t => t.UpdatedBy, model.FirstOrDefault()?.CreatedBy)
                    .Set(t => t.UpdatedOn, DateTime.Now);
            await mdbContext.RecurringSchedules.UpdateOneAsync(scheduleFilter, updateFields);
        }
        private async Task UpdateDeliveryScheduleInfo(List<DeliveryRequestViewModel> model, KeyValuePair<ObjectId, decimal> Keyitem)
        {
            var scheduleFilter = Builders<DeliveryRequest>.Filter.And(
                                 Builders<DeliveryRequest>.Filter.Where(x => Keyitem.Key == x.RecurringScheduleId));
            Builders<DeliveryRequest>.Filter.Where(t => t.IsActive && !t.IsDeleted);
            Builders<DeliveryRequest>.Filter.Where(t => t.Status == DeliveryReqStatus.None || t.Status == DeliveryReqStatus.Pending || t.Status == DeliveryReqStatus.Assigned);
            UpdateDefinition<DeliveryRequest> updateFields = null;
            updateFields = Builders<DeliveryRequest>.Update
                    .Set(t => t.RequiredQuantity, Keyitem.Value)
                    .Set(t => t.UpdatedBy, model.FirstOrDefault()?.CreatedBy)
                    .Set(t => t.UpdatedOn, DateTime.Now);
            await mdbContext.DeliveryRequests.UpdateOneAsync(scheduleFilter, updateFields);
        }
        private async Task DeleteRecurringSchedules(List<DeliveryRequestViewModel> model, List<ObjectId> deleteRecurringScheduleId)
        {

            if (deleteRecurringScheduleId.Count > 0)
            {
                //delete exisiting recurring schedule due to change.
                await DeleteRecurringScheduleInfo(model, deleteRecurringScheduleId);

                //delete the delivery requests that associated that schedules.
                await DeleteDeliveryRequestsInfo(model, deleteRecurringScheduleId);
            }
        }

        private async Task DeleteRecurringScheduleInfo(List<DeliveryRequestViewModel> model, List<ObjectId> deleteRecurringScheduleId)
        {

            //delete recurring schedules
            var scheduleFilter = Builders<RecurringSchedules>.Filter.And(
                 Builders<RecurringSchedules>.Filter.Where(x => deleteRecurringScheduleId.Contains(x.Id)));
            UpdateDefinition<RecurringSchedules> updateFields = null;
            updateFields = Builders<RecurringSchedules>.Update
                    .Set(t => t.IsActive, false)
                    .Set(t => t.IsDeleted, true)
                    .Set(t => t.UpdatedBy, model.FirstOrDefault()?.CreatedBy)
                    .Set(t => t.UpdatedOn, DateTime.Now);
            await mdbContext.RecurringSchedules.UpdateManyAsync(scheduleFilter, updateFields);
        }

        private async Task DeleteDeliveryRequestsInfo(List<DeliveryRequestViewModel> model, List<ObjectId> deleteRecurringScheduleId)
        {
            var deliveryReqFilter = Builders<DeliveryRequest>.Filter.And(
                                    Builders<DeliveryRequest>.Filter.Where(x => deleteRecurringScheduleId.Contains(x.RecurringScheduleId)));
            Builders<DeliveryRequest>.Filter.Where(t => t.IsActive && !t.IsDeleted);
            Builders<DeliveryRequest>.Filter.Where(t => t.Status == DeliveryReqStatus.None || t.Status == DeliveryReqStatus.Pending || t.Status == DeliveryReqStatus.Assigned);
            var deliveryRequests = mdbContext.DeliveryRequests.Find(deliveryReqFilter).ToList();
            if (deliveryRequests.Count == 1)
            {
                UpdateDefinition<DeliveryRequest> updateDeliveryFields = null;
                updateDeliveryFields = Builders<DeliveryRequest>.Update
                        .Set(t => t.IsActive, false)
                        .Set(t => t.IsDeleted, true)
                        .Set(t => t.UpdatedBy, model.FirstOrDefault()?.CreatedBy)
                        .Set(t => t.UpdatedOn, DateTime.Now);
                await mdbContext.DeliveryRequests.UpdateManyAsync(deliveryReqFilter, updateDeliveryFields);
            }
        }

        private async Task<List<RecurringSchedules>> CreateRecurringDeliveryRequests(List<DeliveryRequestViewModel> model)
        {
            //insert data into recurring schedules
            var recurringSchedules = model.ToRecurringDREntity();
            if (recurringSchedules.Count > 0)
            {
                await CreateNewRecurringSchedules(recurringSchedules);
                await CreateDeliveryRequestIfDeleted(recurringSchedules);
            }

            return recurringSchedules;
        }

        private async Task CreateDeliveryRequestIfDeleted(List<RecurringSchedules> recurringSchedules)
        {
            var recurringItemsChanges = recurringSchedules.Where(top => top.isIgnoreRecord == true).ToList();
            if (recurringItemsChanges.Count > 0)
            {
                foreach (var rcItem in recurringItemsChanges)
                {
                    var deliveryRequestExists = await mdbContext.DeliveryRequests.Find(top => top.RecurringScheduleId == rcItem.Id && top.IsActive && !top.IsDeleted).FirstOrDefaultAsync();
                    if (deliveryRequestExists == null)
                    {
                        rcItem.DeliveryRequests.ForEach(t => t.DeliveryRequestFor = rcItem.DeliveryRequestFor);
                        var deliveryRequests = rcItem.DeliveryRequests.ToEntity();
                        foreach (var drItem in deliveryRequests)
                        {
                            drItem.DeliveryLevelPO = rcItem.DeliveryLevelPO;
                            drItem.CreatedBy = rcItem.CreatedBy;
                            drItem.UpdatedBy = rcItem.UpdatedBy;
                            drItem.IsRecurringSchedule = true;
                            drItem.RecurringScheduleId = rcItem.Id;
                            drItem.ScheduleQuantityType = rcItem.ScheduleQuantityType;
                        }
                        await mdbContext.DeliveryRequests.InsertManyAsync(deliveryRequests);
                    }
                }
            }
        }

        private async Task CreateNewRecurringSchedules(List<RecurringSchedules> recurringSchedules)
        {
            var recurringItems = recurringSchedules.Where(top => top.isIgnoreRecord == false && top.IsBlendedRequest == false).ToList();
            var recurringBlendedRequest = recurringSchedules.Where(top => top.isIgnoreRecord == false && top.IsBlendedRequest == true).ToList();
            if (recurringItems.Count > 0)
            {
                foreach (var rcItem in recurringItems)
                {
                    rcItem.Id = ObjectId.GenerateNewId();
                    await mdbContext.RecurringSchedules.InsertOneAsync(rcItem);
                    rcItem.DeliveryRequests.ForEach(t => t.DeliveryRequestFor = rcItem.DeliveryRequestFor);
                    var deliveryRequests = rcItem.DeliveryRequests.ToEntity();
                    foreach (var drItem in deliveryRequests)
                    {
                        drItem.DeliveryLevelPO = rcItem.DeliveryLevelPO;
                        drItem.CreatedBy = rcItem.CreatedBy;
                        drItem.UpdatedBy = rcItem.UpdatedBy;
                        drItem.IsRecurringSchedule = true;
                        drItem.RecurringScheduleId = rcItem.Id;
                        drItem.ScheduleQuantityType = rcItem.ScheduleQuantityType;
                    }
                    await mdbContext.DeliveryRequests.InsertManyAsync(deliveryRequests);
                }
            }
            if (recurringBlendedRequest.Count > 0)
            {
                var blendedGroupIds = recurringBlendedRequest.Where(top => top.isIgnoreRecord == false && top.IsBlendedRequest == true).GroupBy(x => new { x.BlendedGroupId, x.RecurringBlendedGroupId }).Select(x => x.Key).ToList();
                foreach (var item in blendedGroupIds)
                {
                    var recurringblendedDetails = recurringBlendedRequest.Where(top => top.BlendedGroupId == item.BlendedGroupId && top.RecurringBlendedGroupId == item.RecurringBlendedGroupId).FirstOrDefault();
                    if (recurringblendedDetails != null)
                    {
                        var recurringblendedDRDetails = recurringBlendedRequest.Where(top => top.BlendedGroupId == item.BlendedGroupId && top.RecurringBlendedGroupId == item.RecurringBlendedGroupId).SelectMany(x => x.DeliveryRequests).ToList();
                        var recurringBlendedGUID = "rec_" + CorrelationIdGenerator.GetNextId();
                        recurringblendedDRDetails.ForEach(x => x.BlendedGroupId = recurringBlendedGUID);
                        recurringblendedDetails.Id = ObjectId.GenerateNewId();
                        recurringblendedDetails.BlendedGroupId = recurringBlendedGUID;
                        recurringblendedDetails.RequiredQuantity = recurringblendedDRDetails.Sum(x => x.RequiredQuantity);
                        recurringblendedDetails.DeliveryRequests = recurringblendedDRDetails;
                        await mdbContext.RecurringSchedules.InsertOneAsync(recurringblendedDetails);
                        recurringblendedDetails.DeliveryRequests.ForEach(t => t.DeliveryRequestFor = recurringblendedDetails.DeliveryRequestFor);
                        var deliveryRequests = recurringblendedDetails.DeliveryRequests.ToEntity();
                        foreach (var drItem in deliveryRequests)
                        {
                            drItem.CreatedBy = recurringblendedDetails.CreatedBy;
                            drItem.UpdatedBy = recurringblendedDetails.UpdatedBy;
                            drItem.IsRecurringSchedule = true;
                            drItem.RecurringScheduleId = recurringblendedDetails.Id;
                            drItem.ScheduleQuantityType = recurringblendedDetails.ScheduleQuantityType;
                        }
                        await mdbContext.DeliveryRequests.InsertManyAsync(deliveryRequests);
                    }
                }
            }
        }

        private static bool ValidateRecurringScheduleForChange(RecurringDRSchdule recurringSchdule, RecurringSchedules existingScheduleDetails, bool needToupdateRecord)
        {
            if (existingScheduleDetails.ScheduleType != recurringSchdule.ScheduleType)
            {
                needToupdateRecord = true;
            }
            else if (existingScheduleDetails.ScheduleQuantityType != recurringSchdule.ScheduleQuantityType)
            {
                needToupdateRecord = true;
            }
            else
            {
                if (existingScheduleDetails.ScheduleType == (int)ScheduleTypes.Weekly || existingScheduleDetails.ScheduleType == (int)ScheduleTypes.BiWeekly)
                {
                    if (existingScheduleDetails.WeekDayId.Count() != recurringSchdule.WeekDayId.Count())
                    {
                        needToupdateRecord = true;
                    }
                    else
                    {
                        var weekDayChanges = existingScheduleDetails.WeekDayId.SequenceEqual(recurringSchdule.WeekDayId);
                        needToupdateRecord = !weekDayChanges;
                    }
                }
                else
                {
                    if (existingScheduleDetails.MonthDayId != recurringSchdule.MonthDayId)
                    {
                        needToupdateRecord = true;
                    }
                }
            }

            return needToupdateRecord;
        }
        private static bool ValidateEmptyRecurringSchedule(RecurringDRSchdule recurringSchdule, bool validateEmptyRecord)
        {
            if (recurringSchdule.ScheduleType == (int)ScheduleTypes.Weekly || recurringSchdule.ScheduleType == (int)ScheduleTypes.BiWeekly)
            {
                if (recurringSchdule.WeekDayId == null || recurringSchdule.WeekDayId.Count() == 0)
                {
                    validateEmptyRecord = true;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(recurringSchdule.Date))
                {
                    validateEmptyRecord = true;
                }
            }

            return validateEmptyRecord;
        }
        public List<RecurringDRSchdule> GetRecurringSchedule(int JobId)
        {
            List<RecurringDRSchdule> recurringSchdules = new List<RecurringDRSchdule>();
            var recurringScheduleDetails = mdbContext.RecurringSchedules.Find(top => top.JobId == JobId && top.IsActive == true && top.IsDeleted == false)
                            .Project(t => new RecurringSchedules
                            {
                                Id = t.Id,
                                ScheduleType = t.ScheduleType,
                                WeekDayId = t.WeekDayId,
                                MonthDayId = t.MonthDayId,
                                Date = t.Date,
                                ScheduleQuantityType = t.ScheduleQuantityType,
                                OrderId = t.OrderId,
                                SiteId = t.SiteId,
                                JobId = t.JobId,
                                TfxSupplierCompanyId = t.TfxSupplierCompanyId,
                                TfxCompanyName = t.TfxCompanyName,
                                TfxUserId = t.TfxUserId,
                                AssignedToCompanyId = t.AssignedToCompanyId,
                                RequiredQuantity = t.RequiredQuantity,
                                BuyerCompanyId = t.BuyerCompanyId,
                                PoNumber = t.PoNumber,
                                TankName = t.TankName,
                                AssetId = t.AssetId,
                                ProductTypeId = t.ProductTypeId,
                                IsBlendedRequest = t.IsBlendedRequest,
                                DeliveryLevelPO = t.DeliveryLevelPO,

                            }).ToList();
            foreach (var item in recurringScheduleDetails)
            {
                var recurringItem = item.ToRecurringEntity();
                recurringSchdules.Add(recurringItem);
            }
            return recurringSchdules;
        }
        public async Task<StatusModel> DeleteRecurringSchedule(string Id, int userId)
        {
            StatusModel status = new StatusModel();
            using (var session = await mdbContext.Client.StartSessionAsync())
            {
                session.StartTransaction();
                try
                {
                    var objectIdForDr = ObjectId.Parse(Id);
                    var existingDrs = await mdbContext.RecurringSchedules.Find(t => t.Id == objectIdForDr).FirstOrDefaultAsync();
                    if (existingDrs != null)
                    {
                        await DeleteRecurringSchedulesInfo(userId, objectIdForDr);

                        await DeleteDeliveryRequestsInfo(userId, objectIdForDr);
                    }
                    status.StatusCode = (int)Status.Success;
                    status.StatusMessage = Resource.msgDelReqDeleteSuccessRecurring;
                    await session.CommitTransactionAsync();
                    return status;
                }
                catch (Exception)
                {
                    status.StatusCode = (int)Status.Failed;
                    await session.AbortTransactionAsync();
                    throw;
                }
            }
        }

        private async Task DeleteRecurringSchedulesInfo(int userId, ObjectId objectIdForDr)
        {
            UpdateDefinition<RecurringSchedules> updateFields = null;
            updateFields = Builders<RecurringSchedules>.Update
                    .Set(t => t.IsActive, false)
                    .Set(t => t.IsDeleted, true)
                    .Set(t => t.UpdatedBy, userId)
                    .Set(t => t.UpdatedOn, DateTime.Now);
            var filter = Builders<RecurringSchedules>.Filter.And(
                Builders<RecurringSchedules>.Filter.Where(x => x.Id == objectIdForDr));
            await mdbContext.RecurringSchedules.UpdateOneAsync(filter, updateFields);
        }

        private async Task DeleteDeliveryRequestsInfo(int userId, ObjectId deleteRecurringScheduleId)
        {
            var deliveryReqFilter = Builders<DeliveryRequest>.Filter.And(
                                    Builders<DeliveryRequest>.Filter.Where(x => x.RecurringScheduleId == deleteRecurringScheduleId));
            Builders<DeliveryRequest>.Filter.Where(t => t.IsActive && !t.IsDeleted);
            Builders<DeliveryRequest>.Filter.Where(t => t.Status == DeliveryReqStatus.None || t.Status == DeliveryReqStatus.Pending || t.Status == DeliveryReqStatus.Assigned);
            var deliveryRequests = mdbContext.DeliveryRequests.Find(deliveryReqFilter).ToList();
            if (deliveryRequests.Count == 1)
            {
                UpdateDefinition<DeliveryRequest> updateDeliveryFields = null;
                updateDeliveryFields = Builders<DeliveryRequest>.Update
                        .Set(t => t.IsActive, false)
                        .Set(t => t.IsDeleted, true)
                        .Set(t => t.UpdatedBy, userId)
                        .Set(t => t.UpdatedOn, DateTime.Now);
                await mdbContext.DeliveryRequests.UpdateManyAsync(deliveryReqFilter, updateDeliveryFields);
            }
        }
        private void AssignRouteInformation(List<DeliveryRequestViewModel> deliveryRequests)
        {
            var JobIds = deliveryRequests.Select(top => top.JobId).ToList();
            var routeFilter = Builders<RouteInformations>.Filter.And(
                     Builders<RouteInformations>.Filter.Where(t => (t.IsActive && !t.IsDeleted && t.TfxJobs.Any(x => JobIds.Contains(x.Id)))));
            var routeInfoDetails = mdbContext.RouteInformations.Find(routeFilter).ToList();
            if (routeInfoDetails.Count > 0)
            {
                foreach (var deliveryitem in deliveryRequests)
                {
                    if (deliveryitem.AssignedToCompanyId == deliveryitem.CreatedByCompanyId)
                    {
                        var routeInfo = routeInfoDetails.FirstOrDefault(top => top.RegionId == ObjectId.Parse(deliveryitem.CreatedByRegionId) && top.TfxCompanyId == deliveryitem.CreatedByCompanyId && top.TfxJobs.Any(x => x.Id == deliveryitem.JobId));
                        if (routeInfo != null)
                        {
                            int seqNo = 0;
                            var seqInfo = routeInfo.TfxJobs.FirstOrDefault(top => top.Id == deliveryitem.JobId);
                            if (seqInfo != null)
                            {
                                seqNo = seqInfo.SequenceNo;
                            }

                            RouteInfo routeObj = new RouteInfo();
                            routeObj.Name = routeInfo.Name;
                            routeObj.Id = routeInfo.Id.ToString();
                            routeObj.LocationSeqNo = seqNo;
                            deliveryitem.RouteInfo = routeObj;
                        }
                    }
                    else
                    {
                        var routeInfo = routeInfoDetails.FirstOrDefault(top => top.TfxCompanyId == deliveryitem.AssignedToCompanyId && top.TfxJobs.Any(x => x.Id == deliveryitem.JobId));
                        if (routeInfo != null)
                        {
                            int seqNo = 0;
                            var seqInfo = routeInfo.TfxJobs.FirstOrDefault(top => top.Id == deliveryitem.JobId);
                            if (seqInfo != null)
                            {
                                seqNo = seqInfo.SequenceNo;
                            }

                            RouteInfo routeObj = new RouteInfo();
                            routeObj.Name = routeInfo.Name;
                            routeObj.Id = routeInfo.Id.ToString();
                            routeObj.LocationSeqNo = seqNo;
                            deliveryitem.RouteInfo = routeObj;
                        }
                    }

                }
            }
        }

        public async Task<List<DeliveryRequestViewModel>> CreateDrForRecurringSchedule(List<DeliveryRequestViewModel> deliveryRequest)
        {
            var response = new List<DeliveryRequestViewModel>();
            using (var session = await mdbContext.Client.StartSessionAsync())
            {
                session.StartTransaction();
                try
                {
                    var delReq = new List<DeliveryRequest>();
                    deliveryRequest.ForEach(x => delReq.Add(x.CloneRECEntity()));
                    await mdbContext.DeliveryRequests.InsertManyAsync(delReq);
                    delReq.ForEach(x => response.Add(x.ToDeliveryRequestViewModel()));
                    await session.CommitTransactionAsync();
                }
                catch (Exception)
                {
                    await session.AbortTransactionAsync();
                    throw;
                }
            }
            return response;
        }
        private List<DeliveryRequest> GetRecurringDetails(List<string> delReqIds)
        {
            List<ObjectId> objectIds = new List<ObjectId>();
            delReqIds.ForEach(x => objectIds.Add(ObjectId.Parse(x)));
            return mdbContext.DeliveryRequests.Find(top => objectIds.Contains(top.Id) && top.IsRecurringSchedule).ToList();
        }

        public async Task<StatusModel> ChangeBrokeredDrStatus(string ChildDrId, string ParentDrId, BrokeredDrCarrierStatus status)
        {
            var response = new StatusModel();
            await UpdateDRStatusAsync(ChildDrId, status);
            await UpdateDRStatusAsync(ParentDrId, status);
            if (status == BrokeredDrCarrierStatus.Accepted)
            {
                var objDrId = !string.IsNullOrEmpty(ChildDrId) ? ObjectId.Parse(ChildDrId) : ObjectId.Empty;
                await AutoAssignJobToBrokeredDRJobs(objDrId);
            }
            response.StatusCode = (int)Status.Success;
            response.StatusMessage = $"Delivery request {status.ToString()} successfully";
            return response;
        }

        private async Task AutoAssignJobToBrokeredDRJobs(ObjectId objDrId)
        {
            var deliveryRequestDetails = await mdbContext.DeliveryRequests.Find(top => top.Id == objDrId).Project(t => new { t.TfxAssignedToCompanyId, t.TfxJobId, t.TfxSupplierCompanyId, t.TfxCustomerCompany, t.TfxOrderId }).FirstOrDefaultAsync();
            if (deliveryRequestDetails != null)
            {
                var brokerDRs = mdbContext.BrokeredDRJobs.Find
                    (top => top.TfxAssignToCompanyId == deliveryRequestDetails.TfxAssignedToCompanyId
                    && top.TfxSupplierCompanyId == deliveryRequestDetails.TfxSupplierCompanyId && top.TfxJobId == deliveryRequestDetails.TfxJobId
                    && top.TfxOrderId == deliveryRequestDetails.TfxOrderId).ToList();
                if (brokerDRs.Count == 0)
                {
                    BrokeredDRJob brokeredDRJob = new BrokeredDRJob();
                    brokeredDRJob.TfxAssignToCompanyId = deliveryRequestDetails.TfxAssignedToCompanyId;
                    brokeredDRJob.TfxSupplierCompanyId = deliveryRequestDetails.TfxSupplierCompanyId;
                    brokeredDRJob.TfxCustomerCompany = deliveryRequestDetails.TfxCustomerCompany;
                    brokeredDRJob.TfxJobId = deliveryRequestDetails.TfxJobId;
                    brokeredDRJob.TfxOrderId = deliveryRequestDetails.TfxOrderId;
                    await mdbContext.BrokeredDRJobs.InsertOneAsync(brokeredDRJob);
                }


            }
        }

        private async Task UpdateDRStatusAsync(string drId, BrokeredDrCarrierStatus status)
        {
            var objDrId = ObjectId.Parse(drId);
            var filter = Builders<DeliveryRequest>.Filter.Where(t => t.Id == objDrId);
            var update = Builders<DeliveryRequest>.Update.Set(t => t.CarrierStatus, (int)status);
            await mdbContext.DeliveryRequests.UpdateOneAsync(filter, update);
        }
        public async Task<string> GetParentDeliveryRequestId(string childDrId)
        {
            string brokeredParentId = null;
            if (!string.IsNullOrWhiteSpace(childDrId))
            {
                var objChildDrId = ObjectId.Parse(childDrId);
                var filter = Builders<DeliveryRequest>.Filter.Where(t => t.Id == objChildDrId);
                brokeredParentId = await mdbContext.DeliveryRequests.Find(filter)
                                    .Project(t => t.BrokeredParentId).FirstOrDefaultAsync();
            }
            return brokeredParentId;
        }

        public async Task<ChildDeliveryRequestInfoViewModel> GetChildDeliveryRequestInfo(string parentDrId)
        {
            var response = new ChildDeliveryRequestInfoViewModel();
            if (!string.IsNullOrWhiteSpace(parentDrId))
            {
                var filter = Builders<DeliveryRequest>.Filter.Where(t => t.BrokeredParentId == parentDrId);
                var brokeredDr = await mdbContext.DeliveryRequests.Find(filter).SortByDescending(t => t.CreatedOn)
                                    .Project(t => new { t.Id, t.TfxOrderId, t.BrokeredParentId, t.Status }).FirstOrDefaultAsync();
                if (brokeredDr != null)
                {
                    if (brokeredDr.Status != DeliveryReqStatus.ScheduleCreated)
                    {
                        response.DrId = brokeredDr.Id.ToString();
                        response.OrderId = brokeredDr.TfxOrderId ?? 0;
                        response.BrokeredParentId = brokeredDr.BrokeredParentId;
                        response.StatusCode = (int)Status.Success;
                    }
                    else
                    {
                        response.StatusCode = (int)Status.Failed;
                        response.StatusMessage = Resource.valCantRecallScheduledDr;
                    }
                }

            }
            return response;
        }

        public async Task<StatusModel> RecallDeliveryRequest(string parentDrId, string childDrId, int tfxUserId)
        {
            var response = new StatusModel();
            using (var session = await mdbContext.Client.StartSessionAsync())
            {
                var objParentId = ObjectId.Parse(parentDrId);
                var parentFilter = Builders<DeliveryRequest>.Filter.Where(t => t.Id == objParentId);
                var parentUpdate = Builders<DeliveryRequest>.Update
                                    .Set(t => t.BrokeredChildId, null)
                                    .Set(t => t.CarrierStatus, (int)BrokeredDrCarrierStatus.Recalled)
                                    .Set(t => t.UpdatedOn, DateTimeOffset.Now)
                                    .Set(t => t.UpdatedBy, tfxUserId);

                var objChildId = ObjectId.Parse(childDrId);
                var childFilter = Builders<DeliveryRequest>.Filter.Where(t => t.Id == objChildId);
                var childUpdate = Builders<DeliveryRequest>.Update
                                    .Set(t => t.CarrierStatus, (int)BrokeredDrCarrierStatus.Recalled)
                                    .Set(t => t.IsActive, false)
                                    .Set(t => t.IsDeleted, true)
                                    .Set(t => t.UpdatedOn, DateTimeOffset.Now)
                                    .Set(t => t.UpdatedBy, tfxUserId);

                //delete the existing DR carrier sequence info.
                var dRCarrierSeqFilter = Builders<DRCarrierSequence>.Filter.Where(t => t.DeliveryRequestId == objChildId && t.IsActive && !t.IsDeleted);
                var dRCarrierSeqUpdate = Builders<DRCarrierSequence>.Update
                                    .Set(t => t.IsActive, false)
                                    .Set(t => t.IsDeleted, true)
                                    .Set(t => t.CreatedDate, DateTimeOffset.Now);
                try
                {
                    session.StartTransaction();
                    await mdbContext.DeliveryRequests.UpdateOneAsync(parentFilter, parentUpdate);
                    await mdbContext.DeliveryRequests.UpdateOneAsync(childFilter, childUpdate);
                    await mdbContext.DRCarrierSequences.UpdateOneAsync(dRCarrierSeqFilter, dRCarrierSeqUpdate);
                    await session.CommitTransactionAsync();
                    response.StatusCode = (int)Status.Success;
                    response.StatusMessage = "Delivery request recalled successfully.";
                }
                catch
                {
                    await session.AbortTransactionAsync();
                    throw;
                }
            }
            return response;
        }
        private async Task UpdateParentDeliveryCarrierStatus(string Id)
        {
            if (!string.IsNullOrEmpty(Id))
            {
                var deliveryFilters = Builders<DeliveryRequest>.Filter.And(
                          Builders<DeliveryRequest>.Filter.Where(t => t.BrokeredChildId == Id && !string.IsNullOrEmpty(t.BrokeredChildId)));
                UpdateDefinition<DeliveryRequest> updateFields = null;
                updateFields = Builders<DeliveryRequest>.Update
                        .Set(t => t.CarrierStatus, (int)BrokeredDrCarrierStatus.Delivered);
                await mdbContext.DeliveryRequests.UpdateManyAsync(deliveryFilters, updateFields);
            }
        }
        private async Task<List<Demand>> GetDemands(List<string> siteIdList, List<string> tankIdList, List<string> storageList, int timeout = 120)
        {
            List<Demand> demands = new List<Demand>();
            try
            {
                //Create SiteIds Table
                DataTable SiteIds, TankIds, StorageIds;
                IntializeDemandsParameters(siteIdList, tankIdList, storageList, out SiteIds, out TankIds, out StorageIds);
                var siteIdParam = new SqlParameter("@SiteList", SqlDbType.Structured);
                siteIdParam.Value = SiteIds;
                siteIdParam.TypeName = "dbo.DemandsSearchTypes";

                var tankIdsParam = new SqlParameter("@TankList", SqlDbType.Structured);
                tankIdsParam.Value = TankIds;
                tankIdsParam.TypeName = "dbo.DemandsSearchTypes";

                var storageIdParams = new SqlParameter("@StorageList", SqlDbType.Structured);
                storageIdParams.Value = StorageIds;
                storageIdParams.TypeName = "dbo.DemandsSearchTypes";

                context.Database.CommandTimeout = timeout;
                demands = await context.Database.SqlQuery<Demand>("usp_GetDemandsData @SiteList,@TankList,@StorageList", siteIdParam, tankIdsParam, storageIdParams).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryRequestRepository", "GetDemands", ex.Message, ex);
            }
            return demands;
        }
        public List<ForcastingServiceSetting> GetForcastingSetting(List<int> tankIds)
        {
            var response = new List<ForcastingServiceSetting>();
            try
            {
                DataTable objDt = new DataTable();

                string connectionString = ConfigurationManager.ConnectionStrings["DatabaseConnection"] != null ? ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString : string.Empty;
                //Create Table
                DataTable myTable = CreateTable();
                foreach (var item in tankIds)
                {
                    var row = myTable.NewRow();
                    row["Id"] = item;
                    myTable.Rows.Add(row);
                }
                if (!string.IsNullOrEmpty(connectionString))
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        try
                        {
                            using (SqlCommand cmd = new SqlCommand("usp_Get_Assetforcasting_Setting_Details"))
                            {
                                con.Open();
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Connection = con;
                                cmd.Parameters.AddWithValue("@TankIds", myTable);
                                SqlDataAdapter objDa = new SqlDataAdapter();
                                objDa.SelectCommand = cmd;
                                DataSet objDs = new DataSet();
                                objDa.Fill(objDs);
                                if (objDs.Tables.Count > 0)
                                {
                                    objDt = objDs.Tables[0];
                                }
                                con.Close();
                            }
                        }
                        catch (Exception ex)
                        {
                            con.Close();
                            LogManager.Logger.WriteException("StoredProcedureLogDomain", "GetForcastingSetting", ex.Message, ex);
                        }

                    }
                    response = ConvertDataTableToList<ForcastingServiceSetting>(objDt);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureLogDomain", "GetForcastingSetting", ex.Message, ex);
            }
            return response;
        }
        public List<ForcastingServiceCarrier> GetForcastingCarriers(List<int> forcastingIds)
        {
            var response = new List<ForcastingServiceCarrier>();
            try
            {
                DataTable objDt = new DataTable();

                string connectionString = ConfigurationManager.ConnectionStrings["DatabaseConnection"] != null ? ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString : string.Empty;
                //Create Table
                DataTable myTable = CreateTable();
                foreach (var item in forcastingIds)
                {
                    var row = myTable.NewRow();
                    row["Id"] = item;
                    myTable.Rows.Add(row);
                }
                if (!string.IsNullOrEmpty(connectionString))
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        try
                        {
                            using (SqlCommand cmd = new SqlCommand("usp_Get_Forcasting_Carrier_Details"))
                            {
                                con.Open();
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Connection = con;
                                cmd.Parameters.AddWithValue("@ForcastingSettingIds", myTable);
                                SqlDataAdapter objDa = new SqlDataAdapter();
                                objDa.SelectCommand = cmd;
                                DataSet objDs = new DataSet();
                                objDa.Fill(objDs);
                                if (objDs.Tables.Count > 0)
                                {
                                    objDt = objDs.Tables[0];
                                }
                                con.Close();
                            }
                        }
                        catch (Exception ex)
                        {
                            con.Close();
                            LogManager.Logger.WriteException("StoredProcedureLogDomain", "GetForcastingCarriers", ex.Message, ex);
                        }

                    }
                    response = ConvertDataTableToList<ForcastingServiceCarrier>(objDt);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureLogDomain", "GetForcastingCarriers", ex.Message, ex);
            }
            return response;
        }
        public async Task ScheduleOttoDeliveryRequest()
        {
            try
            {
                List<TankOttoDetails> tankOttoDetails = new List<TankOttoDetails>();
                int ottoDRCreationDays = 0;
                int.TryParse(ConfigurationManager.AppSettings["OttoDRCreationDays"].ToString(), out ottoDRCreationDays);
                if (ottoDRCreationDays > 0)
                {
                    DateTime startDate = DateTime.Now.Date;
                    DateTime endDate = DateTime.Now.AddDays(ottoDRCreationDays);
                    //get all otto delivery request details.
                    var allOttoDeliveryRequests = mdbContext.DeliveryRequests
                                                .Find(t => t.DelReqSource == DRSource.OttoForecasting && t.IsActive && !t.IsDeleted
                                                 && (t.Status != DeliveryReqStatus.ScheduleCreated && t.DeliveryWindowInfo != null
                                                 && t.DeliveryWindowInfo.StartDate >= startDate && t.GroupChildDRs.Count() == 0
                                                     ||
                                                     (t.TfxScheduleEnrouteStatus != (int)EnrouteDeliveryStatus.CompletedDrop
                                                       && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.Completed
                                                       && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.CompletedLate
                                                       && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.RescheduledCompleted
                                                       && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.Canceled
                                                       && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.RescheduledMissed
                                                       && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.Missed)
                                                       )
                                                       && t.Status != DeliveryReqStatus.Deleted
                                                       && (t.ScheduleQuantityType == 0 || t.ScheduleQuantityType == (int)ScheduleQuantityType.Quantity))
                                                .ToList();
                    allOttoDeliveryRequests = allOttoDeliveryRequests.OrderBy(top => top.DeliveryWindowInfo.StartDate).ToList();
                    //group by delivery request of otto for process of creating new otto dr based on last retain date.
                    var ottoGroupingDeliveryRequest = (from item in allOttoDeliveryRequests
                                                       orderby item.DeliveryWindowInfo.RetainDate.Date descending
                                                       group item by new { item.TfxJobId, item.StorageId, item.StorageTypeId, item.TfxDisplayJobId } into grouping
                                                       select new
                                                       {
                                                           DeliveryReq = grouping.FirstOrDefault()
                                                       }).Select(top => top.DeliveryReq).ToList();
                    //get the forcasting created dr and group by unique records and process for creating new otto dr.
                    var forecastingDeliveryReqs = await mdbContext.DeliveryRequests
                                                .Find(t => t.DelReqSource == DRSource.Forecasting
                                                 && t.IsActive && !t.IsDeleted && t.DeliveryWindowInfo != null
                                                 && (t.Status != DeliveryReqStatus.ScheduleCreated && t.GroupChildDRs.Count() == 0
                                                     ||
                                                     (t.TfxScheduleEnrouteStatus != (int)EnrouteDeliveryStatus.CompletedDrop
                                                       && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.Completed
                                                       && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.CompletedLate
                                                       && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.RescheduledCompleted
                                                       && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.Canceled
                                                       && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.RescheduledMissed
                                                       && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.Missed)
                                                       )
                                                       && t.Status != DeliveryReqStatus.Deleted
                                                       && (t.ScheduleQuantityType == 0 || t.ScheduleQuantityType == (int)ScheduleQuantityType.Quantity)).ToListAsync();
                    forecastingDeliveryReqs = forecastingDeliveryReqs.Where(t => t.DeliveryWindowInfo.StartDate.Date >= startDate.Date).ToList();
                    //group by forcasting records.
                    forecastingDeliveryReqs = (from item in forecastingDeliveryReqs
                                               orderby item.DeliveryWindowInfo.RetainDate.Date descending
                                               group item by new { item.TfxJobId, item.StorageId, item.StorageTypeId, item.TfxDisplayJobId } into grouping
                                               select new
                                               {
                                                   DeliveryReq = grouping.FirstOrDefault()
                                               }).Select(top => top.DeliveryReq).ToList();

                    //update the otto drs delivery window if any changes.
                    if (allOttoDeliveryRequests.Count() > 0)
                    {
                        var allTfxJobIds = allOttoDeliveryRequests.Select(t => t.TfxJobId).Distinct().ToList();

                        var allcarrierJobs = await mdbContext.CarrierJobs.Find(t => allTfxJobIds.Contains(t.TfxJobId) && t.IsActive)
                                                                        .Project(t => new CarrierJobInfoModel
                                                                        {
                                                                            TfxJobId = t.TfxJobId,
                                                                            TfxSupplierCompanyId = t.TfxSupplierCompanyId,
                                                                            TfxCarrierCompanyId = t.TfxCarrierCompanyId
                                                                        }).ToListAsync();
                        tankOttoDetails = await GetTankForcastingDetails(allOttoDeliveryRequests);
                        foreach (var ottoDelivery in allOttoDeliveryRequests)
                        {
                            var tankOtto = tankOttoDetails.Where(top => top.TfxJobId == ottoDelivery.TfxJobId
                                                          && top.TfxDisplayJobId == ottoDelivery.TfxDisplayJobId
                                                          && top.StorageId == ottoDelivery.StorageId
                                                          && top.TankId == ottoDelivery.StorageTypeId).FirstOrDefault();
                            TankLevelHelper tankLevelHelper = new TankLevelHelper();

                            if (tankOtto != null && tankOtto.forcastingServiceSettings != null && ottoDelivery.DeliveryWindowInfo != null)
                            {
                                if (tankOtto.FuelCapacity == null || tankOtto.FuelCapacity.Value <= 0)
                                {
                                    return; // Do not create DR as capacity is not mentioned for the tank.
                                }

                                tankLevelHelper.GetTankDetails(tankOtto);
                                var retainWindowInfo = ottoDelivery.DeliveryWindowInfo;
                                var forcastingSetting = tankOtto.forcastingServiceSettings;

                                var saleTankMonthlyData = await GetSaleTankMonthlyData(tankOtto.TfxDisplayJobId, tankOtto.TankId, tankOtto.StorageId);
                                //caculate the average load qty.
                                var averageLoadQty = (tankOtto.FuelCapacity.Value * forcastingSetting.AverageLoadQty) / 100;
                                DeliveryWindowInfo deliveryWindowInfo = new DeliveryWindowInfo();
                                var tankMaxFill = tankOtto.FuelCapacity.Value;
                                tankMaxFill = GetTankMaxFill(tankOtto);
                                var retainDate = retainWindowInfo.RetainDate;
                                DateTime retainTime = DateTime.MaxValue;
                                DateTime.TryParse(retainWindowInfo.RetainTime, out retainTime);
                                retainDate = retainDate.AddHours(retainTime.TimeOfDay.TotalHours);
                                decimal currentVolume = tankOtto.Retain + averageLoadQty;
                                //30*24(hours)=720 = 1 months hours to create the Otto DR.
                                var thisTankLevels = tankLevelHelper.GetTankLevels(saleTankMonthlyData.SaleMonthlyDataModel, retainDate.Date, currentVolume, saleTankMonthlyData.SaleTankModel.BandPeriod, 720, tankOtto.Retain, tankOtto.SafetyStock, tankOtto.RunOutLevel.GetValueOrDefault());
                                var thisTankLevelPriority = DeliveryReqPriority.None;
                                if (thisTankLevels.Any())
                                {
                                    var deliveryWindowInfoModel = CaculateTankLevelInfo(tankOtto.FuelCapacity, forcastingSetting, thisTankLevels, thisTankLevelPriority);
                                    if (deliveryWindowInfoModel != null)
                                    {
                                        if (thisTankLevelPriority == DeliveryReqPriority.None)
                                        {
                                            thisTankLevelPriority = ottoDelivery.Priority;
                                        }
                                        deliveryWindowInfo = deliveryWindowInfoModel?.ToEntity();
                                        var currentDeliveryWindowInfo = ottoDelivery.DeliveryWindowInfo;
                                        //verify the delivery window change data.
                                        if (ottoDelivery.Priority != thisTankLevelPriority
                                            || currentDeliveryWindowInfo.RetainDate.Date.ToString(Resource.constFormatDate) != deliveryWindowInfo.RetainDate.Date.ToString(Resource.constFormatDate)
                                            || currentDeliveryWindowInfo.RetainTime != deliveryWindowInfo.RetainTime
                                            || currentDeliveryWindowInfo.StartDate.Date.ToString(Resource.constFormatDate) != deliveryWindowInfo.StartDate.Date.ToString(Resource.constFormatDate)
                                            || currentDeliveryWindowInfo.StartTime != deliveryWindowInfo.StartTime)
                                        {
                                            var filter = Builders<DeliveryRequest>.Filter.Where(t => t.Id == ottoDelivery.Id);
                                            UpdateDefinition<DeliveryRequest> updateFields = Builders<DeliveryRequest>.Update
                                                .Set(t => t.DeliveryWindowInfo, deliveryWindowInfo)
                                                .Set(t => t.Priority, thisTankLevelPriority);
                                            await mdbContext.DeliveryRequests.UpdateOneAsync(filter, updateFields);
                                            LogManager.Logger.WriteDebug("DeliveryRequestRepository", "ScheduleOttoDeliveryRequest", $"Update Automate DR DeliveryWindowInfo to Id: {ottoDelivery.Id},Retain Date:{deliveryWindowInfo.RetainDate.ToString("MM/dd/yyyy")}, Retain Time:{deliveryWindowInfo.RetainTime}, Start Date:{deliveryWindowInfo.StartDate.ToString("MM/dd/yyyy")}, End Date:{deliveryWindowInfo.EndDate.ToString("MM/dd/yyyy")}, Start Time:{deliveryWindowInfo.StartTime}, End Time:{deliveryWindowInfo.EndTime}");
                                            //Need to insert the records in notification table for inform user to change the delivery window.
                                            if (currentDeliveryWindowInfo.RetainDate.Date.ToString(Resource.constFormatDate) != deliveryWindowInfo.RetainDate.Date.ToString(Resource.constFormatDate)
                                            || currentDeliveryWindowInfo.RetainTime != deliveryWindowInfo.RetainTime)
                                            {
                                                //get schedule builder details
                                                if (!string.IsNullOrEmpty(ottoDelivery.ScheduleBuilderId))
                                                {
                                                    await SaveOttoNotificationDetails(ottoDelivery, deliveryWindowInfo);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //create the otto drs based on forcasting records.
                    if (forecastingDeliveryReqs.Count > 0)
                    {
                        var allTfxJobIds = forecastingDeliveryReqs.Select(t => t.TfxJobId).Distinct().ToList();

                        var allcarrierJobs = await mdbContext.CarrierJobs.Find(t => allTfxJobIds.Contains(t.TfxJobId) && t.IsActive)
                                                                         .Project(t => new CarrierJobInfoModel
                                                                         {
                                                                             TfxJobId = t.TfxJobId,
                                                                             TfxSupplierCompanyId = t.TfxSupplierCompanyId,
                                                                             TfxCarrierCompanyId = t.TfxCarrierCompanyId
                                                                         }).ToListAsync();
                        tankOttoDetails = await GetTankForcastingDetails(forecastingDeliveryReqs);
                        foreach (var ottoDelivery in forecastingDeliveryReqs)
                        {
                            var tankOtto = tankOttoDetails.Where(top => top.TfxJobId == ottoDelivery.TfxJobId
                                                           && top.TfxDisplayJobId == ottoDelivery.TfxDisplayJobId
                                                           && top.StorageId == ottoDelivery.StorageId
                                                           && top.TankId == ottoDelivery.StorageTypeId).FirstOrDefault();
                            TankLevelHelper tankLevelHelper = new TankLevelHelper();

                            if (tankOtto != null && tankOtto.forcastingServiceSettings != null && ottoDelivery.DeliveryWindowInfo != null)
                            {
                                if (tankOtto.FuelCapacity == null || tankOtto.FuelCapacity.Value <= 0)
                                {
                                    return; // Do not create DR as capacity is not mentioned for the tank.
                                }

                                tankLevelHelper.GetTankDetails(tankOtto);
                                var retainWindowInfo = ottoDelivery.DeliveryWindowInfo;
                                var forcastingSetting = tankOtto.forcastingServiceSettings;

                                var saleTankMonthlyData = await GetSaleTankMonthlyData(tankOtto.TfxDisplayJobId, tankOtto.TankId, tankOtto.StorageId);
                                //check otto delivery request exists.
                                var existingOttoDeliveryRequests = allOttoDeliveryRequests.Where(t => t.TfxDisplayJobId == ottoDelivery.TfxDisplayJobId
                                                                          && t.StorageId == ottoDelivery.StorageId
                                                                          && t.StorageTypeId == ottoDelivery.StorageTypeId).Count();
                                //caculate the average load qty.
                                var averageLoadQty = (tankOtto.FuelCapacity.Value * forcastingSetting.AverageLoadQty) / 100;
                                if (existingOttoDeliveryRequests == 0 && saleTankMonthlyData != null && saleTankMonthlyData.SaleMonthlyDataModel.Count() > 0)
                                {
                                    DeliveryWindowInfoModel deliveryWindowInfoModel = new DeliveryWindowInfoModel();

                                    var tankMaxFill = tankOtto.FuelCapacity.Value;
                                    tankMaxFill = GetTankMaxFill(tankOtto);

                                    var retainDate = retainWindowInfo.RetainDate;
                                    DateTime retainTime = DateTime.MaxValue;
                                    DateTime.TryParse(retainWindowInfo.RetainTime, out retainTime);
                                    retainDate = retainDate.AddHours(retainTime.TimeOfDay.TotalHours);
                                    var startTime = DateTimeOffset.Now;
                                    startTime = CaculateOttoRetainStartTime(forcastingSetting, retainDate);
                                    var thisTankLevels = GetTankLevels(tankOtto.FuelCapacity, saleTankMonthlyData.SaleMonthlyDataModel, forcastingSetting, startTime.Date, saleTankMonthlyData.SaleTankModel.BandPeriod, tankOtto.SafetyStock, tankOtto.Retain, tankOtto.RunOutLevel.GetValueOrDefault(), endDate);
                                    //grouping the records based on date. so only one record with one date.remove duplicate records.
                                    thisTankLevels = (from item in thisTankLevels.OrderByDescending(top => top.DeliveryWindowInfoModel.RetainDate.Date)
                                                      where item.DeliveryWindowInfoModel.RetainDate.Date <= endDate.Date
                                                      group item by item.DeliveryWindowInfoModel.RetainDate.Date into grouping
                                                      select new SCDeliveryWindowInfo
                                                      {
                                                          DeliveryReqPriority = grouping.FirstOrDefault().DeliveryReqPriority,
                                                          DeliveryWindowInfoModel = grouping.FirstOrDefault().DeliveryWindowInfoModel
                                                      }).ToList();


                                    List<DeliveryRequestViewModel> drToCreate = new List<DeliveryRequestViewModel>();
                                    if (thisTankLevels.Count > 0)
                                    {
                                        var deliveryReqDetails = await CreateOttoDeliveryRequests(allcarrierJobs, ottoDelivery, tankOtto, forcastingSetting, tankMaxFill, thisTankLevels, drToCreate, averageLoadQty);
                                        //create schedule if forcastingSetting.IsOttoScheduleCreation enable.
                                        if (!forcastingSetting.IsOttoScheduleCreation)
                                        {
                                            LogManager.Logger.WriteDebug("DeliveryRequestRepository", "ScheduleOttoDeliveryRequest", "IsOttoScheduleCreation disabled." + ottoDelivery.TfxJobId);
                                        }
                                        if (deliveryReqDetails.Count() > 0 && forcastingSetting.IsOttoScheduleCreation)
                                        {
                                            //save the future Otto dr in schedule if any schedule information is there.
                                            await ProcessScheduleBuilder(ottoDelivery.TfxDisplayJobId, ottoDelivery.StorageTypeId, ottoDelivery.StorageId, ottoDelivery.TfxJobId, deliveryReqDetails);
                                        }
                                    }
                                }
                            }
                        }

                    }
                    //used for create future otto dr based on last processed otto retain date.
                    if (ottoGroupingDeliveryRequest.Count() > 0)
                    {
                        var allTfxJobIds = ottoGroupingDeliveryRequest.Select(t => t.TfxJobId).Distinct().ToList();

                        var allcarrierJobs = await mdbContext.CarrierJobs.Find(t => allTfxJobIds.Contains(t.TfxJobId) && t.IsActive)
                                                                         .Project(t => new CarrierJobInfoModel
                                                                         {
                                                                             TfxJobId = t.TfxJobId,
                                                                             TfxSupplierCompanyId = t.TfxSupplierCompanyId,
                                                                             TfxCarrierCompanyId = t.TfxCarrierCompanyId
                                                                         }).ToListAsync();
                        tankOttoDetails = await GetTankForcastingDetails(ottoGroupingDeliveryRequest);
                        foreach (var ottoDelivery in ottoGroupingDeliveryRequest)
                        {
                            var tankOtto = tankOttoDetails.Where(top => top.TfxJobId == ottoDelivery.TfxJobId
                                                           && top.TfxDisplayJobId == ottoDelivery.TfxDisplayJobId
                                                           && top.StorageId == ottoDelivery.StorageId
                                                           && top.TankId == ottoDelivery.StorageTypeId).FirstOrDefault();
                            TankLevelHelper tankLevelHelper = new TankLevelHelper();

                            if (tankOtto != null && tankOtto.forcastingServiceSettings != null && ottoDelivery.DeliveryWindowInfo != null)
                            {
                                if (tankOtto.FuelCapacity == null || tankOtto.FuelCapacity.Value <= 0)
                                {
                                    return; // Do not create DR as capacity is not mentioned for the tank.
                                }

                                tankLevelHelper.GetTankDetails(tankOtto);
                                var retainWindowInfo = ottoDelivery.DeliveryWindowInfo;
                                var forcastingSetting = tankOtto.forcastingServiceSettings;

                                var saleTankMonthlyData = await GetSaleTankMonthlyData(tankOtto.TfxDisplayJobId, tankOtto.TankId, tankOtto.StorageId);
                                //caculate the average load qty.
                                var averageLoadQty = (tankOtto.FuelCapacity.Value * forcastingSetting.AverageLoadQty) / 100;
                                if (saleTankMonthlyData != null && saleTankMonthlyData.SaleMonthlyDataModel.Count() > 0)
                                {
                                    DeliveryWindowInfoModel deliveryWindowInfoModel = new DeliveryWindowInfoModel();

                                    var tankMaxFill = tankOtto.FuelCapacity.Value;
                                    tankMaxFill = GetTankMaxFill(tankOtto);

                                    var retainDate = retainWindowInfo.RetainDate;
                                    DateTime retainTime = DateTime.MaxValue;
                                    DateTime.TryParse(retainWindowInfo.RetainTime, out retainTime);
                                    retainDate = retainDate.AddHours(retainTime.TimeOfDay.TotalHours);
                                    var startTime = DateTimeOffset.Now;
                                    startTime = CaculateOttoRetainStartTime(forcastingSetting, retainDate);
                                    var thisTankLevels = GetTankLevels(tankOtto.FuelCapacity, saleTankMonthlyData.SaleMonthlyDataModel, forcastingSetting, startTime.Date, saleTankMonthlyData.SaleTankModel.BandPeriod, tankOtto.SafetyStock, tankOtto.Retain, tankOtto.RunOutLevel.GetValueOrDefault(), endDate);
                                    //grouping the records based on date. so only one record with one date.remove duplicate records
                                    //get the current and future DRs tank level details for create DRs.
                                    thisTankLevels = (from item in thisTankLevels.OrderByDescending(top => top.DeliveryWindowInfoModel.RetainDate.Date)
                                                      where item.DeliveryWindowInfoModel.RetainDate.Date <= endDate.Date && item.DeliveryWindowInfoModel.RetainDate.Date >= startDate.Date
                                                      group item by item.DeliveryWindowInfoModel.RetainDate.Date into grouping
                                                      select new SCDeliveryWindowInfo
                                                      {
                                                          DeliveryReqPriority = grouping.FirstOrDefault().DeliveryReqPriority,
                                                          DeliveryWindowInfoModel = grouping.FirstOrDefault().DeliveryWindowInfoModel
                                                      }).ToList();


                                    List<DeliveryRequestViewModel> drToCreate = new List<DeliveryRequestViewModel>();
                                    if (thisTankLevels.Count > 0)
                                    {
                                        var deliveryReqDetails = await CreateOttoDeliveryRequests(allcarrierJobs, ottoDelivery, tankOtto, forcastingSetting, tankMaxFill, thisTankLevels, drToCreate, averageLoadQty);
                                        //create schedule if forcastingSetting.IsOttoScheduleCreation enable.
                                        if (!forcastingSetting.IsOttoScheduleCreation)
                                        {
                                            LogManager.Logger.WriteDebug("DeliveryRequestRepository", "ScheduleOttoDeliveryRequest", "IsOttoScheduleCreation disabled." + ottoDelivery.TfxJobId);
                                        }
                                        if (deliveryReqDetails.Count() > 0 && forcastingSetting.IsOttoScheduleCreation)
                                        {
                                            //save the future Otto dr in schedule if any schedule information is there.
                                            await ProcessScheduleBuilder(ottoDelivery.TfxDisplayJobId, ottoDelivery.StorageTypeId, ottoDelivery.StorageId, ottoDelivery.TfxJobId, deliveryReqDetails);
                                        }
                                    }
                                }
                            }
                        }
                    }


                }
                else
                {
                    LogManager.Logger.WriteError("DeliveryRequestRepository", "ScheduleOttoDeliveryRequest", "ottoDRCreationDays missing in config file.");
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryRequestRepository", "ScheduleOttoDeliveryRequest", ex.Message, ex);
            }
        }

        private async Task SaveOttoNotificationDetails(DeliveryRequest item, DeliveryWindowInfo deliveryWindowInfo)
        {
            ObjectId objectId = ObjectId.Parse(item.ScheduleBuilderId);
            var scheduleBuilderInfo = mdbContext.ScheduleBuilders.Find(top => top.Id == objectId).FirstOrDefault();
            if (scheduleBuilderInfo != null)
            {
                string shiftInfo = string.Empty;
                string shiftRowIndex = string.Empty;
                bool deliveryReqExists = false;
                foreach (var tripitem in scheduleBuilderInfo.Trips)
                {
                    if (tripitem.DeliveryRequests.Contains(item.Id))
                    {
                        deliveryReqExists = true;
                        shiftInfo = tripitem.ShiftStartTime + "-" + tripitem.EndTime;
                        shiftRowIndex = "Row-" + tripitem.DriverColIndex;
                        break;
                    }

                }
                if (deliveryReqExists)
                {
                    DsbNotificationModel dsbNotificationModel = new DsbNotificationModel();
                    dsbNotificationModel.TfxJobId = item.TfxJobId;
                    dsbNotificationModel.RegionId = item.TfxAssignedToRegionId;
                    dsbNotificationModel.CreatedBy = 1;
                    dsbNotificationModel.Status = (int)DsbNotificationStatus.UnRead;
                    dsbNotificationModel.Type = (int)DsbNotificationType.Otto;
                    dsbNotificationModel.ScheduleBuilderId = string.Empty;
                    dsbNotificationModel.ScheduleBuilderId = scheduleBuilderInfo.Id.ToString();
                    dsbNotificationModel.Message = FormatOttoNotification(item, deliveryWindowInfo, shiftInfo, shiftRowIndex, scheduleBuilderInfo.DateFilter.ToString(Resource.constFormatDate));
                    var forecastingRepository = new ForecastingRepository();
                    await forecastingRepository.SaveDsbNotification(dsbNotificationModel);
                }
            }
        }

        private async Task<List<ObjectId>> CreateOttoDeliveryRequests(List<CarrierJobInfoModel> allcarrierJobs, DeliveryRequest ottoDelivery, TankOttoDetails tankOtto, ForcastingServiceSetting forcastingSetting, decimal tankMaxFill, List<SCDeliveryWindowInfo> thisTankLevels, List<DeliveryRequestViewModel> drToCreate, decimal averageLoadQty)
        {
            List<ObjectId> delvieryReq = new List<ObjectId>();
            int supplierCompanyId = 0;
            foreach (var item in thisTankLevels)
            {

                var allOttoDeliveryRequests = await mdbContext.DeliveryRequests
                                                .Find(t => t.DelReqSource == DRSource.OttoForecasting && t.IsActive && !t.IsDeleted
                                                 && t.Status != DeliveryReqStatus.ScheduleCreated && t.DeliveryWindowInfo != null
                                                 && t.TfxJobId == ottoDelivery.TfxJobId && t.TfxDisplayJobId == ottoDelivery.TfxDisplayJobId && t.StorageId == ottoDelivery.StorageId && t.StorageTypeId == ottoDelivery.StorageTypeId
                                                       && t.Status != DeliveryReqStatus.Deleted
                                                       && (t.ScheduleQuantityType == 0 || t.ScheduleQuantityType == (int)ScheduleQuantityType.Quantity))
                                                .ToListAsync();
                var existingRecord = allOttoDeliveryRequests.FirstOrDefault(top => top.DeliveryWindowInfo.RetainDate.Date.ToString(Resource.constFormatDate) == item.DeliveryWindowInfoModel.RetainDate.Date.ToString(Resource.constFormatDate) && top.TfxDisplayJobId == ottoDelivery.TfxDisplayJobId && top.StorageId == ottoDelivery.StorageId && top.StorageTypeId == ottoDelivery.StorageTypeId);
                if (existingRecord == null)
                {
                    DeliveryRequestViewModel dr = new DeliveryRequestViewModel()
                    {
                        RequiredQuantity = averageLoadQty,
                        CurrentQuantity = tankMaxFill - averageLoadQty,
                        CurrentThreshold = ottoDelivery.CurrentThreshold.GetValueOrDefault(),
                        ProductTypeId = tankOtto.FuelTypeId,
                        ProductType = tankOtto.ProductTypeName,
                        StorageId = ottoDelivery.StorageId,
                        TankId = ottoDelivery.StorageTypeId,
                        JobId = ottoDelivery.TfxJobId,
                        JobName = ottoDelivery.TfxJobName,
                        Priority = item.DeliveryReqPriority,
                        TankMaxFill = tankMaxFill,
                        SiteId = ottoDelivery.TfxDisplayJobId,
                        CreatedOn = DateTimeOffset.Now,
                        UpdatedOn = DateTimeOffset.Now,
                        IsActive = true,
                        IsDeleted = false,
                        DelReqSource = DRSource.OttoForecasting,
                        AutoDRStatus = AutoDrStatus.Create,
                        AutoCreatedOn = DateTimeOffset.Now,
                        ScheduleQuantityType = (int)ScheduleQuantityType.Quantity,
                        ScheduleQuantityTypeText = ScheduleQuantityType.Quantity.GetDisplayName(),
                        DeliveryWindowInfo = item.DeliveryWindowInfoModel,
                        CreatedBy = (int)SystemUser.System
                    };
                    supplierCompanyId = AssignSupplierCompanyId(allcarrierJobs, tankOtto, dr, forcastingSetting);
                    if (supplierCompanyId == 0)
                    {
                        LogManager.Logger.WriteDebug("DeliveryRequestRepository", "ScheduleOttoDeliveryRequest", $"Supplier not found for jobId:{tankOtto.TfxJobId}, supplierCompanyId:{supplierCompanyId}, TfxProductTypeId:{tankOtto.FuelTypeId}");
                    }
                    else if (supplierCompanyId == -2)
                    {
                        LogManager.Logger.WriteDebug("DeliveryRequestRepository", "ScheduleOttoDeliveryRequest", $"Carrier Assignment None for jobId:{tankOtto.TfxJobId}, supplierCompanyId:{supplierCompanyId}, TfxProductTypeId:{tankOtto.FuelTypeId}, ForcastingSettingId:{forcastingSetting.ForcastingSettingId}");
                    }
                    else if (supplierCompanyId == -3)
                    {
                        LogManager.Logger.WriteDebug("DeliveryRequestRepository", "ScheduleOttoDeliveryRequest", $"Carrier Assignment Not Found for jobId:{tankOtto.TfxJobId}, supplierCompanyId:{supplierCompanyId}, TfxProductTypeId:{tankOtto.FuelTypeId}, ForcastingSettingId:{forcastingSetting.ForcastingSettingId}");
                    }
                    else
                    {
                        await SetDRAdditionalDetails(dr, tankOtto.TfxJobId, supplierCompanyId, tankOtto.FuelTypeId);
                        dr.CreatedBy = dr.CreatedBy > 0 ? dr.CreatedBy : (int)SystemUser.System;
                        drToCreate.Add(dr);
                    }
                }
            }
            if (drToCreate.Count > 0)
            {
                var response = await CreateDeliveryRequest(drToCreate);
                if (response.StatusCode == (int)Status.Success)
                {
                    delvieryReq.AddRange(response.EntityIds.Select(top => ObjectId.Parse(top)));
                    DeliveryRequestAccess drExchangeAccess = new DeliveryRequestAccess();
                    foreach (var drItem in response.EntityIds)
                    {
                        var message = new TankDeliveryRequestMessageModel { EntityId = drItem };
                        var jsonMessage = new JavaScriptSerializer().Serialize(message);
                        var createdBy = drToCreate.FirstOrDefault().CreatedBy > 0 ? drToCreate.FirstOrDefault().CreatedBy : (int)SystemUser.System;
                        drExchangeAccess.AddNotificationEvent((int)EventType.TankDeliveryRequestCreated, createdBy, jsonMessage);
                    }
                    LogManager.Logger.WriteDebug("DeliveryRequestRepository", "CreateDRUsingScheduleOttoDeliveryRequest", $"Automate DR Creation Status:{(Status)response.StatusCode} {response.StatusMessage}, for jobId:{tankOtto.TfxJobId}, supplierCompanyId:{supplierCompanyId}, StorageTYpeId:{tankOtto.TankId}, storageId:{tankOtto.StorageId}, Quantity:{forcastingSetting.AverageLoadQty}");
                }
                else
                {
                    LogManager.Logger.WriteDebug("DeliveryRequestRepository", "Error-CreateDRUsingScheduleOttoDeliveryRequest", response.StatusMessage);
                }
            }
            return delvieryReq;
        }

        private static DateTimeOffset CaculateOttoRetainStartTime(ForcastingServiceSetting forcastingSetting, DateTimeOffset retainDate)
        {
            DateTimeOffset startTime = retainDate;
            if (forcastingSetting.RetainTimeBufferUOM == forcastingSetting.StartBufferUOM)
            {
                var maxBuffer = forcastingSetting.StartBuffer > forcastingSetting.RetainTimeBuffer ? forcastingSetting.StartBuffer : forcastingSetting.RetainTimeBuffer;
                startTime = retainDate.AddHours(-maxBuffer);
            }
            else if (forcastingSetting.RetainTimeBufferUOM == (int)RateOfConsumsionUOM.Hours && forcastingSetting.StartBufferUOM == (int)RateOfConsumsionUOM.Days)
            {
                var maxBuffer = forcastingSetting.StartBuffer;
                startTime = retainDate.AddDays(-maxBuffer);
            }
            else if (forcastingSetting.RetainTimeBufferUOM == (int)RateOfConsumsionUOM.Days && forcastingSetting.StartBufferUOM == (int)RateOfConsumsionUOM.Hours)
            {
                var maxBuffer = forcastingSetting.RetainTimeBufferUOM;
                startTime = retainDate.AddDays(-maxBuffer);
            }
            return startTime;
        }

        private DeliveryReqPriority GetDeliveryReqPriority(TankOttoDetails tankOtto, decimal currentVolume)
        {
            var current = (currentVolume / tankOtto.FuelCapacity.Value) * 100;
            var currentInventory = (current < 0) ? 0 : (current > 100) ? 100 : current;
            var definedPrioirity = GetDeliveryRequestPriority(currentInventory, tankOtto.MinFill, (int)tankOtto.FillType, tankOtto.RunOutLevel, tankOtto.FuelCapacity, tankOtto.ThresholdDeliveryRequest);
            return definedPrioirity;
        }

        private int AssignSupplierCompanyId(List<CarrierJobInfoModel> allcarrierJobs, TankOttoDetails tankOtto, DeliveryRequestViewModel dr, ForcastingServiceSetting forcastingServiceSetting = null)
        {
            int supplierCompanyId;
            var carrierJob = allcarrierJobs.Where(t => t.TfxJobId == tankOtto.TfxJobId).FirstOrDefault();
            if (carrierJob != null)
            {
                if (forcastingServiceSetting == null)
                {
                    supplierCompanyId = carrierJob.TfxSupplierCompanyId;
                    dr.AssignedToCompanyId = carrierJob.TfxCarrierCompanyId;
                }
                else
                {
                    supplierCompanyId = ForecastingCarrierAssignment(allcarrierJobs, dr, forcastingServiceSetting, carrierJob);
                }
            }
            else
            {
                supplierCompanyId = mdbContext.Regions.Find(t => t.TfxJobs.Any(t1 => t1.Id == tankOtto.TfxJobId) && t.IsActive)
                                                  .Project(t => t.TfxCompanyId).FirstOrDefault();
                dr.AssignedToCompanyId = supplierCompanyId;
            }

            return supplierCompanyId;
        }

        private static int ForecastingCarrierAssignment(List<CarrierJobInfoModel> allcarrierJobs, DeliveryRequestViewModel dr, ForcastingServiceSetting forcastingServiceSetting, CarrierJobInfoModel carrierJob)
        {
            int supplierCompanyId;
            var forcastingCarrierSetting = forcastingServiceSetting.IsAllCarrierEnabled;
            if (forcastingCarrierSetting == -1)
            {
                supplierCompanyId = -2; //None selected
                dr.AssignedToCompanyId = carrierJob.TfxCarrierCompanyId;
            }
            else if (forcastingCarrierSetting == 1)
            {
                supplierCompanyId = carrierJob.TfxSupplierCompanyId;
                dr.AssignedToCompanyId = carrierJob.TfxCarrierCompanyId;
            }
            else
            {
                var carrierList = forcastingServiceSetting.CarrierList;
                var existsrecords = allcarrierJobs.Any(top => carrierList.Contains(top.TfxCarrierCompanyId));
                if (existsrecords)
                {
                    supplierCompanyId = carrierJob.TfxSupplierCompanyId;
                    dr.AssignedToCompanyId = carrierJob.TfxCarrierCompanyId;
                }
                else
                {
                    supplierCompanyId = -3; // not found carrier.
                    dr.AssignedToCompanyId = carrierJob.TfxCarrierCompanyId;
                }
            }

            return supplierCompanyId;
        }

        private static decimal GetTankMaxFill(TankOttoDetails tankOtto)
        {
            decimal tankMaxFill;
            if (tankOtto.FillType == (int)FillType.UoM)
            {
                tankMaxFill = tankOtto.MaxFill ?? tankOtto.FuelCapacity.Value;
            }
            else
            {
                var maxFill = tankOtto.MaxFill ?? 100;
                tankMaxFill = (maxFill * tankOtto.FuelCapacity.Value / 100);
            }

            return tankMaxFill;
        }

        private async Task<List<TankOttoDetails>> GetTankForcastingDetails(List<DeliveryRequest> forecastingDeliveryReqs)
        {
            List<TankOttoDetails> tankOttoDetails = new List<TankOttoDetails>();
            List<ForcastingServiceSetting> forcastingServiceSettings = new List<ForcastingServiceSetting>();
            var getForcastingSetting = forecastingDeliveryReqs.Select(top => new { top.TfxJobId, top.TfxDisplayJobId, top.StorageTypeId, top.StorageId }).ToList();
            var tempStorageId = getForcastingSetting.Select(top => top.StorageId).ToList();
            var tempStorageTypeId = getForcastingSetting.Select(top => top.StorageTypeId).ToList();
            var tempTfxJobId = getForcastingSetting.Select(top => top.TfxJobId).ToList();
            var tempTfxDisplayJobId = getForcastingSetting.Select(top => top.TfxDisplayJobId).ToList();
            var filter = Builders<JobAdditionalDetail>.Filter.And(
                       Builders<JobAdditionalDetail>.Filter.Where(x => x.IsActive && x.Tanks != null && x.Tanks.Any(x1 => tempStorageId.Contains(x1.StorageId)) && x.Tanks.Any(x1 => tempStorageTypeId.Contains(x1.StorageTypeId))),
                       Builders<JobAdditionalDetail>.Filter.In(x => x.TfxJobId, tempTfxJobId),
                        Builders<JobAdditionalDetail>.Filter.In(x => x.TfxDisplayJobId, tempTfxDisplayJobId));

            var tempjobAdditionalDetails = mdbContext.JobAdditionalDetails.Find(filter).Project(t => new { t.TfxJobId, t.TfxDisplayJobId, t.Tanks }).ToList();
            foreach (var deliveryRequest in forecastingDeliveryReqs)
            {
                var tankDetails = tempjobAdditionalDetails.FirstOrDefault(top => top.TfxJobId == deliveryRequest.TfxJobId && top.TfxDisplayJobId == deliveryRequest.TfxDisplayJobId);
                if (tankDetails != null)
                {
                    var tankInfo = tankDetails.Tanks.Where(top => top.StorageId == deliveryRequest.StorageId && top.StorageTypeId == deliveryRequest.StorageTypeId).ToList();
                    foreach (var item in tankInfo)
                    {
                        TankOttoDetails tankOtto = new TankOttoDetails();
                        tankOtto.TfxAssetId = item.TfxAssetId;
                        tankOtto.StorageId = item.StorageId;
                        tankOtto.TankId = item.StorageTypeId;
                        tankOtto.TfxJobId = tankDetails.TfxJobId;
                        tankOtto.TfxDisplayJobId = tankDetails.TfxDisplayJobId;
                        tankOtto.FillType = item.FillType;
                        tankOtto.FuelCapacity = item.FuelCapacity;
                        tankOtto.MaxFill = item.MaxFill;
                        tankOtto.FuelTypeId = item.TfxProductTypeId;
                        tankOtto.ProductTypeName = item.TfxProductTypeName;
                        tankOtto.MinFill = item.MinFill;
                        tankOtto.RunOutLevel = item.RunOutLevel;
                        tankOtto.ThresholdDeliveryRequest = item.ThresholdDeliveryRequest;
                        tankOttoDetails.Add(tankOtto);
                    }
                }
            }
            if (tankOttoDetails.Count() > 0)
            {
                var allTankAssetIds = tankOttoDetails.Select(top => top.TfxAssetId).Distinct().ToList();
                forcastingServiceSettings = GetForcastingSetting(allTankAssetIds).Where(top => top.IsOttoAutoDRCreation).ToList();
                if (forcastingServiceSettings.Count() > 0)
                {
                    //getting the carrier list for forecasting setting.
                    var carierForcastingDetails = GetForcastingCarriers(forcastingServiceSettings.Select(top => top.ForcastingSettingId).Distinct().ToList()).ToList();
                    foreach (var item in forcastingServiceSettings)
                    {
                        var carierForcasting = carierForcastingDetails.FirstOrDefault(top => top.ForcastingServiceSettingId == item.ForcastingSettingId);
                        if (carierForcasting != null)
                        {
                            if (carierForcasting.IsAllCarrierEnabled)
                            {
                                item.IsAllCarrierEnabled = 1;//All selected.
                            }
                            else if (!carierForcasting.IsAllCarrierEnabled && carierForcasting.CarrierIdList.Count() == 0)
                            {
                                item.IsAllCarrierEnabled = -1; //None selected.
                            }
                            else
                            {
                                item.IsAllCarrierEnabled = 2; //selected carrier.
                            }
                            item.CarrierList.AddRange(carierForcasting.CarrierIdList);
                        }
                    }
                }
                var allSiteIds = tankOttoDetails.Select(t => t.TfxDisplayJobId).Distinct().ToList();
                var allStorageIds = tankOttoDetails.Select(t => t.StorageId).Distinct().ToList();
                var allTankIds = tankOttoDetails.Select(t => t.TankId).Distinct().ToList();
                var demandsData = await GetDemands(allSiteIds, allTankIds, allStorageIds);
                if (forcastingServiceSettings.Count() > 0)
                {
                    foreach (var tankOttoItem in tankOttoDetails)
                    {
                        var forcastingServiceSetting = forcastingServiceSettings.Where(top => top.Id == tankOttoItem.TfxAssetId).FirstOrDefault();
                        if (forcastingServiceSetting != null)
                        {
                            tankOttoItem.forcastingServiceSettings = new ForcastingServiceSetting();
                            tankOttoItem.forcastingServiceSettings = forcastingServiceSetting;
                        }
                    }
                }
            }
            return tankOttoDetails;
        }

        private static DataTable CreateTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Id", typeof(Int32));
            return dt;
        }
        private static List<T> ConvertDataTableToList<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                    {
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            return obj;
        }
        public async Task<SaleMonthlyDataModelDetails> GetSaleTankMonthlyData(string TfxDisplayJobId, string TankId, string StorageId)
        {
            SaleMonthlyDataModelDetails saleMonthlyDataModelDetails = new SaleMonthlyDataModelDetails();
            ForecastingRepository _forecastingRepository = new ForecastingRepository();
            // Get tank retain, safty-stock and runout levels
            var saleTank = await _forecastingRepository.GetSaleTank(TfxDisplayJobId, TankId, StorageId);
            if (saleTank == null)
            {
                return saleMonthlyDataModelDetails;
            }

            var monthStartDate = DateTime.Now.AddDays(-29).Date;
            var monthEndDate = DateTime.Now.AddDays(-1).Date;
            saleMonthlyDataModelDetails.SaleMonthlyDataModel = await _forecastingRepository.GetSaleTankMonthlyData(monthStartDate, monthEndDate, saleTank.Id);
            saleMonthlyDataModelDetails.SaleTankModel = saleTank;
            return saleMonthlyDataModelDetails;
        }
        public List<SCDeliveryWindowInfo> GetTankLevels(decimal? fuelCapacity, List<SaleMonthlyDataModel> monthlyDatas, ForcastingServiceSetting forecastingSettings, DateTime startTime, int BandPeriod, decimal SafetyStock, decimal Retain, decimal RunOutLevel, DateTime endDate)
        {

            List<SCDeliveryWindowInfo> sCDeliveryWindowInfo = new List<SCDeliveryWindowInfo>();
            TankLevelHelper tankLevelHelper = new TankLevelHelper();
            int daysCount = 0; //to prevent the infinity loop.
            while (startTime.Date < endDate.Date && daysCount <= 30)
            {
                decimal averageLoadQty = 0;
                if (fuelCapacity != null && fuelCapacity.HasValue && fuelCapacity > 0)
                {
                    averageLoadQty = (fuelCapacity.Value * forecastingSettings.AverageLoadQty) / 100;
                }
                decimal currentVolume = Retain + averageLoadQty;
                //30*24(hours)=720 = 1 months hours to create the Otto DR.
                var thisTankLevels = tankLevelHelper.GetTankLevels(monthlyDatas, startTime, currentVolume, BandPeriod, 720, Retain, SafetyStock, RunOutLevel);
                var thisTankLevelPriority = DeliveryReqPriority.None;
                if (thisTankLevels.Any())
                {
                    DeliveryWindowInfoModel deliveryWindowInfo = null;
                    SCDeliveryWindowInfo sCDeliveryWindow = new SCDeliveryWindowInfo();
                    startTime = CaculateTankLevelInfo(forecastingSettings, startTime, sCDeliveryWindowInfo, thisTankLevels, ref thisTankLevelPriority, ref deliveryWindowInfo, sCDeliveryWindow);
                }
                else
                {
                    startTime = startTime.Date.AddDays(1);
                }
                daysCount++;
            }

            return sCDeliveryWindowInfo;
        }

        private static DateTime CaculateTankLevelInfo(ForcastingServiceSetting forecastingSettings, DateTime startTime, List<SCDeliveryWindowInfo> sCDeliveryWindowInfo, List<TankLevelModel> thisTankLevels, ref DeliveryReqPriority thisTankLevelPriority, ref DeliveryWindowInfoModel deliveryWindowInfo, SCDeliveryWindowInfo sCDeliveryWindow)
        {
            var startTimeDetails = startTime;
            var level = thisTankLevels.OrderBy(t => t.Type).First();
            if (level.Type == 1)
            {
                thisTankLevelPriority = DeliveryReqPriority.CouldGo;
            }
            else if (level.Type == 2)
            {
                thisTankLevelPriority = DeliveryReqPriority.ShouldGo;
            }
            else if (level.Type == 3)
            {
                thisTankLevelPriority = DeliveryReqPriority.MustGo;
            }
            if (thisTankLevelPriority != DeliveryReqPriority.None)
            {
                deliveryWindowInfo = new DeliveryWindowInfoModel();
                if (forecastingSettings.RetainTimeBufferUOM == ((int)RateOfConsumsionUOM.Hours))
                {
                    deliveryWindowInfo.RetainDate = startTime.AddHours(level.Hours);
                }
                else
                {
                    deliveryWindowInfo.RetainDate = startTime.AddDays(level.Hours);
                }
                deliveryWindowInfo.RetainTime = deliveryWindowInfo.RetainDate.ToString(Resource.constFormat12HourTime);

                var retainHoursStart = level.Hours - Math.Max(forecastingSettings.RetainTimeBuffer, forecastingSettings.StartBuffer);
                if (forecastingSettings.StartBufferUOM == ((int)RateOfConsumsionUOM.Hours))
                {
                    deliveryWindowInfo.StartDate = startTime.AddHours(retainHoursStart);
                }
                else
                {
                    deliveryWindowInfo.StartDate = startTime.AddDays(retainHoursStart);
                }
                deliveryWindowInfo.StartTime = deliveryWindowInfo.StartDate.ToString(Resource.constFormat12HourTime);

                var retainHoursEnd = level.Hours + forecastingSettings.EndBuffer;
                if (forecastingSettings.EndBufferUOM == ((int)RateOfConsumsionUOM.Hours))
                {
                    deliveryWindowInfo.EndDate = startTime.AddHours(retainHoursEnd);
                }
                else
                {
                    deliveryWindowInfo.EndDate = startTime.AddDays(retainHoursEnd);
                }
                deliveryWindowInfo.EndTime = deliveryWindowInfo.EndDate.ToString(Resource.constFormat12HourTime);
                sCDeliveryWindow.DeliveryWindowInfoModel = deliveryWindowInfo;
                sCDeliveryWindow.DeliveryReqPriority = thisTankLevelPriority;
                sCDeliveryWindowInfo.Add(sCDeliveryWindow);
                //update the start date and process.
                startTimeDetails = deliveryWindowInfo.RetainDate;
            }
            return startTimeDetails;
        }
        private static DeliveryWindowInfoModel CaculateTankLevelInfo(decimal? fuelCapacity, ForcastingServiceSetting forecastingSettings, List<TankLevelModel> thisTankLevels, DeliveryReqPriority thisTankLevelPriority)
        {
            DeliveryWindowInfoModel deliveryWindowInfo = null;
            var level = thisTankLevels.OrderBy(t => t.Type).First();
            if (forecastingSettings.InventoryPriorityType == 2)
            {
                if (forecastingSettings.InventoryUOM == 2)
                {
                    int defaulthours = 24;
                    forecastingSettings.RetainCouldGo = forecastingSettings.RetainCouldGo * defaulthours;
                    forecastingSettings.SafetyStockShouldGo = forecastingSettings.SafetyStockShouldGo * defaulthours;
                    forecastingSettings.RunoutLevelMustGo = forecastingSettings.RunoutLevelMustGo * 24;
                }
                if (level.Type == 1 && level.Hours <= forecastingSettings.RetainCouldGo)
                {
                    thisTankLevelPriority = DeliveryReqPriority.CouldGo;
                }
                else if (level.Type == 2 && level.Hours <= forecastingSettings.SafetyStockShouldGo)
                {
                    thisTankLevelPriority = DeliveryReqPriority.ShouldGo;
                }
                else if (level.Type == 3 && level.Hours <= forecastingSettings.RunoutLevelMustGo)
                {
                    thisTankLevelPriority = DeliveryReqPriority.MustGo;
                }
            }
            else
            {
                if (forecastingSettings.InventoryUOM == 1)
                {
                    if (level.Type == 1 && level.Quantity <= forecastingSettings.RetainCouldGo)
                    {
                        thisTankLevelPriority = DeliveryReqPriority.CouldGo;
                    }
                    else if (level.Type == 2 && level.Quantity <= forecastingSettings.SafetyStockShouldGo)
                    {
                        thisTankLevelPriority = DeliveryReqPriority.ShouldGo;
                    }
                    else if (level.Type == 3 && level.Quantity <= forecastingSettings.RunoutLevelMustGo)
                    {
                        thisTankLevelPriority = DeliveryReqPriority.MustGo;
                    }
                }
                else
                {
                    if (fuelCapacity != null && fuelCapacity.HasValue && fuelCapacity.Value > 0)
                    {
                        forecastingSettings.RetainCouldGo = Convert.ToInt32((fuelCapacity.Value * forecastingSettings.RetainCouldGo) / 100);
                        forecastingSettings.SafetyStockShouldGo = Convert.ToInt32((fuelCapacity.Value * forecastingSettings.SafetyStockShouldGo) / 100);
                        forecastingSettings.RetainCouldGo = Convert.ToInt32((fuelCapacity.Value * forecastingSettings.RetainCouldGo) / 100);
                    }
                    if (level.Type == 1 && level.Quantity <= forecastingSettings.RetainCouldGo)
                    {
                        thisTankLevelPriority = DeliveryReqPriority.CouldGo;
                    }
                    else if (level.Type == 2 && level.Quantity <= forecastingSettings.SafetyStockShouldGo)
                    {
                        thisTankLevelPriority = DeliveryReqPriority.ShouldGo;
                    }
                    else if (level.Type == 3 && level.Quantity <= forecastingSettings.RunoutLevelMustGo)
                    {
                        thisTankLevelPriority = DeliveryReqPriority.MustGo;
                    }
                }
            }
            if (thisTankLevelPriority != DeliveryReqPriority.None)
            {
                deliveryWindowInfo = new DeliveryWindowInfoModel();
                if (forecastingSettings.RetainTimeBufferUOM == ((int)RateOfConsumsionUOM.Hours))
                {
                    deliveryWindowInfo.RetainDate = DateTime.Now.AddHours(level.Hours);
                }
                else
                {
                    deliveryWindowInfo.RetainDate = DateTime.Now.AddDays(level.Hours);
                }
                deliveryWindowInfo.RetainTime = deliveryWindowInfo.RetainDate.ToString(Resource.constFormat12HourTime);


                var retainHoursStart = level.Hours - Math.Max(forecastingSettings.RetainTimeBuffer, forecastingSettings.StartBuffer);
                if (forecastingSettings.StartBufferUOM == ((int)RateOfConsumsionUOM.Hours))
                {
                    deliveryWindowInfo.StartDate = DateTime.Now.AddHours(retainHoursStart);
                }
                else
                {
                    deliveryWindowInfo.StartDate = DateTime.Now.AddDays(retainHoursStart);
                }
                deliveryWindowInfo.StartTime = deliveryWindowInfo.StartDate.ToString(Resource.constFormat12HourTime);

                var retainHoursEnd = level.Hours + forecastingSettings.EndBuffer;
                if (forecastingSettings.EndBufferUOM == ((int)RateOfConsumsionUOM.Hours))
                {
                    deliveryWindowInfo.EndDate = DateTime.Now.AddHours(retainHoursEnd);
                }
                else
                {
                    deliveryWindowInfo.EndDate = DateTime.Now.AddDays(retainHoursEnd);
                }
                deliveryWindowInfo.EndTime = deliveryWindowInfo.EndDate.ToString(Resource.constFormat12HourTime);
            }
            return deliveryWindowInfo;
        }

        public async Task<List<DeliveryRequestViewModel>> GetOttoDeliveryRequests(int companyId, string regionId, DateTime startDate, DateTime endDate)
        {
            var response = new List<DeliveryRequestViewModel>();
            if (!string.IsNullOrWhiteSpace(regionId) && !string.IsNullOrEmpty(regionId))
            {
                var drStartDate = startDate.Date;
                var drEndDate = endDate.Date.AddDays(1);
                var filter = Builders<DeliveryRequest>.Filter.And(
                    Builders<DeliveryRequest>.Filter.Where(t => t.TfxAssignedToCompanyId == companyId && t.IsActive),
                    Builders<DeliveryRequest>.Filter.Where(t => t.Status != DeliveryReqStatus.ScheduleCreated && t.Status != DeliveryReqStatus.Draft),
                    Builders<DeliveryRequest>.Filter.Where(t => t.DelReqSource == DRSource.Forecasting || t.DelReqSource == DRSource.OttoForecasting),
                    Builders<DeliveryRequest>.Filter.Where(t => t.DeliveryWindowInfo != null && t.DeliveryWindowInfo.StartDate >= drStartDate && t.DeliveryWindowInfo.EndDate < drEndDate),
                    Builders<DeliveryRequest>.Filter.Where(t => t.TfxAssignedToRegionId == regionId && (t.DeliveryRequestType == 0 || t.DeliveryRequestType == 1)),
                    Builders<DeliveryRequest>.Filter.Where(t => (t.CarrierStatus == (int)BrokeredDrCarrierStatus.None || t.CarrierStatus == (int)BrokeredDrCarrierStatus.Recalled) // Supplier
                                                             || (t.BrokeredParentId != null && t.CarrierStatus == (int)BrokeredDrCarrierStatus.Accepted)) // Carrier
                );

                var deliveryRequests = await mdbContext.DeliveryRequests.FindAsync(filter);
                var deliveryRequestList = await deliveryRequests.ToListAsync();
                deliveryRequestList.ForEach(dr => response.Add(dr.ToDeliveryRequestViewModel()));
                response = response.Where(t => t.DeliveryWindowInfo != null
                            && t.DeliveryWindowInfo.StartDate.Add(DateTime.Parse(t.DeliveryWindowInfo.StartTime).TimeOfDay) >= startDate
                            && t.DeliveryWindowInfo.EndDate.Add(DateTime.Parse(t.DeliveryWindowInfo.EndTime).TimeOfDay) <= endDate).ToList();
                foreach (var dr in response)
                {
                    if (dr.ScheduleQuantityType != 0)
                    {
                        dr.ScheduleQuantityTypeText = GetSchduleQtyTypeText(dr.ScheduleQuantityType);
                    }
                }
            }
            return response;
        }

        public async Task<int> GetShiftSlotPeriod(string regionId)
        {
            var objRegionId = ObjectId.Parse(regionId);
            var filter = Builders<Region>.Filter.Where(t => t.Id == objRegionId);
            int slotPeriod = await mdbContext.Regions.Find(filter).Project(t => t.SlotPeriod).FirstOrDefaultAsync();
            return slotPeriod;
        }
        private string FormatOttoNotification(DeliveryRequest delivery, DeliveryWindowInfo deliveryWindowInfo, string ShiftInfo, string RowInfo, string scheduleDate)
        {
            string UOM = "L";
            if (delivery.TfxUoM == (int)UoM.Gallons)
            {
                UOM = "G";
            }
            string message = string.Empty;
            message = "<h2 class='fs14'>";
            message = message + delivery.TfxJobName + " | " + delivery.TfxCustomerCompany + " - " + delivery.TfxProductType + ", " + delivery.RequiredQuantity.GetCommaSeperatedValue() + " " + UOM + "<br/>";
            message = message + "<small>" + "Schedule Date: " + scheduleDate + "</small>";
            message = message + "<small>" + "(" + ShiftInfo + ")" + " " + RowInfo + "</small>";
            message = message + "</h2>";
            message = message + "<label class='mb0'>Modified delivery window</label>";
            message = message + "<h3 class='fs12 mb0'>";
            message = message + deliveryWindowInfo.StartDate.ToString(Resource.constFormatDate) + "(" + deliveryWindowInfo.StartTime + ")";
            message = message + " - " + deliveryWindowInfo.EndDate.ToString(Resource.constFormatDate) + "(" + deliveryWindowInfo.EndTime + ")";
            message = message + "</h3>";
            return message;
        }
        private async Task ProcessScheduleBuilder(string SiteId, string TankId, string StorageId, int TfxJobId, List<ObjectId> deliveryReqsId)
        {
            var ottoscheduleInfo = mdbContext.OttoScheduleInfos.Find(top => top.SiteId == SiteId && top.TankId == TankId && top.StorageId == StorageId && top.TfxJobId == TfxJobId && top.IsActive).Project(x => new { x.Id, x.ScheduleBuilderId, x.ShiftInfo, x.DeliveryRequestInfo }).FirstOrDefault();
            if (ottoscheduleInfo != null)
            {
                var delfilter = Builders<DeliveryRequest>.Filter.And(
                           Builders<DeliveryRequest>.Filter.Where(x => x.IsActive && !x.IsDeleted && x.DeliveryWindowInfo != null),
                           Builders<DeliveryRequest>.Filter.In(x => x.Id, deliveryReqsId));

                var deliveryRequestDetails = mdbContext.DeliveryRequests.Find(delfilter).ToList();

                foreach (var item in deliveryRequestDetails)
                {
                    var deliveryUpdateInfo = ottoscheduleInfo.DeliveryRequestInfo;
                    var scheduleBuilderInfo = item.DeliveryWindowInfo;
                    if (deliveryUpdateInfo != null)
                    {
                        await UpdateOttoDeliveryRequestInfo(item, deliveryUpdateInfo);
                    }
                    if (scheduleBuilderInfo != null)
                    {
                        //get schedule builder details  based on window start date.
                        ScheduleBuilderRepository scheduleBuilderDomain = new ScheduleBuilderRepository(mdbContext);
                        var scheduleBuilder = await scheduleBuilderDomain.GetScheduleBuilderDetails(item.TfxAssignedToCompanyId, item.TfxAssignedToUserId
                                        , item.TfxAssignedToRegionId,
                                        item.DeliveryWindowInfo.StartDate.ToString(Resource.constFormatDate), 0, string.Empty);
                        if (scheduleBuilder.StatusCode == (int)Status.Success)
                        {
                            var dcReqDetails = await mdbContext.DeliveryRequests.Find(top => top.Id == item.Id).FirstOrDefaultAsync();
                            if (dcReqDetails != null)
                            {
                                var delReqViewModel = dcReqDetails.ToDeliveryRequestViewModel();
                                bool shiftInfoAssign = AssignShiftInformation(scheduleBuilder, ottoscheduleInfo.Id.ToString(), ottoscheduleInfo.ShiftInfo, delReqViewModel);
                                //verfiy that delivery request assign particular shift. if yes then we save the schedule builder.
                                if (shiftInfoAssign)
                                {
                                    await SaveScheduleBuilder(item, scheduleBuilderDomain, scheduleBuilder);
                                }
                            }
                        }
                    }
                }
            }
        }

        private async Task SaveScheduleBuilder(DeliveryRequest item, ScheduleBuilderRepository scheduleBuilderDomain, ScheduleBuilderViewModel scheduleBuilder)
        {
            var dsbSaveModel = scheduleBuilder.ToDsbSaveModel();
            dsbSaveModel.Trips = new List<TripViewModel>();
            dsbSaveModel.Trips.AddRange(scheduleBuilder.Trips);
            DSBSaveModel response = dsbSaveModel;
            try
            {
                List<DSBSaveModel> scheduleBuilders = await ProcessAcrossTheDateDrsEdit(dsbSaveModel);
                var saveResponse = await scheduleBuilderDomain.SaveScheduleBuilder(scheduleBuilders);
                if (saveResponse != null && saveResponse.Any())
                {
                    response = saveResponse.First();
                    if (response.StatusCode == (int)Status.Success)
                    {
                        LogManager.Logger.WriteInfo($"OttoSchedule.Workflow, ProcessScheduleBuilderAsync, OttOSchedule .Created for Region : {item.TfxAssignedToRegionId}", "Main", "End");
                    }
                    else
                    {
                        LogManager.Logger.WriteError($"OttoSchedule.Workflow, ProcessScheduleBuilderAsync, OttOSchedule .Failed Created for Region : {item.TfxAssignedToRegionId + ":" + response.StatusMessage}", "Main", "End");
                    }
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = (int)Status.Failed;
                LogManager.Logger.WriteException("DeliveryRequestRepository", "ProcessScheduleBuilderAsync-OttoSchedule", ex.Message, ex);
            }
        }

        private async Task UpdateOttoDeliveryRequestInfo(DeliveryRequest item, OttoDeliveryRequestInfo deliveryUpdateInfo)
        {
            var delInfilter = Builders<DeliveryRequest>.Filter.Where(t => t.Id == item.Id);
            if (deliveryUpdateInfo.PickupLocationType == PickupLocationType.BulkPlant)
            {
                var update = Builders<DeliveryRequest>.Update
                            .Set(t => t.TfxBulkPlant, deliveryUpdateInfo.TfxBulkPlant)
                            .Set(t => t.RouteInfo, deliveryUpdateInfo.RouteInfo)
                            .Set(t => t.BadgeNo1, deliveryUpdateInfo.BadgeNo1)
                            .Set(t => t.BadgeNo2, deliveryUpdateInfo.BadgeNo2)
                            .Set(t => t.BadgeNo3, deliveryUpdateInfo.BadgeNo3)
                            .Set(t => t.IsCommonBadge, deliveryUpdateInfo.IsCommonBadge)
                            .Set(t => t.DispactherNote, deliveryUpdateInfo.DispactherNote)
                            .Set(t => t.Notes, deliveryUpdateInfo.Notes);
                await mdbContext.DeliveryRequests.UpdateOneAsync(delInfilter, update);
            }
            else
            {
                var TfxTerminal = new MdbDataAccess.Collections.DropdownDisplayItem()
                {
                    Id = deliveryUpdateInfo.TfxTerminal.Id,
                    Name = deliveryUpdateInfo.TfxTerminal.Name
                };
                var update = Builders<DeliveryRequest>.Update
                             .Set(t => t.TfxTerminal, TfxTerminal)
                             .Set(t => t.RouteInfo, deliveryUpdateInfo.RouteInfo)
                             .Set(t => t.BadgeNo1, deliveryUpdateInfo.BadgeNo1)
                             .Set(t => t.BadgeNo2, deliveryUpdateInfo.BadgeNo2)
                             .Set(t => t.BadgeNo3, deliveryUpdateInfo.BadgeNo3)
                             .Set(t => t.IsCommonBadge, deliveryUpdateInfo.IsCommonBadge)
                             .Set(t => t.DispactherNote, deliveryUpdateInfo.DispactherNote)
                             .Set(t => t.Notes, deliveryUpdateInfo.Notes);
                await mdbContext.DeliveryRequests.UpdateOneAsync(delInfilter, update);
            }
        }

        private static bool AssignShiftInformation(ScheduleBuilderViewModel scheduleBuilder, string ottoScheduleId, OttoShiftInfo shiftInfo, DeliveryRequestViewModel delData)
        {
            bool shiftInfoExists = false;
            var shiftTripDetails = scheduleBuilder.Trips.Where(top => (top.DriverColIndex) == shiftInfo.DriverColIndex
                      && top.ShiftId == shiftInfo.ShiftId && (top.DriverRowIndex) == shiftInfo.DriverRowIndex
                      && (top.ShiftIndex) == shiftInfo.ShiftIndex).FirstOrDefault();
            if (shiftTripDetails != null)
            {
                if (shiftTripDetails.DeliveryGroupPrevStatus == DeliveryGroupStatus.Published)
                {
                    shiftTripDetails.TripStatus = TripStatus.Modified;
                    shiftTripDetails.DeliveryGroupStatus = DeliveryGroupStatus.Published;
                    delData.Status = DeliveryReqStatus.ScheduleCreated;
                    shiftTripDetails.DeliveryRequests.Add(delData);
                    string message = "Otto Schedule ID :" + ottoScheduleId + "Delivery ID :" + delData.Id;
                    LogManager.Logger.WriteInfo("ProcessOttoSchedule", "ProcessOttoSchedule-Published", message);
                }
                else
                {
                    shiftTripDetails.TripStatus = TripStatus.Modified;
                    shiftTripDetails.DeliveryGroupStatus = DeliveryGroupStatus.Draft;
                    shiftTripDetails.DeliveryGroupPrevStatus = DeliveryGroupStatus.None;
                    delData.Status = DeliveryReqStatus.Draft;
                    delData.ScheduleStatus = 14;
                    shiftTripDetails.DeliveryRequests.Add(delData);
                    string message = "Otto Schedule ID :" + ottoScheduleId + "Delivery ID :" + delData.Id;
                    LogManager.Logger.WriteInfo("ProcessOttoSchedule", "ProcessOttoSchedule-Saved", message);
                }
                shiftInfoExists = true;
            }
            return shiftInfoExists;
        }
        private async Task<List<DSBSaveModel>> ProcessAcrossTheDateDrsEdit(DSBSaveModel scheduleBuilderViewModel)
        {
            var scheduleBuilders = new List<DSBSaveModel>() { scheduleBuilderViewModel };
            var preloadedDrs = scheduleBuilderViewModel.Trips.SelectMany(t1 => t1.DeliveryRequests
                                    .Where(t2 => !string.IsNullOrWhiteSpace(t2.PreLoadedFor) && t2.PostLoadInfo == null)).ToList();
            if (preloadedDrs.Any())
            {
                var postloadedDrIds = preloadedDrs.Select(t => t.PreLoadedFor).ToList();
                var scheduleBuilderRepository = new ScheduleBuilderRepository(mdbContext);
                var scheduleBuildersForEditDrs = await scheduleBuilderRepository.GetScheduleBuildersByDrIds(postloadedDrIds);
                if (scheduleBuildersForEditDrs != null && scheduleBuildersForEditDrs.Any())
                {
                    EditDrsInAcrossTheDateScheduleBuilders(scheduleBuilders, preloadedDrs, postloadedDrIds, scheduleBuildersForEditDrs);
                }
            }
            return scheduleBuilders;
        }

        private static void EditDrsInAcrossTheDateScheduleBuilders(List<DSBSaveModel> scheduleBuilders, List<DeliveryRequestViewModel> preloadedDrs, List<string> deliveryRequestIds, List<ScheduleBuilderViewModel> scheduleBuildersForDeleteDrs)
        {
            foreach (var drId in deliveryRequestIds)
            {
                var dsbForDeleteTrip = scheduleBuildersForDeleteDrs.Where(t1 => t1.Trips.Any(t2 => t2.DeliveryRequests.Any(t3 => t3.Id == drId))).FirstOrDefault();
                var dsbSaveModel = scheduleBuilders.Where(t1 => t1.Trips.Any(t2 => t2.DeliveryRequests.Any(t3 => t3.Id == drId))).FirstOrDefault();
                if (dsbSaveModel == null)
                {
                    if (dsbForDeleteTrip != null)
                    {
                        dsbSaveModel = dsbForDeleteTrip.ToDsbSaveModel();
                        dsbSaveModel.Trips = new List<TripViewModel>();
                    }
                }
                if (dsbSaveModel != null && dsbForDeleteTrip != null)
                {
                    var tripModel = dsbSaveModel.Trips.Where(t1 => t1.DeliveryRequests.Any(t2 => t2.Id == drId)).FirstOrDefault();
                    if (tripModel == null)
                    {
                        tripModel = dsbForDeleteTrip.Trips.Where(t1 => t1.DeliveryRequests.Any(t2 => t2.Id == drId)).FirstOrDefault();
                        if (tripModel != null)
                        {
                            dsbSaveModel.Trips.Add(tripModel);
                        }
                    }
                    if (tripModel != null && tripModel.DeliveryRequests.Any(t => t.Id == drId))
                    {
                        var modifiedPreloadedDr = preloadedDrs.First(t => t.PreLoadedFor == drId);
                        var modifiedPostloadedDr = tripModel.DeliveryRequests.First(t => t.Id == drId);
                        modifiedPostloadedDr.UpdateModifiedPostLoadedDrValues(modifiedPreloadedDr);
                        tripModel.TripStatus = TripStatus.Modified;
                        if (tripModel.DeliveryGroupPrevStatus == DeliveryGroupStatus.Published)
                        {
                            tripModel.DeliveryGroupStatus = DeliveryGroupStatus.Published;
                        }
                    }
                    if (!scheduleBuilders.Any(t => t.Id == dsbSaveModel.Id))
                    {
                        scheduleBuilders.Add(dsbSaveModel);
                    }
                }
            }
        }
        public async Task<DeliveryRequestsViewModel> CreateSplitDeliveryRequests(SplitDeliveryRequestModel model)
        {
            var response = new DeliveryRequestsViewModel();
            List<DeliveryRequestViewModel> deliveryInfo = new List<DeliveryRequestViewModel>();
            try
            {
                if (model != null && !string.IsNullOrEmpty(model.ParentDRId) && model.RequiredQtyDetails.Count() > 0)
                {

                    ObjectId objectId = ObjectId.Empty;
                    bool valid = ObjectId.TryParse(model.ParentDRId, out objectId);
                    if (valid)
                    {
                        var parentDeliveryRequests = mdbContext.DeliveryRequests.Find(top => top.Id == objectId).FirstOrDefault();
                        if (parentDeliveryRequests != null)
                        {
                            if (parentDeliveryRequests.Status == DeliveryReqStatus.ScheduleCreated || parentDeliveryRequests.Status == DeliveryReqStatus.Draft)
                            {
                                response.StatusCode = (int)Status.Failed;
                                response.StatusMessage = Resource.ValParentDRIsInLoad;
                                return response;
                            }
                            using (var session = await mdbContext.Client.StartSessionAsync())
                            {
                                session.StartTransaction();
                                try
                                {
                                    foreach (var item in model.RequiredQtyDetails)
                                    {
                                        DeliveryRequestViewModel deliveryRequestViewModel = new DeliveryRequestViewModel();
                                        deliveryRequestViewModel = parentDeliveryRequests.ToDeliveryRequestViewModel();
                                        deliveryRequestViewModel.GroupParentDRId = model.ParentDRId;
                                        deliveryRequestViewModel.RequiredQuantity = item.RequiredQty;
                                        deliveryRequestViewModel.DeliveryLevelPO = item.DeliveryLevelPO;
                                        deliveryRequestViewModel.UniqueOrderNo = item.UniqueOrderNo;
                                        deliveryRequestViewModel.CreatedOn = deliveryRequestViewModel.UpdatedOn = DateTimeOffset.Now;
                                        deliveryRequestViewModel.CreatedBy = deliveryRequestViewModel.UpdatedBy = model.UserId;
                                        deliveryRequestViewModel.IsActive = true;
                                        deliveryRequestViewModel.IsDeleted = false;
                                        deliveryInfo.Add(deliveryRequestViewModel);
                                    }
                                    var deliveryRequests = deliveryInfo.ToEntity();
                                    await mdbContext.DeliveryRequests.InsertManyAsync(deliveryRequests);
                                    foreach (var item in deliveryRequests)
                                    {
                                        response.EntityIds.Add(item.Id.ToString());
                                    }
                                    //update ChildDRs collection.
                                    await UpdateParentChildDRsCollection(objectId, deliveryRequests);
                                    response.DeliveryRequests = deliveryInfo;
                                    response.StatusCode = (int)Status.Success;
                                    await session.CommitTransactionAsync();
                                }
                                catch (Exception)
                                {
                                    await session.AbortTransactionAsync();
                                    response.StatusCode = (int)Status.Failed;
                                    throw;
                                }
                            }
                        }
                        else
                        {
                            response.StatusCode = (int)Status.Warning;
                            response.StatusMessage = Resource.msgDelReqCreationFailed;
                        }
                    }
                    else
                    {
                        response.StatusCode = (int)Status.Warning;
                        response.StatusMessage = Resource.msgDelReqCreationFailed;
                    }

                }
                else
                {
                    response.StatusCode = (int)Status.Warning;
                    response.StatusMessage = Resource.msgDelReqCreationFailed;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }

        public async Task<DeliveryRequestsViewModel> CreateSplitBlendDeliveryRequests(List<SplitDrArray> splitDrArray)
        {
            var response = new DeliveryRequestsViewModel();
            List<DeliveryRequestViewModel> deliveryInfo = new List<DeliveryRequestViewModel>();
            try
            {
                if (splitDrArray != null && splitDrArray.SelectMany(t => t.BlendDrArray).All(t => !string.IsNullOrEmpty(t.ParentDrId) && t.RequiredQuantity > 0))
                {
                    using (var session = await mdbContext.Client.StartSessionAsync())
                    {
                        session.StartTransaction();
                        try
                        {
                            ObjectId objectId = ObjectId.Empty;
                            List<ObjectId> parentIds = splitDrArray.SelectMany(t => t.BlendDrArray.Select(t1 => t1.ParentDrId)).Select(t => ObjectId.Parse(t)).ToList();

                            var parentDeliveryRequests = mdbContext.DeliveryRequests.Find(top => parentIds.Contains(top.Id)).ToList();
                            if (parentDeliveryRequests != null)
                            {
                                foreach (var blendDr in splitDrArray)
                                {
                                    foreach (var item in blendDr.BlendDrArray)
                                    {
                                        var dr = parentDeliveryRequests.Where(t => t.Id.ToString() == item.ParentDrId).FirstOrDefault();
                                        DeliveryRequestViewModel deliveryRequestViewModel = new DeliveryRequestViewModel();
                                        deliveryRequestViewModel = dr.ToDeliveryRequestViewModel();
                                        deliveryRequestViewModel.GroupParentDRId = item.ParentDrId;
                                        deliveryRequestViewModel.RequiredQuantity = item.RequiredQuantity;
                                        deliveryRequestViewModel.BlendedGroupId = blendDr.BlendGroupId;
                                        deliveryRequestViewModel.IsActive = true;
                                        deliveryRequestViewModel.IsDeleted = false;
                                        deliveryInfo.Add(deliveryRequestViewModel);
                                    }
                                }
                                var deliveryRequests = deliveryInfo.ToEntity();
                                await mdbContext.DeliveryRequests.InsertManyAsync(deliveryRequests);
                                foreach (var item in deliveryRequests)
                                {
                                    response.EntityIds.Add(item.Id.ToString());
                                }
                                //update ChildDRs collection.
                                foreach (var split in deliveryRequests.GroupBy(t => t.GroupParentDRId))
                                {
                                    await UpdateParentChildDRsCollection(split.Key.Value, split.ToList());
                                }
                                response.DeliveryRequests = deliveryInfo;
                                response.StatusCode = (int)Status.Success;
                            }
                            else
                            {
                                response.StatusCode = (int)Status.Warning;
                                response.StatusMessage = Resource.msgDelReqCreationFailed;
                            }

                            await session.CommitTransactionAsync();
                        }
                        catch (Exception)
                        {
                            await session.AbortTransactionAsync();
                            response.StatusCode = (int)Status.Failed;
                            throw;
                        }
                    }
                }
                else
                {
                    response.StatusCode = (int)Status.Warning;
                    response.StatusMessage = Resource.msgDelReqCreationFailed;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return response;
        }

        private async Task UpdateParentChildDRsCollection(ObjectId objectId, List<DeliveryRequest> deliveryRequests)
        {
            List<ObjectId> childDRsobjectIds = new List<ObjectId>();
            decimal updatedParentDrQty = 0;
            deliveryRequests.ForEach(x =>
            {
                childDRsobjectIds.Add(x.Id);
                updatedParentDrQty += x.RequiredQuantity;
            });
            var filter = Builders<DeliveryRequest>.Filter.Where(t => t.Id == objectId);
            var update = Builders<DeliveryRequest>.Update
                        .Set(t => t.GroupChildDRs, childDRsobjectIds)
                        .Set(t => t.RequiredQuantity, updatedParentDrQty);

            await mdbContext.DeliveryRequests.UpdateOneAsync(filter, update);
        }



        public async Task<DRReportFilterViewModel> GetDRReportDropDownFilters(int userId)
        {
            var response = new DRReportFilterViewModel();
            try
            {
                var regionList = await mdbContext.Regions.Find(t => t.IsActive && !t.IsDeleted && t.TfxDispatchers.Any(t1 => t1.Id == userId)).Project(t => new { t.Id, t.Name, t.TfxJobs }).SortBy(t => t.Name).ToListAsync();
                if (regionList != null && regionList.Any())
                {
                    foreach (var region in regionList)
                    {
                        // set all locations in response  
                        if (region.TfxJobs != null && region.TfxJobs.Any())
                        {
                            region.TfxJobs.ForEach(t => response.Locations.Add(new FreightModels.DropdownDisplayItem { Id = t.Id, Name = t.Name }));
                        }

                        response.Regions.Add(new DropdownDisplayExtended { Id = region.Id.ToString(), Name = region.Name });
                    }
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryRequestRepository", "GetDRReportDropDownFilters", ex.Message, ex);
                throw;
            }
            return response;

        }

        public async Task<List<DeliveryRequestReportGridViewModel>> GetAllDeliveryRequests(DRReportFilterInputViewModel inputData)
        {
            var response = new List<DeliveryRequestReportGridViewModel>();
            try
            {
                if (inputData != null)
                {
                    if (!string.IsNullOrWhiteSpace(inputData.RegionIds) && !string.IsNullOrEmpty(inputData.LocationIds))
                    {
                        char[] separator = { ',' };
                        var regIds = inputData.RegionIds.Split(separator).ToList();
                        var StrlocIds = inputData.LocationIds.Split(separator).ToList();
                        var locIds = new List<int>();
                        StrlocIds.ForEach(t => locIds.Add(Convert.ToInt32(t)));
                        var deliveryRequests = await mdbContext.DeliveryRequests.Find(dr => (
                                                          (dr.TfxAssignedToCompanyId == inputData.CompanyId)
                                                          ||
                                                          (dr.TfxCreatedByCompanyId == inputData.CompanyId)
                                                          )
                                                              && dr.Status != DeliveryReqStatus.ScheduleCreated
                                                              && dr.Status != DeliveryReqStatus.Draft
                                                              && !dr.IsDeleted
                                                              && dr.IsActive
                                                              && dr.Priority == DeliveryReqPriority.MustGo
                                                              &&
                                                              (
                                                                  (dr.CarrierStatus == (int)BrokeredDrCarrierStatus.None || dr.CarrierStatus == (int)BrokeredDrCarrierStatus.Recalled) // Supplier
                                                                  ||
                                                                  (dr.BrokeredParentId != null && dr.CarrierStatus == (int)BrokeredDrCarrierStatus.Accepted) // Carrier
                                                              )
                                                              && (regIds.Contains(dr.TfxAssignedToRegionId))
                                                              && (locIds.Contains(dr.TfxJobId))
                                                              && (dr.DeliveryRequestType == 0 || dr.DeliveryRequestType == 1)
                                                              && (dr.ParentId == null)
                                                              && dr.GroupChildDRs.Count() == 0
                                                ).SortBy(x => x.CurrentThreshold).ToListAsync();

                        deliveryRequests.ForEach(dr => response.Add(dr.ToDeliveryRequestReportGridViewModel()));

                        if (response != null && response.Any())
                        {
                            var distinctRegIds = regIds.Distinct().ToList();
                            var regObjectIDs = distinctRegIds.Select(t => new ObjectId(t)).ToList();
                            var RegionInfo = mdbContext.Regions.Find(t => regObjectIDs.Contains(t.Id))
                                            .Project(t1 =>
                                            new
                                            {
                                                Id = t1.Id.ToString(),
                                                RegName = t1.Name
                                            }).ToList();
                            foreach (var regId in distinctRegIds)
                            {
                                var regInfo = RegionInfo.Where(t => t.Id == regId).FirstOrDefault();
                                if (regInfo != null)
                                {
                                    var setRegionNameInfo = response.FindAll(t => t.RegionId == regId).ToList();
                                    if (setRegionNameInfo != null && setRegionNameInfo.Any())
                                    {
                                        setRegionNameInfo.ForEach(t => t.RegionName = regInfo.RegName);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryRequestRepository", "GetAllDeliveryRequests", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusModel> UpdateBrokerDeliveryRequestInfo(BrokeredDeliveryRequestInput model)
        {
            var response = new DeliveryRequestsViewModel();
            bool isSessionStarted = false;
            using (var session = await mdbContext.Client.StartSessionAsync())
            {
                try
                {
                    ObjectId objectIdForDr = ObjectId.Empty;
                    ObjectId.TryParse(model.Id, out objectIdForDr);

                    var existingDr = await mdbContext.DeliveryRequests.Find(t => t.Id == objectIdForDr && t.IsActive && !t.IsDeleted).FirstOrDefaultAsync();
                    if (existingDr != null)
                    {
                        var updateFields = Builders<DeliveryRequest>.Update
                          .Set(t => t.TfxOrderId, model.OrderId)
                          .Set(t => t.TfxAssignedToCompanyId, model.AssignedToCompanyId)
                          .Set(t => t.TfxAssignedToRegionId, model.AssignedToRegionId)
                          .Set(t => t.AssignedOn, DateTimeOffset.Now);

                        var filter = Builders<DeliveryRequest>.Filter.And(
                                Builders<DeliveryRequest>.Filter.Where(x => x.Id == objectIdForDr)
                            );
                        session.StartTransaction();
                        isSessionStarted = true;
                        await mdbContext.DeliveryRequests.UpdateOneAsync(filter, updateFields);
                        await session.CommitTransactionAsync();
                        response.StatusCode = (int)Status.Success;
                    }
                }
                catch (Exception)
                {
                    if (isSessionStarted)
                    {
                        await session.AbortTransactionAsync();
                    }
                    response.StatusCode = (int)Status.Failed;
                    throw;
                }
            }
            return response;
        }
        public async Task<StatusModel> UpdateSpiltDRs(SpiltDeliveryRequestViewModel model)
        {
            var response = new StatusModel();
            List<DeliveryRequestViewModel> deliveryRequestViewModel = new List<DeliveryRequestViewModel>();
            bool isSessionStarted = false;
            if (!string.IsNullOrEmpty(model.GroupParentDrId) && model.SpiltDRsViewModel != null && model.SpiltDRsViewModel.Any())
            {
                using (var session = await mdbContext.Client.StartSessionAsync())
                {
                    try
                    {
                        isSessionStarted = true;
                        session.StartTransaction();
                        ObjectId objectIdForDr = ObjectId.Empty;
                        ObjectId.TryParse(model.GroupParentDrId, out objectIdForDr);
                        var existingDr = await mdbContext.DeliveryRequests.Find(t => t.Id == objectIdForDr && t.IsActive && !t.IsDeleted).FirstOrDefaultAsync();
                        if (existingDr != null)
                        {
                            var deliveryRequests = new DeliveryRequestsViewModel();
                            foreach (var dr in model.SpiltDRsViewModel)
                            {
                                var newDeliveryRequest = existingDr.ToEntity();
                                newDeliveryRequest.RequiredQuantity = dr.RequiredQuantity;
                                newDeliveryRequest.ScheduleQuantityType = dr.ScheduleQuantityType;
                                newDeliveryRequest.ParentId = null;
                                newDeliveryRequest.GroupParentDRId = ObjectId.Parse(model.GroupParentDrId);
                                newDeliveryRequest.CreatedBy = newDeliveryRequest.UpdatedBy = model.UserId;
                                newDeliveryRequest.DelReqSource = existingDr.DelReqSource;
                                await mdbContext.DeliveryRequests.InsertOneAsync(newDeliveryRequest);
                                deliveryRequests.EntityIds.Add(newDeliveryRequest.Id.ToString());
                            }
                            await UpdateParentSpiltDRInfo(model.SpiltDRsViewModel.Sum(t => t.RequiredQuantity), deliveryRequests, existingDr);
                        }
                        await session.CommitTransactionAsync();
                        response.StatusCode = (int)Status.Success;
                        response.StatusMessage = Resource.msgDelReqCreationSuccess;
                    }
                    catch (Exception)
                    {
                        if (isSessionStarted)
                        {
                            await session.AbortTransactionAsync();
                        }
                        response.StatusCode = (int)Status.Failed;
                        throw;
                    }
                }
            }
            return response;
        }

        private static decimal IntializeSpiltDRsModel(SpiltDeliveryRequestViewModel model, List<DeliveryRequestViewModel> deliveryRequestViewModel, decimal spiltDRsQuantity, DeliveryRequest existingDr)
        {
            foreach (var item in model.SpiltDRsViewModel)
            {
                var drCreate = existingDr.ToDeliveryRequestViewModel();
                drCreate.RequiredQuantity = item.RequiredQuantity;
                if (item.ScheduleQuantityType > 1)
                {
                    drCreate.ScheduleQuantityTypeText = EnumHelperMethods.GetDisplayName((ScheduleQuantityType)item.ScheduleQuantityType);
                }
                else
                {
                    drCreate.ScheduleQuantityTypeText = string.Empty;
                }
                drCreate.ScheduleQuantityType = item.ScheduleQuantityType;
                spiltDRsQuantity += item.RequiredQuantity;
                deliveryRequestViewModel.Add(drCreate);
            }

            return spiltDRsQuantity;
        }

        private async Task UpdateParentSpiltDRInfo(decimal spiltDRsQuantity, DeliveryRequestsViewModel createSubDRsStatus, DeliveryRequest groupParentDRs)
        {
            groupParentDRs.RequiredQuantity = spiltDRsQuantity > 0 ? groupParentDRs.RequiredQuantity + spiltDRsQuantity : groupParentDRs.RequiredQuantity;
            createSubDRsStatus.EntityIds.ForEach(x =>
            {
                groupParentDRs.GroupChildDRs.Add(ObjectId.Parse(x));
            });

            var updateFields = Builders<DeliveryRequest>.Update
                          .Set(t => t.GroupChildDRs, groupParentDRs.GroupChildDRs)
                          .Set(t => t.RequiredQuantity, groupParentDRs.RequiredQuantity)
                          .Set(t => t.UpdatedOn, DateTimeOffset.Now);

            var filter = Builders<DeliveryRequest>.Filter.And(
                    Builders<DeliveryRequest>.Filter.Where(x => x.Id == groupParentDRs.Id)
                );


            await mdbContext.DeliveryRequests.UpdateOneAsync(filter, updateFields);
        }
        public async Task<List<CancelDeliverySchedule>> UpdateDeliveryRequestCancelStatus(List<DeliveryReqCancelScheduleUpdateModel> delReqs)
        {
            bool isSessionStarted = false;
            List<CancelDeliverySchedule> cancelDeliverySchedules = new List<CancelDeliverySchedule>();
            List<int> enrouteInCompleted = new List<int> { 3, 15, 23 };
            List<int> enrouteInProgress = new List<int> { 1, 3, 11, 12, 13, 14, 15, 16, 17, 18 };
            if (delReqs != null && delReqs.Any())
            {
                using (var session = await mdbContext.Client.StartSessionAsync())
                {
                    try
                    {
                        isSessionStarted = true;
                        session.StartTransaction();

                        var modifiedRequests = delReqs.Select(t => ObjectId.Parse(t.DeliveryReqId)).ToList();
                        var cancellDRsList = await mdbContext.DeliveryRequests.Find(x => modifiedRequests.Contains(x.Id) && (enrouteInProgress.Contains(x.TfxScheduleEnrouteStatus) || enrouteInCompleted.Contains(x.TfxScheduleStatus))).Project(x1 => x1.Id).ToListAsync();
                        var scheduleBuilderIds = delReqs.Select(x => ObjectId.Parse(x.ScheduleBuilderId)).Distinct().ToList();
                        var scheduleBuilder = await mdbContext.ScheduleBuilders
                                          .Find(t => t.IsActive && !t.IsDeleted && scheduleBuilderIds.Contains(t.Id)).ToListAsync();
                        if (scheduleBuilder.Any())
                        {
                            foreach (var dr in delReqs)
                            {
                                var tripInfo = scheduleBuilder.Where(x => x.Id == ObjectId.Parse(dr.ScheduleBuilderId)).SelectMany(x1 => x1.Trips).ToList();
                                if (tripInfo.Any())
                                {
                                    var tripDRInfo = tripInfo.FirstOrDefault(x => x.ShiftId == dr.ShiftId && x.DriverRowIndex == dr.DriverRowIndex && x.DriverColIndex == dr.DriverColIndex && x.DeliveryRequests.Contains(ObjectId.Parse(dr.DeliveryReqId)));
                                    if (tripDRInfo != null)
                                    {
                                        var objectIdForDr = ObjectId.Parse(dr.DeliveryReqId);
                                        if (cancellDRsList.IndexOf(objectIdForDr) >= 0)
                                        {
                                            var existingDrs = await mdbContext.DeliveryRequests.Find(t => t.Id == objectIdForDr).ToListAsync();
                                            if (existingDrs != null && existingDrs.Any())
                                            {
                                                UpdateDefinition<DeliveryRequest> updateFields = null;
                                                if (dr.ScheduleStatusId != 0)
                                                {
                                                    updateFields = Builders<DeliveryRequest>.Update
                                                        .Set(t => t.TfxScheduleStatus, dr.ScheduleStatusId)
                                                        .Set(t => t.TfxScheduleStatusName, dr.ScheduleStatusName)
                                                        .Set(t => t.UpdatedBy, dr.UserId);
                                                }
                                                var filter = Builders<DeliveryRequest>.Filter.And(
                                                        Builders<DeliveryRequest>.Filter.Where(x => x.Id == objectIdForDr)
                                                    );

                                                await mdbContext.DeliveryRequests.UpdateOneAsync(filter, updateFields);
                                                cancelDeliverySchedules.Add(new CancelDeliverySchedule { DeliveryReqId = dr.DeliveryReqId, DriverColIndex = dr.DriverColIndex, DriverRowIndex = dr.DriverRowIndex, DriverId = dr.DriverId, ScheduleBuilderId = dr.ScheduleBuilderId, ShiftIndex = dr.ShiftIndex, TrackableScheduleId = dr.TrackableScheduleId, UserId = dr.UserId, ScheduleStatus = (int)DeliveryScheduleStatus.Canceled, StatusClassId = (int)DeliveryScheduleStatus.Canceled, TrackScheduleStatus = (int)DeliveryScheduleStatus.Canceled, TrackScheduleStatusName = DeliveryScheduleStatus.Canceled.ToString() });
                                            }
                                        }
                                    }
                                }

                            }
                        }
                        await session.CommitTransactionAsync();

                    }
                    catch (Exception)
                    {
                        cancelDeliverySchedules = new List<CancelDeliverySchedule>();
                        if (isSessionStarted)
                        {
                            await session.AbortTransactionAsync();
                        }
                        throw;
                    }
                }

            }
            return cancelDeliverySchedules;

        }

        public async Task<List<CancelDSDeliveryScheduleViewModel>> GetSubDRInfoCancelDS(CancelDSDeliveryScheduleInfo delReqs)
        {
            List<CancelDSDeliveryScheduleViewModel> cancelDSDeliverySchedule = new List<CancelDSDeliveryScheduleViewModel>();
            if (delReqs.CancelDSDeliverySchedules != null && delReqs.CancelDSDeliverySchedules.Any())
            {
                var deliveryReqInfo = delReqs.CancelDSDeliverySchedules.Select(x1 => x1.DeliveryReqId).ToList();
                List<ObjectId> objdeliveryReqInfo = new List<ObjectId>();
                deliveryReqInfo.ForEach(x =>
                {
                    if (!string.IsNullOrEmpty(x))
                    {
                        objdeliveryReqInfo.Add(ObjectId.Parse(x));
                    }
                });
                var deliveryRequestInfo = await mdbContext.DeliveryRequests.Find(x => objdeliveryReqInfo.Contains(x.Id)).Project(x1 => new { x1.Id, x1.RequiredQuantity, x1.TfxProductType, x1.TfxScheduleEnrouteStatus, x1.TfxScheduleStatusName, x1.TfxScheduleStatus, x1.TfxTrackableScheduleId, x1.TfxUoM }).ToListAsync();
                DateTime dateFilter = DateTimeOffset.Now.Date;
                List<ScheduleBuilder> scheduleBuilder = await mdbContext.ScheduleBuilders
                                        .Find(t => t.IsActive && !t.IsDeleted && t.TfxCompanyId == delReqs.TfxCompanyId
                                            && (t.RegionId == delReqs.RegionId)
                                            && t.DateFilter >= dateFilter).ToListAsync();
                foreach (var item in scheduleBuilder)
                {
                    foreach (var normalDSInfo in deliveryReqInfo)
                    {
                        var tripsInfo = item.Trips.ToList();
                        foreach (var tripitem in tripsInfo)
                        {
                            var tripDRInfo = tripitem.DeliveryRequests.ToList();
                            foreach (var tripDR in tripDRInfo)
                            {
                                if (tripDR.ToString() == normalDSInfo)
                                {
                                    CancelDSDeliveryScheduleViewModel dSDeliveryScheduleViewModel = new CancelDSDeliveryScheduleViewModel();
                                    if (delReqs.CancelDSDeliverySchedules.Any(x => x.DeliveryReqId == normalDSInfo && x.IsSubDR == false))
                                    {
                                        dSDeliveryScheduleViewModel.IsChecked = true;
                                    }
                                    dSDeliveryScheduleViewModel.ScheduleBuilderDate = item.DateFilter.ToString(Resource.constFormatDate);
                                    dSDeliveryScheduleViewModel.ScheduleBuilderId = item.Id.ToString();
                                    if (tripitem.TfxDrivers.Any())
                                    {
                                        dSDeliveryScheduleViewModel.DriverId = tripitem.TfxDrivers.FirstOrDefault().Id;
                                        dSDeliveryScheduleViewModel.DriverName = tripitem.TfxDrivers.FirstOrDefault().Name;
                                    }
                                    var deliveryReqDetails = deliveryRequestInfo.FirstOrDefault(x1 => x1.Id == tripDR);
                                    if (deliveryReqDetails != null)
                                    {
                                        dSDeliveryScheduleViewModel.Quantity = deliveryReqDetails.RequiredQuantity.ToString() + " " + (UoM)deliveryReqDetails.TfxUoM;
                                        dSDeliveryScheduleViewModel.FuelType = deliveryReqDetails.TfxProductType;
                                        if (deliveryReqDetails.TfxScheduleEnrouteStatus == 0)
                                        {
                                            dSDeliveryScheduleViewModel.CurrentState = deliveryReqDetails.TfxScheduleStatusName;
                                        }
                                        else
                                        {
                                            dSDeliveryScheduleViewModel.CurrentState = EnumHelperMethods.GetDisplayName((EnrouteDeliveryStatus)deliveryReqDetails.TfxScheduleEnrouteStatus);
                                        }
                                        dSDeliveryScheduleViewModel.TrackableScheduleId = deliveryReqDetails.TfxTrackableScheduleId.Value;
                                    }
                                    dSDeliveryScheduleViewModel.ShiftId = tripitem.ShiftId;
                                    dSDeliveryScheduleViewModel.ShiftIndex = tripitem.ShiftIndex.Value;
                                    dSDeliveryScheduleViewModel.DriverRowIndex = tripitem.DriverRowIndex.Value;
                                    dSDeliveryScheduleViewModel.DriverColIndex = tripitem.DriverColIndex.Value;
                                    dSDeliveryScheduleViewModel.DeliveryReqId = tripDR.ToString();
                                    cancelDSDeliverySchedule.Add(dSDeliveryScheduleViewModel);

                                }
                            }
                        }
                    }

                }

            }
            return cancelDSDeliverySchedule;
        }
        public CarrierXDeliveryRequestInfo GetCompanyDeliveryRequestsDetails(List<int> companyId)
        {
            var response = new CarrierXDeliveryRequestInfo();
            if (companyId != null && companyId.Any())
            {
                DateTime dateFilter = DateTimeOffset.Now.Date;
                var deliveryRequests = (from dr in mdbContext.DeliveryRequests.AsQueryable()
                                        where (companyId.Contains(dr.TfxAssignedToCompanyId))
                                                && dr.Status != DeliveryReqStatus.ScheduleCreated
                                                && dr.Status != DeliveryReqStatus.Draft && dr.IsActive
                                                && !dr.IsDeleted
                                                &&
                                                (
                                                    (dr.CarrierStatus == (int)BrokeredDrCarrierStatus.None || dr.CarrierStatus == (int)BrokeredDrCarrierStatus.Recalled) // Supplier
                                                    ||
                                                    (dr.BrokeredParentId != null && dr.CarrierStatus == (int)BrokeredDrCarrierStatus.Accepted) // Carrier
                                                )
                                                && (dr.DeliveryRequestType == 0 || dr.DeliveryRequestType == 1)
                                                && (dr.DelReqSource != DRSource.OttoForecasting || (dr.DelReqSource == DRSource.OttoForecasting && dr.DeliveryWindowInfo != null && dr.DeliveryWindowInfo.StartDate == dateFilter)) && dr.GroupChildDRs.Count() == 0
                                        select dr
                                                ).ToList();
                if (deliveryRequests.Any())
                {
                    deliveryRequests.ForEach(dr => response.DeliveryRequestDetails.Add(dr.ToDeliveryRequestViewModel()));
                }

                var assignedToMeDeliveryRequests = mdbContext.DeliveryRequests.Find(dr => companyId.Contains(dr.TfxAssignedToCompanyId) &&
                                                                                  !companyId.Contains(dr.TfxCreatedByCompanyId) &&
                                                                                  dr.CarrierStatus == (int)BrokeredDrCarrierStatus.Pending &&
                                                                                  (dr.DeliveryRequestType == 0 || dr.DeliveryRequestType == 1) &&
                                                                                  dr.IsActive && !dr.IsDeleted
                                                                                   ).ToList();

                if (assignedToMeDeliveryRequests.Any())
                {
                    assignedToMeDeliveryRequests.ForEach(dr => response.AssignedByMeDeliveryRequestDetails.Add(dr.ToDeliveryRequestViewModel()));
                }
            }
            return response;
        }

        public List<DeliveryRequestViewModel> GetDeliveryRequestDetails(List<int> trackableSCId)
        {
            var response = new List<DeliveryRequestViewModel>();
            if (trackableSCId != null && trackableSCId.Any())
            {
                var deliveryRequests = mdbContext.DeliveryRequests.Find(x => x.TfxTrackableScheduleId != null && trackableSCId.Contains(x.TfxTrackableScheduleId.Value)).ToList();
                if (deliveryRequests.Any())
                {
                    deliveryRequests.ForEach(dr => response.Add(dr.ToDeliveryRequestViewModel()));
                }
            }
            return response;
        }
        public async Task<StatusModel> DeleteDeliveryRequestOnOrderClose(List<int> OrderIds)
        {
            var status = new StatusModel() { StatusCode = (int)Status.Failed };
            bool isSessionStarted = false;

            using (var session = await mdbContext.Client.StartSessionAsync())
            {
                try
                {
                    isSessionStarted = true;
                    session.StartTransaction();

                    //Get all delivery Requests
                    List<int> enrouteInScheduled = new List<int> { 3, 15 };
                    var deliveryRequests = await mdbContext.DeliveryRequests.Find(dr => OrderIds.Contains(dr.TfxOrderId.Value) && dr.TfxOrderId != null && dr.IsActive && !dr.IsDeleted).ToListAsync();
                    var deliveryRequestsInfo = deliveryRequests.Where(dr => dr.Status == DeliveryReqStatus.None || dr.Status == DeliveryReqStatus.Pending || dr.Status == DeliveryReqStatus.Assigned || dr.Status == DeliveryReqStatus.Draft).Select(t => new { t.Id, t.TfxAssignedToRegionId, t.TfxAssignedToCompanyId }).ToList();
                    var scheduleDRInfo = deliveryRequests.Where(dr => dr.Status == DeliveryReqStatus.ScheduleCreated && enrouteInScheduled.Contains(dr.TfxScheduleStatus)).Select(t => new { t.Id, t.TfxAssignedToRegionId, t.TfxAssignedToCompanyId }).ToList();

                    var deliveryRequestIds = deliveryRequestsInfo.Select(x => x.Id).ToList();
                    if (scheduleDRInfo.Any())
                    {
                        deliveryRequestIds.AddRange(scheduleDRInfo.Select(x => x.Id).ToList());
                    }
                    if (deliveryRequestIds.Any())
                    {
                        //delete the delivery requests.
                        var filter = Builders<DeliveryRequest>.Filter.Where(t => deliveryRequestIds.Contains(t.Id));
                        var update = Builders<DeliveryRequest>.Update
                                    .Set(t => t.IsDeleted, true)
                                    .Set(t => t.IsActive, false);
                        await mdbContext.DeliveryRequests.UpdateManyAsync(filter, update);

                        //get schedule builder details.
                        DateTime dateFilter = DateTimeOffset.Now.Date;

                        var RegionIds = deliveryRequests.Select(x => x.TfxAssignedToRegionId).Distinct().ToList();
                        var CompanyIds = deliveryRequests.Select(x => x.TfxAssignedToCompanyId).Distinct().ToList();

                        var scheduleBuilder = await mdbContext.ScheduleBuilders
                                             .Find(t => t.IsActive && !t.IsDeleted && CompanyIds.Contains(t.TfxCompanyId)
                                                 && RegionIds.Contains(t.RegionId)
                                                 && t.DateFilter >= dateFilter).ToListAsync();

                        //remove the DRs from the trip object schedule builder.
                        foreach (var scItem in scheduleBuilder)
                        {
                            bool scheduleUpdate = false;
                            foreach (var tripItem in scItem.Trips)
                            {
                                var tripDRs = tripItem.DeliveryRequests.ToList();
                                var tripMatchingDRs = deliveryRequestIds.Intersect(tripDRs);
                                if (tripMatchingDRs.Any())
                                {
                                    scheduleUpdate = true;
                                    tripItem.DeliveryRequests.RemoveAll(x => tripMatchingDRs.Contains(x));
                                }
                            }
                            if (scheduleUpdate)
                            {
                                var sbfilter = Builders<ScheduleBuilder>.Filter.Where(t => t.Id == scItem.Id);
                                var sbupdate = Builders<ScheduleBuilder>.Update
                                            .Set(t => t.Trips, scItem.Trips);
                                await mdbContext.ScheduleBuilders.UpdateOneAsync(sbfilter, sbupdate);
                            }
                        }

                        //delete Recurring Schedules.
                        var recurringSCfilter = Builders<RecurringSchedules>.Filter.Where(t => OrderIds.Contains(t.OrderId.Value) && t.OrderId != null);
                        var recurringSCupdate = Builders<RecurringSchedules>.Update
                                    .Set(t => t.IsDeleted, true)
                                    .Set(t => t.IsActive, false);
                        await mdbContext.RecurringSchedules.UpdateManyAsync(recurringSCfilter, recurringSCupdate);
                    }
                    await session.CommitTransactionAsync();
                    status.StatusCode = (int)Status.Success;
                }
                catch (Exception)
                {
                    if (isSessionStarted)
                    {
                        await session.AbortTransactionAsync();
                    }
                    status.StatusCode = (int)Status.Failed;
                    throw;
                }
            }
            return status;
        }
        public async Task<StatusModel> ValidateDeliveryRequestInUse(List<string> DeliveryReqIds)
        {
            var status = new StatusModel() { StatusCode = (int)Status.Success };
            List<ObjectId> delRequestObjIds = new List<ObjectId>();
            if (DeliveryReqIds != null && DeliveryReqIds.Any())
            {
                DeliveryReqIds.ForEach(x => delRequestObjIds.Add(ObjectId.Parse(x)));
            }
            if (delRequestObjIds.Any())
            {
                var deliveryRequests = await mdbContext.DeliveryRequests.Find(dr => delRequestObjIds.Contains(dr.Id) && !string.IsNullOrEmpty(dr.TripId) && !string.IsNullOrEmpty(dr.ScheduleBuilderId) && dr.IsActive && !dr.IsDeleted).AnyAsync();
                if (deliveryRequests)
                {
                    status.StatusCode = (int)Status.Failed;
                }
            }
            return status;
        }
        public async Task<List<DeliveryRequestViewModel>> GetBlendedGroupDeliveryRequestDetails(List<string> BlendedGroupIds)
        {
            var response = new List<DeliveryRequestViewModel>();
            if (BlendedGroupIds != null && BlendedGroupIds.Any())
            {
                var deliveryRequests = await mdbContext.DeliveryRequests.Find(dr => BlendedGroupIds.Contains(dr.BlendedGroupId) && dr.IsActive).ToListAsync();

                var drModels = deliveryRequests.Select(t => t.ToDeliveryRequestViewModel()).ToList();
                drModels.ForEach(dr =>
                {
                    response.Add(dr);
                });
            }
            return response;
        }
        public async Task<List<ChildDeliveryRequestInfoViewModel>> GetBlendedChildDeliveryRequestInfo(string blendedGroupId)
        {
            var response = new List<ChildDeliveryRequestInfoViewModel>();
            if (!string.IsNullOrWhiteSpace(blendedGroupId))
            {
                var filter = Builders<DeliveryRequest>.Filter.Where(t => t.BlendedGroupId == blendedGroupId && t.CarrierStatus == (int)BrokeredDrCarrierStatus.Pending &&
                !string.IsNullOrEmpty(t.BrokeredParentId));
                var brokeredDr = await mdbContext.DeliveryRequests.Find(filter).SortByDescending(t => t.CreatedOn)
                                    .Project(t => new { t.Id, t.TfxOrderId, t.BrokeredParentId, t.Status }).ToListAsync();
                if (brokeredDr.All(t => t.Status != DeliveryReqStatus.ScheduleCreated))
                {
                    brokeredDr.ForEach(x =>
                    {
                        response.Add(new ChildDeliveryRequestInfoViewModel { DrId = x.Id.ToString(), BrokeredParentId = x.BrokeredParentId, OrderId = x.TfxOrderId.GetValueOrDefault(), StatusCode = (int)Status.Success });
                    });
                }
            }
            return response;
        }
        public async Task<StatusModel> DeleteDeliveryRequest(List<string> delReqId)
        {
            var response = new StatusModel();
            if (delReqId != null && delReqId.Any())
            {
                try
                {

                    var objectIdsForDr = delReqId.Select(t => ObjectId.Parse(t)).Distinct().ToList();
                    var filter = Builders<DeliveryRequest>.Filter.And(
                                            Builders<DeliveryRequest>.Filter.In(t => t.Id, objectIdsForDr),
                                             Builders<DeliveryRequest>.Filter.Where(t => t.IsActive && !t.IsDeleted));
                    var updateFields = Builders<DeliveryRequest>.Update
                                        .Set(t => t.IsActive, false)
                                        .Set(t => t.UpdatedOn, DateTime.Now)
                                        .Set(t => t.IsDeleted, true);
                    await mdbContext.DeliveryRequests.UpdateManyAsync(filter, updateFields);
                    response.StatusCode = (int)Status.Success;
                }
                catch (Exception ex)
                {
                    response.StatusCode = (int)Status.Failed;
                    LogManager.Logger.WriteException("DeliveryRequestRepository", "DeleteDeliveryRequest", ex.Message, ex);
                }
            }
            return response;
        }
        public async Task<bool> MoveCalendarDeliveryRequest()
        {
            var response = false;
            try
            {
                DateTime commpareDate = DateTime.Now.Date.AddDays(2);
                var filter = Builders<DeliveryRequest>.Filter.Where(t => t.IsCalendarView
                                        && (t.SelectedDate != null && t.SelectedDate < commpareDate)
                                        && t.IsActive && !t.IsDeleted && t.Status == DeliveryReqStatus.Pending);
                var updatefileds = Builders<DeliveryRequest>.Update
                            .Set(t => t.IsCalendarView, false)
                            .Set(t => t.UpdatedOn, DateTime.Now);
                await mdbContext.DeliveryRequests.UpdateManyAsync(filter, updatefileds);
                response = true;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryRequestRepository", "MoveCalendarDeliveryRequest", ex.Message, ex);
            }
            return response;
        }

        public async Task<DeliveryReqPriority> GetPriorityForSalesDR(TankDetailsModel tank)
        {
            if (tank.FuelCapacity == null || tank.FuelCapacity.Value <= 0)
            {
                return DeliveryReqPriority.CouldGo; // Do not create DR as capacity is not mentioned for the tank.
            }
            tank.RunOutLevel = (((decimal)tank.MinFill * 100) / (decimal)tank.FuelCapacity).GetPreciseValue(1);
            var demandsData = await GetDemands(new List<string> { tank.JobName }, new List<string> { tank.TankId }, new List<string> { tank.StorageId });
            var updatedDipTest = demandsData.FirstOrDefault(t => t.SiteId == tank.JobName
                                                                            && t.StorageId == tank.StorageId
                                                                            && t.TankId == tank.TankId);

            if (updatedDipTest == null || updatedDipTest.NetVolume == -1 || updatedDipTest.IsProcessed)
            {
                return DeliveryReqPriority.CouldGo;
            }
            var netVolume = (updatedDipTest?.NetVolume) ?? 0;
            decimal.TryParse(netVolume.ToString(), out decimal currentVolume);
            var current = (currentVolume / tank.FuelCapacity.Value) * 100;
            var currentInventory = (current < 0) ? 0 : (current > 100) ? 100 : current;
            var definedPrioirity = GetDeliveryRequestPriority(currentInventory, tank.MinFill, (int)tank.FillType, tank.RunOutLevel, tank.FuelCapacity, tank.ThresholdDeliveryRequest);
            var priority = definedPrioirity == DeliveryReqPriority.None ? DeliveryReqPriority.CouldGo : definedPrioirity;
            return priority;
        }
        private static void IntializeDemandsParameters(List<string> siteIdList, List<string> tankIdList, List<string> storageList, out DataTable SiteIds, out DataTable TankIds, out DataTable StorageIds)
        {
            SiteIds = CreateDemandTable();
            foreach (var item in siteIdList.Distinct().ToList())
            {
                var row = SiteIds.NewRow();
                row["SearchVar"] = item;
                SiteIds.Rows.Add(row);
            }
            //Create TankIds Table
            TankIds = CreateDemandTable();
            foreach (var item in tankIdList.Distinct().ToList())
            {
                var row = TankIds.NewRow();
                row["SearchVar"] = item;
                TankIds.Rows.Add(row);
            }

            //Create StorageIds Table
            StorageIds = CreateDemandTable();
            foreach (var item in storageList.Distinct().ToList())
            {
                var row = StorageIds.NewRow();
                row["SearchVar"] = item;
                StorageIds.Rows.Add(row);
            }
        }

        private static DataTable CreateDemandTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("SearchVar", typeof(string));
            return dt;
        }
        protected void GetJobCompatibilityInfo(List<int> jobIds, List<DeliveryRequestViewModel> drs, string selectedDate)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(selectedDate) && drs != null && drs.Any() && jobIds != null && jobIds.Any())
                {
                    var date = DateTime.Parse(selectedDate);
                    var deliveryDay = (int)date.DayOfWeek;
                    deliveryDay = deliveryDay == 0 ? 7 : deliveryDay; // sunday is saved as 7 in our code but 0 in DayOfWeek enum
                    var compatibleTrailerTypes = new List<FreightModels.DropdownDisplayItem>();
                    bool IsAcceptNightDeliveries = false;
                    var jobsDetails = mdbContext.JobAdditionalDetails.Find(t => jobIds.Contains(t.TfxJobId)).Project(t => new { t.TfxJobId, t.TrailerType, t.DeliveryDaysList, t.DistanceCovered }).ToList();
                    foreach (var jobId in jobIds)
                    {
                        IsAcceptNightDeliveries = false;
                        compatibleTrailerTypes = new List<FreightModels.DropdownDisplayItem>();
                        var jobInfoModel = jobsDetails.Where(t => t.TfxJobId == jobId).Select(t => new
                        {
                            TrailerTypes = t.TrailerType,
                            t.DeliveryDaysList,
                            t.DistanceCovered
                        }).FirstOrDefault();
                        if (jobInfoModel != null)
                        {
                            if (jobInfoModel.TrailerTypes != null && jobInfoModel.TrailerTypes.Any())
                            {
                                //compatibleTrailerTypes = new List<FreightModels.DropdownDisplayItem>();
                                foreach (var trailer in jobInfoModel.TrailerTypes)
                                {
                                    var item = new FreightModels.DropdownDisplayItem();
                                    item.Id = (int)trailer;
                                    item.Name = trailer.GetDisplayName();
                                    compatibleTrailerTypes.Add(item);
                                }
                            }
                            if (jobInfoModel.DeliveryDaysList != null && jobInfoModel.DeliveryDaysList.Any())
                            {
                                var deliveryday = jobInfoModel.DeliveryDaysList.Where(t => t.DeliveryDays.HasValue && t.DeliveryDays == deliveryDay).FirstOrDefault();
                                if (deliveryday != null)
                                {
                                    IsAcceptNightDeliveries = deliveryday.IsAcceptNightDeliveries;
                                }
                            }
                            var setCompatibilityInfo = drs.FindAll(t => t.JobId == jobId).ToList();
                            if (setCompatibilityInfo != null && setCompatibilityInfo.Any())
                            {
                                setCompatibilityInfo.ForEach(t => t.TrailerTypes.AddRange(compatibleTrailerTypes));
                                setCompatibilityInfo.ForEach(t => t.IsAcceptNightDeliveries = IsAcceptNightDeliveries);
                                setCompatibilityInfo.ForEach(t => t.HoursToCoverDistance = jobInfoModel.DistanceCovered);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryRequestRepository", "GetJobCompatibilityInfo", ex.Message, ex);
            }
        }
        
    }
}
