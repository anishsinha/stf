using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TrueFill.Exchange.Tjob.ManualAlert
{
	public class ManualDiptestAlert
	{
		public ManualDiptestAlert()
		{
			//Register Context
			ContextFactory.Register(new ApplicationContext());
		}

		public async Task<Status> ProcessManualDiptestAlerts()
		{
			var response = Status.Failed;
			try
			{
				Console.WriteLine("ProcessManualDiptestAlerts- Start");

				response = await ContextFactory.Current.GetDomain<FreightServiceDomain>().SaveDiptestPendingNotificationEventsAync(InventoryDataCaptureType.Manual);
				Console.WriteLine("ProcessManualDiptestAlerts- End");

			}
			catch (Exception ex)
			{
				LogManager.Logger.WriteException("ManualDiptestAlert", "ProcessManualDiptestAlerts", "Exception Details : ", ex);
			}
			return response;
		}
	}
}
