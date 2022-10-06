using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace SiteFuel.MdbDataAccess.Collections
{
    public class RegionScheduleMapping : CommonFields
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public ObjectId RegionId { get; set; }
        public ObjectId RouteId { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local, DateOnly = true)]
        public DateTime StartDate { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local, DateOnly = true)]
        public DateTime EndDate { get; set; }
       
        public List<string> RepeatDayList { get; set; }
        public string Description { get; set; }
        public List<RegionShiftDetail> RegionShiftDetail { get; set; } = new List<RegionShiftDetail>();
    }
}
