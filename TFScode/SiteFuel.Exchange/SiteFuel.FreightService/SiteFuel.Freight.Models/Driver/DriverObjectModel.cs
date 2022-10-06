using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels.Driver
{
    public class DriverObjectModel : CommonFieldsModel
    {
        public string Id { get; set; }
        public int DriverId { get; set; }
        public string DriverName { get; set; }
        public string CompanyName { get; set; }
        public string ExpiryDate { get; set; }
        public string LicenseTypeId { get; set; }
        public int CompanyId { get; set; }
        public string ProfilePhoto { get; set; }
        public string LicenseNumber { get; set; }
        public List<string> ShiftId { get; set; }
        public List<string> Regions { get; set; }
        public List<TrailerTypeStatus> TrailerType { get; set; }
        public List<TerminalCardNumberModel> CardNumbers { get; set; } = new List<TerminalCardNumberModel>();
        public bool IsFilldAuthorized { get; set; }
    }
}
