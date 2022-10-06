using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain
{
	public class ZipCodeServiceDomain : BaseDomain
	{
		
		public ZipCodeServiceDomain() : base(ContextFactory.Current.ConnectionString)
		{
		}

		public ZipCodeServiceDomain(BaseDomain domain) : base(domain)
		{
		}

        public async Task<List<DropdownDisplayExtended>> GetZipCodeList(string state, string city)
        {
            List<DropdownDisplayExtended> zipCodes = new List<DropdownDisplayExtended>();
            try
            {
                var mstcity = await Context.DataContext.MstCities.Where(x => x.MstState.Name == state && x.Name == city).Select(x => x.ZipCodes).FirstOrDefaultAsync();
                if (mstcity != null)
                {
                    zipCodes = mstcity.Split(',').OrderBy(x => x).Select(x => new DropdownDisplayExtended { Code = x, Name = x, Id = x }).ToList();
                    //if (zipCodes.Count > 0)
                    //    zipCodes.Insert(0, new DropdownDisplayExtended() { Id = "0", Code = "0", Name = Resource.lblSelectAll });
                }
                return zipCodes;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ZipCodeServiceDomain", "GetZipCodeList", ex.Message, ex);
            }
            return zipCodes;
        }


        public async Task<string> GetZipCodes(string state, string city)
        {
            var zipCodes = string.Empty;
            try
            {
                var mstcity = await Context.DataContext.MstCities.Where(x => x.MstState.Name == state && x.Name == city).FirstOrDefaultAsync();
                if (mstcity != null)
                {
                    zipCodes = mstcity.ZipCodes;
                }
                return zipCodes;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ZipCodeServiceDomain", "GetZipCodeList", ex.Message, ex);
            }
            return zipCodes;
        }
    }

	public class ZipCodesFromService
	{
		public ZipCodesFromService()
		{
			zip_codes = new List<string>();
		}

		public List<string> zip_codes { get; set; }
	}
}
