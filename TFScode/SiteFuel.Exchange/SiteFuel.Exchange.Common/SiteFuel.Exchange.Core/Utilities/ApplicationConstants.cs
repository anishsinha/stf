using System;

namespace SiteFuel.Exchange.Utilities
{
    public static class ApplicationConstants
    {
        public const string ProductionDatabaseName = "SiteFuel-Prod1";

        //Constant keys for resource strings - Application Configuration
        public const string KeyAppSettingSiteFuelExchangeUrl = "SiteFuelExchangeUrl";
        public const string KeyAppSettingTFExchangePdfUrl = "TFExchangePdfUrl";
        public const string KeyAppSettingAccountLockoutTimeSpan = "AccountLockoutTimeSpan";
        public const string KeyAppSettingMaxFailedAccessAttemptsBeforeLockout = "MaxFailedAccessAttemptsBeforeLockout";
        public const string KeyAppSettingCookieExpirationTime = "CookieExpirationTime";
        public const string IdpOkta = "okta";
        public const string IdpReturnUrlAccessDenied = "access_denied";
        public const string IdpReturnUrlGenericerror = "genericerror";
        public const string KeyAppSettingCookieRevalidationTime = "CookieRevalidationTime";
        public const string KeyAppSettingLiveStripePrivateKey = "LiveStripePrivateKey";
        public const string KeyAppSettingLiveStripePublicKey = "LiveStripePublicKey";
        public const string KeyAppSettingTestStripePrivateKey = "TestStripePrivateKey";
        public const string KeyAppSettingTestStripePublicKey = "TestStripePublicKey";
        public const string KeyAppSettingTokenExpiryTime = "TokenExpiryTime";
        public const string KeyAppSettingServiceStatusMailingList = "ServiceStatusMailingList";
        public const string KeyAppSettingDriverAppExactOrderProximity = "DriverAppExactOrderProximity";
        public const string KeyAppSettingDriverAppGetOrdersProximity = "DriverAppGetOrdersProximity";
        public const string KeyAppSettingApplicationEventNotificationTemplate = "ApplicationEventNotificationTemplate";
        public const string KeyAppSettingWindowsServiceStatusNotificationTemplate = "WindowsServiceStatusNotificationTemplate";
        public const string KeyAppSettingContactUsMailingList = "ContactUsMailingList";
        public const string KeyAppSettingSalesMailingList = "SalesMailingList";
        public const string KeyAppSettingCompanyOnboardedMailingList = "ExternalCompanyOnboardedMailingList";
        public const string KeyAppSettingMobileTokenExpiryTime = "MobileTokenExpiryTime";
        public const string KeyAppSettingFCMServerKey = "FCMServerKey";
        public const string KeyAppSettingFCMSenderId = "FCMSenderId";
        public const string KeyAppSettingApplicationUserDefaultPassword = "ApplicationUserDefaultPassword";
        public const string KeyAppSettingProductionApiUrl = "ProductionApiUrl";
        public const string KeyAppSettingCertificationApiUrl = "CertificationApiUrl";
        public const string KeyAppSettingAmpFuelTypes = "AmpFuelTypes";
        public const string KeyAppSettingAmpBrokerName = "AmpBrokerName";
        public const string KeyAppSettingAmpBrokerAddress = "AmpBrokerAddress";
        public const string KeyAppSettingMapsApiKey = "GoogleMaps.ApiKey";
        public const string KeyAppSettingAmpBrokerContactPerson = "AmpBrokerContactPerson";
        public const string KeyAppSettingAmpBrokerContactEmail = "AmpBrokerContactEmail";
        public const string KeyAppSettingProgressReportMailingList = "ProgressReportMailingList";
        public const string KeyAppSettingDemoAccounts = "DemoAccounts";
        public const string KeyAppSettingFRExpirationReminderTime = "FRExpirationReminderTime";
        public const string KeyAppSettingFRExpiryMailingList = "FRExpiryMailingList";
        public const string KeyAppSettingNewIncomingFRMailingList = "NewIncomingFRMailingList";
        public const string KeyAppSettingAxxisMonitoringEmailList = "AxxisMonitoringEmailList";
        public const string KeyPricingDataLastUpdatedDate = "PricingDataLastUpdatedDate";
        public const string KeyAppSettingQueueServiceRunTimePeriod = "QueueServiceFrequencyPeriod";
        public const string KeyAppSettingTankMinFillPercent = "TankMinFillPercent";
        public const string KeyAppSettingTankMaxFillPercent = "TankMaxFillPercent";
        public const string KeyAppSettingTankReorderLevel = "TankReorderLevel";
        public const string KeyAppSettingTankShouldGoLevel = "TankShouldGoLevel";
        public const string KeyAppSettingTankOrderAssignement = "TankOrderAssignement";
        public const string KeyAppSettingDefaultTolerancePercentage = "DefaultTolerancePercentage";
        public const string KeyAppSettingDefaultTankCapacityPercentage = "DefaultTankCapacityPercentage";

        public const string KeyAppSettingIosBuyerMobileApp = "IosBuyerMobileApp";
        public const string KeyAppSettingAndroidBuyerMobileApp = "AndroidBuyerMobileApp";
        public const string KeyAppSettingWetHoseFeeChangedDate = "WetHoseFeeCalculationChangedDate";
        public const string KeyAppSettingAtlasOilReportTime = "AtlasOilReportTime";
        public const string KeyAppSettingAtlasOilReportEmail = "AtlasOilReportEmail";
        public const string KeyAppSettingAtlasOilReportTimeZone = "AtlasOilReportTimeZone";
        public const string KeyAppSettingAtlasOilReportFtpUrl = "AtlasOilReportFtpUrl";
        public const string KeyAppSettingAtlasOilReportFtpUserName = "AtlasOilReportFtpUserName";
        public const string KeyAppSettingAtlasOilReportFtpPassword = "AtlasOilReportFtpPassword";
        public const string KeyAppSettingFTLSupplierDtnDetails = "FTLSupplierDTNDetails";
        public const string MaxAllowedUploadFileSize = "MaxAllowedUploadFileSize";
        public const string KeyAppSettingTelaOrderAddApiSettings = "TelaOrderAddApiSettings";
        public const string KeyAppSettingTelaFuelServiceAddress = "TelaFuelServiceAddress";
        public const string KeyAppSettingTelaFuelExceptionEmail = "TelaFuelServiceExceptionEMail";
        public const string KeyAppSettingLiftValidationStatusApiSettings = "LiftValidationStatusUrl";
        public const string KeyAppSettingParklandBadgeMgmtApiUserId = "ParklandBadgeMgmtApiUserId";
        public const string KeyAppSettingParklandBadgeMgmtApiPass = "ParklandBadgeMgmtApiPass";
        public const string KeyAppSettingParklandxApiKey = "ParklandXApiKey";
        public const string KeyAppSettingLFVResponseToCompanyId = "LFVResponseToCompanyId";
        public const string KeyAppSettingParklandBadgeMgmtApiURL = "ParklandBadgeMgmtApiURL";
        public const string KeyAppSettingParklandFailedPostAPICallRetry = "KeyAppSettingParklandFailedPostAPICallRetry";
        public const string PostAPITimeoutInSeconds = "PostAPITimeoutInSeconds";
        public const string KeyAppSettingLiftFileReportMailingList = "LiftFileRecordsReportMailingList";
        public const string KeyOtherCountryRetainThreshold = "OtherCountryRetainThreshold";
        public const string KeyCanadaRetainThreshold = "CanadaRetainThreshold";
        public const decimal OneLitterEqualsToOneGallon = 0.264M;
        public const decimal OneGallonEqualsToOneLitter = 3.785M;
        public const string KeyRetainManagementException = "Retain Management";


        public const string KeyCarrierInventoryDataExportTimeWindow = "CarrierInventoryDataExportTimeWindow";
        public const string KeyLastInventoryExportEmailDateTime = "LastInventoryExportEmailDateTime";



        //Constants for mobile app setting
        public const string KeyAppSettingBuildNumber = "BuildNumber";
        public const string KeyAppSettingMobileOsType = "OsType";
        public const string KeyAppSettingAppType = "AppType";
        public const string KeyAppSettingTargetUrl = "TargetUrl";
        public const string KeyAppSettingApplicationDefaultTimeZone = "ApplicationDefaultTimeZone";
        public const string KeyAppSettingApplicationDefaultOffset = "ApplicationDefaultOffset";

        //Constants for mobile theme app setting
        public const string KeyMobileAppThemeHeaderBackgroundColor = "HeaderBackgroundColor";
        public const string KeyMobileAppThemeHeaderForeColor = "HeaderForeColor";
        public const string KeyMobileAppThemeFooterBackgroundColor = "FooterBackgroundColor";
        public const string KeyMobileAppThemeFooterForeColor = "FooterForeColor";
        public const string KeyMobileAppThemePrimaryButtonBackgroundColor = "PrimaryButtonBackgroundColor";
        public const string KeyMobileAppThemePrimaryButtonForeColor = "PrimaryButtonForeColor";
        public const string KeyMobileAppThemeSecondaryButtonBackgroundColor = "SecondaryButtonBackgroundColor";
        public const string KeyMobileAppThemeSecondaryButtonForeColor = "SecondaryButtonForeColor";

        public const string KeyAppSettingNotifyDeliveries = "NotificationDeliveryIds";
        public const string KeyAppSettingNotifySchedules = "NotificationScheduleIds";
        public const string KeyAppSettingNotificationPreferencesForFtlOrder = "InvoiceNotificationPreferencesForFtlOrder";
        public const string KeyAppSettingDtnLineItemTypeSettings = "DtnLineItemTypeSettings";
        public const string KeyAppSettingTPDUploadWaitingTimeInHrs = "TPDUploadWaitingTimeInHrs";

        public const string KeyAppSettingBuyerDefaultEnabledNotifications = "BuyerDefaultEnabledNotifications";
        public const string KeyAppSettingSupplierDefaultEnabledNotifications = "SupplierDefaultEnabledNotifications";
        public const string KeyAppSettingCarrierDefaultEnabledNotifications = "CarrierDefaultEnabledNotifications";
        public const string KeyAppSettingDailyDataDumpCSVMailingList = "DailyDataDumpCSVMailingList";

        //Constants for job bulk upload company id
        public const string KeyAppSettingJobAssetBulkUplaodCompany = "JobAssetBulkUplaodCompanyId";
        public const string KeyAppSettingUnspscMapping = "UNSPSC_MAPPING";
        public const string KeyAppSettingTaxServiceEnabled = "TaxServiceEnabled";
        public const string KeyAppSettingShowEditInvoiceMenu = "ShowEditInvoiceMenu";
        public const string KeyAppSettingShowCreditRebillMenu = "ShowCreditRebillMenu";

        public const string KeyAppSettingGoogleFirebaseProjectId = "GoogleFirebaseProjectId";
        public const string KeyAppSettingGoogleServiceAccountJson = "GoogleServiceAccountJson";
        public const string KeyAppSettingGoogleFirebaseCollectionName = "GoogleFirebaseCollectionName";
        public const string KeyAppSettingGoogleFirebasePreLoadBolCollectionName = "GoogleFirebasePreLoadBolCollectionName";
        public const string KeyAppSettingGoogleFirebaseUploadDateTime = "GoogleFirebaseUploadDateTime";
        public const string KeyAppSettingGoogleFirebasePreLoadBolUploadDateTime = "GoogleFirebasePreLoadBolUploadDateTime";

        //for edit pre load bol details
        public const string KeyAppSettingGoogleFirebaseEditedPreLoadBolCollectionName = "GoogleFirebaseEditedPreLoadBolCollectionName";
        public const string KeyAppSettingGoogleFirebaseEditedPreLoadBolSyncDateTime = "GoogleFirebaseEditedPreLoadBolSyncDateTime";

        //for delete pre load bol details
        public const string KeyAppSettingGoogleFirebaseDeletedPreLoadBolCollectionName = "GoogleFirebaseDeletedPreLoadBolCollectionName";
        public const string KeyAppSettingGoogleFirebaseDeletedPreLoadBolSyncDateTime = "GoogleFirebaseDeletedPreLoadBolSyncDateTime";

        //for fuel retain bol details
        public const string KeyAppSettingGoogleFirebaseFuelRetainCollectionName = "GoogleFirebaseFuelRetainCollectionName";
        public const string KeyAppSettingGoogleFirebaseFuelRetainSyncDateTime = "GoogleFirebaseFuelRetainSyncDateTime";

        //for fuel retain bol details
        public const string KeyAppSettingGoogleFirebasePickupBOLRetainCollectionName = "GoogleFirebasePickupBOLRetainCollectionName";
        public const string KeyAppSettingGoogleFirebasePickupBOLRetainSyncDateTime = "GoogleFirebasePickupBOLRetainSyncDateTime";

        public const string KeyAppSettingGoogleFirebaseCanceledScheduleCollectionName = "GoogleFirebaseCanceledScheduleCollectionName";
        public const string KeyAppSettingGoogleFirebaseCanceledScheduleSyncDateTime = "GoogleFirebaseCanceledScheduleSyncDateTime";
        //For PDI Delivery Details
        public const string KeyAppSettingPDIEnterpriseApiSettings = "PDIEnterpriseApiSettings";
        public const string KeyAppSettingPDIEnterpriseServiceAddress = "PDIEnterpriseServiceAddress";
        public const string KeyAppSettingPDIEnterpriseApiIsEnabled = "PDIEnterpriseApiIsEnabled";

        // For MFN DDT conversion
        public const string KeyAppSettingMFNDdtConversionPeriod = "MFNDDTConversionPeriod";
        public const string KeyAppSettingMFNDdtConversionFallbackPeriod = "MFNDDTConversionFallbackPeriod";

        //Avalara app setting
        public const string KeyAppSettingAvalaraCountyName = "CountysToBeExcluded";

        //LFV app Settings 
        public const string KeyAppSettingsLFVInvoiceWindow = "LiftFileValidationInvoiceWindow";

        // Key to show no. of max records on buyer dashboard
        public const string KeyBuyerDashboardRecordsCount = "BuyerDashboardRecordsCount";
        public const string KeyAppSettingEBolConfiguration = "EBolConfiguration";
        public const string UnspscTax = "Tax";
        public const string UnspscFee = "FEE";
        public const string UnspscDryRunFee = "FEE4";
        public const string UnspscOtherFee = "FEE14";
        public static readonly string UNSPSC = "UNSPSC";
        public const string DemurrageFee = "Demurrage";
        public const string KeyAppSettingSkybitzConfiguration = "SkybitzConfiguration";
        public const int SuperAdminCompanyId = 3;

        //Constant keys for string constants - Application
        public const string Area = "area";
        public const string Action = "action";
        public const string Controller = "controller";
        public const string Exception = "exception";
        public const string ReturnUrl = "returnUrl";
        public const string Environment = "Environment";
        public const string AppVersion = "AppVersion";
        public const string TimeZoneOffset = "timezoneoffset";
        public const string LastAccessedDate = "lastaccesseddate";
        public const string CurrentCulture = "currentCulture";
        public const string Token = "token";
        public const string BrandedCompanyId = "brandedCompanyId";

        public const string CustomMessage = "customMessage";
        public const string CustomMessageType = "customMessageType";
        public const string CustomMessageLink = "customMessageLink";
        public const string CustomMessageLinkText = "customMessageLinkText";

        // user dashboard setting
        public const string SupplierDashboard = "supplier-dashboard";
        public const string BuyerDashboard = "buyer-dashboard";
        public const string TileKey = "-tile";
        public const string NewBuyerDashboard = "newbuyer-dashboard";

        //Constant keys for string constants - DB Connection Strings
        public const string DatabaseConnection = "DatabaseConnection";

        //Constant keys for string constants - Web.Config
        public const string GenerateStaticPassword = "GenerateStaticPassword";
        public const string RandomPasswordLength = "RandomPasswordLength";
        public const string CanUseTaxService = "CanUseTaxService";
        public const string AvalaraAuthServiceUrl = "AvalaraAuthServiceUrl";
        public const string AvalaraTaxServiceUrl = "AvalaraTaxServiceUrl";
        public const string AvalaraUserId = "AvalaraUserId";
        public const string AvaFeeTax = "AvaFeeTax";

        //SAP Api constants
        public const string SAPOrderCreationUrl = "SAPOrderCreationUrl";
        public const string SAPDrDetailSendUrl = "SAPDrDetailSendUrl";
        public const string SAPUserId = "SAPUserId";
        public const string SAPPassword = "SAPPassword";
        public const string SAPFailedRequestRetryForCompanies = "SAPFailedRequestRetryForCompanies";
        public const string ReprocessTimeForFailedApiRequests = "ReprocessTimeForFailedApiRequests";
        public const int DefaultReprocessTimeForFailedApiRequests = 15;

        //Constant keys for string constants - Windows Services
        public const string PollingInterval = "PollingInterval";
        public const string ExecutionTime = "ExecutionTime";
        public const string DeliveryAlertExecutionTime = "DeliveryAlertExecutionTime";
        public const string InvoiceApprovalExecutionTime = "InvoiceApprovalExecutionTime";
        public const string ExecutionTimeout = "ExecutionTimeout";
        public const string StoredProcedureName = "StoredProcedureName";
        public const string DefaultMailingList = "DefaultMailingList";
        public const string DefaultPricingDecimalPlaces = "DefaultPricingDecimalPlaces";

        //Constant for helping conversions
        public const double ToMiles = 1609.344;
        public const string CustomerNumberPrefix = "TFCU";
        public const string PoNumberPrefix = "TFPO";
        public const string GroupNumberPrefix = "TFGN";
        public const string ProFormaPoNumberPrefix = "TEMP";
        public const string SevenDigit = "D7";
        public const string SFIN = "TFIN";
        public const string TFBD = "TFBD";
        public const string SFDD = "TFDD";
        public const string SFRB = "TFRB";
        public const string SFCI = "TFCI";
        public const string SFEDD = "TFEDD";
        public const string SFPO = "TFPO";
        public const string SFRQ = "TFRQ";
        public const string SourceRequestPrefix = "TFSR";
        public const string TFXLocationIdPrefix = "TFLO";
        //Constant for helping broker conditions
        public const int TexasStateId = 43;

        //Constant for Date Range Filter
        public const string DateRangeFilterStartDate = "01/01/2016";

        public const string DecimalFormat2 = "N2";
        public const string DecimalFormat4 = "N4";
        public const string IntegerFormat = "N0";
        public const string DecimalFormat6 = "N6";

        public const string DecimalFormat4WOZero = "0.####";

        public const string DecimalFormat6WOZero = "0.######";

        public const int ScheduleWarningHours = 24;

        public const string CalculationType = "SFX";
        public const string SalesTaxDescription = "Sales Tax";
        public const string FederalTaxDescription = "Federal Tax";
        public const string StateTaxDescription = "State Tax";
        public const string TaxExemptionInd = "S";
        public const string UnitOfMeasure = "GLL";
        public const string AvaTaxExemptedInd = "Y";
        public const string PDITaxInd = "N";
        public const string Zero = "0";

        public const string CalculationTypeForNonStandard = "MANUAL";
        public const string RateSubTypeForNonStandard = "NONE";

        public const int DateFilterDefaultDays = -90;
        public const int PreviousDate = -1;
        public const int AfterWeek = 7;
        public const int MessagesDefaultPageSize = 10;
        public const int NewsfeedDefaultPageSize = 10;
        public const int FuelRequestExpiringDefaultHours = 168;
        public const decimal QuantityNotSpecified = 9999999;
        public const int ExportRestrictionCount = 2000;
        public const int SequenceNotDefined = 9999;
        public const string Days = " d(s) ";
        public const string Hours = " hr(s) ";
        public const string Minutes = " min(s) ";
        public const string OneMinute = " 1min ";
        public static readonly string Missed = "Missed";
        public static readonly string Declined = "Declined";
       

        //Newsfeed related constants
        public const string UserName = "[UserName]";
        public const string DealName = "[DealName]";
        public const string CompanyName = "[CompanyName]";
        public const string BuyerCompany = "[BuyerCompany]";
        public const string SupplierCompany = "[SupplierCompany]";
        public const string BrokerCompany = "[BrokerCompany]";
        public const string BrokerUserName = "[BrokerUserName]";
        public const string JobName = "[JobName]";
        public const string FuelRequestNumber = "[FuelRequestNumber]";
        public const string OrderNumber = "[OrderNumber]";
        public const string OrderPercentageDelivered = "[OrderPercentageDelivered]";
        public const string CanceledOrClosedPO = "[CanceledOrClosedPO]";
        public const string CanceledOrderPercentageDelivered = "[CanceledOrderPercentageDelivered]";
        public const string InvoiceNumber = "[InvoiceNumber]";
        public const string OriginalInvoiceNumber = "[OriginalInvoiceNumber]";
        public const string DdtNumber = "[DdtNumber]";
        public const string DriverName = "[DriverName]";
        public const string Gallons = "[Gallons]";
        public const string Date = "[Date]";
        public const string StartTime = "[StartTime]";
        public const string EndTime = "[EndTime]";
        public const string ApprovalPerson = "[ApprovalPerson]";
        public const string BuyerUserName = "[BuyerUsername]";
        public const string SupplierUserName = "[SupplierUsername]";
        public const string NumberOfAssets = "[NumberOfAssets]";
        public const string OtherJobName = "[OtherJobName]";
        public const string PreviousOrderNumber = "[PreviousOrderNumber]";
        public const string CurrentAmount = "[CurrentAmount]";
        public const string NewAmount = "[NewAmount]";
        public const string QuoteRequestNumber = "[RFQ_#]";
        public const string QuotationNumber = "[Quote_#]";
        public const string OriginalDueDate = "[OriginalDueDate]";
        public const string NewDueDate = "[NewDueDate]";
        public const string OriginalNumber = "[OriginalNumber]";
        public const string NewNumber = "[NewNumber]";
        public const string NewOrderVersionNumber = "[NewOrderVersionNumber]";
        public const string AttachmentName = "[Name_Att]";
        public const string Time = "[Time]";
        public const string AccountName = "[AccountName]";
        public const string SubcontractorName = "[SubcontractorName]";
        public const string AssetName = "[AssetName]";
        public const string OfferType = "[OfferType]";
        public const string OfferName = "[OfferName]";
        public const string PreviousSubcontractorName = "[PreviousSubcontractorName]";
        public const string NewSubcontractorName = "[NewSubcontractorName]";
        public const string BulkUploadAssetSubcontractor = "[AccountName] uploaded a bulk document on [Date] at [Time] : ";
        public const string BulkUploadAssetSubcontractorAdd = "[SubcontractorAddCount] subcontractor(s) [Verb] been added";
        public const string BulkUploadAssetSubcontractorUpdate = "[SubcontractorUpdateCount] subcontractor(s) [Verb] been changed";
        public const string BulkUploadAssetSubcontractorDelete = "[SubcontractorDeleteCount] subcontractor(s) [Verb] been removed";
        public const string Add = "ADD";
        public const string Update = "UPDATE";
        public const string Delete = "DELETE";
        public const string Reason = "[Reason]";
        public const string FutureDeliverySchedulesCanceled = "[FutureDeliverySchedulesCanceled]";
        public const string DropPercent = "[DropPercent]";
        public const string QuoteNumber = "[QuoteNumber]";
        public const string PoNumber = "[PoNumber]";
        public const string TargetEntityId = "[TargetEntityId]";
        public const string CurrentValue = "[FieldCurrentValue]";
        public const string UpdatedValue = "[FieldUpdatedValue]";
        public const string Yes = "YES";
        public const string No = "NO";

        public const string DecimalMinValue = "0.00000001";
        public const string DecimalMaxValue = "9999999999.99999999";
        public static readonly int FutureSchedulesAvailableFor = 30;
        public const string SingleDelivery = "Single Delivery";
        public const string MultipleDelivery = "Multiple Delivery";

        public const string OnTotalAmount = "On Total Amount";
        public const string PerGallon = "Per Gallon";
        public const string QbTermDueOnReceipt = "Due on receipt";
        public const string QbTermNet = "Net";

        public const string CsvFeeSubTypeFlatFee = "FlatFee";
        public const string CsvFeeSubTypePerGallon = "PerGallon";
        public const string CsvFeeSubTypePerLitre = "PerLitre";
        public const string CsvFeeSubTypePerAsset = "PerAsset";
        public const string CsvFeeSubTypePerHour = "PerHour";
        public const string CsvFeeSubTypePerRoute = "PerRoute";
        public const string CsvFeeIncludeInPPG = "IncludeInPPG";
        public const string CsvWeekendFee = "Weekend";
        public const string CsvSpecialDateFee = "SpecialDate";
        public const string CsvFeeSubTypeByQuantity = "ByQuantity";

        public const string DefaultLocale = "en-us";
        public const string GallonAbbreviation = "GAL";
        public const string LitreAbbreviation = "LTR";
        public const string ExchangeServiceUrl = "ExchangeServiceUrl";
        public const string ExchangeSvcRefreshDuration = "ExchangeSvcRefreshDuration";

        public const string ExternalTaxingLevelSTA = "STA";
        public const string ExternalTaxRateTypeTAX = "TAX";
        public const string ExternalTaxTypeFUEL = "FUEL";
        public const string ExternalTaxTypeSALESUSE = "SALESUSE";

        public const string ZipValidationRegex = @"^\d{5}(?:[-\s]\d{4})?$|^[A-Za-z]\d[A-Za-z][ ]\d[A-Za-z]\d$";
        public const string ZipValidationUSA = @"^\d{5}(?:[-\s]\d{4})?$";
        public const string ZipValidationCAN = @"^[A-Za-z]\d[A-Za-z][ ]\d[A-Za-z]\d$";

        public const string TenDigitNoHypenPhoneNumberRegex = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";

        public const string FilePathFormat = "https://{0}/{1}/{2}{3}";

        public const string PublicHolidayList = "PublicHolidayList";

        public const string Culture_USA = "en-US";
        public const string Culture_CANADA = "en-CA";

        public const string OfferUpdatePrefix = "TFQU";
        public const string OfferNumberPrefix = "TFOF";
        public const string ContentTypeXml = "text/xml";

        public const string KeyAppSettingZipCodeServiceKey = "ZipCodeServiceKey";

        public const string BillingStatementPrefix = "SFBS";
        public const double StatementGenerationWaitingTime = 15;
        public static readonly DateTimeOffset BillModifySyncStartDate = new DateTimeOffset(2019, 6, 12, 12, 0, 0, new TimeSpan(0, 0, 0));
        public static readonly DateTimeOffset DiscountSyncStartDate = new DateTimeOffset(2019, 7, 25, 8, 0, 0, new TimeSpan(0, 0, 0));
        public const int DateFilterDefaultPast30Days = -30;
        public const int SpecialInstructionsDefaultFileUploadCount = 5;
        public const int SpecialInstructionsFileUploadSizeInKB = 2097152; // 2MB
        public const int SiteImageFileUploadSizeInKB = 1048576; // 1MB

        public const int TFXImageAndPdfAllowedFileUploadSizeInBytes = 5242880; //5 MB

        public const string UrlGetDelayInvoiceCreationTimeByCompany = "Company/GetDelayInvoiceCreationTimeByCompany?companyIds={0}";
        public const string UrlGetCompanyExceptions = "Company/GetExceptions?companyId={0}";
        public const string UrlSaveCompanyExceptions = "Company/SaveExceptions";
        public const string UrlIsExceptionsEnabled = "Company/IsExceptionsEnabled?companyId={0}";
        public const string UrlGetCustomerExceptions = "Customer/GetExceptions?companyId={0}&enabledForCompanyId={1}";
        public const string UrlSaveCustomerExceptions = "Customer/SaveExceptions";
        public const string UrlCheckInvoiceExceptions = "Invoice/CheckExceptions";
        public const string UrlCheckInvoiceApiExceptions = "Invoice/CheckInvoiceApiExceptions";
        public const string UrlGetMyApprovalExceptions = "Exception/GetMyApprovals?approvalCompanyId={0}";
        public const string UrlApproveException = "Exception/ApproveException?exceptionId={0}&resolutionTypeId={1}";
        public const string UrlGetBuyerApprovalExceptions = "Exception/GetBuyerApprovalExceptions?supplierCompanyId={0}&exceptionTypes={1}";
        public const string UrlGetSupplierApprovalExceptions = "Exception/GetSupplierApprovalExceptions?buyerCompanyId={0}&exceptionTypes={1}";
        public const string UrlGetAutoApprovalExceptions = "Exception/GetAutoApprovalExceptions?holidayList={0}&isSaturdayOff={1}";
        public const string UrlGetExceptionsForApproval = "Exception/GetExceptionsForApproval?approvalCompanyId={0}&exceptionTypes={1}";
        public const string UrlIsExceptionsEnabledByType = "Company/IsExceptionEnabledByType?ownerCompanyId={0}&exceptionType={1}";
        public const string UrlRaiseDeliveryMismatchExceptions = "Exception/RaiseDeliveryMismatchExceptions";
        public const string UrlGetRaisedExceptions = "Exception/GetRaisedExceptions?exceptionTypeIds={0}&companyId={1}&isBuyerCompany={2}";

        public const string UrlIsPendingException = "Exception/IsPendingException";
        public static readonly string PricingDataLastUpdatedDate = "PricingDataLastUpdatedDate";
        public static readonly string PricingDataSourcesUpdatedDate = "PricingDataSourcesUpdatedDate";
        public const string KeyAppSettingDefaultSlotPeriod = "DefaultSlotPeriod";
        public static readonly string FeatureSettingStorageKey = "FeatureSetting{0}";
        public static readonly string failedMessageIdentification = "<failed>";
        public static readonly string messageSplitTag = "<split>";
        public const string UrlGetExceptionIdsforAutoRejection = "Exception/GetExceptionIdsforAutoRejection?exceptionTypeId={0}&statusId={1}";
        public const string UrlUpdateExceptionIdForAutoReject = "Exception/UpdateExceptionIdForAutoReject";
        public const string UrlGetCompaniesForEnabledException = "Company/GetCompaniesForEnabledException?exceptionTypeId={0}";

        #region PricingServiceConstants
        public const string UrlGetTerminalPriceAsync = "Pricing/GetTerminalPriceAsync?";
        public const string UrlSyncAxxisPricing = "Pricing/SyncAxxisPricingData";
        public const string UrlGetPricingConfig = "Pricing/GetPricingConfig";
        public const string UrlSyncOpisPlattsPricing = "Pricing/SyncOpisPlattsPricingData";
        public const string UrlIsCityRackPriceAvailable = "Pricing/IsCityRackPriceAvailable?productId={0}&cityGroupTerminalId={1}&pricingSourceId={2}&effectiveDate={3}";
        public const string UrlGetTerminalPricesForSalesCalculatorAsync = "Pricing/GetTerminalPricesForSalesCalculatorAsync";
        public const string UrlGetCityRackTerminalPricesForSalesCalculatorAsync = "Pricing/GetCityRackTerminalPricesAsync";
        public const string UrlGetTerminalPricesForAuditAsync = "Pricing/GetTerminalPricesForAuditAsync";
        public const string UrlGetClosestTerminalsAsync = "Pricing/GetClosestTerminalsAsync";
        public const string UrlGetTerminalsForMultiProductsAsync = "Pricing/GetClosestTerminalsForFueltypesAsync";
        public const string UrlGetProductsInYourAreaAsync = "Pricing/GetAxxisProductDetailsAsync";
        public const string UrlGetLatestTerminalPriceAsync = "Pricing/GetLatestTerminalPriceAsync";
        public const string UrlSaveTerminalDetails = "Pricing/SaveTerminalDetails";
        public const string UrlAddNewProduct = "Pricing/AddNewProduct";
        public const string UrlAddNewTfxProduct = "Pricing/AddNewTfxProduct";
        public const string UrlUpdateTfxProduct = "Pricing/UpdateTfxProduct";
        public const string UrlGetPricingCodes = "PricingRequest/GetPricingCodesAsync";
        public const string UrlGetFilterPriceDetailsByPricingType = "PricingRequest/GetFilterPriceDetailsByPricingType";
        public const string UrlSaveRequestDetails = "PricingRequest/SaveRequestDetails";
        public const string UrlUpdateRequestDetails = "PricingRequest/UpdateRequestDetails";
        public const string UrlUpdateSourceRegion = "PricingRequest/UpdateSourceRegion";
        public const string UrlGetPricingRequestDetailById = "PricingRequest/GetPricingRequestDetailByIdAsync";
        public const string UrlGetFuelPriceAsync = "Pricing/GetFuelPriceAsync?";
        public const string UrlSaveExchangeRates = "Currency/SaveExchangeRateAsync";
        public const string UrlGetLastUpdatedPricingDate = "Pricing/GetLastUpdatedPricingDate?requestPriceDetailId={0}";
        public const string UrlGetPriceDetailIdsBySourceAsync = "PricingRequest/GetPriceDetailIdsBySourceAsync";
        public const string UrlUpdateSupplierCostToPriceDetail = "CurrentCost/UpdateSupplierCostToPriceDetail";
        public const string UrlGetPricingConfigDetails = "Pricing/GetPricingConfigDetails?id={0}";
        public const string UrlEditPricingConfig = "Pricing/EditPricingConfig";
        public const string UrlGetAllTerminalsAsync = "Pricing/GetAllTerminalsAsync";
        public const string UrlSyncActualOpisPricing = "Pricing/SyncActualOpisPricingData";
        public const string UrlSyncDyedProductPricingFromClearProducts = "Pricing/SyncDyedProductPricingFromClearProducts";
        public const string UrlAssignNewTerminalForOrder = "Pricing/AssignNewTerminalForTierPricedOrder?terminalId={0}&requestPriceDetailsId={1}";
        public const string UrlResetCumulation = "Pricing/ResetCumulation";
        public const string UrlUpdateCumulationQuantity = "Pricing/UpdateCumulationQtyPostInvoiceCreate";
        public const string UrlGetPricingDetailsByIdList = "PricingRequest/GetPricingDetailsByIdList";
        public const string UrlGetSourceRegionForCustomers = "Pricing/GetSourceRegionForCustomers";
        #endregion

        #region FreightServiceConstants
        public const string UrlSaveAdditionalJobDetails = "Job/SaveAdditionalJobDetails";
        public const string UrlGetRecurringSchedulesForBuyer = "Job/GetRecurringSchedulesForBuyer?jobIds={0}";
        public const string UrlDeleteJobTanks = "Job/DeleteJobTanks";
        public const string UrlUpdateAdditionalJobDetails = "Job/UpdateAdditionalJobDetails";
        public const string UrlGetAdditionalJobDetails = "Job/GetAdditionalJobDetails?jobId={0}&supplierCompanyId={1}";
        public const string UrlGetRegionByJobAndCompanyId = "Job/getRegionByJobAndCompanyId?jobId={0}&companyId={1}";
        public const string UrlSaveTankDetails = "Tank/SaveTankDetails";
        public const string UrlUpdateTankDetails = "Tank/UpdateTankDetails";
        public const string UrlGetTankDetails = "Tank/GetTankDetails?assetId={0}";
        public const string UrlGetTankDetailsBySchedule = "Tank/GetTankDetailsBySchedule?scheduleInputDetails={0}";
        public const string UrlGetTankList = "Tank/GetTankList?tankId={0}";
        public const string UrlGetJobsAssignedToDriver = "Region/GetJobsAssignedToDriver?driverId={0}";
        public const string UrlGetJobTankList = "Tank/GetJobTankList?jobId={0}";
        public const string UrlGetTankVolumeAndUllage = "Tank/GetTankVolumeAndUllage";
        public const string UrlGetDropQuantityByPrePostDip = "Tank/GetDropQuantityByPrePostDip";
        public const string UrlRegionCreate = "Region/Create";
        public const string UrlSaveTanktypes = "Tank/SaveTankTypes";
        public const string UrlGetTankTypeDetails = "Tank/GetTankTypesByCompany?companyId={0}";
        public const string UrlGetTankTypes = "Tank/GetTankModelType?companyId={0}";
        public const string UrlDeleteTankDipChartById = "Tank/DeleteTankDipChartById?id={0}";
        public const string UrlGetAllTankTypeNameForDipChart = "Tank/GetAllTankTypeNameForDipChart?companyId={0}&searchValue={1}";
        public const string UrlRegionUpdate = "Region/Update";
        public const string UrlRegionDelete = "Region/Delete?regionId={0}&deletedBy={1}";
        public const string UrlRegionGet = "Region/GetRegion";
        public const string UrlGetRegions = "Region/GetRegions?companyId={0}";
        public const string UrlGetRegionName = "Region/GetRegionName?regionId={0}";
        public const string UrlRegionsGetShiftDdl = "Region/GetShiftDdl?companyId={0}";
        public const string UrlDriverCreate = "Driver/Create";
        public const string UrlDriverUpdate = "Driver/Update";
        public const string UrlDriverDelete = "Driver/Delete?driverId={0}&companyId={1}";
        public const string UrlDriverGet = "Driver/Get?driverId={0}&companyId={1}";
        public const string UrlGetDriverById = "Driver/GetDriverById?driverId={0}";
        public const string UrlDriverGetAdditionalDetails = "Driver/GetGetDriverAdditionalDetails?driverId={0}";
        public const string UrlSupplierCarriersGet = "Carrier/GetCarriers?companyId={0}&carrierCompanyId={1}";
        public const string UrlAssignedCarriersGet = "Carrier/GetAssignedCarriers?companyId={0}";
        public const string UrlAssignCarriersPost = "Carrier/AssignToSupplier";
        public const string UrlUpdateAssignedCarriersPost = "Carrier/UpdateAssignedCarriers";
        public const string UrlDeleteAssignedCarriersPost = "Carrier/DeleteAssignedCarriers";
        public const string UrlSaveTruckDetails = "Truck/SaveTruckDetails";
        public const string UrlUpdateTruckDetails = "Truck/UpdatetruckDetails";
        public const string UrlGetTruckRegionDetails = "Tank/GetTruckRegionDetails?truckId={0}";
        public const string UrlGetVehiclesForExternalMapping = "Truck/GetVehiclesForExternalMapping?companyId={0}";
        public const string UrlSaveExternalVehicleMapping = "Truck/SaveExternalVehicleMapping";
        public const string UrlSaveBulkUploadVehicleMapping = "Truck/SaveBulkUploadVehicleMapping";
        public const string UrlRaiseDeliveryRequests = "DeliveryRequest/Create";
        public const string UrlUpdateHeldDrValidation = "HeldRequest/UpdateHeldDrValidation?id={0}&message={1}";
        public const string UrlRaiseHeldDeliveryRequests = "HeldRequest/CreateHeldDeliveryRequests";
        public const string UrlEditHeldDr = "HeldRequest/EditHeldDeliveryRequest";
        public const string UrlGetHeldDeliveryRequestCount = "HeldRequest/GetHeldDeliveryRequestCount?companyId={0}";
        public const string UrlGetHeldDeliveryRequests = "HeldRequest/GetHeldDeliveryRequests?companyId={0}";
        public const string UrlGetHeldDeliveryRequestById = "HeldRequest/GetHeldDeliveryRequestById?id={0}";
        public const string UrlUpdateHeldDrCreditCheckStatus = "HeldRequest/UpdateHeldDrCreditCheckStatus";
        public const string UrlOverrideCreditCheckApproval = "HeldRequest/OverrideCreditCheckApproval";
        public const string UrlDeleteHeldDr = "HeldRequest/DeleteHeldDr?id={0}&userId={1}";
        public const string UrlUpdateHeldDrStatus = "HeldRequest/UpdateHeldDrStatus?id={0}";
        public const string UrlRaiseDeliveryRequestsFromBuyerApp = "DeliveryRequest/CreateFromBuyerApp";
        public const string UrlReCreateDeliveryRequests = "DeliveryRequest/ReCreate";
        public const string UrlValidateDeliveryRequestInUse = "DeliveryRequest/ValidateDeliveryRequestInUse";
        public const string UrlGetBlendedGroupDeliveryRequestDetails = "DeliveryRequest/GetBlendedGroupDeliveryRequestDetails";
        public const string UrlGetBlendedChildDeliveryRequestInfo = "DeliveryRequest/GetBlendedChildDeliveryRequestInfo?blendedGroupId={0}";
        public const string UrlUpdateDeliveryRequest = "DeliveryRequest/Update";
        public const string UrlChangeBrokeredDrStatus = "DeliveryRequest/ChangeBrokeredDrStatus?drId={0}&status={1}";
        public const string UrlCreateOrderTankMapping = "Tank/UpdateOrderTankMapping";
        public const string UrlGetDeliveryRequests = "DeliveryRequest/GetDeliveryRequests?companyId={0}&regionId={1}&selectedDate={2}";
        public const string UrlGetCalendarDeliveryRequests = "DeliveryRequest/GetCalendarDeliveryRequests?companyId={0}";
        public const string UrlGetBrokeredDrRequestedToMe = "DeliveryRequest/GetBrokeredDrRequestedToMe?companyId={0}&regionId={1}&selectedDate={2}";
        public const string UrlGetBrokeredDrRequestedByMe = "DeliveryRequest/GetBrokeredDrRequestedByMe?companyId={0}&regionId={1}&selectedDate={2}";
        public const string UrlGetDeliveryRequestById = "DeliveryRequest/GetDeliveryRequestById?deliveryRequestId={0}";
        public const string UrlGetDipTest = "DemandCapture/GetDemands";
        public const string UrlGetSiteList = "DemandCapture/GetSites?companyId={0}&regionId={1}";
        public const string UrlGetJobListForCarrier = "DemandCapture/GetJobListForCarrier?companyId={0}&regionId={1}";
        public const string UrlGetSiteCustomers = "DemandCapture/GetSiteCustomers?companyId={0}&regionId={1}";
        public const string UrlGetAllTruckDetails = "Truck/GetAllTruckDetails?companyId={0}";
        public const string UrlGetAllTruckRetainFuelDetails = "Truck/GetAllTruckRetainFuelDetails?companyId={0}";
        public const string UrlResetFuelRetainDetails = "TrailerFuelRetain/ResetFuelRetainDetails?truckId={1}";
        public const string UrlUpdateRetainFuelDetails = "TrailerFuelRetain/UpdateRetainFuelDetails";
        public const string UrlConfirmRetainFuelException = "TrailerFuelRetain/ConfirmTrailerFuelRetainedException";
        public const string UrlGetTruckDetails = "Truck/GetTruckDetails?truckId={0}";
        public const string UrlDeleteTruck = "Truck/DeleteTruck";
        public const string UrlGetRegionsForDispatcher = "ScheduleBuilder/GetRegions?userId={0}";
        public const string UrlCheckAndLockDrs = "ScheduleBuilder/CheckAndLockDrs";
        public const string UrlCheckAndReleaseDrs = "ScheduleBuilder/CheckAndReleaseDrs";
        public const string UrlGetRegionDetails = "ScheduleBuilder/GetRegionDetails?regionId={0}";
        public const string UrlGetSupplierCompanyList = "ScheduleBuilder/GetCarrierSuppliersBySiteId?jobId={0}&carrierCompanyId={1}";
        public const string UrlGetSheduleBuilderDetails = "ScheduleBuilder/GetSheduleBuilderDetails?companyId={0}&userId={1}&regionId={2}&date={3}&sbView={4}&scheduleBuilderId={5}&sbDsbView={6}&IsDsbDriverSchedule={7}";
        public const string UrlSaveSheduleBuilder = "ScheduleBuilder/SaveScheduleBuilder";
        public const string UrlAssignDriverAndTrailer = "ScheduleBuilder/AssignDriverAndTrailer";
        public const string UrlCancelSchedules = "ScheduleBuilder/CancelSchedules";
        public const string UrlGetCreateScheduleInput = "ScheduleBuilder/GetLoads";
        public const string UrlCreateSchedules = "ScheduleBuilder/CreateSchedules";
        public const string UrlCreateTankDipTest = "DemandCapture/CreateTankDipTest";
        public const string UrlGetLastUpdatedTimeStamp = "ScheduleBuilder/IsValidTimeStamp?selectedDate={0}&regionid={1}&companyId={2}&lastTimeStamp={3}";
        public const string UrlGetDeliveryRequestsbyPriority = "DeliveryRequest/GetDeliveryRequestsbyPriority?priority={0}&companyId={1}";
        public const string UrlUpdateDeliveryRequestStatus = "DeliveryRequest/UpdateDeliveryRequestStatus?drId={0}&statusId={1}&userId={2}";
        public const string UrlDeleteDeliveryRequests = "DeliveryRequest/DeleteDeliveryRequest";
        public const string UrlUpdateDeliveryRequestStatusByTrackableScheduleId= "DeliveryRequest/UpdateDeliveryRequestStatusByTrackableScheduleId";
        public const string UrlRemoveScheduleBuilderDrs = "ScheduleBuilder/RemoveScheduleBuilderDrs";
        public const string UrlRemoveDeliverySchedule = "ScheduleBuilder/RemoveDeliverySchedule";
        public const string UrlUpdateDeliveryRequestCompartmentInfo = "DeliveryRequest/UpdateDeliveryRequestCompartmentInfo?drId={0}";
        public const string UrlSaveTractorDetails = "Tractor/SaveTractorDetails";
        public const string UrlUpdateTractorDetails = "Tractor/UpdateTractorDetails";
        public const string UrlDeleteTractor = "Tractor/DeleteTractor";
        public const string UrlGetAllTractorDetails = "Tractor/GetAllTractorDetails?companyId={0}";
        public const string UrlValidateScheduleBuilder = "ScheduleBuilder/IsValidScheduleBuilder";
        //public const string UrlUpdateDeletedRequests = "ScheduleBuilder/UpdateDeletedRequests";
        public const string UrlDeleteTrip = "ScheduleBuilder/DeleteTrip";
        public const string UrlGetAllDrivers = "Tractor/GetAllDrivers?companyId={0}&trailerTypeId={1}";
        public const string UrlGetAllDriversScheduleBuilder = "ScheduleBuilder/GetAllDrivers";
        public const string UrlGetAllDriverDetailsScheduleBuilder = "ScheduleBuilder/GetAllDriverDetails";
        public const string UrlGetAllShiftDriverDetailsScheduleBuilder = "ScheduleBuilder/GetShiftDriverDetails";
        public const string UrlGetDemandCaptureChartData = "DeliveryRequest/GetDemandCaptureChartdata?SiteId={0}&noOfDays={1}&tfxJobId={2}&companyId={3}";
        public const string UrlGetDemandCaptureChartDataByTankAndSite = "DemandCapture/GetDemandCaptureChartDataByTankAndSite";
        public const string UrlGetJobLocationRelatedDetails = "Job/GetJobLocationRelatedDetails";
        public const string UrlGetDipTestDetails = "Job/GetDipTestDetails?siteID={0}&TankId={1}&noOfDays={2}";
        public const string UrlJOBGetDemandCaptureChartData = "Job/GetDemandCaptureChartData";
        public const string UrlGetDeliveryRequestDetailsByIds = "DeliveryRequest/GetDeliveryRequestDetailsByIds";
        public const string UrlGetDeliveryRequestDetailsById = "DeliveryRequest/GetDeliveryRequestDetailsById?deliveryRequestId={0}";
        public const string UrlGetDispatcherDashboardRegions = "Region/GetDispatcherDashboardRegions?companyId={0}&dispatcherId={1}";
        public const string UrlGetDispatcherRegionIds = "Region/GetDispatcherRegionIds?companyId={0}&dispatcherId={1}";
        public const string UrlGetDipatchersRegionDetails = "DeliveryRequest/GetRegionDispactherDetails?driverId={0}&companyId={1}&regionId={2}";
        public const string UrlGetDriverDetailsByCompany = "Driver/GetAllDrivers?companyId={0}";
        public const string UrlGetTrailerRetainDetailsByDriverIds = "Driver/GetTrailerRetainDetailsByDriverIds";
        public const string UrlSaveJobRegionCarrierDetails = "Job/SaveJobRegionCarrierDetails";
        public const string UrlUpdateDistanceCoveredOfAdditionalJobDetails = "Job/UpdateDistanceCoveredOfAdditionalJobDetails";
        public const string UrlAddRegionSchedule = "Region/AddRegionSchedule";
        public const string UrlGetTbdDeliveryRequestDetails = "DeliveryRequest/GetTbdDeliveryRequestDetails";
        public const string UrlGetRegionShiftSchedule = "Region/getRegionShiftSchedule?regionId={0}&routeId={1}";
        public const string UrlGetRegionShiftScheduleByRegionId = "Region/getRegionShiftSchedule?regionId={0}&scheduleType={1}";
        public const string UrlGetClosestTerminalsByDistanceAsync = "Pricing/GetClosestTerminalsByDistanceAsync";
        public const string UrlGetShiftByDrivers = "Driver/GetShiftByDrivers?driverList={0}&scheduleType={1}";

        public const string UrlGetDriverDetailsByCompanyId = "Region/GetDriverDetailsByCompanyId?companyId={0}&&dispatcherId={1}&&regionID={2}";
        public const string UrlAssignJobToRegionPost = "Region/AssignTPOJobToRegion";
        public const string UrlGetRegionIdForJob = "Region/GetRegionIdForJob?jobId={0}&companyId={1}";
        public const string UrlGetJobsForAllRegions = "Region/GetJobsForAllRegions?companyId={0}";
        public const string UrlGetPriorityForSalesDR = "DeliveryRequest/GetPriorityForSalesDR";
        public const string UrlAddDriverSchedule = "Driver/AddDriverSchedule";
        public const string UrlUpdateDriverSchedule = "Driver/UpdateDriverSchedule";
        public const string UrlDeleteDriverSchedule = "Driver/DeleteAllSchedulesOfDriver";
        public const string UrlAddTrailerSchedule = "Driver/AddTrailerSchedule";
        public const string UrlGetRegionsDdl = "Region/GetRegionsDdl?companyId={0}";
        public const string UrlGetCarriersAssignedToRegion = "Region/GetCarriersAssignedToRegion?regionId={0}";
        public const string UrlGetDispatchersAssignedToRegion = "Region/GetDispatchersAssignedToRegion";

        public const string UrlGetJobsAssignedToRegions = "Region/GetJobsAssignedToRegions?companyId={0}";
        public const string UrlGetSelectedDateDriverScheduleByDriverId = "ScheduleBuilder/GetSelectedDateDriverScheduleByDriverId?driverId={0}&selectedDate={1}";
        public const string UrlGetSelectedDateDriverScheduleByDriverIdGridView = "ScheduleBuilder/GetSelectedDateDriverScheduleByDriverIdGridView?driverId={0}&selectedDate={1}&shiftId={2}";
        public const string UrlValidateTrailerJobCompatibility = "ScheduleBuilder/ValidateTrailerJobCompatibility";
        public const string UrlValidateTrailerJobCompatibilityForLoadQueue = "ScheduleBuilder/ValidateTrailerJobCompatibilityForLoadQueue";
        public const string UrlSaveTPOImageDetails = "Order/SaveTPOImageDetails";
        public const string UrlGetTPOImageDetails = "Order/GetTPOImageDetails?orderId={0}";
        public const string UrlUpdateTPOImageDetails = "Order/UpdateTPOImageDetails";
        public const string UrlUpdateTankSequence = "Tank/UpdateTankSequence";
        public const string UrlCheckDuplicateTankSequence = "Tank/CheckDuplicateTankSequence";
        public const string UrlGetRegionsForJobs = "Region/GetRegionsForJobs";
        public const string UrlCloneDrsForPreload = "DeliveryRequest/CloneDrsForPreload";
        public const string UrlGetJobDetailsForSupplier = "Job/GetJobDetailsForSupplier";
        public const string UrlAssignCarrierToJob = "Carrier/AssignCarrierToJob";
        public const string UrlGetCarrierForJobs = "Carrier/GetAssignedCarrierForJobs?companyId={0}";
        public const string UrlGetCarriersJobs = "Carrier/GetCarriersJobs?carrierCompanyId={0}&customerCompanyId={1}";
        public const string UrlGetCarrierDetailsByJob = "Carrier/GetCarrierDetailsByJob";
        public const string UrlGetScheduleBuildersByDrIds = "ScheduleBuilder/GetScheduleBuildersByDrIds";
        public const string UrlSaveDrFilterPreferences = "ScheduleBuilder/SaveDrFilterPreferences";
        public const string UrlGetDrFilterPreferences = "ScheduleBuilder/GetDrFilterPreferences?userId={0}";

        public const string UrlGetRecurringSchedule = "DeliveryRequest/GetRecurringScheduleDetails?JobId={0}";
        public const string UrlDeleteRecurringSchedule = "DeliveryRequest/DeleteRecurringSchedule?Id={0}&userId={1}";
        public const string UrlGetChildDeliveryRequestInfo = "DeliveryRequest/GetChildDeliveryRequestInfo?drId={0}";
        public const string UrlRecallDeliveryRequest = "DeliveryRequest/RecallDeliveryRequest?parentDrId={0}&childDrId={1}&tfxUserId={2}";

        public const string UrlGetDRReportFilterData = "DeliveryRequest/GetDRReportDropDownFilters?userId={0}";
        public const string UrlGetAllDeliveryRequestsForCompany = "DeliveryRequest/GetAllDeliveryRequests";

        public const string UrlGetAllCarrierRegions = "Region/GetAllCarrierRegions";

        public const string UrlRemoveJobDetailsForCustomer = "Job/RemoveJobDetailsForCustomer";
        public const string UrlGetRegionFavouriteProducts = "Region/GetRegionFavouriteProducts?jobId={0}&regionId={1}&companyId={2}";
        //public const string UrlGeFavouriteProductsByJob = "Region/GeFavouriteProductsByJob?jobId={0}&companyId={1}";
        //routeInformation
        public const string UrlRouteInfoCreate = "RouteInfo/Create";
        public const string UrlRouteInfoUpdate = "RouteInfo/Update";
        public const string UrlRouteInfoDelete = "RouteInfo/Delete";
        public const string UrlGetRouteLocationDetails = "RouteInfo/GetLocationDetails?companyId={0}&regionId={1}";
        public const string UrlGetRouteEditLocationDetails = "RouteInfo/GetLocationDetails?Id={0}&regionId={1}";
        public const string UrlGetRouteInfoDetails = "RouteInfo/GetRouteInfoDetails?companyId={0}&regionId={1}";
        public const string UrlGetTPORouteInfoDetails = "RouteInfo/GetRouteInfoDetails?regionId={0}";
        public const string UrlGetRouteInfoDetailsForCustomerLocation = "RouteInfo/GetRouteInfoDetails";
        public const string UrlAssignJobToRoutePost = "RouteInfo/AssignTPOJobToRoute";
        public const string UrlRouteUpdateShiftInfo = "RouteInfo/UpdateShiftInfo";
        public const string UrlGetInvoiceRouteInfo = "RouteInfo/GetInvoiceRouteInfo";
        public const string UrlGetRouteIdForJob = "RouteInfo/GetRouteIdForJob?jobId={0}&companyId={1}&regionId={2}";

        public const string UrlUnassignDriverFromShift = "ScheduleBuilder/UnAssignDriverFromShift";

        public const string UrlProcessCarrierDeliveyForOttoAlerts = "DeliveryRequest/ProcessCarrierDeliveyForOttoAlerts";
        //public const string UrlGetJobDRPrioritiesForBuyer = "Job/GetJobDRPrioritiesForBuyer?companyId={0}&jobIds={1}";
        public const string UrlGetJobDRPrioritiesForBuyer = "Job/GetJobDRPrioritiesForBuyer";
        //Forecasting
        public const string UrlGetForecastingTankDetails = "Forecasting/GetForecastingTankDetails?jobId={0}&tankId={1}&storageId={2}&uoM={3}&jobTimeZone={4}";
        public const string UrlGetForecastingTankEstimatedUsageDetails = "Forecasting/GetForecastingTankEstimatedUsageDetails?jobId={0}&startDate={1}&endDate={2}&tankId={3}&storageId={4}&uoM={5}";
        public const string UrlGetForecastingTankInventoryDetails = "Forecasting/GetForecastingTankInventoryDetails?jobId={0}&tankId={1}&storageId={2}&uoM={3}";
        public const string UrlGetForecastingTankDeliveryDetails = "Forecasting/GetForecastingTankDeliveryDetails";
        public const string UrlGetForecastingTankScheduleDetails = "Forecasting/GetForecastingTankScheduleDetails";
        public const string UrlGetForecastingTankDataForChart = "Forecasting/GetForecastingTankDataForChart";
        public const string UrlPostCalculateTankRetainWindowInfo = "Forecasting/CalculateTankRetainWindowInfo";

        //SalesUrl
        public const string UrlGetSalesData = "Sales/GetSalesData";
        public const string UrlGetLocationTanks = "Sales/GetLocationTanks";
        public const string UrlGetSalesDataForGraph = "Sales/GetSalesGraphData?jobId={0}&noOfDays={1}";
        public const string UrlGetExistingSchedules = "Sales/GetExistingSchedules?jobId={0}&productTypeId={1}&companyId={2}";
        public const string UrlGetInventoryDataForDashboard = "Sales/GetInventoryDataForDashboard";

        //pedigree Url
        public const string UrlProcessPedigree = "DemandCapture/ProcessPedigree";
        //skybitz URl
        public const string UrlProcessSkybitz = "DemandCapture/ProcessSkybitz";
        public const string UrlProcessSkybitzAPI = "DemandCapture/ProcessSkybitzAPI";
        public const string UrlProcessIs360 = "DemandCapture/ProcessIs360";

        public const string UrlIsPublishedDR = "Region/IsPublishedDR?companyId={0}&productIds={1}&orderIds={2}";

        #endregion

        #region InvoiceServiceConstants
        public const string UrlGetBuyerInvoiceGrid = "Invoice/GetBuyerInvoiceGrid";
        public const string UrlSaveDiscount = "Invoice/SaveDiscount";
        public const string UrlDiscountGrid = "Invoice/DiscountGrid?invoiceId={0}";
        public const string UrlBuyerInvoiceGridByOrder = "Invoice/BuyerInvoiceGridByOrder?orderId={0}&userId={1}&invoiceType={2}";
        public const string UrlDealAgree = "Invoice/DealAgree?discountId={0}&invoiceId={1}&invoiceHeaderId={2}";
        public const string UrlDeclineDeal = "Invoice/DeclineDeal?discountId={0}&invoiceId={1}";
        public const string UrlEditInvoicePoNumber = "/Invoice/EditInvoicePoNumber";
        public const string UrlGetInvoiceHistoryGridBuyerAsync = "Invoice/GetInvoiceHistoryGridBuyerAsync?userId={0}&id={1}";
        public const string UrlInvoiceApprovalHistoryGrid = "Invoice/InvoiceApprovalHistoryGrid?id={0}";
        public const string UrlGetBuyerInvoiceStatus = "Invoice/GetBuyerInvoiceStatusAsync?id={0}";
        public const string UrlGetStatementPdfDetails = "Invoice/GetStatementPdfDetails?id={0}";
        public const string UrlGetBuyerWaitingForApprovalList = "Invoice/GetBuyerWaitingForApprovalListAsync?userId={0}";
        public const string UrlGetMapDataAsync = "Invoice/GetMapDataAsync?userId={0}";
        public const string UrlGetBuyerInvoiceDetails = "Invoice/GetBuyerInvoiceDetailAsync?id={0}";
        public const string UrlGetInvoicePdfNewAsync = "Invoice/GetInvoicePdfNewAsync?id={0}&companyType={1}";
        public const string UrlGetConsolidatedInvoicePdf = "Invoice/GetConsolidatedInvoicePdfAsync";
        public const string UrlValidatePoNumberBulkFile = "Invoice/ValidatePoNumberBulkFile";
        public const string UrlBuyerBolInvoiceGrid = "Invoice/BuyerBolInvoiceGrid";

        public const string UrlGetSupplierInvoiceGrid = "/Invoice/GetSupplierInvoiceGridAsync";
        public const string UrlGetInvoiceHistoryGrid = "/Invoice/InvoiceHistoryGrid?id={0}&userId={1}";
        public const string UrlGetSupplierInvoiceDetail = "/Invoice/GetSupplierInvoiceDetail?id={0}";
        public const string UrlGetPartialInvoicePdf = "/Invoice/PartialInvoicePdf?id={0}&companyType={1}";
        public const string UrlGetBDRPdf = "/Invoice/BDRPdf";
        public const string UrlValidateInvoiceBulkFile = "Invoice/ValidateInvoiceBulkFile";
        public const string UrlDownloadBDRSummary = "/Invoice/DownloadBDRSummary";
        public const string UrlGetDryRunInvoice = "/Invoice/GetDryRunInvoice?id={0}&currentUserId={1}";
        public const string UrlGetDryRunInvoiceForEdit = "/Invoice/GetDryRunInvoiceForEdit?id={0}&currentUserId={1}";
        public const string UrlCreateDryRunInvoice = "/Invoice/CreateDryRunInvoice";
        public const string UrlAssignToOrderGrid = "/Invoice/AssignToOrderGrid?currentUserId={0}";
        public const string UrlOrderPreView = "/Invoice/OrderPreView?orderId={0}&invoiceId={1}";
        public const string UrlAssignInvoiceToOrder = "/Invoice/AssignInvoiceToOrder?orderId={0}&invoiceId={1}&currentUserId={2}";
        public const string UrlGetManualInvoice = "/Invoice/GetManualInvoice?orderId={0}";
        public const string UrlGetManualInvoiceForEdit = "/Invoice/GetManualInvoiceForEdit?id={0}";
        public const string UrlCreateManualFtlInvoice = "/Invoice/CreateManualFtlInvoice";
        public const string UrlCreateManualInvoice = "/Invoice/CreateManualInvoice";
        public const string UrlCreateConsolidatedInvoice = "/Invoice/CreateConsolidatedInvoice";
        public const string UrlGetAssetsForInvoice = "/Invoice/GetAssetsForInvoice";
        public const string UrlEditDraftDDTAsync = "/Invoice/EditDraftDDTAsync";
        public const string UrlCreateInvoiceFromDropTicketForNonStandardFuel = "/Invoice/CreateInvoiceFromDropTicketForNonStandardFuel";
        public const string UrlCreateInvoiceFromDropTicketWithBol = "/Invoice/CreateInvoiceFromDropTicketWithBol";
        public const string UrlInvoiceEdit = "/Invoice/InvoiceEdit";
        public const string UrlRebillInvoiceAsync = "/Invoice/RebillInvoiceAsync";
        public const string UrlCancelDraftAsync = "/Invoice/CancelDraftAsync";
        public const string UrlConvertDdtToInvoiceManually = "/Invoice/ConvertDdtToInvoiceManually";
        public const string UrlGetNewsfeed = "/Invoice/GetNewsfeed?entityId={0}&currentPage={1}&latestId={2}&entityTypeId={3}";
        public const string UrlEditInvoiceNumber = "/Invoice/EditInvoiceNumber?invoiceId={0}&displayInvoiceNumber={1}";
        public const string UrlCreateInvoiceFromDraftDdt = "/Invoice/CreateInvoiceFromDraftDdt";
        public const string UrlGetOrderDetailsForBalanceInvoice = "/Invoice/GetOrderDetailsForBalanceInvoice?orderId={0}";
        public const string UrlCreditRebillBalanceInvoice = "Invoice/CreditRebillBalanceInvoiceAsync";
        public const string UrlCreateBalanceInvoice = "Invoice/CreateBalanceInvoiceAsync";
        public const string UrlGetOrderDetailsForTankRentalInvoice = "Invoice/GetOrderDetailsForTankRentalInvoice?orderId={0}";
        public const string UrlRebillTankRentalInvoice = "Invoice/RebillTankRentalInvoiceAsync";
        public const string UrlCreateTankRentalInvoice = "Invoice/CreateTankRentalInvoiceAsync";
        public const string UrlGetSupplierBolInfo = "Invoice/GetSupplierBolInvoicesAsync";
        public const string UrlGetOriginalInvoiceDetails = "Invoice/GetOriginalInvoiceDetails?invoiceId={0}&companyId={1}";
        public const string UrlPayConfirm = "Invoice/PayConfirm?userId={0}";
        public const string UrlGetMarineInvoiceBolList = "Invoice/GetMarineInvoiceBolListAsync?companyId={0}&headerId={1}";
        public const string UrlGetMarineInspectionVoucher = "Invoice/GetMarineInspectionVoucherDocumentInfo?invoiceHeaderId={0}";
        public const string UrlGetMarineCGInspectionDocumentInfo = "Invoice/GetMarineCGInspectionDocumentInfo?invoiceHeaderId={0}";
        public const string UrlGetMarineBDNInfo = "Invoice/GetMarineBDNInfo?invoiceHeaderId={0}";
        public const string UrlGetMarineTaxAffidavitInfo = "Invoice/GetMarineTaxAffidavitInfo?invoiceHeaderId={0}";
        public const string UrlGetInvoiceDetailSummary = "Invoice/GetInvoiceDetailSummary?id={0}";
        public const string UrlGetInvoiceHeaderIdById = "Invoice/GetInvoiceHeaderIdByIdAsync?id={0}";
        public const string UrlDeclineInvoice = "Invoice/DeclineInvoice";
        public const string UrlCalculateDropQuantitiesFromPrePostForCreateInvoice = "Invoice/CalculateDropQuantitiesFromPrePostForCreateInvoice";
        public const string UrlApproveInvoice = "Invoice/ApproveInvoice?id={0}";
        public const string UrlConvertToInvoice = "Invoice/ConvertToInvoice?ddtId={0}";
        public const string UrlGetInvoiceDropModel = "Invoice/GetInvoiceDropModel?orderId={0}";
        public const string UrlGetAssignedAssets = "Invoice/GetAssignedAssets?orderId={0}";
        public const string UrlGetTerminalPrice = "Invoice/GetTerminalPrice";
        public const string UrlGetTerminals = "Invoice/GetTerminals";
        public const string UrlSaveBDNInvoiceDetails = "Invoice/SaveBDNInvoiceDetails";
        public const string UrlGetPoDetailsToCreateInvoice = "Invoice/GetPoDetailsToCreateInvoice?orderId={0}&trackableScheduleId={1}";
        public const string UrlGetGallonsPerMetricTonAsync = "Invoice/GetGallonsPerMetricTonAsync?gravity={0}";
        public const string UrlValidateGravityByInvoiceId = "Invoice/ValidateGravityByInvoiceId?invoiceId={0}&gravity={1}";
        public const string UrlValidateGravityAndConvertForMFN = "Invoice/ValidateGravityAndConvertForMFN";
        public const string UrlGetDeclineInvoiceDetail = "Invoice/GetDeclineInvoiceDetail?id={0}&statusId={1}";
        public const string UrlIsValidApiGravity = "Invoice/IsValidApiGravity?invoiceId={0}&gravity={1}";
        public const string UrlGetBulkPlantsForAutoFreightMethod= "Invoice/GetBulkPlantsForAutoFreightMethod";

        public const string UrlDeleteBolForMarineInvoice = "Invoice/DeleteBolForMarineInvoice?invoiceHeaderId={0}&invoiceFtlDetailsId={1}&invoiceId={2}";
        public const string UrlTriggerDailyDataDumpReportCreation = "Invoice/TriggerDailyDataDumpReportCreation";

        #endregion

#region RecurringSchedule
public const string UrlGetRecurringScheduleDetails = "ScheduleBuilder/RecurringScheduleDetails?dayOfWeek={0}&currentDay={1}&date={2}";
        public const string UrlCreateDRForRecurring = "DeliveryRequest/CreateDrForRecurringSchedule";
        public const string UrlGetRecurringSheduleBuilderDetails = "ScheduleBuilder/GetRecurringScheduleBuilderDetails";
        #endregion

        #region AssignCarrier
        public const string UrlGetAssignCarrierRegion = "Region/GetRegionCarriers?regionId={0}";
        #endregion

        #region TrailerCompartment
        public const string UrlGetTrailerCompartmentDetails = "/ScheduleBuilder/GetTrailerCompartmentDetails";
        public const string UrlGetTrailerFuelRetainInfo = "/ScheduleBuilder/GetTrailerFuelRetainDetails";

        public const string UrlGetBrokereJobListForCarrier = "DemandCapture/GetBrokerJobListForCarrier?companyId={0}&regionId={1}";
        #endregion
        #region TrailerFuelRetainDetails
        public const string UrlGetTrailerFuelRetainDetails = "TrailerFuelRetain/GetTrailerFuelRetainDetails?trailerId={0}";
        public const string UrlSaveTrailerFuelRetain = "TrailerFuelRetain/SaveTrailerFuelRetain";
        public const string UrlGetBrokerJobOrderDetails = "DemandCapture/GetBrokerJobOrderDetails";
        #endregion

        #region OttoSchedule
        public const string UrlGetOttoScheduleDetails = "DeliveryRequest/GetOttoScheduleDetails?companyId={0}&regionId={1}&shiftStartTime={2}&shiftEndTime={3}&date={4}";
        public const string UrlUpdateDsbNotificationStatus = "Forecasting/UpdateDsbNotificationStatus";
        public const string UrlGetShifts = "Shift/GetShifts?companyId={0}&regionId={1}";
        public const string UrlGetDsbNotification = "Forecasting/GetDsbNotification?regionId={0}";
        public const string UrlGetDsbNotificationCount = "Forecasting/GetDsbNotificationCount?regionId={0}";
        public const string UrlGetDriversShifts = "Shift/GetDriversShifts?companyId={0}&regionId={1}&SelectedDate={2}&IsDsbDriverSchedule={3}";
        #endregion

        #region SplitDRs
        public const string UrlRaiseSplitDeliveryRequests = "DeliveryRequest/CreateSplitDeliveryRequests";
        public const string UrlCreateSplitBlendDeliveryRequests = "DeliveryRequest/CreateSplitBlendDeliveryRequests";
        #endregion


        #region FillApiService
        public const string UrlGetCompletedOrders = "/delivery-location/{0}/orders/";
        public const string UrlFilldCreateTruck = "/truck";
        public const string UrlFilldCreateUser = "/user";
        public const string UrlFilldCreateDriver = "/drivers";
        public const string UrlFilldCreateDelivery = "/delivery-locations/";
        public const string UrlFilldDeleteDelivery = "/delivery-locations/{0}";
        public const string UrlFilldUpdateDelivery = "/delivery-locations/{0}";
        public const string UrlFilldCreateFillRequest = "/assets";
        public const string UrlFilldCreateTerritory = "/territory";
        public const string keyFilldAuthorisation = "X-Api-Key";
        #endregion
        #region DsbLoadQueue
        public const string UrlCreateDsbLoadQueue = "DsbLoadQueue/Create";
        public const string UrlDeleteDsbLoadQueue = "DsbLoadQueue/Delete";
        #endregion

        #region DRCarrierSeq
        public const string UrlSaveDRCarrierSequence = "DeliveryRequest/SaveDRCarrierSequence";
        public const string UrlUpdateDRCarrierSequence = "DeliveryRequest/UpdateDRCarrierSequence";
        public const string UrlGetDRCarrierSequence = "DeliveryRequest/GetDRCarrierSequenceDetails?deliveryReqId={0}";
        public const string UrlUpdateDRCarrierRejectList = "DeliveryRequest/UpdateDRCarrierRejectList";
        public const string UrlGetAvailableDRCarrierDetails = "DeliveryRequest/GetAvailableDRCarrierDetails?deliveryReqId={0}";
        public const string UrlUpdateBrokerDeliveryRequestInfo = "DeliveryRequest/UpdateBrokerDeliveryRequestInfo";
        #endregion

        #region OptionalPickup
        public const string UrlAddOptionalPickup = "ScheduleBuilder/AddOptionalPickup";
        public const string UrlGetOptionalPickupDetails = "ScheduleBuilder/GetOptionalPickupDetails";
        public const string UrlDeleteOptionalPickupDetails = "ScheduleBuilder/DeleteOptionalPickupDetails";
        public const string UrlUpdateDROptionalPickupInfo = "ScheduleBuilder/UpdateDROptionalPickupInfo";
        #endregion
        #region RetailBOL
        public const string UrlGetBOLRetainDetails = "ScheduleBuilder/GetPreLoadDSRetainInfo";
        #endregion

        #region DeleteInvitedDriver
        public const string UrlRemoveDriverFromRegion = "Region/RemoveDriverFromRegion";
        public const string UrlCheckInvitedDriverScheduleExists = "Region/CheckInvitedDriverScheduleExists";
        #endregion
        #region SpiltDRs
        public const string UrlUpdateSpiltDRs = "DeliveryRequest/UpdateSpiltDRs";
        #endregion
        #region CancelSchedule
        public const string UrlCancelSheduleBuilder = "ScheduleBuilder/CancelScheduleBuilder";
        public const string UrlUpdateDeliveryRequestCancelStatus = "DeliveryRequest/UpdateDeliveryRequestCancelStatus";
        public const string UrlGetDeliveryRequestCancelDRs = "DeliveryRequest/GetSubDRInfoCancelDS";
        #endregion
        #region CarrierXDelivery
        public const string UrlGetCarrierXDeliveryInfo = "DeliveryRequest/GetCompanyDeliveryRequestsDetails";
        public const string UrlGetJobsTankInfo = "Tank/GetJobsTankList";
        public const string UrlGetCarrierDeliveryRequestDetails = "DeliveryRequest/GetDeliveryRequestDetails";

        #endregion

        #region InactiveDSOnOrderClose
        public const string UrlDeleteDeliveryRequestOnOrderClose = "DeliveryRequest/DeleteDeliveryRequestOnOrderClose";
        #endregion
        public const string TankId = "100";
        public const string ContentTypePdf = "application/pdf";

        public static readonly string[] FilePathSeparation = { "||" };

        public static readonly string[] NonImageSupportedFileExtensions = { ".pdf", ".docx", ".doc", ".txt" };

        //define constant for AzureFileUpload
        public const string DropImageFileNamePrefix = "drop";
        public const string CustomerSignatureFileNamePrefix = "sign";
        public const string AssetDropImageFileNamePrefix = "assetDrop";
        public const string CompanyLogoFileNamePrefix = "company";
        public const string CompanyBackgroundFileNamePrefix = "company-background";

        public const string CompanyFaviconNamePrefix = "company-favicon";
        public const string CompanyFaviconCacheName = "BrandFavicon";
        public const string CompanyLogoCacheName = "BrandLogo";

        public const string CarrierOnboardingNamePrefix = "carrier-onboarding";
        public const string CarrierOnboardingCacheName = "CarrierOnboarding";

        public const int OrderGroupsDefaultShowCount = 25;

        public const string SendbirdAppId = "SendbirdAppId";
        public const string SupplierURL = "SupplierURL_";

        public const int DefaultNotificationPeriod = 24;
        public const int DriverOnlineTimeInMinutes = 5;

        public const double MetricTonsToGallons = 315;
        public const double MetricTonsToLiters = 1192.4;
        public const double BarrelToGallons = 42;
        public const double BarrelToLiters = 158.987;
        // Invoice total display decimals
        public const int InvoiceDropQuantityDecimalDisplay = 2;
        public const int InvoiceBasicAmountDecimalDisplay = 2;
        public const int InvoiceFeeTotalAmountDecimalDisplay = 2;
        public const int InvoiceDiscountTotalAmountDecimalDisplay = 2;
        public const int InvoiceTaxTotalAmountDecimalDisplay = 2;
        public const int InvoiceSuppplierAllowanceTotalDecimalDisplay = 2;
        public const int InvoiceFuelSurchargeDecimalDisplay = 2;

        // Invoice itrm unit pprice display decimals
        public const int InvoicePricePerGallonDecimalDisplay = 4;
        public const int InvoiceSuppplierAllowanceUnitPriceDecimalDisplay = 6;
        public const int InvoiceFeeUnitPriceDecimalDisplay = 4;

        // marine invoice diplay decimals 
        public const int MarineInvoicePricePerUnitDecimalDisplay = 2;
        public const int MarineInvoiceSuppplierAllowanceUnitPriceDecimalDisplay = 2;
        public const int MarineInvoiceFeeUnitPriceDecimalDisplay = 2;
        // marine Invoice total display decimals
        public const int MarineInvoiceFeeTotalAmountDecimalDisplay = 6;
        public const int MarineInvoiceBasicAmountDecimalDisplay = 6;
        public const int MarineInvoiceTaxTotalAmountDecimalDisplay = 6;

        // LFV parameters
        public const int DefaultNoMatchRecordDays = 3;

        // terminal supplier mapping
        public const int DefaultTerminalItemCodeMappingExpiryDate = 20;

        // Forecasting sales data chart hours
        public const int VisibleHoursOnChart = 48;

        public const int DefaultDateRangeFromDate = -60;

        public const string CentralTimeZone = "Central Standard Time";

        public const double LitreToGallonsConversion = 0.264172;

        public const decimal GallonsToBarrelConversion = 42;

        public const string UrlCarrierInventoryExport = "ExchangeData/InventoryDetails?token={0}&supplierid={1}";

        
        //Marine
        public const string Marine = "Marine";
    }
}