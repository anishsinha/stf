using SiteFuel.Exchange.Utilities;
using System;
using System.Text.RegularExpressions;

namespace SiteFuel.Exchange.ViewModels
{
    public class CsvFreightFee
    {
        public CsvFreightFee(string freightFee)
        {
            FeeSubType = FeeSubType.NoFee;
            if (!string.IsNullOrWhiteSpace(freightFee))
            {
                var fields = freightFee.Split(' ');
                if (fields.Length == 1 && fields[0].Equals("NoFee", StringComparison.OrdinalIgnoreCase))
                {
                    FeeSubType = FeeSubType.NoFee;
                }
                else if (fields[0].Equals(ApplicationConstants.CsvFeeSubTypePerRoute, StringComparison.OrdinalIgnoreCase) && Regex.IsMatch(fields[1], @"^\$[0-9]+(\.[0-9]{1,2})*$"))
                {
                    FeeSubType = FeeSubType.PerRoute;
                    Fee = decimal.Parse(fields[1].Replace("$", string.Empty));
                }
                else if (fields[0].Equals(ApplicationConstants.CsvFeeSubTypePerAsset, StringComparison.OrdinalIgnoreCase) && Regex.IsMatch(fields[1], @"^\$[0-9]+(\.[0-9]{1,2})*$"))
                {
                    FeeSubType = FeeSubType.ByAssetCount;
                    Fee = decimal.Parse(fields[1].Replace("$", string.Empty));
                }
                else if ((fields[0].Equals(ApplicationConstants.CsvFeeSubTypePerGallon, StringComparison.OrdinalIgnoreCase)
                    || fields[0].Equals(ApplicationConstants.CsvFeeSubTypePerLitre, StringComparison.OrdinalIgnoreCase))
                    && Regex.IsMatch(fields[1], @"^\$[0-9]+(\.[0-9]{1,2})*$"))
                {
                    FeeSubType = FeeSubType.PerGallon;
                    Fee = decimal.Parse(fields[1].Replace("$", string.Empty));
                }
                else
                {
                    throw new FormatException("Freight Fee is not in proper format");
                }
            }
        }

        public FeeSubType FeeSubType { get; set; }

        public decimal Fee { get; set; }
    }
}
