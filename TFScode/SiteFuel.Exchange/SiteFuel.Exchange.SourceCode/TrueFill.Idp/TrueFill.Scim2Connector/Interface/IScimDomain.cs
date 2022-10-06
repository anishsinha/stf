using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TrueFill.SCIM2.Model.Authentication;

namespace TrueFill.SCIM2Service
{
   public interface IScimDomain
    {
        BasicAuth GetBasicAuth();
         List<SCIMUser> GetUsers(int companyId, int count=1);
        void AddNewUser(JsonDocument jsonData);
        UserViewModel GetUserByEmail(string email);

    }
}
