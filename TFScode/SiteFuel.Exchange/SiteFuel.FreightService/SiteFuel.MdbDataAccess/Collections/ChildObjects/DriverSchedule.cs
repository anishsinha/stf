using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SiteFuel.MdbDataAccess.Collections
{
   public  class DriverSchedule
    {
       
        public String Id { get; set; }
        public ObjectId ShiftId { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public List<DateTimeOffset> RepeatDayList { get; set; }           
        public bool IsActive { get; set; }
        public string Description { get; set; }
        public int? RepeatEveryDay { get; set; }
        public int TypeId { get; set; }
    }
}
