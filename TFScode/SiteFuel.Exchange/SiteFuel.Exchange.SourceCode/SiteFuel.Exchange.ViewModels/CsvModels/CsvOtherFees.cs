using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace SiteFuel.Exchange.ViewModels
{
    public class CsvOtherFees
    {
        public CsvOtherFees(string otherFees)
        {
            Fees = new List<CsvOtherFee>();
            if (!string.IsNullOrWhiteSpace(otherFees))
            {
                var fields = otherFees.Split(';');
                foreach (var field in fields)
                {
                    var feeRecord = field.Split(' ');

                    if (feeRecord.Length < 4)
                        throw new FormatException("OtherFee is not in proper format");

                    Fees.Add(new CsvOtherFee(field));
                }
            }
        }

        public List<CsvOtherFee> Fees { get; set; }

    }

    public class CsvOtherFee
    {
        public CsvOtherFee(string field)
        {
            var feeRecord = field.Split(' ');

            var userDefinedFee = feeRecord[0].Replace('-', ' ');

            if (string.IsNullOrWhiteSpace(feeRecord[1]) || string.IsNullOrWhiteSpace(feeRecord[2]) || string.IsNullOrWhiteSpace(userDefinedFee))
                throw new FormatException("OtherFee is not in proper format");

            if (feeRecord[1].Equals(ApplicationConstants.CsvFeeSubTypeFlatFee, StringComparison.OrdinalIgnoreCase))
                FeeSubType = FeeSubType.FlatFee;
            else if (feeRecord[1].Equals(ApplicationConstants.CsvFeeSubTypePerGallon, StringComparison.OrdinalIgnoreCase))
                FeeSubType = FeeSubType.PerGallon;
            else if (feeRecord[1].Equals(ApplicationConstants.CsvFeeSubTypePerHour, StringComparison.OrdinalIgnoreCase))
                FeeSubType = FeeSubType.HourlyRate;
            else
                throw new FormatException("OtherFee allowed only FlatFee, PerGallon/PerLitre and PerHour sub types");

            Fee = decimal.Parse(feeRecord[2].Replace("$", string.Empty));

            FeeDescription = userDefinedFee;
            if (feeRecord[3].Equals(ApplicationConstants.CsvFeeIncludeInPPG, StringComparison.OrdinalIgnoreCase)) { IncludeInPPG = true; }
            if (feeRecord.Length > 4)
            {
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
        }

        public FeeSubType FeeSubType { get; set; }

        public string FeeDescription { get; set; }
        public decimal Fee { get; set; }

        public bool IncludeInPPG { get; set; }

        public FeeConstraintType? FeeConstraintTypeId { get; set; }

        public DateTimeOffset? SpecialDate { get; set; }
    }
}
