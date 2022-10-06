using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace SiteFuel.MdbDataAccess.Collections
{
   public  class TrailerScheduleMapping : CommonFields
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public ObjectId RegionId { get; set; }
        public ObjectId TrailerId { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public List<DateTimeOffset> RepeatDayList { get; set; }
        public List<TrailerShiftDetail> TrailerShiftDetail { get; set; }= new List<TrailerShiftDetail>();
    }
}
