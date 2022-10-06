using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class CityRackSalesCalculatorViewModel
    {
        public CityRackSalesCalculatorViewModel()
        {
            SalesViewModel = new List<SalesCalculatorGridViewModel>();
            CityRackInputModel = new CityRackCalculatorInputViewModel();
        }
        public List<SalesCalculatorGridViewModel> SalesViewModel { get; set; }

        public bool IsCityRackTerminal { get; set; }

        public CityRackCalculatorInputViewModel CityRackInputModel { get; set; }
    }

    public class CityRackCalculatorInputViewModel
    {
        public DateTime PriceDate { get; set; }

        public int ExternalProductId { get; set; }

        public string StateOrTerminalIds { get; set; }

        public int CityTerminalPricingType { get; set; }
    }
}
