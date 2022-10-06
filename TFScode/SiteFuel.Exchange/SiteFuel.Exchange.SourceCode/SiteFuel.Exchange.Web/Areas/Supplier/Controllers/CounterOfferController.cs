using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.Web.Attributes;
using SiteFuel.Exchange.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;
using SiteFuel.Exchange.ViewModels;

namespace SiteFuel.Exchange.Web.Areas.Supplier.Controllers
{
    [AuthorizeCompany(CompanyType.Supplier)]
    public class CounterOfferController : BaseController
    {
        [HttpGet]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
        public async Task<ActionResult> Decline(int fuelRequestId, int supplierId = 0)
        {
            using (var tracer = new Tracer("CounterOfferController", "Decline"))
            {
                StatusViewModel response;
                if (supplierId > 0)
                {
                    response = await ContextFactory.Current.GetDomain<CounterOfferDomain>().DeclineCounterOfferByBuyerAsync(fuelRequestId, UserContext, supplierId);
                }
                else
                {
                    response = await ContextFactory.Current.GetDomain<CounterOfferDomain>().DeclineCounterOfferBySupplierAsync(fuelRequestId, UserContext);
                }
                if (response.StatusCode == Status.Failed)
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, Resource.errMessageDeclineCounterOfferFailed);
                }
                return RedirectToAction("Details", "CounterOffer", new { area = "Supplier", fuelRequestId = fuelRequestId, supplierId = supplierId });
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
        public async Task<ActionResult> Cancel(int fuelRequestId, int supplierId = 0)
        {
            using (var tracer = new Tracer("CounterOfferController", "Cancel"))
            {
                bool isBrokerRequest = false;
                StatusViewModel response;
                if (supplierId > 0)
                {
                    isBrokerRequest = true;
                    response = await ContextFactory.Current.GetDomain<CounterOfferDomain>().CancelCounterOfferByBuyerAsync(fuelRequestId, supplierId);
                }
                else
                {
                    response = await ContextFactory.Current.GetDomain<CounterOfferDomain>().CancelCounterOfferBySupplierAsync(fuelRequestId, CurrentUser.Id);
                }
                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                if (!isBrokerRequest)
                {
                    return RedirectToAction("View", "FuelRequest", new { area = "Supplier" });
                }
                else
                {
                    return RedirectToAction("BrokerSupplier", "Broker", new { area = "Supplier" });
                }
            }
        }

        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
        public async Task<ActionResult> Accept(int fuelRequestId, int supplierId = 0)
        {
            using (var tracer = new Tracer("CounterOfferController", "Accept"))
            {
                FuelRequestStatusViewModel response;
                bool isBrokerRequest = false;
                if (supplierId > 0)
                {
                    isBrokerRequest = true;
                    response = await ContextFactory.Current.GetDomain<CounterOfferDomain>().AcceptCounterOfferByBuyerAsync(UserContext, supplierId, fuelRequestId);
                }
                else
                {
                    response = await ContextFactory.Current.GetDomain<CounterOfferDomain>().AcceptCounterOfferBySupplierAsync(UserContext, fuelRequestId);
                }
                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                if (response.StatusCode == Status.Success)
                {
                    if (response.IsFirstTimeBuyer)
                    {
                        SendCreditAppNotification(response);
                    }
                    if (!isBrokerRequest)
                    {
                        return RedirectToAction("View", "Order", new { area = "Supplier" });
                    }
                    else
                    {
                        TempData["IsOrderSummary"] = true;
                        return RedirectToAction("BrokerSupplier", "Broker", new { area = "Supplier" });
                    }
                }
                else
                {
                    return RedirectToAction("Details", "CounterOffer", new { area = "Supplier", fuelRequestId = fuelRequestId, supplierId = supplierId });
                }
            }
        }

        [HttpGet]
        public async Task<ActionResult> Details(int fuelRequestId, int supplierId = 0)
        {
            using (var tracer = new Tracer("CounterOfferController", "Details"))
            {
                StatusViewModel response;

                if (supplierId > 0)
                {
                    response = await ContextFactory.Current.GetDomain<CounterOfferDomain>().GetBuyerCounterOfferDetailsAsync(fuelRequestId, CurrentUser.Id, supplierId);
                }
                else
                {
                    response = await ContextFactory.Current.GetDomain<CounterOfferDomain>().GetSupplierCounterOfferDetailsAsync(fuelRequestId, CurrentUser.Id);
                }
                if (response.StatusCode == Status.Failed)
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, Resource.errMessageLoadCounterOfferDetailsFailed);
                }
                return View(response);
            }
        }
    }
}