namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class ExternalAssetDropDetails
    {
        public int Id { get; set; }

        public int ThirdPartyId { get; set; }

        public int JobXAssetId { get; set; }
        public int FuelTypeId { get; set; }
        public long StopId { get; set; }
        public decimal DroppedGallons { get; set; }

        public DateTimeOffset DropStartDate { get; set; }

        public DateTimeOffset DropEndDate { get; set; }

        public bool IsActive { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public string Status { get; set; } 

        [ForeignKey("ThirdPartyId")]
        public virtual MstExternalThirdPartyCompanies ExternalThirdPartyCompanies { get; set; }
    }
}
