using System;
using System.Collections.Generic;
using System.Linq;

namespace SiteFuel.FreightModels
{
    public class ForcastingServiceSetting
    {
        public int Id { get; set; }
        public int ForcastingSettingId { get; set; }
        public int BandPeriod { get; set; }
        public TimeSpan StartTiming { get; set; }
        public decimal MinimumLoadQty { get; set; }
        public decimal AverageLoadQty { get; set; }
        public int InventoryPriorityType { get; set; }
        public int InventoryUOM { get; set; }
        public int RetainCouldGo { get; set; }
        public int SafetyStockShouldGo { get; set; }
        public int RunoutLevelMustGo { get; set; }
        public bool IsAutoDRCreation { get; set; }
        public bool IsOttoAutoDRCreation { get; set; }
        public int StartBuffer { get; set; }
        public int StartBufferUOM { get; set; }
        public int EndBuffer { get; set; }
        public int EndBufferUOM { get; set; }
        public int RetainTimeBuffer { get; set; }
        public int RetainTimeBufferUOM { get; set; }
        public int LeadTime { get; set; }
        public int LeadTimeUOM { get; set; }
        public int SupplierLead { get; set; }
        public int SupplierLeadUOM { get; set; }
        public List<int> CarrierList { get; set; } = new List<int>();
        public int IsAllCarrierEnabled { get; set; }
        public bool IsOttoScheduleCreation { get; set; }

    }
    public class ForcastingServiceCarrier
    {
        public int ForcastingServiceSettingId { get; set; }
        public string CarrierIds { get; set; }
        public bool IsAllCarrierEnabled { get; set; }
        public List<int> CarrierIdList
        {
            get
            {
                if (!string.IsNullOrEmpty(CarrierIds))
                {
                    return CarrierIds.Split(',').Select(Int32.Parse).ToList();
                }
                else
                {
                    return new List<int>();
                }
            }
        }
    }

}