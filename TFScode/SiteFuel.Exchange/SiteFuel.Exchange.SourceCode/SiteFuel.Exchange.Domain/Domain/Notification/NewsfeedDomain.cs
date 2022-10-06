using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.NewsfeedRequest;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain
{
    public class NewsfeedDomain : BaseDomain
    {
        public NewsfeedDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public NewsfeedDomain(SiteFuelUow SiteFuelDbContext) : base(SiteFuelDbContext)
        {
        }

        public NewsfeedDomain(BaseDomain domain) : base(domain)
        {
        }

        public async Task SetSystemDeliveryScheduleMissedNewsfeed(int orderId, int jobId, int jobCompanyId, int acceptedCompanyId, string jobTimeZoneName, string poNumber)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = orderId,
                EntityType = EntityType.Order,
                EventTypeId = NewsfeedEvent.SystemOrderDeliveryScheduleMissed,
                CreatedByUserId = (int)SystemUser.System,
                BuyerCompanyId = jobCompanyId,
                SupplierCompanyId = acceptedCompanyId,
                TargetEntityId = orderId,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(jobTimeZoneName)
            };
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, poNumber);
            var newsfeed = Context.DataContext.Newsfeeds.Where(t => t.EventId == (int)newsfeedParameters.EventTypeId && t.TargetEntityId == orderId
                                                                    && t.EntityId == orderId && (t.RecipientCompanyId == jobCompanyId
                                                                    || t.RecipientCompanyId == acceptedCompanyId));
            if (newsfeed.Any())
            {
                Context.DataContext.Newsfeeds.RemoveRange(newsfeed);
                await Context.CommitAsync();
            }
            await SetNewsfeed(newsfeedParameters);
            await SetJobNewsfeed(newsfeedParameters, jobId);
        }

        public async Task SetFuelSurchargeEnableOrDisabledNewsfeed(int orderId, int buyerCompanyId, string jobTimeZoneName, string poNumber, bool isEnabled, UserContext userContext)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = orderId,
                EntityType = EntityType.Order,
                EventTypeId = isEnabled ? NewsfeedEvent.FuelSurchargeEnabled : NewsfeedEvent.FuelSurchargeDisabled,
                CreatedByUserId = userContext.Id,
                BuyerCompanyId = buyerCompanyId,
                SupplierCompanyId = userContext.CompanyId,
                TargetEntityId = orderId,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(jobTimeZoneName)
            };
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierCompany, userContext.CompanyName);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, poNumber);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
            await SetNewsfeed(newsfeedParameters);
        }

        public async Task SetSystemInvoiceWaitingForApprovalNewsfeed(Invoice invoice, User approvalUser)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = invoice.OrderId.Value,
                EntityType = EntityType.Order,
                CreatedByUserId = (int)SystemUser.System,
                BuyerCompanyId = approvalUser.Company.Id,
                SupplierCompanyId = invoice.Order.AcceptedCompanyId,
                TargetEntityId = invoice.Id,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(invoice.Order.FuelRequest.Job.TimeZoneName)
            };
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.ApprovalPerson, $"{approvalUser.FirstName} {approvalUser.LastName}");
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.BuyerCompany, approvalUser.Company.Name);
            if (invoice.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketManual || invoice.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp)
            {
                newsfeedParameters.EventTypeId = NewsfeedEvent.SystemDigitalDropTicketWaitingforApproval;
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.DdtNumber, invoice.DisplayInvoiceNumber.Replace(ApplicationConstants.SFIN, ApplicationConstants.SFDD));
            }
            else
            {
                newsfeedParameters.EventTypeId = NewsfeedEvent.SystemInvoiceWaitingforApproval;
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.InvoiceNumber, invoice.DisplayInvoiceNumber);
            }
            var newsfeed = Context.DataContext.Newsfeeds.Where(t => t.EventId == (int)newsfeedParameters.EventTypeId && t.TargetEntityId == invoice.Id
                                                                    && t.EntityId == invoice.OrderId.Value && (t.RecipientCompanyId == approvalUser.Company.Id
                                                                    || t.RecipientCompanyId == invoice.Order.AcceptedCompanyId));
            if (newsfeed.Any())
            {
                Context.DataContext.Newsfeeds.RemoveRange(newsfeed);
                await Context.CommitAsync();
            }
            await SetNewsfeed(newsfeedParameters);

            newsfeedParameters.EntityId = invoice.InvoiceHeaderId;
            newsfeedParameters.EntityType = newsfeedParameters.EventTypeId == NewsfeedEvent.SystemDigitalDropTicketWaitingforApproval ? EntityType.DigitalDropTicket : EntityType.Invoice;
            await SetNewsfeed(newsfeedParameters);

            await SetJobNewsfeed(newsfeedParameters, invoice.Order.FuelRequest.Job.Id);
        }

        public async Task SetSupplierCreatedDDTWaitingForApprovalNewsfeed(Invoice invoice, User approvalUser, NewsfeedEvent newsfeedEvent)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = invoice.OrderId.Value,
                EntityType = EntityType.Order,
                EventTypeId = newsfeedEvent,
                CreatedByUserId = invoice.CreatedBy,
                BuyerCompanyId = approvalUser.Company.Id,
                SupplierCompanyId = invoice.Order.AcceptedCompanyId,
                TargetEntityId = invoice.Id,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(invoice.Order.FuelRequest.Job.TimeZoneName)
            };
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, $"{invoice.Order.User.FirstName} {invoice.Order.User.LastName}");
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierCompany, invoice.Order.User.Company.Name);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.DdtNumber, invoice.DisplayInvoiceNumber.Replace(ApplicationConstants.SFIN, ApplicationConstants.SFDD));
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, invoice.PoNumber);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.ApprovalPerson, $"{approvalUser.FirstName} {approvalUser.LastName}");
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.BuyerCompany, approvalUser.Company.Name);
            await SetNewsfeed(newsfeedParameters);

            newsfeedParameters.EntityId = invoice.InvoiceHeaderId;
            newsfeedParameters.EntityType = EntityType.DigitalDropTicket;
            await SetNewsfeed(newsfeedParameters);

            await SetJobNewsfeed(newsfeedParameters, invoice.Order.FuelRequest.Job.Id);
        }

        public async Task SetSupplierCreatedDDTWaitingForUpdatedPriceNewsfeed(UserContext userContext, ManualInvoiceViewModel viewModel)
        {
            var order = Context.DataContext.Orders.Where(t => t.Id == viewModel.OrderId).FirstOrDefault();
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = viewModel.OrderId,
                EntityType = EntityType.Order,
                EventTypeId = NewsfeedEvent.SupplierCreatedDDTWaitingForUpdatedPrice,
                CreatedByUserId = userContext.Id,
                BuyerCompanyId = viewModel.BuyerCompanyId,
                SupplierCompanyId = userContext.CompanyId,
                TargetEntityId = viewModel.InvoiceId,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(viewModel.TimeZoneName)
            };
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierCompany, userContext.CompanyName);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.DdtNumber, viewModel.InvoiceNumber.Number.Replace(ApplicationConstants.SFIN, ApplicationConstants.SFDD));
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, order.PoNumber);
            await SetNewsfeed(newsfeedParameters);

            newsfeedParameters.EntityId = viewModel.InvoiceHeaderId;
            newsfeedParameters.EntityType = EntityType.DigitalDropTicket;
            await SetNewsfeed(newsfeedParameters);
        }

        public async Task SetSupplierCreatedDDTWaitingForTaxesNewsfeed(UserContext userContext, ManualInvoiceViewModel viewModel)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = viewModel.OrderId,
                EntityType = EntityType.Order,
                EventTypeId = NewsfeedEvent.DDTCreatedWaitingForTaxes,
                CreatedByUserId = userContext.Id,
                BuyerCompanyId = viewModel.BuyerCompanyId,
                SupplierCompanyId = userContext.CompanyId,
                TargetEntityId = viewModel.InvoiceId,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(viewModel.TimeZoneName)
            };
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierCompany, userContext.CompanyName);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.DdtNumber, viewModel.InvoiceNumber.Number.Replace(ApplicationConstants.SFIN, ApplicationConstants.SFDD));
            await SetNewsfeed(newsfeedParameters);

            newsfeedParameters.EntityId = viewModel.InvoiceHeaderId;
            newsfeedParameters.EntityType = EntityType.DigitalDropTicket;
            await SetNewsfeed(newsfeedParameters);
        }

        private async Task SetDDTWaitingForApprovalNewsfeed(DigitalDropTicketApprovalNewsfeedModel viewModel, NewsfeedEvent newsfeedEvent)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = viewModel.OrderId,
                EntityType = EntityType.Order,
                EventTypeId = newsfeedEvent,
                CreatedByUserId = viewModel.CreatedBy,
                BuyerCompanyId = viewModel.ApprovalUserCompanyId,
                SupplierCompanyId = viewModel.SupplierCompanyId,
                TargetEntityId = viewModel.InvoiceId,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(viewModel.TimeZoneName)
            };
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, viewModel.UserName);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierCompany, viewModel.SupplierCompanyName);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.DdtNumber, viewModel.InvoiceNumber);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, viewModel.PoNumber);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.ApprovalPerson, viewModel.ApprovalUserName);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.BuyerCompany, viewModel.ApprovalUserCompany);
            await SetNewsfeed(newsfeedParameters);

            newsfeedParameters.EntityId = viewModel.InvoiceHeaderId;
            newsfeedParameters.EntityType = EntityType.DigitalDropTicket;
            await SetNewsfeed(newsfeedParameters);

            await SetJobNewsfeed(newsfeedParameters, viewModel.JobId);
        }

        public async Task SetDDTWaitingForNewsfeed(UserContext userContext, DigitalDropTicketNewsfeedModel viewModel, NewsfeedEvent newsfeedEvent)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = viewModel.OrderId,
                EntityType = EntityType.Order,
                EventTypeId = newsfeedEvent,
                CreatedByUserId = userContext.Id,
                BuyerCompanyId = viewModel.BuyerCompanyId,
                SupplierCompanyId = viewModel.SupplierCompanyId,
                TargetEntityId = viewModel.InvoiceId,
                CreatedDate = viewModel.CreatedDate
            };
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierCompany, userContext.CompanyName);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.DdtNumber, viewModel.InvoiceNumber);
            if (newsfeedParameters.EventTypeId == NewsfeedEvent.SupplierCreatedDDTWaitingForUpdatedPrice)
            {
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, viewModel.PoNumber);
            }
            await SetNewsfeed(newsfeedParameters);

            newsfeedParameters.EntityId = viewModel.InvoiceHeaderId;
            newsfeedParameters.EntityType = EntityType.DigitalDropTicket;
            await SetNewsfeed(newsfeedParameters);
        }

        public async Task SetDDTWaitingForPrePostDipDataNewsfeed(UserContext userContext, DigitalDropTicketNewsfeedModel viewModel, NewsfeedEvent newsfeedEvent)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = viewModel.OrderId,
                EntityType = EntityType.Order,
                EventTypeId = newsfeedEvent,
                CreatedByUserId = userContext.Id,
                BuyerCompanyId = viewModel.BuyerCompanyId,
                SupplierCompanyId = viewModel.SupplierCompanyId,
                TargetEntityId = viewModel.InvoiceId,
                CreatedDate = viewModel.CreatedDate
            };
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.PoNumber, viewModel.PoNumber);
            //  newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierCompany, userContext.CompanyName);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.DdtNumber, viewModel.InvoiceNumber);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.Date, viewModel.CreatedDate.ToString());
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.TargetEntityId,viewModel.InvoiceHeaderId.ToString());
            await SetNewsfeed(newsfeedParameters);

            newsfeedParameters.EntityId = viewModel.InvoiceHeaderId;
            newsfeedParameters.EntityType = EntityType.DigitalDropTicket;
            await SetNewsfeed(newsfeedParameters);
        }


        public async Task SetDdtToInvoiceWaitingForTaxesNewsfeed(UserContext userContext, int orderId, int invoiceId, string invoiceNumber, int invoiceHeaderId)
        {
            var order = Context.DataContext.Orders.Where(t => t.Id == orderId).FirstOrDefault();
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = orderId,
                EntityType = EntityType.Order,
                EventTypeId = NewsfeedEvent.DdtToInvoiceWaitingForTaxes,
                CreatedByUserId = userContext.Id,
                BuyerCompanyId = order.BuyerCompanyId,
                SupplierCompanyId = userContext.CompanyId,
                TargetEntityId = invoiceId,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(order.FuelRequest.Job.TimeZoneName)
            };
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierCompany, userContext.CompanyName);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.DdtNumber, invoiceNumber);
            await SetNewsfeed(newsfeedParameters);

            newsfeedParameters.EntityId = invoiceHeaderId;
            newsfeedParameters.EntityType = EntityType.DigitalDropTicket;
            await SetNewsfeed(newsfeedParameters);
        }

        public async Task SetAnotherCustomerWaitingForTaxesNewsfeed(Order order, InvoiceViewModel invoiceViewModel, int userId)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = invoiceViewModel.InvoiceHeaderId,
                EntityType = EntityType.DigitalDropTicket,
                EventTypeId = NewsfeedEvent.SupplierAssignDDTToOrderWaitingForTaxes,
                CreatedByUserId = userId,
                BuyerCompanyId = order.FuelRequest.User.CompanyId.Value,
                SupplierCompanyId = order.AcceptedCompanyId,
                TargetEntityId = invoiceViewModel.Id,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(order.FuelRequest.Job.TimeZoneName)
            };

            var currentUser = Context.DataContext.Users.First(t => t.Id == invoiceViewModel.UserId);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, $"{currentUser.FirstName} {currentUser.LastName}");
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.DdtNumber, invoiceViewModel.InvoiceNumber.Number.Replace(ApplicationConstants.SFIN, ApplicationConstants.SFDD));
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, order.PoNumber);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierCompany, currentUser.Company.Name);
            await SetNewsfeed(newsfeedParameters);

            newsfeedParameters.EntityId = order.Id;
            newsfeedParameters.EntityType = EntityType.Order;
            await SetNewsfeed(newsfeedParameters);
        }

        public async Task SetInvoiceGeneratedEstablishConnectionWithAvalaraNewsfeed(int supplierCompanyId, ManualInvoiceViewModel viewModel, string ddtNumber, int ddtId, int ddtHeaderId)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = viewModel.InvoiceHeaderId,
                EntityType = EntityType.Invoice,
                EventTypeId = NewsfeedEvent.InvoiceGeneratedAfterEstablishConnectionWithAvalara,
                CreatedByUserId = (int)SystemUser.System,
                BuyerCompanyId = viewModel.BuyerCompanyId,
                SupplierCompanyId = supplierCompanyId,
                TargetEntityId = viewModel.InvoiceId,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(viewModel.TimeZoneName)
            };

            newsfeedParameters.MessageParameters.Add(ApplicationConstants.DdtNumber, ddtNumber);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.InvoiceNumber, viewModel.InvoiceNumber.Number);
            await SetNewsfeed(newsfeedParameters);

            newsfeedParameters.EntityId = viewModel.OrderId;
            newsfeedParameters.EntityType = EntityType.Order;
            await SetNewsfeed(newsfeedParameters);

            newsfeedParameters.EntityId = ddtHeaderId;
            newsfeedParameters.EntityType = EntityType.DigitalDropTicket;
            await SetNewsfeed(newsfeedParameters);
        }

        public async Task SetSupplierInvoiceDDTCreatedNewsfeed(Invoice invoice, User approvalUser)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = invoice.OrderId.Value,
                EntityType = EntityType.Order,
                CreatedByUserId = invoice.CreatedBy,
                SupplierCompanyId = invoice.Order.AcceptedCompanyId,
                TargetEntityId = invoice.Id,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(invoice.Order.FuelRequest.Job.TimeZoneName)
            };

            if (invoice.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketManual || invoice.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp)
            {
                newsfeedParameters.EventTypeId = NewsfeedEvent.SupplierDigitalDropTicketCreated;
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.DdtNumber, invoice.DisplayInvoiceNumber.Replace(ApplicationConstants.SFIN, ApplicationConstants.SFDD));
            }
            else
            {
                newsfeedParameters.EventTypeId = NewsfeedEvent.SupplierInvoiceCreated;
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.InvoiceNumber, invoice.DisplayInvoiceNumber);
            }

            newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierCompany, invoice.Order.User.Company.Name);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, $"{invoice.Order.User.FirstName} {invoice.Order.User.LastName}");
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, invoice.PoNumber);
            await SetNewsfeed(newsfeedParameters);

            newsfeedParameters.EntityId = invoice.InvoiceHeaderId;
            newsfeedParameters.EntityType = EntityType.DigitalDropTicket;
            await SetNewsfeed(newsfeedParameters);
        }

        public async Task SetSystemInvoiceCreatedNewsfeed(Order order, InvoiceViewModel invoiceViewModel)
        {
            var driver = Context.DataContext.Users.First(t => t.Id == invoiceViewModel.UserId);
            var fueldeliveryDetails = order.FuelRequest.FuelRequestDetail;

            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = order.Id,
                EntityType = EntityType.Order,
                EventTypeId = fueldeliveryDetails.DeliveryTypeId == (int)DeliveryType.OneTimeDelivery ? NewsfeedEvent.SystemInvoiceCreatedSingleDelivery : NewsfeedEvent.SystemInvoiceCreated,
                CreatedByUserId = (int)SystemUser.System,
                BuyerCompanyId = order.FuelRequest.User.Company.Id,
                SupplierCompanyId = order.AcceptedCompanyId,
                TargetEntityId = invoiceViewModel.Id,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(order.FuelRequest.Job.TimeZoneName)
            };
            if (order.DefaultInvoiceType == (int)InvoiceType.DigitalDropTicketManual || invoiceViewModel.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp)
            {
                invoiceViewModel.InvoiceNumber.Number = invoiceViewModel.InvoiceNumber.Number.Replace(ApplicationConstants.SFIN, ApplicationConstants.SFDD);
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.DdtNumber, invoiceViewModel.InvoiceNumber.Number);
                newsfeedParameters.EventTypeId = fueldeliveryDetails.DeliveryTypeId == (int)DeliveryType.OneTimeDelivery ? NewsfeedEvent.SystemDDTCreatedSingleDelivery : NewsfeedEvent.SystemDigitalDropTicketCreated;
            }
            else
            {
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.InvoiceNumber, invoiceViewModel.InvoiceNumber.Number);
            }
            var quantityDelivered = ($"{invoiceViewModel.DroppedGallons.GetPreciseValue(2).GetCommaSeperatedValue()} {invoiceViewModel.UoM}").ToLower();
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierCompany, driver.Company.Name);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.DriverName, $"{driver.FirstName} {driver.LastName}");
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.Gallons, quantityDelivered);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.Date, invoiceViewModel.DropStartDate.ToString(Resource.constFormatDate));
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.StartTime, invoiceViewModel.DropStartDate.ToString(Resource.constFormat12HourTime));
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.EndTime, invoiceViewModel.DropEndDate.ToString(Resource.constFormat12HourTime));
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, order.PoNumber);

            if (newsfeedParameters.EventTypeId == NewsfeedEvent.SystemDDTCreatedSingleDelivery || newsfeedParameters.EventTypeId == NewsfeedEvent.SystemInvoiceCreatedSingleDelivery)
            {
                var avgGallonsPercentagePerDelivery = new HelperDomain(this).GetAverageFuelDropPercentagePerOrder(order);
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.DropPercent, avgGallonsPercentagePerDelivery.GetPreciseValue(2).ToString());
            }

            await SetNewsfeed(newsfeedParameters);

            newsfeedParameters.EntityId = invoiceViewModel.InvoiceHeaderId;
            newsfeedParameters.EntityType = (newsfeedParameters.EventTypeId == NewsfeedEvent.SystemDigitalDropTicketCreated || newsfeedParameters.EventTypeId == NewsfeedEvent.SystemDDTCreatedSingleDelivery) ? EntityType.DigitalDropTicket : EntityType.Invoice;
            await SetNewsfeed(newsfeedParameters);

            await SetJobNewsfeed(newsfeedParameters, order.FuelRequest.Job.Id);
        }

        public async Task SetSystemInvoiceCreatedNewsfeed(SystemInvoiceCreatedNewsfeedModel viewModel)
        {
            var driver = Context.DataContext.Users.Where(t => t.Id == viewModel.DriverId)
                        .Select(t => new { CompanyName = t.Company.Name, t.FirstName, t.LastName }).First();
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = viewModel.OrderId,
                EntityType = EntityType.Order,
                EventTypeId = viewModel.DeliveryTypeId == (int)DeliveryType.OneTimeDelivery ? NewsfeedEvent.SystemInvoiceCreatedSingleDelivery : NewsfeedEvent.SystemInvoiceCreated,
                CreatedByUserId = (int)SystemUser.System,
                BuyerCompanyId = viewModel.BuyerCompanyId,
                SupplierCompanyId = viewModel.SupplierCompanyId,
                TargetEntityId = viewModel.InvoiceId,
                CreatedDate = viewModel.CreatedDate
            };
            if (viewModel.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp)
            {
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.DdtNumber, viewModel.InvoiceNumber);
                newsfeedParameters.EventTypeId = viewModel.DeliveryTypeId == (int)DeliveryType.OneTimeDelivery ? NewsfeedEvent.SystemDDTCreatedSingleDelivery : NewsfeedEvent.SystemDigitalDropTicketCreated;
            }
            else
            {
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.InvoiceNumber, viewModel.InvoiceNumber);
            }
            var quantityDelivered = ($"{viewModel.DroppedGallons.GetPreciseValue(2).GetCommaSeperatedValue()} {viewModel.UoM}").ToLower();
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierCompany, driver.CompanyName);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.DriverName, $"{driver.FirstName} {driver.LastName}");
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.Gallons, quantityDelivered);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.Date, viewModel.DropStartDate.ToString(Resource.constFormatDate));
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.StartTime, viewModel.DropStartDate.ToString(Resource.constFormat12HourTime));
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.EndTime, viewModel.DropEndDate.ToString(Resource.constFormat12HourTime));
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, viewModel.PoNumber);

            if (newsfeedParameters.EventTypeId == NewsfeedEvent.SystemDDTCreatedSingleDelivery || newsfeedParameters.EventTypeId == NewsfeedEvent.SystemInvoiceCreatedSingleDelivery)
            {
                var avgGallonsPercentagePerDelivery = new HelperDomain(this).GetFuelDropPercentage(viewModel.DroppedGallons, viewModel.OrderMaxQuantity);
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.DropPercent, avgGallonsPercentagePerDelivery.GetPreciseValue(2).ToString());
            }

            await SetNewsfeed(newsfeedParameters);

            newsfeedParameters.EntityId = viewModel.InvoiceHeaderId;
            newsfeedParameters.EntityType = (newsfeedParameters.EventTypeId == NewsfeedEvent.SystemDigitalDropTicketCreated || newsfeedParameters.EventTypeId == NewsfeedEvent.SystemDDTCreatedSingleDelivery) ? EntityType.DigitalDropTicket : EntityType.Invoice;
            await SetNewsfeed(newsfeedParameters);

            if (viewModel.BuyerCompanyId == viewModel.JobCompanyId)
            {
                await SetJobNewsfeed(newsfeedParameters, viewModel.JobId);
            }
        }

        public async Task SetDriverDroppedInvoiceCreatedWaitingForNewsfeed(Order order, InvoiceViewModel invoiceViewModel, NewsfeedEvent newsfeedEvent)
        {
            var driver = Context.DataContext.Users.First(t => t.Id == invoiceViewModel.UserId);

            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = invoiceViewModel.InvoiceHeaderId,
                EntityType = EntityType.DigitalDropTicket,
                EventTypeId = newsfeedEvent,
                CreatedByUserId = (int)SystemUser.System,
                BuyerCompanyId = order.FuelRequest.User.Company.Id,
                SupplierCompanyId = order.AcceptedCompanyId,
                TargetEntityId = invoiceViewModel.Id,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(order.FuelRequest.Job.TimeZoneName)
            };

            var quantityDelivered = ($"{invoiceViewModel.DroppedGallons.GetPreciseValue(2).GetCommaSeperatedValue()} {invoiceViewModel.UoM}").ToLower();
            invoiceViewModel.InvoiceNumber.Number = invoiceViewModel.InvoiceNumber.Number.Replace(ApplicationConstants.SFIN, ApplicationConstants.SFDD);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.DdtNumber, invoiceViewModel.InvoiceNumber.Number);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierCompany, driver.Company.Name);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.DriverName, $"{driver.FirstName} {driver.LastName}");
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.Gallons, quantityDelivered);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.Date, invoiceViewModel.DropStartDate.ToString(Resource.constFormatDate));
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.StartTime, invoiceViewModel.DropStartDate.ToString(Resource.constFormat12HourTime));
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.EndTime, invoiceViewModel.DropEndDate.ToString(Resource.constFormat12HourTime));
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, order.PoNumber);
            if (newsfeedEvent == NewsfeedEvent.DriverDropsWaitingForApproval)
            {
                var approvalUser = order.FuelRequest.Job.JobXApprovalUsers.FirstOrDefault(t => t.IsActive).User;
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.ApprovalPerson, $"{approvalUser.FirstName} {approvalUser.LastName}");
            }
            await SetNewsfeed(newsfeedParameters);

            newsfeedParameters.EntityId = order.Id;
            newsfeedParameters.EntityType = EntityType.Order;
            await SetNewsfeed(newsfeedParameters);
        }

        public async Task SetDriverDroppedInvoiceCreatedWaitingForNewsfeed(DigitalDropTicketNewsfeedModel viewModel, NewsfeedEvent newsfeedEvent)
        {
            var driver = Context.DataContext.Users.Where(t => t.Id == viewModel.DriverId)
                        .Select(t => new { CompanyName = t.Company.Name, t.FirstName, t.LastName }).First();

            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = viewModel.InvoiceHeaderId,
                EntityType = EntityType.DigitalDropTicket,
                EventTypeId = newsfeedEvent,
                CreatedByUserId = (int)SystemUser.System,
                BuyerCompanyId = viewModel.BuyerCompanyId,
                SupplierCompanyId = viewModel.SupplierCompanyId,
                TargetEntityId = viewModel.InvoiceId,
                CreatedDate = viewModel.CreatedDate
            };

            var quantityDelivered = ($"{viewModel.DroppedGallons.GetPreciseValue(2).GetCommaSeperatedValue()} {viewModel.UoM}").ToLower();
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.DdtNumber, viewModel.InvoiceNumber);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierCompany, driver.CompanyName);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.DriverName, $"{driver.FirstName} {driver.LastName}");
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.Gallons, quantityDelivered);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.Date, viewModel.DropStartDate.ToString(Resource.constFormatDate));
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.StartTime, viewModel.DropStartDate.ToString(Resource.constFormat12HourTime));
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.EndTime, viewModel.DropEndDate.ToString(Resource.constFormat12HourTime));
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, viewModel.PoNumber);
            if (newsfeedEvent == NewsfeedEvent.DriverDropsWaitingForApproval)
            {
                if (viewModel.ApprovalUserId > 0)
                {
                    var approvalUser = Context.DataContext.Users.Where(t => t.Id == viewModel.ApprovalUserId).Select(t => new { t.FirstName, t.LastName }).First();
                    newsfeedParameters.MessageParameters.Add(ApplicationConstants.ApprovalPerson, $"{approvalUser.FirstName} {approvalUser.LastName}");
                }
                else
                {
                    newsfeedParameters.MessageParameters.Add(ApplicationConstants.ApprovalPerson, string.Empty);
                }
            }

            await SetNewsfeed(newsfeedParameters);

            newsfeedParameters.EntityId = viewModel.OrderId;
            newsfeedParameters.EntityType = EntityType.Order;
            await SetNewsfeed(newsfeedParameters);
        }

        public async Task SetSystemDigitalDropTicketDraftCreatedNewsfeed(Order order, InvoiceViewModel invoiceViewModel)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = invoiceViewModel.InvoiceHeaderId,
                EntityType = EntityType.DigitalDropTicket,
                EventTypeId = NewsfeedEvent.SupplierDigitalDropTicketDraftCreated,
                CreatedByUserId = (int)SystemUser.System,
                BuyerCompanyId = 0,
                SupplierCompanyId = order.AcceptedCompanyId,
                TargetEntityId = invoiceViewModel.Id,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(order.FuelRequest.Job.TimeZoneName)
            };

            var driver = Context.DataContext.Users.First(t => t.Id == invoiceViewModel.UserId);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.DriverName, $"{driver.FirstName} {driver.LastName}");
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.Date, invoiceViewModel.DropStartDate.ToString(Resource.constFormatDate));
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.StartTime, invoiceViewModel.DropStartDate.ToString(Resource.constFormat12HourTime));
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.EndTime, invoiceViewModel.DropEndDate.ToString(Resource.constFormat12HourTime));
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, order.PoNumber);
            invoiceViewModel.InvoiceNumber.Number = invoiceViewModel.InvoiceNumber.Number.Replace(ApplicationConstants.SFIN, ApplicationConstants.SFDD);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.DdtNumber, invoiceViewModel.InvoiceNumber.Number);
            await SetNewsfeed(newsfeedParameters);

            newsfeedParameters.EntityId = order.Id;
            newsfeedParameters.EntityType = EntityType.Order;
            newsfeedParameters.EventTypeId = NewsfeedEvent.SupplierDigitalDropTicketDraftCreated;
            await SetNewsfeed(newsfeedParameters);
        }

        public async Task SetSystemDigitalDropTicketDraftCreatedNewsfeed(DigitalDropTicketDraftNewsfeedModel viewModel)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = viewModel.InvoiceHeaderId,
                EntityType = EntityType.DigitalDropTicket,
                EventTypeId = NewsfeedEvent.SupplierDigitalDropTicketDraftCreated,
                CreatedByUserId = (int)SystemUser.System,
                BuyerCompanyId = 0,
                SupplierCompanyId = viewModel.SupplierCompanyId,
                TargetEntityId = viewModel.InvoiceId,
                CreatedDate = viewModel.CreatedDate
            };

            var driver = Context.DataContext.Users.First(t => t.Id == viewModel.DriverId);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.DriverName, $"{driver.FirstName} {driver.LastName}");
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.Date, viewModel.DropStartDate.ToString(Resource.constFormatDate));
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.StartTime, viewModel.DropStartDate.ToString(Resource.constFormat12HourTime));
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.EndTime, viewModel.DropEndDate.ToString(Resource.constFormat12HourTime));
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, viewModel.PoNumber);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.DdtNumber, viewModel.InvoiceNumber);
            await SetNewsfeed(newsfeedParameters);

            newsfeedParameters.EntityId = viewModel.OrderId;
            newsfeedParameters.EntityType = EntityType.Order;
            newsfeedParameters.EventTypeId = NewsfeedEvent.SupplierDigitalDropTicketDraftCreated;
            await SetNewsfeed(newsfeedParameters);
        }

        public async Task SetDigitalDropTicketDraftConvertedNewsfeed(Order order, InvoiceViewModel invoiceViewModel)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = invoiceViewModel.InvoiceHeaderId,
                EntityType = EntityType.DigitalDropTicket,
                EventTypeId = NewsfeedEvent.SupplierDigitalDropTicketDraftConverted,
                CreatedByUserId = invoiceViewModel.UserId,
                BuyerCompanyId = 0,
                SupplierCompanyId = order.AcceptedCompanyId,
                TargetEntityId = invoiceViewModel.Id,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(order.FuelRequest.Job.TimeZoneName)
            };

            var driver = Context.DataContext.Users.First(t => t.Id == invoiceViewModel.UserId);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, $"{driver.FirstName} {driver.LastName}");
            invoiceViewModel.InvoiceNumber.Number = invoiceViewModel.InvoiceNumber.Number.Replace(ApplicationConstants.SFIN, ApplicationConstants.SFDD);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.DdtNumber, invoiceViewModel.InvoiceNumber.Number);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.BuyerCompany, order.FuelRequest.User.Company.Name);
            await SetNewsfeed(newsfeedParameters);

            newsfeedParameters.EntityId = order.Id;
            newsfeedParameters.EntityType = EntityType.Order;
            newsfeedParameters.EventTypeId = NewsfeedEvent.SupplierDigitalDropTicketDraftConverted;
            await SetNewsfeed(newsfeedParameters);
        }

        public async Task SetDigitalDropTicketCanceledNewsfeed(UserContext userContext, Invoice invoice, string timeZoneName)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = invoice.InvoiceHeaderId,
                EntityType = EntityType.DigitalDropTicket,
                EventTypeId = NewsfeedEvent.SupplierDigitalDropTicketCanceled,
                CreatedByUserId = userContext.Id,
                BuyerCompanyId = 0,
                SupplierCompanyId = userContext.CompanyId,
                TargetEntityId = invoice.Id,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(timeZoneName)
            };
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
            await SetNewsfeed(newsfeedParameters);

            newsfeedParameters.EntityId = invoice.OrderId.Value;
            newsfeedParameters.EntityType = EntityType.Order;
            newsfeedParameters.EventTypeId = NewsfeedEvent.SupplierOrderDigitalDropTicketCanceled;
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.DdtNumber, invoice.DisplayInvoiceNumber);
            await SetNewsfeed(newsfeedParameters);
        }

        public async Task SetUnassignedDigitalDropTicketCreatedNewsfeed(InvoiceViewModel invoiceViewModel)
        {
            var driver = Context.DataContext.Users.SingleOrDefault(t => t.Id == invoiceViewModel.UserId);

            if (driver != null && driver.CompanyId.HasValue)
            {
                var newsfeedParameters = new NewsfeedParameters
                {
                    EntityId = invoiceViewModel.InvoiceHeaderId,
                    EntityType = EntityType.DigitalDropTicket,
                    EventTypeId = NewsfeedEvent.SupplierUnassignedDigitalDropTicketCreated,
                    CreatedByUserId = invoiceViewModel.UserId,
                    BuyerCompanyId = 0,
                    SupplierCompanyId = driver.CompanyId.HasValue ? driver.CompanyId.Value : 0,
                    TargetEntityId = invoiceViewModel.Id,
                    CreatedDate = invoiceViewModel.CreatedDate
                };

                var quantityDelivered = ($"{invoiceViewModel.DroppedGallons.GetPreciseValue(2).GetCommaSeperatedValue()} {invoiceViewModel.UoM}").ToLower();
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.DriverName, $"{driver.FirstName} {driver.LastName}");
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.Gallons, quantityDelivered);
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.Date, invoiceViewModel.DropStartDate.ToString(Resource.constFormatDate));
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.StartTime, invoiceViewModel.DropStartDate.ToString(Resource.constFormat12HourTime));
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.EndTime, invoiceViewModel.DropEndDate.ToString(Resource.constFormat12HourTime));
                string invNumber = invoiceViewModel.InvoiceNumber.Number;
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.DdtNumber, invNumber.Replace(ApplicationConstants.SFIN, ApplicationConstants.SFDD));
                await SetNewsfeed(newsfeedParameters);
            }
        }

        public async Task SetAssignDigitalDropTicketToOrderNewsfeed(Order order, InvoiceViewModel invoiceViewModel, int userId)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = invoiceViewModel.InvoiceHeaderId,
                EntityType = EntityType.DigitalDropTicket,
                CreatedByUserId = userId,
                BuyerCompanyId = 0,
                SupplierCompanyId = order.AcceptedCompanyId,
                TargetEntityId = invoiceViewModel.Id,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(order.FuelRequest.Job.TimeZoneName)
            };

            var currentUser = Context.DataContext.Users.First(t => t.Id == userId);
            var driver = Context.DataContext.Users.First(t => t.Id == invoiceViewModel.DriverId);
            string invNumber = invoiceViewModel.InvoiceNumber.Number;
            string ddtNumber = invNumber.Replace(ApplicationConstants.SFIN, ApplicationConstants.SFDD);
            var quantityDelivered = ($"{invoiceViewModel.DroppedGallons.GetPreciseValue(2).GetCommaSeperatedValue()} {invoiceViewModel.UoM}").ToLower();

            var fueldeliveryDetails = order.FuelRequest.FuelRequestDetail;
            if (fueldeliveryDetails.DeliveryTypeId == (int)DeliveryType.OneTimeDelivery)
            {
                var currentDate = DateTimeOffset.Now.ToTargetDateTimeOffset(order.FuelRequest.Job.TimeZoneName);
                var closeDate = fueldeliveryDetails.StartDate.Add(fueldeliveryDetails.EndTime);
                if (closeDate.DateTime >= currentDate.DateTime) //// not expired yet
                {
                    //// 3.3 Newsfeed Supplier after linking on Order
                    newsfeedParameters.EntityId = order.Id;
                    newsfeedParameters.EntityType = EntityType.Order;
                    newsfeedParameters.EventTypeId = NewsfeedEvent.SupplierAssignDDTToOrder;
                    newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, $"{currentUser.FirstName} {currentUser.LastName}");
                    newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, order.PoNumber);
                    newsfeedParameters.MessageParameters.Add(ApplicationConstants.DriverName, $"{driver.FirstName} {driver.LastName}");
                    newsfeedParameters.MessageParameters.Add(ApplicationConstants.Gallons, quantityDelivered);
                    newsfeedParameters.MessageParameters.Add(ApplicationConstants.Date, invoiceViewModel.DropStartDate.ToString(Resource.constFormatDate));
                    newsfeedParameters.MessageParameters.Add(ApplicationConstants.StartTime, invoiceViewModel.DropStartDate.ToString(Resource.constFormat12HourTime));
                    newsfeedParameters.MessageParameters.Add(ApplicationConstants.EndTime, invoiceViewModel.DropEndDate.ToString(Resource.constFormat12HourTime));

                    var avgGallonsPercentagePerDelivery = new HelperDomain(this).GetAverageFuelDropPercentagePerOrder(order);
                    newsfeedParameters.MessageParameters.Add(ApplicationConstants.DropPercent, avgGallonsPercentagePerDelivery.GetPreciseValue(2).ToString());
                    await SetNewsfeed(newsfeedParameters);

                    if (order.FuelRequest.FuelRequestTypeId == (int)FuelRequestType.ThirdPartyRequest)
                    {
                        newsfeedParameters.EntityId = invoiceViewModel.InvoiceHeaderId;
                        newsfeedParameters.EntityType = EntityType.DigitalDropTicket;
                        await SetNewsfeed(newsfeedParameters);
                    }

                    ////3.4 Newsfeed Buyer after linking on DDT & Order:
                    if (order.DefaultInvoiceType == (int)InvoiceType.DigitalDropTicketManual || invoiceViewModel.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp)
                    {
                        newsfeedParameters.EventTypeId = NewsfeedEvent.SystemDDTCreatedSingleDelivery;
                        newsfeedParameters.MessageParameters.Add(ApplicationConstants.DdtNumber, ddtNumber);
                    }
                    else
                    {
                        newsfeedParameters.EventTypeId = NewsfeedEvent.SystemInvoiceCreatedSingleDelivery;
                        newsfeedParameters.MessageParameters.Add(ApplicationConstants.InvoiceNumber, invNumber);
                    }
                    //// linking to DDT newsfeed
                    newsfeedParameters.EntityId = invoiceViewModel.InvoiceHeaderId;
                    newsfeedParameters.EntityType = EntityType.DigitalDropTicket;
                    newsfeedParameters.BuyerCompanyId = order.FuelRequest.User.Company.Id;
                    newsfeedParameters.SupplierCompanyId = 0;
                    newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierCompany, driver.Company.Name);
                    newsfeedParameters.MessageParameters.Remove(ApplicationConstants.UserName);
                    await SetNewsfeed(newsfeedParameters);

                    //// linking to Order newsfeed
                    newsfeedParameters.EntityId = order.Id;
                    newsfeedParameters.EntityType = EntityType.Order;
                    await SetNewsfeed(newsfeedParameters);
                }
                else //// expired
                {
                    //// 4.1 Newsfeed Buyer/Supplier on Order before linking
                    newsfeedParameters.EntityId = order.Id;
                    newsfeedParameters.EntityType = EntityType.Order;
                    newsfeedParameters.BuyerCompanyId = order.FuelRequest.User.Company.Id;
                    newsfeedParameters.EventTypeId = NewsfeedEvent.OrderClosedOnDateExpired;
                    newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, order.PoNumber);
                    await SetNewsfeed(newsfeedParameters);

                    //// 4.3 Newsfeed Supplier after linking on DDT
                    newsfeedParameters.MessageParameters.Add(ApplicationConstants.DriverName, $"{driver.FirstName} {driver.LastName}");
                    newsfeedParameters.MessageParameters.Add(ApplicationConstants.Gallons, quantityDelivered);
                    newsfeedParameters.MessageParameters.Add(ApplicationConstants.Date, invoiceViewModel.DropStartDate.ToString(Resource.constFormatDate));
                    newsfeedParameters.MessageParameters.Add(ApplicationConstants.StartTime, invoiceViewModel.DropStartDate.ToString(Resource.constFormat12HourTime));
                    newsfeedParameters.MessageParameters.Add(ApplicationConstants.EndTime, invoiceViewModel.DropEndDate.ToString(Resource.constFormat12HourTime));

                    newsfeedParameters.EntityId = invoiceViewModel.InvoiceHeaderId;
                    newsfeedParameters.EntityType = EntityType.DigitalDropTicket;
                    newsfeedParameters.EventTypeId = NewsfeedEvent.SupplierDDTCreatedAfterDateExpiredSingleDelivery;
                    newsfeedParameters.BuyerCompanyId = order.FuelRequest.User.Company.Id;
                    newsfeedParameters.SupplierCompanyId = order.AcceptedCompanyId;
                    newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, $"{currentUser.FirstName} {currentUser.LastName}");
                    var avgGallonsPercentagePerDelivery = new HelperDomain(this).GetAverageFuelDropPercentagePerOrder(order);
                    newsfeedParameters.MessageParameters.Add(ApplicationConstants.DropPercent, avgGallonsPercentagePerDelivery.GetPreciseValue(2).ToString());
                    await SetNewsfeed(newsfeedParameters);

                    if (order.DefaultInvoiceType == (int)InvoiceType.DigitalDropTicketManual || invoiceViewModel.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp)
                    {
                        newsfeedParameters.EventTypeId = NewsfeedEvent.SupplierAssignDDTToOrderAfterDateExpired;
                        newsfeedParameters.MessageParameters.Add(ApplicationConstants.DdtNumber, ddtNumber);
                    }
                    else
                    {
                        newsfeedParameters.EventTypeId = NewsfeedEvent.SupplierAssignInvoiceToOrderAfterDateExpired;
                        newsfeedParameters.MessageParameters.Add(ApplicationConstants.InvoiceNumber, invNumber);
                    }
                    //// 4.4 Newsfeed Buyer after linking
                    newsfeedParameters.EntityId = invoiceViewModel.InvoiceHeaderId;
                    newsfeedParameters.EntityType = EntityType.DigitalDropTicket;
                    newsfeedParameters.BuyerCompanyId = order.FuelRequest.User.Company.Id;
                    newsfeedParameters.SupplierCompanyId = 0;
                    newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierCompany, driver.Company.Name);
                    newsfeedParameters.MessageParameters.Remove(ApplicationConstants.UserName);
                    await SetNewsfeed(newsfeedParameters);
                }
            }
            else
            {
                var invoiceIds = Context.DataContext.Invoices.Where(t => t.InvoiceHeader.InvoiceNumberId == invoiceViewModel.InvoiceNumber.Id && t.IsActive).Select(t => t.InvoiceHeaderId).ToList();
                if (invoiceIds.Count > 0)
                {
                    var invoiceNewsfeeds = Context.DataContext.Newsfeeds.Where(t => invoiceIds.Contains(t.EntityId) && (t.EntityTypeId == (int)EntityType.Invoice || t.EntityTypeId == (int)EntityType.DigitalDropTicket)).ToList();
                    invoiceNewsfeeds.ForEach(t => { t.EntityId = invoiceViewModel.InvoiceHeaderId; });
                }

                newsfeedParameters.EventTypeId = NewsfeedEvent.SupplierAssignDigitalDropTicketToOrder;
                newsfeedParameters.BuyerCompanyId = order.FuelRequest.User.Company.Id;
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, $"{currentUser.FirstName} {currentUser.LastName}");
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.DdtNumber, ddtNumber);
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, order.PoNumber);

                newsfeedParameters.MessageParameters.Add(ApplicationConstants.DriverName, $"{driver.FirstName} {driver.LastName}");
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierCompany, driver.Company.Name);
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.Gallons, quantityDelivered);
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.Date, invoiceViewModel.DropStartDate.ToString(Resource.constFormatDate));
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.StartTime, invoiceViewModel.DropStartDate.ToString(Resource.constFormat12HourTime));
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.EndTime, invoiceViewModel.DropEndDate.ToString(Resource.constFormat12HourTime));
                await SetNewsfeed(newsfeedParameters);
            }
        }
        public async Task SetAssignDDTToOrderWhenApprovalWFEnabledNewsfeed(Order order, InvoiceViewModel invoiceViewModel, int userId)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = invoiceViewModel.InvoiceHeaderId,
                EntityType = EntityType.DigitalDropTicket,
                CreatedByUserId = userId,
                EventTypeId = NewsfeedEvent.SupplierAssignDDTToOrderWhenApprovalWFEnabled,
                BuyerCompanyId = order.FuelRequest.User.Company.Id,
                SupplierCompanyId = order.AcceptedCompanyId,
                TargetEntityId = invoiceViewModel.Id,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(order.FuelRequest.Job.TimeZoneName)
            };

            var currentUser = Context.DataContext.Users.First(t => t.Id == invoiceViewModel.UserId);
            var driver = Context.DataContext.Users.First(t => t.Id == invoiceViewModel.DriverId);
            var approvalUser = order.FuelRequest.Job.JobXApprovalUsers.FirstOrDefault(t => t.IsActive).User;
            var quantityDelivered = ($"{invoiceViewModel.DroppedGallons.GetPreciseValue(2).GetCommaSeperatedValue()} {invoiceViewModel.UoM}").ToString();

            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, $"{currentUser.FirstName} {currentUser.LastName}");
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.DdtNumber, invoiceViewModel.InvoiceNumber.Number);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, order.PoNumber);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.DriverName, $"{driver.FirstName} {driver.LastName}");
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.ApprovalPerson, $"{approvalUser.FirstName} {approvalUser.LastName}");
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.BuyerCompany, order.FuelRequest.User.Company.Name);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierCompany, driver.Company.Name);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.Gallons, quantityDelivered);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.Date, invoiceViewModel.DropStartDate.ToString(Resource.constFormatDate));
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.StartTime, invoiceViewModel.DropStartDate.ToString(Resource.constFormat12HourTime));
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.EndTime, invoiceViewModel.DropEndDate.ToString(Resource.constFormat12HourTime));
            await SetNewsfeed(newsfeedParameters);

            newsfeedParameters.EntityId = order.Id;
            newsfeedParameters.EntityType = EntityType.Order;
            var invoiceIds = Context.DataContext.Invoices.Where(t => t.InvoiceHeader.InvoiceNumberId == invoiceViewModel.InvoiceNumber.Id && t.IsActive).Select(t => t.InvoiceHeaderId).ToList();
            if (invoiceIds.Count > 0)
            {
                var invoiceNewsfeeds = Context.DataContext.Newsfeeds.Where(t => invoiceIds.Contains(t.EntityId) && (t.EntityTypeId == (int)EntityType.Invoice || t.EntityTypeId == (int)EntityType.DigitalDropTicket)).ToList();
                invoiceNewsfeeds.ForEach(t => { t.EntityId = invoiceViewModel.InvoiceHeaderId; });
            }
            await SetNewsfeed(newsfeedParameters);
        }

        public async Task SetAssignDDTToOrderWaitingForPriceSettingInvoiceNewsfeed(Order order, InvoiceViewModel invoiceViewModel, int userId)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = invoiceViewModel.InvoiceHeaderId,
                EntityType = EntityType.DigitalDropTicket,
                CreatedByUserId = userId,
                EventTypeId = NewsfeedEvent.SupplierAssignDDTToOrderWaitingForUpdatedPrice,
                BuyerCompanyId = order.FuelRequest.User.Company.Id,
                SupplierCompanyId = order.AcceptedCompanyId,
                TargetEntityId = invoiceViewModel.Id,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(order.FuelRequest.Job.TimeZoneName)
            };

            var currentUser = Context.DataContext.Users.First(t => t.Id == invoiceViewModel.UserId);
            var driver = Context.DataContext.Users.First(t => t.Id == invoiceViewModel.DriverId);
            var quantityDelivered = ($"{invoiceViewModel.DroppedGallons.GetPreciseValue(2).GetCommaSeperatedValue()} {invoiceViewModel.UoM}").ToString();

            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, $"{currentUser.FirstName} {currentUser.LastName}");
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.DdtNumber, invoiceViewModel.InvoiceNumber.Number.Replace(ApplicationConstants.SFIN, ApplicationConstants.SFDD));
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, order.PoNumber);

            newsfeedParameters.MessageParameters.Add(ApplicationConstants.DriverName, $"{driver.FirstName} {driver.LastName}");
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierCompany, driver.Company.Name);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.Gallons, quantityDelivered);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.Date, invoiceViewModel.DropStartDate.ToString(Resource.constFormatDate));
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.StartTime, invoiceViewModel.DropStartDate.ToString(Resource.constFormat12HourTime));
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.EndTime, invoiceViewModel.DropEndDate.ToString(Resource.constFormat12HourTime));

            await SetNewsfeed(newsfeedParameters);

            newsfeedParameters.EntityId = order.Id;
            newsfeedParameters.EntityType = EntityType.Order;
            var invoiceIds = Context.DataContext.Invoices.Where(t => t.InvoiceHeader.InvoiceNumberId == invoiceViewModel.InvoiceNumber.Id && t.IsActive).Select(t => t.InvoiceHeaderId).ToList();
            if (invoiceIds.Count > 0)
            {
                var invoiceNewsfeeds = Context.DataContext.Newsfeeds.Where(t => invoiceIds.Contains(t.EntityId) && (t.EntityTypeId == (int)EntityType.Invoice || t.EntityTypeId == (int)EntityType.DigitalDropTicket)).ToList();
                invoiceNewsfeeds.ForEach(t => { t.EntityId = invoiceViewModel.InvoiceHeaderId; });
            }

            await SetNewsfeed(newsfeedParameters);
        }
        public async Task SetUnassignedDDTToOrderLinkNewsfeed(Order order, User driver, InvoiceViewModel invoiceViewModel, int userId)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = order.Id,
                EntityType = EntityType.Order,
                EventTypeId = NewsfeedEvent.SystemDigitalDropTicketCreated,
                CreatedByUserId = userId,
                BuyerCompanyId = order.FuelRequest.User.Company.Id,
                SupplierCompanyId = 0,
                TargetEntityId = invoiceViewModel.Id,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(order.FuelRequest.Job.TimeZoneName)
            };

            var quantityDelivered = ($"{invoiceViewModel.DroppedGallons.GetPreciseValue(2).GetCommaSeperatedValue()} {invoiceViewModel.UoM}").ToLower();
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.DriverName, $"{driver.FirstName} {driver.LastName}");
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierCompany, driver.Company.Name);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.Gallons, quantityDelivered);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.Date, invoiceViewModel.DropStartDate.ToString(Resource.constFormatDate));
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.StartTime, invoiceViewModel.DropStartDate.ToString(Resource.constFormat12HourTime));
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.EndTime, invoiceViewModel.DropEndDate.ToString(Resource.constFormat12HourTime));
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, order.PoNumber);

            string invNumber = invoiceViewModel.InvoiceNumber.Number;
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.DdtNumber, invNumber);

            await SetNewsfeed(newsfeedParameters);
        }

        public async Task SetUnassignedDDTToOrderInvoiceGenerateBuyerNewsfeed(Order order, User driver, InvoiceViewModel invoiceViewModel, int userId)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = order.Id,
                EntityType = EntityType.Order,
                EventTypeId = NewsfeedEvent.SystemInvoiceCreated,
                CreatedByUserId = userId,
                BuyerCompanyId = order.FuelRequest.User.Company.Id,
                SupplierCompanyId = 0,
                TargetEntityId = invoiceViewModel.Id,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(order.FuelRequest.Job.TimeZoneName)
            };

            var quantityDelivered = ($"{invoiceViewModel.DroppedGallons.GetPreciseValue(2).GetCommaSeperatedValue()} {invoiceViewModel.UoM}").ToLower();
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.DriverName, $"{driver.FirstName} {driver.LastName}");
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierCompany, driver.Company.Name);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.Gallons, quantityDelivered);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.Date, invoiceViewModel.DropStartDate.ToString(Resource.constFormatDate));
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.StartTime, invoiceViewModel.DropStartDate.ToString(Resource.constFormat12HourTime));
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.EndTime, invoiceViewModel.DropEndDate.ToString(Resource.constFormat12HourTime));
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, order.PoNumber);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.InvoiceNumber, invoiceViewModel.InvoiceNumber.Number);

            await SetNewsfeed(newsfeedParameters);

            newsfeedParameters.EntityId = invoiceViewModel.InvoiceHeaderId;
            newsfeedParameters.EntityType = EntityType.Invoice;
            await SetNewsfeed(newsfeedParameters);
        }

        public async Task SetUnassignedDDTToOrderLinkSupplierNewsfeed(Order order, User driver, InvoiceViewModel invoiceViewModel, User currentUser)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = order.Id,
                EntityType = EntityType.Order,
                EventTypeId = NewsfeedEvent.SupplierAssignDigitalDropTicketToOrder,
                CreatedByUserId = currentUser.Id,
                BuyerCompanyId = 0,
                SupplierCompanyId = order.AcceptedCompanyId,
                TargetEntityId = invoiceViewModel.Id,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(order.FuelRequest.Job.TimeZoneName)
            };
            var quantityDelivered = ($"{invoiceViewModel.DroppedGallons.GetPreciseValue(2).GetCommaSeperatedValue()} {invoiceViewModel.UoM}").ToString();
            string invNumber = invoiceViewModel.InvoiceNumber.Number;
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, $"{currentUser.FirstName} {currentUser.LastName}");
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.DdtNumber, invNumber);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, order.PoNumber);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.DriverName, $"{driver.FirstName} {driver.LastName}");
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.Gallons, quantityDelivered);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.Date, invoiceViewModel.DropStartDate.ToString(Resource.constFormatDate));
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.StartTime, invoiceViewModel.DropStartDate.ToString(Resource.constFormat12HourTime));
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.EndTime, invoiceViewModel.DropEndDate.ToString(Resource.constFormat12HourTime));
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierCompany, driver.Company.Name);

            await SetNewsfeed(newsfeedParameters);

            newsfeedParameters.EntityId = invoiceViewModel.InvoiceHeaderId;
            newsfeedParameters.EntityType = EntityType.DigitalDropTicket;
            await SetNewsfeed(newsfeedParameters);
        }

        public async Task SetInvoiceGenerateOnLinkDDTToOrderSupplierNewsfeed(Order order, InvoiceViewModel invoiceViewModel, User currentUser)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = order.Id,
                EntityType = EntityType.Order,
                EventTypeId = NewsfeedEvent.SupplierInvoiceGenerateOnLinkUnassignedDDTToOrder,
                CreatedByUserId = currentUser.Id,
                BuyerCompanyId = 0,
                SupplierCompanyId = order.AcceptedCompanyId,
                TargetEntityId = invoiceViewModel.Id,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(order.FuelRequest.Job.TimeZoneName)
            };
            string invNumber = invoiceViewModel.InvoiceNumber.Number;
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, $"{currentUser.FirstName} {currentUser.LastName}");
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.DdtNumber, invNumber.Replace(ApplicationConstants.SFIN, ApplicationConstants.SFDD));
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, order.PoNumber);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.InvoiceNumber, invoiceViewModel.InvoiceNumber.Number);

            await SetNewsfeed(newsfeedParameters);

            newsfeedParameters.EntityId = invoiceViewModel.InvoiceHeaderId;
            newsfeedParameters.EntityType = EntityType.Invoice;
            newsfeedParameters.TargetEntityId = invoiceViewModel.Id;
            await SetNewsfeed(newsfeedParameters);
        }

        public async Task SetAssetLevelTrackingEnableFromAssignDDTToOrderNewsfeed(Order order, InvoiceViewModel invoiceViewModel, int userId)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = order.FuelRequest.Job.Id,
                EntityType = EntityType.Job,
                EventTypeId = NewsfeedEvent.AssetLevelTrackingEnableFromAssignDDTToOrder,
                CreatedByUserId = userId,
                BuyerCompanyId = order.FuelRequest.Job.CompanyId,
                SupplierCompanyId = 0,
                TargetEntityId = order.FuelRequest.Job.Id,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(order.FuelRequest.Job.TimeZoneName)
            };

            newsfeedParameters.MessageParameters.Add(ApplicationConstants.JobName, order.FuelRequest.Job.Name);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, order.PoNumber);
            string invNumber = invoiceViewModel.InvoiceNumber.Number;
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.DdtNumber, invNumber.Replace(ApplicationConstants.SFIN, ApplicationConstants.SFDD));
            await SetNewsfeed(newsfeedParameters);
        }

        public async Task SetSystemOrderAutoClosedNewsfeed(Order order, decimal totalDelivered)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = order.Id,
                EntityType = EntityType.Order,
                EventTypeId = NewsfeedEvent.SystemOrderClosed,
                CreatedByUserId = (int)SystemUser.System,
                BuyerCompanyId = order.FuelRequest.User.Company.Id,
                SupplierCompanyId = order.AcceptedCompanyId,
                TargetEntityId = order.Id,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(order.FuelRequest.Job.TimeZoneName)
            };
            var quantityDelivered = ($"{totalDelivered.GetPreciseValue(2).GetCommaSeperatedValue()} {order.FuelRequest.UoM}").ToLower();
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, order.PoNumber);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.Gallons, quantityDelivered);
            await SetNewsfeed(newsfeedParameters);

            await SetJobNewsfeed(newsfeedParameters, order.FuelRequest.Job.Id);
        }

        public async Task SetSystemOrderAutoClosedNewsfeed(SystemOrderAutoClosedNewsfeedViewModel viewModel)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = viewModel.OrderId,
                EntityType = EntityType.Order,
                EventTypeId = NewsfeedEvent.SystemOrderClosed,
                CreatedByUserId = (int)SystemUser.System,
                BuyerCompanyId = viewModel.BuyerCompanyId,
                SupplierCompanyId = viewModel.SupplierCompanyId,
                TargetEntityId = viewModel.OrderId,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(viewModel.TimeZoneName)
            };
            var quantityDelivered = ($"{viewModel.TotalDelivered.GetPreciseValue(2).GetCommaSeperatedValue()} {viewModel.UoM}").ToLower();
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, viewModel.PoNumber);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.Gallons, quantityDelivered);
            await SetNewsfeed(newsfeedParameters);
            if (viewModel.JobCompanyId == viewModel.BuyerCompanyId)
            {
                await SetJobNewsfeed(newsfeedParameters, viewModel.JobId);
            }
        }

        public async Task SetInvoiceRejectedNewsfeed(UserContext userContext, Invoice invoice, string declineReason)
        {
            // Buyer Invoice Accepted
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = invoice.OrderId.Value,
                EntityType = EntityType.Order,
                CreatedByUserId = userContext.Id,
                BuyerCompanyId = userContext.CompanyId,
                SupplierCompanyId = invoice.Order.AcceptedCompanyId,
                TargetEntityId = invoice.Id,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(invoice.Order.FuelRequest.Job.TimeZoneName)
            };
            if (invoice.UoM == UoM.Litres)
            {
                declineReason = declineReason.ReplaceGallonToLitre();
            }
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.BuyerCompany, userContext.CompanyName);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.Reason, declineReason);
            if (invoice.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketManual || invoice.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp)
            {
                newsfeedParameters.EventTypeId = NewsfeedEvent.BuyerDigitalDropTicketRejected;
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.DdtNumber, invoice.DisplayInvoiceNumber.Replace(ApplicationConstants.SFIN, ApplicationConstants.SFDD));
            }
            else
            {
                newsfeedParameters.EventTypeId = NewsfeedEvent.BuyerInvoiceRejected;
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.InvoiceNumber, invoice.DisplayInvoiceNumber);
            }
            await SetNewsfeed(newsfeedParameters);

            newsfeedParameters.EntityId = invoice.InvoiceHeaderId;
            newsfeedParameters.EntityType = newsfeedParameters.EventTypeId == NewsfeedEvent.BuyerDigitalDropTicketRejected ? EntityType.DigitalDropTicket : EntityType.Invoice;
            await SetNewsfeed(newsfeedParameters);

            await SetJobNewsfeed(newsfeedParameters, invoice.Order.FuelRequest.Job.Id);
        }

        internal async Task SetNewJobCreationNewsFeed(UserContext userContext, Job job, bool inDraft)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = job.Id,
                EntityType = EntityType.Job,
                CreatedByUserId = userContext.Id,
                BuyerCompanyId = userContext.CompanyId,
                EventTypeId = inDraft ? NewsfeedEvent.DraftJobCreated : NewsfeedEvent.NewJobCreated,
                //SupplierCompanyId = invoice.Order.AcceptedCompanyId,
                TargetEntityId = job.Id,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(job.TimeZoneName)
            };
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.JobName, job.Name);

            await SetNewsfeed(newsfeedParameters);
        }

        public async Task SetInvoiceAcceptedNewsfeed(UserContext userContext, Invoice invoice)
        {
            // Buyer Invoice Accepted
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = invoice.OrderId.Value,
                EntityType = EntityType.Order,
                CreatedByUserId = userContext.Id,
                BuyerCompanyId = invoice.Order.BuyerCompanyId,
                SupplierCompanyId = invoice.Order.AcceptedCompanyId,
                TargetEntityId = invoice.Id,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(invoice.Order.FuelRequest.Job.TimeZoneName)
            };
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.BuyerCompany, userContext.CompanyName);
            if (invoice.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketManual || invoice.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp)
            {
                newsfeedParameters.EventTypeId = NewsfeedEvent.BuyerDigitalDropTicketAccepted;
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.DdtNumber, invoice.DisplayInvoiceNumber.Replace(ApplicationConstants.SFIN, ApplicationConstants.SFDD));
            }
            else
            {
                newsfeedParameters.EventTypeId = NewsfeedEvent.BuyerInvoiceAccepted;
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.InvoiceNumber, invoice.DisplayInvoiceNumber);
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, invoice.PoNumber);
            }
            await SetNewsfeed(newsfeedParameters);

            newsfeedParameters.EntityId = invoice.InvoiceHeaderId;
            newsfeedParameters.EntityType = newsfeedParameters.EventTypeId == NewsfeedEvent.BuyerDigitalDropTicketAccepted ? EntityType.DigitalDropTicket : EntityType.Invoice;
            await SetNewsfeed(newsfeedParameters);

            await SetJobNewsfeed(newsfeedParameters, invoice.Order.FuelRequest.Job.Id);
        }

        public async Task SetInvoiceApprovedNewsfeed(UserContext userContext, Invoice invoice)
        {
            // Buyer Invoice Accepted
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = invoice.OrderId.Value,
                EntityType = EntityType.Order,
                CreatedByUserId = userContext.Id,
                BuyerCompanyId = invoice.Order.BuyerCompanyId,
                SupplierCompanyId = invoice.Order.AcceptedCompanyId,
                TargetEntityId = invoice.Id,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(invoice.Order.FuelRequest.Job.TimeZoneName)
            };
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.BuyerCompany, userContext.CompanyName);
            if (invoice.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketManual || invoice.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp)
            {
                newsfeedParameters.EventTypeId = NewsfeedEvent.BuyerDigitalDropTicketApproved;
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.DdtNumber, invoice.DisplayInvoiceNumber.Replace(ApplicationConstants.SFIN, ApplicationConstants.SFDD));
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, invoice.PoNumber);
            }
            else
            {
                newsfeedParameters.EventTypeId = NewsfeedEvent.BuyerInvoiceApproved;
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.InvoiceNumber, invoice.DisplayInvoiceNumber);
            }
            await SetNewsfeed(newsfeedParameters);

            newsfeedParameters.EntityId = invoice.InvoiceHeaderId;
            newsfeedParameters.EntityType = newsfeedParameters.EventTypeId == NewsfeedEvent.BuyerDigitalDropTicketApproved ? EntityType.DigitalDropTicket : EntityType.Invoice;
            await SetNewsfeed(newsfeedParameters);

            await SetJobNewsfeed(newsfeedParameters, invoice.Order.FuelRequest.Job.Id);

        }

        public async Task SetDdtToInvoiceCreatedNewsfeed(UserContext userContext, ManualInvoiceViewModel viewModel, string ddtNumber)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = viewModel.OrderId,
                EntityType = EntityType.Order,
                EventTypeId = NewsfeedEvent.SupplierDigitalDropTicketConvertedtoInvoice,
                CreatedByUserId = userContext.Id,
                BuyerCompanyId = viewModel.BuyerCompanyId,
                SupplierCompanyId = userContext.CompanyId,
                TargetEntityId = viewModel.InvoiceId,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(viewModel.TimeZoneName)
            };
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierCompany, userContext.CompanyName);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.InvoiceNumber, viewModel.InvoiceNumber.Number);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.DdtNumber, ddtNumber);
            await SetNewsfeed(newsfeedParameters);


            if (viewModel.InvoiceNumberId > 0)
            {
                var ddt = Context.DataContext.Invoices.Where(t => t.OrderId == viewModel.OrderId && t.InvoiceHeader.InvoiceNumberId == viewModel.InvoiceNumberId && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active).Select(t => new { t.InvoiceHeaderId, t.Id }).FirstOrDefault();
                if (ddt.Id > 0)
                {
                    newsfeedParameters.EntityId = ddt.InvoiceHeaderId;
                    newsfeedParameters.TargetEntityId = ddt.Id;
                    newsfeedParameters.EntityType = EntityType.DigitalDropTicket;
                    await SetNewsfeed(newsfeedParameters);
                }
            }
            newsfeedParameters.TargetEntityId = viewModel.InvoiceId;
            newsfeedParameters.EntityId = viewModel.InvoiceHeaderId;
            newsfeedParameters.EntityType = EntityType.Invoice;
            await SetNewsfeed(newsfeedParameters);

            var order = Context.DataContext.Orders.Where(t => t.Id == viewModel.OrderId).FirstOrDefault();
            if (order.BuyerCompanyId == order.FuelRequest.Job.CompanyId)
            {
                await SetJobNewsfeed(newsfeedParameters, viewModel.JobId);
            }
        }

        public async Task SetEddtToInvoiceCreatedNewsfeed(UserContext userContext, EddtToInvoiceCreatedNewsfeedModel viewModel, string ddtNumber)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = viewModel.OrderId,
                EntityType = EntityType.Order,
                EventTypeId = NewsfeedEvent.InvoiceCreatedFromEddt,
                CreatedByUserId = userContext.Id,
                BuyerCompanyId = viewModel.BuyerCompanyId,
                SupplierCompanyId = viewModel.SupplierCompanyId,
                TargetEntityId = viewModel.InvoiceId,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(viewModel.TimeZoneName)
            };
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.CompanyName, userContext.CompanyName);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.InvoiceNumber, viewModel.DisplayInvoiceNumber);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.DdtNumber, ddtNumber);
            await SetNewsfeed(newsfeedParameters);

            newsfeedParameters.TargetEntityId = viewModel.InvoiceId;
            newsfeedParameters.EntityId = viewModel.InvoiceHeaderId;
            newsfeedParameters.EntityType = EntityType.Invoice;
            await SetNewsfeed(newsfeedParameters);

            var order = Context.DataContext.Orders.Where(t => t.Id == viewModel.OrderId).FirstOrDefault();
            if (order.BuyerCompanyId == order.FuelRequest.Job.CompanyId)
            {
                await SetJobNewsfeed(newsfeedParameters, viewModel.JobId);
            }
        }

        public async Task SetEddtToDdtCreatedNewsfeed(UserContext userContext, EddtToInvoiceCreatedNewsfeedModel viewModel, string ddtNumber)
        {
            var eventTypeId = NewsfeedEvent.DdtCreateFromEddt;
            if (viewModel.WaitingFor == (int)WaitingAction.UpdatedPrice)
                eventTypeId = NewsfeedEvent.DdtWiatingForPriceCreatedFromEddt;
            else if (viewModel.WaitingFor == (int)WaitingAction.AvalaraTax)
                eventTypeId = NewsfeedEvent.DdtWiatingForTaxCreatedFromEddt;

            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = viewModel.OrderId,
                EntityType = EntityType.Order,
                EventTypeId = eventTypeId,
                CreatedByUserId = userContext.Id,
                BuyerCompanyId = viewModel.BuyerCompanyId,
                SupplierCompanyId = viewModel.SupplierCompanyId,
                TargetEntityId = viewModel.InvoiceId,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(viewModel.TimeZoneName)
            };
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.CompanyName, userContext.CompanyName);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.InvoiceNumber, viewModel.DisplayInvoiceNumber);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.DdtNumber, ddtNumber);
            await SetNewsfeed(newsfeedParameters);

            newsfeedParameters.TargetEntityId = viewModel.InvoiceId;
            newsfeedParameters.EntityId = viewModel.InvoiceHeaderId;
            newsfeedParameters.EntityType = EntityType.DigitalDropTicket;
            await SetNewsfeed(newsfeedParameters);

            var order = Context.DataContext.Orders.Where(t => t.Id == viewModel.OrderId)
                        .Select(t => new { t.BuyerCompanyId, JobCompanyId = t.FuelRequest.Job.CompanyId }).FirstOrDefault();
            if (order.BuyerCompanyId == order.JobCompanyId)
            {
                await SetJobNewsfeed(newsfeedParameters, viewModel.JobId);
            }
        }

        public async Task SetEddtToAutoInvoiceCreatedNewsfeed(UserContext userContext, EddtToInvoiceCreatedNewsfeedModel viewModel, string ddtNumber)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = viewModel.OrderId,
                EntityType = EntityType.Order,
                EventTypeId = NewsfeedEvent.AutoInvoiceCreatedFromEddt,
                CreatedByUserId = userContext.Id,
                BuyerCompanyId = viewModel.BuyerCompanyId,
                SupplierCompanyId = viewModel.SupplierCompanyId,
                TargetEntityId = viewModel.InvoiceId,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(viewModel.TimeZoneName)
            };
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.InvoiceNumber, viewModel.DisplayInvoiceNumber);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.DdtNumber, ddtNumber);
            await SetNewsfeed(newsfeedParameters);

            newsfeedParameters.TargetEntityId = viewModel.InvoiceId;
            newsfeedParameters.EntityId = viewModel.InvoiceHeaderId;
            newsfeedParameters.EntityType = EntityType.Invoice;
            await SetNewsfeed(newsfeedParameters);

            var order = Context.DataContext.Orders.Where(t => t.Id == viewModel.OrderId)
                        .Select(t => new { t.BuyerCompanyId, JobCompanyId = t.FuelRequest.Job.CompanyId }).FirstOrDefault();
            if (order.BuyerCompanyId == order.JobCompanyId)
            {
                await SetJobNewsfeed(newsfeedParameters, viewModel.JobId);
            }
        }

        public async Task SetEddtToAutoDdtCreatedNewsfeed(UserContext userContext, EddtToInvoiceCreatedNewsfeedModel viewModel, string ddtNumber)
        {
            var eventTypeId = NewsfeedEvent.AutoDdtCreateFromEddt;
            if (viewModel.WaitingFor == (int)WaitingAction.UpdatedPrice)
                eventTypeId = NewsfeedEvent.AutoDdtWiatingForPriceCreatedFromEddt;
            else if (viewModel.WaitingFor == (int)WaitingAction.AvalaraTax)
                eventTypeId = NewsfeedEvent.AutoDdtWiatingForTaxCreatedFromEddt;

            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = viewModel.OrderId,
                EntityType = EntityType.Order,
                EventTypeId = eventTypeId,
                CreatedByUserId = userContext.Id,
                BuyerCompanyId = viewModel.BuyerCompanyId,
                SupplierCompanyId = viewModel.SupplierCompanyId,
                TargetEntityId = viewModel.InvoiceId,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(viewModel.TimeZoneName)
            };
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.InvoiceNumber, viewModel.DisplayInvoiceNumber);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.DdtNumber, ddtNumber);
            await SetNewsfeed(newsfeedParameters);

            newsfeedParameters.TargetEntityId = viewModel.InvoiceId;
            newsfeedParameters.EntityId = viewModel.InvoiceHeaderId;
            newsfeedParameters.EntityType = EntityType.DigitalDropTicket;
            await SetNewsfeed(newsfeedParameters);

            var order = Context.DataContext.Orders.Where(t => t.Id == viewModel.OrderId)
                        .Select(t => new { t.BuyerCompanyId, JobCompanyId = t.FuelRequest.Job.CompanyId }).FirstOrDefault();
            if (order.BuyerCompanyId == order.JobCompanyId)
            {
                await SetJobNewsfeed(newsfeedParameters, viewModel.JobId);
            }
        }

        public async Task SetApprovedDDTNewsfeed(int supplierCompanyId, int buyerId, ManualInvoiceViewModel viewModel, string ddtNumber, int digitalDropTicketId, int ddtHeaderId)
        {
            var userContext = Context.DataContext.Users.First(t => t.Id == buyerId);

            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = viewModel.OrderId,
                EntityType = EntityType.Order,
                EventTypeId = NewsfeedEvent.BuyerApprovedDDT,
                CreatedByUserId = userContext.Id,
                BuyerCompanyId = viewModel.BuyerCompanyId,
                SupplierCompanyId = supplierCompanyId,
                TargetEntityId = viewModel.InvoiceId,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(viewModel.TimeZoneName)
            };
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, $"{userContext.FirstName} {userContext.LastName}");
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.BuyerCompany, viewModel.BuyerCompanyName);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.InvoiceNumber, viewModel.InvoiceNumber.Number);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.DdtNumber, ddtNumber);
            await SetNewsfeed(newsfeedParameters);

            newsfeedParameters.EntityId = viewModel.InvoiceHeaderId;
            newsfeedParameters.EntityType = EntityType.Invoice;
            await SetNewsfeed(newsfeedParameters);

            newsfeedParameters.EntityId = ddtHeaderId;
            newsfeedParameters.EntityType = EntityType.DigitalDropTicket;
            await SetNewsfeed(newsfeedParameters);
        }

        public async Task SetSystemDdtToInvoiceCreatedForUpdatedPriceNewsfeed(int supplierCompanyId, ManualInvoiceViewModel viewModel, string ddtNumber, int digitalDropTicketId, int ddtHeaderId)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = viewModel.OrderId,
                EntityType = EntityType.Order,
                EventTypeId = NewsfeedEvent.DDTToInvoiceWaitingForPrice,
                CreatedByUserId = (int)SystemUser.System,
                BuyerCompanyId = viewModel.BuyerCompanyId,
                SupplierCompanyId = supplierCompanyId,
                TargetEntityId = viewModel.InvoiceId,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(viewModel.TimeZoneName)
            };
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.InvoiceNumber, viewModel.InvoiceNumber.Number);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.DdtNumber, ddtNumber);
            await SetNewsfeed(newsfeedParameters);

            newsfeedParameters.EntityId = viewModel.InvoiceHeaderId;
            newsfeedParameters.EntityType = EntityType.Invoice;
            await SetNewsfeed(newsfeedParameters);

            newsfeedParameters.EntityId = ddtHeaderId;
            newsfeedParameters.EntityType = EntityType.DigitalDropTicket;
            await SetNewsfeed(newsfeedParameters);
        }

        public async Task SetBuyerEnabledHidePricingNewsfeed(UserContext userContext, int orderId, bool isBuyer)
        {
            var order = Context.DataContext.Orders.Where(t => t.Id == orderId).FirstOrDefault();
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = orderId,
                EntityType = EntityType.Order,
                EventTypeId = NewsfeedEvent.BuyerEnabledHidePricing,
                CreatedByUserId = userContext.Id,
                BuyerCompanyId = isBuyer ? userContext.CompanyId : 0,
                SupplierCompanyId = isBuyer ? 0 : order.AcceptedCompanyId,
                TargetEntityId = orderId,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(order.FuelRequest.Job.TimeZoneName)
            };
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, order.PoNumber);
            await SetNewsfeed(newsfeedParameters);
        }

        public async Task SetInvoiceCreatedNewsfeed(UserContext userContext, ManualInvoiceViewModel viewModel, bool isEditInvoice)
        {
            var order = Context.DataContext.Orders.Where(t => t.Id == viewModel.OrderId).FirstOrDefault();
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = viewModel.OrderId,
                EntityType = EntityType.Order,
                EventTypeId = (viewModel.InvoiceTypeId == (int)InvoiceType.Manual || viewModel.InvoiceTypeId == (int)InvoiceType.MobileApp)
                                                            ? NewsfeedEvent.SupplierInvoiceCreated : NewsfeedEvent.SupplierDigitalDropTicketCreated,
                CreatedByUserId = userContext.Id,
                BuyerCompanyId = viewModel.BuyerCompanyId,
                SupplierCompanyId = userContext.CompanyId,
                TargetEntityId = viewModel.InvoiceId,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(viewModel.TimeZoneName)
            };

            var invoiceNumber = viewModel.InvoiceTypeId == (int)InvoiceType.Manual
                                    || viewModel.InvoiceTypeId == (int)InvoiceType.MobileApp
                                    || viewModel.InvoiceTypeId == (int)InvoiceType.DryRun
                                    ? viewModel.InvoiceNumber.Number : viewModel.InvoiceNumber.Number.Replace(ApplicationConstants.SFIN, ApplicationConstants.SFDD);
            if (isEditInvoice)
            {
                newsfeedParameters.EventTypeId = (viewModel.InvoiceTypeId == (int)InvoiceType.Manual || viewModel.InvoiceTypeId == (int)InvoiceType.MobileApp || viewModel.InvoiceTypeId == (int)InvoiceType.DryRun) ? NewsfeedEvent.SupplierInvoiceModified : NewsfeedEvent.SupplierDigitalDropTicketModified;
            }

            var fueldeliveryDetails = order.FuelRequest.FuelRequestDetail;
            if (fueldeliveryDetails.DeliveryTypeId == (int)DeliveryType.OneTimeDelivery && !isEditInvoice)
            {
                newsfeedParameters.EventTypeId = (viewModel.InvoiceTypeId == (int)InvoiceType.Manual || viewModel.InvoiceTypeId == (int)InvoiceType.MobileApp) ? NewsfeedEvent.SupplierManualInvoiceCreatedBeforeDateExpiredSingleDelivery : NewsfeedEvent.SupplierManualDDTCreatedBeforeDateExpiredSingleDelivery;
                var currentDate = DateTimeOffset.Now.ToTargetDateTimeOffset(order.FuelRequest.Job.TimeZoneName);
                var closeDate = fueldeliveryDetails.StartDate.Add(fueldeliveryDetails.EndTime);
                if (closeDate.DateTime >= currentDate.DateTime) //// not expired yet
                {
                    var avgGallonsPercentagePerDelivery = new HelperDomain(this).GetAverageFuelDropPercentagePerOrder(order);
                    newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierCompany, userContext.CompanyName);
                    newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, order.PoNumber);
                    newsfeedParameters.MessageParameters.Add(ApplicationConstants.DropPercent, avgGallonsPercentagePerDelivery.GetPreciseValue(2).ToString());
                    newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);

                    if (newsfeedParameters.EventTypeId == NewsfeedEvent.SupplierManualInvoiceCreatedBeforeDateExpiredSingleDelivery)
                    {
                        //// Invoice
                        newsfeedParameters.MessageParameters.Add(ApplicationConstants.InvoiceNumber, invoiceNumber);
                    }
                    else
                    {
                        //// DDT
                        newsfeedParameters.MessageParameters.Add(ApplicationConstants.DdtNumber, invoiceNumber);
                    }

                    await SetNewsfeed(newsfeedParameters);
                }
                else
                {
                    //// expired
                    //// 6.1 Newsfeed Buyer/Supplier
                    newsfeedParameters.EventTypeId = NewsfeedEvent.OrderClosedOnDateExpired;
                    newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, order.PoNumber);
                    await SetNewsfeed(newsfeedParameters);

                    //// Newsfeed Supplier:
                    newsfeedParameters.EventTypeId = (viewModel.InvoiceTypeId == (int)InvoiceType.Manual || viewModel.InvoiceTypeId == (int)InvoiceType.MobileApp) ? NewsfeedEvent.SupplierManualInvoiceCreatedAfterDateExpiredSingleDelivery : NewsfeedEvent.SupplierManualDDTCreatedAfterDateExpiredSingleDelivery;
                    if (newsfeedParameters.EventTypeId == NewsfeedEvent.SupplierManualInvoiceCreatedAfterDateExpiredSingleDelivery)
                    {
                        //// Invoice
                        newsfeedParameters.MessageParameters.Add(ApplicationConstants.InvoiceNumber, invoiceNumber);
                    }
                    else
                    {
                        //// DDT
                        newsfeedParameters.MessageParameters.Add(ApplicationConstants.DdtNumber, invoiceNumber);
                    }

                    newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierCompany, userContext.CompanyName);
                    newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
                    var avgGallonsPercentagePerDelivery = new HelperDomain(this).GetAverageFuelDropPercentagePerOrder(order);
                    newsfeedParameters.MessageParameters.Add(ApplicationConstants.DropPercent, avgGallonsPercentagePerDelivery.GetPreciseValue(2).ToString());
                    await SetNewsfeed(newsfeedParameters);
                }

                newsfeedParameters.MessageParameters.Remove(ApplicationConstants.OrderNumber);
                newsfeedParameters.MessageParameters.Remove(ApplicationConstants.DropPercent);
                newsfeedParameters.EntityId = viewModel.InvoiceId;
                newsfeedParameters.EventTypeId = viewModel.InvoiceTypeId == (int)InvoiceType.Manual || viewModel.InvoiceTypeId == (int)InvoiceType.MobileApp ? NewsfeedEvent.SupplierInvoiceModified : NewsfeedEvent.SupplierDigitalDropTicketModified;

                if (order.BuyerCompanyId == order.FuelRequest.Job.CompanyId)
                {
                    await SetJobNewsfeed(newsfeedParameters, viewModel.JobId);
                }
            }
            else
            {

                if (newsfeedParameters.EventTypeId == NewsfeedEvent.SupplierInvoiceCreated || newsfeedParameters.EventTypeId == NewsfeedEvent.SupplierInvoiceModified)
                {
                    newsfeedParameters.MessageParameters.Add(ApplicationConstants.InvoiceNumber, invoiceNumber);
                    if (newsfeedParameters.EventTypeId == NewsfeedEvent.SupplierInvoiceCreated)
                    {
                        newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, order.PoNumber);
                    }
                }
                else
                {
                    if (viewModel.WaitingForAction == (int)WaitingAction.UpdatedPrice)
                    {
                        newsfeedParameters.EventTypeId = NewsfeedEvent.DDTCreatedwaitingForUpdatedPrice;
                        newsfeedParameters.MessageParameters.Add(ApplicationConstants.DdtNumber, invoiceNumber);
                    }
                    else
                    {
                        newsfeedParameters.MessageParameters.Add(ApplicationConstants.DdtNumber, invoiceNumber);
                        if (newsfeedParameters.EventTypeId == NewsfeedEvent.SupplierDigitalDropTicketCreated)
                        {
                            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, order.PoNumber);
                        }
                    }
                }

                newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierCompany, userContext.CompanyName);
                await SetNewsfeed(newsfeedParameters);

                if ((newsfeedParameters.EventTypeId == NewsfeedEvent.SupplierInvoiceModified) || (newsfeedParameters.EventTypeId == NewsfeedEvent.SupplierDigitalDropTicketModified))
                {
                    var invoiceIds = Context.DataContext.Invoices.Where(t => t.OrderId == viewModel.OrderId && t.InvoiceHeader.InvoiceNumberId == viewModel.InvoiceNumber.Id && t.IsActive).Select(t => new { t.Id, t.InvoiceHeaderId }).ToList();
                    if (invoiceIds.Count > 0)
                    {
                        var invoiceNewsfeeds = Context.DataContext.Newsfeeds.Where(t => invoiceIds.Any(t1 => t1.InvoiceHeaderId == t.EntityId) && (t.EntityTypeId == (int)EntityType.Invoice || t.EntityTypeId == (int)EntityType.DigitalDropTicket)).ToList();
                        invoiceNewsfeeds.ForEach(t => { t.EntityId = viewModel.InvoiceHeaderId; });
                        var orderNewsfeeds = Context.DataContext.Newsfeeds.Where(t => t.EntityId == viewModel.OrderId && invoiceIds.Any(t1 => t1.Id == t.TargetEntityId) && t.EntityTypeId == (int)EntityType.Order).ToList();
                        orderNewsfeeds.ForEach(t => { t.TargetEntityId = viewModel.InvoiceId; });
                    }
                }

                newsfeedParameters.EntityId = viewModel.InvoiceHeaderId;
                newsfeedParameters.EntityType = (newsfeedParameters.EventTypeId == NewsfeedEvent.SupplierDigitalDropTicketModified || newsfeedParameters.EventTypeId == NewsfeedEvent.SupplierDigitalDropTicketCreated) ? EntityType.DigitalDropTicket : EntityType.Invoice;
                await SetNewsfeed(newsfeedParameters);

                if (order.BuyerCompanyId == order.FuelRequest.Job.CompanyId)
                {
                    await SetJobNewsfeed(newsfeedParameters, viewModel.JobId);
                }
            }
        }

        public async Task SetManualInvoiceCreatedNewsfeed(UserContext userContext, ManualInvoiceCreatedNewsfeedModel viewModel)
        {
            DateTimeOffset currentDate = DateTimeOffset.Now.ToTargetDateTimeOffset(viewModel.TimeZoneName);
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = viewModel.OrderId,
                EntityType = EntityType.Order,
                EventTypeId = viewModel.InvoiceTypeId == (int)InvoiceType.Manual ? NewsfeedEvent.SupplierInvoiceCreated : NewsfeedEvent.SupplierDigitalDropTicketCreated,
                CreatedByUserId = userContext.Id,
                BuyerCompanyId = viewModel.BuyerCompanyId,
                SupplierCompanyId = viewModel.SupplierCompanyId,
                TargetEntityId = viewModel.InvoiceId,
                CreatedDate = currentDate
            };

            if (viewModel.DeliveryTypeId == (int)DeliveryType.OneTimeDelivery)
            {
                await UpdateInvoiceCreatedForSingleDeliveryOrderNewsFeed(userContext, viewModel, newsfeedParameters, currentDate);
            }
            else
            {
                await UpdateInvoiceCreatedForMultipleDeliveryOrderNewsFeed(userContext, viewModel, newsfeedParameters);
            }
        }

        public async Task SetBalanceInvoiceCreatedNewsfeed(UserContext userContext, ManualInvoiceCreatedNewsfeedModel viewModel)
        {
            DateTimeOffset currentDate = DateTimeOffset.Now.ToTargetDateTimeOffset(viewModel.TimeZoneName);
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = viewModel.OrderId,
                EntityType = EntityType.Order,
                EventTypeId = NewsfeedEvent.BalanceInvoiceCreated,
                CreatedByUserId = userContext.Id,
                BuyerCompanyId = viewModel.BuyerCompanyId,
                SupplierCompanyId = viewModel.SupplierCompanyId,
                TargetEntityId = viewModel.InvoiceId,
                CreatedDate = currentDate
            };

            newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierCompany, userContext.CompanyName);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, viewModel.PoNumber);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.InvoiceNumber, viewModel.InvoiceNumber);
            await SetNewsfeed(newsfeedParameters);
            await SetJobNewsfeed(newsfeedParameters, viewModel.JobId);
        }

        public async Task SetCreditInvoiceCreatedNewsfeed(UserContext userContext, ManualInvoiceCreatedNewsfeedModel viewModel)
        {
            DateTimeOffset currentDate = DateTimeOffset.Now.ToTargetDateTimeOffset(viewModel.TimeZoneName);
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = viewModel.OrderId,
                EntityType = EntityType.Order,
                EventTypeId = NewsfeedEvent.CreditInvoiceCreated,
                CreatedByUserId = userContext.Id,
                BuyerCompanyId = viewModel.BuyerCompanyId,
                SupplierCompanyId = viewModel.SupplierCompanyId,
                TargetEntityId = viewModel.InvoiceId,
                CreatedDate = currentDate
            };

            newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierCompany, userContext.CompanyName);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, viewModel.PoNumber);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.InvoiceNumber, viewModel.InvoiceNumber);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OriginalInvoiceNumber, viewModel.OriginalInvoiceNumber);
            await SetNewsfeed(newsfeedParameters);

            newsfeedParameters.EntityId = viewModel.InvoiceHeaderId;
            newsfeedParameters.EntityType = EntityType.Invoice;
            await SetNewsfeed(newsfeedParameters);

            await SetJobNewsfeed(newsfeedParameters, viewModel.JobId);
        }

        public async Task SetRebillInvoiceCreatedNewsfeed(UserContext userContext, ManualInvoiceCreatedNewsfeedModel viewModel)
        {
            DateTimeOffset currentDate = DateTimeOffset.Now.ToTargetDateTimeOffset(viewModel.TimeZoneName);
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = viewModel.OrderId,
                EntityType = EntityType.Order,
                EventTypeId = NewsfeedEvent.CreditAndRebilledInvoiceCreated,
                CreatedByUserId = userContext.Id,
                BuyerCompanyId = viewModel.BuyerCompanyId,
                SupplierCompanyId = viewModel.SupplierCompanyId,
                TargetEntityId = viewModel.InvoiceId,
                CreatedDate = currentDate
            };

            newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierCompany, userContext.CompanyName);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, viewModel.PoNumber);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.InvoiceNumber, viewModel.InvoiceNumber);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OriginalInvoiceNumber, viewModel.OriginalInvoiceNumber);
            await SetNewsfeed(newsfeedParameters);
            await SetJobNewsfeed(newsfeedParameters, viewModel.JobId);
        }

        public async Task SetTankRentalInvoiceCreatedNewsfeed(UserContext userContext, ManualInvoiceCreatedNewsfeedModel viewModel)
        {
            DateTimeOffset currentDate = DateTimeOffset.Now.ToTargetDateTimeOffset(viewModel.TimeZoneName);
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = viewModel.OrderId,
                EntityType = EntityType.Order,
                EventTypeId = NewsfeedEvent.TankRentalInvoiceCreated,
                CreatedByUserId = userContext.Id,
                BuyerCompanyId = viewModel.BuyerCompanyId,
                SupplierCompanyId = viewModel.SupplierCompanyId,
                TargetEntityId = viewModel.InvoiceId,
                CreatedDate = currentDate
            };

            newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierCompany, userContext.CompanyName);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, viewModel.PoNumber);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.InvoiceNumber, viewModel.InvoiceNumber);
            await SetNewsfeed(newsfeedParameters);
            await SetJobNewsfeed(newsfeedParameters, viewModel.JobId);
        }

        public async Task SetApprovalWorkflowEnabledNewsFeeds(DigitalDropTicketApprovalNewsfeedModel viewModel)
        {
            if (!viewModel.IsBrokeredOrder)
            {
                var newsfeedEvent = viewModel.SupplierPreferredInvoiceTypeId == (int)InvoiceType.DigitalDropTicketManual ? NewsfeedEvent.SupplierCreatedDDTWaitingForApprovalSettingDDT : NewsfeedEvent.SupplierCreatedDDTWaitingForApprovalSettingInvoice;
                await SetDDTWaitingForApprovalNewsfeed(viewModel, newsfeedEvent);
            }
            else
            {
                await SetInvoiceDDTCreatedNewsfeed(viewModel);
            }
        }

        public async Task SetInvoiceDDTCreatedNewsfeed(DigitalDropTicketApprovalNewsfeedModel viewModel)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = viewModel.OrderId,
                EntityType = EntityType.Order,
                CreatedByUserId = viewModel.CreatedBy,
                SupplierCompanyId = viewModel.SupplierCompanyId,
                TargetEntityId = viewModel.InvoiceId,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(viewModel.TimeZoneName)
            };

            if (viewModel.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketManual || viewModel.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp)
            {
                newsfeedParameters.EventTypeId = NewsfeedEvent.SupplierDigitalDropTicketCreated;
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.DdtNumber, viewModel.InvoiceNumber);
            }
            else
            {
                newsfeedParameters.EventTypeId = NewsfeedEvent.SupplierInvoiceCreated;
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.InvoiceNumber, viewModel.InvoiceNumber);
            }

            newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierCompany, viewModel.SupplierCompanyName);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, viewModel.UserName);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, viewModel.PoNumber);
            await SetNewsfeed(newsfeedParameters);

            newsfeedParameters.EntityId = viewModel.InvoiceHeaderId;
            newsfeedParameters.EntityType = EntityType.DigitalDropTicket;
            await SetNewsfeed(newsfeedParameters);
        }

        private async Task UpdateInvoiceCreatedForSingleDeliveryOrderNewsFeed(UserContext userContext, ManualInvoiceCreatedNewsfeedModel viewModel, NewsfeedParameters newsfeedParameters, DateTimeOffset currentDate)
        {
            newsfeedParameters.EventTypeId = viewModel.InvoiceTypeId == (int)InvoiceType.Manual ? NewsfeedEvent.SupplierManualInvoiceCreatedBeforeDateExpiredSingleDelivery : NewsfeedEvent.SupplierManualDDTCreatedBeforeDateExpiredSingleDelivery;

            if (viewModel.OrderCloseDate.DateTime >= currentDate.DateTime) //// not expired yet
            {
                await SetInvoiceCreatedBeforeExpireDateNewsFeed(userContext, viewModel, newsfeedParameters);
            }
            else                //// expired
            {
                await SetInvoiceCreatedAfterOrderCloseDateNewsfeed(userContext, viewModel, newsfeedParameters);
            }

            newsfeedParameters.MessageParameters.Remove(ApplicationConstants.OrderNumber);
            newsfeedParameters.MessageParameters.Remove(ApplicationConstants.DropPercent);
            newsfeedParameters.EntityId = viewModel.InvoiceId;
            newsfeedParameters.EventTypeId = viewModel.InvoiceTypeId == (int)InvoiceType.Manual || viewModel.InvoiceTypeId == (int)InvoiceType.MobileApp ? NewsfeedEvent.SupplierInvoiceModified : NewsfeedEvent.SupplierDigitalDropTicketModified;
            await SetJobNewsfeed(newsfeedParameters, viewModel.JobId);
        }

        private async Task UpdateInvoiceCreatedForMultipleDeliveryOrderNewsFeed(UserContext userContext, ManualInvoiceCreatedNewsfeedModel viewModel, NewsfeedParameters newsfeedParameters)
        {
            if (newsfeedParameters.EventTypeId == NewsfeedEvent.SupplierInvoiceCreated)
            {
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.InvoiceNumber, viewModel.InvoiceNumber);
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, viewModel.PoNumber);
            }
            else if (viewModel.WaitingFor == WaitingAction.UpdatedPrice)
            {
                newsfeedParameters.EventTypeId = NewsfeedEvent.DDTCreatedwaitingForUpdatedPrice;
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.DdtNumber, viewModel.InvoiceNumber);
            }
            else
            {
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.DdtNumber, viewModel.InvoiceNumber);
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, viewModel.PoNumber);
            }

            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierCompany, userContext.CompanyName);
            await SetNewsfeed(newsfeedParameters);


            newsfeedParameters.EntityId = viewModel.InvoiceHeaderId;
            newsfeedParameters.EntityType = (viewModel.InvoiceTypeId == (int)InvoiceType.Manual) ? EntityType.Invoice : EntityType.DigitalDropTicket;
            await SetNewsfeed(newsfeedParameters);

            await SetJobNewsfeed(newsfeedParameters, viewModel.JobId);
        }

        private async Task SetInvoiceCreatedBeforeExpireDateNewsFeed(UserContext userContext, ManualInvoiceCreatedNewsfeedModel viewModel, NewsfeedParameters newsfeedParameters)
        {
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierCompany, userContext.CompanyName);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, viewModel.PoNumber);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.DropPercent, viewModel.DropPercentage.GetPreciseValue(2).ToString());
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);

            if (newsfeedParameters.EventTypeId == NewsfeedEvent.SupplierManualInvoiceCreatedBeforeDateExpiredSingleDelivery)
            {
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.InvoiceNumber, viewModel.InvoiceNumber);
            }
            else
            {
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.DdtNumber, viewModel.InvoiceNumber);
            }

            await SetNewsfeed(newsfeedParameters);
        }

        private async Task SetInvoiceCreatedAfterOrderCloseDateNewsfeed(UserContext userContext, ManualInvoiceCreatedNewsfeedModel viewModel, NewsfeedParameters newsfeedParameters)
        {
            newsfeedParameters.EventTypeId = NewsfeedEvent.OrderClosedOnDateExpired;
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, viewModel.PoNumber);
            await SetNewsfeed(newsfeedParameters);

            //// Newsfeed Supplier:
            newsfeedParameters.EventTypeId = (viewModel.InvoiceTypeId == (int)InvoiceType.Manual || viewModel.InvoiceTypeId == (int)InvoiceType.MobileApp) ? NewsfeedEvent.SupplierManualInvoiceCreatedAfterDateExpiredSingleDelivery : NewsfeedEvent.SupplierManualDDTCreatedAfterDateExpiredSingleDelivery;
            if (newsfeedParameters.EventTypeId == NewsfeedEvent.SupplierManualInvoiceCreatedAfterDateExpiredSingleDelivery)
            {
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.InvoiceNumber, viewModel.InvoiceNumber);
            }
            else
            {
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.DdtNumber, viewModel.InvoiceNumber);
            }

            newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierCompany, userContext.CompanyName);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.DropPercent, viewModel.DropPercentage.GetPreciseValue(2).ToString());
            await SetNewsfeed(newsfeedParameters);
        }

        public async Task SetInvoiceUpdatedNewsfeedToInvoice(UserContext userContext, ManualInvoiceViewModel viewModel)
        {
            NewsfeedEvent eventType = GetInvoiceUpdatedNewsfeedEventType(viewModel);
            EntityType entityType = GetInvoiceUpdatedNewsfeedEntityType(viewModel);
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = viewModel.InvoiceHeaderId,
                EntityType = entityType,
                EventTypeId = eventType,
                CreatedByUserId = userContext.Id,
                BuyerCompanyId = viewModel.BuyerCompanyId,
                SupplierCompanyId = userContext.CompanyId,
                TargetEntityId = viewModel.InvoiceId,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(viewModel.TimeZoneName)
            };
            if (viewModel.IsDigitalDropTicket())
            {
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.DdtNumber, viewModel.DisplayInvoiceNumber);
            }
            else
            {
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.InvoiceNumber, viewModel.DisplayInvoiceNumber);
            }
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierCompany, userContext.CompanyName);
            await SetNewsfeed(newsfeedParameters);
            await UpdateOldInvoicesWithNewIdentity(viewModel);
        }

        public async Task SetInvoiceUpdatedNewsfeedToOrder(UserContext userContext, ManualInvoiceViewModel viewModel)
        {
            NewsfeedEvent eventType = GetInvoiceUpdatedNewsfeedEventType(viewModel);
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = viewModel.OrderId,
                EntityType = EntityType.Order,
                EventTypeId = eventType,
                CreatedByUserId = userContext.Id,
                BuyerCompanyId = viewModel.BuyerCompanyId,
                SupplierCompanyId = userContext.CompanyId,
                TargetEntityId = viewModel.InvoiceId,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(viewModel.TimeZoneName)
            };
            if (viewModel.IsDigitalDropTicket())
            {
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.DdtNumber, viewModel.DisplayInvoiceNumber);
            }
            else
            {
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.InvoiceNumber, viewModel.DisplayInvoiceNumber);
            }
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierCompany, userContext.CompanyName);
            await SetNewsfeed(newsfeedParameters);
        }

        public async Task SetInvoiceUpdatedNewsfeedToJob(UserContext userContext, ManualInvoiceViewModel viewModel, int jobComapnyId)
        {
            if (viewModel.BuyerCompanyId != jobComapnyId)
                return;

            NewsfeedEvent eventType = GetInvoiceUpdatedNewsfeedEventType(viewModel);
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = viewModel.JobId,
                EntityType = EntityType.Job,
                EventTypeId = eventType,
                CreatedByUserId = userContext.Id,
                BuyerCompanyId = viewModel.BuyerCompanyId,
                SupplierCompanyId = 0,
                TargetEntityId = viewModel.InvoiceId,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(viewModel.TimeZoneName)
            };
            if (viewModel.IsDigitalDropTicket())
            {
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.DdtNumber, viewModel.DisplayInvoiceNumber);
            }
            else
            {
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.InvoiceNumber, viewModel.DisplayInvoiceNumber);
            }
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierCompany, userContext.CompanyName);
            await SetNewsfeed(newsfeedParameters);
        }

        public async Task SetDraftDDTModifiedNewsfeed(UserContext userContext, ManualInvoiceViewModel viewModel, bool isEditInvoice)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = viewModel.InvoiceHeaderId,
                EntityType = EntityType.DigitalDropTicket,
                CreatedByUserId = userContext.Id,
                BuyerCompanyId = 0,
                SupplierCompanyId = userContext.CompanyId,
                TargetEntityId = viewModel.InvoiceId,
                EventTypeId = NewsfeedEvent.SupplierDigitalDropTicketDraftModified,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(viewModel.TimeZoneName)
            };

            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
            await SetNewsfeed(newsfeedParameters);

            newsfeedParameters.EntityId = viewModel.OrderId;
            newsfeedParameters.EntityType = EntityType.Order;
            newsfeedParameters.EventTypeId = NewsfeedEvent.SupplierOrderDigitalDropTicketDraftModified;
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.DdtNumber, viewModel.InvoiceNumber.Number);
            await SetNewsfeed(newsfeedParameters);
            await UpdateOldInvoicesWithNewIdentity(viewModel);
        }

        public async Task SetOrderBrokeredNewsfeed(UserContext userContext, FuelRequest fuelRequest, Order order)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = order.Id,
                EntityType = EntityType.Order,
                EventTypeId = NewsfeedEvent.SupplierOrderBrokered,
                CreatedByUserId = userContext.Id,
                SupplierCompanyId = userContext.CompanyId,
                TargetEntityId = fuelRequest.Id,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(fuelRequest.Job.TimeZoneName)
            };
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, order.PoNumber);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.FuelRequestNumber, fuelRequest.RequestNumber);
            await SetNewsfeed(newsfeedParameters);
        }

        public async Task SetDealAcceptedNewsfeed(UserContext userContext, int supplierCompanyId, DealResponseViewModel dealViewModel,int discountId)
        {
            var dealName = Context.DataContext.Discounts.Select(x => new { x.Id, x.DealName }).FirstOrDefault(t => t.Id == discountId)?.DealName;
            if (!string.IsNullOrEmpty(dealName))
            {
                var newsfeedParameters = new NewsfeedParameters
                {
                    EntityId = dealViewModel.InvoiceId,
                    EntityType = EntityType.Invoice,
                    EventTypeId = NewsfeedEvent.DealAccepted,
                    CreatedByUserId = userContext.Id,
                    TargetEntityId = dealViewModel.InvoiceId,
                    BuyerCompanyId = userContext.IsBuyerCompany ? userContext.CompanyId : dealViewModel.BuyerCompanyId,
                    SupplierCompanyId = userContext.IsSupplierCompany ? userContext.CompanyId : supplierCompanyId,
                    CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(dealViewModel.TimeZoneName)
                };

                newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.CompanyName, userContext.CompanyName);
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.DealName, dealName);
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.InvoiceNumber, dealViewModel.DisplayInvoiceNumber);
                await SetNewsfeed(newsfeedParameters);
            }
        }

        public async Task SetOrderPaymentTermsUpdatedNewsfeed(UserContext userContext, Order order)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = order.Id,
                EntityType = EntityType.Order,
                CreatedByUserId = userContext.Id,
                BuyerCompanyId = order.BuyerCompanyId,
                SupplierCompanyId = order.AcceptedCompanyId,
                TargetEntityId = order.Id,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(order.FuelRequest.Job.TimeZoneName),
                EventTypeId = NewsfeedEvent.OrderPaymentTermsUpdated
            };

            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierCompany, userContext.CompanyName);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, order.OrderDetailVersions.FirstOrDefault(t => t.IsActive).PoNumber);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.NewOrderVersionNumber, "V" + order.OrderDetailVersions.FirstOrDefault(t => t.IsActive).Version);
            await SetNewsfeed(newsfeedParameters);
        }

        public async Task SetDealDeclinedNewsfeed(UserContext userContext, int discountId, int invoiceId)
        {
            var invoice = await Context.DataContext.Invoices
                                            .Select(x => new
                                            {
                                                x.Id,
                                                x.InvoiceHeaderId,
                                                supplierCompanyId = x.Order.AcceptedCompanyId,
                                                buyerCompanyId = x.Order.BuyerCompanyId,
                                                timeZoneName = x.Order.FuelRequest.Job.TimeZoneName
                                            })
                                            .FirstOrDefaultAsync(t => t.Id == invoiceId);
            if (invoice != null)
            {
                var dealName = Context.DataContext.Discounts.Select(x => new { x.Id, x.DealName }).FirstOrDefault(t => t.Id == discountId)?.DealName;
                if (!string.IsNullOrEmpty(dealName))
                {
                    var newsfeedParameters = new NewsfeedParameters
                    {
                        EntityId = invoice.InvoiceHeaderId,
                        EntityType = EntityType.Invoice,
                        EventTypeId = NewsfeedEvent.DealDeclined,
                        CreatedByUserId = userContext.Id,
                        TargetEntityId = invoice.Id,
                        BuyerCompanyId = userContext.IsBuyerCompany ? userContext.CompanyId : invoice.buyerCompanyId,
                        SupplierCompanyId = userContext.IsSupplierCompany ? userContext.CompanyId : invoice.supplierCompanyId,
                        CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(invoice.timeZoneName)
                    };
                    newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
                    newsfeedParameters.MessageParameters.Add(ApplicationConstants.CompanyName, userContext.CompanyName);
                    newsfeedParameters.MessageParameters.Add(ApplicationConstants.DealName, dealName);
                    await SetNewsfeed(newsfeedParameters);
                }
            }
        }

        public async Task SetDealCreatedNewsfeed(UserContext userContext, int discountId, int invoiceId)
        {
            var invoice = await Context.DataContext.Invoices
                                             .Select(x => new
                                             {
                                                 x.Id,
                                                 x.InvoiceHeaderId,
                                                 supplierCompanyId = x.Order.AcceptedCompanyId,
                                                 buyerCompanyId = x.Order.BuyerCompanyId,
                                                 timeZoneName = x.Order.FuelRequest.Job.TimeZoneName
                                             })
                                             .FirstOrDefaultAsync(t => t.Id == invoiceId);
            if (invoice != null)
            {
                var dealName = Context.DataContext.Discounts.FirstOrDefault(t => t.Id == discountId)?.DealName;
                var newsfeedParameters = new NewsfeedParameters
                {
                    EntityId = invoice.InvoiceHeaderId,
                    EntityType = EntityType.Invoice,
                    EventTypeId = NewsfeedEvent.DealCreated,
                    CreatedByUserId = userContext.Id,
                    TargetEntityId = invoice.Id,
                    BuyerCompanyId = invoice.buyerCompanyId,
                    SupplierCompanyId = invoice.supplierCompanyId,
                    CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(invoice.timeZoneName)
                };
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.CompanyName, userContext.CompanyName);
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.DealName, dealName);
                await SetNewsfeed(newsfeedParameters);
            }
        }

        public async Task SetFuelRequestAcceptedNewsfeed(UserContext userContext, FuelRequest fuelRequest, Order order)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = order.Id,
                EntityType = EntityType.Order,
                EventTypeId = NewsfeedEvent.SupplierOrderAccepted,
                CreatedByUserId = userContext.Id,
                BuyerCompanyId = fuelRequest.User.Company.Id,
                SupplierCompanyId = userContext.CompanyId,
                TargetEntityId = order.Id,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(fuelRequest.Job.TimeZoneName)
            };
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierCompany, userContext.CompanyName);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.FuelRequestNumber, fuelRequest.RequestNumber);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.JobName, fuelRequest.Job.Name);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, order.PoNumber);
            await SetNewsfeed(newsfeedParameters);

            newsfeedParameters.TargetEntityId = fuelRequest.Job.Id;
            await SetJobNewsfeed(newsfeedParameters, order.FuelRequest.Job.Id);
        }

        public async Task SetThirdPartyOrderCreatedNewsfeed(UserContext userContext, FuelRequest fuelRequest, Order order)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = order.Id,
                EntityType = EntityType.Order,
                EventTypeId = NewsfeedEvent.SupplierThirdPartyOrderCreated,
                CreatedByUserId = userContext.Id,
                SupplierCompanyId = userContext.CompanyId,
                TargetEntityId = order.Id,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(fuelRequest.Job.TimeZoneName)
            };
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, order.PoNumber);
            await SetNewsfeed(newsfeedParameters);
        }

        public async Task SetFuelRequestCanceledNewsfeed(UserContext userContext, FuelRequest fuelRequest)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = fuelRequest.Job.Id,
                EntityType = EntityType.Job,
                EventTypeId = NewsfeedEvent.BuyerCanceledFuelRequest,
                CreatedByUserId = userContext.Id,
                BuyerCompanyId = fuelRequest.User.Company.Id,
                SupplierCompanyId = 0,
                TargetEntityId = fuelRequest.Id,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(fuelRequest.Job.TimeZoneName)
            };
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.FuelRequestNumber, fuelRequest.RequestNumber);
            await SetNewsfeed(newsfeedParameters);
        }

        public async Task SetCounterOfferDeclinedNewsfeed(UserContext userContext, FuelRequest fuelRequest)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = fuelRequest.Job.Id,
                EntityType = EntityType.Job,
                EventTypeId = NewsfeedEvent.SupplierDeclinedCounterOffer,
                CreatedByUserId = userContext.Id,
                BuyerCompanyId = fuelRequest.User.Company.Id,
                SupplierCompanyId = 0,
                TargetEntityId = fuelRequest.Id,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(fuelRequest.Job.TimeZoneName)
            };
            if ((userContext.IsBuyerAndSupplierCompany && userContext.CompanySubTypeId == CompanyType.Buyer) || userContext.IsBuyerCompany)
            {
                newsfeedParameters.EventTypeId = NewsfeedEvent.BuyerDeclinedCounterOffer;
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierCompany, fuelRequest.FuelRequests1.FirstOrDefault().User.Company.Name);
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierUserName, $"{fuelRequest.FuelRequests1.FirstOrDefault().User.FirstName} {fuelRequest.FuelRequests1.FirstOrDefault().User.LastName}");
            }
            else
            {
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierCompany, userContext.CompanyName);
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.BuyerUserName, $"{fuelRequest.User.FirstName} {fuelRequest.User.LastName}");
            }

            newsfeedParameters.MessageParameters.Add(ApplicationConstants.FuelRequestNumber, fuelRequest.RequestNumber);
            await SetNewsfeed(newsfeedParameters);
        }

        public async Task SetCounterOfferAcceptedNewsfeed(UserContext userContext, FuelRequest fuelRequest, Order order)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = fuelRequest.Job.Id,
                EntityType = EntityType.Job,
                EventTypeId = NewsfeedEvent.SupplierAcceptedCounterOffer,
                CreatedByUserId = userContext.Id,
                BuyerCompanyId = fuelRequest.User.Company.Id,
                SupplierCompanyId = 0,
                TargetEntityId = fuelRequest.Id,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(fuelRequest.Job.TimeZoneName)
            };
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.BuyerCompany, fuelRequest.User.Company.Name);
            if ((userContext.IsBuyerAndSupplierCompany && userContext.CompanySubTypeId == CompanyType.Buyer) || userContext.IsBuyerCompany)
            {
                newsfeedParameters.EventTypeId = NewsfeedEvent.BuyerAcceptedCounterOffer;
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.BuyerUserName, userContext.Name);
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierCompany, fuelRequest.FuelRequests1.FirstOrDefault().User.Company.Name);
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierUserName, $"{fuelRequest.FuelRequests1.FirstOrDefault().User.FirstName} {fuelRequest.FuelRequests1.FirstOrDefault().User.LastName}");
            }
            else
            {
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierUserName, userContext.Name);
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierCompany, userContext.CompanyName);
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.BuyerUserName, $"{fuelRequest.User.FirstName} {fuelRequest.User.LastName}");
            }
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.FuelRequestNumber, fuelRequest.RequestNumber);
            await SetNewsfeed(newsfeedParameters);

            //order level newsfeed
            await SetCounterOfferAcceptedNewsfeedForOrder(userContext, fuelRequest, order);
        }

        private async Task SetCounterOfferAcceptedNewsfeedForOrder(UserContext userContext, FuelRequest fuelRequest, Order order)
        {
            NewsfeedParameters newsfeedParameters = new NewsfeedParameters
            {
                EntityId = order.Id,
                EntityType = EntityType.Order,
                EventTypeId = NewsfeedEvent.CounterOfferAcceptedByBuyerAndOrderCreated,
                CreatedByUserId = userContext.Id,
                BuyerCompanyId = fuelRequest.User.Company.Id,
                SupplierCompanyId = order.AcceptedCompanyId,
                TargetEntityId = order.Id,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(fuelRequest.Job.TimeZoneName)
            };
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.BuyerCompany, fuelRequest.User.Company.Name);
            if ((userContext.IsBuyerAndSupplierCompany && userContext.CompanySubTypeId == CompanyType.Buyer) || userContext.IsBuyerCompany)
            {
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.BuyerUserName, userContext.Name);
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierCompany, fuelRequest.FuelRequests1.FirstOrDefault().User.Company.Name);
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierUserName, $"{fuelRequest.FuelRequests1.FirstOrDefault().User.FirstName} {fuelRequest.FuelRequests1.FirstOrDefault().User.LastName}");
            }
            else
            {
                newsfeedParameters.EventTypeId = NewsfeedEvent.CounterOfferAcceptedBySupplierAndOrderCreated;
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierUserName, userContext.Name);
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierCompany, userContext.CompanyName);
            }
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.FuelRequestNumber, fuelRequest.RequestNumber);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, order.PoNumber);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.JobName, fuelRequest.Job.Name);
            await SetNewsfeed(newsfeedParameters);
        }

        internal async Task SetJobUpdateNewsFeed(UserContext userContext, Job job)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = job.Id,
                EntityType = EntityType.Job,
                EventTypeId = NewsfeedEvent.JobModified,
                CreatedByUserId = userContext.Id,
                BuyerCompanyId = job.CompanyId,
                SupplierCompanyId = 0,
                TargetEntityId = job.Id,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(job.TimeZoneName)
            };

            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, $"{userContext.Name}");
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.JobName, job.Name);
            await SetNewsfeed(newsfeedParameters);
        }

        public async Task SetCounterOfferFuelRequestNewsfeed(int userId, bool isBuyer, FuelRequest fuelRequest)
        {
            var userContext = Context.DataContext.Users.Where(t => t.Id == userId).FirstOrDefault();
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = fuelRequest.Job.Id,
                EntityType = EntityType.Job,
                EventTypeId = NewsfeedEvent.CounterOfferFromSupplier,
                CreatedByUserId = userId,
                BuyerCompanyId = fuelRequest.User.Company.Id,
                SupplierCompanyId = 0,
                TargetEntityId = fuelRequest.Id,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(fuelRequest.Job.TimeZoneName)
            };
            if (isBuyer)
            {
                newsfeedParameters.EventTypeId = NewsfeedEvent.CounterOfferFromBuyer;
            }
            else
            {
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierCompany, userContext.Company.Name);
            }

            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, $"{userContext.FirstName} {userContext.LastName}");
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.FuelRequestNumber, fuelRequest.RequestNumber);
            await SetNewsfeed(newsfeedParameters);
        }

        public async Task SetFuelRequestCreatedNewsfeed(int userId, FuelRequest fuelRequest, bool isModified = false)
        {
            var userContext = Context.DataContext.Users.Where(t => t.Id == userId).FirstOrDefault();
            NewsfeedParameters newsfeedParameters;
            if (fuelRequest.Job.IsMarine)
            {
                newsfeedParameters = new NewsfeedParameters
                {
                    EntityId = fuelRequest.Job.Id,
                    EntityType = EntityType.Job,
                    EventTypeId = isModified ? NewsfeedEvent.NominationModified : NewsfeedEvent.NominationCreated,
                    CreatedByUserId = userId,
                    BuyerCompanyId = userContext.Company.Id,
                    SupplierCompanyId = 0,
                    TargetEntityId = fuelRequest.Id,
                    CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(fuelRequest.Job.TimeZoneName)
                };
            }
            else
            {
                newsfeedParameters = new NewsfeedParameters
                {
                    EntityId = fuelRequest.Job.Id,
                    EntityType = EntityType.Job,
                    EventTypeId = isModified ? NewsfeedEvent.FuelRequestModified : NewsfeedEvent.NewFuelRequestCreated,
                    CreatedByUserId = userId,
                    BuyerCompanyId = userContext.Company.Id,
                    SupplierCompanyId = 0,
                    TargetEntityId = fuelRequest.Id,
                    CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(fuelRequest.Job.TimeZoneName)
                };
            }

            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, $"{userContext.FirstName} {userContext.LastName}");
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.FuelRequestNumber, fuelRequest.RequestNumber);
            await SetNewsfeed(newsfeedParameters);
        }

        public async Task SetAssetTrackingEnableDisableNewsfeed(UserContext userContext, Job job, bool isEnabled = false)
        {
            var newsfeedParameters = GetJobNewsfeedCommonParameters(userContext, job);
            newsfeedParameters.EventTypeId = isEnabled ? NewsfeedEvent.EnabledAssetLevelTracking : NewsfeedEvent.DisabledAssetLevelTracking;
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
            await SetNewsfeed(newsfeedParameters);
        }

        public async Task SetAssetPictureEnableDisableNewsfeed(UserContext userContext, Job job, bool isEnabled = false)
        {
            var newsfeedParameters = GetJobNewsfeedCommonParameters(userContext, job);
            newsfeedParameters.EventTypeId = isEnabled ? NewsfeedEvent.EnabledAssetPictureRequiredDuringDrop : NewsfeedEvent.DisabledAssetPictureRequiredDuringDrop;
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
            await SetNewsfeed(newsfeedParameters);
        }

        public async Task SetResaleEnableDisableNewsfeed(UserContext userContext, Job job, bool isEnabled = false)
        {
            var newsfeedParameters = GetJobNewsfeedCommonParameters(userContext, job);
            newsfeedParameters.EventTypeId = isEnabled ? NewsfeedEvent.EnabledResaleDetails : NewsfeedEvent.DisabledResaleDetails;
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
            await SetNewsfeed(newsfeedParameters);
        }

        public async Task SetProFormaEnableDisableForJobNewsfeed(UserContext userContext, Job job, bool isEnabled = false)
        {
            var newsfeedParameters = GetJobNewsfeedCommonParameters(userContext, job);
            newsfeedParameters.EventTypeId = isEnabled ? NewsfeedEvent.EnabledProFormaPoForJob : NewsfeedEvent.DisabledProFormaPoForJob;
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
            await SetNewsfeed(newsfeedParameters);
        }

        public async Task SetProFormaEnableDisableForOrderNewsfeed(UserContext userContext, Order order)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = order.Id,
                EntityType = EntityType.Order,
                CreatedByUserId = userContext.Id,
                BuyerCompanyId = 0,
                SupplierCompanyId = order.AcceptedCompanyId,
                TargetEntityId = order.Id,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(order.FuelRequest.Job.TimeZoneName),
                EventTypeId = NewsfeedEvent.EnabledProFormaPoForTPOOrder
            };

            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
            await SetNewsfeed(newsfeedParameters);
        }

        public async Task SetTaxExemptEnableDisableNewsfeed(UserContext userContext, Job job, bool isEnabled = false)
        {
            var newsfeedParameters = GetJobNewsfeedCommonParameters(userContext, job);
            newsfeedParameters.EventTypeId = isEnabled ? NewsfeedEvent.EnabledSalesTaxExempt : NewsfeedEvent.DisabledSalesTaxExempt;
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
            await SetNewsfeed(newsfeedParameters);
        }

        public async Task SetApprovalWorkflowEnableDisableNewsfeed(UserContext userContext, Job job, int? approvalUserId, bool isEnabled = false)
        {
            var newsfeedParameters = GetJobNewsfeedCommonParameters(userContext, job);
            newsfeedParameters.EventTypeId = isEnabled ? NewsfeedEvent.EnabledInvoiceDDTWorkflowApproval : NewsfeedEvent.DisabledInvoiceDDTWorkflowApproval;
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
            if (isEnabled)
            {
                var approvalUser = Context.DataContext.Users.Where(t => t.Id == approvalUserId).FirstOrDefault();
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.ApprovalPerson, approvalUser != null ? $"{approvalUser.FirstName} {approvalUser.LastName}" : Resource.lblHyphen);
            }
            await SetNewsfeed(newsfeedParameters);
        }

        public async Task SetAssetAddedRemovedNewsfeed(UserContext userContext, Job job, int assetCount, bool isAdded)
        {
            var newsfeedParameters = GetJobNewsfeedCommonParameters(userContext, job);
            newsfeedParameters.EventTypeId = isAdded ? NewsfeedEvent.AssetAdded : NewsfeedEvent.AssetRemoved;
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.NumberOfAssets, assetCount.ToString());

            await SetNewsfeed(newsfeedParameters);
        }

        public async Task SetAssetModifiedNewsfeed(UserContext userContext, Job job, int assetCount)
        {
            var newsfeedParameters = GetJobNewsfeedCommonParameters(userContext, job);
            newsfeedParameters.EventTypeId = NewsfeedEvent.AssetModified;
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.NumberOfAssets, assetCount.ToString());

            await SetNewsfeed(newsfeedParameters);
        }

        public async Task SetAssetReassignedNewsfeed(UserContext userContext, Job job, int assetCount, string newJobName)
        {
            var newsfeedParameters = GetJobNewsfeedCommonParameters(userContext, job);
            newsfeedParameters.EventTypeId = NewsfeedEvent.AssetReassigned;
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.NumberOfAssets, assetCount.ToString());
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OtherJobName, newJobName);

            await SetNewsfeed(newsfeedParameters);
        }

        public async Task SetSystemFuelRequestExpiredNewsfeed(FuelRequest fuelRequest)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = fuelRequest.Job.Id,
                EntityType = EntityType.Job,
                EventTypeId = NewsfeedEvent.FuelRequestExpired,
                CreatedByUserId = (int)SystemUser.System,
                BuyerCompanyId = fuelRequest.User.Company.Id,
                SupplierCompanyId = 0,
                TargetEntityId = fuelRequest.Id,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(fuelRequest.Job.TimeZoneName)
            };
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.FuelRequestNumber, fuelRequest.RequestNumber);
            await SetNewsfeed(newsfeedParameters);
        }

        public async Task SetSystemFuelRequestGoingToExpiredNewsfeed(FuelRequest fuelRequest)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = fuelRequest.Job.Id,
                EntityType = EntityType.Job,
                EventTypeId = NewsfeedEvent.FuelRequestExpireSoon,
                CreatedByUserId = (int)SystemUser.System,
                BuyerCompanyId = fuelRequest.User.Company.Id,
                SupplierCompanyId = 0,
                TargetEntityId = fuelRequest.Id,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(fuelRequest.Job.TimeZoneName)
            };
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.FuelRequestNumber, fuelRequest.RequestNumber);
            await SetNewsfeed(newsfeedParameters);
        }

        public async Task SetDriverAssignmentOrderNewsfeed(UserContext userContext, Order order, NewsfeedEvent eventId)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = order.Id,
                EntityType = EntityType.Order,
                EventTypeId = eventId,
                CreatedByUserId = userContext.Id,
                BuyerCompanyId = order.BuyerCompanyId,
                SupplierCompanyId = userContext.CompanyId,
                TargetEntityId = order.Id,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(order.FuelRequest.Job.TimeZoneName)
            };
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierCompany, userContext.CompanyName);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, (order.PoNumber));
            await ContextFactory.Current.GetDomain<NewsfeedDomain>().SetNewsfeed(newsfeedParameters);

            await SetJobNewsfeed(newsfeedParameters, order.FuelRequest.Job.Id);
        }

        public async Task SetDeliveryScheduleNewsfeed(UserContext userContext, Order order, NewsfeedEvent eventId)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = order.Id,
                EntityType = EntityType.Order,
                CreatedByUserId = userContext.Id,
                TargetEntityId = order.Id,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(order.FuelRequest.Job.TimeZoneName)
            };
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
            if (userContext.IsBuyerCompany || userContext.CompanySubTypeId == CompanyType.Buyer)
            {
                newsfeedParameters.EventTypeId = eventId;
                newsfeedParameters.BuyerCompanyId = userContext.CompanyId;
                newsfeedParameters.SupplierCompanyId = order.AcceptedCompanyId;
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.BuyerCompany, userContext.CompanyName);
            }
            else
            {
                newsfeedParameters.EventTypeId = eventId;
                newsfeedParameters.BuyerCompanyId = order.BuyerCompanyId;
                newsfeedParameters.SupplierCompanyId = userContext.CompanyId;
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierCompany, userContext.CompanyName);
            }
            await SetNewsfeed(newsfeedParameters);

            if (userContext.IsBuyerCompany || userContext.CompanySubTypeId == CompanyType.Buyer)
            {
                newsfeedParameters.MessageParameters.Remove(ApplicationConstants.BuyerCompany);
            }
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, order.PoNumber);
            bool isJobNewsfeed = true;
            switch (eventId)
            {
                case NewsfeedEvent.BuyerOrderDeliveryScheduleAdded:
                    newsfeedParameters.EventTypeId = NewsfeedEvent.BuyerJobDeliveryScheduleAdded; break;
                case NewsfeedEvent.SupplierOrderDeliveryScheduleAdded:
                    newsfeedParameters.EventTypeId = NewsfeedEvent.SupplierJobDeliveryScheduleAdded; break;
                case NewsfeedEvent.BuyerOrderDeliveryScheduleModified:
                    newsfeedParameters.EventTypeId = NewsfeedEvent.BuyerJobDeliveryScheduleModified; break;
                case NewsfeedEvent.SupplierOrderDeliveryScheduleModified:
                    newsfeedParameters.EventTypeId = NewsfeedEvent.SupplierJobDeliveryScheduleModified; break;
                case NewsfeedEvent.BuyerReschedulesSchedule:
                    newsfeedParameters.EventTypeId = NewsfeedEvent.BuyerJobRescheduledMissedDeliverySchedule; break;
                case NewsfeedEvent.SupplierReschedulesSchedule:
                    newsfeedParameters.EventTypeId = NewsfeedEvent.SupplierJobRescheduledMissedDeliverySchedule; break;
                case NewsfeedEvent.BuyerOrderDeliveryScheduleRemoved:
                    newsfeedParameters.EventTypeId = NewsfeedEvent.BuyerJobDeliveryScheduleRemoved; break;
                case NewsfeedEvent.SupplierOrderDeliveryScheduleRemoved:
                    newsfeedParameters.EventTypeId = NewsfeedEvent.SupplierJobDeliveryScheduleRemoved; break;
                case NewsfeedEvent.BuyerCanceledSchedule:
                    newsfeedParameters.EventTypeId = NewsfeedEvent.BuyerJobDeliveryScheduleCanceled; break;
                case NewsfeedEvent.SupplierCanceledSchedule:
                    newsfeedParameters.EventTypeId = NewsfeedEvent.SupplierJobDeliveryScheduleCanceled; break;
                default:
                    isJobNewsfeed = false; break;
            }
            if (isJobNewsfeed)
            {
                await SetJobNewsfeed(newsfeedParameters, order.FuelRequest.Job.Id);
            }
        }

        public async Task SetPONumberRenamedNewsfeed(UserContext userContext, Order order, string poNumber, string previousPONumber)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = order.Id,
                EntityType = EntityType.Order,
                CreatedByUserId = userContext.Id,
                TargetEntityId = order.Id,
                BuyerCompanyId = order.BuyerCompanyId,
                SupplierCompanyId = order.AcceptedCompanyId,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(order.FuelRequest.Job.TimeZoneName)
            };

            if (order.IsProFormaPo)
            {
                newsfeedParameters.EventTypeId = NewsfeedEvent.PoNumberChangedForProFormaOrder;
            }
            else
            {
                newsfeedParameters.EventTypeId = NewsfeedEvent.BuyerRenamedPONumber;
            }
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, poNumber);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.PreviousOrderNumber, previousPONumber);

            await SetNewsfeed(newsfeedParameters);
        }

        public async Task SetSelectOrderCanceledNewsfeed(UserContext userContext, Order order)
        {
            var hasDeliverySchedules = order.OrderDeliverySchedules.Any(t => t.IsActive);
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = order.Id,
                EntityType = EntityType.Order,
                EventTypeId = NewsfeedEvent.SupplierSelectOrderCanceledNoSchedules,
                CreatedByUserId = order.User.Id,
                BuyerCompanyId = order.BuyerCompanyId,
                SupplierCompanyId = order.AcceptedCompanyId,
                TargetEntityId = order.Id,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(order.FuelRequest.Job.TimeZoneName)
            };

            var helperDomain = new HelperDomain(this);
            var reason = string.Empty;
            if (order.OrderXCancelationReason != null)
            {
                reason = order.OrderXCancelationReason.MstOrderCancelationReason.Name + Resource.lblSingleHyphen + order.OrderXCancelationReason.AdditionalNotes;
            }

            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, order.User.FirstName + " " + order.User.LastName);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.Reason, reason);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.FutureDeliverySchedulesCanceled, hasDeliverySchedules ? Resource.lblMessageFutureDeliverySchedulesCanceled : string.Empty);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierCompany, order.Company.Name);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderPercentageDelivered, helperDomain.GetFuelDeliveredPercentage(order).ToString());
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, order.PoNumber);
            await SetNewsfeed(newsfeedParameters);

            await SetJobNewsfeed(newsfeedParameters, order.FuelRequest.Job.Id);
        }

        public async Task SetOrderCancelNewsfeed(UserContext userContext, Order order, string reason)
        {
            var hasDeliverySchedules = order.OrderDeliverySchedules.Any(t => t.IsActive);
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = order.Id,
                EntityType = EntityType.Order,
                EventTypeId = NewsfeedEvent.SupplierOrderCanceled,
                CreatedByUserId = userContext.Id,
                BuyerCompanyId = order.FuelRequest.User.Company.Id,
                SupplierCompanyId = userContext.CompanyId,
                TargetEntityId = order.Id,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(order.FuelRequest.Job.TimeZoneName)
            };
            if ((userContext.IsBuyerAndSupplierCompany && userContext.CompanySubTypeId == CompanyType.Buyer) || userContext.IsBuyerCompany)
            {
                newsfeedParameters.EventTypeId = NewsfeedEvent.BuyerOrderCanceled;
                newsfeedParameters.BuyerCompanyId = userContext.CompanyId;
                newsfeedParameters.SupplierCompanyId = order.Company.Id;
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.BuyerCompany, userContext.CompanyName);
            }
            else
            {
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierCompany, userContext.CompanyName);
            }

            var helperDomain = new HelperDomain(this);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.FutureDeliverySchedulesCanceled, hasDeliverySchedules ? Resource.lblMessageFutureDeliverySchedulesCanceled : string.Empty);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderPercentageDelivered, helperDomain.GetFuelDeliveredPercentage(order).ToString());
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.Reason, reason);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, order.PoNumber);
            await SetNewsfeed(newsfeedParameters);

            await SetJobNewsfeed(newsfeedParameters, order.FuelRequest.Job.Id);
        }

        public async Task SetBrokeredOrderCancelNewsfeed(UserContext userContext, Order parentOrder, Order order, string reason)
        {
            var helperDomain = new HelperDomain(this);
            var hasDeliverySchedules = order.OrderDeliverySchedules.Any(t => t.IsActive);
            var parentOrderHasDeliverySchedules = parentOrder.OrderDeliverySchedules.Any(t => t.IsActive);

            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = order.Id,
                EntityType = EntityType.Order,
                EventTypeId = parentOrderHasDeliverySchedules ? NewsfeedEvent.SupplierNextBrokeredOrderWithSchedulesAutoCanceled : NewsfeedEvent.SupplierNextBrokeredOrderWithoutSchedulesAutoCanceled,
                CreatedByUserId = userContext.Id,
                BuyerCompanyId = order.FuelRequest.User.Company.Id,
                SupplierCompanyId = order.AcceptedCompanyId,
                TargetEntityId = order.Id,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(order.FuelRequest.Job.TimeZoneName)
            };

            newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierCompany, userContext.CompanyName);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.CanceledOrClosedPO, parentOrder.PoNumber);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.Reason, reason);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.CanceledOrderPercentageDelivered, helperDomain.GetFuelDeliveredPercentage(parentOrder).ToString());
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, order.PoNumber);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderPercentageDelivered, helperDomain.GetFuelDeliveredPercentage(order).ToString());
            await SetNewsfeed(newsfeedParameters);

            newsfeedParameters.EntityId = parentOrder.Id;
            newsfeedParameters.TargetEntityId = parentOrder.Id;
            newsfeedParameters.SupplierCompanyId = userContext.CompanyId;
            newsfeedParameters.EventTypeId = hasDeliverySchedules ? NewsfeedEvent.SupplierCurrentBrokeredOrderWithSchedulesCanceled : NewsfeedEvent.SupplierCurrentBrokeredOrderWithoutSchedulesCanceled;
            await SetNewsfeed(newsfeedParameters);

            await SetJobNewsfeed(newsfeedParameters, order.FuelRequest.Job.Id);
        }

        public async Task SetOrderChosenNewsfeed(UserContext userContext, Order order, bool isOriginalOrderChosen)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = order.Id,
                EntityType = EntityType.Order,
                EventTypeId = isOriginalOrderChosen ? NewsfeedEvent.SupplierOriginalOrderChosen : NewsfeedEvent.SupplierAdaptedOrderChosen,
                CreatedByUserId = userContext.Id,
                BuyerCompanyId = order.BuyerCompanyId,
                SupplierCompanyId = userContext.CompanyId,
                TargetEntityId = order.Id,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(order.FuelRequest.Job.TimeZoneName)
            };

            var buyerCompany = await Context.DataContext.Companies.SingleOrDefaultAsync(t => t.Id == order.BuyerCompanyId);
            var canceledOrder = order.Orders1.FirstOrDefault(t => t.BuyerCompanyId == order.BuyerCompanyId);

            newsfeedParameters.MessageParameters.Add(ApplicationConstants.BuyerCompany, buyerCompany.Name);

            newsfeedParameters.MessageParameters.Add(ApplicationConstants.BrokerCompany, canceledOrder.Company.Name);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.BrokerUserName, string.Format("{0} {1}", canceledOrder.User.FirstName, canceledOrder.User.LastName));

            newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierCompany, userContext.CompanyName);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, order.PoNumber);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.CanceledOrClosedPO, canceledOrder.PoNumber);
            await SetNewsfeed(newsfeedParameters);

            await SetJobNewsfeed(newsfeedParameters, order.FuelRequest.Job.Id);
        }

        internal async Task SetJobReOpenedNewsFeed(UserContext userContext, Job job)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = job.Id,
                EntityType = EntityType.Job,
                EventTypeId = NewsfeedEvent.JobReOpened,
                CreatedByUserId = userContext.Id,
                BuyerCompanyId = userContext.CompanyId,
                SupplierCompanyId = 0,
                TargetEntityId = job.Id,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(job.TimeZoneName)
            };

            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, $"{userContext.Name}");
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.JobName, job.Name);
            await SetNewsfeed(newsfeedParameters);
        }

        public async Task SetOrderClosedNewsfeed(UserContext userContext, Order order)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = order.Id,
                EntityType = EntityType.Order,
                EventTypeId = NewsfeedEvent.SupplierOrderClosed,
                CreatedByUserId = userContext.Id,
                BuyerCompanyId = order.FuelRequest.User.Company.Id,
                SupplierCompanyId = userContext.CompanyId,
                TargetEntityId = order.Id,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(order.FuelRequest.Job.TimeZoneName)
            };
            if ((userContext.IsBuyerAndSupplierCompany && userContext.CompanySubTypeId == CompanyType.Buyer) || userContext.IsBuyerCompany)
            {
                newsfeedParameters.EventTypeId = NewsfeedEvent.BuyerOrderClosed;
                newsfeedParameters.BuyerCompanyId = userContext.CompanyId;
                newsfeedParameters.SupplierCompanyId = order.Company.Id;
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.BuyerCompany, userContext.CompanyName);
            }
            else
            {
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierCompany, userContext.CompanyName);
            }
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, order.PoNumber);
            await SetNewsfeed(newsfeedParameters);

            await SetJobNewsfeed(newsfeedParameters, order.FuelRequest.Job.Id);
        }

        public async Task SetMessageNewsfeed(UserContext userContext, ComposeMessageViewModel viewModel)
        {
            Order order = (Order)null; Invoice invoice = (Invoice)null;
            if (viewModel.Type == AppMessageQueryType.Order)
            {
                order = Context.DataContext.Orders.First(t => t.Id == viewModel.Number);
            }
            else
            {
                invoice = Context.DataContext.Invoices.First(t => t.Id == viewModel.Number);
            }

            var timezone = order == null ? (invoice == null ? TimeZone.CurrentTimeZone.StandardName : invoice.Order.FuelRequest.Job.TimeZoneName) : order.FuelRequest.Job.TimeZoneName;
            var displayNumber = order == null ? (invoice == null ? string.Empty : invoice.DisplayInvoiceNumber) : order.PoNumber;
            var numberKey = viewModel.Type == AppMessageQueryType.Order ? ApplicationConstants.OrderNumber : (viewModel.Type == AppMessageQueryType.Invoice ? ApplicationConstants.InvoiceNumber : ApplicationConstants.DdtNumber);
            var recipientCompanies = Context.DataContext.Users.Where(t => viewModel.Recipients.Contains(t.Id)).Select(t => t.Company.Id).OrderBy(t => t).ToList();
            if (recipientCompanies.Count == 0)
            {
                recipientCompanies = viewModel.RecipientCompanies;
            }
            if (recipientCompanies.Any(t => t != userContext.CompanyId))
            {
                recipientCompanies = recipientCompanies.Where(t => t != userContext.CompanyId).Select(t => t).ToList();
            }
            foreach (var companyId in recipientCompanies.Distinct())
            {
                var newsfeedParameters = new NewsfeedParameters
                {
                    EntityId = order == null ? invoice.Order.Id : order.Id,
                    EntityType = EntityType.Order,
                    CreatedByUserId = userContext.Id,
                    TargetEntityId = viewModel.Id,
                    CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(timezone)
                };
                if ((userContext.IsBuyerAndSupplierCompany && userContext.CompanySubTypeId == CompanyType.Supplier) || userContext.IsSupplierCompany)
                {
                    newsfeedParameters.EventTypeId = viewModel.Type == AppMessageQueryType.Order ? NewsfeedEvent.SupplierOrderMessage : (viewModel.Type == AppMessageQueryType.Invoice ? NewsfeedEvent.SupplierInvoiceMessage : NewsfeedEvent.SupplierDigitalDropTicketMessage);
                    newsfeedParameters.BuyerCompanyId = companyId == userContext.CompanyId ? 0 : companyId;
                    newsfeedParameters.SupplierCompanyId = userContext.CompanyId;
                    newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierCompany, userContext.CompanyName);
                }
                else
                {
                    newsfeedParameters.EventTypeId = viewModel.Type == AppMessageQueryType.Order ? NewsfeedEvent.BuyerOrderMessage : (viewModel.Type == AppMessageQueryType.Invoice ? NewsfeedEvent.BuyerInvoiceMessage : NewsfeedEvent.BuyerDigitalDropTicketMessage);
                    newsfeedParameters.BuyerCompanyId = userContext.CompanyId;
                    newsfeedParameters.SupplierCompanyId = companyId == userContext.CompanyId ? 0 : companyId;
                    newsfeedParameters.MessageParameters.Add(ApplicationConstants.BuyerCompany, userContext.CompanyName);
                }
                newsfeedParameters.MessageParameters.Add(numberKey, displayNumber);
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
                await SetNewsfeed(newsfeedParameters);

                if (newsfeedParameters.EventTypeId == NewsfeedEvent.BuyerInvoiceMessage || newsfeedParameters.EventTypeId == NewsfeedEvent.SupplierInvoiceMessage
                    || newsfeedParameters.EventTypeId == NewsfeedEvent.BuyerDigitalDropTicketMessage || newsfeedParameters.EventTypeId == NewsfeedEvent.SupplierDigitalDropTicketMessage)
                {
                    newsfeedParameters.EntityId = invoice.InvoiceHeaderId;
                    newsfeedParameters.EntityType = newsfeedParameters.EventTypeId == NewsfeedEvent.BuyerInvoiceMessage || newsfeedParameters.EventTypeId == NewsfeedEvent.SupplierInvoiceMessage ? EntityType.Invoice : EntityType.DigitalDropTicket;
                    await SetNewsfeed(newsfeedParameters);
                }

                if (order != null || invoice != null)
                {
                    await SetJobNewsfeed(newsfeedParameters, order == null ? invoice.Order.FuelRequest.Job.Id : order.FuelRequest.Job.Id);
                }
            }
        }

        internal async Task SetJobClosedNewsFeed(UserContext userContext, Job job)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = job.Id,
                EntityType = EntityType.Job,
                EventTypeId = NewsfeedEvent.JobClosed,
                CreatedByUserId = userContext.Id,
                BuyerCompanyId = job.Company.Id,
                SupplierCompanyId = 0,
                TargetEntityId = job.Id,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(job.TimeZoneName)
            };

            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, $"{userContext.Name}");
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.JobName, job.Name);
            await SetNewsfeed(newsfeedParameters);
        }

        public async Task SetGlobalToCurrentCostUpdateNewsfeed(UserContext userContext, FuelRequest fuelRequest, NewsfeedEvent eventType)
        {
            Order order = fuelRequest.Orders.First();
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = order.Id,
                EntityType = EntityType.Order,
                EventTypeId = eventType,
                CreatedByUserId = userContext.Id,
                BuyerCompanyId = 0,
                SupplierCompanyId = order.AcceptedCompanyId,
                TargetEntityId = order.Id,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(fuelRequest.Job.TimeZoneName)
            };

            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, $"{userContext.Name}");
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, order.PoNumber);
            await SetNewsfeed(newsfeedParameters);
        }

        public async Task SetCurrentCostUpdateNewsfeed(UserContext userContext, FuelRequest fuelRequest, NewsfeedEvent eventType, decimal currentCost, decimal newCost)
        {
            Order order = fuelRequest.Orders.First();
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = order.Id,
                EntityType = EntityType.Order,
                EventTypeId = eventType,
                CreatedByUserId = userContext.Id,
                BuyerCompanyId = 0,
                SupplierCompanyId = order.AcceptedCompanyId,
                TargetEntityId = order.Id,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(fuelRequest.Job.TimeZoneName)
            };

            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, $"{userContext.Name}");
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, order.PoNumber);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.CurrentAmount, Resource.constSymbolCurrency + currentCost.GetPreciseValue(6).ToString());
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.NewAmount, Resource.constSymbolCurrency + newCost.GetPreciseValue(6).ToString());
            await SetNewsfeed(newsfeedParameters);
        }

        public async Task SetCurrentToGlobalCostUpdateNewsfeed(UserContext userContext, Order order, NewsfeedEvent eventType)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = order.Id,
                EntityType = EntityType.Order,
                EventTypeId = eventType,
                CreatedByUserId = userContext.Id,
                BuyerCompanyId = 0,
                SupplierCompanyId = order.AcceptedCompanyId,
                TargetEntityId = order.Id,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(order.FuelRequest.Job.TimeZoneName)
            };

            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, $"{userContext.Name}");
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, order.PoNumber);
            await SetNewsfeed(newsfeedParameters);
        }

        public async Task SetGlobalCostUpdateNewsfeed(UserContext userContext, Order order, NewsfeedEvent eventType, decimal currentCost, decimal newCost)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = order.Id,
                EntityType = EntityType.Order,
                EventTypeId = eventType,
                CreatedByUserId = userContext.Id,
                BuyerCompanyId = 0,
                SupplierCompanyId = order.AcceptedCompanyId,
                TargetEntityId = order.Id,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(order.FuelRequest.Job.TimeZoneName)
            };

            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, $"{userContext.Name}");
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, order.PoNumber);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.CurrentAmount, Resource.constSymbolCurrency + currentCost.GetPreciseValue(6).ToString());
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.NewAmount, Resource.constSymbolCurrency + newCost.GetPreciseValue(6).ToString());
            await SetNewsfeed(newsfeedParameters);
        }

        public async Task SetQuoteRequestCreatedCanceledNewsfeed(int userId, QuoteRequest quoteRequest, NewsfeedEvent newsfeedEvent)
        {
            var userContext = Context.DataContext.Users.Where(t => t.Id == userId).FirstOrDefault();
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = quoteRequest.Id,
                EntityType = EntityType.QuoteRequest,
                EventTypeId = newsfeedEvent,
                CreatedByUserId = userId,
                BuyerCompanyId = userContext.Company.Id,
                SupplierCompanyId = 0,
                TargetEntityId = quoteRequest.Id,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(quoteRequest.Job.TimeZoneName)
            };

            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, $"{userContext.FirstName} {userContext.LastName}");
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.QuoteRequestNumber, quoteRequest.RequestNumber);
            if (newsfeedEvent == NewsfeedEvent.QuoteRequestCanceled)
            {
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.Date, newsfeedParameters.CreatedDate.ToString(Resource.constFormatDate));
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.Time, newsfeedParameters.CreatedDate.ToString(Resource.constFormatTime));
            }
            await SetNewsfeed(newsfeedParameters);
        }

        public async Task SetQuoteRequestDueDateQtyNeededUpdatedNewsfeed(int userId, string originalValue, QuoteRequest quoteRequest, NewsfeedEvent newsfeedEvent)
        {
            var userContext = Context.DataContext.Users.Where(t => t.Id == userId).FirstOrDefault();
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = quoteRequest.Id,
                EntityType = EntityType.QuoteRequest,
                EventTypeId = newsfeedEvent,
                CreatedByUserId = userId,
                BuyerCompanyId = userContext.Company.Id,
                SupplierCompanyId = 0,
                TargetEntityId = quoteRequest.Id,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(quoteRequest.Job.TimeZoneName)
            };

            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, $"{userContext.FirstName} {userContext.LastName}");
            if (newsfeedEvent == NewsfeedEvent.RFQDueDateModified)
            {
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.QuoteRequestNumber, quoteRequest.RequestNumber);
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.OriginalDueDate, originalValue);
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.NewDueDate, quoteRequest.QuoteDueDate.ToString(Resource.constFormatDate));
            }
            else
            {
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.OriginalNumber, originalValue);
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.NewNumber, Convert.ToString(quoteRequest.QuotesNeeded));
            }
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.Date, newsfeedParameters.CreatedDate.ToString(Resource.constFormatDate));
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.Time, newsfeedParameters.CreatedDate.ToString(Resource.constFormatTime));
            await SetNewsfeed(newsfeedParameters);
        }

        public async Task SetQuoteRequestAttachmentUpdatedNewsfeed(UserContext userContext, string imageName, int quoteRequestId, NewsfeedEvent addedRemoved)
        {
            var quoteRequest = Context.DataContext.QuoteRequests.Where(t => t.Id == quoteRequestId).FirstOrDefault();
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = quoteRequest.Id,
                EntityType = EntityType.QuoteRequest,
                EventTypeId = addedRemoved,
                CreatedByUserId = userContext.Id,
                BuyerCompanyId = userContext.CompanyId,
                SupplierCompanyId = 0,
                TargetEntityId = quoteRequest.Id,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(quoteRequest.Job.TimeZoneName)
            };

            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.AttachmentName, imageName);
            await SetNewsfeed(newsfeedParameters);
        }

        public async Task SetQuotationCreatedNewsfeed(UserContext userContext, Quotation quotation)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = quotation.QuoteRequest.Id,
                EntityType = EntityType.QuoteRequest,
                EventTypeId = NewsfeedEvent.ReceivedQuote,
                CreatedByUserId = userContext.Id,
                BuyerCompanyId = quotation.QuoteRequest.User.Company.Id,
                SupplierCompanyId = 0,
                TargetEntityId = quotation.QuoteRequest.Id,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(quotation.QuoteRequest.Job.TimeZoneName)
            };

            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.QuotationNumber, quotation.QuoteNumber);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierCompany, userContext.CompanyName);

            await SetNewsfeed(newsfeedParameters);
        }

        public async Task SetBuyerApprovedDDTWaitingForPriceNewsfeed(UserContext userContext, Invoice invoice)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = invoice.InvoiceHeaderId,
                EntityType = EntityType.DigitalDropTicket,
                EventTypeId = NewsfeedEvent.BuyerApprovedDDTWaitingForPrice,
                CreatedByUserId = userContext.Id,
                BuyerCompanyId = userContext.CompanyId,
                SupplierCompanyId = invoice.Order.AcceptedCompanyId,
                TargetEntityId = invoice.Id,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(invoice.Order.FuelRequest.Job.TimeZoneName)
            };
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.BuyerCompany, userContext.CompanyName);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.DdtNumber, invoice.DisplayInvoiceNumber.Replace(ApplicationConstants.SFIN, ApplicationConstants.SFDD));
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, invoice.PoNumber);
            await SetNewsfeed(newsfeedParameters);

            newsfeedParameters.EntityId = invoice.OrderId.Value;
            newsfeedParameters.EntityType = EntityType.Order;
            await SetNewsfeed(newsfeedParameters);
        }

        public async Task<NewsfeedMessagesViewModel> GetNewsfeed(UserContext userContext, EntityType entityType, int entityId, int currentPage = 1, int latestId = 0)
        {
            var response = new NewsfeedMessagesViewModel();
            try
            {
                var pageSize = ApplicationConstants.NewsfeedDefaultPageSize;
                StoredProcedureDomain storedProcedureDomain = new StoredProcedureDomain(this);
                var messages = await storedProcedureDomain.GetNewsfeeds(userContext.CompanyId, userContext.Id, (int)entityType, entityId, currentPage, pageSize, latestId);

                var regex = new Regex(@"\{.*\}");
                foreach (var item in messages)
                {
                    if (regex.IsMatch(item.FeedMessage) && item.TargetUrl != null && (entityType != EntityType.Invoice && entityType != EntityType.DigitalDropTicket && entityType != EntityType.Job || item.FeedMessage.IndexOf("question") > 0))
                    {
                        var text = item.FeedMessage.Replace(".", "").Split(' ').LastOrDefault(t => t.StartsWith("{") && t.EndsWith("}"));
                        if (!string.IsNullOrWhiteSpace(text))
                        {
                            string anchor = string.Empty;
                            if (userContext.IsSupplierCompany && entityType == EntityType.Order && item.TargetUrl != null && item.TargetUrl.ToLower().Contains("invoice/details"))
                            {
                                anchor = "<a onclick='slideInvoiceDetails(" + item.TargetEntityId + ")'>" + text.Substring(1, text.Length - 2) + "</a>";
                            }
                            else
                            {
                                anchor = "<a href='" + item.TargetUrl + "'>" + text.Substring(1, text.Length - 2) + "</a>";
                            }
                            item.FeedMessage = item.FeedMessage.Replace(text, anchor);
                        }
                    }
                    else
                    {
                        var text = item.FeedMessage.Replace(".", "").Split(' ').LastOrDefault(t => t.StartsWith("{") && t.EndsWith("}"));
                        if (!string.IsNullOrWhiteSpace(text))
                        {
                            item.FeedMessage = item.FeedMessage.Replace(text, text.Substring(1, text.Length - 2));
                        }
                    }
                    item.FeedMessage = item.FeedMessage.Replace("{", "").Replace("}", "");
                    response.Messages.Add(item);
                }

                var totalMessages = messages.Select(t => t.TotalMessages).FirstOrDefault();
                response.TotalMessages = totalMessages;

                decimal totalPage = (decimal)response.TotalMessages / ApplicationConstants.NewsfeedDefaultPageSize;
                response.TotalPages = (int)Math.Ceiling(totalPage);
                response.CurrentPage = currentPage;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NewsfeedDomain", "GetNewsfeed", ex.Message + "CompanyId:" + userContext.CompanyId + "UserId:" + userContext.Id + "EntityType:" + entityType + "EntityId:" + entityId, ex);
            }
            return response;
        }

        public async Task<NewsfeedMessagesViewModel> GetNewsfeedForBuyerAndSupplier(UserContext userContext, int CompanyId, int currentPage = 1, int latestId = 0)
        {
            var response = new NewsfeedMessagesViewModel();
            try
            {
                var pageSize = ApplicationConstants.NewsfeedDefaultPageSize;
                var messages = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetNewsfeedsForBuyerAndSupplier(userContext.CompanyId, CompanyId, currentPage, pageSize, latestId);

                foreach (var item in messages)
                {
                    var text = item.FeedMessage.Replace(".", "").Split(' ').LastOrDefault(t => t.StartsWith("{") && t.EndsWith("}"));
                    if (!string.IsNullOrWhiteSpace(text))
                    {
                        item.FeedMessage = item.FeedMessage.Replace(text, text.Substring(1, text.Length - 2));
                    }
                    response.Messages.Add(item);
                }

                var totalMessages = Context.DataContext.Newsfeeds.Count(t => (t.RecipientCompanyId == userContext.CompanyId || t.RecipientCompanyId == CompanyId));

                response.TotalMessages = totalMessages;

                decimal totalPage = (decimal)response.TotalMessages / ApplicationConstants.NewsfeedDefaultPageSize;
                response.TotalPages = (int)Math.Ceiling(totalPage);
                response.CurrentPage = currentPage;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NewsfeedDomain", "GetNewsfeedForBuyerAndSupplier", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> SetNewsfeed(NewsfeedParameters parameters)
        {
            var response = new StatusViewModel(Status.Success);
            LogManager.Logger.WriteDebug("NewsfeedDomain", "SetNewsfeed", "Start [" + parameters.ToString() + "]");
            try
            {
                var messages = await GetNewsfeedMessages();
                var eventMessage = messages[(int)parameters.EventTypeId];
                var buyerMessage = eventMessage.GetBuyerMessage(parameters.MessageParameters);
                var supplierMessage = eventMessage.GetSupplierMessage(parameters.MessageParameters);
                var newsfeed = new NewsfeedViewModel
                {
                    EventId = parameters.EventTypeId,
                    EntityId = parameters.EntityId,
                    EntityTypeId = parameters.EntityType,
                    TargetEntityId = parameters.TargetEntityId,
                    CreatedBy = parameters.CreatedByUserId,
                    CreatedDate = parameters.CreatedDate
                };
                await SaveNewsfeed(newsfeed, parameters.BuyerCompanyId, buyerMessage);
                await SaveNewsfeed(newsfeed, parameters.SupplierCompanyId, supplierMessage);
            }
            catch (Exception ex)
            {
                response.StatusCode = Status.Failed;
                LogManager.Logger.WriteException("NewsfeedDomain", "SetNewsfeed", ex.Message, ex);
            }
            LogManager.Logger.WriteDebug("NewsfeedDomain", "SetNewsfeed", "End [" + response.ToString() + "]");
            return response;
        }

        public async Task<StatusViewModel> SetNewsfeedCustomMessage(NewsfeedParameters parameters, string buyerMessage)
        {
            var response = new StatusViewModel(Status.Success);
            LogManager.Logger.WriteDebug("NewsfeedDomain", "SetNewsfeedCustomMessage", "Start [" + parameters.ToString() + "]");
            try
            {
                var newsfeed = new NewsfeedViewModel
                {
                    EventId = parameters.EventTypeId,
                    EntityId = parameters.EntityId,
                    EntityTypeId = parameters.EntityType,
                    TargetEntityId = parameters.TargetEntityId,
                    CreatedBy = parameters.CreatedByUserId,
                    CreatedDate = parameters.CreatedDate
                };
                await SaveNewsfeed(newsfeed, parameters.BuyerCompanyId, buyerMessage);
            }
            catch (Exception ex)
            {
                response.StatusCode = Status.Failed;
                LogManager.Logger.WriteException("NewsfeedDomain", "SetNewsfeedCustomMessage", ex.Message, ex);
            }
            LogManager.Logger.WriteDebug("NewsfeedDomain", "SetNewsfeedCustomMessage", "End [" + response.ToString() + "]");
            return response;
        }

        public async Task SetSplitDropTicketModifiedNewsfeed(UserContext userContext, Invoice invoice, InvoiceEditResponseViewModel viewModel)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = viewModel.OrderId,
                EntityType = EntityType.Order,
                EventTypeId = NewsfeedEvent.SupplierSplitDropTicketUpdatedAutomatically,
                CreatedByUserId = userContext.Id,
                BuyerCompanyId = viewModel.BuyerCompanyId,
                SupplierCompanyId = viewModel.SupplierCompanyId,
                TargetEntityId = viewModel.InvoiceId,
                CreatedDate = invoice.UpdatedDate
            };

            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierCompany, userContext.CompanyName);
            await SetNewsfeed(newsfeedParameters);

            newsfeedParameters.EntityId = invoice.InvoiceHeaderId;
            newsfeedParameters.EntityType = EntityType.DigitalDropTicket;
            await SetNewsfeed(newsfeedParameters);

            if (viewModel.BuyerCompanyId == viewModel.JobCompanyId)
            {
                await SetJobNewsfeed(newsfeedParameters, viewModel.JobId);
            }
        }

        public async Task SetSubcontractorAssetAssignNewsfeed(UserContext userContext, NewsfeedEvent newsFeedEvent, Job job, string subContractorName, string assetName, string prevSubContractorName = "")
        {
            var newsfeedParameters = GetJobNewsfeedCommonParameters(userContext, job);
            var jobDate = DateTimeOffset.Now.ToTargetDateTimeOffset(job.TimeZoneName);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.Date, jobDate.ToString(Resource.constFormatDate));
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.Time, jobDate.ToString(Resource.constFormat12HourTime));

            if (newsFeedEvent == NewsfeedEvent.SubContractorAddedToAsset)
            {
                newsfeedParameters.EventTypeId = NewsfeedEvent.SubContractorAddedToAsset;
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.AccountName, userContext.Name);
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.SubcontractorName, subContractorName);
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.AssetName, assetName);
            }
            else if (newsFeedEvent == NewsfeedEvent.SubContractorRemovedFromAsset)
            {
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.AccountName, userContext.Name);
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.SubcontractorName, subContractorName);
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.AssetName, assetName);
                newsfeedParameters.EventTypeId = NewsfeedEvent.SubContractorRemovedFromAsset;
            }
            else if (newsFeedEvent == NewsfeedEvent.SubContractorUpdatedForAsset)
            {
                newsfeedParameters.EventTypeId = NewsfeedEvent.SubContractorUpdatedForAsset;
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.AccountName, userContext.Name);
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.AssetName, assetName);
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.PreviousSubcontractorName, prevSubContractorName);
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.NewSubcontractorName, subContractorName);
            }

            await SetNewsfeed(newsfeedParameters);
        }

        public async Task SetSubcontractorAssetAssignNewsfeedBulkUpload(UserContext userContext, NewsfeedEvent newsFeedEvent, Job job, int addCount = 0, int updateCount = 0, int deleteCount = 0)
        {
            if (newsFeedEvent == NewsfeedEvent.SubContractorBulkUploadAsset)
            {
                var newsfeedParameters = GetJobNewsfeedCommonParameters(userContext, job);

                newsfeedParameters.EventTypeId = NewsfeedEvent.SubContractorAddedToAsset;
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.AccountName, userContext.Name);

                var jobDate = DateTimeOffset.Now.ToTargetDateTimeOffset(job.TimeZoneName);
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.Date, jobDate.ToString(Resource.constFormatDate));
                newsfeedParameters.MessageParameters.Add(ApplicationConstants.Time, jobDate.ToString(Resource.constFormat12HourTime));
                StringBuilder sb = GetBulkUploadMessage(userContext, addCount, updateCount, deleteCount, jobDate);

                await SetNewsfeedCustomMessage(newsfeedParameters, sb.ToString());
            }
        }

        public async Task OrderClosedOnDateExpiredNewsfeed(Order order)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = order.Id,
                EntityType = EntityType.Order,
                EventTypeId = NewsfeedEvent.OrderClosedOnDateExpired,
                CreatedByUserId = (int)SystemUser.System,
                BuyerCompanyId = order.FuelRequest.User.Company.Id,
                SupplierCompanyId = order.AcceptedCompanyId,
                TargetEntityId = order.Id,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(order.FuelRequest.Job.TimeZoneName)
            };
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, order.PoNumber);

            await SetNewsfeed(newsfeedParameters);
        }

        public async Task BuyerAwardedToSupplierOnQuoteNewsfeed(FuelRequest fuelRequest, UserContext userContext, Quotation quotation, Order order)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = order.Id,
                EntityType = EntityType.Order,
                EventTypeId = NewsfeedEvent.BuyerAwardedToSupplierOnQuote,
                CreatedByUserId = userContext.Id,
                BuyerCompanyId = userContext.CompanyId,
                SupplierCompanyId = quotation.User.Company.Id,
                TargetEntityId = order.Id,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(fuelRequest.Job.TimeZoneName)
            };
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.BuyerCompany, userContext.CompanyName);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierCompany, quotation.User.Company.Name);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.QuoteNumber, quotation.QuoteNumber);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, (order.PoNumber));
            await SetNewsfeed(newsfeedParameters);
        }

        private async Task SaveNewsfeed(NewsfeedViewModel viewModel, int recipientCompanyId, string message)
        {
            if (viewModel != null && recipientCompanyId > 0 && !string.IsNullOrWhiteSpace(message))
            {
                var newsfeed = new NewsfeedViewModel
                {
                    EventId = viewModel.EventId,
                    EntityId = viewModel.EntityId,
                    EntityTypeId = viewModel.EntityTypeId,
                    TargetEntityId = viewModel.TargetEntityId,
                    RecipientCompanyId = recipientCompanyId,
                    FeedMessage = message,
                    CreatedBy = viewModel.CreatedBy,
                    CreatedDate = viewModel.CreatedDate,
                    IsRead = viewModel.IsRead
                };
                var newsfeedEntity = newsfeed.ToEntity();
                Context.DataContext.Newsfeeds.Add(newsfeedEntity);
                await Context.CommitAsync();
            }
        }

        private NewsfeedParameters GetJobNewsfeedCommonParameters(UserContext userContext, Job job)
        {
            return new NewsfeedParameters
            {
                EntityId = job.Id,
                EntityType = EntityType.Job,
                CreatedByUserId = userContext.Id,
                BuyerCompanyId = job.CompanyId,
                SupplierCompanyId = 0,
                TargetEntityId = job.Id,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(job.TimeZoneName)
            };
        }

        private async Task SetJobNewsfeed(NewsfeedParameters newsfeedParameters, int jobId)
        {
            newsfeedParameters.SupplierCompanyId = 0;
            newsfeedParameters.EntityId = jobId;
            newsfeedParameters.EntityType = EntityType.Job;
            await SetNewsfeed(newsfeedParameters);
        }

        private async Task<Dictionary<int, NewsfeedMessage>> GetNewsfeedMessages()
        {
            var cacheKey = "CachedNewsfeedMessages";
            var response = CacheManager.Get<Dictionary<int, NewsfeedMessage>>(cacheKey);
            if (response == null)
            {
                response = await GetMessagesFromDatabase();
                CacheManager.Set(cacheKey, response, 3600 * 8);
            }
            return response;
        }

        private async Task<Dictionary<int, NewsfeedMessage>> GetMessagesFromDatabase()
        {
            var response = new Dictionary<int, NewsfeedMessage>();
            var messages = await Context.DataContext.MstNewsfeedMessages.AsNoTracking().ToListAsync();
            foreach (var item in messages)
            {
                response.Add(item.EventId, new NewsfeedMessage(item.EventId, item.BuyerMessage, item.SupplierMessage, item.TargetUrl));
            }

            return response;
        }

        private static StringBuilder GetBulkUploadMessage(UserContext userContext, int addCount, int updateCount, int deleteCount, DateTimeOffset jobDate)
        {
            StringBuilder sbMessage = new StringBuilder
            (
                ApplicationConstants.BulkUploadAssetSubcontractor.Replace("[AccountName]", userContext.Name)
                                                                    .Replace("[Date]", jobDate.ToString(Resource.constFormatDate))
                                                                    .Replace("[Time]", jobDate.ToString(Resource.constFormat12HourTime))
            );

            if (addCount > 0 && updateCount > 0 && deleteCount > 0)
            {
                sbMessage.Append(AddMessageAssetSubcontractor(addCount));
                sbMessage.Append(", ");
                sbMessage.Append(UpdateMessageAssetSubcontractor(updateCount));
                sbMessage.Append(" and ");
                sbMessage.Append(DeleteMessageAssetSubcontractor(deleteCount));
            }
            else if (addCount > 0 && updateCount > 0 && deleteCount == 0)
            {
                sbMessage.Append(AddMessageAssetSubcontractor(addCount));
                sbMessage.Append(" and ");
                sbMessage.Append(UpdateMessageAssetSubcontractor(updateCount));
            }
            else if (addCount == 0 && updateCount > 0 && deleteCount > 0)
            {
                sbMessage.Append(UpdateMessageAssetSubcontractor(updateCount));
                sbMessage.Append(" and ");
                sbMessage.Append(DeleteMessageAssetSubcontractor(deleteCount));
            }
            else if (addCount > 0 && updateCount == 0 && deleteCount > 0)
            {
                sbMessage.Append(AddMessageAssetSubcontractor(addCount));
                sbMessage.Append(" and ");
                sbMessage.Append(DeleteMessageAssetSubcontractor(deleteCount));
            }
            else if (addCount > 0 && updateCount == 0 && deleteCount == 0)
            {
                sbMessage.Append(AddMessageAssetSubcontractor(addCount));
            }
            else if (addCount == 0 && updateCount > 0 && deleteCount == 0)
            {
                sbMessage.Append(UpdateMessageAssetSubcontractor(updateCount));
            }
            else if (addCount == 0 && updateCount == 0 && deleteCount > 0)
            {
                sbMessage.Append(DeleteMessageAssetSubcontractor(deleteCount));
            }

            return sbMessage;
        }

        private static string DeleteMessageAssetSubcontractor(int deleteCount)
        {
            return ApplicationConstants.BulkUploadAssetSubcontractorDelete.Replace("[SubcontractorDeleteCount]", deleteCount.ToString())
                                                                .Replace("[Verb]", deleteCount == 1 ? "has" : "have");
        }

        private static string UpdateMessageAssetSubcontractor(int updateCount)
        {
            return ApplicationConstants.BulkUploadAssetSubcontractorUpdate.Replace("[SubcontractorUpdateCount]", updateCount.ToString())
                                                                .Replace("[Verb]", updateCount == 1 ? "has" : "have");
        }

        private static string AddMessageAssetSubcontractor(int addCount)
        {
            return ApplicationConstants.BulkUploadAssetSubcontractorAdd.Replace("[SubcontractorAddCount]", addCount.ToString())
                                                                                    .Replace("[Verb]", addCount == 1 ? "has" : "have");
        }

        private async Task UpdateOldInvoicesWithNewIdentity(ManualInvoiceViewModel viewModel)
        {
            var invoiceIds = Context.DataContext.Invoices.Where(t => t.OrderId == viewModel.OrderId && t.InvoiceHeader.InvoiceNumberId == viewModel.InvoiceNumberId && t.IsActive).Select(t => new { t.Id, t.InvoiceHeaderId }).ToList();
            if (invoiceIds.Count > 0)
            {
                var invoiceHeaderIds = invoiceIds.Select(t => t.InvoiceHeaderId).ToList();
                var invoiceNewsfeeds = Context.DataContext.Newsfeeds.Where(t => invoiceHeaderIds.Any(t1 => t1 == t.EntityId) 
                                                                    && (t.EntityTypeId == (int)EntityType.Invoice || t.EntityTypeId == (int)EntityType.DigitalDropTicket)
                                                                    ).ToList();
                invoiceNewsfeeds.ForEach(t => { t.EntityId = viewModel.InvoiceHeaderId; });
                var ids = invoiceIds.Select(t => t.Id).ToList();
                var orderNewsfeeds = Context.DataContext.Newsfeeds.Where(t => t.EntityId == viewModel.OrderId && ids.Any(t1 => t1 == t.TargetEntityId) && t.EntityTypeId == (int)EntityType.Order).ToList();
                orderNewsfeeds.ForEach(t => { t.TargetEntityId = viewModel.InvoiceId; });
            }
            await Context.CommitAsync();
        }

        private static NewsfeedEvent GetInvoiceUpdatedNewsfeedEventType(ManualInvoiceViewModel viewModel)
        {
            return viewModel.IsDigitalDropTicket() ? NewsfeedEvent.SupplierDigitalDropTicketModified : NewsfeedEvent.SupplierInvoiceModified;
        }

        private static EntityType GetInvoiceUpdatedNewsfeedEntityType(ManualInvoiceViewModel viewModel)
        {
            return viewModel.IsDigitalDropTicket() ? EntityType.DigitalDropTicket : EntityType.Invoice;
        }

        public async Task SetOrderCreateOnAcceptOfferNewsfeed(UserContext userContext, FuelRequest fuelRequest, Order order, OfferPricing offerPricing)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = order.Id,
                EntityType = EntityType.Order,
                EventTypeId = NewsfeedEvent.OfferAcceptOrderCreated,
                CreatedByUserId = userContext.Id,
                BuyerCompanyId = userContext.CompanyId,
                SupplierCompanyId = order.AcceptedCompanyId,
                TargetEntityId = order.Id,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(fuelRequest.Job.TimeZoneName)
            };

            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OfferType, offerPricing.MstOfferType.Name);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OfferName, offerPricing.Name);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, order.PoNumber);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.BuyerCompany, userContext.CompanyName);

            await SetNewsfeed(newsfeedParameters);
        }

        public async Task SetDdtToInvoiceCreatedNewsfeed_New(UserContext userContext, ConversionNewsfeedViewModel viewModel, string ddtNumber)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = viewModel.OrderId,
                EntityType = EntityType.Order,
                EventTypeId = NewsfeedEvent.SupplierDigitalDropTicketConvertedtoInvoice,
                CreatedByUserId = userContext.Id,
                BuyerCompanyId = viewModel.BuyerCompanyId,
                SupplierCompanyId = viewModel.SupplierCompanyId,
                TargetEntityId = viewModel.InvoiceId,
                CreatedDate = viewModel.CreatedDate,
            };
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierCompany, userContext.CompanyName);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.InvoiceNumber, viewModel.InvoiceNumber);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.DdtNumber, ddtNumber);
            await SetNewsfeed(newsfeedParameters);


            if (viewModel.InvoiceNumberId > 0)
            {
                var ddt = Context.DataContext.Invoices.Where(t => t.OrderId == viewModel.OrderId && t.InvoiceHeader.InvoiceNumberId == viewModel.InvoiceNumberId && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active).Select(t => new { t.Id, t.InvoiceHeaderId }).FirstOrDefault();
                if (ddt.Id > 0)
                {
                    newsfeedParameters.EntityId = ddt.InvoiceHeaderId;
                    newsfeedParameters.TargetEntityId = ddt.Id;
                    newsfeedParameters.EntityType = EntityType.DigitalDropTicket;
                    await SetNewsfeed(newsfeedParameters);
                }
            }
            newsfeedParameters.TargetEntityId = viewModel.InvoiceId;
            newsfeedParameters.EntityId = viewModel.InvoiceHeaderId;
            newsfeedParameters.EntityType = EntityType.Invoice;
            await SetNewsfeed(newsfeedParameters);

            var order = Context.DataContext.Orders.Where(t => t.Id == viewModel.OrderId).FirstOrDefault();
            if (order.BuyerCompanyId == order.FuelRequest.Job.CompanyId)
            {
                await SetJobNewsfeed(newsfeedParameters, viewModel.JobId);
            }
        }

        public async Task SetThirdPartyOrderEditedNewsfeed(UserContext userContext, Order order, string fieldCurrentValue, string fieldValue)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = order.Id,
                EntityType = EntityType.Order,
                EventTypeId = NewsfeedEvent.OrderFieldsModified,
                CreatedByUserId = userContext.Id,
                BuyerCompanyId = order.BuyerCompanyId,
                SupplierCompanyId = order.AcceptedCompanyId,
                TargetEntityId = order.Id,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(order.FuelRequest.Job.TimeZoneName)
            };
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierCompany, userContext.CompanyName);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.CurrentValue, fieldCurrentValue);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UpdatedValue, fieldValue);
            await SetNewsfeed(newsfeedParameters);
        }
        public async Task SetThirdPartyOrderModifiedNewsfeed(UserContext userContext, Order order, string orderPO)
        {
            var newsfeedParameters = new NewsfeedParameters
            {
                EntityId = order.Id,
                EntityType = EntityType.Order,
                EventTypeId = NewsfeedEvent.SupplierOrderModified,
                CreatedByUserId = userContext.Id,
                BuyerCompanyId = order.BuyerCompanyId,
                SupplierCompanyId = order.AcceptedCompanyId,
                TargetEntityId = order.Id,
                CreatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(order.FuelRequest.Job.TimeZoneName)
            };
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.UserName, userContext.Name);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.SupplierCompany, userContext.CompanyName);
            newsfeedParameters.MessageParameters.Add(ApplicationConstants.OrderNumber, orderPO);
            await SetNewsfeed(newsfeedParameters);
        }

    }
}

