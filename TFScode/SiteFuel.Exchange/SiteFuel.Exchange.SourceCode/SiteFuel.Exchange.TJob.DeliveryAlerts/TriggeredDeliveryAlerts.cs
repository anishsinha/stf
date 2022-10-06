using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using System;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.TJob.DeliveryAlerts
{
    public class TriggeredDeliveryAlerts
    {
        public TriggeredDeliveryAlerts()
        {
            using (var tracer = new Tracer("TriggeredDeliveryAlerts", "TriggeredDeliveryAlerts"))
            {
                //Register Context
                ContextFactory.Register(new ApplicationContext());
            }
        }

        public async Task<bool> ProcessDeliveryAlters()
        {
            using (var tracer = new Tracer("TriggeredDeliveryAlerts", "ProcessDeliveryAlters"))
            {
                try
                {
                    //Start your delivery alert logic here
                    await StartProcessingDeliveryAlerts();

                    // Invoice approval reminders logic here
                    await StartProcessingInvoiceApprovals();

                    //Not onboarded driver assign to order - supplier reminder
                    await StartProcessingNotOnboardedDriver();
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("TriggeredDeliveryAlerts", "ProcessDeliveryAlters", "Exception Details : ", ex);
                }
                return true;
            }
        }

        private async Task StartProcessingDeliveryAlerts()
        {
            using (var tracer = new Tracer("TriggeredDeliveryAlerts", "StartProcessingDeliveryAlerts"))
            {
                try
                {
                    var deliveryIds = await ContextFactory.Current.GetDomain<OrderDomain>().GetDeliveryRequestIdsAync();
                    foreach (var deliveryId in deliveryIds)
                    {
                        await ProcessDeliveryAlerts(deliveryId);
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("TriggeredDeliveryAlerts", "StartProcessingDeliveries", "Exception Details : ", ex);
                }
            }
        }

        private async Task StartProcessingInvoiceApprovals()
        {
            using (var tracer = new Tracer("TriggeredDeliveryAlerts", "StartProcessingInvoiceApprovals"))
            {
                try
                {
                    await ContextFactory.Current.GetDomain<InvoiceDomain>().ProcessInvoiceApprovalReminders();
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("TriggeredDeliveryAlerts", "StartProcessingInvoiceApprovalReminders", "Exception Details : ", ex);
                }
            }
        }

        private async Task ProcessDeliveryAlerts(int id)
        {
            using (var tracer = new Tracer("TriggeredDeliveryAlerts", "ProcessDeliveryAlerts"))
            {
                try
                {
                    await ContextFactory.Current.GetDomain<OrderDomain>().ProcessDeliveryScheduleReminderAsync(id);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("TriggeredDeliveryAlerts", "ProcessDeliveryAlerts", "Exception Details : ", ex);
                }
            }
        }

        private async Task StartProcessingNotOnboardedDriver()
        {
            try
            {
                await ContextFactory.Current.GetDomain<DispatchDomain>().ProcessInActiveDriverNotification();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("TriggeredDeliveryAlerts", "StartProcessingNotOnboardedDriver", "Exception Details : ", ex);
            }
        }
    }
}
