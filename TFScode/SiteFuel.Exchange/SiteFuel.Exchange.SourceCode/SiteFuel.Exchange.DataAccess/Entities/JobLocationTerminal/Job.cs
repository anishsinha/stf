namespace SiteFuel.Exchange.DataAccess.Entities
{
    using SiteFuel.Exchange.Utilities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Job
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Job()
        {
            JobXApprovalUsers = new HashSet<JobXApprovalUser>();
            JobXAssets = new HashSet<JobXAsset>();
            JobXStatuses = new HashSet<JobXStatus>();
            CompanyXAdditionalUserInvites = new HashSet<CompanyXAdditionalUserInvite>();
            FuelRequests = new HashSet<FuelRequest>();
            Users = new HashSet<User>();
            ResaleCustomers = new HashSet<ResaleCustomer>();
            Subcontractors = new HashSet<Subcontractor>();
            Users1 = new HashSet<User>();
            TaxExemptLicenses = new HashSet<TaxExemptLicens>();
            QuoteRequests = new HashSet<QuoteRequest>();
            Currency = Currency.USD;
            UoM = UoM.Gallons;
        }

        public int Id { get; set; }

        [Required, StringLength(256)]
        public string Name { get; set; }

        [StringLength(256)]
        public string DisplayJobID { get; set; }

        [Required]
        public string Address { get; set; }

        [StringLength(512)]
        public string AddressLine2 { get; set; }

        [StringLength(512)]
        public string AddressLine3 { get; set; }

        [Required, StringLength(128)]
        public string City { get; set; }

        public int StateId { get; set; }

        [Required, StringLength(32)]
        public string ZipCode { get; set; }

        public int CountryId { get; set; }

        public bool IsGeocodeUsed { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public decimal HedgeDroppedGallons { get; set; }

        public decimal HedgeDroppedAmount { get; set; }

        public decimal SpotDroppedGallons { get; set; }

        public decimal SpotDroppedAmount { get; set; }

        public Nullable<int> TerminalId { get; set; }

        public bool IsBackdatedJob { get; set; }

        public int CreatedBy { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public DateTimeOffset StartDate { get; set; }

        public Nullable<DateTimeOffset> EndDate { get; set; }

        public bool IsReopened { get; set; }

        public DateTimeOffset ReopenDate { get; set; }

        public Nullable<int> PoContactId { get; set; }

        public bool IsActive { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        public bool IsApprovalWorkflowEnabled { get; set; }

        public bool IsResaleEnabled { get; set; }

        public bool IsProFormaPoEnabled { get; set; }

        public bool IsRetailJob { get; set; }

        [StringLength(256)]
        public string ContractNumber { get; set; }

        [StringLength(256)]
        public string TimeZoneName { get; set; }

        [Required, StringLength(64)]
        public string CountyName { get; set; }

        public int CompanyId { get; set; }

        public int JobBudgetId { get; set; }

        public decimal BaseHedgeDroppedQuantity { get; set; }
        public decimal BaseSpotDroppedQuantity { get; set; }
        public decimal BaseHedgeDroppedAmount { get; set; }
        public decimal BaseSpotDroppedAmount { get; set; }
        public Currency Currency { get; set; }
        public UoM UoM { get; set; }
        public JobLocationTypes LocationType { get; set; }
        public bool SignatureEnabled { get; set; }

        public bool IsBillToEnabled { get; set; }
        [StringLength(128)]
        public string BillToName { get; set; }

        public int? BillingAddressId { get; set; }

        [StringLength(128)]
        public string BillToAddress { get; set; }

        [StringLength(128)]
        public string BillToCity { get; set; }

        [StringLength(32)]
        public string BillToZipCode { get; set; }

        public int? BillToStateId { get; set; }
        public int? BillToCountryId { get; set; }

        [StringLength(16)]
        public string BillToPhone { get; set; }

        [StringLength(128)]
        public string BillToCounty { get; set; }

        [StringLength(128)]
        public string BillToStateName { get; set; }

        [StringLength(128)]
        public string BillToCountryName { get; set; }

        [StringLength(1024)]
        public string SiteInstructions { get; set; }

        public int CreatedByCompanyId { get; set; }

        public int? ParentJobId { get; set; }

        public string ExternalRefID { get; set; }

        [ForeignKey("CountryId")]
        public virtual MstCountry MstCountry { get; set; }

        //[ForeignKey("BillToCountryId")]
        //public virtual MstCountry BillToCountry { get; set; }

        public virtual MstExternalTerminal MstExternalTerminal { get; set; }

        [ForeignKey("StateId")]
        public virtual MstState MstState { get; set; }

        //[ForeignKey("BillToStateId")]
        //public virtual MstState BillToState { get; set; }

        public virtual User User { get; set; }

        public virtual User User1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JobXApprovalUser> JobXApprovalUsers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JobXAsset> JobXAssets { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JobXStatus> JobXStatuses { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompanyXAdditionalUserInvite> CompanyXAdditionalUserInvites { get; set; }

        public virtual Company Company { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FuelRequest> FuelRequests { get; set; }

        public virtual JobBudget JobBudget { get; set; }

        [ForeignKey("BillingAddressId")]
        public virtual BillingAddress BillingAddress { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User> Users { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ResaleCustomer> ResaleCustomers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Subcontractor> Subcontractors { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User> Users1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaxExemptLicens> TaxExemptLicenses { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QuoteRequest> QuoteRequests { get; set; }

        public LocationManagedType LocationManagedType { get; set; }

        public Nullable<LocationInventoryManagedBy> LocationInventoryManagedBy { get; set; }

        public bool IsMarine { get; set; }
        public InventoryDataCaptureType InventoryDataCaptureType { get; set; }

    }
}
