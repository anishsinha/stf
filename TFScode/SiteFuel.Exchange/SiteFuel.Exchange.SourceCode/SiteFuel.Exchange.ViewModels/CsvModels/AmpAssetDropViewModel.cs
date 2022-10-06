using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class AmpAssetDropViewModel
    {
        public AmpAssetDropViewModel()
        {
            InstanceInitialize();
        }

        public AmpAssetDropViewModel(string jobString, string dropString)
        {
            InstanceInitialize();
            if (string.IsNullOrWhiteSpace(jobString) || string.IsNullOrWhiteSpace(dropString))
            {
                throw new FormatException("Invalid Data Passed");
            }

            var jobDetails = jobString.Split(' ');
            StartDate = jobDetails[1].ToDateTimeOffset();
            StartTime = StartDate.TimeOfDay;

            var dropDetails = dropString.Split(' ');
            AmpProductType = Regex.Replace(dropDetails[0], @"[\d-]", string.Empty);
            AmpProductType = AmpProductType.Substring(0, 3);

            var indexStart = dropDetails[0].IndexOf(AmpProductType) + AmpProductType.Length;
            var length = dropDetails[0].Length - indexStart;

            AssetName = dropDetails[0].Substring(indexStart, length) .Replace(AmpProductType, string.Empty);
            DropQuantity = decimal.Parse(dropDetails[1]);
            TankCompartment = dropDetails[2];
        }

        private void InstanceInitialize()
        {
            StartDate = DateTimeOffset.Now;
            StartTime = DateTimeOffset.Now.TimeOfDay;
        }

        public DateTimeOffset StartDate { get; set; }

        public TimeSpan StartTime { get; set; }

        public string AssetName { get; set; }

        public string AmpProductType { get; set; }

        public decimal DropQuantity { get; set; }

        public string TankCompartment { get; set; }
    }

    static class DateTimeConverter
    {
        private static readonly string[] Months = new string[] { "jan", "feb", "mar", "apr", "may", "jun", "jul", "aug", "sep", "oct", "nov", "dec" };

        public static DateTimeOffset ToDateTimeOffset(this string input)
        {
            var month = Array.IndexOf(Months, input.Substring(0, 3).ToLower()) + 1;
            var year = Convert.ToInt32(input.Substring(5, 2));
            var day = Convert.ToInt32(input.Substring(3, 2));
            var hour = Convert.ToInt32(input.Substring(7, 2));
            var minute = Convert.ToInt32(input.Substring(10, 2));
            var response = new DateTimeOffset(2000 + year, month, day, hour, minute, 0, DateTimeOffset.Now.Offset);
            return response;
        }
    }
}
