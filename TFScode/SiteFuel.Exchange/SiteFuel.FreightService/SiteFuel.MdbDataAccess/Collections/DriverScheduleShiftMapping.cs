using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace SiteFuel.MdbDataAccess.Collections
{
   public  class DriverScheduleShiftMapping : CommonFields
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public int DriverId { get; set; }
        public bool IsUnplanedSchedule { get; set; }    
        public List<DriverSchedule> DriverScheduleList { get; set; }= new List<DriverSchedule>();
    }
}
