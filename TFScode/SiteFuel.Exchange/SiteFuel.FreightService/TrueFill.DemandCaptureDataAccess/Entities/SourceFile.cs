using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueFill.DemandCaptureDataAccess.Entities
{
	public class SourceFile
	{
		public long Id { get; set; }

		public string FileName { get; set; }

		public long Uid { get; set; }
		public int DataSourceType { get; set; }

		public DateTimeOffset CreationDate { get; set; }
	}
}
