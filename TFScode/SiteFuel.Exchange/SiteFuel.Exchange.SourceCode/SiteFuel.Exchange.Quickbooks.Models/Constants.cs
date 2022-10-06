using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Quickbooks.Models
{
    public static class QbConstants
    {
        public const int MaxDecimal = 5;
        public const int MaxStringLength = 41;
        public const int MaxCompanyLength = 39;
        public const string QbEncoding = "iso-8859-1";

        public const string DateYYYMMDD = "yyyy-MM-dd";
        public const string DryRunFee = "Dry Run Fee";
        public const string AdditionalFee = "Additional Fee";
        public const string FuelSurcharges = "Fuel Surcharges";
        public const string UnderNGallon = "(Under {0} Gallons)";
        public const string TxnLineID = "TxnLineID{0}";
        public const string CustomerNumberPrefix = "TFCU";
        public const string Vendor = "-V";
        public const string DiscountOn = "Discount on";
        public const string SupplierAllowance = "Supplier Allowance";
        public const string PercentSymbol = "%";
    }
}
