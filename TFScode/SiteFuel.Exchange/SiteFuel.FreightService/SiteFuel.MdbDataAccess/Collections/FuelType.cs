using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.MdbDataAccess.Collections
{
    public class Compartment
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public decimal FuelCapacity { get; set; }
    }
}
