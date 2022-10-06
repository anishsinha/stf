using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
   public class InvoiceGridBuyerDashboardInputModel
    {
        public int UserId { get; set; }
        public int CompanyId { get; set; }
        public bool IsBuyerAdmin { get; set; }
        public int CountryId { get; set; }
        public Currency CurrencyTypeId { get; set; }
        public string GroupIds { get; set; } = "";
        public int InvoiceTypeId { get; set; }
        public int BrandedCompanyId { get; set; }
        
    }
}
