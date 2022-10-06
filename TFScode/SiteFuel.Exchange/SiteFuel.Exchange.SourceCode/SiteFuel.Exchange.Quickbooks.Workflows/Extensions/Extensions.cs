using SiteFuel.Exchange.Quickbooks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Quickbooks.Workflows.Extensions
{
    public static class Extensions
    {
        public static string ToDateString(this DateTimeOffset dateTimeOffset)
        {
            return dateTimeOffset > DateTimeOffset.MinValue ? dateTimeOffset.ToString(QbConstants.DateYYYMMDD) : null;
        }

        public static string ToQbItemName(this string itemName, int fuelTypeId)
        {
            if (!string.IsNullOrWhiteSpace(itemName))
            {
                itemName = itemName.RemoveSpecialCharacter();
                var suffix = $" - {fuelTypeId}";
                itemName = itemName.Substring(0, Math.Min(itemName.Length, 28 - suffix.Length)) + suffix;
            }
            return itemName;
        }

        public static string CropToLastChars(this string stringValue, int numChars)
        {
            var startIndex = stringValue.Length > numChars ? stringValue.Length - numChars : 0;
            return stringValue.Substring(startIndex, Math.Min(stringValue.Length, numChars));
        }

        public static string SubstringBeforeLast(this string value, string character, int maxStringLength)
        {
            if (!string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(character) && maxStringLength > 0)
            {
                while (value.Contains(character) && value.Length > maxStringLength)
                {
                    int lastIndex = value.LastIndexOf(character);
                    value = value.Substring(0, lastIndex);
                }
                if (value.Length > maxStringLength)
                {
                    value = value.Substring(0, Math.Min(value.Length, maxStringLength));
                }
            }
            return value;
        }

        public static string RemoveSpecialCharacter(this string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                value = value.Replace(":", string.Empty);
            }
            return value;
        }
    }
}
