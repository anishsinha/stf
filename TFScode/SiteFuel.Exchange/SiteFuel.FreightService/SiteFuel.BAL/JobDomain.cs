using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
using SiteFuel.FreightRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteFuel.BAL
{
    public class JobDomain : IJobDomain
    {
        readonly IJobRepository _jobRepository;
        public JobDomain(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }
        public async Task<bool> RemoveJobAdditionalDetails(int jobId)
        {
            var response = false;
            try
            {
                response = await _jobRepository.RemoveJobAdditionalDetails(jobId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobDomain", "RemoveJobAdditionalDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<bool> SaveAdditionalJobDetails(JobAdditionalDetailsModel table)
        {
            var response = false;
            try
            {
                response = await _jobRepository.SaveAdditionalJobDetails(table);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobDomain", "SaveAdditionalJobDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<JobAdditionalDetailsModel> GetAdditionalJobDetails(int jobId, int supplierCompanyId = 0)
        {
            JobAdditionalDetailsModel response = null;
            try
            {
                response = await _jobRepository.GetAdditionalJobDetails(jobId, supplierCompanyId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobDomain", "GetAdditionalJobDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<JobLocationRelatedDetailsModel> GetJobLocationRelatedDetails(int companyId, string jobID, bool IsBuyerCompany)
        {
            var response = new JobLocationRelatedDetailsModel();
            try
            {
                response = await _jobRepository.GetJobLocationRelatedDetails(companyId, jobID, IsBuyerCompany);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobDomain", "GetJobLocationRelatedDetails", ex.Message, ex);
            }
            return response;

        }
        public List<JobDipChartDetails> GetDipTestDetails(string siteID, string TankId, int noOfDays)
        {
            var response = new List<JobDipChartDetails>();
            try
            {
                response = _jobRepository.GetDipTestDetails(siteID, TankId, noOfDays);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobDomain", "GetDipTestDetails", ex.Message, ex);
            }
            return response;

        }
        public List<JobDipChartDetails> GetDemandCaptureChartData(List<string> siteID, List<string> TankId, int noOfDays)
        {
            var response = new List<JobDipChartDetails>();
            try
            {
                response = _jobRepository.GetDemandCaptureChartData(siteID, TankId, noOfDays);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobDomain", "GetDemandCaptureChartData", ex.Message, ex);
            }
            return response;

        }

        public async Task<bool> UpdateAdditionalJobDetails(JobAdditionalDetailsModel table)
        {
            var response = false;
            try
            {
                response = await _jobRepository.UpdateAdditionalJobDetails(table);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobDomain", "UpdateAdditionalJobDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<bool> UpdateDistanceCoveredOfAdditionalJobDetails(int JobId, string DistanceCovered)
        {
            var response = false;
            try
            {
                response = await _jobRepository.UpdateDistanceCoveredOfAdditionalJobDetail(JobId, DistanceCovered);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobDomain", "UpdateDistanceCoveredOfAdditionalJobDetails", ex.Message, ex);
            }
            return response;
        }
        public async Task<StatusModel> SaveJobRegionCarrierDetails(JobDetails jobDetails)
        {
            var response = new StatusModel(Status.Success);
            if (jobDetails.JobModel != null)
            {
                var isSaved = false;
                if (jobDetails.JobModel.JobId > 0)
                {
                    isSaved = await UpdateAdditionalJobDetails(jobDetails.JobModel);
                }
                else
                {
                    isSaved = await SaveAdditionalJobDetails(jobDetails.JobModel);
                }

                if (!isSaved)
                    response.StatusCode = (int)Status.Failed;
            }
            if (jobDetails.SupplierCarriers != null)
            {
                var valResult = ValidateAssignCarrierModel(jobDetails.SupplierCarriers);
                if (valResult.IsValid)
                {
                    var status = await _jobRepository.AssignToSupplier(jobDetails.SupplierCarriers);
                    if (response.StatusCode != (int)Status.Success)
                        response.StatusCode = status.StatusCode;
                }
            }
            if (jobDetails.JobToRegion != null)
            {
                var status = await _jobRepository.AssignTPOJobToRegion(jobDetails.JobToRegion);
                if (response.StatusCode != (int)Status.Success)
                    response.StatusCode = status.StatusCode;

            }
            if (jobDetails.JobToRegion != null && !string.IsNullOrEmpty(jobDetails.JobToRegion.RouteId))
            {
                var routeInfoStatus = await _jobRepository.AssignTPOJobToRoute(jobDetails.JobToRegion);
                if (routeInfoStatus.StatusCode != (int)Status.Success)
                    response.StatusCode = routeInfoStatus.StatusCode;
            }
            return response;
        }

        public async Task<bool> DeleteJobTanks(DeleteTanksModel deleteTankModel)
        {
            var response = false;
            try
            {
                response = await _jobRepository.DeleteJobTanks(deleteTankModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobDomain", "DeleteJobTanks", ex.Message, ex);
            }
            return response;
        }

        public async Task<string> getRegionByJobAndCompanyId(int JobId, int companyId)
        {
            
            try
            {
                 return  await _jobRepository.getRegionByJobAndCompanyId(JobId, companyId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobDomain", "DeleteJobTanks", ex.Message, ex);
            }
            return string.Empty;
        }

        public async Task<List<RecurringDRSchdule>> GetRecurringSchedulesForBuyer(JobRecurringDRInput jobIds)
        {
            var response = new List<RecurringDRSchdule>();
            try
            {
                response = await _jobRepository.GetRecurringSchedulesForBuyer(jobIds);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobDomain", "GetRecurringSchedulesForBuyer", ex.Message, ex);
            }
            return response;
        }

        public async Task<JobAdditionalDetailsForSummary> GetJobSummaryForSupplier(JobSummaryRequestModel request)
        {
            var response = new JobAdditionalDetailsForSummary();
            try
            {
                response = await _jobRepository.GetJobSummaryForSupplier(request);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobDomain", "GetJobSummaryForSupplier", ex.Message, ex);
            }
            return response;
        }

        private ValidatationResult ValidateAssignCarrierModel(List<CarrierViewModel> model)
        {
            var result = new ValidatationResult() { IsValid = true };
            var messages = new List<string>();

            if (model.Count == 0)
                messages.Add("atlease one carrier is required");

            if (model.Any(t => t.Carrier == null))
                messages.Add("Carrier is required");

            if (model.Any(t => t.CreatedOn == DateTimeOffset.MinValue))
                messages.Add("CreatedOn");

            if (model.Any(t => t.CreatedBy <= 0))
                messages.Add("CreatedBy");

            if (messages.Any())
            {
                result.IsValid = false;
                result.Message = "Invalid " + string.Join(", ", messages);
            }
            return result;
        }
        public async Task<List<JobDRDetailsModel>> GetJobDRPrioritiesForBuyer(JobDRPriorityInputModel request)
        {
            var response = new List<JobDRDetailsModel>();
            try
            {
                response = await _jobRepository.GetJobDRPrioritiesForBuyer(request);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobDomain", "GetJobDRPrioritiesForBuyer", ex.Message, ex);
            }
            return response;

        }

        public async Task<StatusModel> InActiveJobDetailsForCustomer(List<int>JobIds)
        {
            var response = new StatusModel();
            try
            {
                response = await _jobRepository.InActiveJobDetails(JobIds);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobDomain", "InActiveJobDetailsForCustomer", ex.Message, ex);
            }
            return response;
        }
    }

}

