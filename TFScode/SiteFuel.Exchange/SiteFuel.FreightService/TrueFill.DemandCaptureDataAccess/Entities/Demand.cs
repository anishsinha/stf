using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueFill.DemandCaptureDataAccess.Entities
{
	public class Demand
	{
		public long Id { get; set; }
        [StringLength(256)]
		public string SiteId { get; set; }
        [StringLength(256)]
        public string TankId { get; set; }
        [StringLength(256)]
        public string StorageId { get; set; }
		public decimal Level { get; set; }
		public float Ullage { get; set; }
		public float GrossVolume { get; set; }
		public float NetVolume { get; set; }
		public float WaterNetLevel { get; set; }
		public float WaterGrossLevel { get; set; }
		public DateTime CaptureTime { get; set; }
		public string ProductName { get; set; }
		public int DataSourceTypeId { get; set; }
		public int SupplierId { get; set; }
		public long? SourceFileId { get; set; }
		public bool IsProcessed { get; set; }
		[ForeignKey("SourceFileId")]
		public virtual SourceFile SourceFile { get; set; }
        public float DipTestValue { get; set; }
        public TankScaleMeasurement DipTestUoM { get; set; }
        public bool IsActive { get; set; }
    }
}
