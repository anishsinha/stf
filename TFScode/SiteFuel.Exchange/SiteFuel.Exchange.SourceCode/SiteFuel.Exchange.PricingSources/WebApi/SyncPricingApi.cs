using SiteFuel.Exchange.Logger;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace SiteFuel.Exchange.PricingSources.WebApi
{
    public class SyncPricingApi
    {
        public void StartSyncing()
        {
            try
            {
                var WebServiceUrl = ConfigurationManager.AppSettings.Get("WebServiceUrl");
                var ServiceUrls = WebServiceUrl.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var url in ServiceUrls)
                {
                    LogManager.Logger.WriteDebug("SyncPricingApi", "StartSyncing", "calling url : "+ url);
                    CallApi(url);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SyncPricingApi", "StartSyncing", ex.Message, ex);
            }
        }

        private void CallApi(string apiUrl)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(apiUrl))
                {
                    var ex = new Exception("API URL is null or empty.");
                    LogManager.Logger.WriteException("SyncPricingApi", "CallApi", $"URL[{apiUrl}] => " + ex.Message, ex);
                    return;
                }
                LogManager.Logger.WriteInfo("SyncPricingApi", "CallApi", "Calling API: " + apiUrl);
                using (var client = new WebClient()) //WebClient  
                {
                    client.Headers.Add("Content-Type:application/json"); //Content-Type  
                    client.Headers.Add("Accept:application/json");
                    var input = new { token = "6e8c99f7-0357-4d9b-b893-cab826f52f16" };
                    string inputJson = (new JavaScriptSerializer()).Serialize(input);
                    client.UploadString(apiUrl, inputJson); //URI  
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SyncPricingApi", "CallApi", $"URL[{apiUrl}] => " + ex.Message, ex);
            }
        }
    }
}
