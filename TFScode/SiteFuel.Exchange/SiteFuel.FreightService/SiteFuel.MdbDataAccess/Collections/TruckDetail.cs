using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SiteFuel.Exchange.Utilities;
using SiteFuel.MdbDataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.MdbDataAccess.Collections
{
    public class TruckDetail
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }
        public string TruckId { get; set; }
        public decimal FuelCapacity { get; set; }
        public decimal OptimizedCapacity { get; set; }
        public string ContractNumber { get; set; }
        public TrailerTypeStatus TrailerType { get; set; }
        public List<TruckCompartment> Compartments { get; set; }
        public int TfxCreatedBy { get; set; }
        public int TfxCompanyId { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public TruckStatus Status { get; set; }

        public LicenceRequirementStatus LicenceRequirement { get; set; }
        public string LicencePlate { get; set; }
        public string ExpirationDate { get; set; }
        public string IsPump { get; set; }
        public int TfxUpdatedBy { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public bool IsFilldCompatible { get; set; }
        public string FuelResetUserId { get; set; }
        public string FuelResetUserName { get; set; }
        public string SmartDeviceId { get; set; }
    }

    public class TruckCompartment
    {
        public string CompartmentId { get; set; }
        public decimal Capacity { get; set; }
        public int FuelType { get; set; }
        public string PumpId { get; set; }
    }
}
