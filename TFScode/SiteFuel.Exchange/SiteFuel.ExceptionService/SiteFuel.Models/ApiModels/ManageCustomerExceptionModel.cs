using SiteFuel.Models.Common;
using SiteFuel.Models.CustomerException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Models.ApiModels
{
    public class ManageCustomerExceptionModel : BaseModel
    {
        public int UserId { get; set; }
        public int OwnerCompanyId { get; set; }
        public int EnabledForCompanyId { get; set; }
        public List<CustomerExceptionModel> Exceptions { get; set; } = new List<CustomerExceptionModel>();
    }
}
