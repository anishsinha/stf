using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteFuel.Exchange.ViewModels
{
    public static class Constants
    {
        public static readonly string InvalidOrder = "Invalid Order Number";
        
        public static readonly int OKStatus = 200;
        public static readonly int FailStatus = 0;
        public static readonly string NoJobsFound = "No Order(s) found";
        public static readonly string NoStateFound = "No State(s) found";
        public static readonly string NoFuelTypeFound = "No FuelType(s) found";
        public static readonly string NoImageFound = "Image not found";
        public static readonly string NoAssetFound = "Asset not found";
        public static readonly string NoSpillFound = "No Spill found";
        public static readonly string Success = "Success";
        public static readonly string RequestError = "Request error";
        public static readonly string MeterReadingError = "Incorrect meter reading combination";
        public static readonly string PdfNotAvailable = "Pdf Not Available";

        public static readonly string DataMissing = "data is missing";
        public static readonly string ConversionError = "Conversion error";
        public static readonly string InvalidDropRequest = "Invalid drop request";
        public static readonly string InvalidCreateRequest = "Invalid create request";
        public static readonly string CanNotSaveImageOnServer = "Can not save image on server";
        public static readonly string UserNotExists = "Email does not exists";
        public static readonly double MinimumKmForExactMatch = 1.6;

        public static readonly string ErrorInAssetAdding = "Error while adding asset";
        public static readonly string ErrorInJobAssignment = "Error while assigning Asset to order";
        
        public static readonly string AvaTaxUserKey = "AvaTaxUserKey";
        public static readonly string AvaTaxPassKey = "AvaTaxPassKey";
        public static readonly string AvaTaxCompanyKey = "AvaTaxCompanyKey";
        public static readonly string AvaTaxLoginUrlKey = "AvaTaxLoginUrlKey";
        public static readonly string AvaTaxTaxUrlKey = "AvaTaxTaxUrlKey";
        public static readonly string AvaTaxExemptionUrlKey = "AvaTaxExemptionUrlKey";
        public static readonly string AvaTaxCanUseServiceKey = "AvaTaxCanUseServiceKey";
        public static readonly string AvaTaxExemptionEnabled = "AvaTaxExemptionEnabled";
        public static readonly string AvaTaxKey = "AvaTax";
        public static readonly string SFXProductSuffix = "SFXP";
        public static readonly string TimeCardDisabledMessage = "Timecard feature is disabled!!";
        public static readonly string Missed = "Missed";
        public static readonly string Declined = "Declined";

        public static readonly string CountryCAN = "CAN";
        public static readonly string CountryUSA = "USA";

        public static readonly string DefaultPassword = "Truefill123";
        public static readonly string Key = "b14ca5898a4e4133bbce2ea2315a1916";
        public static readonly int LengthOfPassword = 8;
        public static readonly int DefaultTheme = 1;

        public static readonly string OrderBulk = "OrderBulk";
        public static readonly string InvoiceBulk = "InvoiceBulk";
        public static readonly string JobAssetBulk = "JobAssetBulkUpload";
        public static readonly string PoNumberBulk = "PoNumberBulk";
        public static readonly string ProductMappingBulk = "ProductMappingBulk";
        public static readonly string TankBulk = "TankBulk";
        public static readonly string AssetBulk = "AssetBulk";
        public static readonly string SFXOrderBulkUploadSuffix = "TFXBulk-";
        public static readonly string SFXProductMappingBulkUploadSuffix = "TFXProductMappingBulk-";
        public static readonly string TFXTerminalItemCodeMappingBulkUploadSuffix = "TFXTerminalItemCodeMappingBulk-";
        public static readonly string TerminalItemCodeMappingBulk = "TerminalItemCodeMappingBulk";
        public static readonly string SFXAMPDataUploadSuffix = "SFX-AMPData-"; 
        public static readonly string Between = " between ";
        public static readonly string For = " for ";
        public static readonly string And = " and ";
        public const string ErrorWhileProcessingBulkOrder = "Error ocurred while processing the bulk uploaded file. Please contact support";
        public const string county = "County";
        public static readonly string ApplicationSettingsValue = "ApplicationSettingsCacheKey";
        public static readonly string JobBulkUpload = "JobBulkUpload";

        public static readonly string InvoiceCSV = "InvoiceCsv-";
        public static readonly string ErrorWhileProcessingAMPJobData = "Error ocurred while processing AMP stream data. Please contact support";
        public static readonly string PricingDataLastUpdatedDate = "PricingDataLastUpdatedDate";
        public static readonly string PricingDataSourcesUpdatedDate = "PricingDataSourcesUpdatedDate";
        public static readonly string SupportContactNumber = "SupportContactNumber"; 
        public static readonly string DummyAxxisZipCode = "00001";
        public static readonly string DummyOpisZipCode = "00002";
        public static readonly string DummyPlattsZipCode = "00003";
        public static readonly string DummyPhoneNumber = "1111111111";
        public static readonly string OtherCommonFeeCode = "OCF";
        public static readonly string PerGallon = "Per Gallon";
        public static readonly string Gallon = "Gallon";
        public static readonly string Litre = "Litre";
        public static readonly string PerLitre = "Per Litre";
        public static readonly string PerHour = "Per Hour";
        public static readonly string PerAsset = "Per Asset";
        public static readonly string FlatFee = "Flat Fee";
        public static readonly TimeSpan StartTime = new TimeSpan(8, 00, 0);
        public static readonly TimeSpan EndTime = new TimeSpan(17, 00, 0);

        public static readonly string AssetFilled = "Filled";
        public static readonly string NoFuelNeeded = "No Fuel Needed";
        public static readonly string AssetNotAvailable = "Asset Not Available";

        public static readonly string TimeZoneCountryUS = "US";
        public static readonly string TimeZoneCountryCanada = "Canada";

        public static readonly int InvoiceNotesDefaultLength = 500;
        public static readonly string MaxInvoiceAttachmentsPerEmailKey = "MaxInvoiceAttachmentsPerEmail";
        public static readonly string MaxInvoiceCountPerSessionKey = "MaxInvoiceCountPerSession";

        public static readonly string EaiUrlKey = "EaiUrlKey";
        public static readonly string EaiHolidayList = "EaiHolidayList";
        public static readonly string EaiPetroliumProduct = "PET.EMD_EPD2DXL0_PTE_NUS_DPG.";
        public static readonly string EaiGasolineProduct = "PET.EMM_EPMR_PTE_NUS_DPG.";
        public static readonly string EaiWeekly = "W";
        public static readonly string EaiMonthly = "M";
        public static readonly string EaiAnnually = "A";
        public static readonly char FileNameSplitCharacter = '_';

        public static readonly string TwilioAccountSid = "TwilioAccountSid";
        public static readonly string TwilioAuthToken = "TwilioAuthToken";
        public static readonly string TwilioFromPhoneNumber = "TwilioFromPhoneNumber";
        public static readonly string SmsSendingCountryCode = "SmsSendingCountryCode";
        public static readonly string SmsSendingEnabled = "SmsSendingEnabled";
        public static readonly string EmailSendingEnabled = "EmailSendingEnabled";
        public static readonly string DtnTXDLRateDecimalFormat = "DtnTXDLRateDecimalFormat";

        public static readonly string ApiCodeRQ01 = "RQ01";
        public static readonly string ApiCodeRQ02 = "RQ02";
        public static readonly string ApiCodeRQ03 = "RQ03";
        public static readonly string ApiCodeRQ04 = "RQ04";
        public static readonly string ApiCodeRQ05 = "RQ05";
        public static readonly string ApiCodeRQ06 = "RQ06";
        public static readonly string ApiCodeRQ07 = "RQ07";

        public static readonly string ApiCodeRS01 = "RS01";
        public static readonly string ApiCodeRS02 = "RS02";
        public static readonly string ApiCodeRS03 = "RS03";
        public static readonly string ApiCodeRS04 = "RS04";
        public static readonly string ApiCodeRS05 = "RS05";
        public static readonly string ApiCodeRS06 = "RS06";
        public static readonly string ApiCodeRS07 = "RS07";

        public static readonly string ApiCodeTF01 = "TF01";
        public static readonly string ApiCodeTF02 = "TF02";
        public static readonly string ApiCodeTF03 = "TF03";
        public static readonly string ApiCodeTF04 = "TF04";

        public static readonly string ApiCodeEV01 = "EV01";
        public static readonly string ApiCodeEV02 = "EV02";
        public static readonly string ApiCodeEV03 = "EV03";


        public static readonly string ApiCodeFR01 = "FR01";
        public static readonly string ApiCodeFR02 = "FR02";
        public static readonly string ApiCodeFR03 = "FR03";
        public static readonly string ApiCodeFR04 = "FR04";
        public static readonly string ApiCodeFR05 = "FR05";
        public static readonly string ApiCodeFR06 = "FR06";
        public static readonly string ApiCodeFR07 = "FR07";
        public static readonly string ApiCodeFR08 = "FR08";
       

        public static readonly string WholeSaleBadgeFile = "WholeSaleBadgeNoBulk";
        public static readonly string LiftFileCarrierNamesFile = "LiftFileCarrierNamesBulk";
        public static readonly string LiftFileQuebecBadgeFile = "LiftFileQuebecBadgeFileBulk";
        public static readonly string NFNSupplierCompanyId = "NFNSupplierCompanyId"; 
        public static readonly string HandaBandSupplierCompanyId = "HandaBandSupplierCompanyId";

        public static readonly string PDIDefaultKeyForCreditInvoice = "C";
        public static readonly string PDIDefaultKeyForRebillInvoice = "A";

    }
}