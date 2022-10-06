using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.Web.Attributes;
using SiteFuel.Exchange.Web.Controllers;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Areas.Supplier.Controllers
{
    public class ProductMappingController : BaseController
    {
        [HttpGet]
        [ActionName("View")]
        [AuthorizeRole(UserRoles.Supplier, UserRoles.SupplierAdmin)]
        public ActionResult ProductMapping()
        {
            return View();
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.Supplier, UserRoles.SupplierAdmin)]
        public async Task<ActionResult> ProductMappingGrid()
        {
            using (var tracer = new Tracer("ProductMappingController", "ProductMappingGrid"))
            {
                var response = await ContextFactory.Current.GetDomain<ProductMappingDomain>().GetSupplierProductMappingGridAsync(CurrentUser.CompanyId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.Supplier, UserRoles.SupplierAdmin)]
        public ActionResult Create()
        {
            return PartialView("Create", new SupplierProductsViewModel() { DisplayMode = PageDisplayMode.Create });
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.Supplier, UserRoles.SupplierAdmin)]
        public async Task<ActionResult> Details(int id = 0)
        {
            var response = await ContextFactory.Current.GetDomain<ProductMappingDomain>().GetSupplierProductMappingAsync(id);
            return PartialView("Create", response);
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.Supplier, UserRoles.SupplierAdmin)]
        public async Task<ActionResult> Create(SupplierProductsViewModel viewModel)
        {
            using (var tracer = new Tracer("ProductMappingController", "CreateProductMapping"))
            {
                bool validateResponse = ValidateModel(viewModel);
                if (ModelState.IsValid && validateResponse)
                {
                    StatusViewModel response;
                    viewModel.DisplayMode = PageDisplayMode.Create;
                    response = await ContextFactory.Current.GetDomain<ProductMappingDomain>().SaveSupplierProductMapping(viewModel, UserContext);
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                    if (response.StatusCode == Status.Success)
                    {
                        return RedirectToAction("View", "ProductMapping", new { area = "Supplier" });
                    }
                    else
                    {
                        return View("Create", viewModel);
                    }
                }

                return View("Create", viewModel);
            }
        }

        private bool ValidateModel(SupplierProductsViewModel viewModel)
        {
            bool response = true;
            if (viewModel.SupplierProducts != null && viewModel.SupplierProducts.Count > 0)
            {
                if (viewModel.DisplayMode == PageDisplayMode.Create)
                {
                    var checkDuplicateAssignName = viewModel.SupplierProducts.GroupBy(x => x.AssignedName).Any(grp => grp.Count() > 1);
                    if (checkDuplicateAssignName)
                    {
                        response = false;
                        viewModel.StatusCode = Status.Failed;
                        viewModel.StatusMessage = Resource.errMsgDuplicateAssignName;
                        DisplayCustomMessages((MessageType)viewModel.StatusCode, viewModel.StatusMessage);
                        return response;
                    }

                    var checkDuplicateTerimalName = viewModel.SupplierProducts.GroupBy(x => x.TerminalId).Any(grp => grp.Count() > 1);
                    if (checkDuplicateTerimalName)
                    {
                        response = false;
                        viewModel.StatusCode = Status.Failed;
                        viewModel.StatusMessage = Resource.errMsgDuplicateTerimalName;
                        DisplayCustomMessages((MessageType)viewModel.StatusCode, viewModel.StatusMessage);
                        return response;
                    }
                }

                var helperDomain = new HelperDomain();
                foreach (var productDetails in viewModel.SupplierProducts)
                {
                   // if (viewModel.DisplayMode == PageDisplayMode.Create)
                   // {
                        var checkAlreadyExistingAssignedName = helperDomain.IsAssignedProductNameAlreadyExist(productDetails.Id, productDetails.AssignCompanyId, productDetails.AssignedName);
                        if (!checkAlreadyExistingAssignedName)
                        {
                            viewModel.StatusCode = Status.Failed;
                            viewModel.StatusMessage = string.Format(Resource.errMsgAssignedNameExists, productDetails.AssignedName);
                            DisplayCustomMessages((MessageType)viewModel.StatusCode, viewModel.StatusMessage);
                            response = false;
                            return response;
                        }

                        //var checkAlreadyExistingATerminalName = helperDomain.IsTerminalNameAlreadyExist(productDetails.TerminalId, productDetails.AssignCompanyId, out string terminalName);
                        //if (!checkAlreadyExistingATerminalName)
                        //{
                        //    response = false;
                        //    viewModel.StatusCode = Status.Failed;
                        //    viewModel.StatusMessage = string.Format(Resource.errMsgTerminalNameMapped, terminalName);
                        //    DisplayCustomMessages((MessageType)viewModel.StatusCode, viewModel.StatusMessage);
                        //    return response;
                        //}
                   // }

                    if(productDetails.ProductMappingFuelTypeDetailsViewModels?.Count > 0)
                    {
                        var checkAlreadyExistingTerminalName = helperDomain.IsTerminalNameAlreadyMapped(productDetails.TerminalId, productDetails.AssignCompanyId, productDetails.ProductMappingFuelTypeDetailsViewModels, out string terminalName);
                        if (checkAlreadyExistingTerminalName)
                        {
                            response = false;
                            viewModel.StatusCode = Status.Failed;
                            viewModel.StatusMessage = string.Format(Resource.errMsgTerminalNameMapped, terminalName);
                            DisplayCustomMessages((MessageType)viewModel.StatusCode, viewModel.StatusMessage);
                            return response;
                        }

                        var checkDuplicateFuelDetails = productDetails.ProductMappingFuelTypeDetailsViewModels.Where(x => !string.IsNullOrWhiteSpace(x.BackOfficeProductCode) && !string.IsNullOrWhiteSpace(x.SeaboardProductCode))
                                                   .GroupBy(x => new { x.BackOfficeProductCode, x.SeaboardProductCode })
                                                   .Any(grp => grp.Count() > 1);
                        if (checkDuplicateFuelDetails)
                        {
                            viewModel.StatusCode = Status.Failed;
                            response = false;
                            viewModel.StatusMessage = Resource.errMsgFuelDetailsMapped;
                            DisplayCustomMessages((MessageType)viewModel.StatusCode, viewModel.StatusMessage);
                            return response;
                        }
                    } 
                }
            }

            return response;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(UserRoles.Supplier, UserRoles.SupplierAdmin)]
        public async Task<ActionResult> Edit(SupplierProductsViewModel viewModel)
        {
            using (var tracer = new Tracer("ProductMappingController", "EditProductMapping"))
            {
                bool validateResponse = ValidateModel(viewModel);
                if (ModelState.IsValid && validateResponse)
                {
                    var response = await ContextFactory.Current.GetDomain<ProductMappingDomain>().UpdateSupplierProductMapping(viewModel, UserContext);

                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                    if (response.StatusCode == Status.Failed)
                    {
                        viewModel.DisplayMode = PageDisplayMode.Edit;
                        return View("Create", viewModel);
                    }

                    return RedirectToAction("View", "ProductMapping", new { area = "Supplier" });
                }
                else
                {
                    return View("Create", viewModel);
                }
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.Supplier, UserRoles.SupplierAdmin)]
        public ActionResult AnotherSupplierProduct()
        {
            using (var tracer = new Tracer("ProductMappingController", "AnotherSupplierProduct"))
            {
                return PartialView("_PartialSupplierProduct", new SupplierProductViewModel() { AssignCompanyId = UserContext.CompanyId });
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.Supplier, UserRoles.SupplierAdmin)]
        public ActionResult ProductMappingFuelTypeDetails(string prefix)
        {
            using (var tracer = new Tracer("ProductMappingController", "ProductMappingFuelTypeDetails"))
            {
                return PartialView("_PartialProductMappingFuelDetails", new ProductMappingFuelTypeDetailsViewModel() { CompanyId = UserContext.CompanyId, CollectionHtmlPrefix = prefix });
            }
        }
    }
}