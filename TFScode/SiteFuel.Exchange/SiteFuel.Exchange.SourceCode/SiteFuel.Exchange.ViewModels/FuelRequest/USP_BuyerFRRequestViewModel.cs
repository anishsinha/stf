using SiteFuel.Exchange.Core;
using System;
using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.ViewModels
{
    public class USP_BuyerFRRequestViewModel
    {
        public USP_BuyerFRRequestViewModel()
        {
            CountryId = (int)Country.All;
            CurrencyType = (int)Currency.USD;
            Broadcast = (int)BroadcastType.All;
        }

        public int CompanyId { get; set; }

        public int UserId { get; set; }

        public int Broadcast { get; set; }

        public int StatusId { get; set; }

        public int JobId { get; set; }

        public DateTimeOffset StartDate { get; set; }

        public DateTimeOffset EndDate { get; set; }

        public int CountryId { get; set; }

        public int CurrencyType { get; set; }

        public string GroupIds { get; set; }

        public DataTableSearchModel dataTableSearchValues{ get; set; }
    }
}
