using MongoDB.Bson;
using SiteFuel.FreightModels;
using SiteFuel.MdbDataAccess.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightRepository
{
    public interface IFreightTableRepository
    {
        Task<bool> DeleteAllRecords();
        Task<string> AddFreightTable(FreightTableModel freightTable);
        Task<StatusModel> AddFreightTablePrices(List<FreightTablePriceModel> freightPrices);
    }
}
