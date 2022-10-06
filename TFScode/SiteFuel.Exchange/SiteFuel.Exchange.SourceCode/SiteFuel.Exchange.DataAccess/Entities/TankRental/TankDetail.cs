namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class TankDetail
    {
        public int Id { get; set; }

        public int RentalFrequencyId { get; set; }

        public decimal RentalFee { get; set; }

        public string FeeDescription { get; set; }

        public decimal? TaxPercentage { get; set; }

        public decimal? TaxAmount { get; set; }

        public string TaxDescription { get; set; }

        public DateTimeOffset StartDate { get; set; }

        public DateTimeOffset? EndDate { get; set; }

        public DateTimeOffset? DeactivationDate { get; set; }

        public int ActivationStatusId { get; set; }

        public int CreatedBy { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        [ForeignKey("RentalFrequencyId")]
        public virtual FuelRequestTankRentalFrequency RentalFrequency { get; set; }
    }
}