using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.MdbDataAccess.Collections
{
    public class Freight
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public decimal FuelCapacity { get; set; }
        public string ContractNumber { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string Description { get; set; }
        public FreightType FreightType { get; set; }
        public List<Compartment> Compartments { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public Freight_Type Type { get; set; }
    }
}
