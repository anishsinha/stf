using SiteFuel.Exchange.Logger;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Text;

namespace SiteFuel.Exchange.Utilities
{
	public static class StringHelperMethods
	{
		//Use this for converting primitive datatypes only and not objects
		public static T GetValue<T>(this string value)
		{
			T response = default(T);
			try
			{
				if (!string.IsNullOrWhiteSpace(value))
				{
					response = (T)Convert.ChangeType(value, typeof(T));
				}
			}
			catch (Exception ex)
			{
				LogManager.Logger.WriteException("StringHelperMethods", "GetValue", ex.Message, ex);
			}
			return response;
		}

		public static DateTimeOffset GetDateTimeOffsetValue(this string value, string format = "MM/dd/yyyy HH:mm:ss zzz")
		{
			DateTimeOffset response = DateTimeOffset.MinValue;
			try
			{
				response = DateTimeOffset.ParseExact(
											value,
											format,
											CultureInfo.InvariantCulture);
			}
			catch (Exception ex)
			{
				LogManager.Logger.WriteException("StringHelperMethods", "GetDateTimeOffsetValue", $"Failed to parse {value} to DateTimeOffset with format {format}", ex);
			}
			return response;
		}

		public static DateTimeOffset GetFilterStartDateInDateTimeOffset(this string value)
		{
			DateTimeOffset StartDate = Convert.ToDateTime("01/01/2016");
			if (string.IsNullOrEmpty(value))
			{
				StartDate = DateTimeOffset.Now.Date.AddDays(ApplicationConstants.DateFilterDefaultDays);
			}
			if (!string.IsNullOrEmpty(value))
			{
				StartDate = Convert.ToDateTime(value).Date;
			}
			return StartDate;
		}

		public static DateTimeOffset GetFilterEndDateInDateTimeOffset(this string value)
		{
			DateTimeOffset EndDate = DateTimeOffset.Now.Date.AddDays(1);
			if (!string.IsNullOrEmpty(value))
			{
				EndDate = Convert.ToDateTime(value).Date.AddDays(1);
			}
			return EndDate;
		}

		public static string ToFormattedPhoneNumber(this string value)
		{
			if (!string.IsNullOrWhiteSpace(value) && value.Length == 10)
			{
				value = $"{value.Substring(0, 3)}-{value.Substring(3, 3)}-{value.Substring(6, 4)}";
			}
			return value;
		}

		public static string ToUnFormattedPhoneNumber(this string value)
		{
			if (!string.IsNullOrWhiteSpace(value))
			{
				value = value.Replace("-",string.Empty).Replace("(", string.Empty).Replace(")", string.Empty);
			}
			return value;
		}
		public static string FormattedInvoiceNumber(this string invoiceNumber, int invoiceTypeId, WaitingAction waitingFor = WaitingAction.Nothing)
		{
			if (string.IsNullOrWhiteSpace(invoiceNumber))
			{
				return "";
			}
            if (waitingFor == WaitingAction.ExceptionApproval)
            {
                invoiceNumber = invoiceNumber.Replace(ApplicationConstants.SFIN, ApplicationConstants.SFEDD);
                invoiceNumber = invoiceNumber.Replace(ApplicationConstants.SFDD, ApplicationConstants.SFEDD);
                invoiceNumber = invoiceNumber.Replace(ApplicationConstants.SFRB, ApplicationConstants.SFEDD);
            }
			else if (invoiceTypeId == (int)InvoiceType.DigitalDropTicketManual || invoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp)
			{
				invoiceNumber = invoiceNumber.Replace(ApplicationConstants.SFIN, ApplicationConstants.SFDD);
			}
            else if (invoiceTypeId == (int)InvoiceType.CreditInvoice || invoiceTypeId == (int)InvoiceType.PartialCredit)
            {
                invoiceNumber = invoiceNumber.Replace(ApplicationConstants.SFIN, ApplicationConstants.SFCI);
            }
            else if (invoiceTypeId == (int)InvoiceType.RebillInvoice)
            {
                invoiceNumber = invoiceNumber.Replace(ApplicationConstants.SFIN, ApplicationConstants.SFRB);
            }
            else
            {
                invoiceNumber = invoiceNumber.Replace(ApplicationConstants.SFDD, ApplicationConstants.SFIN);
                invoiceNumber = invoiceNumber.Replace(ApplicationConstants.SFEDD, ApplicationConstants.SFIN);
            }

            return invoiceNumber;
		}

		public static string ToAuxiliaryID(this string itemName, int itemId)
		{
			if (!string.IsNullOrWhiteSpace(itemName))
			{
				var suffix = $"-{itemId}";
				itemName = itemName.Substring(0, Math.Min(itemName.Length, 28 - suffix.Length)) + suffix;
			}
			return itemName;
		}

		public static string CovertUnicodeToNormal(this string text)
		{
            //try
            //{
            //	var normalizedString = text.Normalize(NormalizationForm.FormD);
            //	var stringBuilder = new StringBuilder();

            //	foreach (var c in normalizedString)
            //	{
            //		var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            //		if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            //		{
            //			stringBuilder.Append(c);
            //		}
            //	}
            //	return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
            //}
            //catch (Exception)
            //{
            //	Encoding ascii = Encoding.ASCII;
            //	Encoding unicode = Encoding.Unicode;
            //	byte[] unicodeBytes = unicode.GetBytes(text);

            //	// // Perform the conversion from one encoding to the other.
            //	byte[] ascibytes = Encoding.Convert(unicode, ascii, unicodeBytes);

            //	// // Convert the new byte[] into a char[] and then into a string.
            //	char[] asciiChars = new char[ascii.GetCharCount(ascibytes, 0, ascibytes.Length)];
            //	ascii.GetChars(ascibytes, 0, ascibytes.Length, asciiChars, 0);
            //	return new string(asciiChars);
            //}
            return text;
		}

        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static string ToFormattedStringInLowerCase(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            return value.ToLower().Trim().Replace("'", "`");
        }

		public static bool IsKeyMatchAnyValue(this string key, string value1, string value2, string value3 = null)
		{
			var tKey = key.ToLower();
			return tKey == value1.ToLower() || tKey == value2.ToLower() || (value3 != null && tKey == value3.ToLower());
		}

		public static bool IsValidCountryForTax(this string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				return value.ToLower().Equals("us") || value.ToLower().Equals("usa") || value.ToLower().Equals("can");
			}
			return false;
		}

		public static string ToHexWithFixedNumber(this int number)
        {
			return  String.Format("0x{0:X}", number * 2021);
		}

		public static int ToIntNumber(this string stringToConvert)
		{
			try
			{
				if (!string.IsNullOrWhiteSpace(stringToConvert))
				{
					if (stringToConvert.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
					{
						stringToConvert = stringToConvert.Substring(2);
					}
					return Int32.Parse(stringToConvert, NumberStyles.HexNumber) / 2021;
				}
			}
			catch (Exception)
			{
			}
			return 0;
		}

        public static string ToURLQueryString(this IDictionary<string,string> queryParams)
        {            
            NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
            foreach (var entry in queryParams)
            {               
                queryString.Add(entry.Key, entry.Value);
            }
            return "?"+queryString.ToString();
        }

		public static string IfNullSetNewValue(this string value, string newValue)
		{
			if (!string.IsNullOrWhiteSpace(value))
				return value;
			else
				return newValue;
		}
	}
}
