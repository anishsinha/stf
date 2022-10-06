using SiteFuel.FreightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightRepository
{
    public interface IDRCarrierSequenceRepository
    {
        Task<StatusModel> SaveDRCarrierSequence(List<DRCarrierSequenceModel> model);
        Task<StatusModel> UpdateDRCarrierSequence(List<DRCarrierSequenceModel> model);
        Task<DRCarrierSequenceModel> GetDRCarrierSequence(string deliveryReqId);
        Task<StatusModel> UpdateDRCarrierRejectList(DRCarrierRejectInfoModel model);
        Task<TfxDRAvailableCarrierInfoModel> GetAvailableDRCarrierDetails(string deliveryReqId);
    }
}
