using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.MdbDataAccess.Collections
{
    public class DrFilterPreferences
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public int UserId { get; set; }
        public int CompanyId { get; set; }
        public BsonDocument FilterData { get; set; }
        public string RegionId { get; set; }
        public DateTimeOffset Date { get; set; }
    }
}
