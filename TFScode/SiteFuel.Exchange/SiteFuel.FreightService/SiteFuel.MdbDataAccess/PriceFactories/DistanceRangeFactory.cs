using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.MdbDataAccess.PriceFactories
{
    public class DistanceRangeFactory : PriceFactory
    {
        private readonly BsonDocument _bsonDoc;
        public DistanceRangeFactory(BsonDocument bsonDoc)
        {
            _bsonDoc = bsonDoc;
        }
        public override Price GetPrice()
        {
            return BsonSerializer.Deserialize<DistanceRangePrice>(_bsonDoc);
        }
    }
}
