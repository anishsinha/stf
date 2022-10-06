namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class BillingSchedule
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BillingSchedule()
        {
            BillingStatements = new HashSet<BillingStatement>();
            BillingScheduleXCustomerOrders = new HashSet<BillingScheduleXCustomerOrder>();
        }

        public int Id { get; set; }

        [StringLength(256)]
        public string BillingStatementId { get; set; }
        public int FrequencyTypeId { get; set; }
        public int CustomerId { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public int PaymentTermId { get; set; }
        public int PaymentNetDays { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public int CreatedByCompanyId { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public int VersionNumber { get; set; }
        public string ScheduleChainId { get; set; }        
        [StringLength(512)]
        public string TimeZoneName { get; set; }
        public int? WeekDayId { get; set; }
        public int UpdateFrequencyType { get; set; }
        public int UpdateFrequencyValue { get; set; }
        public int CountryId { get; set; }
        public bool IsIncludePreviousStatement { get; set; }

        [ForeignKey("FrequencyTypeId")]
        public virtual MstFrequencyType FrequencyType { get; set; }

        [ForeignKey("PaymentTermId")]
        public virtual MstPaymentTerm PaymentTerm { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Company CustomerCompany { get; set; }

        [ForeignKey("CreatedByCompanyId")]
        public virtual Company CreatedByCompany { get; set; }

        [ForeignKey("WeekDayId")]
        public virtual MstWeekDay WeekDay { get; set; }

        [ForeignKey("CountryId")]
        public virtual MstCountry Country { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BillingStatement> BillingStatements { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BillingScheduleXCustomerOrder> BillingScheduleXCustomerOrders { get; set; }
    }
}
