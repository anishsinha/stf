using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.DataAccess.Entities
{
	public class AccountingWorkflow
	{
		public int Id { get; set; }
		public int Type { get; set; }

		[StringLength(1024)]
		public string ParameterJson { get; set; }
		public int Status { get; set; }
		public int QbCompanyProfileId { get; set; }
		[StringLength(256)]
		public string SoftwareVersion { get; set; }
		public DateTimeOffset CreatedOn { get; set; }
		public DateTimeOffset UpdatedOn { get; set; }

		[ForeignKey("QbCompanyProfileId")]
		public virtual QbCompanyProfile QbCompanyProfile { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<QbRequest> QbRequests { get; set; }

	}
}
