using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class FreightCostFeeViewModel
    {
        public FreightCostFeeViewModel()
        {
            Currency = Currency.USD;
            UoM = UoM.Gallons;
            FeeTypeId = (int)FeeType.FreightCost;
            FeeSubTypeId = (int)FeeSubType.FlatFee;
            IsFreightCostApplicable = false;
        }

        public int Id { get; set; }

        public int FeeTypeId { get; set; }

        public int FeeSubTypeId { get; set; }

        public decimal Fee { get; set; }

        public Currency Currency { get; set; }

        public UoM UoM { get; set; }

        public bool IsFreightCostApplicable { get; set; }
    }
}
