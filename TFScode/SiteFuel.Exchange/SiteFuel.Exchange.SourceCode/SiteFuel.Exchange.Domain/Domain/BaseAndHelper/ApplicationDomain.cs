using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Core.Logger;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain
{
    public class ApplicationDomain : BaseDomain
    {
        public ApplicationDomain()
            : base(ContextFactory.Current.ConnectionString)
        {
        }

        public ApplicationDomain(BaseDomain domain)
            : base(domain)
        {
        }

        public static ApplicationDomain ApplicationSettings
        {
            get
            {
                return new ApplicationDomain();
            }
        }

        public T GetApplicationSettingValue<T>(string key, T defaultValue = default(T))
        {
                T response = defaultValue;
                try
                {
                    var cacheKey = Constants.ApplicationSettingsValue;
                    var cacheResponse = CacheManager.Get<List<MstAppSetting>>(cacheKey);
                    //First time cache set
                    if (cacheResponse == null)
                    {
                        response = GetKeySettingValue(key, response);
                        SetCacheForAppSettings(cacheKey);
                    }
                    else
                    {
                        var appSettingFromCache = cacheResponse.SingleOrDefault(t => t.Key == key);
                        if (appSettingFromCache != null)
                        {
                            response = (T)Convert.ChangeType(appSettingFromCache.Value, typeof(T));
                        }
                        else
                        {
                            LogManager.Logger.WriteException("ApplicationDomain", "GetApplicationSettingValue", "App setting Key not found: " + key, new Exception());
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("ApplicationDomain", "GetApplicationSettingValue", ex.Message, ex);
                }
                return response;
        }

        public void RemoveCacheForAppSettings(string cacheKey)
        {
            if (CacheManager.IsSet(cacheKey))
                CacheManager.Remove(cacheKey);
        }

        public void SetCacheForAppSettings(string cacheKey)
        {
            if (!CacheManager.IsSet(cacheKey))
            {
                var allSettings = Context.DataContext.MstAppSettings.Where(t => t.IsActive).ToList();
                CacheManager.Set(cacheKey, allSettings, 3600);
            }
        }

        public bool IsItProductionEnvironment()
        {
            var response = false;
            try
            {
                response = Context.DataContext.Database.Connection.Database.Equals(ApplicationConstants.ProductionDatabaseName);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CompanyDomain", "GetAtlasOilReportCsvStream", ex.Message, ex);
            }
            return response;
        }

        public AppSettingResponseViewModel GetAppSetting(AppSettingRequestViewModel request)
        {
            var response = new AppSettingResponseViewModel();
            try
            {
                if (request.Parameters.Any(t => t.Key.Equals($"{ApplicationConstants.KeyAppSettingTargetUrl}", StringComparison.OrdinalIgnoreCase) && t.Value.Equals("true", StringComparison.OrdinalIgnoreCase)))
                {
                    var buildNumber = request.Parameters.FirstOrDefault(t => t.Key.Equals($"{ApplicationConstants.KeyAppSettingBuildNumber}", StringComparison.OrdinalIgnoreCase));
                    var appType = request.Parameters.FirstOrDefault(t => t.Key.Equals($"{ApplicationConstants.KeyAppSettingAppType}", StringComparison.OrdinalIgnoreCase));
                    var osType = request.Parameters.FirstOrDefault(t => t.Key.Equals($"{ApplicationConstants.KeyAppSettingMobileOsType}", StringComparison.OrdinalIgnoreCase));
                    if (!string.IsNullOrWhiteSpace(buildNumber.Value) && !string.IsNullOrWhiteSpace(appType.Value) && !string.IsNullOrWhiteSpace(osType.Value))
                    {
                        var url = GetTargetUrl(buildNumber.Value, appType.Value, osType.Value);
                        response.AppSettings.Add(new KeyValuePair<string, string>($"{ApplicationConstants.KeyAppSettingTargetUrl.ToLower()}", url));
                    }
                }

                if (request.Parameters.Any(t => t.Key.Equals($"{ApplicationConstants.KeyAppSettingApplicationDefaultTimeZone}", StringComparison.OrdinalIgnoreCase) && t.Value.Equals("true", StringComparison.OrdinalIgnoreCase)))
                {
                    var timezone = GetApplicationSettingValue<string>(ApplicationConstants.KeyAppSettingApplicationDefaultTimeZone);
                    response.AppSettings.Add(new KeyValuePair<string, string>($"{ApplicationConstants.KeyAppSettingApplicationDefaultTimeZone.ToLower()}", timezone));

                    var offset = TimeZoneInfo.FindSystemTimeZoneById(timezone);
                    response.AppSettings.Add(new KeyValuePair<string, string>($"{ApplicationConstants.KeyAppSettingApplicationDefaultOffset.ToLower()}", offset.BaseUtcOffset.TotalMinutes.ToString()));
                }

                if (response.AppSettings.Count > 0)
                {
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageSuccess;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ApplicationDomain", "GetAppSetting", ex.Message, ex);
            }
            return response;
        }

        public T GetKeySettingValue<T>(string key, T response)
        {
            var appSetting = Context.DataContext.MstAppSettings.SingleOrDefault(t => t.Key == key);
            if (appSetting != null)
            {
                try
                {
                    response = (T)Convert.ChangeType(appSetting.Value, typeof(T));
                }
                catch
                {
                    // Do not throw exception as we are sending default value back to caller method
                }
            }
            return response;
        }

        private string GetTargetUrl(string currentBuild, string appType, string osType)
        {
            var response = string.Empty;
            try
            {
                if (!string.IsNullOrWhiteSpace(currentBuild))
                {
                    var mobileApp = Enum.Parse(typeof(AppType), appType);
                    var mobileOS = Enum.Parse(typeof(MobileOSType), osType);
                    var kayAppSettingBuildNumber = $"{mobileOS}{mobileApp}{ApplicationConstants.KeyAppSettingBuildNumber}";
                    var productionBuild = GetApplicationSettingValue<string>(kayAppSettingBuildNumber);
                    if (currentBuild == productionBuild)
                    {
                        response = GetApplicationSettingValue<string>(ApplicationConstants.KeyAppSettingProductionApiUrl);
                    }
                    else
                    {
                        response = GetApplicationSettingValue<string>(ApplicationConstants.KeyAppSettingCertificationApiUrl);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ApplicationDomain", "GetTargetUrl", ex.Message, ex);
            }
            return response;
        }
    }
}
