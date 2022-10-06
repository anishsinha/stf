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
        Task<JobAdditionalDetailsModel> GetAdditionalJobDetails(int jobId);
    }
}
