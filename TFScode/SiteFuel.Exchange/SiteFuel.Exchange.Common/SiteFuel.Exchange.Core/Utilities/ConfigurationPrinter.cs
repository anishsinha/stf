using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Core.Utilities
{
    public class ConfigurationPrinter
    {
        public void PrintConnectionStrings()
        {
            var connectionStrings = ConfigurationManager.ConnectionStrings;
            if (connectionStrings != null)
            {
                foreach (var connectionString in connectionStrings)
                {
                    Console.WriteLine();
                    Console.WriteLine(connectionString.ToString());
                }
            }
        }

        public void PrintAppSettings()
        {
            var appSettings = ConfigurationManager.AppSettings;
            if (appSettings != null)
            {
                foreach (var appSetting in appSettings.Keys)
                {
                    Console.WriteLine(appSetting.ToString()+":\t" + appSettings[appSetting.ToString()]);
                }
            }
        }
    }
}
