using MongoDB.Bson;
using SiteFuel.MdbDataAccess.Interfaces;
using SiteFuel.MdbDataAccess.PriceFactories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.MdbDataAccess.Collections
{
    public class FreightTablePrice : IFreightTablePrice
    {
        public ObjectId Id { get; set; }
        public int CompanyId { get; set; }
        public ObjectId FreightTableId { get; set; }
        public List<Price> FreightPrices { get; set; }
    }
}
