using Newtonsoft.Json.Linq;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.FreightOnlyOrder;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Web;
namespace SiteFuel.Exchange.Domain
{
   public  class APILogger: BaseDomain
    {
        public APILogger() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public APILogger(BaseDomain domain) : base(domain)
        {
        }
        public static void AddAPILog(string token,string apiName,string req,string resp,string ExternalRefID)
        {
            try
            {
                ApiLogViewModel apiLog = new ApiLogViewModel();
                var authDomain = new AuthenticationDomain();
                var apiUserContext = Task.Run(() => authDomain.GetUserContextFromTokenAsync(token)).Result;
                if (apiUserContext != null)
                {
                    apiLog.CreatedBy = apiUserContext.Id;
                    apiLog.Request = req;
                    apiLog.Response = GetObjectFromResponseJSONString(resp);
                    apiLog.Url = apiName;
                    apiLog.CompanyId = apiUserContext.CompanyId;
                    apiLog.ExternalRefID = ExternalRefID;
                    var message = GetStatusFromResponseJSONString(resp)["status"];
                    apiLog.Message = message != null ? message.ToString() : apiLog.Response;
                }
            

                var logDomain = new ExceptionLogDomain();
                logDomain.AddApiLogs(apiLog.ToEntity());
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("APILogger", "AddAPILog", ex.Message, ex);
            }
        }

        private static IDictionary<string, object> GetStatusFromResponseJSONString(string jsonString) {
            dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(jsonString);
            var json_serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
           return (IDictionary<string, object>)json_serializer.DeserializeObject(result);
        }

        private static string GetObjectFromResponseJSONString(string jsonString)
        {
            string result = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(jsonString);
            return result;
        }
    }
}
