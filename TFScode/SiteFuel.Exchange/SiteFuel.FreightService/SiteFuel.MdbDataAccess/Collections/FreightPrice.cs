using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SiteFuel.MdbDataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.MdbDataAccess.Collections
{
    public class FreightPrice : IFreightPrice
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public int Type { get; set; }
        public int CompanyId { get; set; }
        public decimal MinValue { get; set; }
        public decimal MaxValue { get; set; }
        public decimal Price { get; set; }
        public int Currency { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
