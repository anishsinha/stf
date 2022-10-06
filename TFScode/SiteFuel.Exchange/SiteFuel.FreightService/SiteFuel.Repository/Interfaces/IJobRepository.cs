using MongoDB.Bson;
using SiteFuel.FreightModels;
using SiteFuel.MdbDataAccess.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightRepository
{
    public interface IJobRepository
    {
        Task<bool> SaveAdditionalJobDetails(JobAdditionalDetailsModel table);
        Task<bool> RemoveJobAdditionalDetails(int jobId);
        Task<bool> UpdateAdditionalJobDetails(JobAdditionalDetailsModel table);
        Task<JobAdditionalDetailsModel> GetAdditionalJobDetails(int jobId,int supplierCompanyId);
        Task<JobLocationRelatedDetailsModel> GetJobLocationRelatedDetails(int companyId,string jobId, bool IsBuyerCompany);
        List<JobDipChartDetails> GetDipTestDetails(string siteId, string TankId,int noOfDays);
        List<JobDipChartDetails> GetDemandCaptureChartData(List<String> siteId, List<String> TankId, int noOfDays);
        Task<StatusModel> AssignToSupplier(List<CarrierViewModel> supplierCarriers);
        Task<string> GetRegionIdForJob(int jobId, int companyId);
        Task<StatusModel> AssignTPOJobToRegion(JobToRegionAssignViewModel jobToUpdate);
        Task<bool> DeleteJobTanks(DeleteTanksModel deleteTankModel);
        Task<StatusModel> AssignTPOJobToRoute(JobToRegionAssignViewModel jobToUpdate);
        Task<JobAdditionalDetailsForSummary> GetJobSummaryForSupplier(JobSummaryRequestModel request);
        Task<List<RecurringDRSchdule>> GetRecurringSchedulesForBuyer(JobRecurringDRInput jobIds);
        Task<List<JobDRDetailsModel>> GetJobDRPrioritiesForBuyer(JobDRPriorityInputModel request);
        Task<bool> UpdateDistanceCoveredOfAdditionalJobDetail(int JobId, string DistanceCovered);
         Task<StatusModel> InActiveJobDetails(List<int> jobId);
        Task<string> getRegionByJobAndCompanyId(int jobId, int companyId);
    }
}
