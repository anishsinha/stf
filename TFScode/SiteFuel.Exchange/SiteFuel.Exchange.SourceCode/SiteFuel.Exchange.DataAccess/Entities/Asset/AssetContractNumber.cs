namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class AssetContractNumber
    {
        public int Id { get; set; }

        public int AssetId { get; set; }

        [StringLength(256)]
        public string ContractNumber { get; set; }

        public int AddedBy { get; set; }

        public DateTimeOffset AddedDate { get; set; }

        public Nullable<int> RemovedBy { get; set; }

        public Nullable<System.DateTimeOffset> RemovedDate { get; set; }

        public bool IsActive { get; set; }

        [ForeignKey("AssetId")]
        public virtual Asset Asset { get; set; }
    }
}
