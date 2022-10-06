namespace SiteFuel.Exchange.DataAccess.Entities
{
    using SiteFuel.Exchange.Utilities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Company
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Company()
        {
            AssetDuplicates = new HashSet<AssetDuplicate>();
            CompanyAddresses = new HashSet<CompanyAddress>();
            CompanyBlacklists = new HashSet<CompanyBlacklist>();
            CompanyBlacklists1 = new HashSet<CompanyBlacklist>();
            CompanyXAdditionalUserInvites = new HashSet<CompanyXAdditionalUserInvite>();
            CompanyXStripeCards = new HashSet<CompanyXStripeCard>();
            CreditAppDetails = new HashSet<CreditAppDetail>();
            CreditAppDocuments = new HashSet<CreditAppDocument>();
            Orders = new HashSet<Order>();
            Orders1 = new HashSet<Order>();
            PrivateSupplierLists = new HashSet<PrivateSupplierList>();
            Assets = new HashSet<Asset>();
            Jobs = new HashSet<Job>();
            Notifications = new HashSet<Notification>();
            PrivateSupplierLists1 = new HashSet<PrivateSupplierList>();
            Users = new HashSet<User>();
            CompanyFavoriteFuels = new HashSet<CompanyFavoriteFuel>();
            TaxExemptLicenses = new HashSet<TaxExemptLicens>();
           // MstProducts = new HashSet<MstProduct>();
            Companies1 = new HashSet<Company>();
            Companies = new HashSet<Company>();
            CompanyXMobileAppThemes = new HashSet<CompanyXMobileAppTheme>();
            BillingAddresses = new HashSet<BillingAddress>();
            OnboardingQuestionAnswers = new HashSet<OnboardingQuestionAnswer>();
            HaulerPricingMatrices = new HashSet<HaulerPricingMatrix>();
            CurrentCosts = new HashSet<CurrentCost>();
            AccountTypeId = 1;
            OfferPricings = new HashSet<OfferPricing>();
			OfferTierMappings = new HashSet<OfferTierMapping>();
			TaxExclusions = new HashSet<TaxExclusion>();
			DirectTaxes = new HashSet<DirectTax>();
            OnboardingPreferences = new HashSet<OnboardingPreference>();
            FleetInformations = new HashSet<FleetInformation>();
            InvitedUsers  = new HashSet<InvitedUser>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        public int CompanyTypeId { get; set; }

        public int CompanySizeId { get; set; }

        public int BusinessTenureId { get; set; }

        public int FuelQuantityId { get; set; }

        public Nullable<int> CompanyLogoId { get; set; }

        public int BudgetAlertPercentage { get; set; }

        public bool IsCreditAppEnabled { get; set; }

        public bool IsActive { get; set; }

        public int CreatedBy { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        public bool IsResaleEnabled { get; set; }

        public bool IsAssetTrackingEnabled { get; set; }

        public bool IsTimeCardEnabled { get; set; }

        [StringLength(50)]
        public string SupplierCode { get; set; }

        [DefaultValue(1)]
        public int AccountTypeId { get; set; }

        [DefaultValue(false)]
        public bool IsAuditApplicable { get; set; }

        public Nullable<int> AccountOwnerId { get; set; }

        public Nullable<int> ParentCompanyId { get; set; }

        public WorkPreference WorkPreference { get; set; }

        [StringLength(512)]
        public string AccouningCompanyId { get; set; }

        public bool IsDeleted { get; set; }

        public bool EnableInvoicePDF { get; set; }

        public bool EnableOrderPDF { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AssetDuplicate> AssetDuplicates { get; set; }

        public virtual Image Image { get; set; }

        //public virtual MstBusinessTenure MstBusinessTenure { get; set; }

        //public virtual MstCompanySize MstCompanySize { get; set; }

        public virtual MstCompanyType MstCompanyType { get; set; }

        //public virtual MstFuelQuantity MstFuelQuantity { get; set; }

        [ForeignKey("AccountTypeId")]
        public virtual MstAccountType MstAccountType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompanyAddress> CompanyAddresses { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompanyBlacklist> CompanyBlacklists { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompanyBlacklist> CompanyBlacklists1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompanyXAdditionalUserInvite> CompanyXAdditionalUserInvites { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompanyXStripeCard> CompanyXStripeCards { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CreditAppDetail> CreditAppDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CreditAppDocument> CreditAppDocuments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PrivateSupplierList> PrivateSupplierLists { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Asset> Assets { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Job> Jobs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Notification> Notifications { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PrivateSupplierList> PrivateSupplierLists1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User> Users { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompanyFavoriteFuel> CompanyFavoriteFuels { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaxExemptLicens> TaxExemptLicenses { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<MstProduct> MstProducts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Company> Companies1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Company> Companies { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompanyXMobileAppTheme> CompanyXMobileAppThemes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BillingAddress> BillingAddresses { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OnboardingQuestionAnswer> OnboardingQuestionAnswers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HaulerPricingMatrix> HaulerPricingMatrices { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CurrentCost> CurrentCosts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OfferPricing> OfferPricings { get; set; }
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompanyGroup> CompanyGroups { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompanyGroupXCompany> SubCompanies { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<OfferTierMapping> OfferTierMappings { get; }

        public virtual CompanySetting CompanySetting { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaxExclusion> TaxExclusions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DirectTax> DirectTaxes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OnboardingPreference> OnboardingPreferences { get; set; }
        public virtual ICollection<FleetInformation> FleetInformations { get; set; }
        public virtual ICollection<InvitedUser> InvitedUsers { get; set; }
    }
}
