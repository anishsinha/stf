using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TrueFill.Exchange.Tjob.ThirdPartyDiptestAlert
{
	public class ThirdPartyDiptestAlert
	{
		public ThirdPartyDiptestAlert()
		{
			//Register Context
			ContextFactory.Register(new ApplicationContext());
		}

		public async Task<Status> ProcessThirdPartyDiptestAlerts()
		{
			var response = Status.Failed;
			try
			{
				response = await ContextFactory.Current.GetDomain<FreightServiceDomain>().SaveDiptestPendingNotificationEventsAync(InventoryDataCaptureType.Connected);
				
			}
			catch (Exception ex)
			{
				LogManager.Logger.WriteException("ThirdPartyDiptestAlert", "ProcessThirdPartyDiptestAlerts", "Exception Details : "+ ex.Message, ex);
			}
			return response;
		}
	}
}
