using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Domain.Domain.ThirdParty;
using SiteFuel.Exchange.Logger;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.CJob.DeliveryAlerts
{
    public class ProcessWaitingForTaxEntity
    {
        public ProcessWaitingForTaxEntity()
        {
            //Register Context
            ContextFactory.Register(new ApplicationContext());
        }

        public async Task ProcessWaitingForTaxDdt()
        {
            var watch = Stopwatch.StartNew();
            try
            {
                await GenerateInvoicesWaitingForAvalaraTax();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ProcessWaitingForTaxEntity", "ProcessDeliveryAlters", "Exception Details : ", ex);
            }
            watch.Stop();
            LogManager.Logger.WriteInfo("CJob.ProcessWaitingForTaxEntity", "ProcessWaitingForTaxDdt", "End:TotalTime:" + watch.ElapsedMilliseconds);
        }

        private async Task GenerateInvoicesWaitingForAvalaraTax()
        {
            int generatedInvoices = 0;
            using (var tracer = new Tracer("ProcessWaitingForTaxEntity", "GenerateInvoicesWhichAreWaitingForAvalaraTax"))
            {
                try
                {
                    generatedInvoices = await ContextFactory.Current.GetDomain<ConsolidatedDdtToInvoiceDomain>().ProcessInvoicesWaitingForTax();
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("ProcessWaitingForTaxEntity", "GenerateInvoicesWaitingForAvalaraTax", "Exception Details : ", ex);
                }
                LogManager.Logger.WriteInfo("ProcessWaitingForTaxEntity", "GenerateInvoicesWaitingForAvalaraTax", $"Number of Invoices generated = {generatedInvoices}");
            }
        }

        public async Task ProcessFailedLocationCreateRequests()
        {
            var watch = Stopwatch.StartNew();
            try
            {
                var result = await ContextFactory.Current.GetDomain<SAPEnterpriseDomain>().ProcessFailedLocationCreateRequests();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ProcessWaitingForTaxEntity", "ProcessFailedLocationCreateRequests", "Exception Details : ", ex);
            }
            watch.Stop();
            LogManager.Logger.WriteInfo("CJob.ProcessWaitingForTaxEntity", "ProcessFailedLocationCreateRequests", "End:TotalTime:" + watch.ElapsedMilliseconds);
        }

    }
}
