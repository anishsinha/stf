using S22.Imap;
using SiteFuel.BAL;
using SiteFuel.FreightModels;
using SiteFuel.Repository;
using SiteFuel.Exchange.Logger;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using SiteFuel.Exchange.Utilities;
using System.IO;
using System.Data;

namespace TrueFill.HourlyWebJob
{
	public class DemandCaptureService
	{
		public async Task<bool> ProcessData()
		{
			var response = false;
			try
			{
				string hostname = ConfigurationManager.AppSettings.Get("HostName");
				int defaultPortNumber = Convert.ToInt32(ConfigurationManager.AppSettings.Get("PortNumber"));
				string username = ConfigurationManager.AppSettings.Get("UserEmail");
				string password = ConfigurationManager.AppSettings.Get("UserPassword");
				LogManager.Logger.WriteDebug("DemandCaptureService", "ProcessData", "Start Processing Demand Capture Emails");
				// The default port for IMAP over SSL is 993.
				using (var client = new ImapClient(hostname, defaultPortNumber, username, password, AuthMethod.Login, true))
				{
					// Returns a collection of identifiers of all mails matching the specified search criteria.
					var demandCaptureDomain = new DemandCaptureDomain(new DemandCaptureRepository());
					//var uids = client.Search(SearchCondition.Unseen()); // need to change its logic
					var lastProcessedUid = await demandCaptureDomain.GetLastProcessedUid();
					if (lastProcessedUid.StatusCode == (int)Status.Success)
					{
						var lastUid = Convert.ToUInt32(lastProcessedUid.Result);
						var lastUids = new List<uint>() { lastUid };
						var uids = client.Search(SearchCondition.GreaterThan(lastUid)).Except(lastUids).ToList();
						LogManager.Logger.WriteDebug("DemandCaptureService", "ProcessData", "uids to process => " + uids.Count());
						await ProcessAttachmentFiles(client, demandCaptureDomain, uids);
						LogManager.Logger.WriteDebug("DemandCaptureService", "ProcessData", "uids processed => " + uids.Count());
					}
					response = true;
				}
			}
			catch (Exception ex)
			{
				LogManager.Logger.WriteException("DemandCaptureService", "ProcessData", ex.Message, ex);
			}
			return response;
		}

		private static async Task ProcessAttachmentFiles(ImapClient client, DemandCaptureDomain demandCaptureDomain, List<uint> uids)
		{
			// filter tanks to be processed for FFS
			var tanksToBeProcessed = await demandCaptureDomain.GetProcessTanks(DipTestMethod.FranklinFuelSystem);
			var tomorrowDate = DateTime.UtcNow.AddDays(1).Date;
			var lastMonthDate = DateTime.UtcNow.AddDays(-30).Date;

			foreach (var uid in uids)
			{
				// Download mail messages from the default mailbox.
				var messages = client.GetMessage(uid);
				if (messages.Subject.Contains("Parkland Hourly E Dips"))
				{
					LogManager.Logger.WriteDebug("DemandCaptureService", "ProcessAttachmentFiles", "processing uid => " + uid);

					var attachments = messages.Attachments;
					var attachmentfiles = attachments.Where(t => t.Name.EndsWith(".csv")).ToList();

					foreach (var file in attachmentfiles)
					{
						var name = file.Name;
						LogManager.Logger.WriteDebug("DemandCaptureService", "ProcessAttachmentFiles", "Process file => " + name);

						var reader = new StreamReader(file.ContentStream);
						var fileText = reader.ReadToEnd();
						var demandList = new List<DemandModel>();
                        if (tanksToBeProcessed.AnyAndNotNull())
                        {
							demandList = demandCaptureDomain.ProcessCsvFileContent(fileText, (int)DipTestMethod.FranklinFuelSystem);
							//remove additionaltanks
							demandList.RemoveAll(t => !tanksToBeProcessed.Any(t1 => t1.SiteId == t.SiteId && t1.StorageId == t.StorageId && t1.TankId == t.TankId) || t.CaptureTime > tomorrowDate || t.CaptureTime < lastMonthDate);
						}
						var supplierIdStr = ConfigurationManager.AppSettings["DemandCaptureFileSupplierId"];
						var supplierId = Int32.Parse(supplierIdStr);
						var result = await demandCaptureDomain.CreateDemandFromCsv(demandList, demandList.Count, name, supplierId, uid);
					}
				}
			}
		}
		public async Task<bool> ProcessVedorRootData()
		{
			var response = false;
			try
			{
				var demandCaptureDomain = new DemandCaptureDomain(new DemandCaptureRepository());
				return await demandCaptureDomain.ProcessVedorRoot();
			}
			catch (Exception ex)
			{
				LogManager.Logger.WriteException("DemandCaptureService", "ProcessVedorRootData", ex.Message, ex);
			}
			return response;
		}
		
	}
}
