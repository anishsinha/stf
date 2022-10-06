using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.PricingSources.Processors
{
    public class PendingDirectory
    {
        private readonly string _fileExtension = "*.csv";
        public PendingDirectory()
        {
            var basePath = ConfigurationManager.AppSettings.Get("PendingFilePath");
            Files = Directory.GetFiles(basePath, _fileExtension).ToList();
        }
        public List<string> Files { get; set; }
    }
}
