using MongoDB.Bson;
using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
using SiteFuel.MdbDataAccess.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightRepository
{
    public interface ITankRepository
    {
        Task<bool> SaveTankDetails(TankDetailsModel model);
        Task<bool> OrderTankMappings(List<OrderTankMappingViewModel> model);

        Task<bool> UpdateTankDetails(TankDetailsModel model);

        Task<TankDetailsModel> GetTankDetails(int id);

        Task<List<TankDetailListModel>> GetTankList(List<int> tanks);

        Task<ScheduleOutputModel> GetTankDetailsBySchedule(List<ScheduleInputModel> scheduleInputDetails);

        Task<List<JobTankDetailModel>> GetJobTankList(int jobId);
        Task<TankVolumeAndUllageModel> GetTankVolumeAndUllage(TankVolumeAndUllageInputModel requestModel);
        Task<StatusModel> SaveTankTypeDipChart(TankModalTypeViewModel tankTypes);
        Task<List<TankModalTypeViewModel>> GetTankTypesByCompany(int companyId);
        Task<StatusModel> DeleteTankDipChartById(string id);
        Task<List<string>> GetAllTankTypeNameForDipChart(int companyId, string searchValue);
        Task<List<DropdownDisplayExtended>> GetTankModelType(List<int> companyId);
        Task<StatusModel> UpdateTankSequence(TankDetailsModel requestModel);
        Task<bool> CheckDuplicateTankSequence(TankDetailsModel requestModel);
        Task<List<DropQuantityByPrePostDipResponseModel>> GetDropQuantityByPrePostDip(List<DropQuantityByPrePostDipRequestModel> requestModel);
        Task<List<JobTankAdditionalDetailModel>> GetJobsTankList(List<int> jobId);
    }
}
