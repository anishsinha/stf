using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Globalization;

namespace SiteFuel.Exchange.ViewModels
{
    public class CsvDeliveryFee
    {
        public CsvDeliveryFee(string deliveryFee,string maxQuantity)
        {
            FeesByQuantity = new List<ByQuantity>();
            FeeSubType = FeeSubType.NoFee;
            if (!string.IsNullOrWhiteSpace(deliveryFee))
            {
                var fields = deliveryFee.Split(' ');
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
                if (fields[1].Equals(ApplicationConstants.CsvFeeSubTypeFlatFee, StringComparison.OrdinalIgnoreCase) && Regex.IsMatch(fields[2], @"^\$[0-9]+(\.[0-9]{1,2})*$"))
                {
                    FeeSubType = FeeSubType.FlatFee;
                    Fee = decimal.Parse(fields[2].Replace("$", string.Empty));
                }
                else if ((fields[1].Equals(ApplicationConstants.CsvFeeSubTypePerGallon, StringComparison.OrdinalIgnoreCase) 
                    || fields[1].Equals(ApplicationConstants.CsvFeeSubTypePerLitre, StringComparison.OrdinalIgnoreCase))
                    && Regex.IsMatch(fields[2], @"^\$[0-9]+(\.[0-9]{1,2})*$"))
                {
                    FeeSubType = FeeSubType.PerGallon;
                    Fee = decimal.Parse(fields[2].Replace("$", string.Empty));
                }
                else if (fields[1].Equals(ApplicationConstants.CsvFeeSubTypeByQuantity, StringComparison.OrdinalIgnoreCase))
                {
                    FeeSubType = FeeSubType.ByQuantity;
                    foreach (var field in fields[2].Split('|'))
                    {
                        if (Regex.IsMatch(field, @"^[0-9]*\-[0-9]*-\$[0-9]*$"))
                        {
                            var feeRecord = field.Split('-');
                            FeesByQuantity.Add(new ByQuantity(feeRecord[0], feeRecord[1], feeRecord[2]));
                        }
                        else
                        {
                            throw new FormatException("Delivery Fee is not in proper format");
                        }
                    }
                    if (FeesByQuantity.Last().MaxQuantity != Decimal.Parse(maxQuantity))
                    {
                        throw new FormatException("Delivery Fee Quantity should covered Total quantity");
                    }
                }
                else
                {
                    throw new FormatException("Delivery Fee is not in proper format");
                }
            }
        }

        public FeeSubType FeeSubType { get; set; }

        public decimal Fee { get; set; }

        public bool IncludeInPPG { get; set; }

        public List<ByQuantity> FeesByQuantity { get; set; }

        public FeeConstraintType? FeeConstraintTypeId { get; set; }

        public DateTimeOffset? SpecialDate { get; set; }
    }

    public class ByQuantity
    {
        public ByQuantity(string minQuantity, string maxQuantity, string fee)
        {
            MinQuantity = decimal.Parse(minQuantity);
            MaxQuantity = decimal.Parse(maxQuantity);
            Fee = decimal.Parse(fee.Replace("$", string.Empty));
        }

        public decimal MinQuantity { get; set; }

        public decimal MaxQuantity { get; set; }

        public decimal Fee { get; set; }
    }
}
