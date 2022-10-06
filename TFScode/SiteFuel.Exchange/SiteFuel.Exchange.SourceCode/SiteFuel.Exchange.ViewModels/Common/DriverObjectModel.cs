using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class DriverObjectModel : DriverInfo
    {
        public int DriverId { get; set; }
        public string DriverName { get; set; }
        public string CompanyName { get; set; }
        public string ExpiryDate { get; set; }
        public string LicenseTypeId { get; set; }
        public string ProfilePhoto { get; set; }
        public string LicenseNumber { get; set; }
        public List<string> ShiftId { get; set; }
        public List<TrailerTypeStatus> TrailerType { get; set; }
        public List<TerminalCardNumberViewModel> CardNumbers { get; set; } = new List<TerminalCardNumberViewModel>();
        public DateTimeOffset CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int CompanyId { get; set; }
        public List<string> Regions { get; set; }
        public bool IsFilldAuthorized { get; set; }
        public bool IsScheduleExists { get; set; } = false;
        public List<string> ScheduleBuilderIds = new List<string>();
        public string ScheduleBuilderIdInfo { get { return string.Join(",", ScheduleBuilderIds); } }
    }

    public class DriverInfo
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string ContactNumber { get; set; }

        public string DisplayTrailerTypes { get; set; }

        public string DisplayLicenseType { get; set; }

        public string InvitedBy { get; set; }

        public int UserId { get; set; }
    }

    public class DriverGridViewModel
    {
        public DriverGridViewModel()
        {
            this.InvitedDrivers = new List<DriverObjectModel>();
            this.OnboardedDrivers = new List<DriverObjectModel>();
        }
        public List<DriverObjectModel> InvitedDrivers { get; set; }
        public List<DriverObjectModel> OnboardedDrivers { get; set; }
    }
}
