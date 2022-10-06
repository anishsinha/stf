using SiteFuel.Exchange.Utilities;
using System;
using System.ComponentModel.DataAnnotations;

namespace TrueFill.DemandCaptureDataAccess.Entities
{
	public class DailySale
	{
		public long Id { get; set; }
        [StringLength(256)]
		public string SiteId { get; set; }
        [StringLength(256)]
        public string TankId { get; set; }
        [StringLength(256)]
		public string StorageId { get; set; }
		public float StartUllage { get; set; }
		public float EndUllage { get; set; }
		public float NetVolume { get; set; }
		public TankScaleMeasurement UoM { get; set; }
		public decimal DroppedQuantity { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime StartUllageDate { get; set; }
		public DateTime EndUllageDate { get; set; }
	}
}
