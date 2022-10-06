using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.Offer
{
    public class UspSupplierOfferGridViewModel
    {
        public int OfferPricingId { get; set; }
        public string Name { get; set; }
        public int OfferTypeId { get; set; }
        public string Tiers { get; set; }
        public string Customers { get; set; }
        public string FuelTypes { get; set; }
        public string States { get; set; }
        public string Cities { get; set; }
        public string ZipCodes { get; set; }
        public int FeeCount { get; set; }
        public string Pricing { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedDate { get; set; }
        public bool IsActive { get; set; }
        public int StatusId { get; set; }
        public int TotalCount { get; set; }
    }

    public class SupplierOfferGridViewModelItem
    {
        public int OfferPricingId { get; set; }
        public string Name { get; set; }
        public OfferType OfferType { get; set; }
        public string Tiers { get; set; }
        public string Customers { get; set; }
        public string FuelTypes { get; set; }
        public string Locations { get; set; }
        public string Fees { get; set; }
        public string Pricing { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedDate { get; set; }
        public int StatusId { get; set; }
        public int TotalCount { get; set; }
        public string TruckLoadType { get; set; }
    }


    public class UspBuyerOfferGridViewModel
    {
        public int OfferPricingId { get; set; }
        public int SupplierCompanyId { get; set; }
        public string Name { get; set; }
        public string OfferType { get; set; }
        public string Supplier { get; set; }
        public int? SupplierLogoId { get; set; }
        public string SupplierLogoURL { get; set; }
        public string FuelType { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public int FeeCount { get; set; }
        public string Pricing { get; set; }
        public decimal EstimatedPrice { get; set; }
        public decimal LoadedPrice { get; set; }
        public string LastUpdatedDate { get; set; }
        public int TotalCount { get; set; }
        public string EncrptedUrl { get; set; }
    }

    public class OfferItemDataTableViewModel : DataTableAjaxPostModel
    {
        public OfferItemDataTableViewModel()
        {
            OfferType = OfferType.All;
            Customers = new List<int>();
        }

        public Currency Currency { get; set; }

        public int CountryId { get; set; }

        public OfferType OfferType { get; set; }

        public List<int> Customers { get; set; }
    }

    public class OfferSummaryFilter
    {
        public OfferSummaryFilter()
        {
            CustomerIds = new List<int>();
        }
        public OfferType OfferEnumType { get; set; }

        public string OfferType { get; set; }

        public string Name { get; set; }
        public string Customers { get; set; }
        public string Tier { get; set; }
        public string FuelTypes { get; set; }
        public string Locations { get; set; }
        public string PricingFormat { get; set; }
        public string Fees { get; set; }
        public List<int> CustomerIds { get; set; }



    }
    public class OfferFilterViewModel : DataTableAjaxPostModel
    {
        public OfferFilterViewModel()
        {
            OfferType = OfferType.All;
            States = new List<int>();
            FuelTypes = new List<int>();
            CountryId = (int)Country.USA;
            CurrencyType = (int)Currency.USD;
            IsJobSearch = true;
            Customers = new List<int>();
        }
        public List<int> Customers { get; set; }


        public OfferType OfferType { get; set; }
        public int JobId { get; set; }
        public bool IsJobSearch { get; set; }
        public List<int> States { get; set; }
        public string Cities { get; set; }
        public string ZipCodes { get; set; }
        public List<int> Suppliers { get; set; }
        public List<int> FuelTypes { get; set; }
        //public List<int> PricingTypes { get; set; }
        public int Quantity { get; set; }
        public int CountryId { get; set; }
        public int CurrencyType { get; set; }
        public int PricingTypeId { get; set; }

        public TruckLoadTypes TruckLoadType { get; set; } = TruckLoadTypes.LessTruckLoad;
        public PricingSource PricingSource { get; set; } = PricingSource.Axxis;
        //public PricingSourceFeedTypes FeedType { get; set; }
        //public QuantityIndicatorTypes QuantityIndicator { get; set; }
        //public FuelClassTypes FuelClass { get; set; }
        public int? CityGroupTerminalId { get; set; }
        public int? TerminalId { get; set; }

        public int PricingCodeId { get; set; }
        public string PricingCode { get; set; }
        public string PricingCodeDesc { get; set; }
    }

    public class UspSupplierOfferGridRequestViewModel
    {
        public UspSupplierOfferGridRequestViewModel()
        {
            CountryId = (int)Country.All;
            CurrencyType = (int)Currency.USD;
        }

        public int CompanyId { get; set; }
        public int OfferType { get; set; }
        public int CountryId { get; set; }
        public int CurrencyType { get; set; }
        public string Customers { get; set; }
        public DataTableSearchModel dataTableSearchValues { get; set; }
    }

    public class UspBuyerOfferGridRequestViewModel
    {
        public UspBuyerOfferGridRequestViewModel()
        {
            CountryId = (int)Country.All;
            CurrencyType = (int)Currency.USD;
        }
        public int CompanyId { get; set; }
        public int JobId { get; set; }
        public int OfferType { get; set; }
        public string States { get; set; }
        public string Cities { get; set; }
        public string ZipCodes { get; set; }
        public string FuelTypes { get; set; }
        public int Quantity { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public int CountryId { get; set; }
        public int CurrencyType { get; set; }
        public int PricingTypeId { get; set; }
        public decimal LowPrice { get; set; }
        public decimal AvgPrice { get; set; }
        public decimal HighPrice { get; set; }
        public int RackTypeId { get; set; }
        public int PricingCodeId { get; set; }
        public DataTableSearchModel dataTableSearchValues { get; set; }
    }

    public class UspBuyerFtlOfferGridRequestViewModel
    {
        public UspBuyerFtlOfferGridRequestViewModel()
        {
            CountryId = (int)Country.All;
            CurrencyType = (int)Currency.USD;
        }
        public int CompanyId { get; set; }
        public int JobId { get; set; }
        public int OfferType { get; set; }
        public string States { get; set; }
        public string Cities { get; set; }
        public string ZipCodes { get; set; }
        public string FuelTypes { get; set; }
        public int Quantity { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public int CountryId { get; set; }
        public int CurrencyType { get; set; }
        public int PricingTypeId { get; set; }
        public int TruckLoadType { get; set; }
        public int PricingSource { get; set; }
        //public int FeedType { get; set; }
        //public int QuantityIndicator { get; set; }
        //public int FuelClass { get; set; }
        public int CityGroupTerminalId { get; set; }
        public int TerminalId { get; set; }
        public decimal LowPrice { get; set; }
        public decimal AvgPrice { get; set; }
        public decimal HighPrice { get; set; }
        public int RackTypeId { get; set; }
        public DataTableSearchModel dataTableSearchValues { get; set; }
    }

    public class PricingTableGridViewModel : UspSupplierOfferGridViewModel
    {

    }
}
