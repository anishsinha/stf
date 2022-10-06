using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class BaseResponse
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
    }

    public class UserResponseModel : BaseResponse
    {
        public int UserID { get; set; }
        public int CompanyID { get; set; }
    }


    public class FtlDropResponseModel : BaseResponse
    {
        public string SplitLoadChainId { get; set; }
    }

    public class InsertResponseModel : BaseResponse
    {
        public int NewInsertedId { get; set; }
        public int AssetImageId { get; set; }
        public int JobXAssignmentId { get; set; }
    }

    public class NewOrderData : OrderBaseData
    {
        public bool overWater { get; set; }
        public bool wetHosing { get; set; }
        public string customerName { get; set; }
        public override string ToString()
        {
            return base.ToString() + ",overWater: " + overWater + ",wetHosing: " + wetHosing + ",customerName: " + customerName;
        }
    }

    public class NewOrderRequestModel
    {
        public NewOrderData data { get; set; }
        public string receipt { get; set; }
        public SignatureData SignatureData { get; set; }
        public override string ToString()
        {
            base.ToString();
            return " receipt: " + receipt + ",data: " + data.ToString();
        }
    }

    public class SignatureData
    {
        public int Id { get; set; }
        public string SignatoryName { get; set; }
        public string SignatureImage { get; set; }
        public bool SignatoryAvailable { get; set; }
    }

    public class DemurrageDetailsData
    {
        public long StartTime { get; set; }
        public int StartOffset { get; set; }
        public long EndTime { get; set; }
        public int EndOffset { get; set; }
        public int FeeTypeId { get; set; }
    }

    public class FuelTruckRetainDetailsData
    {
        public long StartTime { get; set; }
        public int StartOffset { get; set; }
        public long EndTime { get; set; }
        public int EndOffset { get; set; }
    }

    public class DisptachLocationData
    {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public bool IsBulkPlant { get; set; }
    }

    public class BolDetailsData
    {
        public decimal GrossQuantity { get; set; }
        public decimal NetQuantity { get; set; }
        public string BolNumber { get; set; }
        public string Carrier { get; set; }
        public string BolImage { get; set; }
        public int ImageId { get; set; }
        public int Id { get; set; }
        public DateTimeOffset? LiftDate { get; set; }
    }

    public class DropRequestModel
    {
        public DropOrderData data { get; set; }
        public string receipt { get; set; }
        public Dictionary<string, bool> specialInstructions { get; set; }
        public SignatureData SignatureData { get; set; }
        public DisptachLocationData FuelPickLocation { get; set; }
        public BolDetailsData BolDetails { get; set; }
        public int? AdditionalImageId { get; set; }
        public override string ToString()
        {
            return " receipt: " + receipt + ",data: " + data.ToString();
        }
    }

    public class FtlDropRequestModel : DropRequestModel
    {
        public List<DemurrageDetailsData> DemurrageDetails { get; set; }
        public FuelTruckRetainDetailsData FuelTruckRetainDetails { get; set; }
        public DisptachLocationData FuelDropLocation { get; set; }
        public bool IsSplitTank{ get; set; }
        public bool IsSplitLoad { get; set; }
        public string SplitLoadChainId { get; set; }
        public int FuelSurchargeDistance { get; set; }
        public override string ToString()
        {
            return " receipt: " + receipt + ",data: " + data.ToString();
        }
    }

    public class AssetDropData : OrderBaseData
    {
        public int AssetId { get; set; }
        public int InvoiceId { get; set; }
        public int OrderId { get; set; }
        public RunningMeterMode RunningMeterMode { get; set; }
        public decimal PrimaryMeterStartReading { get; set; }
        public decimal SecondaryMeterStartReading { get; set; }
        public decimal PrimaryMeterEndReading { get; set; }
        public decimal SecondaryMeterReading { get; set; }
        public decimal AdditionalDrop { get; set; }
        public bool IsNoFuelNeeded { get; set; }
        public int AssetDropId { get; set; }
        public int JobXAssignmentId { get; set; }
        public int DropStatus { get; set; }
        public bool IsNewAsset { get; set; }
        public string Gravity { get; set; }
        public List<AssetDropRequestViewModel> AssetDropDetail { get; set; }
        public override string ToString()
        {
            return base.ToString() + " ,AssetId: " + AssetId + " ,InvoiceId: " + InvoiceId + " ,AdditionalDrop: " + AdditionalDrop + " ,OrderId: " + OrderId + " ,IsNoFuelNeeded: " + IsNoFuelNeeded;
        }
    }

    public class AssetDropRequestModel
    {
        public AssetDropData data { get; set; }
        public string receipt { get; set; }
        public override string ToString()
        {
            return base.ToString() + ", receipt: " + receipt + ",data: " + data.ToString();
        }
    }

    public class DropOrderData : OrderBaseData
    {
        public string orderId { get; set; }
        public string fuelId { get; set; } // fuel detail ID

        public override string ToString()
        {
            return base.ToString() + ",orderId: " + orderId + ",fuelId: " + fuelId;
        }
    }

    public abstract class OrderBaseData
    {
        public int userid { get; set; }
        public int companyid { get; set; }
        public string quantity { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string FCMAppId { get; set; }
        public string TraceId { get; set; }
        public int assetCount { get; set; }
        public int InvoiceStatusId { get; set; }
        public TimeStamps timeStamps { get; set; }
        public int? TrackableScheduleId { get; set; }
        public int? DeliveryScheduleId { get; set; }
        public int UnitOfMeasurement { get; set; }
        public override string ToString()
        {
            return " userid: " + userid + ",companyid: " + companyid + ",quantity: " + quantity + ",Latitude: " + Latitude + ",Longitude: " + Longitude + ",assetCount: " + assetCount;
        }
    }

    public class DeleteSpillDetails
    {
        public int deleteSpillId { get; set; }
        public int companyID { get; set; }
    }

    public class DeleteSpillImage
    {
        public int spillId { get; set; }
        public int imageId { get; set; }
    }

}
