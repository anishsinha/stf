using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class SuperAdminCompanyGridViewModel
    {
        public int Id { get; set; }

        public string CompanyName { get; set; }

        public string AddedDate { get; set; }

        public string Address { get; set; }

        public bool IsActive { get; set; }

        public bool IsAccountSfxOwned { get; set; }

        public int CompanyTypeId { get; set; }

        public string CompanyType { get; set; }

        public bool IsAuditApplicable { get; set; }

        public int? AccountOwnerId { get; set; }

        public string AddedBy { get; set; }

        public bool? IsOnboardingComplete { get; set; }

        public string OnboardedDate { get; set; }

        public int? UserId { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public int TotalCount { get; set; }
    }

}
