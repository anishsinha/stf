using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiteFuel.Exchange.DataAccess.Entities
{
   public partial class CarrierCustomerMapping
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int CarrierCustomerId { get; set; }

        public string CarrierAssignedCustomerId { get; set; }

        public int CarrierCompanyId { get; set; }

        public int CreatedBy { get; set; }

        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.Now;

        public bool IsActive { get; set; }

        [ForeignKey("CarrierCustomerId")]
        public virtual Company CustomerCompany { get; set; }

        [ForeignKey("CarrierCompanyId")]
        public virtual Company CarrierCompany { get; set; }
    }
}
