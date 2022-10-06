using SiteFuel.Exchange.Utilities;
using SiteFuel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.BAL
{
    public interface ICurrentCostDomain
    {
        Task<CurrentCostResponseModel> UpdateSupplierCostToPriceDetail(CurrentCostRequestModel requestModel);
    }
}
