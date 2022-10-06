using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.MdbDataAccess.PriceFactories
{
    public abstract class PriceFactory
    {
        public abstract Price GetPrice();
    }
}
