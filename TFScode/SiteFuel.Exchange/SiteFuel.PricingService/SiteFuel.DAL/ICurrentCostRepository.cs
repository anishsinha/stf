using SiteFuel.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.DAL
{
	public interface ICurrentCostRepository
    {
        Task<CurrentCostResponseModel> UpdateSupplierCostToPriceDetail(CurrentCostRequestModel requestModel);
    }
}
