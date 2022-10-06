using SiteFuel.Exchange.Utilities;
using System;
using System.Text.RegularExpressions;
using System.Globalization;

namespace SiteFuel.Exchange.ViewModels
{
    public class CsvWetHoseFee
    {
        public CsvWetHoseFee(string wetHoseFee)
        {
            FeeSubType = FeeSubType.NoFee;
            if (!string.IsNullOrWhiteSpace(wetHoseFee))
            {
                var fields = wetHoseFee.Split(' ');
                if (fields[3].Equals(ApplicationConstants.CsvFeeIncludeInPPG, StringComparison.OrdinalIgnoreCase)) { IncludeInPPG = true; }
                if (fields.Length > 4)
                {
                    if (fields[4].Equals(ApplicationConstants.CsvWeekendFee, StringComparison.OrdinalIgnoreCase))
                    {
                        FeeConstraintTypeId = FeeConstraintType.Weekend;
                    }
                    else if (fields[4].Equals(ApplicationConstants.CsvSpecialDateFee, StringComparison.OrdinalIgnoreCase))
                    {
                        if (fields.Length > 5 && fields[5].Contains("/"))
                        {
                            FeeConstraintTypeId = FeeConstraintType.SpecialDate;
                            SpecialDate = Convert.ToDateTime(fields[5], new CultureInfo("en-US"));
                        }
                        else
                        {
                            throw new FormatException("Special Date is not in proper format");
                        }
                    }
                }
                if (fields[1].Equals(ApplicationConstants.CsvFeeSubTypePerAsset, StringComparison.OrdinalIgnoreCase) && Regex.IsMatch(fields[2], @"^\$[0-9]+(\.[0-9]{1,2})*$"))
                {
                    FeeSubType = FeeSubType.ByAssetCount;
                    Fee = decimal.Parse(fields[2].Replace("$", string.Empty));
                }
                else if (fields[1].Equals(ApplicationConstants.CsvFeeSubTypePerHour, StringComparison.OrdinalIgnoreCase) && Regex.IsMatch(fields[2], @"^\$[0-9]+(\.[0-9]{1,2})*$"))
                {
                    FeeSubType = FeeSubType.HourlyRate;
                    Fee = decimal.Parse(fields[2].Replace("$", string.Empty));
                }
                else
                {
                    throw new FormatException("Wet Hose Fee is not in proper format");
                }
            }
        }

        public FeeSubType FeeSubType { get; set; }

        public decimal Fee { get; set; }

        public bool IncludeInPPG { get; set; }

        public FeeConstraintType? FeeConstraintTypeId { get; set; }

        public DateTimeOffset? SpecialDate { get; set; }
    }
}
