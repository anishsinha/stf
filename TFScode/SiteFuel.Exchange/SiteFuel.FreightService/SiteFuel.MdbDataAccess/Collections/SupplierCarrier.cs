using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.MdbDataAccess.Collections
{
    public class Carrier : CommonFields
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public int TfxCarrierCompanyId { get; set; }
        public string TfxCarrierCompanyName { get; set; }
        public int TfxSupplierCompanyId { get; set; }
        public string TfxSupplierCompanyName { get; set; }
    }
}
