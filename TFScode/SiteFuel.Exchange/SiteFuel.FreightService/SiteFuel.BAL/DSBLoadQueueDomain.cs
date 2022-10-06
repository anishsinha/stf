using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
using SiteFuel.FreightRepository;

namespace SiteFuel.BAL
{
    public class DSBLoadQueueDomain : IDSBLoadQueueDomain
    {
        private IDSBLoadQueueRepository _dSBLoadQueueRepository;
        public DSBLoadQueueDomain(IDSBLoadQueueRepository dSBLoadQueueRepository)
        {
            _dSBLoadQueueRepository = dSBLoadQueueRepository;
        }
        public async Task<StatusModel> CreateDsbLoadQueue(List<DSBLoadQueueModel> dSBLoadQueue)
        {
            var response = new StatusModel();
            try
            {
                response = await _dSBLoadQueueRepository.CreateDsbLoadQueue(dSBLoadQueue);
            }
            catch (Exception ex)
            {
                response.StatusCode = (int)Status.Failed;
                LogManager.Logger.WriteException("DSBLoadQueueDomain", "CreateDSBLoadQueue", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusModel> DeleteDsbLoadQueue(List<string> dSBLoadQueue)
        {
            var response = new StatusModel();
            try
            {
                response = await _dSBLoadQueueRepository.DeleteDsbLoadQueue(dSBLoadQueue);
            }
            catch (Exception ex)
            {
                response.StatusCode = (int)Status.Failed;
                LogManager.Logger.WriteException("DSBLoadQueueDomain", "DeleteDSBLoadQueue", ex.Message, ex);
            }
            return response;
        }
    }
}
