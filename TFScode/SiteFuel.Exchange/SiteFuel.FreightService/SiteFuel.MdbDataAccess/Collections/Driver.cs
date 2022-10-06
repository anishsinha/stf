using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.MdbDataAccess.Collections
{
    public class Driver : CommonFields
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public int DriverId { get; set; }
        public string DriverName { get; set; }
        public string CompanyName { get; set; }
        public string ExpiryDate { get; set; }
        public string LicenseTypeId { get; set; }

        public int CompanyId { get; set; }
        public string ProfilePhoto { get; set; }
        public string LicenseNumber { get; set; }
        public List<ObjectId> ShiftId { get; set; } = new List<ObjectId>();
        public List<ObjectId> Regions { get; set; } = new List<ObjectId>();

        public List<TrailerTypeStatus> TrailerType { get; set; } = new List<TrailerTypeStatus>();
        public List<TerminalCardNumber> CardNumbers { get; set; } = new List<TerminalCardNumber>();
        public bool IsFilldAuthorized { get; set; }
    }
}
