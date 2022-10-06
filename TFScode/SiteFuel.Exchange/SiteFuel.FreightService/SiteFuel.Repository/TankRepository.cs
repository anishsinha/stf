using MongoDB.Bson;
using MongoDB.Driver;
using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
using SiteFuel.MdbDataAccess.Collections;
using SiteFuel.MdbDataAccess.DbContext;
using SiteFuel.FreightRepository.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueFill.DemandCaptureDataAccess;
using System.Data.Entity;
using SiteFuel.Exchange.Logger;
using SiteFuel.MdbDataAccess;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Core.Infrastructure;
using TrueFill.ExchangeDataAccess.DataAccess;
using System.Data;
using System.Data.SqlClient;
using TrueFill.DemandCaptureDataAccess.Entities;

namespace SiteFuel.FreightRepository
{
    public class TankRepository : ITankRepository
    {
        private readonly DemandCaptureContext context=new DemandCaptureContext();
        private readonly MdbContext mdbContext;
        public TankRepository()
        {
            if (mdbContext == null)
            {
                mdbContext = new MdbContext();
            }
        }
        public TankRepository(MdbContext _mdbcontext)
        {
            mdbContext = _mdbcontext;
        }
        public async Task<bool> SaveTankDetails(TankDetailsModel model)
        {
            try
            {
                var tank = model.ToEntity();
                var existingJob = await mdbContext.JobAdditionalDetails.Find(t => t.TfxJobId == model.JobId && t.IsActive).FirstOrDefaultAsync();
                if (existingJob != null)
                {
                    await mdbContext.JobAdditionalDetails.UpdateOneAsync(Builders<JobAdditionalDetail>.Filter.Eq(t => t.Id, existingJob.Id), Builders<JobAdditionalDetail>.Update.Push(t => t.Tanks, tank));
                }
                else
                {
                    var newJob = new JobAdditionalDetail { TfxJobId = model.JobId, TfxDisplayJobId = model.JobDisplayId, IsActive = true, TfxJobName = model.JobName };
                    newJob.Tanks.Add(tank);
                    await mdbContext.JobAdditionalDetails.InsertOneAsync(newJob);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("TankRepository", "SaveTankDetails", ex.Message, ex);
                return false;
            }
            return true;
        }

        public async Task<bool> UpdateTankDetails(TankDetailsModel model)
        {
            using (var session = await mdbContext.Client.StartSessionAsync())
            {
                session.StartTransaction();
                try
                {
                    var existingJob = await mdbContext.JobAdditionalDetails.Find(t => t.TfxJobId == model.JobId && t.IsActive).FirstOrDefaultAsync();
                    if (existingJob != null)
                    {
                        var existingTank = existingJob.Tanks.FirstOrDefault(t => t.TfxAssetId == model.AssetId);
                        var newtank = model.ToEntity();
                        if (existingTank != null)
                        {
                            existingJob.Tanks.Remove(existingTank);
                            if (existingTank.TfxProductTypeId != model.FuelTypeId || existingTank.StorageId != model.StorageId || existingTank.StorageTypeId != model.TankId)
                            {
                                var filter = Builders<DeliveryRequest>.Filter.And(
                                                Builders<DeliveryRequest>.Filter.Where(t => t.TfxJobId == model.JobId && t.TfxProductTypeId == existingTank.TfxProductTypeId && t.StorageId == existingTank.StorageId && t.StorageTypeId == existingTank.StorageTypeId && t.IsActive && !t.IsDeleted && t.Status != DeliveryReqStatus.ScheduleCreated)
                                         );
                                var updateFields = Builders<DeliveryRequest>.Update
                                    .Set(t => t.TfxProductTypeId, model.FuelTypeId)
                                    .Set(t => t.StorageId, model.StorageId)
                                    .Set(t => t.StorageTypeId, model.TankId)
                                    .Set(t => t.TfxProductType, model.ProductTypeName);

                                await mdbContext.DeliveryRequests.UpdateManyAsync(filter, updateFields);
                            }
                        }
                        existingJob.Tanks.Add(newtank);
                        await mdbContext.JobAdditionalDetails.ReplaceOneAsync(Builders<JobAdditionalDetail>.Filter.Eq(t => t.Id, existingJob.Id), existingJob);
                    }
                    else
                    {
                        return await SaveTankDetails(model);
                    }
                    await session.CommitTransactionAsync();
                }
                catch (Exception ex)
                {
                    await session.AbortTransactionAsync();
                    LogManager.Logger.WriteException("TankRepository", "UpdateTankDetails", ex.Message, ex);
                    return false;
                }
            }
            return true;
        }

        public async Task<TankDetailsModel> GetTankDetails(int id)
        {
            try
            {
                var response = await mdbContext.JobAdditionalDetails.Find(x => x.Tanks.Any(x1 => x1.TfxAssetId == id)).FirstOrDefaultAsync();
                if (response != null)
                    return response.Tanks.FirstOrDefault(x => x.TfxAssetId == id).ToViewModel();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("TankRepository", "GetTankDetails", ex.Message, ex);
            }
            return null;
        }

        public async Task<List<TankDetailListModel>> GetTankList(List<int> tanks)
        {
            var response = new List<TankDetailListModel>();
            try
            {
                var siteIdList = new List<String>();
                if (tanks != null && tanks.Any())
                {
                    var jobTanks = await mdbContext.JobAdditionalDetails.Find(x => x.Tanks.Any(x1 => tanks.Contains(x1.TfxAssetId))).Project(d => new { d.TfxDisplayJobId, d.TfxJobId, Tanks = d.Tanks.Where(d1 => tanks.Contains(d1.TfxAssetId)).OrderBy(t => t.TankSequence == null || t.TankSequence == 0 ? 99999 : t.TankSequence) }).ToListAsync();
                    foreach (var job in jobTanks)
                    {
                        siteIdList.Add(job.TfxDisplayJobId);
                        foreach (var item in job.Tanks)
                        {
                            response.Add(item.ToDetailsViewModel(job.TfxJobId, job.TfxDisplayJobId));
                        }
                    }

                    var tankModelTypeIds = response.Where(t => t.TankModelTypeId != null).Select(t => t.TankModelTypeId).Distinct().Select(x => ObjectId.Parse(x)).ToList();
                    var tankModelType = await mdbContext.TankModalTypes.Find(t => tankModelTypeIds.Contains(t.Id) && t.IsActive).Project(x => new { x.Id, x.Name, x.Modal, x.PdfFilePath, x.ScaleMeasurement }).ToListAsync();
                    foreach (var item in response.Where(t => t.TankModelTypeId != null))
                    {
                        var tankobject = tankModelType.FirstOrDefault(t => t.Id == ObjectId.Parse(item.TankModelTypeId));
                        if (tankobject != null)
                        {
                            item.TankMakeModel = tankobject.Name + " - " + tankobject.Modal;
                            item.TankMake = tankobject.Name;
                            item.TankModel = tankobject.Modal;
                            item.ScaleMeasurement = tankobject.ScaleMeasurement;
                            item.TankChart = GetFilePath(tankobject.PdfFilePath);
                            item.TankChartFileName = tankobject.PdfFilePath;
                        }
                    }

                    var tankIdList = response.Select(t => t.TankId).ToList();
                    var storageIdList = response.Select(t => t.StorageId).ToList();
                    if (response.Count > 0) 
                    {
                        DataTable SiteIds, TankIds, StorageIds;
                        IntializeDemandsParameters(siteIdList, tankIdList, storageIdList, out SiteIds, out TankIds, out StorageIds);
                        var siteIdParam = new SqlParameter("@SiteIds", SqlDbType.Structured);
                        siteIdParam.Value = SiteIds;
                        siteIdParam.TypeName = "dbo.DemandsSearchTypes";

                        var tankIdsParam = new SqlParameter("@TankIds", SqlDbType.Structured);
                        tankIdsParam.Value = TankIds;
                        tankIdsParam.TypeName = "dbo.DemandsSearchTypes";

                        var storageIdParams = new SqlParameter("@StorageIds", SqlDbType.Structured);
                        storageIdParams.Value = StorageIds;
                        storageIdParams.TypeName = "dbo.DemandsSearchTypes";

                        context.Database.CommandTimeout = 60;
                        var demands = await context.Database.SqlQuery<DemandModel>("usp_GetDemandsBySiteIdsTankIdsStorageIds @SiteIds,@TankIds,@StorageIds", siteIdParam, tankIdsParam, storageIdParams).ToListAsync();

                        if (demands != null)
                        {
                            foreach (var item in demands)
                            {
                                var demandDetails = response.FirstOrDefault(t => t.JobDisplayId == item.SiteId && t.TankId == item.TankId && t.StorageId == item.StorageId);
                                if (demandDetails != null)
                                {
                                    demandDetails.LastReading = item.NetVolume;
                                    demandDetails.CaptureTime = item.CaptureTime;
                                    demandDetails.Ullage = item.Ullage;
                                    demandDetails.GrossVolume = item.GrossVolume;
                                    demandDetails.NetVolume = item.NetVolume;
                                    demandDetails.DipTestValue = item.DipTestValue;
                                    demandDetails.DipTestUoM = item.DipTestUoM;
                                }
                            }
                        }
                    }

                    response = ProcessTanksToSetPriority(response);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("TankRepository", "GetTankList", ex.Message, ex);
            }
            return response;
        }

        private List<TankDetailListModel> ProcessTanksToSetPriority(List<TankDetailListModel> tankList)
        {
            try
            {
                foreach (var tank in tankList)
                {
                    if (tank.FuelCapacity.HasValue && tank.FuelCapacity.Value > 0)
                    {
                        var netVolume = (tank?.NetVolume) ?? 0;
                        decimal.TryParse(netVolume.ToString(), out decimal currentVolume);

                        var current = (currentVolume / tank.FuelCapacity.Value) * 100;
                        var currentInventory = (current < 0) ? 0 : (current > 100) ? 100 : current;

                        decimal minFillPercent = 0;
                        if (tank.FillType.Value == FillType.UoM && tank.MinFill.HasValue && tank.FuelCapacity.HasValue)
                            minFillPercent = (tank.MinFill.Value * 100) / tank.FuelCapacity.Value;
                        else if (tank.FillType.Value == FillType.Percent && tank.MinFillPercent.HasValue)
                            minFillPercent = tank.MinFillPercent.Value;

                        var definedPrioirity = GetDeliveryRequestPriority(currentInventory, minFillPercent, tank.RunOutLevel, tank.ThresholdDeliveryRequest);
                        tank.Priority = definedPrioirity == (int)DeliveryReqPriority.None ? (int)DeliveryReqPriority.CouldGo : (int)definedPrioirity;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("TankRepository", "ProcessTanksToSetPriority", ex.Message, ex);
            }
            return tankList;
        }

        private DeliveryReqPriority GetDeliveryRequestPriority(decimal currentInventory, decimal? minFillPercent, decimal? runOutLevel, decimal? reOrderLevel)
        {
            var response = DeliveryReqPriority.None;
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
            return response;
        }

        private string GetFilePath(string fileName)
        {
            var filePath = string.Format(ApplicationConstants.FilePathFormat, AzureBlobStorage.GetStorageAccountName(), BlobContainerType.TankTypeDipChart.ToString().ToLower(), fileName, AzureBlobStorage.GetSaS(BlobContainerType.TankTypeDipChart.ToString().ToLower()));
            return filePath;
        }

        public async Task<ScheduleOutputModel> GetTankDetailsBySchedule(List<ScheduleInputModel> scheduleInputDetails)
        {
            var response = new ScheduleOutputModel();

            //Tank List
            var jobIds = scheduleInputDetails.Select(t => t.JobId).Distinct().ToList();
            var tankList = new List<TankDetailListModel>();
            var jobAdditionalDetails = await mdbContext.JobAdditionalDetails.Find(x => jobIds.Contains(x.TfxJobId) && x.IsActive).ToListAsync();
            foreach (var jobId in jobIds)
            {
                var jobTanks = jobAdditionalDetails.Where(t => t.TfxJobId == jobId).Select(x => new { x.TfxJobId, x.TfxDisplayJobId, x.Tanks }).ToList();
                foreach (var job in jobTanks)
                {
                    foreach (var item in job.Tanks)
                    {
                        tankList.Add(item.ToDetailsViewModel(job.TfxJobId, job.TfxDisplayJobId));
                    }
                }
            }

            var tankModelTypeIds = tankList.Where(t => t.TankModelTypeId != null).Select(t => t.TankModelTypeId).Distinct().Select(x => ObjectId.Parse(x)).ToList();
            var tankModelType = await mdbContext.TankModalTypes.Find(t => tankModelTypeIds.Contains(t.Id) && t.IsActive).Project(x => new { x.Id, x.Name, x.Modal, x.PdfFilePath, x.ScaleMeasurement }).ToListAsync();
            foreach (var item in tankList.Where(t => t.TankModelTypeId != null))
            {
                var tankobject = tankModelType.FirstOrDefault(t => t.Id == ObjectId.Parse(item.TankModelTypeId));
                if (tankobject != null)
                {
                    item.TankMakeModel = tankobject.Name + " - " + tankobject.Modal;
                    item.TankMake = tankobject.Name;
                    item.TankModel = tankobject.Modal;
                    item.ScaleMeasurement = tankobject.ScaleMeasurement;
                    item.TankChart = GetFilePath(tankobject.PdfFilePath);
                    item.TankChartFileName = tankobject.PdfFilePath;
                }
            }

            response.TankDetailList = tankList;

            //Job List
            var jobDetails = new List<JobAdditionalDetailsModel>();
            foreach (var item in jobAdditionalDetails)
            {
                jobDetails.Add(item.ToViewModel());
            }
            response.JobDetails = jobDetails;

            //Assigned Tank Id
            var deliveryScheduleOutputDetails = new List<ScheduleTankModel>();
            foreach (var scheduleDetail in scheduleInputDetails)
            {
                if (scheduleDetail.DeliveryScheduleId != 0 || scheduleDetail.TrackableScheduleId != 0)
                {
                    var deliveryScheduleOutputItem = new ScheduleTankModel();
                    var deliveryRequest = new DeliveryRequest();
                    deliveryRequest = await mdbContext.DeliveryRequests.Find(t => t.TfxDeliveryScheduleId == scheduleDetail.DeliveryScheduleId && t.TfxTrackableScheduleId == scheduleDetail.TrackableScheduleId && t.IsActive && !t.IsDeleted).FirstOrDefaultAsync();

                    if (deliveryRequest != null)
                    {
                        var jobTanks = await mdbContext.JobAdditionalDetails.Find(x => x.TfxJobId == deliveryRequest.TfxJobId)
                                        .Project(d => new
                                        {
                                            d.TfxDisplayJobId,
                                            d.TfxJobId,
                                            Tanks = d.Tanks.Where(d1 => d1.TfxProductTypeId == deliveryRequest.TfxProductTypeId && d1.StorageTypeId == deliveryRequest.StorageTypeId && d1.StorageId == deliveryRequest.StorageId)
                                        }).ToListAsync();
                        foreach (var job in jobTanks)
                        {
                            foreach (var item in job.Tanks)
                            {
                                deliveryScheduleOutputItem.DeliveryScheduleId = scheduleDetail.DeliveryScheduleId;
                                deliveryScheduleOutputItem.TrackableScheduleId = scheduleDetail.TrackableScheduleId;
                                deliveryScheduleOutputItem.AssetId = item.TfxAssetId;
                                deliveryScheduleOutputItem.ProductTypeId = deliveryRequest.TfxProductTypeId;
                                deliveryScheduleOutputDetails.Add(deliveryScheduleOutputItem);
                            }
                        }
                    }
                }
            }

            response.ScheduleTank = deliveryScheduleOutputDetails;
            return response;
        }

        public async Task<List<JobTankDetailModel>> GetJobTankList(int jobId)
        {
            var response = new List<JobTankDetailModel>();
            var jobTanks = await mdbContext.JobAdditionalDetails.Find(t => t.TfxJobId == jobId && t.IsActive).Project(x => new { x.TfxJobId, x.TfxDisplayJobId, x.Tanks }).ToListAsync();
            foreach (var job in jobTanks)
            {
                foreach (var item in job.Tanks)
                {
                    response.Add(item.ToJobTankDetailsViewModel(job.TfxJobId, job.TfxDisplayJobId));
                }
            }

            return response;
        }

        public async Task<List<DropQuantityByPrePostDipResponseModel>> GetDropQuantityByPrePostDip(List<DropQuantityByPrePostDipRequestModel> requestModel)
        {
            var response = new List<DropQuantityByPrePostDipResponseModel>();
            try
            {
                foreach (var request in requestModel)
                {
                    var tankResponse = new DropQuantityByPrePostDipResponseModel() { JobxAssetId = request.JobxAssetId };
                    var tankDetail = await mdbContext.JobAdditionalDetails.Find(t => t.TfxJobId == request.JobId).Project(i => i.Tanks.FirstOrDefault(x => x.TfxAssetId == request.TankId)).FirstOrDefaultAsync();
                    if ((tankDetail == null || string.IsNullOrWhiteSpace(tankDetail.TankModelTypeId)) && request.ScaleMeasurement != (int)TankScaleMeasurement.Gallons && request.ScaleMeasurement != (int)TankScaleMeasurement.Litres)
                    {
                        tankResponse.StatusMessage = "Invalid tank make and model.";
                        tankResponse.MessageCode = (int)MessageCodes.InvalidTankMakeModel;
                        response.Add(tankResponse);
                        continue;
                    }
                    decimal tankMaxFill = tankDetail.FillType == (int)FillType.UoM ? tankDetail.MaxFill.Value : tankDetail.MaxFill.Value * tankDetail.FuelCapacity.Value / 100;
                    TankVolumeAndUllageInputModel preDipModel = new TankVolumeAndUllageInputModel()
                    {
                        TankMakeModelId = tankDetail.TankModelTypeId,
                        ScaleMeasurement = request.ScaleMeasurement,
                        DipValue = request.PreDipValue,
                        FuelCapacity = tankDetail.FuelCapacity ?? 0,
                        MaxFill = tankMaxFill
                    };
                    var preDipUllageModel = await GetTankVolumeAndUllage(preDipModel);
                    if (preDipUllageModel.StatusCode == (int)Status.Failed)
                    {
                        tankResponse.StatusMessage = preDipUllageModel.StatusMessage;
                        tankResponse.MessageCode = preDipUllageModel.MessageCode;
                        response.Add(tankResponse);
                        continue;
                    }
                    preDipModel.DipValue = request.PostDipValue;
                    var postDipUllageModel = await GetTankVolumeAndUllage(preDipModel);
                    if (postDipUllageModel.StatusCode == (int)Status.Failed)
                    {
                        tankResponse.StatusMessage = postDipUllageModel.StatusMessage;
                        tankResponse.MessageCode = postDipUllageModel.MessageCode;
                        response.Add(tankResponse);
                        continue;
                    }
                    tankResponse.DropQuantity = Math.Round(preDipUllageModel.TankUllage - postDipUllageModel.TankUllage, 2);
                    tankResponse.StatusCode = (int)Status.Success;
                    response.Add(tankResponse);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("TankDomain", "GetDropQuantityByPrePostDip", ex.Message, ex);
            }
            return response;
        }

        public async Task<TankVolumeAndUllageModel> GetTankVolumeAndUllage(TankVolumeAndUllageInputModel requestModel)
        {
            var response = new TankVolumeAndUllageModel();
            decimal tankDip = 0;
            TankModalType tankModelType = null;
            if (requestModel.TankMakeModelId != null && !String.IsNullOrEmpty(requestModel.TankMakeModelId) && requestModel.TankMakeModelId != "0")
            {
                tankModelType = await mdbContext.TankModalTypes.Find(x => x.Id == ObjectId.Parse(requestModel.TankMakeModelId)).FirstOrDefaultAsync();
                if (tankModelType == null)
                {
                    response.StatusMessage = "Invalid tank make and model.";
                    response.MessageCode = (int)MessageCodes.InvalidTankMakeModel;
                    return response;
                }
                tankDip = GetDipValue(requestModel, tankModelType.ScaleMeasurement);
            }
            else
            {
                tankDip = GetDipValue(requestModel, requestModel.ScaleMeasurement);
            }


            requestModel.DipValue = tankDip;
            var isValidInput = await ValidateInputTankDetails(requestModel);
            response.StatusCode = isValidInput.StatusCode;
            response.StatusMessage = isValidInput.StatusMessage;

            if (response.StatusCode == (int)Status.Success && (requestModel.ScaleMeasurement == (int)TankScaleMeasurement.Cm || requestModel.ScaleMeasurement == (int)TankScaleMeasurement.Inches))
            {
                var dipChartDetails = tankModelType.DipChartDetails.OrderBy(t => t.Dip).ToList();
                var volumeAndUllage = dipChartDetails.FirstOrDefault(t => t.Dip == tankDip);
                if (volumeAndUllage != null)
                {
                    response.TankVolume = volumeAndUllage.Volume;
                    //if (volumeAndUllage.Ullage != 0)
                    //    response.TankUllage = volumeAndUllage.Ullage;
                    //else
                    response.TankUllage = CalculateUllage(requestModel.FuelCapacity, requestModel.MaxFill, volumeAndUllage.Volume); //If Ullage is not given
                }
                else
                {
                    //Get Nearest Dip Value
                    var nearestLowerDipDetails = dipChartDetails.Where(t => t.Dip < tankDip).OrderByDescending(t => t.Dip).FirstOrDefault();
                    var nearestHigherDipDetails = dipChartDetails.Where(t => t.Dip > tankDip).OrderBy(t => t.Dip).FirstOrDefault();

                    if (nearestLowerDipDetails != null && nearestHigherDipDetails != null)
                    {
                        //Calculate Volume
                        response.TankVolume = ((nearestLowerDipDetails.Volume + nearestHigherDipDetails.Volume) / (nearestLowerDipDetails.Dip + nearestHigherDipDetails.Dip)) * tankDip;

                        //calculate ullage by max fill
                        response.TankUllage = CalculateUllage(requestModel.FuelCapacity, requestModel.MaxFill, response.TankVolume);
                        ////Calculate Ullage
                        //if (nearestLowerDipDetails.Ullage != 0 && nearestHigherDipDetails.Ullage != 0)
                        //{
                        //    response.TankUllage = ((nearestLowerDipDetails.Ullage + nearestHigherDipDetails.Ullage) / (nearestLowerDipDetails.Dip + nearestHigherDipDetails.Dip)) * tankDip;
                        //}
                        //else
                        //{
                        //    //If Ullage is not given
                        //    var nearestLowerTankUllage = CalculateUllage(requestModel.FuelCapacity, requestModel.MaxFill, nearestLowerDipDetails.Volume);
                        //    var nearestHigherTankUllage = CalculateUllage(requestModel.FuelCapacity, requestModel.MaxFill, nearestHigherDipDetails.Volume);
                        //    response.TankUllage = ((nearestLowerTankUllage + nearestHigherTankUllage) / (nearestLowerDipDetails.Dip + nearestHigherDipDetails.Dip)) * tankDip;
                        //}
                    }
                }
            }
            else if (response.StatusCode == (int)Status.Success && (requestModel.ScaleMeasurement == (int)TankScaleMeasurement.Gallons || requestModel.ScaleMeasurement == (int)TankScaleMeasurement.Litres))
            {
                response.TankVolume = requestModel.DipValue;
                response.TankUllage = CalculateUllage(requestModel.FuelCapacity, requestModel.MaxFill, response.TankVolume);
            }

            if (response.TankVolume > requestModel.MaxFill)
            {
                response.StatusCode = (int)Status.Failed;
                response.StatusMessage = "Entered dip test value is greater than max fill capacity.";
                response.MessageCode = (int)MessageCodes.DipValueGreaterThanMaxCapacity;
            }
            return response;
        }

        private decimal GetDipValue(TankVolumeAndUllageInputModel requestModel, int tankScaleMeasurement)
        {
            decimal tankDip = 0;
            if (requestModel.ScaleMeasurement == tankScaleMeasurement)
            {
                //From MobielApp ScaleMeasurement Parameter is Passed in Cm or Inches and Tank Chart is also defined in Same Scale Measurement Cm or Inches.
                tankDip = requestModel.DipValue;
            }
            else if (requestModel.ScaleMeasurement != tankScaleMeasurement)
            {
                if (requestModel.ScaleMeasurement == (int)TankScaleMeasurement.Cm)
                {
                    //From MobielApp ScaleMeasurement Parameter is Passed in Cm and Tank Chart is Defined in Inches. 
                    //So Cm should be Converted to Inches to get TankUllage and TankVolume of specific dipValue
                    var dipInches = requestModel.DipValue * Convert.ToDecimal(0.39370);
                    tankDip = dipInches.GetPreciseValue(2);
                }
                else if (requestModel.ScaleMeasurement == (int)TankScaleMeasurement.Inches)
                {
                    //From MobielApp ScaleMeasurement Parameter is Passed in Inches and Tank Chart is Defined in Cm. 
                    //So Inches should be Converted to Cm to get TankUllage and TankVolume of specific dipValue
                    var dipCm = requestModel.DipValue * Convert.ToDecimal(2.54);
                    tankDip = dipCm.GetPreciseValue(2);
                }
                else if (requestModel.ScaleMeasurement == (int)TankScaleMeasurement.Gallons || requestModel.ScaleMeasurement == (int)TankScaleMeasurement.Litres)
                {
                    tankDip = requestModel.DipValue;
                }
            }
            return tankDip;
        }

        private decimal CalculateUllage(decimal fuelCapacity, decimal maxFill, decimal currentVolume)
        {
            var maxFillPercent = (maxFill * 100) / fuelCapacity;
            maxFillPercent = maxFillPercent / 100;
            var response = (maxFillPercent * fuelCapacity) - currentVolume;
            if (response > 0)
                return response;
            else
                return 0;
        }

        private async Task<StatusModel> ValidateInputTankDetails(TankVolumeAndUllageInputModel requestModel)
        {
            var response = new StatusModel();

            if (string.IsNullOrEmpty(requestModel.TankMakeModelId) && (requestModel.ScaleMeasurement == 1 && requestModel.ScaleMeasurement == 2))
            {
                response.StatusCode = (int)Status.Failed;
                response.StatusMessage = "Tank make and model is required";
                response.MessageCode = (int)MessageCodes.TankMakeModelRequired;
                return response;
            }
            else if (requestModel.ScaleMeasurement == 0)
            {
                response.StatusCode = (int)Status.Failed;
                response.StatusMessage = "Scale measurement is required";
                response.MessageCode = (int)MessageCodes.ScaleMeasurementRequired;
                return response;
            }
            else if (requestModel.DipValue == 0)
            {
                response.StatusCode = (int)Status.Failed;
                response.StatusMessage = "Dip test value is required";
                response.MessageCode = (int)MessageCodes.DipValueRequired;
                return response;
            }
            else if (requestModel.FuelCapacity == 0)
            {
                response.StatusCode = (int)Status.Failed;
                response.StatusMessage = "Fuel capacity is required";
                response.MessageCode = (int)MessageCodes.FuelCapacityRequired;
                return response;
            }

            if (requestModel.ScaleMeasurement == (int)TankScaleMeasurement.Cm || requestModel.ScaleMeasurement == (int)TankScaleMeasurement.Inches)
            {
                var tankModelType = await mdbContext.TankModalTypes.Find(x => x.Id == ObjectId.Parse(requestModel.TankMakeModelId)).FirstOrDefaultAsync();
                var dipChartDetails = tankModelType.DipChartDetails.OrderBy(t => t.Dip).ToList();

                var dipLowerValue = dipChartDetails.FirstOrDefault();
                if (dipLowerValue != null && requestModel.DipValue < dipLowerValue.Dip)
                {
                    response.StatusCode = (int)Status.Failed;
                    response.StatusMessage = "Entered dip test value is smaller than minimum value of Dip chart.";
                    response.MessageCode = (int)MessageCodes.DipValSmallerThanMinValOfDipChart;
                    return response;
                }
                else
                {
                    var dipHigherValue = dipChartDetails.LastOrDefault();
                    if (dipHigherValue != null && requestModel.DipValue > dipHigherValue.Dip)
                    {
                        response.StatusCode = (int)Status.Failed;
                        response.StatusMessage = "Entered dip test value is higher than maximum value of Dip chart.";
                        response.MessageCode = (int)MessageCodes.DipValMoreThanMaxValOfDipChart;
                        return response;
                    }
                }
            }
            response.StatusCode = (int)Status.Success;
            response.StatusMessage = Resource.errMessageSuccess;
            return response;
        }

        public async Task<bool> OrderTankMappings(List<OrderTankMappingViewModel> model)
        {
            try
            {
                var entity = model.ToEntity();
                foreach (var item in entity)
                {
                    await mdbContext.OrderTanks.FindOneAndDeleteAsync(t => t.JobId == item.JobId && t.TankId == item.TankId && t.OrderId == item.OrderId);
                }
                await mdbContext.OrderTanks.InsertManyAsync(entity);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("TankRepository", "OrderTankMappings", ex.Message, ex);
                return false;
            }
            return true;
        }

        public async Task<List<DropdownDisplayExtended>> GetTankModelType(List<int> companyId)
        {
            List<DropdownDisplayExtended> response = new List<DropdownDisplayExtended>();
            try
            {
                var tankTypes = await mdbContext.TankModalTypes.Find(t => companyId.Contains(t.CreatedByCompanyId)).ToListAsync();
                tankTypes.ForEach(t => response.Add(new DropdownDisplayExtended() { Id = t.Id.ToString(), Name = t.Name, Code = t.Modal }));
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("TankRepository", "GetTankModelType", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusModel> SaveTankTypeDipChart(TankModalTypeViewModel tankType)
        {
            var response = new StatusModel();

            var record = await mdbContext.TankModalTypes.Find(t => (t.BuyerCompanyId == tankType.BuyerCompanyId || t.SupplierCompanyId == tankType.SupplierCompanyId) && t.IsActive && t.Name.ToLower() == tankType.Name.ToLower() && t.Modal.ToLower() == tankType.Modal.ToLower()).ToListAsync();
            if (record.Count > 0)
            {
                response.StatusMessage = string.Format(Resource.errorTankMakeAndModelAlreadyExist, tankType.Name, tankType.Modal);
            }
            else
            {
                var entity = tankType.ToEntity();
                await mdbContext.TankModalTypes.InsertOneAsync(entity);
                response.StatusCode = (int)Status.Success;
                response.StatusMessage = string.Format(Resource.successTankTypeDipChartSaved, tankType.Name, tankType.Modal);
            }
            return response;

        }

        public async Task<List<TankModalTypeViewModel>> GetTankTypesByCompany(int companyId)
        {
            var response = new List<TankModalTypeViewModel>();
            var tanks = await mdbContext.TankModalTypes.Find(t => t.CreatedByCompanyId == companyId && t.IsActive).ToListAsync();
            foreach (var job in tanks)
            {
                response.Add(job.ToViewModel());
            }
            return response;
        }

        public async Task<StatusModel> DeleteTankDipChartById(string id)
        {
            var response = new StatusModel();

            //if tank type is already assigned to a tank

            var filter = Builders<JobAdditionalDetail>.Filter.And(
                 Builders<JobAdditionalDetail>.Filter.Where(t => t.IsActive),
                 Builders<JobAdditionalDetail>.Filter.Where(t => t.Tanks.Any(y => y.TankModelTypeId == id))
                );

            var tankAlreadyAssigned = mdbContext.JobAdditionalDetails.Find(filter).Any();
            if (!tankAlreadyAssigned)
            {
                var tankModalTypeTilter = Builders<TankModalType>.Filter.And(Builders<TankModalType>.Filter.Where(x => x.Id == ObjectId.Parse(id)));
                var data_ = await mdbContext.TankModalTypes.DeleteOneAsync(tankModalTypeTilter);
                if (data_.DeletedCount > 0)
                {
                    response.StatusCode = (int)Status.Success;
                    response.StatusMessage = string.Format(Resource.successTankMakeAndModelDeleted);
                }
                else
                {
                    response.StatusMessage = Resource.errRecordNotFound;
                }
            }
            else
            {
                response.StatusMessage = Resource.errorTankMakeAssignedToTank;
            }
            return response;
        }
        public async Task<List<string>> GetAllTankTypeNameForDipChart(int companyId, string searchValue = "")
        {
            var response = new List<string>();

            var data_ = mdbContext.TankModalTypes.Find(x => x.CreatedByCompanyId == companyId && x.IsActive && x.Name.ToLower().Contains(searchValue)).ToListAsync().Result;
            response = data_.Select(t => t.Name).ToList();

            return response;
        }

        public async Task<StatusModel> UpdateTankSequence(TankDetailsModel model)
        {
            var response = new StatusModel(Status.Failed);
            try
            {
                var filter = Builders<JobAdditionalDetail>.Filter.And(
               Builders<JobAdditionalDetail>.Filter.Where(x => x.TfxJobId == model.JobId),
               Builders<JobAdditionalDetail>.Filter.Where(x => x.Tanks.Any(t => t.TfxAssetId == model.AssetId)),
               Builders<JobAdditionalDetail>.Filter.Where(x => x.IsActive)
           );
                var update = Builders<JobAdditionalDetail>.Update.Set(x => x.Tanks[-1].TankSequence, model.TankSequence);
                await mdbContext.JobAdditionalDetails.FindOneAndUpdateAsync(filter, update);
                response.StatusCode = (int)Status.Success;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("TankRepository", "UpdateTankSequence", ex.Message, ex);
                response.StatusCode = (int)Status.Failed;
                response.StatusMessage = Status.Failed.ToString();
            }
            return response;
        }

        public async Task<bool> CheckDuplicateTankSequence(TankDetailsModel model)
        {
            var response = true;
            try
            {
                var tank = await mdbContext.JobAdditionalDetails.Find(x => x.TfxJobId == model.JobId && x.IsActive && x.Tanks.Any(t => t.TankSequence == model.TankSequence && t.TfxAssetId != model.AssetId)).FirstOrDefaultAsync();
                if (tank != null)
                {
                    response = true;
                }
                else
                {
                    response = false;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("TankRepository", "CheckDuplicateTankSequence", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<JobTankAdditionalDetailModel>> GetJobsTankList(List<int> jobId)
        {
            var response = new List<JobTankAdditionalDetailModel>();
            var jobTanks = await mdbContext.JobAdditionalDetails.Find(t => jobId.Contains(t.TfxJobId) && t.IsActive).Project(x => new { x.TfxJobId, x.TfxDisplayJobId, x.Tanks, x.TfxJobName }).ToListAsync();
            foreach (var job in jobTanks)
            {
                foreach (var item in job.Tanks)
                {
                    response.Add(item.ToJobTankAdditionalDetailsViewModel(job.TfxJobId, job.TfxDisplayJobId, job.TfxJobName));
                }
            }
            var tankIds = response.Select(x => x.TankId).ToList();
            var storageIds = response.Select(x => x.StorageId).ToList();
            var siteIds = response.Select(x => x.SiteId).ToList();

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

            context.Database.CommandTimeout = 30;
            var tankRounOutSalesData = await context.Database.SqlQuery<TankRounOutSaleData>("usp_GetSalesTankRuntOutData @SiteList,@TankList,@StorageList", siteIdParam, tankIdsParam, storageIdParams).ToListAsync();
            if (tankRounOutSalesData.Any())
            {
                foreach (var tankitem in response)
                {
                    var tankInfo = tankRounOutSalesData.FirstOrDefault(x => x.SiteId == tankitem.SiteId && x.TankId == tankitem.TankId && x.StorageId == tankitem.StorageId);
                    if (tankInfo != null)
                    {
                        decimal runOutLevelQty = 0;
                        if (tankitem.FillType == (int)FillType.Percent)
                        {
                            runOutLevelQty = ((decimal)(tankitem.RunOut * tankitem.FuelCapacity) / 100).GetPreciseValue(1);
                        }
                        else
                        {
                            runOutLevelQty = tankitem.RunOut.GetPreciseValue(2);
                        }
                        tankitem.ISRunOut = tankInfo.InventoryData < runOutLevelQty;
                        tankitem.DaysRemaining = tankInfo.DaysRemaining;
                    }

                }
            }
            return response.ToList();
        }

        private static void IntializeDemandsParameters(List<string> siteIdList, List<string> tankIdList, List<string> storageList, out DataTable SiteIds, out DataTable TankIds, out DataTable StorageIds)
        {
            SiteIds = CreateTable();
            foreach (var item in siteIdList.Distinct().ToList())
            {
                var row = SiteIds.NewRow();
                row["SearchVar"] = item;
                SiteIds.Rows.Add(row);
            }
            //Create TankIds Table
            TankIds = CreateTable();
            foreach (var item in tankIdList.Distinct().ToList())
            {
                var row = TankIds.NewRow();
                row["SearchVar"] = item;
                TankIds.Rows.Add(row);
            }

            //Create StorageIds Table
            StorageIds = CreateTable();
            foreach (var item in storageList.Distinct().ToList())
            {
                var row = StorageIds.NewRow();
                row["SearchVar"] = item;
                StorageIds.Rows.Add(row);
            }
        }
        private static DataTable CreateTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("SearchVar", typeof(string));
            return dt;
        }
    }
}
