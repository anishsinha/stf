using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class SupplierProductsViewModel : StatusViewModel
    {
        public SupplierProductsViewModel()
        {
            SupplierProducts = new List<SupplierProductViewModel>();
            StatusCode = Status.Success;
        }

        public int Id { get; set; }

        public List<SupplierProductViewModel> SupplierProducts { get; set; }
    }
}
