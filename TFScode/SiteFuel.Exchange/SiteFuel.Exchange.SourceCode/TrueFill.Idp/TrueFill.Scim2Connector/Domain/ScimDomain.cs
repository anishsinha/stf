using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using TrueFill.SCIM2.Model.Authentication;

namespace TrueFill.SCIM2Service
{
    public class ScimDomain : IScimDomain
    {
        public BasicAuth GetBasicAuth()
        {
            var jsonStr = GetBasicCredentials();
            var json = JsonConvert.DeserializeObject<Auth>(jsonStr);
            return new BasicAuth() { Username = json.Username, Password = json.Password };
        }

        public virtual List<SCIMUser> GetUsers(int companyId, int count = 1)
        {
            var authenticationDomain = ContextFactory.Current.GetDomain<AuthenticationDomain>();
            List<SCIMUser> response = new List<SCIMUser>();

            List<User> users = Task.Run(() => authenticationDomain.GetUsersForOktaAsync(companyId, count)).Result;
            foreach (var item in users)
            {
                response.Add(new SCIMUser(item));
            }
            return response;
        }

        // jsonData will be like : { "RootElement": { "schemas": [ "urn:ietf:params:scim:schemas:core:2.0:User" ],
        // "userName": "anish@truefill.com", "name": { "givenName": "anish", "familyName": "sinha" },
        // "emails": [ { "primary": true, "value": "anish@truefill.com", "type": "work" } ],
        // "displayName": "anish sinha", "locale": "en-US", "externalId": "00u3s8hma9ZnrTBfp5d7", "groups": [],
        // "password": "qb46IkyQ", "active": true } }
        public void AddNewUser(JsonDocument jsonData)
        {
            
            var emailIdOkta = jsonData.RootElement.GetProperty("userName").ToString();
            bool isActiveOkta = bool.Parse(jsonData.RootElement.GetProperty("active").ToString());

            var options = new JsonSerializerOptions { WriteIndented = true };

            SaveDebugInfo(System.Text.Json.JsonSerializer.Serialize(jsonData, options), "AddNewUser");

            var authenticationDomain = ContextFactory.Current.GetDomain<AuthenticationDomain>();
            UserViewModel user = GetUserByEmail(emailIdOkta);

            if (user.StatusCode == AuthStatus.Success)
            {
                if (user.IsActive != isActiveOkta)
                {
                    UpdateUserActiveAsnc(emailIdOkta, isActiveOkta, authenticationDomain);
                }
            }
            else
            {
                if (1 == 2) // Todo : not yet matured , 1. need have have created user id 2. companyId 3. ValidateDriverTrailerType
                {
                    //https://qaexchange.truefill.com/Settings/Profile/AddCompanyUsers
                    AdditionalUsersViewModel request = new AdditionalUsersViewModel();
                    var settingsDomain = ContextFactory.Current.GetDomain<SettingsDomain>();
                    //created or invited by user id 
                    UserViewModel createdByUser = GetCreatedByUser(GetCreatedByUserId(), authenticationDomain);
                    if (createdByUser != null)
                    {
                        request.CompanyId = createdByUser.CompanyId;
                        request.UserId = GetCreatedByUserId();
                        request.SupplierURL = createdByUser.SupplierURL;
                        request.ApplicationTemplateId = createdByUser.ApplicationTemplateId;

                        AdditionalUsersViewModel response = AddCompanyUsers(settingsDomain, request);

                        SaveDebugInfo(System.Text.Json.JsonSerializer.Serialize(response, options), "AddNewUser");

                    }
                }
            }
        }

        private AdditionalUsersViewModel AddCompanyUsers(SettingsDomain settingsDomain, AdditionalUsersViewModel request)
        {
            
            return null /*Task.Run(() => settingsDomain.AddCompanyUsers(request)).Result*/; 

        }

        /// <summary>
        /// Get created or invited by user id or we must have SupplierAdmin/CarrierAdmin/BuyerAdmin 
        /// </summary>
        /// <returns></returns>
        public int GetCreatedByUserId()
        {
            return 1111; //SupplierAdmin / CarrierAdmin / BuyerAdmin id
        }

        public virtual string GetBasicCredentials()
        {
            var helperDomain = ContextFactory.Current.GetDomain<HelperDomain>();
            var idp = helperDomain.GeActiveIdentityProvider();
            if(idp!=null && idp.Count>0)
            {
                return idp.FirstOrDefault().Auth;
            }
            return string.Empty;
           
        }

        private UserViewModel GetCreatedByUser(int createdByUserId, AuthenticationDomain authenticationDomain)
        {
            return Task.Run(() => authenticationDomain.GetUserByIdAsync(createdByUserId)).Result;
        }
        public virtual UserViewModel GetUserByEmail(string email)
        {
            var authenticationDomain = ContextFactory.Current.GetDomain<AuthenticationDomain>();
            return Task.Run(() => authenticationDomain.GetUserByEmailAsync(email)).Result;
        }

        private void UpdateUserActiveAsnc(string email, bool IsActive, AuthenticationDomain authenticationDomain)
        {
            Task.Run(() => authenticationDomain.UpdateUserActiveAsnc(email, IsActive));
        }

        public void SaveDebugInfo<T>(T infoObject, string actionName)
        {
            var json = JsonConvert.SerializeObject(infoObject, Formatting.Indented);

            LogManager.Logger.WriteDebug(this.GetType().ToString(), actionName, json);
        }

    }
}