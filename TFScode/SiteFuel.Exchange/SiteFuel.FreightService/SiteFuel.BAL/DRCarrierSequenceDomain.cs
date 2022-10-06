using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
using SiteFuel.FreightRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.BAL
{
    public class DRCarrierSequenceDomain : IDRCarrierSequenceDomain
    {
        private IDRCarrierSequenceRepository _dRCarrierRepository;
        public DRCarrierSequenceDomain(IDRCarrierSequenceRepository dRCarrierRepository)
        {
            _dRCarrierRepository = dRCarrierRepository;
        }

        public async Task<TfxDRAvailableCarrierInfoModel> GetAvailableDRCarrierDetails(string deliveryReqId)
        {
            var response = new TfxDRAvailableCarrierInfoModel();
            try
            {
                response = await _dRCarrierRepository.GetAvailableDRCarrierDetails(deliveryReqId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DRCarrierSequenceDomain", "GetAvailableDRCarrierDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<DRCarrierSequenceModel> GetDRCarrierSequence(string deliveryReqId)
        {
            var response = new DRCarrierSequenceModel();
            try
            {
                response = await _dRCarrierRepository.GetDRCarrierSequence(deliveryReqId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DRCarrierSequenceDomain", "GetDRCarrierSequence", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusModel> SaveDRCarrierSequence(List<DRCarrierSequenceModel> model)
        {
            var response = new StatusModel();
            try
            {

                response = await _dRCarrierRepository.SaveDRCarrierSequence(model);
            }
            catch (Exception ex)
            {
                response.StatusCode = (int)Status.Failed;
                response.StatusMessage = Resource.errorDRCarrierSeq;
                LogManager.Logger.WriteException("DRCarrierSequenceDomain", "SaveDRCarrierSequence", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusModel> UpdateDRCarrierRejectList(DRCarrierRejectInfoModel model)
        {
            var response = new StatusModel();
            try
            {

                response = await _dRCarrierRepository.UpdateDRCarrierRejectList(model);
            }
            catch (Exception ex)
            {
                response.StatusCode = (int)Status.Failed;
                response.StatusMessage = Resource.errorDRCarrierSeq;
                LogManager.Logger.WriteException("DRCarrierSequenceDomain", "UpdateDRCarrierRejectList", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusModel> UpdateDRCarrierSequence(List<DRCarrierSequenceModel> model)
        {
            var response = new StatusModel();
            try
            {

                response = await _dRCarrierRepository.UpdateDRCarrierSequence(model);
            }
            catch (Exception ex)
            {
                response.StatusCode = (int)Status.Failed;
                response.StatusMessage = Resource.errorDRCarrierSeq;
                LogManager.Logger.WriteException("DRCarrierSequenceDomain", "UpdateDRCarrierSequence", ex.Message, ex);
            }
            return response;
        }
    }
}
