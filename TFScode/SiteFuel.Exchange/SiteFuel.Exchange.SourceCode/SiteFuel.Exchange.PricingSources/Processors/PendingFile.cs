using System;
using System.Configuration;

namespace SiteFuel.Exchange.PricingSources.Processors
{
	public class PendingFile
	{
		static string _basePath = ConfigurationManager.AppSettings.Get("PendingFilePath");

		public int ID { get; set; }
		public string Name { get; set; }
		public string Path { get; set; }

		public string FullPath
		{
			get
			{
				return _basePath + Name;
			}
		}

		public DateTimeOffset DownloadedOn { get; set; }
		public bool IsDownloaded { get; set; }
		public bool IsProcessed { get; set; }
		public bool IsSaved { get; set; }
		public override string ToString()
		{
			return $"{Name}, {Path}, {DownloadedOn}, {IsDownloaded}, {IsProcessed}, {IsSaved}";
		}

		public long Uid { get; set; }
	}
}
