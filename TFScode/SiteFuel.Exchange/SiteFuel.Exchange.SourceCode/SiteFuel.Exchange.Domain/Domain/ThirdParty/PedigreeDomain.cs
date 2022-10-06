using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
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
    public class PedigreeDomain : FreightServiceApiDomain
    {
        public PedigreeDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public PedigreeDomain(BaseDomain domain) : base(domain)
        {
        }

        public async Task<bool> ProcessPedigree()
        {
            bool response = false;
            try
            {
                var pedigreeIdList = await Context.DataContext.AssetAdditionalDetails.Where(w => w.PedigreeAssetDBId != null).Select(s => s.PedigreeAssetDBId).ToListAsync();
                if (pedigreeIdList != null && pedigreeIdList.Any())
                {
                    var pedigreeIds = String.Join(",", pedigreeIdList);
                    var spDomain = new StoredProcedureDomain(this);
                    var spResponse = await spDomain.GetPegedreeConfigurationInfo(pedigreeIds);
                    if (spResponse != null && spResponse.ProductTypeList.Any())
                    {
                        response = await PostPedigreeConfiguration(spResponse);
                    }

                    return response;


                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PedigreeDomain", "ProcessPedigree", ex.Message, ex);
            }

            return response;
        }

        private async Task<bool> PostPedigreeConfiguration(PedegreeConfigurationModel reqData)
        {
            var apiUrl = string.Format(ApplicationConstants.UrlProcessPedigree);
            var respData = await ApiPostCall<bool>(apiUrl, reqData);
            return respData;
        }

        public async Task<bool> ProcessSkybitz()
        {
            bool response = false;
            try
            {
                
                    var spDomain = new StoredProcedureDomain(this);
                    var spResponse = await spDomain.GetSkybitzConfiguration();
                    if (spResponse != null && spResponse.DropdownDisplayExtendedItems!=null && spResponse.DropdownDisplayExtendedItems.Any())
                    {
                        response = await PostSkybitzConfiguration(spResponse.DropdownDisplayExtendedItems, spResponse.IsApi);
                    }

                    return response;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PedigreeDomain", "ProcessSkybitz", ex.Message, ex);
            }
            return response;
        }
        private async Task<bool> PostSkybitzConfiguration(List<DropdownDisplayExtendedItem> reqData,int isAPI)
        {

            var apiUrl = string.Format(isAPI== 1 ? ApplicationConstants.UrlProcessSkybitzAPI: ApplicationConstants.UrlProcessSkybitz);
            var respData = await ApiPostCall<bool>(apiUrl, reqData);
            return respData;
        }

        public async Task<bool> ProcessIS360()
        {
            bool response = false;
            try
            {
                    var spDomain = new StoredProcedureDomain(this);
                    var spResponse = await spDomain.GetIS360Configuration();
                    if (spResponse != null && spResponse.ProductTypeList.Any())
                    {
                        response = await PostIs360Configuration(spResponse);
                    }
                    return response;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PedigreeDomain", "ProcessIS360", ex.Message, ex);
            }
            return response;
        }

        private async Task<bool> PostIs360Configuration(ExternalTankConfigurationModel reqData)
        {
            var apiUrl = string.Format(ApplicationConstants.UrlProcessIs360);
            var respData = await ApiPostCall<bool>(apiUrl, reqData);
            return respData;
        }

    }
}
