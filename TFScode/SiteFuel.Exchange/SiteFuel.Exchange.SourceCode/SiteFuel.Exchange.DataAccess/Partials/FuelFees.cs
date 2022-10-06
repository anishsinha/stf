using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    public partial class FuelFee
    {
        public FuelFee Clone()
        {
            var newFee = new FuelFee();
            newFee.Currency = Currency;
            newFee.Fee = Fee;
            newFee.FeeConstraintTypeId = FeeConstraintTypeId;
            newFee.FeeDetails = FeeDetails;
            newFee.FeeSubQuantity = FeeSubQuantity;
            newFee.FeeSubTypeId = FeeSubTypeId;
            newFee.FeeTypeId = FeeTypeId;
            newFee.IncludeInPPG = IncludeInPPG;
            newFee.MinimumGallons = MinimumGallons;
            newFee.OtherFeeTypeId = OtherFeeTypeId;
            newFee.SpecialDate = SpecialDate;
            newFee.TotalFee = TotalFee;
            newFee.UoM = UoM;
            newFee.WaiveOffTime = WaiveOffTime;
            
            foreach (var byQuantity in FeeByQuantities)
            {
                var feeByQty = new FeeByQuantity();
                feeByQty.Currency = byQuantity.Currency;
                feeByQty.Fee = byQuantity.Fee;
                feeByQty.FeeSubTypeId = byQuantity.FeeSubTypeId;
                feeByQty.FeeTypeId = byQuantity.FeeTypeId;
                feeByQty.MaxQuantity = byQuantity.MaxQuantity;
                feeByQty.MinQuantity = byQuantity.MinQuantity;
                feeByQty.UoM = byQuantity.UoM;
                newFee.FeeByQuantities.Add(feeByQty);
            }
            return newFee;
        }
    }
}
