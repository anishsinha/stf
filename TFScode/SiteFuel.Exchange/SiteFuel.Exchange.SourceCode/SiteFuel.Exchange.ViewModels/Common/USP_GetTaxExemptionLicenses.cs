using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class USP_GetTaxExemptionLicenses
    {
        public int Id { get; set; }

        public string IDCode { get; set; }

        public string LicenseNumber { get; set; }

        public string BusinessSubType { get; set; }

        public string Status { get; set; }

        public string AddedBy { get; set; }

        public bool IsAssignedToAnyJob { get; set; }
    }
}
