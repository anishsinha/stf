using MongoDB.Bson;
using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
using SiteFuel.MdbDataAccess.Collections;
using SiteFuel.MdbDataAccess.DbContext;
using SiteFuel.FreightRepository.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightRepository
{
    public class FreightTableRepository : IFreightTableRepository
    {
        private readonly MdbContext mdbContext;
        public FreightTableRepository()
        {
            if (mdbContext == null)
            {
                mdbContext = new MdbContext();

            }
        }
        public FreightTableRepository(MdbContext _context)
        {
            mdbContext = _context;
        }
        public async Task<bool> DeleteAllRecords()
        {
            var filter1 = MongoDB.Driver.FilterDefinition<FreightTable>.Empty;
            await mdbContext.FreightTables.DeleteManyAsync(filter1);
            var filter2 = MongoDB.Driver.FilterDefinition<FreightTablePrice>.Empty;
            await mdbContext.FreightTablePrices.DeleteManyAsync(filter2);
            return true;
        }

        public async Task<string> AddFreightTable(FreightTableModel freightTableModel)
        {
            var freightTable = freightTableModel.ToEntity();
            await mdbContext.FreightTables.InsertOneAsync(freightTable);
            return freightTable.Id.ToString();
        }

        public async Task<StatusModel> AddFreightTablePrices(List<FreightTablePriceModel> freightPrices)
        {
            var response = new StatusModel();
            var prices = freightPrices.Select(t => t.ToEntity());
            await mdbContext.FreightTablePrices.InsertManyAsync(prices);
            response.StatusCode = (int)Status.Success;
            return response;
        }
    }
}
