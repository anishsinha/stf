using MongoDB.Bson;
using SiteFuel.FreightModels;
using SiteFuel.MdbDataAccess.Collections;
using System.Collections.Generic;

namespace SiteFuel.FreightRepository.Mappers
{
    public static class DSBColumnOptionalPickupInfoMapper
    {
        public static List<DSBColumnOptionalPickupInfo> ToEntity(this List<DSBColumnOptionalPickupInfoModel> model)
        {
            List<DSBColumnOptionalPickupInfo> dSBColumnOptionalPickups = new List<DSBColumnOptionalPickupInfo>();
            if (model == null)
            {
                return null;
            }
            else
            {

                foreach (var item in model)
                {

                    ObjectId ObjRegionId = ObjectId.Empty;
                    ObjectId.TryParse(item.RegionId, out ObjRegionId);

                    ObjectId ObjScheduleBuilderId = ObjectId.Empty;
                    ObjectId.TryParse(item.ScheduleBuilderId, out ObjScheduleBuilderId);

                    ObjectId ObjShiftId = ObjectId.Empty;
                    ObjectId.TryParse(item.ShiftId, out ObjShiftId);

                    DSBColumnOptionalPickupInfo dSBColumnOptionalPickup = new DSBColumnOptionalPickupInfo();
                    dSBColumnOptionalPickup.RegionId = ObjRegionId;
                    dSBColumnOptionalPickup.CompanyId = item.CompanyId;
                    dSBColumnOptionalPickup.ScheduleBuilderId = ObjScheduleBuilderId;
                    dSBColumnOptionalPickup.ShiftId = ObjShiftId;
                    dSBColumnOptionalPickup.ShiftIndex = item.ShiftIndex;
                    dSBColumnOptionalPickup.DriverColIndex = item.DriverColIndex;
                    dSBColumnOptionalPickup.TfxFuelTypeId = item.TfxFuelTypeId;
                    dSBColumnOptionalPickup.TfxFuelTypeName = item.TfxFuelTypeName;
                    dSBColumnOptionalPickup.DSBPickupLocationInfo = item.DSBPickupLocationInfo.ToEntity();
                    dSBColumnOptionalPickups.Add(dSBColumnOptionalPickup);

                }
            }
            return dSBColumnOptionalPickups;
        }
        public static DSBPickupLocationInfo ToEntity(this DSBPickupLocationInfoModel model)
        {

            if (model == null)
            {
                return null;
            }
            else
            {

                DSBPickupLocationInfo dSBPickupLocationInfo = new DSBPickupLocationInfo();
                dSBPickupLocationInfo.PickupLocationType = model.PickupLocationType;
                if (model.PickupLocationType == (int)Exchange.Utilities.PickupLocationType.BulkPlant)
                {
                    dSBPickupLocationInfo.TfxBulkPlant = model.TfxBulkPlant;
                }
                else
                {
                    dSBPickupLocationInfo.TfxTerminal = new MdbDataAccess.Collections.DropdownDisplayItem()
                    {
                        Id = model.TfxTerminal.Id,
                        Name = model.TfxTerminal.Name
                    };
                }
                dSBPickupLocationInfo.BadgeNo1 = model.BadgeNo1;
                dSBPickupLocationInfo.BadgeNo2 = model.BadgeNo2;
                dSBPickupLocationInfo.BadgeNo3 = model.BadgeNo3;
                return dSBPickupLocationInfo;
            }

        }
        public static List<DSBColumnOptionalPickupInfoModel> ToEntity(this List<DSBColumnOptionalPickupInfo> model)
        {
            List<DSBColumnOptionalPickupInfoModel> dSBColumnOptionalPickups = new List<DSBColumnOptionalPickupInfoModel>();
            if (model == null)
            {
                return null;
            }
            else
            {

                foreach (var item in model)
                {

                    DSBColumnOptionalPickupInfoModel dSBColumnOptionalPickup = new DSBColumnOptionalPickupInfoModel();
                    dSBColumnOptionalPickup.Id = item.Id.ToString();
                    dSBColumnOptionalPickup.RegionId = item.RegionId.ToString();
                    dSBColumnOptionalPickup.CompanyId = item.CompanyId;
                    dSBColumnOptionalPickup.ScheduleBuilderId = item.ScheduleBuilderId.ToString();
                    dSBColumnOptionalPickup.ShiftId = item.ShiftId.ToString();
                    dSBColumnOptionalPickup.ShiftIndex = item.ShiftIndex;
                    dSBColumnOptionalPickup.DriverColIndex = item.DriverColIndex;
                    dSBColumnOptionalPickup.TfxFuelTypeId = item.TfxFuelTypeId;
                    dSBColumnOptionalPickup.TfxFuelTypeName = item.TfxFuelTypeName;
                    dSBColumnOptionalPickup.DSBPickupLocationInfo = item.DSBPickupLocationInfo.ToEntity();
                    dSBColumnOptionalPickups.Add(dSBColumnOptionalPickup);

                }
            }
            return dSBColumnOptionalPickups;
        }
        public static DSBPickupLocationInfoModel ToEntity(this DSBPickupLocationInfo model)
        {

            if (model == null)
            {
                return null;
            }
            else
            {

                DSBPickupLocationInfoModel dSBPickupLocationInfo = new DSBPickupLocationInfoModel();
                dSBPickupLocationInfo.PickupLocationType = model.PickupLocationType;
                if (model.PickupLocationType == (int)Exchange.Utilities.PickupLocationType.BulkPlant)
                {
                    dSBPickupLocationInfo.TfxBulkPlant = model.TfxBulkPlant;
                }
                else
                {
                    dSBPickupLocationInfo.TfxTerminal = new FreightModels.DropdownDisplayItem
                    {
                        Id = model.TfxTerminal.Id,
                        Name = model.TfxTerminal.Name
                    };
                }
                dSBPickupLocationInfo.BadgeNo1 = model.BadgeNo1;
                dSBPickupLocationInfo.BadgeNo2 = model.BadgeNo2;
                dSBPickupLocationInfo.BadgeNo3 = model.BadgeNo3;
                return dSBPickupLocationInfo;
            }

        }
    }
}
