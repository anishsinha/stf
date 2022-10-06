using FileHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels
{
	[DelimitedRecord(",")]
	public class DemandCsvModel
	{
		[FieldQuoted]
		public string SiteId { get; set; }
		[FieldQuoted]
		public string TankId { get; set; }
		[FieldQuoted]
		public string StorageId { get; set; }
		
		[FieldQuoted]
		public string CaptureTime { get; set; }
		[FieldQuoted]
		public string ProductName { get; set; }

		[FieldQuoted]
		public string GrossVolume { get; set; }
		[FieldQuoted]
		public string NetVolume { get; set; }

		[FieldQuoted]
		public string Ullage { get; set; }

		[FieldQuoted]
		public string Level { get; set; }

		[FieldQuoted]
		public string Temperature { get; set; }

		[FieldQuoted]
		public string WaterNetLevel { get; set; }
		[FieldQuoted]
		public string WaterGrossLevel { get; set; }
		
	}
}
