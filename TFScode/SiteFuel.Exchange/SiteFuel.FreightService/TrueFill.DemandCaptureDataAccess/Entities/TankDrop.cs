using SiteFuel.Exchange.Utilities;
using System;
using System.ComponentModel.DataAnnotations;

namespace TrueFill.DemandCaptureDataAccess.Entities
{
	public class TankDrop
	{
		public int Id { get; set; }
        [StringLength(256)]
		public string SiteId { get; set; }
        [StringLength(256)]
        public string TankId { get; set; }
        [StringLength(256)]
		public string StorageId { get; set; }
		public int AssetDropId { get; set; }
		public decimal DroppedQuantity { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public bool IsActive { get; set; }
	}
}
