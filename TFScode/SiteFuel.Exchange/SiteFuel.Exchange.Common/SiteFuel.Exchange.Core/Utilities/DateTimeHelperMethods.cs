using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Logger;
using System;

namespace SiteFuel.Exchange.Utilities
{
    public static class DateTimeHelperMethods
    {

        public static DateTimeOffset ToClientDateTimeOffset(this DateTimeOffset value)
        {
            DateTimeOffset response = value;
            try
            {
                //// read the value from session
                //var timeOffSet = HttpContext.Current.Session[ApplicationConstants.TimeZoneOffset];
                //if (timeOffSet != null)
                //{
                //    var offset = double.Parse(timeOffSet.ToString());
                //    var timespanOffset = TimeSpan.FromMinutes(offset);
                //    response = value.ToOffset(timespanOffset);
                //}
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DateTimeHelperMethods", "ToClientDateTimeOffset", ex.Message, ex);
            }
            return response;
        }

        public static DateTimeOffset ToTargetDateTimeOffset(this DateTimeOffset value, string timeZoneName)
        {
            DateTimeOffset response = value;
            try
            {
                if (!string.IsNullOrEmpty(timeZoneName))
                {
                    timeZoneName = timeZoneName.ParseTimeZone();
                    TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneName);
                    return TimeZoneInfo.ConvertTime(value, timeZone);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DateTimeHelperMethods", "ToTargetDateTimeOffset", ex.Message, ex);
            }
            return response;
        }

        public static DateTimeOffset AttachOffset(this DateTimeOffset value, TimeSpan offset)
        {
            DateTimeOffset response = value;
            try
            {
                response = new DateTimeOffset(value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second, offset);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DateTimeHelperMethods", "AttachOffset", ex.Message, ex);
            }
            return response;
        }

        public static string ParseTimeZone(this string value)
        {
            string response = value;
            try
            {
                //add times zones here if its not found in standard list of TimeZones. - TimeZoneNotFoundException
                if (!string.IsNullOrWhiteSpace(response))
                {
                    response = response.Replace("Daylight", "Standard");
                    response = response.Replace("Alaska ", "Alaskan ");
                    response = response.Replace("Hawaii-Aleutian", "Hawaiian");
                    if (response.Contains("Mexican"))
                    {
                        response = response.Replace("Mexican ", string.Empty) + " (Mexico)";
                    }
                    response = response.Replace("Suriname Time", "E. South America Standard Time");
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DateTimeHelperMethods", "ParseTimeZone", ex.Message, ex);
            }
            return response;
        }

        public static string ToClientDateTime(this DateTimeOffset value, DateTimeFormat format = DateTimeFormat.DateTimeHHMM24)
        {
            string response = string.Empty;
            try
            {
                response = GetFormattedDateTime(value, format);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DateTimeHelperMethods", "ToClientDateTime", ex.Message, ex);
            }
            return response;
        }

        private static string GetFormattedDateTime(DateTimeOffset value, DateTimeFormat format = DateTimeFormat.DateTimeHHMM24)
        {
            string response = value.ToString();
            try
            {
                switch (format)
                {
                    case DateTimeFormat.Date:
                        response = value.ToClientDateTimeOffset().ToString("dd-MMM-yyyy");
                        break;
                    case DateTimeFormat.DateTimeHHMM24:
                        response = value.ToClientDateTimeOffset().ToString("dd-MMM-yyyy HH:mm");
                        break;
                    case DateTimeFormat.DateTimeHHMM12:
                        response = value.ToClientDateTimeOffset().ToString("dd-MMM-yyyy hh:mm t");
                        break;
                    case DateTimeFormat.DateTimeHHMMSS24:
                        response = value.ToClientDateTimeOffset().ToString("dd-MMM-yyyy HH:mm:ss");
                        break;
                    case DateTimeFormat.DateTimeHHMMSS12:
                        response = value.ToClientDateTimeOffset().ToString("dd-MMM-yyyy hh:mm:ss t");
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DateTimeHelperMethods", "ToClientDateTime", ex.Message, ex);
            }
            return response;
        }

        public static DateTimeOffset GetDateTimeOffsetWithTimeZone(this long timeInMilis)
        {
            DateTimeOffset localDateTime = DateTimeOffset.FromUnixTimeMilliseconds(timeInMilis);
            try
            {
                if (localDateTime != null)
                {
                    localDateTime = localDateTime.ToOffset(TimeZoneInfo.Local.GetUtcOffset(localDateTime));
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DateTimeHelperMethods", "GetDateTimeOffsetWithTimeZone", ex.Message, ex);
            }
            return localDateTime;
        }

        public static long ToUnixTimeMilliseconds(this string time)
        {
            long UtcTime = 0;
            try
            {
                DateTimeOffset parsedDate;
                if (DateTimeOffset.TryParse(time, out parsedDate))
                {
                    UtcTime = parsedDate.ToUnixTimeMilliseconds();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DateTimeHelperMethods", "ToUnixTimeMilliseconds", ex.Message, ex);
            }
            return UtcTime;
        }

        public static string GetDurationInHoursAndMinutes(this TimeSpan duration)
        {
            var days = duration.Days;
            var hours = duration.Hours;
            var minutes = duration.Minutes;

            string response = days > 0 ? days + ApplicationConstants.Days : string.Empty;
            response = response + (hours > 0 ? hours + ApplicationConstants.Hours : string.Empty);
            response = response + (minutes > 0 ? minutes + ApplicationConstants.Minutes : (duration.Seconds > 0 ? ApplicationConstants.OneMinute : string.Empty));

            return response;
        }

        public static string GetTimeInHhMmFormat(this DateTimeOffset dateTime)
        {
            string response = string.Empty;
            response = dateTime.DateTime.ToString("hh:mm tt");
            return response;
        }

        public static bool IsWeekEnd(this DateTimeOffset dateTime)
        {
            bool response = false;
            response = dateTime.DayOfWeek == DayOfWeek.Saturday || dateTime.DayOfWeek == DayOfWeek.Sunday;
            return response;
        }

        public static string GetTimeInAmPmFormat(this TimeSpan timeSpan)
        {
            string response = string.Empty;
            var date = DateTime.Today.Add(timeSpan);
            response = date.ToString("hh:mm tt");
            return response;
        }

        public static TimeSpan GetOffset(this DateTimeOffset value, string timeZoneName)
        {
            TimeSpan response = TimeSpan.FromSeconds(0);
            try
            {
                if (!string.IsNullOrEmpty(timeZoneName))
                {
                    timeZoneName = timeZoneName.ParseTimeZone();
                    TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneName);
                    response = timeZone.GetUtcOffset(value.UtcDateTime);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DateTimeHelperMethods", "GetOffset", ex.Message, ex);
            }
            return response;
        }

        public static DateTimeOffset FirstDayOfWeek(this DateTimeOffset dateTimeOffset)
        {
            var currentCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
            var differenceInDays = dateTimeOffset.DayOfWeek - currentCulture.DateTimeFormat.FirstDayOfWeek; //TAKING SUNDAY AS WEEK FIRST DAY
            if (differenceInDays < 0)
                differenceInDays += 7;
            return dateTimeOffset.AddDays(-differenceInDays).Date;
        }

        public static DateTimeOffset FirstDayOfMonth(this DateTimeOffset dateTimeOffset)
        {
            return new DateTime(dateTimeOffset.Year, dateTimeOffset.Month, 1);
        }

        public static DateTimeOffset LastDayOfMonth(this DateTimeOffset dateTimeOffset)
        {
            return dateTimeOffset.FirstDayOfMonth().AddMonths(1).AddDays(-1);
        }

        public static bool IsFirstDayOfWeek(this DateTime dateTime)
        {
            return dateTime.DayOfWeek == DayOfWeek.Monday;
        }
        public static bool IsFirstDayOfMonth(this DateTime dateTime)
        {
            return dateTime.Day == 1;
        }
        public static bool IsFirstDayOfYear(this DateTime dateTime)
        {
            return dateTime.DayOfYear == 1;
        }

        public static int DaysInYear(this DateTime dateTime)
        {
            return DateTime.IsLeapYear(dateTime.Year) ? 366 : 365;
        }

        public static string ToShortTimeSafe(this TimeSpan? timeSpan)
        {
            if (timeSpan.HasValue && timeSpan.Value != TimeSpan.Zero)
            {
                return new DateTime().Add(timeSpan.Value).ToString(Resource.constFormat12HourTime2);
            }
            return null;

        }
        public static string isShortTimeSafe(this TimeSpan? timeSpan)
        {
            if (timeSpan.HasValue && timeSpan.Value != TimeSpan.Zero)
            {
                return new DateTime().Add(timeSpan.Value).ToShortTimeString();
            }
            return Resource.lblHyphen;
        } 
    }
}
