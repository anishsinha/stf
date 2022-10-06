using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.Web.Attributes;
using SiteFuel.Exchange.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Areas.Messages.Controllers
{
    [AuthorizeRole]
    public class MailboxController : BaseController
    {
        public async Task<ActionResult> Compose(AppMessageComposeType composeType = AppMessageComposeType.Compose, int messageId = 0, AppMessageQueryType queryType = AppMessageQueryType.Order, int number = 0, string recipients = null)
        {
            var viewModel = new ComposeMessageViewModel
            {
                Id = messageId,
                ComposeType = composeType
            };

            if (number > 0)
            {
                viewModel.Type = queryType;
                viewModel.Number = number;
            }
            if (recipients != null && recipients.Length > 0)
            {
                viewModel.Recipients = recipients.Split(',').Select(int.Parse).ToList();
            }
            if (composeType != AppMessageComposeType.Compose)
            {
                viewModel = await ContextFactory.Current.GetDomain<AppMessageDomain>().GetComposeMessageAsync(CurrentUser.Id, viewModel);
            }
            if (queryType == AppMessageQueryType.Dispatch)
            {
                viewModel.Type = queryType;
            }
            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Compose(ComposeMessageViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                viewModel.TimeStamp = DateTimeOffset.Now;
                var response = await ContextFactory.Current.GetDomain<AppMessageDomain>().SaveAppMessageAsync(CurrentUser.Id, viewModel);
                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                if (response.StatusCode == Status.Success)
                {
                    if (viewModel.ComposeType == AppMessageComposeType.Compose)
                    {
                        await ContextFactory.Current.GetDomain<NewsfeedDomain>().SetMessageNewsfeed(UserContext, viewModel);
                    }
                    return RedirectToAction("Messages", "Mailbox", new { area = "Messages" });
                }
            }
            return View(viewModel);
        }

        public async Task<ActionResult> Messages(AppMessageFilterType type = AppMessageFilterType.Inbox)
        {
            var viewModel = new MailboxMessageFilterViewModel
            {
                UserId = CurrentUser.Id,
                Type = type,
                TimeZoneOffset = GetBrowserTimeZoneOffset()
            };
            viewModel = await ContextFactory.Current.GetDomain<AppMessageDomain>().GetAppMessagesAsync(viewModel);
            return View(viewModel);
        }

        [HttpGet]
        public async Task<ActionResult> GetMessagesForBuyerDashboard()
        {
            var TimeZoneOffset = GetBrowserTimeZoneOffset();
            var  response = await ContextFactory.Current.GetDomain<AppMessageDomain>().GetMessagesForBuyerDashboard(CurrentUser.Id, TimeZoneOffset);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<ActionResult> Messages(MailboxMessageFilterViewModel viewModel)
        {
            viewModel = await ContextFactory.Current.GetDomain<AppMessageDomain>().GetAppMessagesAsync(viewModel);
            ModelState.Clear();
            return View(viewModel);
        }

        public async Task<JsonResult> MarkMessages(AppMessageMarkingType type, List<int> selectedMessages)
        {
            var response = false;
            if (selectedMessages != null && selectedMessages.Count > 0)
            {
                response = await ContextFactory.Current.GetDomain<AppMessageDomain>().MarkMessagesAsync(CurrentUser.Id, type, selectedMessages);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> ViewEmail(int messageId)
        {
            TimeSpan TimeZoneOffset = GetBrowserTimeZoneOffset();
            var viewModel = await ContextFactory.Current.GetDomain<AppMessageDomain>().GetAppMessageAsync(CurrentUser.Id, messageId, TimeZoneOffset);
            return View(viewModel);
        }

        public async Task<JsonResult> GetNumbers(AppMessageQueryType type)
        {
            var response = new List<DropdownDisplayItem>();
            if (type == AppMessageQueryType.Order)
            {
                response = await ContextFactory.Current.GetDomain<OrderDomain>().GetOrderNumbersAsync(CurrentUser.CompanyId);
            }
            else if (type == AppMessageQueryType.Invoice)
            {
                response = await ContextFactory.Current.GetDomain<InvoiceDomain>().GetInvoiceNumbersAsync(CurrentUser.CompanyId);
            }
            else
            {
                response = await ContextFactory.Current.GetDomain<InvoiceDomain>().GetInvoiceNumbersAsync(CurrentUser.CompanyId, (int)InvoiceType.DigitalDropTicketManual);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetRecipients(AppMessageQueryType type, int number)
        {
            var viewModel = new RecipientFilterViewModel
            {
                SendersId = CurrentUser.Id,
                SendersCompanyId = CurrentUser.CompanyId,
                Type = type,
                Number = number
            };
            var response = await ContextFactory.Current.GetDomain<AppMessageDomain>().GetRecipientsAsync(viewModel);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetAppMessageCount()
        {
            var response = await ContextFactory.Current.GetDomain<AppMessageDomain>().GetAppMessageCountAsync(CurrentUser.Id);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetMailboxMessagesCount()
        {
            var response = await ContextFactory.Current.GetDomain<AppMessageDomain>().GetMailboxMessagesCountAsync(CurrentUser.Id);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetUnreadMessages()
        {
            TimeSpan TimeZoneOffset = GetBrowserTimeZoneOffset();
            var response = await ContextFactory.Current.GetDomain<AppMessageDomain>().GetUnreadAppMessagesAsync(CurrentUser.Id, TimeZoneOffset);
            MessagesCountViewModel objResponse = new MessagesCountViewModel();
            objResponse.TotalUnreadMessagesCount = response.Count;
            objResponse.LastFiveUnreadMessageModel = response.Take(5).ToList();
            return Json(objResponse, JsonRequestBehavior.AllowGet);
        }
    }
}