using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.MdbDataAccess
{
    public class DSBLoadQueue
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public ObjectId ScheduleBuilderId { get; set; } = ObjectId.Empty;
        public ObjectId RegionId { get; set; } = ObjectId.Empty;

        [BsonDateTimeOptions(Kind = DateTimeKind.Local, DateOnly = true)]
        public DateTime Date { get; set; }
        public ObjectId ShiftId { get; set; } = ObjectId.Empty;
        public int ShiftIndex { get; set; }
        public int DriverRowIndex { get; set; }
        public int TfxUserId { get; set; }
        public int TfxCompanyId { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local, DateOnly = true)]
        public DateTime CreateDate { get; set; }
        public int TfxCreatedBy { get; set; }

    }
}
