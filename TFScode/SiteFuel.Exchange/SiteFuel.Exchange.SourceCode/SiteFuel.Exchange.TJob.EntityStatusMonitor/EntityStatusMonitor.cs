using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.TJob.EntityStatusMonitor
{
	public class TriggeredEntityStatusMonitor
	{
		public TriggeredEntityStatusMonitor()
		{
			//Register Context
			ContextFactory.Register(new ApplicationContext());
		}

		public async Task<bool> ProecssEntityStatusMonitor()
		{
			using (var tracer = new Tracer("TriggeredEntityStatusMonitor", "ProecssEntityStatusMonitor"))
			{
				try
				{
					//Start your email notfication logic here
					await StartProcessingEventStatus();
				}
				catch (Exception ex)
				{
					LogManager.Logger.WriteException("TriggeredEntityStatusMonitor", "ProecssEntityStatusMonitor", "Exception Details : ", ex);
				}
				return true;
			}
		}

		private async Task StartProcessingEventStatus()
		{
			try
			{
				var jobs = await ContextFactory.Current.GetDomain<JobDomain>().GetOpenJobsWithEndDateAsync();
				foreach (var job in jobs)
				{
					await ProcessJobStatus(job);
				}

				var orders = await ContextFactory.Current.GetDomain<OrderDomain>().GetOpenOrdersWithEndDateAsync();
				foreach (var order in orders)
				{
					await ProcessOrderStatus(order);
				}
			}
			catch (Exception ex)
			{
				LogManager.Logger.WriteException("TriggeredEntityStatusMonitor", "StartProcessingEventStatus", "Exception Details : ", ex);
			}
		}

		private async Task ProcessJobStatus(int id)
		{
			try
			{
				await ContextFactory.Current.GetDomain<JobDomain>().ProcessJobClosureAsync(UserContext.GetSystemUserContext(), id);
			}
			catch (Exception ex)
			{
				LogManager.Logger.WriteException("TriggeredEntityStatusMonitor", "ProcessJobStatus", "Exception Details : ", ex);
			}
		}

		private async Task ProcessOrderStatus(int id)
		{
			try
			{
				await ContextFactory.Current.GetDomain<OrderDomain>().ProcessOrderClosureAsync(id);
			}
			catch (Exception ex)
			{
				LogManager.Logger.WriteException("TriggeredEntityStatusMonitor", "ProcessOrderStatus", "Exception Details : ", ex);
			}
		}
	}
}
