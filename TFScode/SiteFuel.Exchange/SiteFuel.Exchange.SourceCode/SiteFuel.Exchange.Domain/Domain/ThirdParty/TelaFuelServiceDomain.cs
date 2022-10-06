using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Domain.TelaFuelServiceReference;
using SiteFuel.Exchange.EmailManager;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain
{
    public class TelaFuelServiceDomain : BaseDomain
    {
        TelaFuelServiceClient api;
        public TelaFuelServiceDomain(string userName, string password)
           : base(ContextFactory.Current.ConnectionString)
        {
            api = new TelaFuelServiceClient();

            string endpointAddress = string.Format(ConfigurationManager.AppSettings[ApplicationConstants.KeyAppSettingTelaFuelServiceAddress]);
            api.ClientCredentials.UserName.UserName = userName;
            api.ClientCredentials.UserName.Password = password;
            api.Endpoint.Address = new EndpointAddress(new Uri(endpointAddress));
        }

        public TelaFuelServiceDomain(BaseDomain domain)
            : base(domain)
        {
        }
        public int OrderAdd(TelaFuelServiceViewModel telaFuelServiceViewModel)
        {
            int orderId = 0;
            OrderFuel orderFuel = new OrderFuel();
            DateTime startTime = new DateTime();
            DateTime endTime = new DateTime();

            try
            {

                orderFuel.BillToName = telaFuelServiceViewModel.BillToName;
                orderFuel.BillToType = TelaFuelServiceReference.BillToTypeEnum.Unknown;
                orderFuel.CarrierLookup = telaFuelServiceViewModel.CarrierLookup;
                orderFuel.FreightLaneLookup = telaFuelServiceViewModel.FreightLaneLookup;
                orderFuel.IsDelivered = telaFuelServiceViewModel.IsDelivered;
                orderFuel.OrderStatus = TelaFuelServiceReference.OrderStatusEnum.New;
                orderFuel.OrderType = TelaFuelServiceReference.OrderTypeEnum.Unknown;
                orderFuel.ReferenceNumber = telaFuelServiceViewModel.ReferenceNumber;
                // Lift Data
                if (telaFuelServiceViewModel.OrderLifts != null)
                {
                    orderFuel.OrderLifts = telaFuelServiceViewModel.OrderLifts.Select(t => new TelaFuelServiceReference.OrderLift()
                    {
                        BillOfLadingNumber = t.BillOfLadingNumber,
                        LiftDateTime = DateTime.SpecifyKind(t.LiftDateTime.Value.DateTime, DateTimeKind.Utc),
                        LiftDateTimeLocal = t.LiftDateTimeLocal.Value.DateTime,
                        LiftDateTimeLocalTimeZone = t.LiftDateTimeLocalTimeZone,
                        TerminalLookup = t.TerminalName,
                        SupplierLookup = t.SupplierLookup,
                        SequenceNumber = t.SequenceNumber,
                        LiftProducts = t.LiftProducts.Select(t1 => new TelaFuelServiceReference.LiftProduct()
                        {
                            GrossQuantity = t1.GrossQuantity,
                            NetQuantity = t1.NetQuantity,
                            ProductLookup = t1.ProductLookup,
                            TMWProductId = t1.TMWProductId,
                            SequenceNumber = t1.SequenceNumber
                        }).ToArray()
                    }).ToArray();
                }
                // Drop data
                if (telaFuelServiceViewModel.OrderDrops != null)
                {
                    orderFuel.OrderDrops = telaFuelServiceViewModel.OrderDrops.Select(t => new TelaFuelServiceReference.OrderDrop()
                    {
                        DroppedDateTime = DateTime.SpecifyKind(t.DroppedDateTime.Value.DateTime, DateTimeKind.Utc),
                        DroppedDateTimeLocal = t.DroppedDateTimeLocal.Value.DateTime,
                        DroppedDateTimeLocalTimeZone = t.DroppedDateTimeLocalTimeZone,
                        EarliestDateTime = DateTime.SpecifyKind(t.EarliestDateTime.DateTime, DateTimeKind.Utc),
                        EarliestDateTimeLocal = t.EarliestDateTimeLocal.DateTime,
                        EarliestDateTimeLocalTimeZone = t.EarliestDateTimeLocalTimeZone,
                        LatestDateTime = DateTime.SpecifyKind(t.LatestDateTime.DateTime, DateTimeKind.Utc),
                        LatestDateTimeLocal = t.LatestDateTime.DateTime,
                        LatestDateTimeLocalTimeZone = t.LatestDateTimeLocalTimeZone,
                        ScheduleDateTime = DateTime.SpecifyKind(t.ScheduleDateTime.Value.DateTime, DateTimeKind.Utc),
                        ScheduledDateTimeLocal = t.ScheduledDateTimeLocal.Value.DateTime,
                        ScheduledDateTimeLocalTimeZone = t.ScheduledDateTimeLocalTimeZone,
                        SequenceNumber = t.SequenceNumber,
                        SiteLookup = t.SiteLookup,
                        TMWSiteId = t.TMWSiteId,
                        DropProducts = t.DropProducts.Select(t1 => new TelaFuelServiceReference.DropProduct()
                        {
                            GrossQuantity = t1.GrossQuantity,
                            NetQuantity = t1.NetQuantity,
                            ProductLookup = t1.ProductLookup,
                            TMWProductId = t1.TMWProductId,
                            TankLookup = t1.TankLookup,
                            SourceLiftSequenceNumber = t1.SourceLiftSequenceNumber,
                            TankNumber = t1.TankNumber,
                            TMWTankId = t1.TMWTankId,
                            OrderQuantity = t1.OrderQuantity,
                        }).ToArray()
                    }).ToArray();
                }
                startTime = DateTime.UtcNow;
                orderId = api.OrderAdd(orderFuel);

            }
            catch (Exception ex)
            {
                SendTelaFuelExceptionNotification(ex, telaFuelServiceViewModel);
                throw;
            }
            finally
            {
                string requestJson = JsonConvert.SerializeObject(orderFuel);
                string responseJson = JsonConvert.SerializeObject(orderId);
                endTime = DateTime.UtcNow;
                TelaFuelServiceLogs(requestJson, responseJson, "OrderAdd", startTime, endTime);
            }
            return orderId;
        }

        public List<string> GetOrdersByStatus(DateTime orderStartDate, DateTime orderEndDate)
        {
            DateTime startTime = new DateTime();
            DateTime endTime = new DateTime();
            List<string> orderIds = new List<string>();
            try
            {
                startTime = DateTime.UtcNow;
                orderStartDate = orderStartDate.Date.AddDays(-1);
                orderEndDate = orderEndDate.Date.AddDays(1);
                var telafuelOrderIds = api.OrdersGetByStatus(OrderStatusEnum.Dispatched, orderStartDate.Date, orderEndDate.Date);
                orderIds = telafuelOrderIds.ToList();


            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("TelaFuelServiceDomain", "GetOrdersByStatus", ex.Message, ex);
                throw;
            }
            finally
            {
                var inputJson = new { OrderStartDate = orderStartDate, OrderEndDate = orderEndDate, status = OrderStatusEnum.Dispatched };
                string requestJson = JsonConvert.SerializeObject(inputJson);
                string responseJson = JsonConvert.SerializeObject(orderIds);
                endTime = DateTime.UtcNow;
                TelaFuelServiceLogs(requestJson, responseJson, "GetOrdersByStatus", startTime, endTime);

            }
            return orderIds;
        }

        public int GetTelaFuelServiceOrderNumber(int orderId, ConsolidatedInvoicePdfViewModel consolidatedInvoicePdfViewModel, string supplierId)
        {
            DateTime startTime = new DateTime();
            DateTime endTime = new DateTime();
            OrderFuel orderFuel = new OrderFuel();
            bool isValidOrder = false;
            int telaFuelServiceOrderNumber = 0;
            try
            {
                startTime = DateTime.UtcNow;
                orderFuel = api.OrderGetByOrderNumber(orderId);


                foreach (var invoice in consolidatedInvoicePdfViewModel.Invoices)
                {
                    isValidOrder = orderFuel.OrderDrops.Any(t => t.TMWSiteId == invoice.JobName
                                                   && t.DropProducts.Any(t1 => t1.TMWProductId == invoice.FuelType))
                                                   && orderFuel.BillToName == consolidatedInvoicePdfViewModel.InvoicePdfHeaderDetail.BuyerCompanyName
                                                   && orderFuel.OrderLifts.All(t => t.TMWSupplierId == supplierId);
                }

                if (isValidOrder)
                {
                    telaFuelServiceOrderNumber = orderFuel.OrderNumber;
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("TelaFuelServiceDomain", "GetTelaFuelServiceOrderNumber", ex.Message, ex);
                throw;
            }
            finally
            {
                string requestJson = JsonConvert.SerializeObject(orderId);
                string responseJson = JsonConvert.SerializeObject(orderFuel);
                endTime = DateTime.UtcNow;
                TelaFuelServiceLogs(requestJson, responseJson, "GetTelaFuelServiceOrderNumber", startTime, endTime);
            }
            return telaFuelServiceOrderNumber;
        }

        public void TelaFuelServiceLogs(string requestJson, string responseJson, string actionName, DateTime startTime, DateTime endTime)
        {
            try
            {
                double TotalMilliseconds = (endTime - startTime).TotalMilliseconds;
                LogManager.Logger.WriteAPIInfo("", "TelaFuelServiceDomain", actionName, requestJson, responseJson, TotalMilliseconds, "", startTime, endTime);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("TelaFuelServiceDomain", "AddOrderAddLog", ex.Message, ex);
            }

        }

        private void SendTelaFuelExceptionNotification(Exception ex, TelaFuelServiceViewModel telaFuelServiceViewModel)
        {
            try
            {
                string siteLookup;
                string productLookup;
                string tankLookup;
                string invoiceNumber;

                siteLookup = telaFuelServiceViewModel?.OrderDrops[0].SiteLookup;
                tankLookup = telaFuelServiceViewModel?.OrderDrops[0]?.DropProducts?[0].TankLookup;
                productLookup = telaFuelServiceViewModel?.OrderDrops[0]?.DropProducts?[0].ProductLookup;
                invoiceNumber = telaFuelServiceViewModel?.ReferenceNumber;

                if (!string.IsNullOrEmpty(siteLookup) && !string.IsNullOrEmpty(tankLookup) && !string.IsNullOrEmpty(productLookup) && !string.IsNullOrEmpty(invoiceNumber))
                {
                    var helperDomain = new HelperDomain(this);
                    var notificationDomain = new NotificationDomain(this);
                    var serverUrl = helperDomain.GetServerUrl();
                    var notification = notificationDomain.GetNotificationContent(EventSubType.TelFuelException, serverUrl, string.Empty);
                    var emailTemplate = helperDomain.GetApplicationEventNotificationTemplate();
                    string telaFuelExceptionReceivers = Context.DataContext.MstAppSettings.Where(t => t.Key == ApplicationConstants.KeyAppSettingTelaFuelExceptionEmail).Select(t => t.Value).FirstOrDefault();
                    if (telaFuelExceptionReceivers != null)
                    {
                        List<string> emailReceivers = new List<string>();
                        emailReceivers = JsonConvert.DeserializeObject<List<string>>(telaFuelExceptionReceivers);
                        if (emailReceivers != null && emailReceivers.Any())
                        {
                            var emailModel = new ApplicationEventNotificationViewModel
                            {
                                To = emailReceivers,
                                Subject = string.Format(notification.Subject),
                                CompanyLogo = notification.CompanyLogo,
                                BodyText = string.Format(notification.BodyText, siteLookup, productLookup, tankLookup, invoiceNumber, ex.Message),
                                ShowFooterContent = false,
                                ShowHelpLineInfo = false,
                                ShowUserSettingsLink = false
                            };
                            Task.Run(() => new EmailDomain(this).SendEmail(emailTemplate, emailModel));
                        }
                    }
                }

            }
            catch (Exception ex1)
            {

                LogManager.Logger.WriteException("TelaFuelServiceDomain", "SendTelaFuelExceptionNotification", ex1.Message, ex1);
            }
        }
    }
}
