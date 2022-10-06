using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class CompanyUserGridViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string RoleNames { get; set; }

        public string CompanyName { get; set; }

        public string AddedBy { get; set; }

        public string AddedDate { get; set; }

        public bool IsSalesCalculatorAllowed { get; set; }

        public bool IsFirstLogin { get; set; }

        public bool IsOnboarded { get; set; }

        public bool IsImpersonated { get; set; }

        public int TotalCount { get; set; }

        public bool IsActive { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public int OnboardedTypeId { get; set; }
    }

}
