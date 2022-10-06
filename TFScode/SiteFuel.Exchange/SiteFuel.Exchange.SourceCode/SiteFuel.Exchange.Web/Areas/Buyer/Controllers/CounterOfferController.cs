using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.Web.Attributes;
using SiteFuel.Exchange.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;
using SiteFuel.Exchange.Core.Logger;

namespace SiteFuel.Exchange.Web.Areas.Buyer.Controllers
{
    [AuthorizeCompany(CompanyType.Buyer)]
    public class CounterOfferController : BaseController
    {
        [HttpGet]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        public async Task<ActionResult> Decline(int fuelRequestId, int supplierId)
        {
            using (var tracer = new Tracer("CounterOfferController", "Decline"))
            {
                var response = await ContextFactory.Current.GetDomain<CounterOfferDomain>().DeclineCounterOfferByBuyerAsync(fuelRequestId, UserContext, supplierId);
                if (response.StatusCode == Status.Failed)
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, Resource.errMessageDeclineCounterOfferSuccess);
                }
                else if (response.StatusCode == Status.Success)
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                }
                //this will redirect to Job details page when cancel fuel request is requested from Job Details
                if (IsReturnUrlExist())
                {
                    return Redirect(GetReturnUrl());
                }
                return RedirectToAction("Details", "CounterOffer", new { area = "Buyer", fuelRequestId = fuelRequestId, supplierId = supplierId });
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        public async Task<ActionResult> Cancel(int fuelRequestId, int supplierId)
        {
            using (var tracer = new Tracer("CounterOfferController", "Cancel"))
            {
                var response = await ContextFactory.Current.GetDomain<CounterOfferDomain>().CancelCounterOfferByBuyerAsync(fuelRequestId, supplierId);
                if (response.StatusCode == Status.Failed)
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, Resource.errMessageCancelCounterOfferFailed);
                }
                else if (response.StatusCode == Status.Success)
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                }

                //this will redirect to Job details page when cancel fuel request is requested from Job Details
                if (IsReturnUrlExist())
                {
                    return Redirect(GetReturnUrl());
                }
                return RedirectToAction("View", "FuelRequest", new { area = "Buyer" });
            }
        }

        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        public async Task<ActionResult> Accept(int fuelRequestId, int supplierId)
        {
            using (var tracer = new Tracer("CounterOfferController", "Accept"))
            {
                var response = await ContextFactory.Current.GetDomain<CounterOfferDomain>().AcceptCounterOfferByBuyerAsync(UserContext, supplierId, fuelRequestId);
                if (response.StatusCode == Status.Failed)
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, Resource.errMessageCounterOfferAcceptFailed);
                }
                else if(response.StatusCode == Status.Success)
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                    if(response.IsFirstTimeBuyer)
                        SendCreditAppNotification(response);
                    return RedirectToAction("View", "Order", new { area = "Buyer" });
                }
                //this will redirect to Job details page when cancel fuel request is requested from Job Details
                if (IsReturnUrlExist())
                {
                    return Redirect(GetReturnUrl());
                }
                return RedirectToAction("View", "FuelRequest", new { area = "Buyer" });
            }
        }

        [HttpGet]
        public async Task<ActionResult> Details(int fuelRequestId, int supplierId)
        {
            using (var tracer = new Tracer("CounterOfferController", "Details"))
            {
                var response = await ContextFactory.Current.GetDomain<CounterOfferDomain>().GetBuyerCounterOfferDetailsAsync(fuelRequestId, CurrentUser.Id, supplierId);
                if (response.StatusCode == Status.Failed)
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, Resource.errMessageLoadCounterOfferDetailsFailed);
                }
                return View(response);
            }
        }
    }
}