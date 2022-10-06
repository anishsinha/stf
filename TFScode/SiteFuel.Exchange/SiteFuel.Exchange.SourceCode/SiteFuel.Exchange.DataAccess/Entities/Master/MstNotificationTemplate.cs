namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System.ComponentModel.DataAnnotations;

    public partial class MstNotificationTemplate
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string Subject { get; set; }

        public string Body { get; set; }

        [StringLength(40)]
        public string ButtonText { get; set; }

        public string SmsText { get; set; }

        public string BodyLogo { get; set; }

        public string CompanyText { get; set; }
    }
}
