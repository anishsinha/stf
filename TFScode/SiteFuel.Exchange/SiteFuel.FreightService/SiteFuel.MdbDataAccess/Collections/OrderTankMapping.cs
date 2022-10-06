using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.MdbDataAccess.Collections
{
    public class OrderTankMapping
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public int JobId { get; set; }
        public string TankId { get; set; }
        public int OrderId { get; set; }
        public int FuelTypeId { get; set; }
        public int ProductTypeId { get; set; }
        public int SupplierCompanyId { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
    }
}
