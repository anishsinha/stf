using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.MdbDataAccess.PriceFactories
{
    public class PointToPointPrice : Price
    {
        private readonly int _type;
        public PointToPointPrice()
        {
            _type = 1;
        }
        public PointToPointPrice(BsonDocument bsonDoc)
        {
            _type =1;
        }
        public override int Type { get { return _type; } }
        public override int CompanyId { get; set; }
        public override decimal Rate { get; set; }
        public override int Currency { get; set; }
        public override DateTimeOffset CreatedOn { get; set; }
        public string StartPoint { get; set; }
        public string EndPoint { get; set; }
        public override bool IsActive { get; set; }
        public override bool IsDeleted { get; set; }
    }
}
