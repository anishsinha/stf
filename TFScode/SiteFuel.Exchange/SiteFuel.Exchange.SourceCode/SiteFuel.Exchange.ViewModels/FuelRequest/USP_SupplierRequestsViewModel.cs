using SiteFuel.Exchange.Core;
using System;
using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.ViewModels
{
    public class USP_SupplierRequestsViewModel
    {
        public USP_SupplierRequestsViewModel()
        {
            CountryId = (int)Country.All;
            CurrencyType = (int)Currency.None;
            Broadcast = (int)BroadcastType.All;
            AddressId = 0;
            isCallFromDashboard = false;
            StatusFilter = 0;
        }

        public int CompanyId { get; set; }

        public int UserId { get; set; }

        public int AddressId { get; set; }

        public int Broadcast { get; set; }

        public DateTimeOffset StartDate { get; set; }

        public DateTimeOffset EndDate { get; set; }

        public int StatusFilter { get; set; }

        public bool isCallFromDashboard { get; set; }

        public int CountryId { get; set; }

        public int CurrencyType { get; set; }

        public DataTableSearchModel dataTableSearchValues{ get; set; }
    }
}
