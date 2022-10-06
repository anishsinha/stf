using SiteFuel.Exchange.Core.StringResources;
using System;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class USP_FuelRequestStatsModel
    {
		public int TotalFuelRequestCount { get; set; }
		public int ThirdPartyFRCount { get; set; }
		public int OpenFuelRequestCount { get; set; }
		public int ExpiredFuelRequestCount { get; set; }
		public int BrokeredFuelRequestRequestCount { get; set; }
		public int AboutToExpireCount { get; set; }
	}
}

