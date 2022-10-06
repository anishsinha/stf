using SiteFuel.FreightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueFill.DemandCaptureDataAccess.Entities;

namespace SiteFuel.Repository.Mappers
{
	public class DemandCaptureMapper
	{
		public static Demand ToEntity(DemandModel demandModel, long? fileId = null, int supplierId = 0)
		{
			if (demandModel == null)
				return null;
			return new Demand
			{
				CaptureTime = demandModel.CaptureTime,
				DataSourceTypeId = demandModel.DataSourceTypeId,
				GrossVolume = demandModel.GrossVolume,
				Id = demandModel.Id,
				Level = demandModel.Level,
				NetVolume = demandModel.NetVolume,
				ProductName = demandModel.ProductName,
				SiteId = demandModel.SiteId,
				StorageId = demandModel.StorageId,
				TankId = demandModel.TankId,
				Ullage = demandModel.Ullage,
				WaterGrossLevel = demandModel.WaterGrossLevel,
				WaterNetLevel = demandModel.WaterNetLevel,
				SourceFileId = fileId,
				SupplierId = supplierId,
                DipTestValue = demandModel.DipTestValue,
                DipTestUoM = demandModel.DipTestUoM,
				IsActive = true
            };
		}
	}
}
