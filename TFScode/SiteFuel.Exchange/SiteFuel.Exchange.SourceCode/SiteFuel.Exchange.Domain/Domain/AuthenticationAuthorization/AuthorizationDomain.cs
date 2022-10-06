using SiteFuel.Exchange.DataAccess;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain
{
    public class AuthorizationDomain : BaseDomain
    {
        public AuthorizationDomain(BaseDomain domain) : base(domain)
        {
        }
        public AuthorizationDomain(SiteFuelUow SiteFuelDbContext) : base(SiteFuelDbContext)
        {
        }

        public bool CheckAccessOnEntity(UserContext userContext, EntityType entityType, int entityId)
        {
            if (userContext.IsSuperAdmin)
                return true;

            var storProcDomain = new StoredProcedureDomain(this);
            return storProcDomain.CheckAccessOnEntity(userContext.Id, userContext.CompanyId, (int)userContext.ActingCompanyTypeId, (int)entityType, entityId, userContext.Roles[0], 30);
        }
    }
}
