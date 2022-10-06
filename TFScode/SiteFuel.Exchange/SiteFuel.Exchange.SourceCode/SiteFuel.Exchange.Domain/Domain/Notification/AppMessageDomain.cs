using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain
{
    public class AppMessageDomain : BaseDomain
    {
        public AppMessageDomain()
            : base(ContextFactory.Current.ConnectionString)
        {
        }

        public AppMessageDomain(BaseDomain domain)
            : base(domain)
        {
        }

        public async Task<int> GetAppMessageCountAsync(int userId)
        {
            var response = 0;
            try
            {
                response = await Context.DataContext.AppMessageXUserStatuses
                                        .Where(
                                            t =>
                                            t.UserId == userId &&
                                            t.UserTypeId == (int)AppMessageUserType.Recipient &&
                                            t.AppMessageStatusId == (int)AppMessageStatus.Recieved &&
                                            t.IsMarkedAsRead == false)
                                        .CountAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AppMessageDomain", "GetAppMessageCountAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<MailboxMessageCountViewModel> GetMailboxMessagesCountAsync(int userId)
        {
            var response = new MailboxMessageCountViewModel();
            try
            {
                var query = Context.DataContext.AppMessageXUserStatuses.Where(t => t.UserId == userId);

                response.UnreadInboxCount = await query.Where
                (
                    t =>
                    t.UserTypeId == (int)AppMessageUserType.Recipient &&
                    t.AppMessageStatusId == (int)AppMessageStatus.Recieved &&
                    t.IsMarkedAsRead == false
                ).CountAsync();

                response.TotalInboxCount = await query.Where
                (
                    t =>
                    t.UserTypeId == (int)AppMessageUserType.Recipient &&
                    t.AppMessageStatusId == (int)AppMessageStatus.Recieved
                ).CountAsync();

                response.TotalDraftsCount = await query.Where
                (
                    t =>
                    t.AppMessageStatusId == (int)AppMessageStatus.Draft
                ).CountAsync();

                response.ImportantsCount = await query.Where
                (
                    t => t.IsMarkedAsImportant &&
                    t.AppMessageStatusId != (int)AppMessageStatus.Deleted
                ).CountAsync();

                response.TrashCount = await query.Where
                (
                      t => t.AppMessageStatusId == (int)AppMessageStatus.Deleted
                ).CountAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AppMessageDomain", "GetMailboxMessagesCountAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DropdownDisplayItem>> GetRecipientsAsync(RecipientFilterViewModel viewModel)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                var recipientCompanyId = 0;
                if (viewModel.Type == AppMessageQueryType.Order)
                {
                    var order = await Context.DataContext.Orders
                                            .Include(t => t.FuelRequest)
                                            .Include(t => t.Company)
                                            .Include(t => t.BuyerCompany)
                                            .SingleOrDefaultAsync(t => t.Id == viewModel.Number);
                    if (order != null)
                    {
                        recipientCompanyId = (order.Company.Id == viewModel.SendersCompanyId)
                                                ? order.BuyerCompany.Id
                                                : order.Company.Id;
                    }
                }
                else
                {
                    var invoice = await Context.DataContext.Invoices
                                            .Include(t => t.Order)
                                            .Include(t => t.Order.FuelRequest)
                                            .Include(t => t.Order.Company)
                                            .Include(t => t.Order.BuyerCompany)
                                            .SingleOrDefaultAsync(t => t.Id == viewModel.Number && t.Order != null);
                    if (invoice != null)
                    {
                        recipientCompanyId = (invoice.Order.Company.Id == viewModel.SendersCompanyId)
                                                ? invoice.Order.BuyerCompany.Id
                                                : invoice.Order.Company.Id;
                    }
                }

                //get the company users
                var recipientUsers = Context.DataContext.Users.Include(t => t.Company).Where
                (
                    t =>
                    t.Id != viewModel.SendersId &&
                    t.Company != null &&
                    (t.Company.Id == recipientCompanyId || t.Company.Id == viewModel.SendersCompanyId)
                );
                await recipientUsers.ForEachAsync(t => response.Add(new DropdownDisplayItem
                {
                    Id = t.Id,
                    Name = $"{t.FirstName} {t.LastName} [{t.Company.Name}]"
                }));

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AppMessageDomain", "GetRecipientsAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DropdownDisplayItem>> GetRecipientsAsync(int userId, List<int> recipientCompanies)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                if (recipientCompanies != null && recipientCompanies.Count > 0)
                {
                    var recipientUsers = Context.DataContext.Users.Include(t => t.Company).Where
                    (
                        t =>
                        t.Id != userId &&
                        t.Company != null &&
                        recipientCompanies.Contains(t.Company.Id)
                    );
                    await recipientUsers.ForEachAsync(t => response.Add(new DropdownDisplayItem
                    {
                        Id = t.Id,
                        Name = $"{t.FirstName} {t.LastName} [{t.Company.Name}]"
                    }));
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AppMessageDomain", "GetRecipientsAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<ResponseViewModel> SaveAppMessageAsync(int senderId, ComposeMessageViewModel viewModel)
        {
            var response = new ResponseViewModel
            {
                StatusCode = Status.Failed,
                StatusMessage = viewModel.IsDraft ? Resource.errMessageSaveFailed : Resource.errMessageSentFailed
            };
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var message = (AppMessage)null;
                    if (viewModel.Id > 0 && viewModel.ComposeType == AppMessageComposeType.Draft)
                    {
                        message = await Context.DataContext.AppMessages
                                                    .Include(t => t.AppMessageXUserStatuses)
                                                    .Include(t => t.Users)
                                                    .SingleOrDefaultAsync(t => t.Id == viewModel.Id);
                    }

                    if (message != null)
                    {
                        var userStaus = message.AppMessageXUserStatuses.Single();

                        message.Subject = viewModel.Subject;
                        message.Message = viewModel.Message;
                        message.TimeStamp = viewModel.TimeStamp;

                        if (!viewModel.IsDraft)
                        {
                            userStaus.IsMarkedAsRead = true;
                            userStaus.AppMessageStatusId = (int)AppMessageStatus.Sent;

                            //Add Recievers
                            foreach (var item in message.Users)
                            {
                                Context.DataContext.AppMessageXUserStatuses.Add(new AppMessageXUserStatus
                                {
                                    MessageId = message.Id,
                                    UserId = item.Id,
                                    UserTypeId = (int)AppMessageUserType.Recipient,
                                    AppMessageStatusId = (int)AppMessageStatus.Recieved,
                                    IsMarkedAsRead = false,
                                    IsMarkedAsImportant = false,
                                    TimeStamp = viewModel.TimeStamp
                                });
                            }

                            //Finally delete the draft recipient table entries
                            message.Users.Clear();
                        }
                        else
                        {
                            message.Users = Context.DataContext.Users.Where(t => viewModel.Recipients.Contains(t.Id)).ToList();
                        }
                    }
                    else
                    {
                        message = new AppMessage
                        {
                            Subject = viewModel.Subject,
                            Message = viewModel.Message,
                            TimeStamp = viewModel.TimeStamp
                        };

                        Context.DataContext.AppMessages.Add(message);
                        await Context.CommitAsync();

                        //Add sender
                        Context.DataContext.AppMessageXUserStatuses.Add(new AppMessageXUserStatus
                        {
                            MessageId = message.Id,
                            UserId = senderId,
                            UserTypeId = (int)AppMessageUserType.Sender,
                            AppMessageStatusId = viewModel.IsDraft ? (int)AppMessageStatus.Draft : (int)AppMessageStatus.Sent,
                            IsMarkedAsRead = true,
                            IsMarkedAsImportant = false,
                            TimeStamp = viewModel.TimeStamp
                        });

                        if (!viewModel.IsDraft)
                        {
                            //Add Recievers
                            foreach (var item in viewModel.Recipients)
                            {
                                Context.DataContext.AppMessageXUserStatuses.Add(new AppMessageXUserStatus
                                {
                                    MessageId = message.Id,
                                    UserId = item,
                                    UserTypeId = (int)AppMessageUserType.Recipient,
                                    AppMessageStatusId = (int)AppMessageStatus.Recieved,
                                    IsMarkedAsRead = false,
                                    IsMarkedAsImportant = false,
                                    TimeStamp = viewModel.TimeStamp
                                });
                            }
                        }
                        else
                        {
                            message.Users = Context.DataContext.Users.Where(t => viewModel.Recipients.Contains(t.Id)).ToList();
                        }
                    }

                    await Context.CommitAsync();
                    transaction.Commit();

                    if (viewModel.Type == AppMessageQueryType.Dispatch)
                    {
                        await ContextFactory.Current.GetDomain<PushNotificationDomain>().PushNotificationMessage(viewModel);
                    }

                    response.StatusCode = Status.Success;
                    response.StatusMessage = viewModel.IsDraft ? Resource.errMessageSaveSuccess : Resource.errMessageSentSuccess;

                    viewModel.Id = message.Id;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("AppMessageDomain", "SaveAppMessageAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<MailboxMessageFilterViewModel> GetAppMessagesAsync(MailboxMessageFilterViewModel viewModel)
        {
            try
            {
                var query = Context.DataContext.AppMessageXUserStatuses.Include(t => t.AppMessage).Include(t => t.User)
                    .Where(t => t.UserId == viewModel.UserId);

                //Appy filter category
                switch (viewModel.Type)
                {
                    case AppMessageFilterType.Inbox:
                        query = query.Where(t => t.AppMessageStatusId == (int)AppMessageStatus.Recieved);
                        break;
                    case AppMessageFilterType.SentMails:
                        query = query.Where(t => t.AppMessageStatusId == (int)AppMessageStatus.Sent);
                        break;
                    case AppMessageFilterType.Important:
                        query = query.Where(t => t.IsMarkedAsImportant);
                        break;
                    case AppMessageFilterType.Drafts:
                        query = query.Where(t => t.AppMessageStatusId == (int)AppMessageStatus.Draft);
                        break;
                    case AppMessageFilterType.Deleted:
                        query = query.Where(t => t.AppMessageStatusId == (int)AppMessageStatus.Deleted);
                        break;
                }

                //Sort the order of display messages
                query = query.OrderByDescending(t => t.MessageId);

                //Get total message count under filtered category
                viewModel.TotalCount = await query.CountAsync();
                decimal lastPage = (decimal)viewModel.TotalCount / ApplicationConstants.MessagesDefaultPageSize;
                viewModel.LastPage = (int)Math.Ceiling(lastPage);

                //Take only messages displayed on single page - pagination
                var skipCount = (viewModel.CurrentPage - 1) * ApplicationConstants.MessagesDefaultPageSize;
                query = query.Skip(skipCount).Take(ApplicationConstants.MessagesDefaultPageSize);

                viewModel.StartCount = skipCount + 1;
                viewModel.EndCount = skipCount + query.Count();

                foreach (var item in query)
                {
                    var sender = GetSender(item.AppMessage);
                    viewModel.Messages.Add(new MailboxMessageGridViewModel
                    {
                        Id = item.MessageId,
                        SenderName = $"{sender.FirstName} {sender.LastName}",
                        Subject = item.AppMessage.Subject,
                        MessageStatusId = (AppMessageStatus)item.AppMessageStatusId,
                        IsMarkedAsRead = item.IsMarkedAsRead,
                        IsMarkedAsImportant = item.IsMarkedAsImportant,
                        TimeStamp = item.TimeStamp.ToBrowserDateTime(viewModel.TimeZoneOffset)
                    });
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AppMessageDomain", "GetAppMessagesAsync", ex.Message, ex);
            }
            return viewModel;
        }

        public async Task<DispalyMessageViewModel> GetAppMessageAsync(int userId, int messageId, TimeSpan timeZoneOffset)
        {
            var response = new DispalyMessageViewModel();
            try
            {
                var query = Context.DataContext.AppMessageXUserStatuses
                                        .Include(t => t.AppMessage)
                                        .Include(t => t.User)
                                        .Where(t => t.MessageId == messageId);

                var message = await query.SingleOrDefaultAsync(t => t.UserId == userId);
                if (message != null)
                {
                    var sender = GetSender(message.AppMessage);

                    response.Id = message.MessageId;
                    response.Subject = message.AppMessage.Subject;
                    response.From = $"{sender.FirstName} {sender.LastName}";
                    response.TimeStamp = message.TimeStamp.ToBrowserDateTime(timeZoneOffset);
                    response.Message = message.AppMessage.Message;

                    var recipients = query.Where(t => t.UserTypeId == (int)AppMessageUserType.Recipient).Select(t => t.User);
                    await recipients.ForEachAsync(t => response.To.Add($"{t.FirstName} {t.LastName}"));

                    message.IsMarkedAsRead = true;
                    await Context.CommitAsync();

                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageSuccess;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AppMessageDomain", "GetAppMessageAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<ComposeMessageViewModel> GetComposeMessageAsync(int userId, ComposeMessageViewModel viewModel)
        {
            try
            {
                var message = await Context.DataContext.AppMessages.Include(t => t.AppMessageXUserStatuses).Include(t => t.Users).SingleOrDefaultAsync
                (
                    t =>
                    t.Id == viewModel.Id
                );

                if (message != null)
                {
                    viewModel.Id = message.Id;
                    viewModel.Subject = message.Subject;
                    viewModel.Message = message.Message;
                    viewModel.Recipients = message.AppMessageXUserStatuses.Where(t => t.UserId != userId).Select(t => t.UserId).Distinct().ToList();
                    viewModel.RecipientCompanies = message.AppMessageXUserStatuses.Select(t => t.User.Company.Id).Distinct().ToList();

                    var sender = GetSender(message);
                    var from = $"{sender.FirstName} {sender.LastName} ({sender.Email})";

                    var recipients = message.AppMessageXUserStatuses.Where(t => t.UserTypeId == (int)AppMessageUserType.Recipient).Select(t => t.User);
                    var to = recipients.Select(t => $"{t.FirstName} {t.LastName} ({t.Email})").ToList();

                    var appendMessage = $"<p><br /></p><hr><p>Subject: {message.Subject}<br />From: {from}<br />To: {string.Join(";", to)}<br />Date: {message.TimeStamp.ToString(Resource.constFormatDateTime)}</p>";

                    switch (viewModel.ComposeType)
                    {
                        case AppMessageComposeType.Draft:
                            viewModel.Recipients = message.Users.Select(t => t.Id).Distinct().ToList();
                            viewModel.RecipientCompanies.AddRange(message.Users.Select(t => t.Company.Id).Distinct().ToList());
                            break;
                        case AppMessageComposeType.Reply:
                            viewModel.Subject = message.Subject.StartsWith("Re:") ? message.Subject : $"Re:{message.Subject}";
                            viewModel.Message = $"{Uri.EscapeUriString(appendMessage)}{viewModel.Message}";
                            break;
                        case AppMessageComposeType.Forward:
                            viewModel.Recipients.Clear();
                            viewModel.Subject = message.Subject.StartsWith("Fwd:") ? message.Subject : $"Fwd:{message.Subject}";
                            viewModel.Message = $"{Uri.EscapeUriString(appendMessage)}{viewModel.Message}";
                            break;
                    }

                    viewModel.StatusCode = Status.Success;
                    viewModel.StatusMessage = Resource.errMessageSuccess;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AppMessageDomain", "GetComposeMessageAsync", ex.Message, ex);
            }
            return viewModel;
        }

        public async Task<bool> MarkMessagesAsync(int userId, AppMessageMarkingType type, List<int> selectedMessages)
        {
            var response = false;
            try
            {
                var query = Context.DataContext.AppMessageXUserStatuses.Where
                (
                    t =>
                    t.UserId == userId &&
                    selectedMessages.Contains(t.MessageId)
                );

                switch (type)
                {
                    case AppMessageMarkingType.Read:
                        await query.ForEachAsync(t => t.IsMarkedAsRead = true);
                        break;
                    case AppMessageMarkingType.Unread:
                        await query.ForEachAsync(t => t.IsMarkedAsRead = false);
                        break;
                    case AppMessageMarkingType.Important:
                        await query.ForEachAsync(t => t.IsMarkedAsImportant = true);
                        break;
                    case AppMessageMarkingType.Unimportant:
                        await query.ForEachAsync(t => t.IsMarkedAsImportant = false);
                        break;
                    case AppMessageMarkingType.Deleted:
                        foreach (var item in query)
                        {
                            var appMessage = item.AppMessage;
                            switch ((AppMessageStatus)item.AppMessageStatusId)
                            {
                                case AppMessageStatus.Draft:
                                    Context.DataContext.AppMessageXUserStatuses.Remove(item);
                                    appMessage.Users.Clear();
                                    Context.DataContext.AppMessages.Remove(appMessage);
                                    break;
                                case AppMessageStatus.Sent:
                                case AppMessageStatus.Recieved:
                                    item.AppMessageStatusId = (int)AppMessageStatus.Deleted;
                                    item.IsMarkedAsImportant = false;
                                    item.IsMarkedAsRead = true;
                                    break;
                                case AppMessageStatus.Deleted:
                                    if (item.AppMessage.AppMessageXUserStatuses.Count == 1)
                                    {
                                        Context.DataContext.AppMessageXUserStatuses.Remove(item);
                                        Context.DataContext.AppMessages.Remove(appMessage);
                                    }
                                    else
                                    {
                                        Context.DataContext.AppMessageXUserStatuses.Remove(item);
                                    }
                                    break;
                            }
                        }
                        break;
                }
                await Context.CommitAsync();
                response = true;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AppMessageDomain", "MarkMessagesAsync", ex.Message, ex);
            }
            return response;
        }

        private User GetSender(AppMessage entity)
        {
            return entity.AppMessageXUserStatuses.Single(t1 => t1.UserTypeId == (int)AppMessageUserType.Sender).User;
        }

        public async Task<List<DispalyMessageViewModel>> GetUnreadAppMessagesAsync(int userId, TimeSpan timeZoneOffset)
        {
            var response = new List<DispalyMessageViewModel>();
            try
            {
                var helperDomain = new HelperDomain(this);

                var unreadMsgs = await Context.DataContext.AppMessageXUserStatuses
                                        .Where(
                                            t =>
                                            t.UserId == userId &&
                                            t.UserTypeId == (int)AppMessageUserType.Recipient &&
                                            t.AppMessageStatusId == (int)AppMessageStatus.Recieved &&
                                            !t.IsMarkedAsRead)
                                        .Select(t => new
                                        {
                                            t.MessageId,
                                            t.AppMessage.Subject,
                                            t.TimeStamp,
                                            t.AppMessage.AppMessageXUserStatuses.FirstOrDefault(t1 => t1.UserTypeId == (int)AppMessageUserType.Sender).User.FirstName
                                        })
                                        .OrderByDescending(t => t.TimeStamp).ToListAsync();
                unreadMsgs.ForEach(t => response.Add(new DispalyMessageViewModel
                {
                    Id = t.MessageId,
                    Subject = t.Subject,
                    TimeStamp = t.TimeStamp.ToBrowserDateTime(timeZoneOffset),
                    From = t.FirstName,
                    DaysAgo = helperDomain.GetNewMessageReceivedTime(t.TimeStamp.ToBrowserDateTime(timeZoneOffset)),
                    HoursAgo = Math.Round((DateTimeOffset.Now.Subtract(t.TimeStamp.ToBrowserDateTime(timeZoneOffset))).TotalHours) > 0 && Math.Round((DateTimeOffset.Now.Subtract(t.TimeStamp.ToBrowserDateTime(timeZoneOffset))).TotalHours) <= 72 ? Math.Round((DateTimeOffset.Now.Subtract(t.TimeStamp.ToBrowserDateTime(timeZoneOffset))).TotalHours) + "h ago" : ""
                }));
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AppMessageDomain", "GetUnreadAppMessagesAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task SendDeliveryScheduleAddedMessage(UserContext userContext, IEnumerable<DeliverySchedule> schedules, Order order)
        {
            try
            {
                var deliveryMessage = GetDeliveryMessageDetails(userContext, schedules, order);
                var subject = GetFormatedSubject(Resource.msgDeliveryScheduleAddedSubject, deliveryMessage);
                var message = GetFormatedMessage(Resource.msgDeliveryScheduleAddedBodyText, deliveryMessage);

                var helperDomain = new HelperDomain(this);
                var deliveryWindow = helperDomain.GetAddedScheduleDetails(deliveryMessage.CurrentSchedules);
                var messageWithDeliveryDetails = message.Replace("[DeliveryWindow]", deliveryWindow.ToString());

                await SendDeliveryMessage(deliveryMessage, messageWithDeliveryDetails, subject, userContext.Id);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AppMessageDomain", "SendDeliveryScheduleAddedMessage", ex.Message, ex);
            }
        }

        public async Task SendDeliveryScheduleModifiedMessage(UserContext userContext, IEnumerable<DeliverySchedule> allSchedules, IEnumerable<DeliverySchedule> schedules, Order order)
        {
            try
            {
                var modifiedGroups = schedules.Where(t => t.StatusId == (int)DeliveryScheduleStatus.Modified).Select(t => t.GroupId);
                var Modifiedschedules = allSchedules.Where(t => modifiedGroups.Contains(t.GroupId));
                var deliveryMessage = GetDeliveryMessageDetails(userContext, Modifiedschedules, order);
                var subject = GetFormatedSubject(Resource.msgDeliveryScheduleModifiedSubject, deliveryMessage);
                var message = GetFormatedMessage(Resource.msgDeliveryScheduleModifiedBodyText, deliveryMessage);

                var helperDomain = new HelperDomain(this);
                var deliveryWindow = helperDomain.GetModifiedScheduleDetails(deliveryMessage.CurrentSchedules, deliveryMessage.PreviousSchedules);
                var messageWithDeliveryDetails = message.Replace("[DeliveryWindow]", deliveryWindow);

                await SendDeliveryMessage(deliveryMessage, messageWithDeliveryDetails, subject, userContext.Id);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AppMessageDomain", "SendDeliveryScheduleModifiedMessage", ex.Message, ex);
            }
        }

        public async Task SendDeliveryScheduleRescheduledMessage(UserContext userContext, IEnumerable<DeliverySchedule> allSchedules, IEnumerable<DeliverySchedule> schedules, Order order)
        {
            try
            {
                var rescheduledGroups = schedules.Where(t => t.StatusId == (int)DeliveryScheduleStatus.Rescheduled).Select(t => t.GroupId);
                var rescheduledSchedules = allSchedules.Where(t => rescheduledGroups.Contains(t.GroupId));
                var deliveryMessage = GetDeliveryMessageDetails(userContext, rescheduledSchedules, order);
                var subject = GetFormatedSubject(Resource.msgDeliveryScheduleRescheduledSubject, deliveryMessage);
                var message = GetFormatedMessage(Resource.msgDeliveryScheduleRescheduledBodyText, deliveryMessage);

                var helperDomain = new HelperDomain(this);
                var deliveryWindow = helperDomain.GetRescheduledScheduleDetails(deliveryMessage.CurrentSchedules);
                var messageWithDeliveryDetails = message.Replace("[DeliveryWindow]", deliveryWindow);

                await SendDeliveryMessage(deliveryMessage, messageWithDeliveryDetails, subject, userContext.Id);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AppMessageDomain", "SendDeliveryScheduleModifiedMessage", ex.Message, ex);
            }
        }

        public async Task SendDraftDDTMessage(Order order, InvoiceViewModel invoiceViewModel)
        {
            try
            {
                var helperDomain = new HelperDomain(this);

                var driver = Context.DataContext.Users.First(t => t.Id == invoiceViewModel.UserId);
                var drivername = $"{driver.FirstName} {driver.LastName}";
                var Date = invoiceViewModel.DropStartDate.ToString(Resource.constFormatDate);
                var StartTime = invoiceViewModel.DropStartDate.ToString(Resource.constFormat12HourTime);
                var EndTime = invoiceViewModel.DropEndDate.ToString(Resource.constFormat12HourTime);
                var OrderNumber = order.PoNumber;
                var DdtNumber = invoiceViewModel.InvoiceNumber.Number.Replace(ApplicationConstants.SFIN, ApplicationConstants.SFDD);

                var serverUrl = helperDomain.GetServerUrl();
                var targetUrl = serverUrl + "Supplier/Invoice/Details/" + invoiceViewModel.Id;

                var user = Context.DataContext.Users.First(t => t.Id == order.AcceptedBy);
                var accountName = $"{user.FirstName} {user.LastName}";

                var composeMessageViewModel = new ComposeMessageViewModel
                {
                    ComposeType = AppMessageComposeType.Compose,
                    Type = AppMessageQueryType.DDT,
                    Number = invoiceViewModel.Id,
                    Subject = string.Format(Resource.msgDraftDigitalDropTicketSubject, drivername, Date, StartTime, EndTime, OrderNumber, DdtNumber),
                    Message = string.Format(Resource.msgDraftDigitalDropTicketBody, accountName, drivername, Date, StartTime, EndTime, OrderNumber, DdtNumber, targetUrl),
                    TimeStamp = DateTimeOffset.Now,
                };

                composeMessageViewModel.Recipients.Add(order.AcceptedBy);
                await SaveAppMessageAsync((int)SystemUser.System, composeMessageViewModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AppMessageDomain", "SendDraftDDTMessage", ex.Message, ex);
            }
        }

        public async Task SendUnassignedDigitalDropTicketMessage(InvoiceViewModel invoiceViewModel)
        {
            try
            {
                var helperDomain = new HelperDomain(this);

                var driver = Context.DataContext.Users.First(t => t.Id == invoiceViewModel.UserId);
                if (driver != null)
                {
                    var drivername = $"{driver.FirstName} {driver.LastName}";
                    var droppedGallons = invoiceViewModel.DroppedGallons.GetPreciseValue(2).GetCommaSeperatedValue();
                    var Date = invoiceViewModel.DropStartDate.ToString(Resource.constFormatDate);
                    var StartTime = invoiceViewModel.DropStartDate.ToString(Resource.constFormat12HourTime);
                    var EndTime = invoiceViewModel.DropEndDate.ToString(Resource.constFormat12HourTime);
                    var DdtNumber = invoiceViewModel.InvoiceNumber.Number.Replace(ApplicationConstants.SFIN, ApplicationConstants.SFDD);

                    var serverUrl = helperDomain.GetServerUrl();
                    var targetUrl = serverUrl + "Supplier/Invoice/AssignToOrder?invoiceId=" + invoiceViewModel.Id;

                    var company = Context.DataContext.Companies.FirstOrDefault(t => t.Id == driver.CompanyId);
                    if (company != null)
                    {
                        var accountName = $"{company.Name}";

                        var composeMessageViewModel = new ComposeMessageViewModel
                        {
                            ComposeType = AppMessageComposeType.Compose,
                            Type = AppMessageQueryType.DDT,
                            Number = invoiceViewModel.Id,
                            Subject = string.Format(Resource.msgUnassignedDigitalDropTicketSubject, DdtNumber),
                            Message = string.Format(Resource.msgUnassignedDigitalDropTicketBody, accountName, drivername, droppedGallons, Date, StartTime, EndTime, DdtNumber, targetUrl),
                            TimeStamp = DateTimeOffset.Now
                        };

                        var eligibleUsers = company.Users.Where(
                                                        t => t.IsActive && t.MstRoles.Any(t1 => t1.Id == (int)UserRoles.Admin ||
                                                        t1.Id == (int)UserRoles.SuperAdmin || t1.Id == (int)UserRoles.Supplier)).Select(t1 => t1.Id).ToList();

                        composeMessageViewModel.Recipients = eligibleUsers;
                        await SaveAppMessageAsync((int)SystemUser.System, composeMessageViewModel);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AppMessageDomain", "SendUnassignedDigitalDropTicketMessage", ex.Message, ex);
            }
        }

        public async Task SendUnassignedDdtToOrderLinkedMessage(InvoiceViewModel invoiceViewModel, Order order, CompanyFilterType companyType, int userId)
        {
            try
            {
                var driver = Context.DataContext.Users.FirstOrDefault(t => t.Id == invoiceViewModel.DriverId);
                var supplierUser = Context.DataContext.Users.FirstOrDefault(t => t.Id == userId);
                if (driver != null)
                {
                    var drivername = $"{driver.FirstName} {driver.LastName}";
                    var supplierCompany = driver.Company.Name;
                    var droppedGallons = invoiceViewModel.DroppedGallons.GetPreciseValue(2).GetCommaSeperatedValue();
                    var Date = invoiceViewModel.DropStartDate.ToString(Resource.constFormatDate);
                    var StartTime = invoiceViewModel.DropStartDate.ToString(Resource.constFormat12HourTime);
                    var EndTime = invoiceViewModel.DropEndDate.ToString(Resource.constFormat12HourTime);
                    var PoNumber = order.PoNumber;
                    var DdtNumber = invoiceViewModel.InvoiceNumber.Number.Replace(ApplicationConstants.SFIN, ApplicationConstants.SFDD);

                    var supplierName = $"{supplierUser.FirstName} {supplierUser.LastName}";
                    switch (companyType)
                    {
                        case CompanyFilterType.Buyer:
                            var company = Context.DataContext.Companies.FirstOrDefault(t => t.Id == order.FuelRequest.User.Company.Id);
                            if (company != null)
                            {
                                var buyerMessage = string.Empty;
                                if (invoiceViewModel.IsTaxServiceFailure)
                                {
                                    buyerMessage = string.Format(Resource.msgBuyerUnassignedDDTToOrderWaitingForTaxLinkBody, supplierName, supplierCompany, DdtNumber, PoNumber);
                                }
                                else
                                {
                                    buyerMessage = string.Format(Resource.msgUnassignedDDTToOrderLinkBody, drivername, supplierCompany, droppedGallons, Date, StartTime, EndTime, PoNumber, DdtNumber);
                                }
                                var composeMessageViewModel = new ComposeMessageViewModel
                                {
                                    ComposeType = AppMessageComposeType.Compose,
                                    Type = AppMessageQueryType.DDT,
                                    Number = invoiceViewModel.Id,
                                    Subject = string.Format(Resource.msgUnassignedDigitalDropTicketToOrderLinkSubject, DdtNumber, PoNumber),
                                    Message = buyerMessage,
                                    TimeStamp = DateTimeOffset.Now
                                };

                                var eligibleUsers = company.Users.Where(t => t.IsActive && t.MstRoles.Any(t1 => t1.Id == (int)UserRoles.BuyerAdmin ||
                                                                                                                t1.Id == (int)UserRoles.Admin ||
                                                                                                                t1.Id == (int)UserRoles.Buyer))
                                                                                                     .Select(t1 => t1.Id).ToList();

                                composeMessageViewModel.Recipients = eligibleUsers;
                                await SaveAppMessageAsync((int)SystemUser.System, composeMessageViewModel);
                            }
                            break;
                        case CompanyFilterType.Supplier:
                            var supplier = Context.DataContext.Companies.FirstOrDefault(t => t.Id == order.AcceptedCompanyId);
                            if (supplier != null)
                            {
                                var supplieMessage = string.Empty;
                                if (invoiceViewModel.IsTaxServiceFailure)
                                {
                                    supplieMessage = string.Format(Resource.msgSupplierUnassignedDDTToOrderWaitingForTaxLinkBody, supplierName, DdtNumber, PoNumber);
                                }
                                else
                                {
                                    supplieMessage = string.Format(Resource.msgSupplierUnassignedDDTToOrderLinkBody, supplierName, DdtNumber, PoNumber);
                                }
                                var composeMessageViewModel = new ComposeMessageViewModel
                                {
                                    ComposeType = AppMessageComposeType.Compose,
                                    Type = AppMessageQueryType.DDT,
                                    Number = invoiceViewModel.Id,
                                    Subject = string.Format(Resource.msgUnassignedDigitalDropTicketToOrderLinkSubject, DdtNumber, PoNumber),
                                    Message = supplieMessage,
                                    TimeStamp = DateTimeOffset.Now
                                };

                                var eligibleUsers = supplier.Users.Where(t => t.IsActive && t.MstRoles.Any(t1 => t1.Id == (int)UserRoles.SupplierAdmin ||
                                                                                                                 t1.Id == (int)UserRoles.Admin ||
                                                                                                                t1.Id == (int)UserRoles.Supplier))
                                                                                                     .Select(t1 => t1.Id).ToList();

                                composeMessageViewModel.Recipients = eligibleUsers;
                                await SaveAppMessageAsync((int)SystemUser.System, composeMessageViewModel);
                            }
                            break;
                        default:
                            break;
                    }

                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AppMessageDomain", "SendUnassignedDdtToOrderLinkedMessage", ex.Message, ex);
            }
        }

        public async Task SendUnassignedDdtToOrderLinkedInvoiceGenerateMessage(InvoiceViewModel invoiceViewModel, Order order, CompanyFilterType companyType, int userId)
        {
            try
            {
                var driver = Context.DataContext.Users.FirstOrDefault(t => t.Id == invoiceViewModel.DriverId);
                var supplierUser = Context.DataContext.Users.FirstOrDefault(t => t.Id == userId);
                if (driver != null)
                {
                    var drivername = $"{driver.FirstName} {driver.LastName}";
                    var supplierCompany = driver.Company.Name;
                    var droppedGallons = invoiceViewModel.DroppedGallons.GetPreciseValue(2).GetCommaSeperatedValue();
                    var Date = invoiceViewModel.DropStartDate.ToString(Resource.constFormatDate);
                    var StartTime = invoiceViewModel.DropStartDate.ToString(Resource.constFormat12HourTime);
                    var EndTime = invoiceViewModel.DropEndDate.ToString(Resource.constFormat12HourTime);
                    var PoNumber = order.PoNumber;
                    var InvoiceNumber = invoiceViewModel.InvoiceNumber.Number;
                    var DdtNumber = invoiceViewModel.InvoiceNumber.Number.Replace(ApplicationConstants.SFIN, ApplicationConstants.SFDD);
                    var supplierName = $"{supplierUser.FirstName} {supplierUser.LastName}";
                    switch (companyType)
                    {
                        case CompanyFilterType.Buyer:
                            var company = Context.DataContext.Companies.FirstOrDefault(t => t.Id == order.FuelRequest.User.Company.Id);
                            if (company != null)
                            {
                                var composeMessageViewModel = new ComposeMessageViewModel
                                {
                                    ComposeType = AppMessageComposeType.Compose,
                                    Type = AppMessageQueryType.DDT,
                                    Number = invoiceViewModel.Id,
                                    Subject = string.Format(Resource.msgUnassignedDDTToOrderLinkInvoiceGenerateSubject, InvoiceNumber, PoNumber),
                                    Message = string.Format(Resource.msgUnassignedDDTToOrderLinkInvoiceGenerateBody, drivername, supplierCompany, droppedGallons, Date, StartTime, EndTime, PoNumber, InvoiceNumber),
                                    TimeStamp = DateTimeOffset.Now
                                };

                                var eligibleUsers = company.Users.Where(t => t.IsActive && t.MstRoles.Any(t1 => t1.Id == (int)UserRoles.BuyerAdmin ||
                                                                                                                t1.Id == (int)UserRoles.Admin ||
                                                                                                                t1.Id == (int)UserRoles.Buyer))
                                                                                                     .Select(t1 => t1.Id).ToList();

                                composeMessageViewModel.Recipients = eligibleUsers;
                                await SaveAppMessageAsync((int)SystemUser.System, composeMessageViewModel);
                            }
                            break;
                        case CompanyFilterType.Supplier:
                            var supplier = Context.DataContext.Companies.FirstOrDefault(t => t.Id == order.AcceptedCompanyId);
                            if (supplier != null)
                            {
                                var composeMessageViewModel = new ComposeMessageViewModel
                                {
                                    ComposeType = AppMessageComposeType.Compose,
                                    Type = AppMessageQueryType.DDT,
                                    Number = invoiceViewModel.Id,
                                    Subject = string.Format(Resource.msgUnassignedDDTToOrderLinkInvoiceGenerateSubject, InvoiceNumber, PoNumber),
                                    Message = string.Format(Resource.msgUnassignedDDTToOrderLinkInvoiceGenerateSupplierBody, supplierName, DdtNumber, PoNumber, InvoiceNumber),
                                    TimeStamp = DateTimeOffset.Now
                                };

                                var eligibleUsers = supplier.Users.Where(t => t.IsActive && t.MstRoles.Any(t1 => t1.Id == (int)UserRoles.SupplierAdmin ||
                                                                                                                t1.Id == (int)UserRoles.Admin ||
                                                                                                                t1.Id == (int)UserRoles.Supplier))
                                                                                                     .Select(t1 => t1.Id).ToList();

                                composeMessageViewModel.Recipients = eligibleUsers;
                                await SaveAppMessageAsync((int)SystemUser.System, composeMessageViewModel);
                            }
                            break;
                        default:
                            break;
                    }

                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AppMessageDomain", "SendUnassignedDdtToOrderLinkedInvoiceGenerateMessage", ex.Message, ex);
            }
        }

        private DeliveryScheduleMessageViewModel GetDeliveryMessageDetails(UserContext userContext, IEnumerable<DeliverySchedule> schedules, Order order)
        {
            var helperDomain = new HelperDomain(this);
            var serverUrl = helperDomain.GetServerUrl();
            var response = new DeliveryScheduleMessageViewModel();
            try
            {
                response.CompanyName = userContext.CompanyName;
                response.CompanyUserName = userContext.Name;
                response.PoNumber = order.PoNumber;
                if (userContext.IsBuyerCompany || (userContext.IsBuyerAndSupplierCompany && userContext.CompanySubTypeId == CompanyType.Buyer))
                {
                    response.RecipientUserId = order.User.Id;
                    response.RecipientUserName = $"{order.User.FirstName} {order.User.LastName}";
                    response.TargetUrl = serverUrl + "Supplier/Order/Details/" + order.Id;
                }
                else
                {
                    if (order.BuyerCompanyId == order.FuelRequest.User.Company.Id)
                    {
                        response.RecipientUserId = order.FuelRequest.User.Id;
                        response.RecipientUserName = $"{order.FuelRequest.User.FirstName} {order.FuelRequest.User.LastName}";
                    }
                    else
                    {
                        response.RecipientUserId = order.FuelRequest.FuelRequest1.User.Id;
                        response.RecipientUserName = $"{order.FuelRequest.FuelRequest1.User.FirstName} {order.FuelRequest.FuelRequest1.User.LastName}";
                    }
                    response.TargetUrl = serverUrl + "Buyer/Order/Details/" + order.Id;
                }
                var scheduleGroups = schedules.GroupBy(t => t.GroupId).Select(g => new { Items = g.ToList() });
                var scheduleList = new List<DeliveryScheduleDetail>();
                foreach (var schedule in scheduleGroups)
                {
                    var first = schedule.Items.First();
                    var scheduleDetail = new DeliveryScheduleDetail
                    {
                        Date = first.Date.ToString(Resource.constFormatDate),
                        DayNames = schedule.Items.Select(t => t.Date.ToString("ddd")),
                        Start = first.Date.Add(first.StartTime).ToString(Resource.constFormat12HourTime),
                        End = first.Date.Add(first.EndTime).ToString(Resource.constFormat12HourTime),
                        Gallons = first.Quantity.GetPreciseValue(2).GetCommaSeperatedValue(),
                        GroupId = first.GroupId,
                        StatusId = first.StatusId,
                        Type = first.Type,
                        RescheduledTrackableId = first.RescheduledTrackableId
                    };
                    scheduleList.Add(scheduleDetail);
                }
                response.CurrentSchedules = scheduleList;

                var currentScheduleGroups = schedules.Select(t => t.GroupId).Distinct();
                response.PreviousSchedules = order.OrderDeliverySchedules
                .Where(t => t.DeliveryRequestId.HasValue && currentScheduleGroups.Contains(t.DeliverySchedule.GroupId))
                .Select(t => t.DeliverySchedule).GroupBy(t => t.GroupId)
                .Select(g => new { Items = g.ToList() }).Select(t => t.Items.ToViewModel())
                .OrderBy(t => t.ScheduleDate).Select(t => new DeliveryScheduleDetail
                {
                    Date = t.ScheduleDate.ToString(Resource.constFormatDate),
                    DayNames = t.ScheduleDayNames,
                    Start = t.ScheduleStartTime,
                    End = t.ScheduleEndTime,
                    Gallons = t.ScheduleQuantity.GetPreciseValue(2).GetCommaSeperatedValue(),
                    GroupId = t.GroupId,
                    StatusId = t.StatusId,
                    Type = t.ScheduleType,
                    RescheduledTrackableId = t.RescheduledTrackableId
                });
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AppMessageDomain", "GetDeliveryMessageDetails", ex.Message, ex);
            }

            return response;
        }

        private string GetFormatedMessage(string template, DeliveryScheduleMessageViewModel message)
        {
            try
            {
                template = template.Replace("[RecipientUserName]", message.RecipientUserName);
                template = template.Replace("[CompanyUserName]", message.CompanyUserName);
                template = template.Replace("[CompanyName]", message.CompanyName);
                template = template.Replace("[PoNumber]", message.PoNumber);
                template = template.Replace("[TargetUrl]", message.TargetUrl);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AppMessageDomain", "GetFormatedMessage", ex.Message, ex);
            }

            return template;
        }

        private string GetFormatedSubject(string subject, DeliveryScheduleMessageViewModel message)
        {
            try
            {
                subject = subject.Replace(ApplicationConstants.UserName, message.CompanyUserName);
                subject = subject.Replace("[CompanyName]", message.CompanyName);
                subject = subject.Replace("[OrderNumber]", message.PoNumber);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AppMessageDomain", "GetFormatedSubject", ex.Message, ex);
            }

            return subject;
        }

        private async Task SendDeliveryMessage(DeliveryScheduleMessageViewModel messageModel, string message, string subject, int senderId)
        {
            if (!string.IsNullOrWhiteSpace(message) && !string.IsNullOrWhiteSpace(subject))
            {
                var messageViewModel = new ComposeMessageViewModel
                {
                    ComposeType = AppMessageComposeType.Compose,
                    Message = message,
                    Subject = subject,
                    TimeStamp = DateTimeOffset.Now
                };
                messageViewModel.Recipients.Add(messageModel.RecipientUserId);
                await SaveAppMessageAsync(senderId, messageViewModel);
            }
        }

        public async Task SendDeliverySchedulePublishMessage(UserContext userContext, List<DSPublishMessageViewModel> dSPublishMessageViewModels)
        {
            try
            {
                foreach (var item in dSPublishMessageViewModels)
                {
                    foreach (var dr in item.DeliveryRequests)
                    {
                        if (dr.OrderId > 0)
                        {
                            var order = await Context.DataContext.Orders.Include(t => t.FuelRequest)
                                                                    .Include(t => t.FuelRequest.FuelRequestDetail)
                                                                  .SingleOrDefaultAsync(t => t.Id == dr.OrderId);
                            if (item.DeliveryGroupPrevStatus != DeliveryGroupStatus.Published)
                            {
                                await DeliverySchedulePublished(userContext, item, dr, order);
                            }
                            else
                            {
                                await DeliveryScheduleModified(userContext, item, dr, order);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AppMessageDomain", "SendDeliverySchedulePublishMessage", ex.Message, ex);
            }
        }

        private async Task DeliverySchedulePublished(UserContext userContext, DSPublishMessageViewModel item, DSDeliveryRequestMessageViewModel dr, Order order)
        {
            var deliveryMessage = GetDeliveryMessagePublishDetails(userContext, item, order);
            var subject = GetFormatedSubject(Resource.msgDeliveryScheduleAddedSubject, deliveryMessage);
            var message = GetFormatedMessage(Resource.msgDeliveryScheduleAddedBodyText, deliveryMessage);
            var deliveryWindow = $"<li><b>{Resource.lblSpecificDates}: {item.StartDate} - Delivery Window: {item.ShiftStartTime} to {item.ShiftEndTime} - {dr.RequiredQuantity} {Enum.GetName(typeof(UoM), dr.UoM)}, {dr.ProductType}</b></li>";
            var messageWithDeliveryDetails = message.Replace("[DeliveryWindow]", deliveryWindow.ToString());
            await SendDeliveryMessage(deliveryMessage, messageWithDeliveryDetails, subject, userContext.Id);
        }

        private async Task DeliveryScheduleModified(UserContext userContext, DSPublishMessageViewModel item, DSDeliveryRequestMessageViewModel dr, Order order)
        {
            var deliveryMessage = GetDeliveryMessagePublishDetails(userContext, item, order);
            var subject = GetFormatedSubject(Resource.msgDeliveryScheduleModifiedSubject, deliveryMessage);
            var message = GetFormatedMessage(Resource.msgDeliveryScheduleModifiedBodyText, deliveryMessage);
            var deliveryWindow = $"<li><b>{Resource.lblSpecificDates}: {item.StartDate} - Delivery Window: {item.ShiftStartTime} to {item.ShiftEndTime} - {dr.RequiredQuantity} {Enum.GetName(typeof(UoM), dr.UoM)}, {dr.ProductType}</b></li>";
            var messageWithDeliveryDetails = message.Replace("[DeliveryWindow]", deliveryWindow.ToString());
            await SendDeliveryMessage(deliveryMessage, messageWithDeliveryDetails, subject, userContext.Id);
        }

        private DeliveryScheduleMessageViewModel GetDeliveryMessagePublishDetails(UserContext userContext, DSPublishMessageViewModel dSPublishMessageViewModel, Order order)
        {
            var helperDomain = new HelperDomain(this);
            var serverUrl = helperDomain.GetServerUrl();
            var response = new DeliveryScheduleMessageViewModel();
            try
            {
                response.CompanyName = userContext.CompanyName;
                response.CompanyUserName = userContext.Name;
                response.PoNumber = order.PoNumber;
                if (userContext.IsBuyerCompany || (userContext.IsBuyerAndSupplierCompany && userContext.CompanySubTypeId == CompanyType.Buyer))
                {
                    response.RecipientUserId = order.User.Id;
                    response.RecipientUserName = $"{order.User.FirstName} {order.User.LastName}";
                    response.TargetUrl = serverUrl + "Supplier/Order/Details/" + order.Id;
                }
                else
                {
                    if (order.BuyerCompanyId == order.FuelRequest.User.Company.Id)
                    {
                        response.RecipientUserId = order.FuelRequest.User.Id;
                        response.RecipientUserName = $"{order.FuelRequest.User.FirstName} {order.FuelRequest.User.LastName}";
                    }
                    else
                    {
                        response.RecipientUserId = order.FuelRequest.FuelRequest1.User.Id;
                        response.RecipientUserName = $"{order.FuelRequest.FuelRequest1.User.FirstName} {order.FuelRequest.FuelRequest1.User.LastName}";
                    }
                    response.TargetUrl = serverUrl + "Buyer/Order/Details/" + order.Id;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AppMessageDomain", "GetDeliveryMessagePublishDetails", ex.Message, ex);
            }

            return response;
        }

        public async Task SendInvoiceDDTMessage(int userId, string companyName, InvoiceModel invoice)
        {
            try
            {
                var helperDomain = new HelperDomain(this);
                var order = await Context.DataContext.Orders.Where(t => t.Id == invoice.OrderId).Select(t => new { PoNumber = t.PoNumber, AcceptedBy = t.AcceptedBy,CreatedBy = t.FuelRequest.User.Id, Buyer = t.FuelRequest.User}).FirstOrDefaultAsync();
                var Date = invoice.DropStartDate.ToString(Resource.constFormatDate);
                var StartTime = invoice.DropStartDate.ToString(Resource.constFormat12HourTime);
                var EndTime = invoice.DropEndDate.ToString(Resource.constFormat12HourTime);
                var serverUrl = helperDomain.GetServerUrl();        
                var targetUrl = "";
                var buyerName = $"{order.Buyer.FirstName} {order.Buyer.LastName}";

                var composeMessageViewModel = new ComposeMessageViewModel();
                targetUrl = serverUrl + "Buyer/Invoice/Details/" + invoice.Id;
                composeMessageViewModel.ComposeType = AppMessageComposeType.Compose;
                composeMessageViewModel.Number = invoice.Id;

                if (IsDigitalDropTicket(invoice.InvoiceTypeId))
                {                 
                    composeMessageViewModel.Type = AppMessageQueryType.DDT;
                    composeMessageViewModel.Subject = string.Format(Resource.msgDigitalDropTicketSubject, invoice.DisplayInvoiceNumber, order.PoNumber, companyName);
                    composeMessageViewModel.Message = string.Format(Resource.msgDigitalDropTicketBody, buyerName, invoice.DisplayInvoiceNumber, order.PoNumber, Date, StartTime, EndTime, companyName, targetUrl);
                    composeMessageViewModel.TimeStamp = DateTimeOffset.Now;
                }
                else
                {
                    composeMessageViewModel.Type = AppMessageQueryType.Invoice;
                    composeMessageViewModel.Subject = string.Format(Resource.msgInvoiceCreatedSubject, invoice.DisplayInvoiceNumber, order.PoNumber, companyName);
                    composeMessageViewModel.Message = string.Format(Resource.msgInvoiceCreatedBody, buyerName, invoice.DisplayInvoiceNumber, order.PoNumber, Date, StartTime, EndTime, companyName, targetUrl);
                    composeMessageViewModel.TimeStamp = DateTimeOffset.Now;
                }

                composeMessageViewModel.Recipients.Add(order.CreatedBy);
                await SaveAppMessageAsync(userId, composeMessageViewModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AppMessageDomain", "SendInvoiceDDTMessage", ex.Message, ex);
            }
        }
        public static bool IsDigitalDropTicket(int invoiceTypeId)
        {
            return invoiceTypeId == (int)InvoiceType.DigitalDropTicketManual || invoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp;
        }

        public async Task<List<MailboxMessageGridViewModel>> GetMessagesForBuyerDashboard(int UserId, TimeSpan TimeZoneOffset)
        {
            List<MailboxMessageGridViewModel> response = new List<MailboxMessageGridViewModel>();
            try
            {              
                var applicationDomain = new ApplicationDomain(this);
                var buyerDashboardCount = applicationDomain.GetApplicationSettingValue<int>(ApplicationConstants.KeyBuyerDashboardRecordsCount, 5);
                var query = await Context.DataContext.AppMessageXUserStatuses.Include(t => t.AppMessage).Include(t => t.User)
                    .Where(t => t.UserId == UserId && t.AppMessageStatusId == (int)AppMessageStatus.Recieved)
                    .OrderByDescending(t => t.MessageId)
                    .Take(buyerDashboardCount).ToListAsync();

                foreach (var item in query)
                {
                    var sender = GetSender(item.AppMessage);
                    response.Add(new MailboxMessageGridViewModel
                    {
                        Id = item.MessageId,
                        SenderName = $"{sender.FirstName} {sender.LastName}",
                        Subject = item.AppMessage.Subject,
                        MessageStatusId = (AppMessageStatus)item.AppMessageStatusId,
                        MessageDeliveredTime = item.TimeStamp.ToBrowserDateTime(TimeZoneOffset).ToString(Resource.constFormatDateTime),
                    });
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AppMessageDomain", "GetMessagesForBuyerDashboard", ex.Message, ex);
            }
            return response;
        }
    }
}

