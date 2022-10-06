using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
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

namespace SiteFuel.Exchange.Web.Areas.Carrier.Controllers
{
    public class TractorController : BaseController
    {
        [AuthorizeCompany(CompanyType.Carrier, CompanyType.Supplier)]
        [ActionName("View")]
        // GET: Carrier/Tractor
        public ActionResult Index()
        {
            var response = Task.Run(() => ContextFactory.Current.GetDomain<MasterDomain>().GetCompanyDefaultCurrency(UserContext.CompanyId)).Result;
            return View("View", response);
        }
        [HttpPost]
        public ActionResult Create(TractorDetailViewModel inputData)
        {
            if (inputData.Drivers != null && inputData.Drivers.Any())// added to remove onboarding status text from name 
            {
                foreach (var driver in inputData.Drivers)
                {
                    if ((driver.Name.Contains(Resource.lblDriverEmailVerfied)))
                    {
                        var driverName = driver.Name.Replace(Resource.lblDriverEmailVerfied, "");
                        driver.Name = driverName;
                    }
                    else if ((driver.Name.Contains(Resource.lblDriverInvited)))
                    {
                        var driverName = driver.Name.Replace(Resource.lblDriverInvited, "");
                        driver.Name = driverName;
                    }
                }
            }
            var response = Task.Run(() => ContextFactory.Current.GetDomain<AssetDomain>().SaveTractorDetailsAsync(UserContext, inputData)).Result;
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult DeleteTractor(TractorDetailViewModel inputData)
        {
            inputData.TfxCompanyId = CurrentUser.CompanyId;
            inputData.TfxCreatedBy = CurrentUser.Id;
            inputData.CreatedDate = DateTimeOffset.Now;
            var response = Task.Run(() => ContextFactory.Current.GetDomain<FreightServiceDomain>().DeleteTractorAsync(inputData)).Result;
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetAllTractorDetails()
        {
            var result = Task.Run(() => ContextFactory.Current.GetDomain<FreightServiceDomain>().GetAllTractorDetails(UserContext.CompanyId)).Result;
            ContextFactory.Current.GetDomain<HelperDomain>().setDriverUserOnboardingStatus(result);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetCompanyDrivers(string trailerId)
        {
            //UserContext.CompanyId
            var response = new List<DropdownDisplayItem>();
            var getAllDrivers = Task.Run(() => ContextFactory.Current.GetDomain<HelperDomain>().GetAllDriversTractor(UserContext.CompanyId, false)).Result;
            var getMongodbDrivers = Task.Run(() => ContextFactory.Current.GetDomain<FreightServiceDomain>().GetAllDrivers(UserContext.CompanyId, trailerId)).Result;
            foreach (var item in getAllDrivers)
            {
                var recordsExists = getMongodbDrivers.Find(top => top.Id == item.Id);
                if (recordsExists != null)
                {
                    var splitstr = item.Name.Split('-');
                    if (splitstr.Length > 1)
                    {
                        var driverstatus = splitstr[splitstr.Length - 1];
                        response.Add(new DropdownDisplayItem { Id = recordsExists.Id, Name = recordsExists.Name + "-" + driverstatus });
                    }
                    else
                    {
                        response.Add(new DropdownDisplayItem { Id = recordsExists.Id, Name = recordsExists.Name });
                    }                   
                }
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetDefaultUOM()
        {
            var response = Task.Run(() => ContextFactory.Current.GetDomain<MasterDomain>().GetCompanyDefaultCurrency(UserContext.CompanyId)).Result;
            return Json(response.GetDisplayName(), JsonRequestBehavior.AllowGet);
        }
    }
}