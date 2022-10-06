using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels.FuelSurcharge
{
    public class FuelSurchargeTableModel
    {
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public decimal? PriceRangeStartValue { get; set; }
        public decimal? PriceRangeEndValue { get; set; }
        public decimal? PriceRangeInterval { get; set; }
        public decimal? FuelSurchargeStartPercentage { get; set; }
        public decimal? SurchargeInterval { get; set; }
        public string Id { get; set; }
        public int SupplierId { get; set; }
    }
    public class FuelSurchargeIndexViewModel
    {
        public int? FuelSurchargeIndexId { get; set; }
        public List<DropdownDisplayItem> Customers { get; set; }
        public List<DropdownDisplayItem> Carriers { get; set; }
        public List<DropdownDisplayItem> SourceRegions { get; set; }
        public List<DropdownDisplayExtended> TerminalsAndBulkPlants { get; set; }
       
        public List<DropdownDisplayItem> FuelSurchargePeriods { get; set; }
      
        public decimal APILatestIndexPrice { get; set; }
        public DateTimeOffset? ApiAdjustIndexPriceDate { get; set; } //json
        public string ApiEffectiveDate { get; set; } //json
        public DateTimeOffset? ManualEffectiveDate { get; set; }
        public decimal ManualLatestIndexPrice { get; set; }
        public bool IsManualUpdate { get; set; }
        public FuelSurchargeTableModel FuelSurchargeTable { get; set; } //input model
        public string TableName { get; set; }
        public string Notes { get; set; }
        public DateTimeOffset IndexPriceDate { get; set; }
        public int StatusId { get; set; }
        public List<FuelSurchargeTableModel> GeneratedSurchargeTable { get; set; } // FS table genearted

       
        public int TableTypeId { get; set; }
        public int? ProductId { get; set; }
        public int? PeriodId { get; set; }
        public int? AreaId { get; set; }
    }

    public class FuelSurchargeIndexEIAViewModel
    {
        public FuelSurchagePricingType SurchargePricingType { get; set; }
        public SurchargeProductTypes SurchargeProductType { get; set; }
        public FuelSurchageArea FuelSurchageArea { get; set; } = FuelSurchageArea.US;
    }
  }
