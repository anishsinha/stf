using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.MdbDataAccess.Collections
{
   public class ExternalVehicleMappings
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string TruckId { get; set; }
        public string TargetVehicleValue { get; set; }
        public int ThirdPartyId { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
    }
}
