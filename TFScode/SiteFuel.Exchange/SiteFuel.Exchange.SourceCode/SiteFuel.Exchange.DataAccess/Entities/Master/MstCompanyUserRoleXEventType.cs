namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MstCompanyUserRoleXEventType
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CompanyTypeId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RoleId { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int EventTypeId { get; set; }

        public int? TemplateId { get; set; }

        public bool IsActive { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        public virtual MstCompanyType MstCompanyType { get; set; }

        public virtual MstNotificationTemplate Template { get; set; }

        public virtual MstEventType MstEventType { get; set; }

        public virtual MstRole MstRole { get; set; }

		public bool IsDefault { get; set; }

        public int NotificationType { get; set; }
    }
}
