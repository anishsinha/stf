using SiteFuel.Exchange.Utilities;
using System;
using System.ComponentModel.DataAnnotations;

namespace TrueFill.DemandCaptureDataAccess.Entities
{
	public class Sale
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
		public DateTimeOffset CreatedDate { get; set; }
		public DateTimeOffset StartUllageDate { get; set; }
		public DateTimeOffset EndUllageDate { get; set; }
	}
}
