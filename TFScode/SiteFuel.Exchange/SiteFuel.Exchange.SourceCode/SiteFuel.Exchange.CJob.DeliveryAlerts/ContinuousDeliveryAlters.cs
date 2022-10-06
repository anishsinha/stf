using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.CJob.DeliveryAlerts
{
	public class ContinuousDeliveryAlters
	{
		public ContinuousDeliveryAlters()
		{
			//Register Context
			ContextFactory.Register(new ApplicationContext());
		}

		public async Task ProcessDeliveryAlters()
		{
            var watch = Stopwatch.StartNew();
            try
			{                
                var openOrders = await ContextFactory.Current.GetDomain<OrderDomain>().GetOpenOrdersAync();
				await StartProcessingDeliverySchedules(openOrders);
				await StartProcessingDeliveryScheduleStatus();
			}
			catch (Exception ex)
			{
				LogManager.Logger.WriteException("ContinuousDeliveryAlters", "ProcessDeliveryAlters", "Exception Details : ", ex);
			}
            watch.Stop();
            LogManager.Logger.WriteInfo("CJob.DeliveryAlerts", "ProcessDeliveryAlters", "End:TotalTime:" + watch.ElapsedMilliseconds);
        }

		private async Task StartProcessingDeliverySchedules(List<int> openOrders)
		{
			try
			{
				foreach (var item in openOrders)
				{
					await ProcessDeliverySchedules(item);
				}
			}
			catch (Exception ex)
			{
				LogManager.Logger.WriteException("ContinuousDeliveryAlters", "StartProcessingDeliverySchedules", "Exception Details : ", ex);
			}
		}

		private async Task StartProcessingDeliveryScheduleStatus()
		{
			try
			{
				var openSchedules = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetOpenSchedulesAync();
				int missedScheduleWaitingPerod = await ContextFactory.Current.GetDomain<MasterDomain>().GetMissingScheduleWaitingPeriod();
				foreach (var item in openSchedules)
				{
					await ContextFactory.Current.GetDomain<OrderDomain>().ProcessDeliveryScheduleStatusAsync(item, missedScheduleWaitingPerod);
				}
			}
			catch (Exception ex)
			{
				LogManager.Logger.WriteException("ContinuousDeliveryAlters", "StartProcessingDeliveryScheduleStatus", "Exception Details : ", ex);
			}
		}


		private async Task ProcessDeliverySchedules(int orderId)
		{
			try
			{
				await ContextFactory.Current.GetDomain<OrderDomain>().ProcessDeliverySchedulesAsync(orderId);
			}
			catch (Exception ex)
			{
				LogManager.Logger.WriteException("ContinuousDeliveryAlters", "ProcessDeliverySchedules", "Exception Details : ", ex);
			}
		}
	}
}
