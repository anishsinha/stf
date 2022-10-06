using MongoDB.Bson;
using MongoDB.Driver;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
using SiteFuel.FreightRepository.Mappers;
using SiteFuel.MdbDataAccess.Collections;
using SiteFuel.MdbDataAccess.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteFuel.FreightRepository
{
    public class DRCarrierSequenceRepository : IDRCarrierSequenceRepository
    {
        private readonly MdbContext mdbContext;
        public DRCarrierSequenceRepository(MdbContext _context)
        {
            mdbContext = _context;
        }
        public async Task<DRCarrierSequenceModel> GetDRCarrierSequence(string deliveryReqId)
        {
            DRCarrierSequenceModel dRCarrierSequenceModel = new DRCarrierSequenceModel();
            ObjectId objDeliveryReqId = ObjectId.Empty;
            ObjectId.TryParse(deliveryReqId, out objDeliveryReqId);
            var drCarrierSeq = await mdbContext.DRCarrierSequences.Find(top => top.DeliveryRequestId == objDeliveryReqId).FirstOrDefaultAsync();
            dRCarrierSequenceModel = drCarrierSeq.ToEntity();
            return dRCarrierSequenceModel;
        }

        public async Task<StatusModel> SaveDRCarrierSequence(List<DRCarrierSequenceModel> model)
        {
            StatusModel response = new StatusModel();
            bool isSessionStarted = false;
            if (model != null && model.Any())
            {
                List<DRCarrierSequence> drCarrierUpdateList = new List<DRCarrierSequence>();
                var drCarrierModel = model.ToEntity();
                var deliveyReqList = drCarrierModel.Select(x => x.DeliveryRequestId).ToList();
                var dRcarrierSeqExists = await mdbContext.DRCarrierSequences.Find(top => deliveyReqList.Contains(top.DeliveryRequestId) && top.IsActive && !top.IsDeleted).ToListAsync();
                foreach (var item in dRcarrierSeqExists)
                {
                    var drCarrierInfo = drCarrierModel.FirstOrDefault(x => x.DeliveryRequestId == item.DeliveryRequestId);
                    if (drCarrierInfo != null)
                    {
                        drCarrierModel.Remove(drCarrierInfo);
                        drCarrierUpdateList.Add(drCarrierInfo);
                    }
                }
                if (drCarrierModel.Any())
                {
                    using (var session = await mdbContext.Client.StartSessionAsync())
                    {
                        try
                        {
                            session.StartTransaction();
                            isSessionStarted = true;
                            await mdbContext.DRCarrierSequences.InsertManyAsync(drCarrierModel);
                            await session.CommitTransactionAsync();
                            response.StatusCode = (int)Status.Success;
                            response.StatusMessage = Resource.successDRCarrierSequence;
                        }
                        catch (Exception ex)
                        {
                            if (isSessionStarted)
                            {
                                await session.AbortTransactionAsync();
                            }
                            response.StatusCode = (int)Status.Failed;
                            response.StatusMessage = Resource.errorDRCarrierSeq;
                            throw ex;
                        }
                    }
                }
                if (drCarrierUpdateList.Any())
                {
                    await UpdateDRCarrierSequence(drCarrierUpdateList.ToEntity());
                }

            }
            return response;
        }
        public async Task<StatusModel> UpdateDRCarrierSequence(List<DRCarrierSequenceModel> model)
        {
            StatusModel response = new StatusModel();
            if (model != null && model.Any())
            {
                bool isSessionStarted = false;
                var drCarrierModel = model.ToEntity();
                using (var session = await mdbContext.Client.StartSessionAsync())
                {
                    try
                    {
                        foreach (var item in drCarrierModel)
                        {
                            session.StartTransaction();
                            isSessionStarted = true;
                            var filter = Builders<DRCarrierSequence>.Filter.And(
                                              Builders<DRCarrierSequence>.Filter.Eq(t => t.DeliveryRequestId, item.DeliveryRequestId),
                                              Builders<DRCarrierSequence>.Filter.Where(t => t.IsActive && !t.IsDeleted));
                            var updateFields = Builders<DRCarrierSequence>.Update
                                                              .Set(t => t.CarrierInfo, item.CarrierInfo);
                            await mdbContext.DRCarrierSequences.UpdateOneAsync(filter, updateFields);
                            await session.CommitTransactionAsync();
                            response.StatusCode = (int)Status.Success;
                        }

                    }
                    catch (Exception ex)
                    {
                        if (isSessionStarted)
                        {
                            await session.AbortTransactionAsync();
                        }
                        response.StatusCode = (int)Status.Failed;
                        response.StatusMessage = Resource.errorDRCarrierSeq;
                        throw ex;
                    }
                }
            }
            return response;
        }
        public async Task<StatusModel> UpdateDRCarrierRejectList(DRCarrierRejectInfoModel model)
        {
            TfxCarrierRejectInfo carrierRejectInfo = new TfxCarrierRejectInfo();
            carrierRejectInfo = model.ToEntity();
            StatusModel response = new StatusModel();
            if (carrierRejectInfo != null)
            {
                bool isSessionStarted = false;
                ObjectId objDeliveryReqId = ObjectId.Empty;
                ObjectId.TryParse(model.DeliveryRequestId, out objDeliveryReqId);
                var drCarrierSeqObj = await mdbContext.DRCarrierSequences.Find(top => top.DeliveryRequestId == objDeliveryReqId && top.IsActive && !top.IsDeleted).Project(x => new { x.Id, x.CarrierRejectInfo }).FirstOrDefaultAsync();
                if (drCarrierSeqObj != null)
                {
                    using (var session = await mdbContext.Client.StartSessionAsync())
                    {
                        try
                        {

                            drCarrierSeqObj.CarrierRejectInfo.Add(carrierRejectInfo);
                            session.StartTransaction();
                            isSessionStarted = true;
                            var filter = Builders<DRCarrierSequence>.Filter.And(
                                              Builders<DRCarrierSequence>.Filter.Eq(t => t.DeliveryRequestId, objDeliveryReqId),
                                              Builders<DRCarrierSequence>.Filter.Where(t => t.IsActive && !t.IsDeleted));
                            var updateFields = Builders<DRCarrierSequence>.Update
                                                              .Set(t => t.CarrierRejectInfo, drCarrierSeqObj.CarrierRejectInfo);
                            await mdbContext.DRCarrierSequences.UpdateOneAsync(filter, updateFields);
                            await session.CommitTransactionAsync();
                            response.StatusCode = (int)Status.Success;
                            response.StatusMessage = "Success";
                        }
                        catch (Exception ex)
                        {
                            if (isSessionStarted)
                            {
                                await session.AbortTransactionAsync();
                            }
                            response.StatusCode = (int)Status.Failed;
                            response.StatusMessage = Resource.errorDRCarrierSeq;
                            throw ex;
                        }
                    }
                }
            }
            return response;
        }

        public async Task<TfxDRAvailableCarrierInfoModel> GetAvailableDRCarrierDetails(string deliveryReqId)
        {
            TfxDRAvailableCarrierInfoModel drCarrierModel = new TfxDRAvailableCarrierInfoModel();
            List<TfxCarrierDropdownDisplayViewModelItem> drAvailableCarrier = new List<TfxCarrierDropdownDisplayViewModelItem>();
            ObjectId objDeliveryReqId = ObjectId.Empty;
            ObjectId.TryParse(deliveryReqId, out objDeliveryReqId);
            var drCarrierSeq = await mdbContext.DRCarrierSequences.Find(top => top.DeliveryRequestId == objDeliveryReqId && top.IsActive && !top.IsDeleted).Project(x => new { x.DeliveryRequestId, x.RegionId, x.CarrierInfo, x.CarrierRejectInfo,x.TfxSupplierCompanyId,x.TfxSupplierOrderId }).FirstOrDefaultAsync();
            var deliveryRequestInfo = await mdbContext.DeliveryRequests.Find(top => top.Id == objDeliveryReqId && top.IsActive && !top.IsDeleted).FirstOrDefaultAsync();
            if (drCarrierSeq != null && deliveryRequestInfo != null)
            {
                drCarrierModel.TfxSupplierCompanyId = drCarrierSeq.TfxSupplierCompanyId;
                drCarrierModel.TfxSupplierOrderId = drCarrierSeq.TfxSupplierOrderId;
                drCarrierModel.DeliveryRequest = deliveryRequestInfo.ToDeliveryRequestViewModel();
                if (drCarrierSeq.CarrierRejectInfo.Any())
                {
                    foreach (var item in drCarrierSeq.CarrierInfo.Where(x => x.Id != deliveryRequestInfo.TfxAssignedToCompanyId).ToList())
                    {
                        var drAvailableCarrierExists = drCarrierSeq.CarrierRejectInfo.FirstOrDefault(x => x.Id == item.Id);
                        if (drAvailableCarrierExists == null)
                        {
                            drAvailableCarrier.Add(new TfxCarrierDropdownDisplayViewModelItem { Id = item.Id, Code = item.Code, Name = item.Name, RegionId = item.RegionId, SequenceNo = item.SequenceNo });
                        }
                    }
                }
                else
                {
                    foreach (var item in drCarrierSeq.CarrierInfo.Where(x => x.Id != deliveryRequestInfo.TfxAssignedToCompanyId).ToList())
                    {
                        drAvailableCarrier.Add(new TfxCarrierDropdownDisplayViewModelItem { Id = item.Id, Code = item.Code, Name = item.Name, RegionId = item.RegionId, SequenceNo = item.SequenceNo });

                    }
                }
                if (drAvailableCarrier.Any())
                {
                    drCarrierModel.StatusCode = (int)Status.Success;
                    drCarrierModel.DeliveryRequestId = deliveryReqId;
                    drCarrierModel.RegionId = drAvailableCarrier.OrderBy(x => x.SequenceNo).FirstOrDefault().RegionId;
                    drCarrierModel.CarrierInfo = drAvailableCarrier.OrderBy(x => x.SequenceNo).FirstOrDefault();
                }
                else
                {
                    drCarrierModel.StatusCode = (int)Status.Warning;
                }
            }
            return drCarrierModel;
        }
    }
}


