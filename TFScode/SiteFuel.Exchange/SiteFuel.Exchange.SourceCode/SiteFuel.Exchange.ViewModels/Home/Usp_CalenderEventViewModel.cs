using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class Usp_CalenderEventViewModel
    {
        public Usp_CalenderEventViewModel()
        {
            CountryId = (int)Country.All;
            CurrencyType = (int)Currency.None;
        }
        
        public int SupplierCompanyId { get; set; }

        public int DriverId { get; set; }

        public int BuyerCompanyId { get; set; }

        public DateTime FirstDayOfMonth { get; set; }

        public DateTime LastDayOfMonth { get; set; }

        public string SelectedOrders { get; set; }

        public int CountryId { get; set; }

        public int CurrencyType { get; set; }
    }
    
}
