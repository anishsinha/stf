using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class CsvBrokeredOtherFees
    {
        public CsvBrokeredOtherFees(string otherFees)
        {
            Fees = new List<CsvBrokeredOtherFee>();
            if (!string.IsNullOrWhiteSpace(otherFees))
            {
                var fields = otherFees.Split(';');
                foreach (var field in fields)
                {
                    var feeRecord = field.Split(' ');

                    if (feeRecord.Length < 3)
                        throw new FormatException("OtherFee is not in proper format");

                    Fees.Add(new CsvBrokeredOtherFee(feeRecord[0], feeRecord[1], feeRecord[2]));
                }
            }
        }

        public List<CsvBrokeredOtherFee> Fees { get; set; }

    }

    public class CsvBrokeredOtherFee
    {
        public CsvBrokeredOtherFee(string feeType, string feeSubType, string fee)
        {
            var userDefinedFee = feeType.Replace('-', ' ');

            if (string.IsNullOrWhiteSpace(feeSubType) || string.IsNullOrWhiteSpace(fee) || string.IsNullOrWhiteSpace(userDefinedFee))
                throw new FormatException("OtherFee is not in proper format");

            if (feeSubType.Equals(ApplicationConstants.CsvFeeSubTypeFlatFee, StringComparison.OrdinalIgnoreCase))
                FeeSubType = FeeSubType.FlatFee;
            else if (feeSubType.Equals(ApplicationConstants.CsvFeeSubTypePerGallon, StringComparison.OrdinalIgnoreCase))
                FeeSubType = FeeSubType.PerGallon;
            else if (feeSubType.Equals(ApplicationConstants.CsvFeeSubTypePerHour, StringComparison.OrdinalIgnoreCase))
                FeeSubType = FeeSubType.HourlyRate;
            else
                throw new FormatException("OtherFee allowed only FlatFee, PerGallon and PerHour sub types");

            Fee = decimal.Parse(fee.Replace("$", string.Empty));

            FeeDescription = userDefinedFee;
        }

        public FeeSubType FeeSubType { get; set; }

        public string FeeDescription { get; set; }
        public decimal Fee { get; set; }
    }
}