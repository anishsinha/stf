using Newtonsoft.Json;
using SiteFuel.BAL;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.FreightApi.Attributes;
using SiteFuel.FreightModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace SiteFuel.FreightApi.Controllers
{
#if DEBUG
    [ApiExplorerSettings(IgnoreApi = false)]
#else
    [ApiExplorerSettings(IgnoreApi = true)]
#endif
    public class JobController : ApiController
    {
        private readonly IJobDomain _jobDomain;

        public JobController(IJobDomain jobDomain)
        {
            _jobDomain = jobDomain;
        }

        [ValidateToken]
        [HttpPost]
        public async Task<bool> SaveAdditionalJobDetails(JobAdditionalDetailsModel table)
        {
            var json = JsonConvert.SerializeObject(table);
            using (var tracer = new Tracer("TrueFill.FreightApi::JobController", $"SaveAdditionalJobDetails(request:{json})"))
            {
                bool response = await _jobDomain.SaveAdditionalJobDetails(table);
                return response;
            }
        }

        [ValidateToken]
        [HttpPost]
        public async Task<bool> DeleteJobTanks(DeleteTanksModel deleteTankModel)
        {
            var json = JsonConvert.SerializeObject(deleteTankModel);
            using (var tracer = new Tracer("TrueFill.FreightApi::JobController", $"DeleteJobTanks(request:{json})"))
            {
                bool response = await _jobDomain.DeleteJobTanks(deleteTankModel);
                return response;
            }
        }

        [ValidateToken]
        [HttpPost]
        public async Task<bool> RemoveJobAdditionalDetails(int jobId)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::JobController", $"RemoveJobAdditionalDetails(jobId:{jobId})"))
            {
                bool response = await _jobDomain.RemoveJobAdditionalDetails(jobId);
                return response;
            }
        }

        [ValidateToken]
        [HttpPost]
        public async Task<bool> UpdateAdditionalJobDetails(JobAdditionalDetailsModel table)
        {
            var json = JsonConvert.SerializeObject(table);
            using (var tracer = new Tracer("TrueFill.FreightApi::JobController", $"UpdateAdditionalJobDetails(request:{json})"))
            {
                bool response = await _jobDomain.UpdateAdditionalJobDetails(table);
                return response;
            }
        }


        [HttpGet]
        public async Task<JobAdditionalDetailsModel> GetAdditionalJobDetails(int jobId, int supplierCompanyId = 0)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::JobController", $"GetAdditionalJobDetails(jobId:{jobId})"))
            {
                JobAdditionalDetailsModel response = await _jobDomain.GetAdditionalJobDetails(jobId, supplierCompanyId);
                return response;
            }
        }
        [HttpPost]
        public async Task<JobLocationRelatedDetailsModel> GetJobLocationRelatedDetails(JobLocationRelatedDetailsModel jobAdditionalDetailsModel)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::JobController", $"GetJobLocationRelatedDetails(jobId:{jobAdditionalDetailsModel.JobID})"))
            {
                var response = await _jobDomain.GetJobLocationRelatedDetails(jobAdditionalDetailsModel.CompanyID, jobAdditionalDetailsModel.JobID, jobAdditionalDetailsModel.IsBuyerCompany);
                return response;
            }
        }

        [HttpGet]
        public List<JobDipChartDetails> GetDipTestDetails(string siteID, string TankId, int noOfDays)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::JobController", $"GetDipTestDetails(siteID:{siteID})"))
            {
                var response = _jobDomain.GetDipTestDetails(siteID, TankId, noOfDays);
                return response;
            }
        }

        [HttpPost]
        public List<JobDipChartDetails> GetDipTestDetails(DemandCaptureChartData demandCaptureChartViewModel)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::JobController", $"GetDipTestDetails(siteID:{demandCaptureChartViewModel.noOfDays})"))
            {
                var response = _jobDomain.GetDemandCaptureChartData(demandCaptureChartViewModel.siteID, demandCaptureChartViewModel.TankId, demandCaptureChartViewModel.noOfDays);
                return response;
            }
        }

        [HttpPost]
        public async Task<StatusModel> SaveJobRegionCarrierDetails(JobDetails jobDetails)
        {
            var json = JsonConvert.SerializeObject(jobDetails);
            using (var tracer = new Tracer("TrueFill.FreightApi::JobController", $"SaveJobRegionCarrierDetails(request:{json})"))
            {
                var response = await _jobDomain.SaveJobRegionCarrierDetails(jobDetails);
                return response;
            }
        }

        [HttpPost]
        public async Task<JobAdditionalDetailsForSummary> GetJobDetailsForSupplier(JobSummaryRequestModel request)
        {
            var json = JsonConvert.SerializeObject(request);
            using (var tracer = new Tracer("TrueFill.FreightApi::JobController", $"GetJobDetailsForSupplier(request:{request})"))
            {
                var response = await _jobDomain.GetJobSummaryForSupplier(request);
                return response;
            }
        }

        [HttpPost]
        public async Task<List<RecurringDRSchdule>> GetRecurringSchedulesForBuyer(JobRecurringDRInput jobIds)
        {
            var json = JsonConvert.SerializeObject(jobIds);
            using (var tracer = new Tracer("TrueFill.FreightApi::JobController", $"GetRecurringSchedulesForBuyer(request:{json})"))
            {
                var response = await _jobDomain.GetRecurringSchedulesForBuyer(jobIds);
                return response;
            }
        }


        [HttpGet]
        public async Task<string> getRegionByJobAndCompanyId(int jobId , int companyId )
        {
             return await _jobDomain.getRegionByJobAndCompanyId(jobId,companyId);
        }

        [HttpPost]
        public Task<List<JobDRDetailsModel>> GetJobDRPrioritiesForBuyer(JobDRPriorityInputModel request)
        {
            var json = JsonConvert.SerializeObject(request);
            using (var tracer = new Tracer("TrueFill.FreightApi::JobController", $"GetJobDRPrioritiesForBuyer(request:{json})"))
            {
                var response = _jobDomain.GetJobDRPrioritiesForBuyer(request);
                return response;
            }
        }


        [HttpPost]
        public async Task<bool> UpdateDistanceCoveredOfAdditionalJobDetails(JobAdditionalDetailsModel jobAdditionalDetailsModel)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::JobController", $"UpdateDistanceCoveredOfAdditionalJobDetails(request:{jobAdditionalDetailsModel})"))
            {
                bool response = await _jobDomain.UpdateDistanceCoveredOfAdditionalJobDetails(jobAdditionalDetailsModel.JobId, jobAdditionalDetailsModel.DistanceCovered);
                return response;
            }
        }
        [ValidateToken]
        [HttpPost]
        public async Task<StatusModel> RemoveJobDetailsForCustomer(List<int> jobIds)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::JobController", $"RemoveJobDetailsForCustomer(jobId:{jobIds})"))
            {
                var response = await _jobDomain.InActiveJobDetailsForCustomer(jobIds);
                return response;
            }
        }
    }
}
