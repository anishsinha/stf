namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            AppLocations = new HashSet<AppLocation>();
            AppMessageXUserStatuses = new HashSet<AppMessageXUserStatus>();
            AssetDrops = new HashSet<AssetDrop>();
            CompanyBlacklists = new HashSet<CompanyBlacklist>();
            CompanyBlacklists1 = new HashSet<CompanyBlacklist>();
            CompanyXAdditionalUserInvites = new HashSet<CompanyXAdditionalUserInvite>();
            CompanyXStripeCards = new HashSet<CompanyXStripeCard>();
            CounterOffers = new HashSet<CounterOffer>();
            CounterOffers1 = new HashSet<CounterOffer>();
            CreditAppDetails = new HashSet<CreditAppDetail>();
            CreditAppDocuments = new HashSet<CreditAppDocument>();
            DeliverySchedules = new HashSet<DeliverySchedule>();
            DeliveryScheduleXDrivers = new HashSet<DeliveryScheduleXDriver>();
            FuelRequests = new HashSet<FuelRequest>();
            Invoices = new HashSet<Invoice>();
            Invoices1 = new HashSet<Invoice>();
            InvoiceXInvoiceStatusDetails = new HashSet<InvoiceXInvoiceStatusDetail>();
            Jobs = new HashSet<Job>();
            Jobs1 = new HashSet<Job>();
            JobXApprovalUsers = new HashSet<JobXApprovalUser>();
            Notifications = new HashSet<Notification>();
            Orders = new HashSet<Order>();
            OrderDeliverySchedule = new HashSet<OrderVersionXDeliverySchedule>();
            OrderXCancelationReasons = new HashSet<OrderXCancelationReason>();
            OrderXDrivers = new HashSet<OrderXDriver>();
            PrivateSupplierLists = new HashSet<PrivateSupplierList>();
            UserCodes = new HashSet<UserCode>();
            UserTokens = new HashSet<UserToken>();
            UserXInvites = new HashSet<UserXInvite>();
            UserXNotificationSettings = new HashSet<UserXNotificationSetting>();
            AppMessages = new HashSet<AppMessage>();
            CompanyAddresses = new HashSet<CompanyAddress>();
            Jobs2 = new HashSet<Job>();
            FuelRequests1 = new HashSet<FuelRequest>();
            Jobs3 = new HashSet<Job>();
            MstRoles = new HashSet<MstRole>();
            PrivateSupplierLists1 = new HashSet<PrivateSupplierList>();
            TimeCardEntries = new HashSet<TimeCardEntry>();
            CompanyFavoriteFuels = new HashSet<CompanyFavoriteFuel>();
            CompanyFavoriteFuels1 = new HashSet<CompanyFavoriteFuel>();
            EnrouteDeliveryHistories = new HashSet<EnrouteDeliveryHistory>();
            OrderXStatuses = new HashSet<OrderXStatus>();
            DeliveryScheduleXTrackableSchedules = new HashSet<DeliveryScheduleXTrackableSchedule>();
            QuoteRequests = new HashSet<QuoteRequest>();
            Quotations = new HashSet<Quotation>();
            DeclinedQuoteRequests = new HashSet<QuoteRequest>();
            UserPageSettings = new HashSet<UserPageSetting>();
            InvitedUsers = new HashSet<InvitedUser>();
            OrderXUsers = new HashSet<Order>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(256)]
        public string LastName { get; set; }

        [Required]
        [StringLength(256)]
        public string UserName { get; set; }

        [Required]
        [StringLength(256)]
        public string Email { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [Required]
        [StringLength(16)]
        public string PhoneNumber { get; set; }

        public bool IsPhoneNumberConfirmed { get; set; }

        public bool IsTwoFactorEnabled { get; set; }

        public int AccessFailedCount { get; set; }

        public bool IsLockoutEnabled { get; set; }

        public Nullable<System.DateTimeOffset> LockoutEndDateUtc { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string SecurityStamp { get; set; }

        [Required]
        public string FingerPrint { get; set; }

        public bool IsOnboardingComplete { get; set; }
		
        public bool IsActive { get; set; }

        public int CreatedBy { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public int UpdatedBy { get; set; }
		
        public System.DateTimeOffset UpdatedDate { get; set; }

        public bool IsFirstLogin { get; set; }

        public bool IsSalesCalculatorAllowed { get; set; }

        public bool IsImpersonated { get; set; }

        public bool IsTaxExemptDisplayed { get; set; }

        public int OnboardedTypeId { get; set; }

        public bool IsEULAAccepted { get; set; }

        public int? CompanyId { get; set; }

        public Nullable<DateTimeOffset> OnboardedDate { get; set; }

        public bool IsApiAccessAllowed { get; set; }

        public bool IsDeleted { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AppLocation> AppLocations { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AppMessageXUserStatus> AppMessageXUserStatuses { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AssetDrop> AssetDrops { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompanyBlacklist> CompanyBlacklists { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompanyBlacklist> CompanyBlacklists1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompanyXAdditionalUserInvite> CompanyXAdditionalUserInvites { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompanyXStripeCard> CompanyXStripeCards { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CounterOffer> CounterOffers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CounterOffer> CounterOffers1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CreditAppDetail> CreditAppDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CreditAppDocument> CreditAppDocuments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliverySchedule> DeliverySchedules { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliveryScheduleXDriver> DeliveryScheduleXDrivers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FuelRequest> FuelRequests { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Invoice> Invoices { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Invoice> Invoices1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FuelRequestDetail> FuelRequestDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvoiceXInvoiceStatusDetail> InvoiceXInvoiceStatusDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Job> Jobs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Job> Jobs1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JobXApprovalUser> JobXApprovalUsers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Notification> Notifications { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderVersionXDeliverySchedule> OrderDeliverySchedule { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderXCancelationReason> OrderXCancelationReasons { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderXDriver> OrderXDrivers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PrivateSupplierList> PrivateSupplierLists { get; set; }
		
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserCode> UserCodes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserToken> UserTokens { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserXInvite> UserXInvites { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserXNotificationSetting> UserXNotificationSettings { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AppMessage> AppMessages { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompanyAddress> CompanyAddresses { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Job> Jobs2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FuelRequest> FuelRequests1 { get; set; }

        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Job> Jobs3 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MstRole> MstRoles { get; set; }
		
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PrivateSupplierList> PrivateSupplierLists1 { get; set; }
		
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TimeCardEntry> TimeCardEntries { get; set; }
		
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompanyFavoriteFuel> CompanyFavoriteFuels { get; set; }
		
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompanyFavoriteFuel> CompanyFavoriteFuels1 { get; set; }
		
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EnrouteDeliveryHistory> EnrouteDeliveryHistories { get; set; }
		
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderXStatus> OrderXStatuses { get; set; }
		
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliveryScheduleXTrackableSchedule> DeliveryScheduleXTrackableSchedules { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QuoteRequest> QuoteRequests { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QuoteRequest> DeclinedQuoteRequests { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Quotation> Quotations { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserPageSetting> UserPageSettings { get; set; }

        public Nullable<DateTimeOffset> LastAccessedDate { get; set; }
        [StringLength(256)]
        public string Title { get; set; }
        public virtual ICollection<InvitedUser> InvitedUsers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> OrderXUsers { get; set; }

    }
}
