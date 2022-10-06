using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.TJob.ExternalPricingData
{
	public class SyncExternalPricingData
	{
		private readonly string _spName;

		public SyncExternalPricingData()
		{
			//Register Context
			ContextFactory.Register(new ApplicationContext());

			_spName = ConfigHelperMethods.GetConfigSetting(ApplicationConstants.StoredProcedureName, "SyncExternalPricingData");
		}

		public async Task<int> SyncExternalPriceData()
		{
            var pricingInsertedRecords = 0;
			using (var tracer = new Tracer("SyncExternalPricingData", "SyncExternalPricingData(void)"))
			{
				try
				{
                    //Exceute the logic
                    pricingInsertedRecords = await SyncAxxisWithSiteFuel();
                }
				catch (Exception ex)
				{
					LogManager.Logger.WriteException("SyncExternalPricingData", "SyncExternalPriceData", "Exception Details : ", ex);
				}
			}
            return pricingInsertedRecords;
		}

        private async Task<int> SyncAxxisWithSiteFuel()
        {
            int recordsInserted = 0;
            try
            {
                var response = await ContextFactory.Current.GetDomain<PricingServiceDomain>().SyncAxxisPricing();
                recordsInserted = response.Result;
                if (response.Status == Status.Success && recordsInserted > 0)
                {
                    // sync dyed product pricing from clear product for canada
                    var dyedPricingSyncCount = await ContextFactory.Current.GetDomain<PricingServiceDomain>().SyncDyedProductPricingFromClearProducts();

                    Thread.Sleep(1000);
                    //update last updated date in mstappsettings
                    await GenerateInvoicesWhichAreWaitingForUpdatedPrice();
                }
                else
                {
                    await AxxisFailureNotification();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SyncExternalPricingData", "SyncAxxisWithSiteFuel", "Exception Details : ", ex);
            }
            LogManager.Logger.WriteInfo("SyncExternalPricingData", "SyncAxxisWithSiteFuel", $"Number of record inserted = {recordsInserted}");
            return recordsInserted;
        }

        private async Task AxxisFailureNotification()
        {
            var pricingServiceDomain = new PricingServiceDomain();
            var keys = new List<string>();
            keys.Add(ApplicationConstants.PublicHolidayList);
            keys.Add(ApplicationConstants.PricingDataLastUpdatedDate);
            var pricingConfigs =  await pricingServiceDomain.GetPricingConfigs(keys);
            var holidayDateList = string.Empty;
            if (pricingConfigs.Status == Status.Success && pricingConfigs.Configs != null)
            {
                holidayDateList = pricingConfigs.Configs.FirstOrDefault(t => t.Key == ApplicationConstants.PublicHolidayList)?.Value;
            }
            var holidayDates = pricingServiceDomain.GetDatelistFromString(new List<DateTime>(), holidayDateList);
            var todaysDate = DateTime.Now;
            if (!(todaysDate.DayOfWeek == DayOfWeek.Saturday || todaysDate.DayOfWeek == DayOfWeek.Sunday || (holidayDates.Count > 0 && holidayDates.Contains(todaysDate.Date))))
            {
                var priceUpdatedDate = pricingConfigs.Configs.FirstOrDefault(t => t.Key == ApplicationConstants.PricingDataLastUpdatedDate);
                if (DateTime.TryParse(priceUpdatedDate?.Value, out DateTime currentDateTime) && currentDateTime.Date < todaysDate.Date && todaysDate.Hour > 18)
                {
                    var notificationDomain = new NotificationDomain(pricingServiceDomain);
                    //add notification to send email
                    bool isNotificationAlreadyExists = notificationDomain.IsNotificationExists(EventType.EmailToMonitorAxxisDataUpdates, todaysDate.Date);
                    if (!isNotificationAlreadyExists)
                    {
                        var task = notificationDomain.AddNotificationEventAsync(EventType.EmailToMonitorAxxisDataUpdates, 0, 1);
                        task.Wait();
                    }
                }
            }
        }

        private async Task GenerateInvoicesWhichAreWaitingForUpdatedPrice()
		{
			int generatedInvoices = 0;
            try
            {
                using (var tracer = new Tracer("SyncExternalPricingData", "GenerateInvoicesWhichAreWaitingForUpdatedPrice"))
                {
                    generatedInvoices = await ContextFactory.Current.GetDomain<ConsolidatedDdtToInvoiceDomain>().ProcessInvoicesWaitingForUpdatedPrice();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SyncExternalPricingData", "GenerateInvoicesWhichAreWaitingForUpdatedPrice", "Exception Details : ", ex);
            }
			LogManager.Logger.WriteInfo("SyncExternalPricingData", "GenerateInvoicesWhichAreWaitingForUpdatedPrice", $"Number of Invoices generated = {generatedInvoices}");
		}
    }
}
