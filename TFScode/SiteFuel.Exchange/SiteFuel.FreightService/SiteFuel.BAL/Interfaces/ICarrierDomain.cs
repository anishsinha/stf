using SiteFuel.FreightModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiteFuel.BAL
{
    public interface ICarrierDomain
    {
        List<CarrierViewModel> GetSupplierCarriers(int companyId, int carrierCompanyId);
        CarrierJobDetailsViewModel GetCarrierUsers(int companyId);

        Task<StatusModel> AssignToSupplier(List<CarrierViewModel> supplierCarriers);
        Task<StatusModel> UpdateAssignedCarriers(CarrierViewModel carrier);
        Task<StatusModel> DeleteAssignedCarriers(CarrierViewModel carrier);
        Task<StatusModel> AssignCarrierToJob(CarrierViewModel jobToCarrierAssign);
        Task<List<int>> GetCarriersJobs(int carrierCompanyId, int customerCompanyId);
        Task<List<DipTestRequestModel>> GetCarrierDetailsByJob(List<int> jobIds);
    }
}
