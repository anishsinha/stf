using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    public static class Extensions
    {
        public static Expression<Func<DeliveryScheduleXTrackableSchedule, bool>> IsTrackableScheduleDelivered()
        {
            return deliveryScheduleXTrackableSchedule => deliveryScheduleXTrackableSchedule.DeliveryScheduleStatusId == 7 || deliveryScheduleXTrackableSchedule.DeliveryScheduleStatusId == 8 || deliveryScheduleXTrackableSchedule.DeliveryScheduleStatusId == 9 || deliveryScheduleXTrackableSchedule.DeliveryScheduleStatusId == 10 || deliveryScheduleXTrackableSchedule.DeliveryScheduleStatusId == 22 || deliveryScheduleXTrackableSchedule.DeliveryScheduleStatusId == 24;
        }

        public static Func<DeliveryScheduleXTrackableSchedule, bool> IsTrackableScheduleDeliveredFunc()
        {
            return deliveryScheduleXTrackableSchedule => deliveryScheduleXTrackableSchedule.DeliveryScheduleStatusId == 7 || deliveryScheduleXTrackableSchedule.DeliveryScheduleStatusId == 8 || deliveryScheduleXTrackableSchedule.DeliveryScheduleStatusId == 9 || deliveryScheduleXTrackableSchedule.DeliveryScheduleStatusId == 10 || deliveryScheduleXTrackableSchedule.DeliveryScheduleStatusId == 22 || deliveryScheduleXTrackableSchedule.DeliveryScheduleStatusId == 24;
        }

        public static Expression<Func<DeliveryScheduleXTrackableSchedule, bool>> IsTrackableScheduleUnDelivered()
        {
            return deliveryScheduleXTrackableSchedule => (deliveryScheduleXTrackableSchedule.DeliveryScheduleStatusId != 7 && deliveryScheduleXTrackableSchedule.DeliveryScheduleStatusId != 8 && deliveryScheduleXTrackableSchedule.DeliveryScheduleStatusId != 9 && deliveryScheduleXTrackableSchedule.DeliveryScheduleStatusId != 10 && deliveryScheduleXTrackableSchedule.DeliveryScheduleStatusId != 22 && deliveryScheduleXTrackableSchedule.DeliveryScheduleStatusId != 24);
        }

        public static Func<DeliveryScheduleXTrackableSchedule, bool> IsTrackableScheduleUnDeliveredFunc()
        {
            return deliveryScheduleXTrackableSchedule => (deliveryScheduleXTrackableSchedule.DeliveryScheduleStatusId != 7 && deliveryScheduleXTrackableSchedule.DeliveryScheduleStatusId != 8 && deliveryScheduleXTrackableSchedule.DeliveryScheduleStatusId != 9 && deliveryScheduleXTrackableSchedule.DeliveryScheduleStatusId != 10 && deliveryScheduleXTrackableSchedule.DeliveryScheduleStatusId != 22 && deliveryScheduleXTrackableSchedule.DeliveryScheduleStatusId != 24);
        }

        public static Expression<Func<DeliveryScheduleXTrackableSchedule, bool>> IsOpenTrackableSchedule()
        {
            return deliveryScheduleXTrackableSchedule => (deliveryScheduleXTrackableSchedule.DeliveryScheduleStatusId != 5 && deliveryScheduleXTrackableSchedule.DeliveryScheduleStatusId != 22 && deliveryScheduleXTrackableSchedule.DeliveryScheduleStatusId != 24 && deliveryScheduleXTrackableSchedule.DeliveryScheduleStatusId != 11 && deliveryScheduleXTrackableSchedule.DeliveryScheduleStatusId != 12 && deliveryScheduleXTrackableSchedule.DeliveryScheduleStatusId != 21 && deliveryScheduleXTrackableSchedule.DeliveryScheduleStatusId != 7 && deliveryScheduleXTrackableSchedule.DeliveryScheduleStatusId != 8 && deliveryScheduleXTrackableSchedule.DeliveryScheduleStatusId != 9 && deliveryScheduleXTrackableSchedule.DeliveryScheduleStatusId != 10);
        }
        public static Expression<Func<DeliveryScheduleXTrackableSchedule, bool>> IsOpenMissedTrackableSchedule()
        {
            return deliveryScheduleXTrackableSchedule => (deliveryScheduleXTrackableSchedule.DeliveryScheduleStatusId != 5 && deliveryScheduleXTrackableSchedule.DeliveryScheduleStatusId != 22 && deliveryScheduleXTrackableSchedule.DeliveryScheduleStatusId != 24 && deliveryScheduleXTrackableSchedule.DeliveryScheduleStatusId != 21 && deliveryScheduleXTrackableSchedule.DeliveryScheduleStatusId != 7 && deliveryScheduleXTrackableSchedule.DeliveryScheduleStatusId != 8 && deliveryScheduleXTrackableSchedule.DeliveryScheduleStatusId != 9 && deliveryScheduleXTrackableSchedule.DeliveryScheduleStatusId != 10);
        }
        // <summary>
        ///     A DbEntityValidationResult extension method that to strings database validation errors.
        /// </summary>
        public static string DbValidationErrorsToString(this DbEntityValidationResult dbEntityValidationResult, IEnumerable<DbValidationError> dbValidationErrors)
        {
            var entityName = string.Format("[{0}]", dbEntityValidationResult.Entry.Entity.GetType().Name);
            const string indentation = "\t - ";
            var aggregatedValidationErrorMessages = dbValidationErrors.Select(error => string.Format("[{0} - {1}]", error.PropertyName, error.ErrorMessage))
                                                   .Aggregate(string.Empty, (current, validationErrorMessage) => current + (Environment.NewLine + indentation + validationErrorMessage));
            return string.Format("{0}{1}", entityName, aggregatedValidationErrorMessages);
        }

        public static DateTimeOffset ToBrowserDateTime(this DateTimeOffset dateTime, TimeSpan timeZoneOffset)
        {
            return dateTime.ToOffset(timeZoneOffset);
        }

        public static string ReplaceGallonToLitre(this string sourceString)
        {
            return sourceString?.Replace("Gallon", "Litre").Replace("gallon", "litre");
        }

        public static string DisplayName<T>(this string propertyName, int currencyType = 0)
        {
            string displayName = "";
            MemberInfo property = typeof(T).GetProperty(propertyName);
            var displayAttribute = property.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;
            if (displayAttribute != null)
            {
                displayName = displayAttribute.Name;
            }
            else
            {
                displayName = propertyName;
            }

            string[] amountProperties = new string[] { "InvoiceAmount", "FuelAmount", "StateSalesTax", "StateTax", "FederalTax", "DeliveryAmount" };
            if (amountProperties.Contains(propertyName) && currencyType > 0)
            {
                var currency = currencyType == 1 ? "USD" : "CAD";
                displayName += " (" + currency + ") ";
            }
            return displayName;
        }

        public static string CropToLastChars(this string stringValue, int numChars)
        {
            if (!string.IsNullOrEmpty(stringValue))
            {
                var startIndex = stringValue.Length > numChars ? stringValue.Length - numChars : 0;
                return stringValue.Substring(startIndex, Math.Min(stringValue.Length, numChars));
            }
            else
            {
                return stringValue;
            }
        }

        public static string CropToFirstChars(this string stringValue, int numChars)
        {
            if (!string.IsNullOrEmpty(stringValue))
                return stringValue.Substring(0, Math.Min(stringValue.Length, numChars));
            else
                return stringValue;
        }

        public static string ReplaceGallonToGallonsLitres(this string sourceString)
        {
            return sourceString?.Replace("Gallons", "Gallons/Litres").Replace("gallons", "Gallons/Litres");
        }

        public static List<string> SplitAddress(this string address, int maxCharLength = 35)
        {
            if (address != null)
                address = address.Trim();
            
            var response = new List<string>() { address, null };
            if (!string.IsNullOrWhiteSpace(address))
            {
                if (address.Length > maxCharLength)
                {
                    var lastIndexOfSpace = address.Substring(0, maxCharLength).LastIndexOf(" ");
                    if (lastIndexOfSpace <= 0 || lastIndexOfSpace > maxCharLength)
                        lastIndexOfSpace = maxCharLength;
                    response[0] = address.Substring(0, lastIndexOfSpace).Trim();
                    if((lastIndexOfSpace+ Math.Min(maxCharLength, address.Length - response[0].Length))> address.Length)
                    response[1] = address.Substring(lastIndexOfSpace, (Math.Min(maxCharLength, address.Length - response[0].Length)-1));
                else
                        response[1] = address.Substring(lastIndexOfSpace, Math.Min(maxCharLength, address.Length - response[0].Length));
                }
            }
            return response;
        }
    }
}
