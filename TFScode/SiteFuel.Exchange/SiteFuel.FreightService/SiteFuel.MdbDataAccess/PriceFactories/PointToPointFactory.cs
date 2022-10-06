using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.MdbDataAccess.PriceFactories
{
    public class PointToPointFactory : PriceFactory
    {
        private readonly BsonDocument _bsonDoc;
        public PointToPointFactory(BsonDocument bsonDoc)
        {
            _bsonDoc = bsonDoc;
        }
        public override Price GetPrice()
        {
            return BsonSerializer.Deserialize<PointToPointPrice>(_bsonDoc);
        }
    }
}
