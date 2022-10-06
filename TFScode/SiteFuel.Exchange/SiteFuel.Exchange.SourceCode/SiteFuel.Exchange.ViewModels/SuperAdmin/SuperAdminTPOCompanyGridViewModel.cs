using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class SuperAdminTPOCompanyGridViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string AddedBy { get; set; }

        public string SupplierName { get; set; }

        public string AddedDate { get; set; }

        public string InvitationSentDate { get; set; }

        public bool IsOnboardingComplete { get; set; }

        public bool IsDeleted { get; set; }
    }
}
