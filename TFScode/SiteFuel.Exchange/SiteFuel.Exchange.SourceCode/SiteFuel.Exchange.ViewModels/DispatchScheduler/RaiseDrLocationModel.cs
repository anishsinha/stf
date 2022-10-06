using System;
using System.Collections.Generic;
using SiteFuel.Exchange.Utilities;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class RaiseDrLocationModel
    {
        public List<int> JobIds = new List<int>();
        public List<JobDetail> JobDetails = new List<JobDetail>();
        public List<CarrierJobDetail> CarrierJobDetails = new List<CarrierJobDetail>();
    }

    public class JobDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayJobID { get; set; }
        public string DisplayName { get; set; }
    }

    public class CarrierJobDetail : JobDetail
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
    }

    public class RaiseDrProductAndOrderInfo
    {
        public List<ProductMappingModel> ProductMappings { get; set; } = new List<ProductMappingModel>();
        public List<ProductMappingModel> BlendProductMappings { get; set; } = new List<ProductMappingModel>();
        public List<OrderPickupDetailModel> Orders = new List<OrderPickupDetailModel>();
    }

    public class RaiseDrOrderInfoOfBuyerAndSupplier
    {
        public List<OrderPickupDetailModel> OrderPickupDetails { get; set; } = new List<OrderPickupDetailModel>();
        public List<DemandModel> DeliveryReqInput { get; set; } = new List<DemandModel>();
        public List<DropdownDisplayItem> OrderList = new List<DropdownDisplayItem>();
    }

    public class ProductMappingModel
    {
        public int ProductTypeId { get; set; }
        public int MappedToProductTypeId { get; set; }
    }

    public class CreateDrPreferences
    {
        public bool IsAdditiveBlendingEnabled { get; set; }
        public CreditCheckTypes CreditCheckType { get; set; }
        public bool IsLoadOptimization { get; set; }
    }

    public class RaiseDrJobInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayJobID { get; set; }
        public UoM UoM { get; set; }
        public string TimeZoneName { get; set; }
        public int LocationManagedType { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string StateCode { get; set; }
        public bool IsTankAndAssetAvailableForJob { get; set; }
    }

    public class SubmitDRInput
    {
        public List<SubmitDrJobInput> Jobs = new List<SubmitDrJobInput>();
        public List<SubmitDrOrderInput> Orders = new List<SubmitDrOrderInput>();
        public List<SubmitDrJobAssetInput> Assets = new List<SubmitDrJobAssetInput>();
        public List<SubmitDrVesselInput> Vessels = new List<SubmitDrVesselInput>();
    }

    public class SubmitDrJobInput
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string CustomerCompany { get; set; }
        public UoM UoM { get; set; }
        public string StateCode { get; set; }
        public JobLocationTypes LocationType { get; set; }
        public UoM DefaultUoM { get; set; }
        public string TimeZoneName { get; set; }
        public bool IsMarine { get; set; }
    }

    public class SubmitDrOrderInput
    {
        public int Id { get; set; }
        public string ProductTypeName { get; set; }
        public int ProductTypeId { get; set; }
        public string FuelType { get; set; }
        public int FuelTypeId { get; set; }
        public UoM Uom { get; set; }
        public string Berth { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string ProductCode { get; set; }
    }

    public class SubmitDrJobAssetInput
    {
        public string VehicleId { get; set; } 
        public string Vendor { get; set; }
        public int? FuelType { get; set; }
        public string Name { get; set; }
        public int JobId { get; set; } 
        public int AssetId { get; set; }
    }

    public class SubmitDrVesselInput
    {
        public string Name { get; set; }
        public int OrderId { get; set; }
    }
}
