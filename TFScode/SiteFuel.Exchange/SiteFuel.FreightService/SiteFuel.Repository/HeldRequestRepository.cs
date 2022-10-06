using MongoDB.Bson;
using MongoDB.Driver;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
using SiteFuel.FreightRepository.Mappers;
using SiteFuel.MdbDataAccess;
using SiteFuel.MdbDataAccess.Collections;
using SiteFuel.MdbDataAccess.DbContext;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SiteFuel.FreightRepository
{
    public class HeldRequestRepository :  IHeldRequestRepository
    {
        private readonly MdbContext mdbContext;
        public HeldRequestRepository()
        {
            if (mdbContext == null)
            {
                mdbContext = new MdbContext();
            }
        }
        public HeldRequestRepository(MdbContext _context)
        {
            mdbContext = _context;
        }

        public async Task<HeldDeliveryRequestsModel> CreateHeldDeliveryRequests(List<HeldDeliveryRequestModel> deliveryRequests)
        {
            var response = new HeldDeliveryRequestsModel();
            bool isSessionStarted = false;
            using (var session = await mdbContext.Client.StartSessionAsync())
            {
                try
                {
                    var requestsWithoutRegion = deliveryRequests.Where(t => t.IndicativePrice > 0 && (t.CreatedByRegionId == null || t.CreatedByRegionId == "")).Select(t => t).ToList();
                    bool isValidRegion = true;

                    isValidRegion = ValidateRegion(requestsWithoutRegion, isValidRegion);
                    if (!isValidRegion)
                    {
                        response.StatusCode = (int)Status.Failed;
                        response.StatusMessage = Resource.valMessageJobNotAssignedToRegion;
                        return response;
                    }

                    if (deliveryRequests.Any())
                    {
                        session.StartTransaction();
                        isSessionStarted = true;
                        var heldDrs = new List<HeldDeliveryRequest>();
                        foreach (var dr in deliveryRequests)
                        {
                            heldDrs.Add(dr.ToHeldDeliveryRequest());
                        }
                        await mdbContext.HeldDeliveryRequests.InsertManyAsync(heldDrs);
                        await session.CommitTransactionAsync();
                        foreach (var entity in heldDrs)
                        {
                            response.Requests.Add(entity.ToHeldDeliveryRequestModel());
                        }
                        response.StatusCode = (int)Status.Success;
                        response.StatusMessage = Resource.SuccessHeldDrCreation;
                    }
                    else
                    {
                        response.StatusCode = (int)Status.Failed;
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
                    response.StatusMessage = Resource.msgDelReqCreationFailed;
                    throw;
                }
            }
            return response;
        }

        public async Task<long> GetHeldDeliveryRequestCount(int companyId)
        {
            long count = 0;
            var filter = Builders<HeldDeliveryRequest>.Filter.And(Builders<HeldDeliveryRequest>.Filter.Where(x => x.IsActive && x.SupplierCompanyId == companyId && x.Status != HeldDrStatus.Passed && x.Status != HeldDrStatus.ByPassed));
            count = await mdbContext.HeldDeliveryRequests.CountDocumentsAsync(filter);
            return count;
        }

        public async Task<HeldDeliveryRequestModel> UpdateHeldDrCreditCheckStatus(SalesOrderStatusModel viewModel)
        {
            var response = new HeldDeliveryRequestModel();
            var updateFields = Builders<HeldDeliveryRequest>.Update
                                .Set(t => t.Sap_Order_Status, viewModel.SAP_Order_Status)
                                .Set(t => t.UpdatedOn, DateTimeOffset.Now)
                                .Set(t => t.Sap_OrderNo, viewModel.SAP_Order_No)
                                .Set(t => t.Status, viewModel.SAP_Order_Status == "00" ? HeldDrStatus.Passed : HeldDrStatus.Incomplete);
            var filter = Builders<HeldDeliveryRequest>.Filter.And(Builders<HeldDeliveryRequest>.Filter.Where(x => x.UniqueOrderNo == viewModel.ExternalOrderNo && x.IsActive && x.Status != HeldDrStatus.Passed));

            var updateResult = await mdbContext.HeldDeliveryRequests.UpdateOneAsync(filter, updateFields);
            if (updateResult.ModifiedCount > 0)
            {
                var heldDr = await mdbContext.HeldDeliveryRequests.Find(t => t.UniqueOrderNo == viewModel.ExternalOrderNo && t.IsActive).FirstOrDefaultAsync();
                if (heldDr != null)
                {
                    response = heldDr.ToHeldDeliveryRequestModel();
                }
            }
            return response;
        }

        public async Task<HeldDeliveryRequestModel> OverrideCreditCheckApproval(OverrideCreditCheckApprovalModel viewModel)
        {
            var response = new HeldDeliveryRequestModel();
            var objectId = ObjectId.Parse(viewModel.HeldDRId);
            var updateFields = Builders<HeldDeliveryRequest>.Update
                                .Set(t => t.UpdatedBy, viewModel.UserId)
                                .Set(t => t.UpdatedOn, DateTimeOffset.Now)
                                .Set(t => t.FileName, viewModel.FileName)
                                .Set(t => t.CreditApprovalFilePath, viewModel.FilePath)
                                .Set(t => t.Status,  HeldDrStatus.ByPassed);
            var filter = Builders<HeldDeliveryRequest>.Filter.And(Builders<HeldDeliveryRequest>.Filter.Where(x => x.Id == objectId && x.IsActive && x.Status != HeldDrStatus.Passed));

            var updateResult = await mdbContext.HeldDeliveryRequests.UpdateOneAsync(filter, updateFields);
            if (updateResult.ModifiedCount > 0)
            {
                var heldDr = await mdbContext.HeldDeliveryRequests.Find(t => t.Id == objectId && t.IsActive).FirstOrDefaultAsync();
                if (heldDr != null)
                {
                    response = heldDr.ToHeldDeliveryRequestModel();
                }
            }
            return response;
        }

        public async Task<List<HeldDeliveryRequestModel>> GetHeldDeliveryRequests(int companyId)
        {
            var response = new List<HeldDeliveryRequestModel>();

            var drs = await mdbContext.HeldDeliveryRequests.Find(dr => dr.IsActive && dr.SupplierCompanyId == companyId && dr.Status != HeldDrStatus.Passed && dr.Status != HeldDrStatus.ByPassed)
                             .ToListAsync();

            foreach (var dr in drs)
            {
                var heldDR = dr.ToHeldDeliveryRequestModel();
                response.Add(heldDR);
            }
            return response;
        }

        public async Task<HeldDeliveryRequestModel> GetHeldDeliveryRequestById(string id)
        {
            var response = new HeldDeliveryRequestModel();
            var objectIdForDr = ObjectId.Parse(id);
            var dr = await mdbContext.HeldDeliveryRequests.Find(t => t.Id == objectIdForDr).FirstOrDefaultAsync();
            if (dr != null)
            {
                response = dr.ToHeldDeliveryRequestModel();
                if (response.Terminal == null)
                {
                    response.Terminal = new FreightModels.DropdownDisplayItem();
                }
                if (response.PickupLocationType == PickupLocationType.None && (response.Bulkplant == null || string.IsNullOrWhiteSpace(response.Bulkplant.SiteName)))
                {
                    response.PickupLocationType = PickupLocationType.Terminal;
                }
                if (response.Bulkplant == null)
                {
                    response.Bulkplant = new BulkPlantAddressModel();
                }
            }
            return response;
        }

        public async Task<HeldDeliveryRequestModel> EditHeldDeliveryRequest(HeldDeliveryRequestModel model)
        {
            var response = new HeldDeliveryRequestModel();
            if (model != null)
            {
                var objectIdForDr = ObjectId.Parse(model.HeldDrId);
                var dr = await mdbContext.HeldDeliveryRequests.Find(t => t.Id == objectIdForDr && t.IsActive && t.Status != HeldDrStatus.Passed && t.Status != HeldDrStatus.ByPassed).Project(t => new { t.Terminal, t.Bulkplant, t.RequiredQuantity, t.IndicativePrice, t.SelectedDate }).FirstOrDefaultAsync();
                if (dr == null)
                {
                    return response;
                }
                var terminal = new MdbDataAccess.Collections.DropdownDisplayItem();
                var bulkplant = new BulkPlantAddressModel();
                if (model.Terminal != null && model.Terminal.Id > 0 && model.PickupLocationType != PickupLocationType.BulkPlant)
                {
                    terminal.Id = model.Terminal.Id;
                    terminal.Name = model.Terminal.Name;
                }
                if (model.Bulkplant != null && !string.IsNullOrWhiteSpace(model.Bulkplant.SiteName))
                {
                    bulkplant = model.Bulkplant;
                }
                bool isDrUpdated = false;
                if (model.RequiredQuantity != dr.RequiredQuantity || (dr.Terminal != null && terminal.Id != dr.Terminal.Id) || (dr.Bulkplant != null && bulkplant.Id != dr.Bulkplant.Id) || model.SelectedDate != dr.SelectedDate || model.IndicativePrice != dr.IndicativePrice)
                {
                    isDrUpdated = true;
                }
                var updateFields = Builders<HeldDeliveryRequest>.Update
                    .Set(t => t.RequiredQuantity, model.RequiredQuantity)
                    .Set(t => t.BadgeNo1, model.BadgeNo1)
                    .Set(t => t.BadgeNo2, model.BadgeNo2)
                    .Set(t => t.BadgeNo3, model.BadgeNo3)
                    .Set(t => t.Terminal, terminal)
                    .Set(t => t.Bulkplant, bulkplant)
                    .Set(t => t.Notes, model.Notes)
                    .Set(t => t.PickupLocationType, model.PickupLocationType)
                    .Set(t => t.SelectedDate, model.SelectedDate)
                    .Set(t => t.ScheduleStartTime, model.ScheduleStartTime)
                    .Set(t => t.ScheduleEndTime, model.ScheduleEndTime)
                    .Set(t => t.Notes, model.Notes)
                    .Set(t => t.IndicativePrice, model.IndicativePrice)
                    .Set(t => t.DeliveryLevelPO, model.DeliveryLevelPO)
                    .Set(t => t.IsDREdited, isDrUpdated)
                    .Set(t => t.Priority, model.Priority);

                var filter = Builders<HeldDeliveryRequest>.Filter.And(
                        Builders<HeldDeliveryRequest>.Filter.Where(x => x.Id == objectIdForDr && x.IsActive && x.Status != HeldDrStatus.Passed)
                    );

                await mdbContext.HeldDeliveryRequests.UpdateOneAsync(filter, updateFields);
                var helddr = await mdbContext.HeldDeliveryRequests.Find(t => t.Id == objectIdForDr && t.IsActive).FirstOrDefaultAsync();
                response = helddr.ToHeldDeliveryRequestModel();
            }
            return response;
        }

        public async Task<HeldDeliveryRequestsModel> DeleteHeldDr(string id, int userId)
        {
            var response = new HeldDeliveryRequestsModel();

            var updateFields = Builders<HeldDeliveryRequest>.Update
                                .Set(t => t.UpdatedBy, userId)
                                .Set(t => t.UpdatedOn, DateTimeOffset.Now)
                                .Set(t => t.IsActive, false);
            ObjectId drId = ObjectId.Parse(id);
            var filter = Builders<HeldDeliveryRequest>.Filter.And(Builders<HeldDeliveryRequest>.Filter.Where(x => x.Id == drId));

            await mdbContext.HeldDeliveryRequests.UpdateOneAsync(filter, updateFields);
            response.StatusCode = (int)Status.Success;
            response.StatusMessage = Resource.SuccessHeldDrDelete;
            var heldDr = await mdbContext.HeldDeliveryRequests.Find(t => t.Id == drId).FirstOrDefaultAsync();
            response.Requests.Add(heldDr.ToHeldDeliveryRequestModel());
            return response;
        }

        public async Task<StatusModel> UpdateHeldDrStatus(string id)
        {
            var response = new StatusModel();

            var updateFields = Builders<HeldDeliveryRequest>.Update
                                .Set(t => t.Status, HeldDrStatus.Pending)
                                .Set(t => t.UpdatedOn, DateTimeOffset.Now);
            ObjectId drId = ObjectId.Parse(id);
            var filter = Builders<HeldDeliveryRequest>.Filter.And(Builders<HeldDeliveryRequest>.Filter.Where(x => x.Id == drId && x.IsActive && x.Status == HeldDrStatus.New));

            await mdbContext.HeldDeliveryRequests.UpdateOneAsync(filter, updateFields);
            response.StatusCode = (int)Status.Success;
            return response;
        }

        public async Task<StatusModel> UpdateHeldDrValidationStatus(string id, string message)
        {
            var response = new StatusModel();

            var updateFields = Builders<HeldDeliveryRequest>.Update
                                .Set(t => t.ValidationMessage, message)
                                .Set(t => t.UpdatedOn, DateTimeOffset.Now);
            ObjectId drId = ObjectId.Parse(id);
            var filter = Builders<HeldDeliveryRequest>.Filter.And(Builders<HeldDeliveryRequest>.Filter.Where(x => x.Id == drId && x.IsActive && x.Status == HeldDrStatus.New));

            await mdbContext.HeldDeliveryRequests.UpdateOneAsync(filter, updateFields);
            response.StatusCode = (int)Status.Success;
            return response;
        }

        private bool ValidateRegion(List<HeldDeliveryRequestModel> requestsWithoutRegion, bool isValidRegion)
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
    }
}
