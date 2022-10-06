using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.DataAccess.Entities
{
	public class QbCompanyProfile
	{
		[Key]
		public int CompanyId { get; set; }

		[StringLength(256)]
		public string Username { get; set; }

		[StringLength(1024)]
		public string Password { get; set; }

		[StringLength(1024)]
		public string LoginToken { get; set; }

        [StringLength(256)]
        public string ExpenseAccountName { get; set; }

        [StringLength(256)]
        public string IncomeAccountName { get; set; }

        [StringLength(256)]
        public string DiscountAccountName { get; set; }

        public DateTimeOffset LastAccessedOn { get; set; }

		public DateTimeOffset ProfileUpdatedOn { get; set; }

		public DateTimeOffset ProfileCreatedOn { get; set; }

		public int CreatedBy { get; set; }

		[StringLength(1024)]
		public string QwcXml { get; set; }

		[StringLength(256)]
		public string QbVersion { get; set; }

        [StringLength(256)]
        public string ClassRef { get; set; }

        [StringLength(3)]
        public string ItemPrefix { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; }

        [StringLength(2048)]
        public string CompanyFilePath { get; set; }

        [StringLength(5)]
        public string SyncReportTime { get; set; }

        [StringLength(64)]
        public string ReportTimeZone { get; set; }

        public DateTimeOffset SyncStartDate { get; set; }

        public bool IsSyncEnabled { get; set; }

        public DateTimeOffset? SyncEndDate { get; set; }

        public virtual ICollection<QbPaymentTerm> QbPaymentTerms { get; set; }
    }
}
