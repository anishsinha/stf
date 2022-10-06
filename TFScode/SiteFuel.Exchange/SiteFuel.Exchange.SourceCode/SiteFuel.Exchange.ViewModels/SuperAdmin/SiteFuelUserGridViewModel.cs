using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class SiteFuelUserGridViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string RoleNames { get; set; }

        public string AddedBy { get; set; }

        public string AddedDate { get; set; }

        public bool IsActive { get; set; }

        public bool IsSalesCalculatorAllowed { get; set; }
    }

}
