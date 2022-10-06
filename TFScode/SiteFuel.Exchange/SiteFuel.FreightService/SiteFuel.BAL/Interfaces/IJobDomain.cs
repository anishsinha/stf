using SiteFuel.FreightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.BAL
{
    public interface IJobDomain
    {
        Task<bool> SaveAdditionalJobDetails(JobAdditionalDetailsModel table);
        Task<bool> RemoveJobAdditionalDetails(int jobId);
        Task<bool> UpdateAdditionalJobDetails(JobAdditionalDetailsModel table);
        Task<JobAdditionalDetailsModel> GetAdditionalJobDetails(int jobId, int supplierCompanyId);
        Task<JobLocationRelatedDetailsModel> GetJobLocationRelatedDetails(int companyId, string jobId, bool IsBuyerCompany);
        List<JobDipChartDetails> GetDipTestDetails(string siteID, string TankID, int noOfDays);
        List<JobDipChartDetails> GetDemandCaptureChartData(List<string> siteID, List<string> TankID, int noOfDays);
        Task<StatusModel> SaveJobRegionCarrierDetails(JobDetails jobDetails);
        Task<bool> DeleteJobTanks(DeleteTanksModel deleteTankModel);
        Task<JobAdditionalDetailsForSummary> GetJobSummaryForSupplier(JobSummaryRequestModel request);
        Task<List<RecurringDRSchdule>> GetRecurringSchedulesForBuyer(JobRecurringDRInput jobIds);
        Task<List<JobDRDetailsModel>> GetJobDRPrioritiesForBuyer(JobDRPriorityInputModel request);
        Task<bool> UpdateDistanceCoveredOfAdditionalJobDetails(int JobId, string DistanceCovered);

        Task<StatusModel> InActiveJobDetailsForCustomer(List<int> JobIds);
        Task<string> getRegionByJobAndCompanyId(int JobId, int companyId);
    }
}
