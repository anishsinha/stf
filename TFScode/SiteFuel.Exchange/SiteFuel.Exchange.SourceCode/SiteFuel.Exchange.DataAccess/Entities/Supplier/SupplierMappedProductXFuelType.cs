using System.ComponentModel.DataAnnotations.Schema;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    public partial class SupplierMappedProductXFuelType
    {

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SupplierMappedProductXFuelType()
        {
        }
        public int Id { get; set; }
        public int TfxProductId { get; set; }
        public string BackOfficeProductCode { get; set; }
        public string SeaBoardProductCode { get; set; }
        public int SupplierMappedProductDetailId { get; set; }
        [ForeignKey("SupplierMappedProductDetailId")]
        public virtual SupplierMappedProductDetails SupplierMappedProductDetails { get; set; }
    }
}
