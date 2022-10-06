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
    public class TankDetail : ITankDetail
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public int TfxAssetId { get; set; }
        public string StorageTypeId { get; set; }
        public string StorageId { get; set; }
        public string TankName { get; set; }
        public string TankNumber { get; set; }
        public string Manufacturer { get; set; }
        public Nullable<decimal> FuelCapacity { get; set; }
        public Nullable<decimal> ThresholdDeliveryRequest { get; set; }
        public Nullable<decimal> MinFill { get; set; }
        public Nullable<int> FillType { get; set; }
        public Nullable<decimal> MaxFill { get; set; }
        public Nullable<decimal> PhysicalPumpStop { get; set; }
        public Nullable<decimal> RunOutLevel { get; set; }
        public Nullable<decimal> NotificationUponUsageSwing { get; set; }
        public Nullable<decimal> NotificationUponUsageSwingValue { get; set; }
        public Nullable<decimal> NotificationUponInventorySwing { get; set; }
        public Nullable<decimal> NotificationUponInventorySwingValue { get; set; }
        public Nullable<int> TankType { get; set; }
        public Nullable<int> DipTestMethod { get; set; }
        public Nullable<int> ManiFolded { get; set; }
        public Nullable<int> TankConstruction { get; set; }
        public string TankAcceptDelivery { get; set; }
        public int TfxProductTypeId { get; set; }
        public string TfxProductTypeName { get; set; }
        public string TankModelTypeId { get; set; }
        public List<int> TanksConnected { get; set; }
        public Nullable<int> TankSequence { get; set; }
        public string PedigreeAssetDBID { get; set; } // for pedigree datasource
        public int TfxFuelTypeId { get; set; }

        public string SkyBitzRTUID { get; set; }// for skybitz datasource

        public string ExternalTankId { get; set; }
        public Nullable<decimal> WaterLevel { get; set; } = 0;
        public string Port { get; set; }
        public string VeederRootIPAddress { get; set; }
        public bool IsStopATGPolling { get; set; }
    }
}
