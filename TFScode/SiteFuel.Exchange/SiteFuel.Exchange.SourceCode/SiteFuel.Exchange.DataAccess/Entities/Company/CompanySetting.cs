namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class CompanySetting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CompanyId { get; set; }

        public int? ProcessingType { get; set; }

        public decimal ProcessingFee { get; set; }

        public int UpdatedBy { get; set; }
                                    
        public DateTimeOffset UpdatedDate { get; set; } = DateTimeOffset.Now;   

        public int CreatedBy { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public virtual Company Company { get; set; }
    }
}
