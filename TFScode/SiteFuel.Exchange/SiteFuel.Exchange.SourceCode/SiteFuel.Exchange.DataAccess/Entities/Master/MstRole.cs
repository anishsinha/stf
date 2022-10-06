namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MstRole
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MstRole()
        {
            MstCompanyTypeXRoles = new HashSet<MstCompanyTypeXRole>();
            MstCompanyUserRoleXEventTypes = new HashSet<MstCompanyUserRoleXEventType>();
            MstRoleXInvoiceDeclineReasons = new HashSet<MstRoleXInvoiceDeclineReason>();
            TaxExemptLicenses = new HashSet<TaxExemptLicens>();
            CompanyXAdditionalUserInvites = new HashSet<CompanyXAdditionalUserInvite>();
            Users = new HashSet<User>();
            InvitedUsers = new HashSet<InvitedUser>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        public bool IsActive { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MstCompanyTypeXRole> MstCompanyTypeXRoles { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MstCompanyUserRoleXEventType> MstCompanyUserRoleXEventTypes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MstRoleXInvoiceDeclineReason> MstRoleXInvoiceDeclineReasons { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompanyXAdditionalUserInvite> CompanyXAdditionalUserInvites { get; set; }
		
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User> Users { get; set; }
		
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaxExemptLicens> TaxExemptLicenses { get; set; }
        public virtual ICollection<InvitedUser> InvitedUsers { get; set; }
    }
}
