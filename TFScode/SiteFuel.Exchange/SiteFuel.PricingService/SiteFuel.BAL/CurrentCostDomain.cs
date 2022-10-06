using SiteFuel.DAL;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Models;
using System;
using System.Collections.Generic;
using SiteFuel.BAL.Mappers;
using System.Threading.Tasks;
using SiteFuel.DataAccess.Entities;
using System.Text;

namespace SiteFuel.BAL
{
    public class CurrentCostDomain : ICurrentCostDomain
    {
        ICurrentCostRepository _currentCostRepository;
        public CurrentCostDomain(ICurrentCostRepository currentCostRepository)
        {
            _currentCostRepository = currentCostRepository;
        }

        public async Task<CurrentCostResponseModel> UpdateSupplierCostToPriceDetail(CurrentCostRequestModel requestModel)
        {
            var response = new CurrentCostResponseModel();
            try
            {
                response = await _currentCostRepository.UpdateSupplierCostToPriceDetail(requestModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CurrentCostDomain", "UpdateSupplierCostToPriceDetail", ex.Message, ex);
            }
            return response;
        }

    }
}
