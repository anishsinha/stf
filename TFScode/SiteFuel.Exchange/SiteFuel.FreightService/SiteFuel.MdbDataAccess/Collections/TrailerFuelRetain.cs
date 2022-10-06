using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace SiteFuel.MdbDataAccess.Collections
{
    public class TrailerFuelRetain : CommonFields
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public ObjectId TrailerId { get; set; } = ObjectId.Empty;
        public string CompartmentId { get; set; }
        public int TfxDriverId { get; set; }
        public decimal Quantity { get; set; }
        public ObjectId DeliveryRequestId { get; set; } = ObjectId.Empty;
        public string ProductType { get; set; }
        public int ProductId { get; set; }
        public int? OrderId { get; set; }
        public int UOM { get; set; } = 0;
        public int IsExceptionConfirmed { get; set; } = 0;

    }
}
