using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Globalization;

namespace SiteFuel.Exchange.ViewModels
{
    public class CsvAdditionalFee
    {
        public CsvAdditionalFee(string field)
        {
            var feeRecord = field.Split(' ');
            if (feeRecord[3].Equals(ApplicationConstants.CsvFeeIncludeInPPG, StringComparison.OrdinalIgnoreCase)) { IncludeInPPG = true; }
            if (feeRecord.Length > 4) {
                if (feeRecord[4].Equals(ApplicationConstants.CsvWeekendFee, StringComparison.OrdinalIgnoreCase))
                {
                    FeeConstraintTypeId = FeeConstraintType.Weekend;
                }
                else if (feeRecord[4].Equals(ApplicationConstants.CsvSpecialDateFee, StringComparison.OrdinalIgnoreCase))
                {
                    if (feeRecord.Length > 5 && feeRecord[5].Contains("/"))
                    {
                        FeeConstraintTypeId = FeeConstraintType.SpecialDate;
                        SpecialDate = Convert.ToDateTime(feeRecord[5], new CultureInfo("en-US"));
                    }
                    else
                    {
                        throw new FormatException("Special Date is not in proper format");
                    }
                }
            }
            if(Regex.IsMatch(feeRecord[2], @"^\$[0-9]+(\.[0-9]{1,2})*$"))
            {
                Fee = decimal.Parse(feeRecord[2].Replace("$", string.Empty));
            }
            else
            {
                throw new FormatException("Fee is not in proper format");
            }
            switch (feeRecord[0].ToLower())
            {
                case "environmentalfee":
                    FeeType = FeeType.EnvironmentalFee;
                    if (feeRecord[1].Equals(ApplicationConstants.CsvFeeSubTypeFlatFee, StringComparison.OrdinalIgnoreCase))
                        FeeSubType = FeeSubType.FlatFee;
                    else if (feeRecord[1].Equals(ApplicationConstants.CsvFeeSubTypePerGallon, StringComparison.OrdinalIgnoreCase))
                        FeeSubType = FeeSubType.PerGallon;
                    else
                        throw new FormatException("Environmentalfee allowed only FlatFee and PerGallon sub types");
                    break;

                case "servicefee":
                    FeeType = FeeType.ServiceFee;
                    if (feeRecord[1].Equals(ApplicationConstants.CsvFeeSubTypeFlatFee, StringComparison.OrdinalIgnoreCase))
                        FeeSubType = FeeSubType.FlatFee;
                    else if (feeRecord[1].Equals(ApplicationConstants.CsvFeeSubTypePerGallon, StringComparison.OrdinalIgnoreCase))
                        FeeSubType = FeeSubType.PerGallon;
                    else if (feeRecord[1].Equals(ApplicationConstants.CsvFeeSubTypePerHour, StringComparison.OrdinalIgnoreCase))
                        FeeSubType = FeeSubType.HourlyRate;
                    else
                        throw new FormatException("Servicefee allowed only FlatFee, PerGallon and PerHour sub types");
                    break;

                case "loadfee":
                    FeeType = FeeType.LoadFee;
                    if (feeRecord[1].Equals(ApplicationConstants.CsvFeeSubTypeFlatFee, StringComparison.OrdinalIgnoreCase))
                        FeeSubType = FeeSubType.FlatFee;
                    else if (feeRecord[1].Equals(ApplicationConstants.CsvFeeSubTypePerGallon, StringComparison.OrdinalIgnoreCase))
                        FeeSubType = FeeSubType.PerGallon;
                    else
                        throw new FormatException("Loadfee allowed only FlatFee and PerGallon sub types");
                    break;

                case "surchargefee":
                    FeeType = FeeType.SurchargeFee;
                    if (feeRecord[1].Equals(ApplicationConstants.CsvFeeSubTypeFlatFee, StringComparison.OrdinalIgnoreCase))
                        FeeSubType = FeeSubType.FlatFee;
                    else if (feeRecord[1].Equals(ApplicationConstants.CsvFeeSubTypePerGallon, StringComparison.OrdinalIgnoreCase))
                        FeeSubType = FeeSubType.PerGallon;
                    else if (feeRecord[1].Equals(ApplicationConstants.CsvFeeSubTypePerHour, StringComparison.OrdinalIgnoreCase))
                        FeeSubType = FeeSubType.HourlyRate;
                    else
                        throw new FormatException("Environmentalfee allowed only FlatFee, PerGallon and PerHour sub types");
                    break;

                case "processingfee":
                    FeeType = FeeType.ProcessingFee;
                    if (feeRecord[1].Equals(ApplicationConstants.CsvFeeSubTypeFlatFee, StringComparison.OrdinalIgnoreCase))
                        FeeSubType = FeeSubType.FlatFee;
                    else if (feeRecord[1].Equals(ApplicationConstants.CsvFeeSubTypePerGallon, StringComparison.OrdinalIgnoreCase))
                        FeeSubType = FeeSubType.PerGallon;
                    else
                        throw new FormatException("Environmentalfee allowed only FlatFee and PerGallon sub types");
                    break;

                default:
                    throw new FormatException("Common Fee is not in proper format");
            }
        }

        public FeeSubType FeeSubType { get; set; }
        public FeeType FeeType { get; set; }

        public decimal Fee { get; set; }
        public bool IncludeInPPG { get; set; }

        public FeeConstraintType? FeeConstraintTypeId { get; set; }

        public DateTimeOffset? SpecialDate { get; set; }
    }
}
