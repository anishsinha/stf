using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
using SiteFuel.MdbDataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.MdbDataAccess.Collections
{
    public class DsbNotification
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Message { get; set; }
        public int Status { get; set; }
        public int Type { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ScheduleBuilderId { get; set; }
        public int TfxJobId { get; set; }
        public string RegionId { get; set; }
        public int CreatedBy { get; set; }
    }
}
