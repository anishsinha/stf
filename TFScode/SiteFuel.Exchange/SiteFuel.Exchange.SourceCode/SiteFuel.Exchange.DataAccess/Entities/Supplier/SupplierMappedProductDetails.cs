using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    public partial class SupplierMappedProductDetails
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SupplierMappedProductDetails()
        {
        }

        public int Id { get; set; }
        //public int StateId { get; set; }
        //public int CityId { get; set; }
        [StringLength(500)]
        public string MyProductId { get; set; }
        public int? TerminalId { get; set; }
        public int FuelTypeId { get; set; }
        [StringLength(500)]
        public string BackOfficeProductId { get; set; }
        [StringLength(500)]
        public string DriverProductId { get; set; }
        public int CompanyId { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }
        [ForeignKey("TerminalId")]
        public virtual MstExternalTerminal MstExternalTerminal { get; set; }
        //[StringLength(512)]
        //public string TerminalItemCode { get; set; } 
    }
}
