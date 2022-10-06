using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class PrivateSupplierListsViewModel : StatusViewModel
    {
        public PrivateSupplierListsViewModel()
        {
            InstanceInitialize();
        }

        public PrivateSupplierListsViewModel(Status status)
            : base(status)
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            PrivateSupplierLists = new List<PrivateSupplierListViewModel>();
        }

        public List<PrivateSupplierListViewModel> PrivateSupplierLists { get; set; }

        public int CompanyId { get; set; }

        public int UserId { get; set; }

        public bool IsAllowDelete { get; set; }
    }
}
