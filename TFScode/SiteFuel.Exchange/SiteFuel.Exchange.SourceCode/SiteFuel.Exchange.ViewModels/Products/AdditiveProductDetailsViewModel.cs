using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class AdditiveProductDetailsViewModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }

        public string AdditiveProductName { get; set; }

        public bool IsDeleted { get; set; }

    }
}
