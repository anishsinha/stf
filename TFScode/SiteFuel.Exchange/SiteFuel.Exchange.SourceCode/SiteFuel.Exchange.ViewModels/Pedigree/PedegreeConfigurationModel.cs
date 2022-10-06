using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class PedegreeConfigurationModel
    {
        public string PedigreeUserId { get; set; }
        public string PedigreePassword { get; set; }
        public string PedigreeGetAssetVolumnUrl { get; set; }
        public string PedigreeGetAssetListUrl { get; set; }
        public List<DropdownDisplayItem> ProductTypeList { get; set; }
        public List<JobUOMDropDwn> JobUOMList { get; set; }

    }

    public class JobUOMDropDwn
    {
        public int Id { get; set; }
        public int UoM { get; set; }
        public string TimeZoneName { get; set; }
    }
    public class ExternalTankConfigurationModel
    {
        public string ConnectionInfo { get; set; }
        
        public List<DropdownDisplayItem> ProductTypeList { get; set; }
        public List<JobUOMDropDwn> JobUOMTimeZoneList { get; set; }

    }
}

