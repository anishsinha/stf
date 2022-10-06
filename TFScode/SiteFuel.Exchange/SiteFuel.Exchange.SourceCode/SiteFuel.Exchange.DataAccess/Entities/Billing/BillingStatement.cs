namespace SiteFuel.Exchange.DataAccess.Entities
{
    using SiteFuel.Exchange.Utilities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class BillingStatement
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BillingStatement()
        {
            Currency = Currency.USD;
            UoM = UoM.Gallons;
            BillingStatementXInvoices = new HashSet<BillingStatementXInvoice>();
        }

        public int Id { get; set; }
        public int StatementNumberId { get; set; }
        public int? BillingScheduleId { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public DateTimeOffset PaymentDueDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public decimal TotalQuantityDropped { get; set; }
        public decimal TotalStatementValue { get; set; }
        public Currency Currency { get; set; }
        public decimal ExchangeRate { get; set; }
        public UoM UoM { get; set; }
        public bool IsActive { get; set; }
        public string StatementChainId { get; set; }
        public int VersionNumber { get; set; }
        public bool IsPaid { get; set; }

        public bool IsGenerated { get; set; }

        public Nullable<int> ParentId { get; set; }
        public int PaymentTermId { get; set; }
        public int PaymentNetDays { get; set; }
        [StringLength(512)]
        public string TimeZoneName { get; set; }
        public int FrequencyTypeId { get; set; }
        public int CreatedCompany { get; set; }
        [ForeignKey("FrequencyTypeId")]
        public virtual MstFrequencyType FrequencyType { get; set; }
        [ForeignKey("BillingScheduleId")]
        public virtual BillingSchedule BillingSchedule { get; set; }

        [ForeignKey("ParentId")]
        public virtual BillingStatement ParentBillingStatement { get; set; }

        [ForeignKey("StatementNumberId")]
        public virtual StatementNumber StatementNumber { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BillingStatementXInvoice> BillingStatementXInvoices { get; set; }

        [ForeignKey("CreatedCompany")]
        public virtual Company CreatedByCompany { get; set; }
    }
}
