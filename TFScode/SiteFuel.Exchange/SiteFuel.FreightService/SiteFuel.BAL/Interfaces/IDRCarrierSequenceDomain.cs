using SiteFuel.FreightModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiteFuel.BAL
{
    public interface IDRCarrierSequenceDomain
    {
        Task<StatusModel> SaveDRCarrierSequence(List<DRCarrierSequenceModel> model);
        Task<StatusModel> UpdateDRCarrierSequence(List<DRCarrierSequenceModel> model);
        Task<DRCarrierSequenceModel> GetDRCarrierSequence(string deliveryReqId);
        Task<StatusModel> UpdateDRCarrierRejectList(DRCarrierRejectInfoModel model);
        Task<TfxDRAvailableCarrierInfoModel> GetAvailableDRCarrierDetails(string deliveryReqId);
    }
}
