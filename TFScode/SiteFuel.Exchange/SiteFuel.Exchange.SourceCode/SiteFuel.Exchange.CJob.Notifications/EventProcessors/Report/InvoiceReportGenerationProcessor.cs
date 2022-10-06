using FileHelpers;
using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.CJob.Notifications.EventProcessors
{
	public class InvoiceReportGenerationProcessor : BaseReportEventProcessor, IEmailProcessor
    {
        public EventType EventType => EventType.InvoiceReportGenerated;

        public InvoiceReportGenerationProcessor()
        {
        }

        public void Initialize(NotificationEventViewModel notificationEventViewModel)
		{
            notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();
		}

        public override void SendDefaultBuyerEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
        {
            
        }

        public override void SendDefaultSupplierEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
        {
            
        }

        public override void SendEmail(NotificationDomain notificationDomain, NotificationEventViewModel notificationEventViewModel)
		{
            try
            {
                var requestModel = JsonConvert.DeserializeObject<InvoiceReportFilter>(notificationEventViewModel.JsonMessage);

                SendEmailInvoiceReport(requestModel, notificationEventViewModel.Id);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceReportGenerationProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }

        public async Task<bool> SendEmailInvoiceReport(InvoiceReportFilter requestModel, int notificationId)
        {
            MemoryStream contentStream = null;
            var report = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetInvoiceReportAsync(requestModel);
            var csvReport = report.Select(t => t.ToCsvViewModel());
            contentStream = await GetCsvAsMemoryStream(csvReport);

            var subject = string.Format(Resource.emailInvoiceReport_SubjectText, requestModel.StartDate, requestModel.EndDate);
            var body = string.Format(Resource.emailInvoiceReport_BodyText, requestModel.StartDate, requestModel.EndDate);
            var fileName = "invoice-report-" + DateTimeOffset.UtcNow.ToString("MM/dd/yyyy HH:mm tt") + ".csv";
            var response = SendEmailCsvFile(contentStream, requestModel.EmailTo, subject, body, fileName, notificationId);
            return response;
        }

        private async Task<MemoryStream> GetCsvAsMemoryStream(IEnumerable<InvoiceReportCsvViewModel> transactionsArray)
        {
            var memoryStream = new MemoryStream();
            var flatFileWriter = new StreamWriter(memoryStream, Encoding.ASCII);
            var fileWriterEngine = new FileHelperEngine(typeof(InvoiceReportCsvViewModel));

            fileWriterEngine.HeaderText = Resource.InvoiceReportCsvHeaderText;
            fileWriterEngine.WriteStream(flatFileWriter, transactionsArray);

            // Flush contents of fileWriterStream to underlying docStream:
            await flatFileWriter.FlushAsync();
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
    }
}
