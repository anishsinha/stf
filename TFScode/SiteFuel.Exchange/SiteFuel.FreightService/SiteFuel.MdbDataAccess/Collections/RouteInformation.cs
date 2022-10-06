using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.MdbDataAccess.Collections
{
    public class RouteInformations
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public List<TfxJobsDetails> TfxJobs { get; set; } = new List<TfxJobsDetails>();
        public ShiftInfo ShiftInfo { get; set; } = null;
        public ObjectId RegionId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime UpdatedDate { get; set; }
        public int TfxCompanyId { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
    }
    public class ShiftInfo
    {

        public ObjectId Id { get; set; }
        public ObjectId TripId { get; set; }
        public int DriverRowIndex { get; set; }
        public int DriverColIndex { get; set; }
        public int ShiftIndex { get; set; }
    }
    public class TfxJobsDetails
    {
        public int Id { get; set; }
        public int SequenceNo { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
