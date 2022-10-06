namespace SiteFuel.Exchange.TJob.ExternalPricingData
{
    using SiteFuel.Exchange.Core;
    using SiteFuel.Exchange.Core.Logger;
    using SiteFuel.Exchange.Domain;
    using SiteFuel.Exchange.Utilities;
    using System;
    using System.Threading.Tasks;

    public class ExchangeService
    {
        public ExchangeService()
        {
            //Register Context
            ContextFactory.Register(new ApplicationContext());
        }

        public async Task<bool> UpdateDailyExchangeRate()
        {
            bool response = false;

            using (var tracer = new Tracer("ExchangeService", "UpdateDailyExchangeRate"))
            {
                try
                {
                    var currentDateTime = DateTimeOffset.Now;
                    CurrencyRateDomain currencyRateDomain = new CurrencyRateDomain();

                    var date = currencyRateDomain.GetLatestConversionEntryDate();

                    var exchangeSvcRefreshDuration = 6;
                    exchangeSvcRefreshDuration = ApplicationDomain.ApplicationSettings.GetApplicationSettingValue(ApplicationConstants.ExchangeSvcRefreshDuration, exchangeSvcRefreshDuration);

                    if (date == null || (currentDateTime - date.Value).Hours > exchangeSvcRefreshDuration - 1)
                    {
                        Console.WriteLine("Starting CurrencyLayer ExchangeService");
                        var url = "http://www.apilayer.net/api/live?access_key=9c69972c5645c4e537c69bd52c961b98&format=1";
                        url = ApplicationDomain.ApplicationSettings.GetApplicationSettingValue(ApplicationConstants.ExchangeServiceUrl, url);
                        var service = new CurrencyLayerExchangeService();
                        var rates = service.GetLatestCurrencyRates(url);
                        currencyRateDomain.SaveExchangeRate(rates, currentDateTime);
                        var status = await currencyRateDomain.SaveExchangeRateInPricing(rates, currentDateTime);
                        if (status.StatusCode == Status.Success)
                        {
                            response = true;
                        }
                        Console.WriteLine("Update completed for CurrencyLayer ExchangeService");
                        Logger.LogManager.Logger.WriteDebug("ExchangeService", "UpdateDailyExchangeRate", $"UpdateDailyExchangeRate completed for {rates.Count} exchange rates");
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogManager.Logger.WriteException("", "", "Critical: Exchange service", ex);
                }
            }
            return response;
        }

        public async Task<bool> ProcessDdtPendingToInvoiceNotifications()
        {
            bool response = false;
            using (var tracer = new Tracer("ExchangeService", "ProcessDdtPendingToInvoiceNotifications"))
            {
                try
                {
                    NotificationDomain domain = new NotificationDomain();
                    response = await domain.ProcessDdtPendingToInvoiceNotifications();
                }
                catch (Exception ex)
                {
                    Logger.LogManager.Logger.WriteException("ExchangeService", "ProcessDdtPendingToInvoiceNotifications", "Exception Details : ", ex);
                }
            }
            return response;
        }
        public async Task<bool> ProcessDdtPendingForFilldResponse()
        {
            bool response = false;
            using (var tracer = new Tracer("ExchangeService", "ProcessDdtPendingForFilldResponse"))
            {
                try
                {
                    response = await ContextFactory.Current.GetDomain<ConsolidatedDdtToInvoiceDomain>().ProcessInvoicesWaitingForFilld();
                }
                catch (Exception ex)
                {
                    Logger.LogManager.Logger.WriteException("ExchangeService", "ProcessDdtPendingForFilldResponse", "Exception Details : ", ex);
                }
            }
            return response;
        }

        public async Task<bool> ProcessUnknownDeliveryExceptionManagement()
        {
            bool response = false;
            using (var tracer = new Tracer("ExchangeService", "ProcessUnknownDeliveryExceptionManagement"))
            {
                try
                {
                    var exceptionDomain = new ExceptionDomain();
                    response = await exceptionDomain.ProcessUnknownDeliveryExceptionManagement();
                }
                catch (Exception ex)
                {
                    Logger.LogManager.Logger.WriteException("ExchangeService", "ProcessUnknownDeliveryExceptionManagement", "Exception Details : ", ex);
                }
            }

            return response;
        }

        public async Task<bool> ProcessMissingDeliveryExceptionManagement()
        {
            bool response = false;
            using (var tracer = new Tracer("ExchangeService", "ProcessMissingDeliveryExceptionManagement"))
            {
                try
                {
                    var exceptionDomain = new ExceptionDomain();
                    response = await exceptionDomain.ProcessMissingDeliveryExceptionManagement();
                }
                catch (Exception ex)
                {
                    Logger.LogManager.Logger.WriteException("ExchangeService", "ProcessMissingDeliveryExceptionManagement", "Exception Details : ", ex);
                }
            }
            return response;
        }

        public async Task<bool> ProcessPedigreeData()
        {
            bool response = false;
            using (var tracer = new Tracer("ExchangeService", "ProcessPedigreeData"))
            {
                try
                {
                    var pedigreeDomain = new PedigreeDomain();
                    response = await pedigreeDomain.ProcessPedigree();
                }
                catch (Exception ex)
                {
                    Logger.LogManager.Logger.WriteException("ExchangeService", "ProcessPedigreeData", "ProcessPedigreeData : ", ex);
                }
            }
            return response;
        }

         public async Task<bool> TriggerLiftFileRecordsDailyReportCreation()
        {
            var response = false;
            using (var tracer = new Tracer("ExchangeService", "TriggerLiftFileRecordsDailyReportCreation"))
            {
                try
                {
                    var dateTimeOffsetCST = DateTimeOffset.Now.ToTargetDateTimeOffset("Central Standard Time");
                    // hour is in 24 hour format
                    if (dateTimeOffsetCST != null && dateTimeOffsetCST.Hour >= 7 && dateTimeOffsetCST.Hour < 8)
                    {
                        // call domain method here
                        var lfvDomain = new LFVDomain();
                        response = await lfvDomain.ProcessLiftFileReportCreationPerCompany();
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogManager.Logger.WriteException("ExchangeService", "TriggerLiftFileDailyReportCreation", "TriggerLiftFileDailyReportCreation : ", ex);
                }
            }
            return response;
        }


        public async Task<bool> TriggerGroupDrConsolidationProcess()
        {
            var response = false;
            using (var tracer = new Tracer("ExchangeService", "TriggerGroupDrConsolidationProcess"))
            {
                try
                {
                    var dateTimeOffsetCST = DateTimeOffset.Now.ToTargetDateTimeOffset(ApplicationConstants.CentralTimeZone);
                    // hour is in 24 hour format
                    if (dateTimeOffsetCST != null && dateTimeOffsetCST.Hour % 2 == 0)
                    {
                        response = await new ConsolidatedDdtDomain().TriggerGroupDrConsolidationProcess();
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogManager.Logger.WriteException("ExchangeService", "TriggerGroupDrConsolidationProcess", "TriggerGroupDrConsolidationProcess : ", ex);
                }
            }
            return response;
        }



        public async Task<bool> ProcessConsolidatedDdtCreation()
        {
            using (var tracer = new Tracer("ExchangeService", "ProcessConsolidatedDdtCreation"))
            {
                try
                {
                    var ddtDomain = ContextFactory.Current.GetDomain<ConsolidatedDdtDomain>();
                    await ddtDomain.ProcessConsolidatedDdtCreation();
                }
                catch (Exception ex)
                {
                    Logger.LogManager.Logger.WriteException("ExchangeService", "ProcessConsolidatedDdtCreation", "ProcessConsolidatedDdtCreation : ", ex);
                }
                return true;
            }
        }
        
        public async Task<bool> ProcessSkybitzData()
        {
            bool response = false;
            using (var tracer = new Tracer("ExchangeService", "ProcessSkybitzData"))
            {
                try
                {
                    var pedigreeDomain = new PedigreeDomain();
                    response = await pedigreeDomain.ProcessSkybitz();
                }
                catch (Exception ex)
                {
                    Logger.LogManager.Logger.WriteException("ExchangeService", "ProcessSkybitzData", "ProcessSkybitzData : ", ex);
                }
            }
            return response;
        }


        public async Task<bool> TriggerDailyDeliveryDataDumpReportCreation()
        {
            var response = false;
            using (var tracer = new Tracer("ExchangeService", "TriggerDailyDeliveryDataDumpReportCreation"))
            {
                try
                {
                    var dateTimeOffsetCST = DateTimeOffset.Now.ToTargetDateTimeOffset("Central Standard Time");
                    // hour is in 24 hour format
                    if (dateTimeOffsetCST != null && dateTimeOffsetCST.Hour >= 2 && dateTimeOffsetCST.Hour < 3)
                    {
                        // call domain method here
                        var invoiceService = new InvoiceServiceApiDomain();
                        response = await  invoiceService.ProcessDailyDeliveryDataDumpReportCreation();
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogManager.Logger.WriteException("ExchangeService", "TriggerDailyDeliveryDataDumpReportCreation", "TriggerDailyDeliveryDataDumpReportCreation failed : "+ex.Message + DateTimeOffset.Now , ex);
                }
            }
            return response;
        }
    }
}
