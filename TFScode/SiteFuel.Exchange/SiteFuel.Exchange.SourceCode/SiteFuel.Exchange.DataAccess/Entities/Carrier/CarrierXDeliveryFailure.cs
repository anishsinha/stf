using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    public partial class CarrierXDeliveryFailure
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string RequestJson { get; set; }
        public string ResponseJson { get; set; }
        public int RequestType { get; set; }
        public string FailureReason { get; set; }
        public int? BuyerCompanyId { get; set; }
        public int? SupplierCompanyId { get; set; }
        public int EntityId { get; set; }
        public int IsEndSupplier { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        [ForeignKey("BuyerCompanyId")]
        public virtual Company BuyerCompany { get; set; }
        [ForeignKey("SupplierCompanyId")]
        public virtual Company SupplierCompany { get; set; }
    }
}
