using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SiteFuel.MdbDataAccess.Collections
{
   public  class TrailerShiftDetail
    {
        public ObjectId ShiftId { get; set; }
        public int ColumnId { get; set; }
    }
}
