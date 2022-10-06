using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.MdbDataAccess.Collections
{
    public class TractorDetail
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string TractorId { get; set; }
        public string VIN { get; set; }

        public string ExpirationDate { get; set; }
        public string Plate { get; set; }
        public List<TrailerTypeStatus> TrailerType { get; set; }

        public string Owner { get; set; }
        public List<DriverDetails> Drivers { get; set; }
        public int TfxCreatedBy { get; set; }
        public int TfxCompanyId { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public TractorStatus Status { get; set; }
        public int TfxUpdatedy { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        

    }
    public class DriverDetails
    {
        public int TfxId { get; set; }
        public string TfxName { get; set; }
    }
}