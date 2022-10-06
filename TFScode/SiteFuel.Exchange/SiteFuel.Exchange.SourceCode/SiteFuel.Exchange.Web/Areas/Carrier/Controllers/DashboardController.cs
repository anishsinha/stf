using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.Web.Attributes;
using SiteFuel.Exchange.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Areas.Carrier.Controllers
{
    [AuthorizeCompany(CompanyType.Carrier, CompanyType.Supplier, CompanyType.SupplierAndCarrier, CompanyType.BuyerSupplierAndCarrier, CompanyType.BuyerAndSupplier)]
    public class DashboardController : BaseController
    {

        [AuthorizeRole(UserRoles.Admin, UserRoles.CarrierAdmin, UserRoles.Carrier, UserRoles.Dispatcher, UserRoles.Driver, UserRoles.ReportingPerson, UserRoles.AccountingPerson)]
        public ActionResult Index()
        {
            return View();
        }
    }
}