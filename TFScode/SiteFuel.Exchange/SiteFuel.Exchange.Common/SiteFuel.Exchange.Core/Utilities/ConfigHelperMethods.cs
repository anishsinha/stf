using System;
using System.Configuration;

namespace SiteFuel.Exchange.Utilities
{
    public class ConfigHelperMethods
    {
        public static bool GetConfigSettingAsBool(string key, bool defaultValue = false)
        {
            bool result = defaultValue;
            try
            {
                var value = ConfigurationManager.AppSettings[key];
                if (value != null)
                {
                    result = Convert.ToBoolean(value);
                }
            }
            catch (Exception)
            {
            }
            return result;
        }

        public static int GetConfigSettingAsInt(string key, int defaultValue = 0)
        {
            int result = defaultValue;
            try
            {
                var value = ConfigurationManager.AppSettings[key];
                if (value != null)
                {
                    int.TryParse(value, out result); // Convert.ToInt32(value);
                }
            }
            catch (Exception)
            {
            }
            return result;
        }

        public static string GetConfigSetting(string key, string defaultValue = "")
        {
            string result = defaultValue;
            try
            {
                var value = ConfigurationManager.AppSettings[key];
                if (value != null)
                {
                    result = value;
                }
            }
            catch (Exception)
            {
            }
            return result;
        }
    }
}
