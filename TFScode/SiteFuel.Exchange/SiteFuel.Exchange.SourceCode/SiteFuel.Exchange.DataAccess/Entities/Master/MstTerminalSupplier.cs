namespace SiteFuel.Exchange.DataAccess.Entities
{
    using SiteFuel.Exchange.Utilities;
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class MstTerminalSupplier
    {
        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        public string Code { get; set; }

        [Required]
        [StringLength(512)]
        public string Name { get; set; }

        [Required]
        public Country CountryId { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public int AddedBy { get; set; }

        [Required]
        public DateTimeOffset AddedDate { get; set; }

        [Required]
        public int UpdatedBy { get; set; }

        [Required]
        public DateTimeOffset UpdatedDate { get; set; }
    }
}
