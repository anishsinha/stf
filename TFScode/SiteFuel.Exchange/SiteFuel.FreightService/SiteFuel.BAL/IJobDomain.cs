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
        Task<JobAdditionalDetailsModel> GetAdditionalJobDetails(int jobId);
    }
}
