using MongoDB.Bson;
using SiteFuel.FreightModels;
using SiteFuel.MdbDataAccess.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SiteFuel.FreightRepository.Mappers
{
    public static class DRCarrierSequenceMapper
    {
        public static List<DRCarrierSequence> ToEntity(this List<DRCarrierSequenceModel> model)
        {
            List<DRCarrierSequence> dRCarrierSequenceList = new List<DRCarrierSequence>();
            if (model == null)
            {
                return null;
            }
            else
            {

                foreach (var item in model)
                {
                    ObjectId ObjDeliveryReqId = ObjectId.Empty;
                    ObjectId.TryParse(item.DeliveryRequestId, out ObjDeliveryReqId);

                    ObjectId ObjRegionIdReqId = ObjectId.Empty;
                    ObjectId.TryParse(item.RegionId, out ObjRegionIdReqId);

                    DRCarrierSequence dRCarrierSequence = new DRCarrierSequence();
                    dRCarrierSequence.TfxSupplierCompanyId = item.TfxSupplierCompanyId;
                    dRCarrierSequence.TfxSupplierOrderId = item.TfxSupplierOrderId;
                    dRCarrierSequence.DeliveryRequestId = ObjDeliveryReqId;
                    dRCarrierSequence.RegionId = ObjRegionIdReqId;
                    dRCarrierSequence.CarrierInfo = item.CarrierInfo.ToEntity();
                    dRCarrierSequenceList.Add(dRCarrierSequence);

                }
            }
            return dRCarrierSequenceList;
        }
        public static DRCarrierSequence ToEntity(this DRCarrierSequenceModel model)
        {

            if (model == null)
            {
                return null;
            }
            else
            {

                ObjectId ObjDeliveryReqId = ObjectId.Empty;
                ObjectId.TryParse(model.DeliveryRequestId, out ObjDeliveryReqId);

                ObjectId ObjRegionIdReqId = ObjectId.Empty;
                ObjectId.TryParse(model.RegionId, out ObjRegionIdReqId);

                DRCarrierSequence dRCarrierSequence = new DRCarrierSequence();
                dRCarrierSequence.DeliveryRequestId = ObjDeliveryReqId;
                dRCarrierSequence.RegionId = ObjRegionIdReqId;
                dRCarrierSequence.CarrierInfo = model.CarrierInfo.ToEntity();
                return dRCarrierSequence;
            }

        }
        public static DRCarrierSequenceModel ToEntity(this DRCarrierSequence model)
        {
            if (model == null)
            {
                return null;
            }
            else
            {
                DRCarrierSequenceModel dRCarrierSequence = new DRCarrierSequenceModel();
                dRCarrierSequence.Id = model.Id.ToString();
                dRCarrierSequence.DeliveryRequestId = model.DeliveryRequestId.ToString();
                dRCarrierSequence.RegionId = model.RegionId.ToString();
                dRCarrierSequence.CarrierInfo = model.CarrierInfo.ToEntity();
                return dRCarrierSequence;
            }
        }
        public static List<DRCarrierSequenceModel> ToEntity(this List<DRCarrierSequence> model)
        {
            List<DRCarrierSequenceModel> dRCarrierSequenceList = new List<DRCarrierSequenceModel>();
            if (model == null)
            {
                return null;
            }
            else
            {
                foreach (var item in model)
                {
                    DRCarrierSequenceModel dRCarrierSequence = new DRCarrierSequenceModel();
                    dRCarrierSequence.Id = item.Id.ToString();
                    dRCarrierSequence.DeliveryRequestId = item.DeliveryRequestId.ToString();
                    dRCarrierSequence.RegionId = item.RegionId.ToString();
                    dRCarrierSequence.TfxSupplierCompanyId = item.TfxSupplierCompanyId;
                    dRCarrierSequence.TfxSupplierOrderId = item.TfxSupplierOrderId;
                    dRCarrierSequence.CarrierInfo = item.CarrierInfo.ToEntity();
                    dRCarrierSequenceList.Add(dRCarrierSequence);
                }

            }
            return dRCarrierSequenceList;
        }
        public static List<TfxCarrierDropdownDisplayItem> ToEntity(this List<TfxCarrierDropdownDisplayViewModelItem> model)
        {
            List<TfxCarrierDropdownDisplayItem> carrierRegionInfos = new List<TfxCarrierDropdownDisplayItem>();
            if (model != null && model.Any())
            {
                foreach (var item in model)
                {
                    TfxCarrierDropdownDisplayItem carrierRegionInfo = new TfxCarrierDropdownDisplayItem();
                    carrierRegionInfo.Id = item.Id;
                    carrierRegionInfo.Name = item.Name;
                    carrierRegionInfo.Code = item.Code;
                    carrierRegionInfo.SequenceNo = item.SequenceNo;
                    carrierRegionInfo.RegionId = item.RegionId;
                    carrierRegionInfos.Add(carrierRegionInfo);
                }
            }
            return carrierRegionInfos;
        }
        public static List<TfxCarrierDropdownDisplayViewModelItem> ToEntity(this List<TfxCarrierDropdownDisplayItem> model)
        {
            List<TfxCarrierDropdownDisplayViewModelItem> carrierRegionInfos = new List<TfxCarrierDropdownDisplayViewModelItem>();
            if (model != null && model.Any())
            {
                foreach (var item in model)
                {
                    TfxCarrierDropdownDisplayViewModelItem carrierRegionInfo = new TfxCarrierDropdownDisplayViewModelItem();
                    carrierRegionInfo.Id = item.Id;
                    carrierRegionInfo.Name = item.Name;
                    carrierRegionInfo.Code = item.Code;
                    carrierRegionInfo.SequenceNo = item.SequenceNo;
                    carrierRegionInfo.RegionId = item.RegionId;
                    carrierRegionInfos.Add(carrierRegionInfo);
                }
            }
            return carrierRegionInfos;
        }

        public static TfxCarrierRejectInfo ToEntity(this DRCarrierRejectInfoModel model)
        {
            if (model == null)
            {
                return null;
            }
            else
            {
                TfxCarrierRejectInfo drCarrierRejectInfo = new TfxCarrierRejectInfo();
                drCarrierRejectInfo.Id = model.CarrierRejectInfo.Id;
                drCarrierRejectInfo.Name = model.CarrierRejectInfo.Name;
                drCarrierRejectInfo.RejectDate = model.CarrierRejectInfo.RejectDate;
                drCarrierRejectInfo.RejectTime = model.CarrierRejectInfo.RejectTime;
                drCarrierRejectInfo.RejectedBy = model.CarrierRejectInfo.RejectedBy;
                return drCarrierRejectInfo;
            }
        }

    }

}
