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
using SiteFuel.Exchange.Logger;
using TrueFill.DemandCaptureDataAccess;
using SiteFuel.Exchange.Core.Infrastructure;
using TrueFill.ExchangeDataAccess.DataAccess;
using System.Data;
using System.Data.SqlClient;
using SiteFuel.MdbDataAccess;

namespace SiteFuel.FreightRepository
{
    public class JobRepository : IJobRepository
    {
        private readonly MdbContext mdbContext;
        private readonly DemandCaptureContext context = new DemandCaptureContext();
        public JobRepository()
        {
            if (mdbContext == null)
            {
                mdbContext = new MdbContext();
            }
        }
        public JobRepository(MdbContext _mdbcontext)
        {
            mdbContext = _mdbcontext;
        }
        public async Task<JobAdditionalDetailsModel> GetAdditionalJobDetails(int jobId, int supplierCompanyId)
        {
            var response = new JobAdditionalDetailsModel();
            try
            {
                var entity = await mdbContext.JobAdditionalDetails.Find(x => x.TfxJobId == jobId && x.IsActive).FirstOrDefaultAsync();
                if (entity != null)
                    response = entity.ToViewModel();
                if (supplierCompanyId > 0)
                {
                    response.RegionId = await new RegionRepository(mdbContext).GetRegionIdForJob(jobId, supplierCompanyId);
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("Job Repository", "GetAdditionalJobDetails", ex.Message, ex);
            }
            return response;
        }
        public async Task<JobLocationRelatedDetailsModel> GetJobLocationRelatedDetails(int companyId, string jobId, bool IsBuyerCompany)
        {
            var response = new JobLocationRelatedDetailsModel();
            try
            {

                List<int> jobIdArray = jobId.Split(',').Select(int.Parse).ToList();

                //get delivery request for particular job.
                var deliveryRequests = (from dr in mdbContext.DeliveryRequests.AsQueryable()
                                        where (dr.TfxSupplierCompanyId == companyId || dr.TfxAssignedToCompanyId == companyId || dr.TfxCreatedByCompanyId == companyId || IsBuyerCompany)
                                        && (dr.ScheduleShiftEndDateTime == null || dr.ScheduleShiftEndDateTime.Value > DateTimeOffset.UtcNow.AddDays(-2).DateTime)
                                               && !dr.GroupChildDRs.Any() && dr.Status != DeliveryReqStatus.Deleted && !dr.IsDeleted && (dr.ParentId == null || dr.ScheduleBuilderId != null) && dr.IsActive && jobIdArray.Contains(dr.TfxJobId)
                                                && (dr.TfxScheduleStatus == (int)DeliveryScheduleStatus.None || dr.TfxScheduleStatus == (int)DeliveryScheduleStatus.New ||
                                                       dr.TfxScheduleStatus == (int)DeliveryScheduleStatus.Acknowledged || dr.TfxScheduleStatus == (int)DeliveryScheduleStatus.Accepted || dr.TfxScheduleStatus == (int)DeliveryScheduleStatus.Modified)
                                              && dr.TfxScheduleEnrouteStatus != (int)EnrouteDeliveryStatus.CompletedDrop
                                        select dr
                                        ).OrderBy(x => x.CurrentThreshold).ToList();

                int missedSchedulePeriod = 4;
                if (deliveryRequests.Any())
                {
                    ExchangeAccess exchange = new ExchangeAccess();
                    int.TryParse(exchange.GetAppSetting("MissedScheduleWaitingPeriod"), out missedSchedulePeriod);
                }
                DateTimeOffset currentTime = DateTimeOffset.UtcNow;
                var query = from item in deliveryRequests
                            let jobTime = currentTime.Add(item.JobTimeZoneOffset)
                            where item.ScheduleShiftEndDateTime == null
                            || item.ScheduleShiftEndDateTime.Value.AddHours(missedSchedulePeriod) >= jobTime.DateTime
                            select item;
                deliveryRequests = query.ToList();
                deliveryRequests.ForEach(dr =>
                {
                    response.deliveryRequestViewModels.Add(dr.ToDeliveryRequestViewModel());
                });

                //get regions
                var regJobs = await (mdbContext.Regions.Find(t => t.TfxCompanyId == companyId &&
                                                          t.IsActive && !t.IsDeleted &&
                                                          t.TfxJobs.Any(t1 => jobIdArray.Contains(t1.Id))).Project(t => t.TfxJobs.Select(t1 => new { jobId = t1.Id, regionId = t.Id.ToString() })).ToListAsync());
                var regionJobs = regJobs?.SelectMany(t => t).Distinct().ToList();

                //get job additional details.
                var jobAdditionalDetails = await mdbContext.JobAdditionalDetails.Find(x => jobIdArray.Contains(x.TfxJobId) && x.IsActive).ToListAsync();
                jobAdditionalDetails.ForEach(dr => response.jobAdditionalDetailsModels.Add(dr.ToViewModel()));

                foreach (var jobitem in response.jobAdditionalDetailsModels)
                {
                    if (jobitem.TankDetails != null && jobitem.TankDetails.Any())
                    {
                        foreach (var jobTankItem in jobitem.TankDetails)
                        {
                            if (jobTankItem.TankModelTypeId != null)
                            {
                                ObjectId objectId = new ObjectId(jobTankItem.TankModelTypeId);
                                var tankModelDetails = mdbContext.TankModalTypes.Find(top => top.Id == objectId).FirstOrDefault();
                                if (tankModelDetails != null)
                                {
                                    jobTankItem.TankChartPath = GetFilePath(tankModelDetails.PdfFilePath);
                                    jobTankItem.TankName = tankModelDetails.Name;
                                    jobTankItem.TankNumber = tankModelDetails.Modal;
                                }
                                else
                                {
                                    jobTankItem.TankName = string.Empty;
                                    jobTankItem.TankNumber = string.Empty;
                                }
                            }
                            else
                            {
                                jobTankItem.TankName = string.Empty;
                                jobTankItem.TankNumber = string.Empty;
                            }
                        }
                    }
                    if (regionJobs != null && regionJobs.Any())
                    {
                        jobitem.RegionId = regionJobs.Where(t => t.jobId == jobitem.JobId).Select(t => t.regionId).FirstOrDefault();
                    }
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobRepository", "GetJobLocationRelatedDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<JobDRDetailsModel>> GetJobDRPrioritiesForBuyer(JobDRPriorityInputModel request)
        {
            var response = new List<JobDRDetailsModel>();
            try
            {
                response = (from dr in mdbContext.DeliveryRequests.AsQueryable()
                            where dr.Status != DeliveryReqStatus.Deleted && dr.IsActive && request.JobIds.Contains(dr.TfxJobId) && dr.TfxScheduleStatus != 7
                                    && dr.TfxScheduleStatus != 8 && dr.TfxScheduleStatus != 9 && dr.TfxScheduleStatus != 10 && dr.TfxScheduleEnrouteStatus != 4
                                    && dr.TfxScheduleStatus != 11 && dr.TfxScheduleStatus != 12
                            select new JobDRDetailsModel { JobId = dr.TfxJobId, Priority = dr.Priority }
                                       ).Distinct().ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobRepository", "GetJobDRPrioritiesForBuyer", ex.Message, ex);
            }
            return response;
        }

        private static void IntializeDemandsParameters(List<string> IdList, out DataTable Ids)
        {
            Ids = CreateTable();
            foreach (var item in IdList.Distinct().ToList())
            {
                var row = Ids.NewRow();
                row["SearchVar"] = item;
                Ids.Rows.Add(row);
            }
        }
        private static DataTable CreateTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("SearchVar", typeof(string));
            return dt;
        }


        public List<JobDipChartDetails> GetDipTestDetails(string siteId, string tankId, int noOfDays)
        {
            List<JobDipChartDetails> jobDipChartDetails = new List<JobDipChartDetails>();

            DateTimeOffset startDate = DateTimeOffset.Now;
            DateTimeOffset endDate = DateTimeOffset.Now.AddDays(-noOfDays);
            var inputmodel = new
            {
                SiteId = siteId,
                TankId = tankId,
                StartDate = startDate,
                EndDate = endDate
            };

            var input = SqlHelperMethods.GetStoredProcedure("usp_GetDemandsBySiteIdTankIdCaptureTime", inputmodel);
            context.Database.CommandTimeout = 500;
            var demands = context.Database.SqlQuery<DemandModel>(input.Query, input.Params.ToArray()).ToList();

            foreach (var item in demands)
            {
                jobDipChartDetails.Add(new JobDipChartDetails { TankId = item.TankId, CaptureTime = item.CaptureTime, GrossVolume = item.GrossVolume, NetVolume = item.NetVolume, SiteId = item.SiteId, Ullage = item.Ullage, CaptureTimeString = item.CaptureTime.ToString() });
            }
            return jobDipChartDetails;
        }
        public List<JobDipChartDetails> GetDemandCaptureChartData(List<String> siteId, List<String> tankId, int noOfDays)
        {
            List<JobDipChartDetails> jobDipChartDetails = new List<JobDipChartDetails>();

            DateTimeOffset startDate = DateTimeOffset.Now;
            DateTimeOffset endDate = DateTimeOffset.Now.AddDays(-noOfDays);

            DataTable tankIds, siteIds;
            IntializeDemandsParameters(tankId, out tankIds);
            var tankIdsParam = new SqlParameter("@TankIds", SqlDbType.Structured);
            tankIdsParam.Value = tankIds;
            tankIdsParam.TypeName = "dbo.DemandsSearchTypes";

            IntializeDemandsParameters(siteId, out siteIds);
            var siteIdsParams = new SqlParameter("@SiteIds", SqlDbType.Structured);
            siteIdsParams.Value = siteIds;
            siteIdsParams.TypeName = "dbo.DemandsSearchTypes";

            var startDateParam = new SqlParameter("@StartDate", SqlDbType.DateTimeOffset);
            startDateParam.Value = startDate;
            var endDateParam = new SqlParameter("@EndDate", SqlDbType.DateTimeOffset);
            endDateParam.Value = endDate;

            context.Database.CommandTimeout = 500;
            var demands = context.Database.SqlQuery<DemandModel>("usp_GetDemandsBySiteIdsTankIdsCaptureTime @SiteIds,@TankIds,@StartDate,@EndDate", siteIdsParams, tankIdsParam, startDateParam, endDateParam).ToList();
            if (demands != null)
            {
                foreach (var item in demands)
                {
                    jobDipChartDetails.Add(new JobDipChartDetails { TankId = item.TankId, CaptureTime = item.CaptureTime, GrossVolume = item.GrossVolume, NetVolume = item.NetVolume, SiteId = item.SiteId, Ullage = item.Ullage, CaptureTimeString = item.CaptureTime.ToString() });
                }
            }
            return jobDipChartDetails;
        }

        public async Task<bool> RemoveJobAdditionalDetails(int jobId)
        {
            try
            {
                await mdbContext.JobAdditionalDetails.FindOneAndDeleteAsync(testc => testc.TfxJobId == jobId);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> SaveAdditionalJobDetails(JobAdditionalDetailsModel table)
        {
            try
            {
                var entity = table.ToEntity();
                await mdbContext.JobAdditionalDetails.InsertOneAsync(entity);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobRepository", "SaveAdditionalJobDetails", ex.Message, ex);
                return false;
            }
            return true;
        }
        public async Task<bool> UpdateDistanceCoveredOfAdditionalJobDetail(int JobId, string DistanceCovered)
        {
            var response = new JobAdditionalDetailsModel();
            try
            {
                response = await GetAdditionalJobDetails(JobId, 0);
                if (response != null)
                {
                    response.DistanceCovered = DistanceCovered;
                    return await UpdateAdditionalJobDetails(response);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobRepository", "UpdateDistanceCoveredForAdditionalJobDetail", ex.Message, ex);
                return false;
            }
            return false;
        }
        public async Task<bool> UpdateAdditionalJobDetails(JobAdditionalDetailsModel table)
        {
            try
            {
                var entity = table.ToEntity();
                var prevField = await mdbContext.JobAdditionalDetails.Find(t => t.TfxJobId == table.JobId && t.IsActive).FirstOrDefaultAsync();
                if (prevField != null)
                {
                    entity.Id = prevField.Id;
                    entity.Tanks = prevField.Tanks;
                    await mdbContext.JobAdditionalDetails.ReplaceOneAsync(Builders<JobAdditionalDetail>.Filter.Eq(t => t.Id, prevField.Id), entity);
                    if (entity.TfxJobName != prevField.TfxJobName)
                    {
                        UpdateJobNameInRegion(table.JobId, table.JobName);
                        UpdateJobNameInCarrierJobs(table.JobId, table.JobName);
                    }
                    if (entity.TfxDisplayJobId != prevField.TfxDisplayJobId)
                    {
                        if (string.IsNullOrWhiteSpace(prevField.TfxDisplayJobId))
                        {
                            var inputmodel = new
                            {
                                SiteId = entity.TfxDisplayJobId,
                                PreviousSiteID = string.Empty
                            };
                            var input = SqlHelperMethods.GetStoredProcedure("usp_Update_Demands_SiteId_Information", inputmodel);
                            await context.Database.SqlQuery<object>(input.Query, input.Params.ToArray()).FirstOrDefaultAsync();
                        }
                        else
                        {
                            var inputmodel = new
                            {
                                SiteId = entity.TfxDisplayJobId,
                                PreviousSiteID = prevField.TfxDisplayJobId
                            };
                            var input = SqlHelperMethods.GetStoredProcedure("usp_Update_Demands_SiteId_Information", inputmodel);
                            await context.Database.SqlQuery<object>(input.Query, input.Params.ToArray()).FirstOrDefaultAsync();
                        }
                        await context.SaveChangesAsync();
                        var updateFields = Builders<HeldDeliveryRequest>.Update
                                            .Set(t => t.SiteId, entity.TfxDisplayJobId)
                                            .Set(t => t.UpdatedOn, DateTimeOffset.Now);
                        var filter = Builders<HeldDeliveryRequest>.Filter.And(Builders<HeldDeliveryRequest>.Filter.Where(x => x.JobId == table.JobId && x.IsActive && x.Status != HeldDrStatus.Passed));

                        await mdbContext.HeldDeliveryRequests.UpdateOneAsync(filter, updateFields);
                    }
                }
                else
                {
                    await mdbContext.JobAdditionalDetails.InsertOneAsync(entity);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobRepository", "UpdateAdditionalJobDetails", ex.Message, ex);
                return false;
            }
            return true;
        }

        public async Task<bool> DeleteJobTanks(DeleteTanksModel deleteTankModel)
        {
            try
            {
                foreach (int jobId in deleteTankModel.JobIds)
                {
                    var prevField = await mdbContext.JobAdditionalDetails.Find(t => t.TfxJobId == jobId && t.IsActive).FirstOrDefaultAsync();
                    if (prevField != null)
                    {
                        var update = Builders<JobAdditionalDetail>.Update.PullFilter(p => p.Tanks,
                                                f => deleteTankModel.TankIds.Contains(f.TfxAssetId));
                        var result = await mdbContext.JobAdditionalDetails.FindOneAndUpdateAsync(t => t.TfxJobId == jobId && t.IsActive, update);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobRepository", "DeleteJobTanks", ex.Message, ex);
                return false;
            }
            return true;
        }

        public async Task<StatusModel> AssignToSupplier(List<CarrierViewModel> supplierCarriers)
        {
            var response = new StatusModel();
            using (var session = await mdbContext.Client.StartSessionAsync())
            {
                session.StartTransaction();
                try
                {
                    foreach (var carrier in supplierCarriers)
                    {
                        Carrier entity = carrier.ToEntity();
                        bool carrierExists = mdbContext.Carriers.Find(t => t.TfxCarrierCompanyId == carrier.Carrier.Id && t.TfxSupplierCompanyId == carrier.SupplierCompanyId && t.IsActive && !t.IsDeleted).Any();
                        if (!carrierExists)
                        {
                            await mdbContext.Carriers.InsertOneAsync(entity);
                        }
                        if (carrier.Jobs != null)
                        {
                            foreach (var job in carrier.Jobs)
                            {
                                bool carrierJobExists = mdbContext.CarrierJobs.Find(t => t.TfxCarrierCompanyId == carrier.Carrier.Id && t.TfxSupplierCompanyId == carrier.SupplierCompanyId && t.TfxJobId == job.Job.Id && t.IsActive).Any();
                                if (!carrierJobExists)
                                {
                                    var carrierJob = new CarrierJob() { TfxCarrierCompanyId = carrier.Carrier.Id, TfxJobId = job.Job.Id, TfxJobName = job.Job.Name, TfxJobCompanyId = job.BuyerCompanyId, TfxBuyerCompanyName = job.BuyerCompanyName, TfxSupplierCompanyId = carrier.SupplierCompanyId, IsActive = true };
                                    await mdbContext.CarrierJobs.InsertOneAsync(carrierJob);
                                }
                            }
                        }
                    }
                    await session.CommitTransactionAsync();
                    response.StatusCode = (int)Status.Success;
                }
                catch (Exception)
                {
                    await session.AbortTransactionAsync();
                    response.StatusCode = (int)Status.Failed;
                    throw;
                }
            }
            return response;
        }

        public async Task<string> getRegionByJobAndCompanyId(int jobId, int companyId)
        {
            var region = await mdbContext.Regions.FindAsync(t => t.TfxCompanyId == companyId &&
                                                              t.IsActive && !t.IsDeleted &&
                                                              t.TfxJobs.Any(t1 => t1.Id == jobId));

            if (region != null)
            {
                var regionId = region.ToList().Select(t => t.Id.ToString()).FirstOrDefault();
                return regionId;
            }
            return string.Empty;
        }

        public async Task<StatusModel> AssignTPOJobToRegion(JobToRegionAssignViewModel jobToUpdate)
        {
            var response = new StatusModel(Status.Failed);
            try
            {
                //Find the region with given job and company
                var regionId = await GetRegionIdForJob(jobToUpdate.JobId, jobToUpdate.CompanyId);
                if (regionId == jobToUpdate.RegionId)// No Assignment
                {
                    response.StatusCode = (int)Status.Success;
                    return response;
                }
                //Implies No Region Assigned To Job. Current Assignment will be removed
                if ((jobToUpdate.RegionId == null || jobToUpdate.RegionId == string.Empty) && regionId != null)
                {
                    var objectId = ObjectId.Parse(regionId);
                    var filter = Builders<Region>.Filter.And(
                            Builders<Region>.Filter.Where(x => x.Id == objectId),
                            Builders<Region>.Filter.Where(x => x.TfxCompanyId == jobToUpdate.CompanyId),
                            Builders<Region>.Filter.Where(x => x.IsActive),
                            Builders<Region>.Filter.Where(x => !x.IsDeleted)
                        );
                    var update = Builders<Region>.Update.PullFilter(p => p.TfxJobs, f => f.Id == jobToUpdate.JobId);
                    await mdbContext.Regions.FindOneAndUpdateAsync(filter, update);
                    response.StatusCode = (int)Status.Success;
                    response.StatusMessage = "Job " + jobToUpdate.JobName + " Succesfully Removed from Region";
                    return response;
                }
                if ((regionId != jobToUpdate.RegionId) && regionId != null)//Implies new Region is selected for given job 
                {
                    //First Remove the given job from existing region as one job can belong to one region only 
                    var objectId = ObjectId.Parse(regionId);
                    var filter = Builders<Region>.Filter.And(
                            Builders<Region>.Filter.Where(x => x.Id == objectId),
                            Builders<Region>.Filter.Where(x => x.TfxCompanyId == jobToUpdate.CompanyId),
                            Builders<Region>.Filter.Where(x => x.IsActive),
                            Builders<Region>.Filter.Where(x => !x.IsDeleted)
                        );
                    var update = Builders<Region>.Update.PullFilter(p => p.TfxJobs, f => f.Id == jobToUpdate.JobId);
                    await mdbContext.Regions.FindOneAndUpdateAsync(filter, update);
                    return await AddJobToRegion(jobToUpdate);
                }
                else
                {
                    response = await AddJobToRegion(jobToUpdate);//First time job Assignment
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobRepository", "AssignTPOJobToRegion", ex.Message, ex);
                response.StatusCode = (int)Status.Failed;
                response.StatusMessage = Status.Failed.ToString();
            }
            return response;
        }
        public async Task<StatusModel> AssignTPOJobToRoute(JobToRegionAssignViewModel jobToUpdate)
        {
            var response = new StatusModel(Status.Failed);
            try
            {
                //Find the routeId with given job and company
                var routeId = await GetRoutesIdForJob(jobToUpdate.JobId, jobToUpdate.CompanyId);
                if (routeId == jobToUpdate.RouteId)// No Assignment
                {
                    response.StatusCode = (int)Status.Success;
                    return response;
                }
                //Implies No Routes Assigned To Job. Current Assignment will be removed
                if ((jobToUpdate.RouteId == null || jobToUpdate.RouteId == string.Empty) && routeId != null)
                {
                    await UpdateRouteInformation(jobToUpdate, routeId);
                    response.StatusCode = (int)Status.Success;
                    response.StatusMessage = "Job " + jobToUpdate.JobName + " Succesfully Removed from Routes";
                    return response;
                }
                if ((routeId != jobToUpdate.RouteId) && routeId != null)//Implies new Routes is selected for given job 
                {
                    //First Remove the given job from existing region as one job can belong to one region only 
                    await UpdateRouteInformation(jobToUpdate, routeId);
                    return await AddJobToRouteInfo(jobToUpdate);
                }
                else
                {
                    response = await AddJobToRouteInfo(jobToUpdate);//First time job Assignment
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobRepository", "AssignTPOJobToRoute", ex.Message, ex);
                response.StatusCode = (int)Status.Failed;
                response.StatusMessage = Status.Failed.ToString();
            }
            return response;
        }

        private async Task UpdateRouteInformation(JobToRegionAssignViewModel jobToUpdate, string routeId)
        {
            var objectId = ObjectId.Parse(routeId);
            var filter = Builders<RouteInformations>.Filter.And(
                    Builders<RouteInformations>.Filter.Where(x => x.Id == objectId),
                    Builders<RouteInformations>.Filter.Where(x => x.TfxCompanyId == jobToUpdate.CompanyId),
                    Builders<RouteInformations>.Filter.Where(x => x.IsActive),
                    Builders<RouteInformations>.Filter.Where(x => !x.IsDeleted)
                );
            var update = Builders<RouteInformations>.Update.PullFilter(p => p.TfxJobs, f => f.Id == jobToUpdate.JobId);
            await mdbContext.RouteInformations.FindOneAndUpdateAsync(filter, update);
        }

        public async Task<string> GetRegionIdForJob(int jobId, int companyId)
        {
            string regionId = string.Empty;
            try
            {
                var region = await (mdbContext.Regions.FindAsync(t => t.TfxCompanyId == companyId &&
                                                                t.IsActive && !t.IsDeleted &&
                                                                t.TfxJobs.Any(t1 => t1.Id == jobId)));

                if (region != null)
                {
                    regionId = region.ToList().Select(t => t.Id.ToString()).FirstOrDefault();
                }

            }
            catch (Exception ex)
            {

                LogManager.Logger.WriteException("JobRepository", "GetRegionIdForJob", ex.Message, ex);
            }
            return regionId;

        }
        public async Task<string> GetRoutesIdForJob(int jobId, int companyId)
        {
            string routeId = string.Empty;
            try
            {
                var routesDetails = await (mdbContext.RouteInformations.FindAsync(t => t.TfxCompanyId == companyId &&
                                                                t.IsActive && !t.IsDeleted &&
                                                                t.TfxJobs.Any(t1 => t1.Id == jobId)));

                if (routesDetails != null)
                {
                    routeId = routesDetails.ToList().Select(t => t.Id.ToString()).FirstOrDefault();
                }

            }
            catch (Exception ex)
            {

                LogManager.Logger.WriteException("JobRepository", "GetRoutesIdForJob", ex.Message, ex);
            }
            return routeId;
        }

        public async Task<JobAdditionalDetailsForSummary> GetJobSummaryForSupplier(JobSummaryRequestModel request)
        {
            var response = new JobAdditionalDetailsForSummary();
            var jobSummary = new List<JobSummaryModel>();
            var regionList = (from region in mdbContext.Regions.AsQueryable()
                              where region.TfxCompanyId == request.CompanyId
                              && region.IsActive && !region.IsDeleted
                              && region.TfxJobs.Any(t => request.JobIds.Contains(t.Id))
                              select new
                              {
                                  Id = region.Id,
                                  Name = region.Name,
                                  Jobs = region.TfxJobs != null ? region.TfxJobs.Select(t => t.Id) : new List<int>()
                              }).ToList();

            var carrierList = (from carrier in mdbContext.Carriers.AsQueryable()
                               join job in mdbContext.CarrierJobs.AsQueryable()
                                on carrier.TfxCarrierCompanyId equals job.TfxCarrierCompanyId into grp
                               where (carrier.TfxSupplierCompanyId == request.CompanyId || carrier.TfxCarrierCompanyId == request.CompanyId) && carrier.IsActive && !carrier.IsDeleted && grp.Any(t1 => t1.IsActive && request.JobIds.Contains(t1.TfxJobId))
                               select new
                               {
                                   carrier.TfxCarrierCompanyId,
                                   carrier.TfxCarrierCompanyName,
                                   Jobs = grp.Where(t => t.IsActive).Select(t => new
                                   {
                                       t.TfxJobId
                                   })
                               }).ToList();

            List<ObjectId> regionObjectIds = regionList.Select(t => t.Id).ToList();
            var routeInfoDetails = await mdbContext.RouteInformations.Find(top => regionObjectIds.Contains(top.RegionId) && top.IsActive == true && top.IsDeleted == false
                                                                            && top.TfxJobs.Any(t1 => request.JobIds.Contains(t1.Id)))
                           .Project(t => new
                           {
                               Id = t.Id.ToString(),
                               Name = t.Name,
                               RegionId = t.RegionId.ToString(),
                               TfxJobs = t.TfxJobs.Select(top => top.Id).ToList()
                           }).ToListAsync();

            var oDistanceCovered = await mdbContext.JobAdditionalDetails.Find(x => request.JobIds.Contains(x.TfxJobId) && x.IsActive).ToListAsync();

            foreach (var item in request.JobIds)
            {
                JobSummaryModel summaryModel = new JobSummaryModel() { JobId = item };
                var region = regionList.Where(t => t.Jobs.Contains(item)).Select(t => new { t.Id, t.Name }).FirstOrDefault();
                if (region != null)
                {
                    summaryModel.RegionId = region.Id.ToString();
                    summaryModel.RegionName = region.Name;
                    var routeInfo = routeInfoDetails.Where(t => t.RegionId == summaryModel.RegionId && t.TfxJobs.Contains(item)).FirstOrDefault();
                    if (routeInfo != null)
                    {
                        summaryModel.RouteId = routeInfo.Id;
                        summaryModel.RouteName = routeInfo.Name;
                    }
                }
                var distanceCoverd = oDistanceCovered.FirstOrDefault(x => x.TfxJobId == summaryModel.JobId);
                if (distanceCoverd != null)
                {
                    summaryModel.DistanceCovered = distanceCoverd.DistanceCovered;
                }
                var carrier = carrierList.Where(t => t.Jobs.Any(t1 => t1.TfxJobId == item)).Select(t => new { t.TfxCarrierCompanyId, t.TfxCarrierCompanyName }).FirstOrDefault();
                if (carrier != null)
                {
                    summaryModel.CarrierId = carrier.TfxCarrierCompanyId.ToString();
                    summaryModel.CarrierName = carrier.TfxCarrierCompanyName;
                }
                response.JobDetails.Add(summaryModel);
            }
            response.StatusCode = (int)Status.Success;
            return response;
        }

        public async Task<StatusModel> AddJobToRegion(JobToRegionAssignViewModel jobToUpdate)
        {
            var response = new StatusModel(Status.Failed);
            try
            {
                MdbDataAccess.Collections.DropdownDisplayItem TfxJob = new MdbDataAccess.Collections.DropdownDisplayItem()
                {
                    Id = jobToUpdate.JobId,
                    Code = null,
                    Name = jobToUpdate.JobName
                };
                var filter = Builders<Region>.Filter.And(
                               Builders<Region>.Filter.Where(x => x.Id == ObjectId.Parse(jobToUpdate.RegionId)),
                                Builders<Region>.Filter.Where(x => x.TfxCompanyId == jobToUpdate.CompanyId),
                                   Builders<Region>.Filter.Where(x => x.IsActive),
                                   Builders<Region>.Filter.Where(x => !x.IsDeleted)
                               );

                var update = Builders<Region>.Update.Push("TfxJobs", TfxJob);
                await mdbContext.Regions.FindOneAndUpdateAsync(filter, update);

                response.StatusCode = (int)Status.Success;
                response.StatusMessage = "Job " + jobToUpdate.JobName + " assigned successfully to Region";
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobRepository", "AddJobToRegion", ex.Message, ex);

            }
            return response;
        }

        public async Task<List<RecurringDRSchdule>> GetRecurringSchedulesForBuyer(JobRecurringDRInput input)
        {
            List<RecurringDRSchdule> recurringSchdules = new List<RecurringDRSchdule>();
            if (input != null && input.JobIds != null && input.JobIds.Any())
            {
                var filter = Builders<RecurringSchedules>.Filter.And(
                                    Builders<RecurringSchedules>.Filter.In(p => p.JobId, input.JobIds),
                                    Builders<RecurringSchedules>.Filter.Where(p => p.IsActive),
                                    Builders<RecurringSchedules>.Filter.Where(p => !p.IsDeleted),
                                    Builders<RecurringSchedules>.Filter.Where(p => p.DeliveryRequestFor == DeliveryRequestFor.ProductType),
                                    Builders<RecurringSchedules>.Filter.Or(Builders<RecurringSchedules>.Filter.Where(p => input.ProductTypeIds == null),
                                    Builders<RecurringSchedules>.Filter.In(p => p.ProductTypeId, input.ProductTypeIds))
                                );
                var recurringScheduleDetails = await mdbContext.RecurringSchedules.Find(filter)
                                .Project(t => new RecurringSchedules
                                {
                                    Id = t.Id,
                                    ScheduleType = t.ScheduleType,
                                    WeekDayId = t.WeekDayId,
                                    MonthDayId = t.MonthDayId,
                                    Date = t.Date,
                                    ScheduleQuantityType = t.ScheduleQuantityType,
                                    SiteId = t.SiteId,
                                    JobId = t.JobId,
                                    TfxSupplierCompanyId = t.TfxSupplierCompanyId,
                                    TfxCompanyName = t.TfxCompanyName,
                                    TfxUserId = t.TfxUserId,
                                    AssignedToCompanyId = t.AssignedToCompanyId,
                                    RequiredQuantity = t.RequiredQuantity,
                                    BuyerCompanyId = t.BuyerCompanyId,
                                    ProductTypeId = t.ProductTypeId
                                }).ToListAsync();
                foreach (var item in recurringScheduleDetails)
                {
                    var recurringItem = item.ToRecurringEntity();
                    recurringSchdules.Add(recurringItem);
                }
            }
            return recurringSchdules;
        }

        public async Task<StatusModel> AddJobToRouteInfo(JobToRegionAssignViewModel jobToUpdate)
        {
            var response = new StatusModel(Status.Failed);
            try
            {

                int sequenceNo = 0;
                var routeInfo = mdbContext.RouteInformations.Find(top => top.Id == ObjectId.Parse(jobToUpdate.RouteId)).FirstOrDefault();
                if (routeInfo != null)
                {
                    sequenceNo = routeInfo.TfxJobs.Max(top => top.SequenceNo) + 1;
                }
                MdbDataAccess.Collections.TfxJobsDetails TfxJob = new MdbDataAccess.Collections.TfxJobsDetails()
                {
                    Id = jobToUpdate.JobId,
                    Code = null,
                    Name = jobToUpdate.JobName,
                    SequenceNo = sequenceNo
                };
                var filter = Builders<RouteInformations>.Filter.And(
                                   Builders<RouteInformations>.Filter.Where(x => x.Id == ObjectId.Parse(jobToUpdate.RouteId)),
                                    Builders<RouteInformations>.Filter.Where(x => x.TfxCompanyId == jobToUpdate.CompanyId),
                                       Builders<RouteInformations>.Filter.Where(x => x.IsActive),
                                       Builders<RouteInformations>.Filter.Where(x => !x.IsDeleted)
                                   );
                var update = Builders<RouteInformations>.Update.Push("TfxJobs", TfxJob);
                await mdbContext.RouteInformations.FindOneAndUpdateAsync(filter, update);

                response.StatusCode = (int)Status.Success;
                response.StatusMessage = "Job " + jobToUpdate.JobName + " assigned successfully to Route Info.";

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobRepository", "AddJobToRouteInfo", ex.Message, ex);

            }
            return response;
        }
        public async Task<StatusModel> InActiveJobDetails(List<int> jobId)
        {
            var response = new StatusModel();

            using (var session = await mdbContext.Client.StartSessionAsync())
            {
                try
                {
                    session.StartTransaction();
                    //JobAdditionalDetail
                    var jobDtlsFilter = Builders<JobAdditionalDetail>.Filter.Where(w => jobId.Contains(w.TfxJobId));
                    var updateJobFields = Builders<JobAdditionalDetail>.Update
                       .Set(t => t.IsActive, false);
                    //DeliveryRequest
                    var updateDRFields = Builders<DeliveryRequest>.Update
                    .Set(t => t.IsActive, false);
                    var drFilter = Builders<DeliveryRequest>.Filter.Where(w => jobId.Contains(w.TfxJobId));
                    //BrokeredDRJob
                    var updateBrokerDRFields = Builders<BrokeredDRJob>.Update
                    .Set(t => t.IsActive, false);
                    var brokerDrFilter = Builders<BrokeredDRJob>.Filter.Where(w => jobId.Contains(w.TfxJobId));

                    //CarrierJob
                    var updateCarrierFields = Builders<CarrierJob>.Update
                    .Set(t => t.IsActive, false);
                    var carrierFilter = Builders<CarrierJob>.Filter.Where(w => jobId.Contains(w.TfxJobId));

                    //RecurringSchedules
                    var updateRecurringScheduleFields = Builders<RecurringSchedules>.Update
                    .Set(t => t.IsActive, false);
                    var recurringScheduleFilter = Builders<RecurringSchedules>.Filter.Where(w => jobId.Contains(w.JobId));

                    //Region

                    var regions = await (mdbContext.Regions.Find(t => t.TfxJobs.Any(t1 => jobId.Contains(t1.Id))).ToListAsync());
                    if (regions != null)
                    {
                        foreach (var region in regions)
                        {
                            region.TfxJobs = region.TfxJobs.FindAll(f => !jobId.Contains(f.Id)).ToList();
                            var regionFields = Builders<Region>.Update
                                               .Set(t => t.TfxJobs, region.TfxJobs);
                            var regionFilter = Builders<Region>.Filter.Where(w => w.Id == region.Id);
                            await mdbContext.Regions.UpdateManyAsync(regionFilter, regionFields);
                        }
                    }
                    //RouteInformation
                    var routes = await (mdbContext.RouteInformations.Find(t => t.TfxJobs.Any(t1 => jobId.Contains(t1.Id))).ToListAsync());
                    if (routes != null)
                    {
                        foreach (var route in routes)
                        {
                            route.TfxJobs = route.TfxJobs.FindAll(f => !jobId.Contains(f.Id)).ToList();
                            var routeFields = Builders<RouteInformations>.Update
                                               .Set(t => t.TfxJobs, route.TfxJobs);
                            var routeFilter = Builders<RouteInformations>.Filter.Where(w => w.Id == route.Id);
                            await mdbContext.RouteInformations.UpdateManyAsync(routeFilter, routeFields);
                        }
                    }

                    await mdbContext.JobAdditionalDetails.UpdateManyAsync(jobDtlsFilter, updateJobFields);
                    await mdbContext.DeliveryRequests.UpdateManyAsync(drFilter, updateDRFields);
                    await mdbContext.RecurringSchedules.UpdateManyAsync(recurringScheduleFilter, updateRecurringScheduleFields);
                    await mdbContext.CarrierJobs.UpdateManyAsync(carrierFilter, updateCarrierFields);
                    await mdbContext.BrokeredDRJobs.UpdateManyAsync(brokerDrFilter, updateBrokerDRFields);
                    response.StatusCode = (int)Status.Success;
                    await session.CommitTransactionAsync();
                }
                catch (Exception ex)
                {
                    response.StatusCode = (int)Status.Failed;
                    session.AbortTransaction();
                    LogManager.Logger.WriteException("Job Repository", "InActiveJobDetails", ex.Message, ex);
                }
            }

            return response;
        }
        private void UpdateJobNameInRegion(int jobId, string jobName)
        {
            var filter = Builders<Region>.Filter.Where(x => x.TfxJobs.Any(i => i.Id == jobId));
            var update = Builders<Region>.Update.Set(x => x.TfxJobs[-1].Name, jobName);
            mdbContext.Regions.UpdateMany(filter, update);
        }

        private void UpdateJobNameInCarrierJobs(int jobId, string jobName)
        {
            var filter = Builders<CarrierJob>.Filter.Where(x => x.TfxJobId == jobId);
            var update = Builders<CarrierJob>.Update.Set(x => x.TfxJobName, jobName);
            mdbContext.CarrierJobs.UpdateMany(filter, update);
        }
        private string GetFilePath(string fileName)
        {
            var filePath = string.Format(ApplicationConstants.FilePathFormat, AzureBlobStorage.GetStorageAccountName(), BlobContainerType.TankTypeDipChart.ToString().ToLower(), fileName, AzureBlobStorage.GetSaS(BlobContainerType.TankTypeDipChart.ToString().ToLower()));
            return filePath;
        }

    }
}
