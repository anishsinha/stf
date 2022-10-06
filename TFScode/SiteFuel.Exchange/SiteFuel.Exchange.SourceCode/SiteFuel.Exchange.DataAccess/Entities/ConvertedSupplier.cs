namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class ConvertedSupplier
    {
        public int Id { get; set; }

        [StringLength(256)]
        public string SupplierName { get; set; }

        public int Type { get; set; }

        [StringLength(256)]
        public string ContactPerson { get; set; }

        [StringLength(256)]
        public string Address { get; set; }

        [StringLength(256)]
        public string AddedBy { get; set; }

        [StringLength(256)]
        public string ConvertedBy { get; set; }

        public DateTimeOffset DateAdded { get; set; }

        public DateTimeOffset DateConverted { get; set; }

        public virtual MstExternalSupplierType MstExternalSupplierType { get; set; }
    }
}
