using MongoDB.Bson;

namespace SiteFuel.MdbDataAccess.Collections
{
   public class RegionShiftDetail
    {
        public ObjectId ShiftId { get; set; }
        public int ColumnIndex { get; set; }
    }
}
