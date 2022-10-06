using S22.Imap;
using SiteFuel.BAL;
using SiteFuel.FreightModels;
using SiteFuel.Repository;
using SiteFuel.Exchange.Logger;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiteFuel.Exchange.Utilities;
using System.IO;
using SiteFuel.FreightRepository;
using System.Data.SqlClient;

namespace TrueFill.HourlyWebJob
{
    public class DeliveryRequestService
	{
		public async Task<bool> ProcessDeliveryRequest()
		{
			var response = false;
			try
			{
                var drDomain = new DeliveryRequestDomain(new DeliveryRequestRepository());
                await drDomain.ScheduleUpdateDeliveryRequest();
                response = true;
			}
			catch (Exception ex)
			{
				LogManager.Logger.WriteException("DeliveryRequestService", "ProcessDeliveryRequest", ex.Message, ex);
			}
			return response;
		}
        public async Task<bool> ProcessOttoDeliveryRequest()
        {
            var response = false;
            try
            {
                var drDomain = new DeliveryRequestDomain(new DeliveryRequestRepository());
                await drDomain.ScheduleOttoDeliveryRequest();
                response = true;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryRequestService", "ProcessOttoDeliveryRequest", ex.Message, ex);
            }
            return response;
        }
    }
}
