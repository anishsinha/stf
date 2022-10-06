namespace SiteFuel.Exchange.Utilities
{
    public static class TaxPricingExtentionMethods
    {
        //Use this for converting primitive datatypes only and not objects
       
        public static string GetTaxPricingDescription(this int value, decimal taxRate)
        {
            if (value > 0)
            {
                switch(value)
                {
                    case 1:
                        return "$" + taxRate.GetPreciseValue(2) + ApplicationConstants.OnTotalAmount;
                    case 2:
                        return taxRate.GetPreciseValue(2) + "%" + ApplicationConstants.OnTotalAmount;
                    case 3:
                        return "$" + taxRate.GetPreciseValue(2) + ApplicationConstants.PerGallon;
                    case 4:
                        return taxRate.GetPreciseValue(2) + "%" + ApplicationConstants.PerGallon;
                }
            }
            return string.Empty;
        }

        public static SurchargeProductTypes GetFuelSurchargeProductType(this int value)
        {
            if (value == (int)ProductTypes.Unleaded || value == (int)ProductTypes.ConventionalGas || value == (int)ProductTypes.MidgradeGas || value == (int)ProductTypes.PremiumGas || value == (int)ProductTypes.RegularGas || value == (int)ProductTypes.OtherGas)
                return SurchargeProductTypes.Gasoline;
            else if (value == (int)ProductTypes.ClearDiesel || value == (int)ProductTypes.ClearDiesel2 || value == (int)ProductTypes.RedDyeDiesel || value == (int)ProductTypes.RedDyeDiesel2)
                return SurchargeProductTypes.Diesel;

            return SurchargeProductTypes.Unknown;
        }
    }
}
