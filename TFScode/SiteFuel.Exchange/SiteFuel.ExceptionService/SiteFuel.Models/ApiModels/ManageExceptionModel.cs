using SiteFuel.Models.Common;
using SiteFuel.Models.CompanyException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Models.ApiModels
{
    public class ManageExceptionModel : BaseModel
    {
        public int UserId { get; set; }
        public int OwnerCompanyId { get; set; }
        public List<CompanyExceptionModel> Exceptions { get; set; } = new List<CompanyExceptionModel>();
    }
}
