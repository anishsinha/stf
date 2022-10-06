using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels.DeliveryRequest;
using System;
using System.Collections.Generic;
using TrueFill.DemandCaptureDataAccess.Entities;

namespace SiteFuel.FreightModels.ForcastingHelpers
{
    public class TankOttoDetails
    {
        public int TfxAssetId { get; set; }
        public string StorageId { get; set; }
        public string TankId { get; set; }
        public int TfxJobId { get; set; }
        public string TfxDisplayJobId { get; set; }
        public decimal? FuelCapacity { get; set; }
        public int? FillType { get; set; }
        public decimal? MaxFill { get; set; }
        public decimal? MinFill { get; set; }
        public decimal? RunOutLevel { get; set; }
        public decimal? ThresholdDeliveryRequest { get; set; }
        public int FuelTypeId { get; set; }
        public string ProductTypeName { get; set; }
        public decimal Retain { get; set; }
        public decimal SafetyStock { get; set; }
        public ForcastingServiceSetting forcastingServiceSettings { get; set; }
    }
    public class SCDeliveryWindowInfo
    {
        public DeliveryWindowInfoModel DeliveryWindowInfoModel { get; set; }
        public DeliveryReqPriority DeliveryReqPriority { get; set; }
    }
    public class SaleMonthlyDataModelDetails
    {
        public List<SaleMonthlyDataModel> SaleMonthlyDataModel { get; set; } = new List<SaleMonthlyDataModel>();
        public SaleTankModel SaleTankModel { get; set; } = new SaleTankModel();
    }
}
