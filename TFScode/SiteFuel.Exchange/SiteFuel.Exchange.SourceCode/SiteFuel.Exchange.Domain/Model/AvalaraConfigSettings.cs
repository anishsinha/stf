using System;
using System.Collections.Generic;
using System.Device.Location;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System.Data.Entity;
using System.Runtime.Caching;
using RestSharp;
using System.Net;
using RestSharp.Authenticators;
using SiteFuel.Exchange.Domain.AvaTaxExciseWebService;

namespace SiteFuel.Exchange.Domain
{
    public static class AvalaraConfigSettings
    {
        public static string UserId { get; set; }
        public static string Password { get; set; }
        public static string CompanyName { get; set; }
        public static string LoginUrl { get; set; }
        public static string TaxUrl { get; set; }
        public static string TaxExemptionUrl { get; set; }
        public static bool TaxExemptionEnabled { get; set; }
        public static bool CanUseTaxService { get; set; }

        static AvalaraConfigSettings()
        {
            try
            {
                var helperDomain = ContextFactory.Current.GetDomain<HelperDomain>();
                helperDomain.SetAvaTaxConfigSettings();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AvalaraConfigSettings", "AvalaraConfigSettings", ex.Message, ex);
            }
        }
    }

    public class AvalaraResponseViewModel
    {
        public Transaction_5_27_0 Request { get; set; }
        public TransactionResultSummary_5_27_0 Result { get; set; }
    }

}
