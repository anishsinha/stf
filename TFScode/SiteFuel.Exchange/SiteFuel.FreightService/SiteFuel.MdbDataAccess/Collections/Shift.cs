using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.MdbDataAccess.Collections
{
    public class Shift : CommonFields
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public ObjectId RegionId { get; set; }
        public string Name { get; set; }
        public int CompanyId { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}
