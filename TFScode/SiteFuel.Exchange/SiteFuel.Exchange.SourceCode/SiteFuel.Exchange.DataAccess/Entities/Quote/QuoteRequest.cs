namespace SiteFuel.Exchange.DataAccess.Entities
{
    using SiteFuel.Exchange.Utilities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class QuoteRequest
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public QuoteRequest()
        {
            PrivateSupplierLists = new HashSet<PrivateSupplierList>();
            MstSupplierQualifications = new HashSet<MstSupplierQualification>();
            QuoteRequestStatuses = new HashSet<QuoteRequestStatus>();
            Quotations = new HashSet<Quotation>();
            DeclinedUsers = new HashSet<User>();
            QuoteRequestDocuments = new HashSet<QuoteRequestDocument>();
            Currency = Currency.USD;
            UoM = UoM.Gallons;
        }

        public int Id { get; set; }

        public int JobId { get; set; }

        public int ProductDisplayGroupId { get; set; }

        public int FuelTypeId { get; set; }

        [StringLength(1024)]
        public string FuelDescription { get; set; }

        public int OrderTypeId { get; set; }

        public bool IsPublicRequest { get; set; }

        public int QuotesNeeded { get; set; }

        public DateTimeOffset QuoteDueDate { get; set; }

        public decimal Quantity { get; set; }

        public DateTimeOffset StartDate { get; set; }

        public Nullable<System.DateTimeOffset> EndDate { get; set; }

        public int DeliveryTypeId { get; set; }

        public int EstimatedGallonsPerDelivery { get; set; }       

        public bool IncludeFees { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [StringLength(12)]
        public string RequestNumber { get; set; }

        public string Notes { get; set; }

        public int? ImageId { get; set; }

        public bool IsActive { get; set; }

        public int CreatedBy { get; set; }

        public int PaymentTermId { get; set; }

        public int NetDays { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public int UpdatedBy { get; set; }

        public Currency Currency { get; set; }

        public UoM UoM { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        public virtual MstOrderType MstOrderType { get; set; }

        public virtual MstProduct MstProduct { get; set; }

        [ForeignKey("PaymentTermId")]
        public virtual MstPaymentTerm MstPaymentTerm { get; set; }

        public virtual User User { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PrivateSupplierList> PrivateSupplierLists { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MstSupplierQualification> MstSupplierQualifications { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QuoteRequestStatus> QuoteRequestStatuses { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Quotation> Quotations { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User> DeclinedUsers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QuoteRequestDocument> QuoteRequestDocuments { get; set; }

        [ForeignKey("JobId")]
        public virtual Job Job { get; set; }

        [ForeignKey("ImageId")]
        public virtual Image Image { get; set; }
    }
}
