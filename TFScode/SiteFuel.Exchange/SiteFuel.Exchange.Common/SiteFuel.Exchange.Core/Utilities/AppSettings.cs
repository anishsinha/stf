using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.Core.Utilities
{
	public static class AppSettings
	{
		static AppSettings()
		{
			GoogleApiKey = ConfigHelperMethods.GetConfigSetting("GoogleMaps.ApiKey");
			GoogleMapApiKey = ConfigHelperMethods.GetConfigSetting("GoogleMaps.MapApiKey");
            BccEmailAddress = ConfigHelperMethods.GetConfigSetting("BccEmailAddress");
            IsEmailBodyLog = ConfigHelperMethods.GetConfigSettingAsBool("IsEmailBodyLog");

            Host = ConfigHelperMethods.GetConfigSetting("SendGridHost");
            Port = ConfigHelperMethods.GetConfigSetting("SendGridPort");
            SmtpServerUserName = ConfigHelperMethods.GetConfigSetting("SendGridUserName");
            SendGridApiKey = ConfigHelperMethods.GetConfigSetting("SendGridApiKey");
            EmailDisplayName = ConfigHelperMethods.GetConfigSetting("EmailDisplayName");
            EmailFromAddress = ConfigHelperMethods.GetConfigSetting("EmailFromAddress");
            SendGridSender = ConfigHelperMethods.GetConfigSettingAsInt("SendGridSender", 0);
            GenerateStaticPassword = ConfigHelperMethods.GetConfigSettingAsBool(ApplicationConstants.GenerateStaticPassword);
            MaxAllowedUploadFileSize = ConfigHelperMethods.GetConfigSettingAsInt(ApplicationConstants.MaxAllowedUploadFileSize);
            AppVersion = ConfigHelperMethods.GetConfigSetting("AppVersion");

            if (string.IsNullOrEmpty(GoogleMapApiKey))
			{
				GoogleMapApiKey = GoogleApiKey;
			}
		}


		public static string GoogleApiKey { get; private set; }

		public static string GoogleMapApiKey { get; set; }

        public static string BccEmailAddress { get; set; }

        public static bool IsEmailBodyLog { get; set; }

        public static string Host { get; set; }

        public static string Port { get; set; }

        public static string SmtpServerUserName { get; set; }

        public static string SendGridApiKey { get; set; }

        public static string EmailDisplayName { get; set; }

        public static string EmailFromAddress { get; set; }

        public static int SendGridSender { get; set; }

        public static bool GenerateStaticPassword { get; set; }

        public static int MaxAllowedUploadFileSize { get; set; }

        public static string AppVersion { get; set; }
    }
}
