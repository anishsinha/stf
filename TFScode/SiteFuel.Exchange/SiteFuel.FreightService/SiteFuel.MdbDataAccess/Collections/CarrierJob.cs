using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.MdbDataAccess.Collections
{
    public class CarrierJob
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public int TfxCarrierCompanyId { get; set; }
        public int TfxJobId { get; set; }
        public string TfxJobName { get; set; }
        public int TfxJobCompanyId { get; set; }
        public string TfxBuyerCompanyName { get; set; }
        public int TfxSupplierCompanyId { get; set; }
        public bool IsActive { get; set; }
    }
}
