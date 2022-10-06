using MongoDB.Bson;
using SiteFuel.MdbDataAccess.PriceFactories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.MdbDataAccess.Interfaces
{
    public interface IFreightTablePrice
    {
        ObjectId Id { get; set; }
        int CompanyId { get; set; }
        ObjectId FreightTableId { get; set; }
        List<Price> FreightPrices { get; set; }
    }
}
