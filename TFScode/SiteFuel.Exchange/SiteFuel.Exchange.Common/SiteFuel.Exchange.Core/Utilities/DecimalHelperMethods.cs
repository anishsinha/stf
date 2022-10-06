using System;
using System.Linq;

namespace SiteFuel.Exchange.Utilities
{
    public static class DecimalHelperMethods
    {
        public static decimal GetPreciseValue(this decimal value, int scale = 8)
        {
            decimal response = value;
            try
            {
                response = Convert.ToDecimal(value.ToString($"0.{string.Concat(Enumerable.Repeat("#", scale))}"));
            }
            catch (Exception)
            {
            }
            return response;
        }

        public static string GetPreciseValue(this decimal? value, int scale = 8)
        {

            try
            {
                if (value.HasValue && value.Value > 0)
                {
                    decimal response = value.Value;
                    response = Convert.ToDecimal(value.Value.ToString($"0.{string.Concat(Enumerable.Repeat("#", scale))}"));
                    return response.ToString();
                }
            }
            catch (Exception)
            {
            }
            return "-";
        }

        public static double GetPreciseValue(this double value, int scale = 8)
        {
            double response = value;
            try
            {
                response = Convert.ToDouble(value.ToString($"0.{string.Concat(Enumerable.Repeat("#", scale))}"));
            }
            catch (Exception)
            {
            }
            return response;
        }

        public static float GetPreciseValue(this float value, int scale = 8)
        {
            float response = value;
            try
            {
                response = float.Parse(value.ToString($"0.{string.Concat(Enumerable.Repeat("#", scale))}"));
            }
            catch (Exception)
            {
            }
            return response;
        }

        public static string GetCommaSeperatedValue(this decimal value)
        {
            string response = value.ToString();
            try
            {

               if (value != 0)
                    response = value.ToString($"#,##.##");              
            }
            catch (Exception)
            {
            }
            return response;
        }

        public static string GetInvoiceAmountValue(this decimal value, int precision, string currency)
        {
            string response = string.Empty;

            if (value > 0)
                response = currency + value.ToString("N" + precision);
            else if (value < 0)
                response = "-" + currency + Math.Abs(value).ToString("N" + precision);
            else
                response = currency + "0";
            return response;
        }

        public static string GetInvoiceAmountReverseValue(this decimal value, int precision, string currency)
        {
            string response = string.Empty;

            if (value > 0)
                response = "-" + currency + value.ToString("N" + precision);
            else if (value < 0)
                response = currency + Math.Abs(value).ToString("N" + precision);
            else
                response = currency + "0";

            return response;
        }
        public static string GetCommaSeperatedValue4Decimals(this decimal value)
        {
            string response = value.ToString();
            try
            {

                if (value != 0)
                    response = value.ToString($"#,##.####");
            }
            catch (Exception)
            {
            }
            return response;
        }
    }
}