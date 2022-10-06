namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class MstApplicationTemplate
    {
        public int Id { get; set; }

        [Required]
        public string BrandedCompanyName { get; set; }

        [Required]
        public string URLName { get; set; }

        [Required]
        public string SenderName { get; set; }

        [Required]
        public string FromEmail { get; set; }

        [Required]
        public string Template { get; set; }

        public string CompanyLogo { get; set; }

        public bool IsActive { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }
    }
}
