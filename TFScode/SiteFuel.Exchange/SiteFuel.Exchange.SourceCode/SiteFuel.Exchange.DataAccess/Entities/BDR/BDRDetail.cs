namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class BDRDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int InvoiceId { get; set; }

        [StringLength(500)]
        public string BDRNumber { get; set; }

        [StringLength(32)]
        public string PumpingStartTime { get; set; }

        [StringLength(32)]
        public string PumpingStopTime { get; set; }

        [StringLength(32)]
        public string OpenMeterReading { get; set; }

        [StringLength(32)]
        public string CloseMeterReading { get; set; }

        [StringLength(32)]
        public string Viscosity { get; set; }

        [StringLength(32)]
        public string SulphurContent { get; set; }

        [StringLength(32)]
        public string FlashPoint { get; set; }

        [StringLength(32)]
        public string DensityInVaccum { get; set; }

        [StringLength(32)]
        public string ObservedTemperature { get; set; }

        [StringLength(32)]
        public string MeasuredVolume { get; set; }

        [StringLength(32)]
        public string StandardVolume { get; set; }

        [StringLength(128)]
        public string MarpolSampleNumbers { get; set; }

        [StringLength(128)]
        public string MVMarpolSampleNumbers { get; set; }

        public bool IsEngineerInvitedToWitnessSample { get; set; }
        public bool IsNoticeToProtestIssued { get; set; }
        public bool IsActive { get; set; }

        [Required]
        public virtual Invoice Invoice { get; set; }
    }
}
