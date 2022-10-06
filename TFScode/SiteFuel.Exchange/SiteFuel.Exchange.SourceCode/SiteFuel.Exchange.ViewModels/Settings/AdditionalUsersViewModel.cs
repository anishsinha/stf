using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class AdditionalUsersViewModel : StatusViewModel
    {
        public AdditionalUsersViewModel()
        {
            InstanceInitialize();
        }

        public AdditionalUsersViewModel(Status status)
            : base(status)
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            AdditionalUsers = new List<AdditionalUserViewModel>();
            ApplicationTemplateId = (int)ApplicationTemplate.TrueFill;
        }

        public int UserId { get; set; }

        public int CompanyId { get; set; }

        public int CompanyTypeId { get; set; }

        public string SupplierURL { get; set; }

        public int ApplicationTemplateId { get; set; } 

        public List<AdditionalUserViewModel> AdditionalUsers { get; set; }
    }
}
