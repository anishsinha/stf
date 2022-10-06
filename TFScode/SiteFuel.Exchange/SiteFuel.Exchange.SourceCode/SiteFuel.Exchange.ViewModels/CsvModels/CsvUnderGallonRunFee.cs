using SiteFuel.Exchange.Utilities;
using System;
using System.Text.RegularExpressions;
using System.Globalization;

namespace SiteFuel.Exchange.ViewModels
{
    public class CsvUnderGallonRunFee
    {
        public CsvUnderGallonRunFee(string underGallonFee)
        {
            FeeSubType = FeeSubType.NoFee;
            if (!string.IsNullOrWhiteSpace(underGallonFee))
            {
                var fields = underGallonFee.Split(' ');
                if (fields.Length < 6)
                    throw new FormatException("Under Gallon/Under Litre Fee is not in proper format");
                if (fields[5].Equals(ApplicationConstants.CsvFeeIncludeInPPG, StringComparison.OrdinalIgnoreCase)) { IncludeInPPG = true; }
                if (fields.Length > 6)
                {
                    if (fields[6].Equals(ApplicationConstants.CsvWeekendFee, StringComparison.OrdinalIgnoreCase))
                    {
                        FeeConstraintTypeId = FeeConstraintType.Weekend;
                    }
                    else if (fields[6].Equals(ApplicationConstants.CsvSpecialDateFee, StringComparison.OrdinalIgnoreCase))
                    {
                        if (fields.Length > 7 && fields[7].Contains("/"))
                        {
                            FeeConstraintTypeId = FeeConstraintType.SpecialDate;
                            SpecialDate = Convert.ToDateTime(fields[7], new CultureInfo("en-US"));
                        }
                        else
                        {
                            throw new FormatException("Special Date in UnderGallonFee/UnderLitreFee is not in proper format");
                        }
                    }
                }
                if (Convert.ToDecimal(fields[1]) > 0 && fields[3].Equals(ApplicationConstants.CsvFeeSubTypeFlatFee, StringComparison.OrdinalIgnoreCase) && Regex.IsMatch(fields[4], @"^\$[0-9]+(\.[0-9]{1,2})*$"))
                {
                    FeeSubType = FeeSubType.FlatFee;
                    MinimumGallons = decimal.Parse(fields[1]);
                    Fee = decimal.Parse(fields[4].Replace("$", string.Empty));
                }
                else
                {
                    throw new FormatException("Under Gallon Fee is not in proper format");
                }
            }
        }

        public FeeSubType FeeSubType { get; set; }

        public decimal Fee { get; set; }

        public decimal MinimumGallons { get; set; }

        public bool IncludeInPPG { get; set; }
        public FeeConstraintType? FeeConstraintTypeId { get; set; }

        public DateTimeOffset? SpecialDate { get; set; }
    }
}
