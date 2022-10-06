using SiteFuel.FreightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightRepository
{
    public interface ICarrierRepository
    {
        List<CarrierViewModel> GetSupplierCarriers(int companyId, int carrierCompanyId);
        CarrierJobDetailsViewModel GetCarrierUsers(int companyId);

        Task<StatusModel> AssignToSupplier(List<CarrierViewModel> supplierCarriers);
        Task<StatusModel> UpdateAssignedCarriers(CarrierViewModel carrier);
        Task<StatusModel> DeleteAssignedCarriers(CarrierViewModel carrier);
        Task<StatusModel> AssignCarrierToJob(CarrierViewModel carrier);
        Task<List<int>> GetCarriersJobs(int carrierCompanyId, int customerCompanyId);
        Task<List<DipTestRequestModel>> GetCarrierDetailsByJob(List<int> jobIds);
    }
}
