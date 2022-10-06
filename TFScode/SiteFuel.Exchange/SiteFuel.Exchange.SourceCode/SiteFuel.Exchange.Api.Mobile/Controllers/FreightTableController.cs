using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Description;
using System.Web.Mvc;

namespace SiteFuel.Exchange.Api.Mobile.Controllers
{
#if DEBUG
    [ApiExplorerSettings(IgnoreApi = false)]
#else
    [ApiExplorerSettings(IgnoreApi = true)]
#endif

    public class FreightTableController : ApiBaseController
    {
        public async Task<List<DropdownDisplayItem>> GetCustomers(int companyId)
        {
            try
            {
                var masterDomain = ContextFactory.Current.GetDomain<MasterDomain>();
                return masterDomain.GetCustomersForCompany(companyId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterController", "GetCustomers", ex.Message, ex);
            }

            return new List<DropdownDisplayItem>();
        }

        public async Task<object> GetBulkPlants(int companyId, int stateId)
        {
            try
            {
                var dispatchDomain = ContextFactory.Current.GetDomain<DispatchDomain>();
                return dispatchDomain.GetBulkPlantsForFreight(string.Empty, companyId, stateId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterController", "GetBulkPlants", ex.Message, ex);
            }

            return null;
        }

        public async Task<List<DropdownDisplayExtended>> GetRegions(int companyId)
        {
            try
            {
                var freightDomain = ContextFactory.Current.GetDomain<FreightServiceDomain>();
                return await freightDomain.GetRegions(companyId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterController", "GetRegions", ex.Message, ex);
            }

            return new List<DropdownDisplayExtended>();
        }

        public async Task<List<DropdownDisplayItem>> GetCountries()
        {
            try
            {
                var masterDomain = ContextFactory.Current.GetDomain<MasterDomain>();
                return masterDomain.GetCountries();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterController", "GetCountries", ex.Message, ex);
            }

            return new List<DropdownDisplayItem>();
        }

        public async Task<List<DropdownDisplayItem>> GetStates(int countryId)
        {
            try
            {
                var masterDomain = ContextFactory.Current.GetDomain<MasterDomain>();
                return masterDomain.GetStates(countryId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterController", "GetStates", ex.Message, ex);
            }

            return new List<DropdownDisplayItem>();
        }

        public async Task<List<DropdownDisplayItem>> GetProductTypes()
        {
            try
            {
                var masterDomain = ContextFactory.Current.GetDomain<MasterDomain>();
                return masterDomain.GetProductTypes();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterController", "GetProductTypes", ex.Message, ex);
            }

            return new List<DropdownDisplayItem>();
        }

        public async Task<List<DropdownDisplayItem>> GetFuelTypes(int productTypeId = 1)
        {
            try
            {
                var masterDomain = ContextFactory.Current.GetDomain<MasterDomain>();
                return masterDomain.GetFuelTypes(productTypeId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterController", "GetFuelTypes", ex.Message, ex);
            }

            return new List<DropdownDisplayItem>();
        }

        public async Task<object> GetTerminals(int countryId, int stateId)
        {
            try
            {
                var masterDomain = ContextFactory.Current.GetDomain<MasterDomain>();
                return masterDomain.GetTerminals(countryId, stateId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterController", "GetTerminals", ex.Message, ex);
            }

            return null;
        }

        public async Task<JsonResult> GetMasterData()
        {
            try
            {
                var masterDomain = ContextFactory.Current.GetDomain<MasterDomain>();
                var result = await masterDomain.GetMasterDataForFreightTable();
                return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterController", "GetMasterData", ex.Message, ex);
            }

            return new JsonResult { Data = Resource.lblNoDataAvailable, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}
