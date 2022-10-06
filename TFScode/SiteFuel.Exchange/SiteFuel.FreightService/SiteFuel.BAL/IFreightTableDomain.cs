using SiteFuel.FreightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.BAL
{
    public interface IFreightTableDomain
    {
        Task<bool> DeleteAllRecords();
        Task<FreightTableResponseModel> AddFreightTable(FreightTableModel table);
        Task<StatusModel> AddFreightTablePricings(List<FreightTablePriceModel> pricings);
    }
}
