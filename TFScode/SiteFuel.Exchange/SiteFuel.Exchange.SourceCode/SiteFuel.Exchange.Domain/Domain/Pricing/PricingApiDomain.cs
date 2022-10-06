using Easy.Common;
using Easy.Common.Interfaces;
using Newtonsoft.Json;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain
{
    public class PricingApiDomain : BaseDomain
    {
        private string _pricingServiceBaseUrl = string.Empty;
        public PricingApiDomain(string connectionString) : base(connectionString)
        {
            InstanceInitialize();
        }

        public PricingApiDomain(BaseDomain domain) : base(domain)
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            var appDomain = new ApplicationDomain(this);
            _pricingServiceBaseUrl = appDomain.GetKeySettingValue("PricingServiceBaseUrl", "");
        }

        protected async Task<T> ApiPostCall<T>(string url, object inputObject)
        {
            T response = default(T);
            if (string.IsNullOrWhiteSpace(_pricingServiceBaseUrl))
                throw new Exception("ApiPostCall: pricingService configuration is missing");

            IDictionary<string, IEnumerable<string>> defaultRequestHeaders = new Dictionary<string, IEnumerable<string>>();
            defaultRequestHeaders.Add(ApplicationConstants.Token, new List<string>() { ApplicationConstants.Token });
            using (IRestClient client = new RestClient(defaultRequestHeaders))
            {
                url = _pricingServiceBaseUrl + url;
                var json = JsonConvert.SerializeObject(inputObject);
                var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                //client.DefaultRequestHeaders.Add(ApplicationConstants.Token, ApplicationConstants.Token);
                HttpResponseMessage apiResponse = await client.PostAsync(url, stringContent);
                if (apiResponse.IsSuccessStatusCode)
                {
                    var responseString = await apiResponse.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<T>(responseString);
                }
            }
            return response;
        }

        protected async Task<T> ApiGetCall<T>(string url, int timeout = 100)
        {
            T response = default(T);
            if (string.IsNullOrWhiteSpace(_pricingServiceBaseUrl))
                throw new Exception("ApiGetCall: pricingService configuration is missing");

            IDictionary<string, IEnumerable<string>> defaultRequestHeaders = new Dictionary<string, IEnumerable<string>>();
            defaultRequestHeaders.Add(ApplicationConstants.Token, new List<string>() { ApplicationConstants.Token });
            using (IRestClient client = new RestClient(defaultRequestHeaders,null,null,true,TimeSpan.FromSeconds(timeout)))
            {
                url = _pricingServiceBaseUrl + url;
                //client.Timeout = TimeSpan.FromSeconds(timeout);
                //client.DefaultRequestHeaders.Add(ApplicationConstants.Token, ApplicationConstants.Token);
                HttpResponseMessage apiResponse = await client.GetAsync(url);
                if (apiResponse.IsSuccessStatusCode)
                {
                    var responseString = await apiResponse.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<T>(responseString);
                }
            }
            return response;
        }
    }
}
