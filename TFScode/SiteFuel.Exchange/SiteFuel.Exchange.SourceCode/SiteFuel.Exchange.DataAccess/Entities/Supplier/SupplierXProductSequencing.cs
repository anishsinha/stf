using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    [Table("SupplierXProductSequencing")]
    public partial class SupplierXProductSequencing
    {
        [Key]
        public int Id { get; set; }
        public int SupplierCompanyId { get; set; }
        public int? BuyerCompanyId { get; set; }
        public int? JobId { get; set; }
        public ProductSequencingCreationMethod SequenceCreationMethod { get; set; }
        public int SequenceNumber { get; set; }
        public int? ProductId { get; set; }
        public int? OrderId { get; set; }
        public DateTimeOffset LastModifiedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsActive { get; set; }

        [ForeignKey("SupplierCompanyId")]
        public virtual Company SupplierCompany { get; set; }

        [ForeignKey("BuyerCompanyId")]
        public virtual Company BuyerCompany { get; set; }

        [ForeignKey("ProductId")]
        public MstProductType MstProductType { get; set; }

        [ForeignKey("OrderId")]
        public Order Order { get; set; }
    }
}
