using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class EBolConfigurationJsonViewModel
    {
        public string FtpUrl { get; set; }
        public string FtpUserName { get; set; }
        public string FtpPassword { get; set; }     
        public string RemoteFileName { get; set; }
        public string TargetFolderName { get; set; }
        public string DestinationFileName { get; set; }
        public bool IsDeleteFtpFile { get; set; }
    }
}
