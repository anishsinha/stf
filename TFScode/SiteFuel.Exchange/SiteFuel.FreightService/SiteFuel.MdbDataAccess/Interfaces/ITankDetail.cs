using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.MdbDataAccess.Interfaces
{
    public interface ITankDetail
    {
        ObjectId Id { get; set; }
        int TfxAssetId { get; set; }
        string StorageTypeId { get; set; }
        string StorageId { get; set; }
        string TankName { get; set; }
        string TankNumber { get; set; }
        string Manufacturer { get; set; }
        Nullable<decimal> FuelCapacity { get; set; }
        Nullable<decimal> ThresholdDeliveryRequest { get; set; }
        Nullable<decimal> MinFill { get; set; }
        Nullable<int> FillType { get; set; }
        Nullable<decimal> MaxFill { get; set; }
        Nullable<decimal> PhysicalPumpStop { get; set; }
        Nullable<decimal> RunOutLevel { get; set; }
        Nullable<decimal> NotificationUponUsageSwing { get; set; }
        Nullable<decimal> NotificationUponUsageSwingValue { get; set; }
        Nullable<decimal> NotificationUponInventorySwing { get; set; }
        Nullable<decimal> NotificationUponInventorySwingValue { get; set; }
        Nullable<int> TankType { get; set; }
        Nullable<int> DipTestMethod { get; set; }
        Nullable<int> ManiFolded { get; set; }
        Nullable<int> TankConstruction { get; set; }
        string TankAcceptDelivery { get; set; }
    }
}
