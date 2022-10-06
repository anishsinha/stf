using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels
{
	public class DemandModel
	{
		public long Id { get; set; }
		public int AssetId { get; set; }
		public string SiteId { get; set; }
		public string TankId { get; set; }
		public string TankName { get; set; }
		public string StorageId { get; set; }
		public decimal Level { get; set; }
		public float Ullage { get; set; }
		public float GrossVolume { get; set; }
		public float NetVolume { get; set; }
		public float WaterNetLevel { get; set; }
		public float WaterGrossLevel { get; set; }
		public DateTime CaptureTime { get; set; }
		public string DisplayCaptureTime { get; set; }
		public string ProductName { get; set; }
		public int DataSourceTypeId { get; set; }
		public bool IsDRExists { get; set; }
		public List<PartialDRDetails> ExistingDR { get; set; } = new List<PartialDRDetails>();
		public decimal TankCapacity { get; set; }
		public decimal TankMinFill { get; set; }
		public decimal TankMaxFill { get; set; }
		public decimal CurrentThreshold { get; set; }
		public int? FillType { get; set; }
		public decimal ReorderPercent { get; set; }
		public decimal ReorderQuantity { get; set; }
		public int JobId { get; set; }
		public int UoM { get; set; }
		public float DipTestValue { get; set; }
		public TankScaleMeasurement DipTestUoM { get; set; }
		public DeliveryReqPriority Priority { get; set; }
		public List<RecurringDRSchdule> RecurringDRScheduleDetails { get; set; } = new List<RecurringDRSchdule>();
		public int CarrierStatus { get; set; }
		public int ProductId { get; set; }
		public string PedigreeAssetDBID { get; set; }
		public int FuelTypeId { get; set; }
		public string SkyBitzRTUID { get; set; }
		public string ExternalTankId { get; set; }
		public string VeederRootIPAddress { get; set; }
		public string Port { get; set; }
	}

	public class CreateDRTankModel
	{
		public List<ProductModelToCreateDR> Tanks { get; set; } = new List<ProductModelToCreateDR>();
		public RegionFavProductModel FavoriteProducts { get; set; }
	}

	public class ProductModelToCreateDR
	{
		public long Id { get; set; }
		public string SiteId { get; set; }
		public string TankId { get; set; }
		public string TankName { get; set; }
		public string StorageId { get; set; }
		public float Ullage { get; set; }
		public float NetVolume { get; set; }
		public string DisplayCaptureTime { get; set; }
		public string ProductName { get; set; }
		public bool IsDRExists { get; set; }
		public List<PartialDRDetails> ExistingDR { get; set; } = new List<PartialDRDetails>();
		public decimal TankCapacity { get; set; }
		public decimal TankMaxFill { get; set; }
		public decimal CurrentThreshold { get; set; }
		public decimal ReorderPercent { get; set; }
		public decimal ReorderQuantity { get; set; }
		public int JobId { get; set; }
		public int UoM { get; set; }
		public DeliveryReqPriority Priority { get; set; }
		public List<RecurringDRSchdule> RecurringDRScheduleDetails { get; set; } = new List<RecurringDRSchdule>();
		public int FuelTypeId { get; set; }
		public int ProductTypeId { get; set; }
		public List<TankDipTestModel> Tanks { get; set; }
	}

	public class TankDipTestModel
	{
		public string TankId { get; set; }
		public string SiteId { get; set; }
		public string TankName { get; set; }
		public string StorageId { get; set; }
		public DeliveryReqPriority Priority { get; set; }
		public float Ullage { get; set; }
		public float NetVolume { get; set; }
		public string DisplayCaptureTime { get; set; }
		public decimal TankCapacity { get; set; }
		public decimal TankMaxFill { get; set; }
		public decimal CurrentThreshold { get; set; }
		public decimal ReorderPercent { get; set; }
		public decimal ReorderQuantity { get; set; }
		public int AssetId { get; set; }
		public float WaterLevel { get; set; }

	}

	public class PartialDRDetails
	{
		public string Id { get; set; }
		public DeliveryReqPriority Priority { get; set; }
		public string ScheduleStatusName { get; set; }
		public int ScheduleStatusId { get; set; }
		public decimal RequiredQuantity { get; set; }
		public string CreatedOn { get; set; }
		public bool IsMissedDr { get; set; }
		public DateTimeOffset CreatedDate { get; set; }
		public bool IsRecurringSchedule { get; set; }
		public int ScheduleQuantityType { get; set; }
		public string ScheduleQuantityTypeName { get; set; }
	}

	public class DemandSalesModel
	{
		public string SiteId { get; set; }
		public string TankId { get; set; }
		public string StorageId { get; set; }
		public DateTime? CaptureTime { get; set; }
		public float? Ullage { get; set; }
		public float? GrossVolume { get; set; }
		public float? NetVolume { get; set; }
		public TankScaleMeasurement DipTestUoM { get; set; }
	}
	public class TankModel
	{
		public string SiteId { get; set; }
		public string TankId { get; set; }
		public string StorageId { get; set; }
	}
}
