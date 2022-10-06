using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace SiteFuel.MdbDataAccess.Collections
{
    public class DRCarrierSequence
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public ObjectId DeliveryRequestId { get; set; } = ObjectId.Empty;
        public ObjectId RegionId { get; set; } = ObjectId.Empty;

        public int TfxSupplierCompanyId { get; set; }
        public int TfxSupplierOrderId { get; set; }
        public List<TfxCarrierDropdownDisplayItem> CarrierInfo { get; set; } = new List<TfxCarrierDropdownDisplayItem>();
        public List<TfxCarrierRejectInfo> CarrierRejectInfo = new List<TfxCarrierRejectInfo>();
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        [BsonElement]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
    public class TfxCarrierRejectInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [BsonElement]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime RejectDate { get; set; } = DateTime.Now;
        public TimeSpan RejectTime { get; set; }
        public int RejectedBy { get; set; }
    }
}
