using RestSharp;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using Velyo.Google.Services;
using Velyo.Google.Services.Models;

namespace SiteFuel.Exchange.Domain
{
    public static class GoogleApiDomain
    {
        private static int cacheTimePeriodInSecs = 14400; //4 hours

        private static Object _obj = new object();
        public static Geocode GetGeocode(string address)
        {
            Geocode geocode = null;
            GeocodingResponse response;

            geocode = CacheManager.Get<Geocode>($"GeoAddress_{address}");
            if (geocode == null)
            {
                address = AddCountryCodeToZipcode(address);

                var context = MapsApiContext.Default;
                var request = new GeocodingRequest(address, context);
                lock (_obj)
                {
                    response = request.GetResponse();
                }
                if (response.Results.Count > 0)
                {
                    GeoAddress addressResponse = null;
                    addressResponse = ParseAddress(response);
                    var zipResponse = response.Results.Where(t => t.Types.Any(t1 => t1.Equals("postal_codes"))).FirstOrDefault();
                    LatLng location = response.Results[0].Geometry.Location;
                    if(zipResponse != null)
                    {
                        location = zipResponse.Geometry.Location;
                    }
                    geocode = new Geocode();
                    geocode.Latitude = location.Latitude;
                    geocode.Longitude = location.Longitude;
                    geocode.Address = addressResponse.Address;
                    geocode.CountyName = Convert.ToString(addressResponse.CountyName);
                    geocode.StateCode = Convert.ToString(addressResponse.StateCode);
                    geocode.StateName = Convert.ToString(addressResponse.StateName);
                    geocode.City = Convert.ToString(addressResponse.City);
                    geocode.CountryCode = Convert.ToString(addressResponse.CountryCode);
                    geocode.CountryName = Convert.ToString(addressResponse.CountryName);
                    geocode.ZipCode = Convert.ToString(addressResponse.ZipCode);
                    geocode.CountryGroupCode = addressResponse.CountryGroupCode;
                    CacheManager.Set($"GeoAddress_{address}", geocode, cacheTimePeriodInSecs);
                }
            }

            return geocode;
        }

        public static GeoAddress GetAddress(double latitude, double longitude)
        {
            GeoAddress address = null;
            GeocodingResponse response;
            address = CacheManager.Get<GeoAddress>($"GeoLatAddress_{latitude}{longitude}");

            if (address != null)
                return address;
            var context = MapsApiContext.Default;
            var request = new GeocodingRequest(latitude, longitude, context);
            lock (_obj)
            {
                response = request.GetResponse();
            }
            if (response.Results.Count > 0)
            {
                address = ParseAddress(response);
                CacheManager.Set($"GeoLatAddress_{latitude}{longitude}", address, cacheTimePeriodInSecs);
            }
            return address;
        }

        public static string GetTimeZone(decimal latitude, decimal longitude)
        {
            RestClient client;
            RestRequest request;
            string timezone = string.Empty;
            DateTime date = DateTime.Now.Date;
            timezone = CacheManager.Get<string>($"GeoTimeZone_{latitude}{longitude}");
            if (!string.IsNullOrEmpty(timezone))
                return timezone;

            try
            {
                client = new RestClient("https://maps.googleapis.com");
                request = new RestRequest("maps/api/timezone/json", Method.GET);
                DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                TimeSpan diff = date - origin;

                request.AddParameter("location", latitude + "," + longitude);
                request.AddParameter("timestamp", diff.TotalSeconds);
                request.AddParameter("sensor", "false");
                request.AddParameter("key", SiteFuel.Exchange.Core.Utilities.AppSettings.GoogleApiKey);
                var response = client.Execute<GoogleTimeZone>(request);
                if (response.Data.Status == "OK")
                {
                    timezone = response.Data.TimeZoneName.ParseTimeZone();
                }
                CacheManager.Set($"GeoTimeZone_{latitude}{longitude}", timezone, cacheTimePeriodInSecs);

            }
            catch (Exception ex)
            {
                timezone = string.Empty;
                Logger.LogManager.Logger.WriteException("GoogleApiDomain", "GetTimeZone", ex.Message, ex);
            }

            return timezone;
        }

        public static string GetTimeZoneId(decimal latitude, decimal longitude)
        {
            RestClient client;
            RestRequest request;
            string timezoneId = string.Empty;
            DateTime date = DateTime.Now.Date;
            timezoneId = CacheManager.Get<string>($"GeoTimeZoneId_{latitude}{longitude}");
            if (!string.IsNullOrEmpty(timezoneId))
                return timezoneId;

            try
            {
                client = new RestClient("https://maps.googleapis.com");
                request = new RestRequest("maps/api/timezone/json", Method.GET);
                DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                TimeSpan diff = date - origin;

                request.AddParameter("location", latitude + "," + longitude);
                request.AddParameter("timestamp", diff.TotalSeconds);
                request.AddParameter("sensor", "false");
                request.AddParameter("key", SiteFuel.Exchange.Core.Utilities.AppSettings.GoogleApiKey);
                var response = client.Execute<GoogleTimeZone>(request);
                if (response.Data.Status == "OK")
                {
                    timezoneId = response.Data.TimeZoneId;
                }
                CacheManager.Set($"GetTimeZoneId_{latitude}{longitude}", timezoneId, cacheTimePeriodInSecs);

            }
            catch (Exception ex)
            {
                timezoneId = string.Empty;
                Logger.LogManager.Logger.WriteException("GoogleApiDomain", "GetTimeZoneId", ex.Message, ex);
            }

            return timezoneId;
        }

        private static GeoAddress ParseAddress(GeocodingResponse geoResponse)
        {
            var geoAddress = new GeoAddress();
            try
            {
                var result = geoResponse.Results.FirstOrDefault(t =>
                                                   t.AddressComponents.Any(t1 => t1.Types.Any(t2 => t2.Equals("street_number"))) &&
                                                   t.AddressComponents.Any(t1 => t1.Types.Any(t2 => t2.Equals("postal_code")))
                                                   && t.AddressComponents.Any(t1 => t1.Types.Any(t2 => t2.Equals("administrative_area_level_2"))));
                if (result == null)
                {
                    result = geoResponse.Results.FirstOrDefault(t =>
                                                t.AddressComponents.Any(t1 => t1.Types.Any(t2 => t2.Equals("postal_code")))
                                                && t.AddressComponents.Any(t1 => t1.Types.Any(t2 => t2.Equals("administrative_area_level_2"))));
                    if (result == null)
                        result = geoResponse.Results[0];

                }
                geoAddress.FormattedAddress = result.FormattedAddress;
                var addressComponents = result.AddressComponents;

                foreach (var addressComponent in addressComponents)
                {
                    var type = addressComponent.Types.Length > 0 ? addressComponent.Types[0] : "street_number";
                    switch (type)
                    {
                        case "street_number":
                            geoAddress.Address = addressComponent.ShortName;
                            break;

                        case "route":
                        case "political":
                            geoAddress.Address += " " + addressComponent.ShortName;
                            break;

                        case "locality":
                            geoAddress.City = addressComponent.ShortName;
                            break;

                        case "administrative_area_level_1":
                            geoAddress.StateCode = addressComponent.ShortName;
                            geoAddress.StateName = addressComponent.LongName;
                            break;
                        //level_2 is for County name
                        case "administrative_area_level_2":
                            geoAddress.CountyName = addressComponent.ShortName;
                            break;

                        case "country":
                            geoAddress.CountryCode = (addressComponent.ShortName == "CA") ? Country.CAN.GetDisplayName() : ((addressComponent.ShortName == "US") ? Country.USA.GetDisplayName() :  addressComponent.ShortName);
                            geoAddress.CountryName = addressComponent.LongName;
                            break;

                        case "postal_code":
                            geoAddress.ZipCode = addressComponent.ShortName;
                            break;
                    }
                }
                if (!string.IsNullOrEmpty(geoAddress.CountryCode) && !Enum.GetNames(typeof(Country)).Contains(geoAddress.CountryCode))
                {
                    geoAddress.CountryGroupCode = geoAddress.CountryCode;
                    geoAddress.CountryCode = Country.CAR.GetDisplayName();
                }
                if (geoAddress.CountyName == null)
                {
                    var countyAddressResult = geoResponse.Results.FirstOrDefault(t => t.AddressComponents.Any(t1 => t1.Types.Any(t2 => t2.Equals("administrative_area_level_2"))));
                    if (countyAddressResult != null) { geoAddress.CountyName = countyAddressResult.AddressComponents.FirstOrDefault(t1 => t1.Types.Any(t2 => t2.Equals("administrative_area_level_2")))?.ShortName; }
                }
                else if (geoAddress.Address == null)
                {
                    var addressResult = geoResponse.Results.FirstOrDefault(t => t.AddressComponents.Any(t1 => t1.Types.Any(t2 => t2.Equals("street_number"))));
                    if (addressResult != null) { geoAddress.Address = addressResult.AddressComponents.FirstOrDefault(t1 => t1.Types.Any(t2 => t2.Equals("street_number")))?.ShortName; }
                }
                if (geoAddress.City == null)
                {
                    var addressResult = geoResponse.Results.FirstOrDefault(t => t.AddressComponents.Any(t1 => t1.Types.Any(t2 => t2.Equals("locality"))));
                    if (addressResult != null) { geoAddress.City = addressResult.AddressComponents.FirstOrDefault(t1 => t1.Types.Any(t2 => t2.Equals("locality")))?.ShortName; }
                }
                if (geoAddress.City == null)
                {
                    var addressResult = geoResponse.Results.FirstOrDefault(t => t.AddressComponents.Any(t1 => t1.Types.Any(t2 => t2.Equals("sublocality"))));
                    if (addressResult != null) { geoAddress.City = addressResult.AddressComponents.FirstOrDefault(t1 => t1.Types.Any(t2 => t2.Equals("sublocality")))?.ShortName; }
                }
                if (geoAddress.City == null)
                {
                    var addressResult = geoResponse.Results.FirstOrDefault(t => t.AddressComponents.Any(t1 => t1.Types.Any(t2 => t2.Equals("administrative_area_level_3"))));
                    if (addressResult != null) { geoAddress.City = addressResult.AddressComponents.FirstOrDefault(t1 => t1.Types.Any(t2 => t2.Equals("administrative_area_level_3")))?.ShortName; }
                }
            }
            catch
            {
                throw;
            }
            return geoAddress;
        }

        public static string AddCountryCodeToZipcode(string address)
        {
            Regex rxZipcodeUSA = new Regex(ApplicationConstants.ZipValidationUSA, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Regex rxZipcodeCAN = new Regex(ApplicationConstants.ZipValidationCAN, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (rxZipcodeUSA.IsMatch(address))
            {
                address = $"{address} {Constants.CountryUSA}";
            }
            else if (rxZipcodeCAN.IsMatch(address))
            {
                address = $"{address} {Constants.CountryCAN}";
            }
            return address;
        }

        public static GeoAddress GetAddress(string address)
        {
            GeoAddress _address = null;
            GeocodingResponse response;

            var context = MapsApiContext.Default;
            var request = new GeocodingRequest(address, context);
            lock (_obj)
            {
                response = request.GetResponse();
            }
            if (response.Results.Count > 0)
            {
                _address = ParseAddress(response);
            }
            return _address;
        }
    }

    public class GoogleTimeZone
    {
        public string Status { get; set; }
        public string TimeZoneId { get; set; }
        public string TimeZoneName { get; set; }
    }

    public class Geocode
    {
        public string Address { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string CountyName { get; set; }

        public string StateCode { get; set; }

        public string StateName { get; set; }

        public string City { get; set; }

        public string CountryCode { get; set; }
        public string CountryGroupCode { get; set; }

        public string CountryName { get; set; }

        public string ZipCode { get; set; }
    }

    public class GeoAddress
    {
        public string Address { get; set; }

        public string City { get; set; }

        public string StateCode { get; set; }

        public string StateName { get; set; }

        public string CountryCode { get; set; }
        public string CountryGroupCode { get; set; }

        public string CountryName { get; set; }

        public string ZipCode { get; set; }

        public string CountyName { get; set; }

        public string FormattedAddress { get; set; }
    }
}
