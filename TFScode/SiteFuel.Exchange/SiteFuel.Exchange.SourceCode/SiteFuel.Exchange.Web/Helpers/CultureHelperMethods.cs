using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SiteFuel.Exchange.Web.Helpers
{
    public static class CultureHelperMethods
    {
        //Try using database instead hard-coding - Shrinivas
        // Include ONLY cultures you are implementing, first culture is the DEFAULT
        private static readonly IList<string> _cultures = new List<string>
        {
            "ar",
            "en",
            "es",
            "fr",
            "ja",
            "mr",
            "te",
        };

        /// <summary>
        /// Returns a valid culture name based on "name" parameter. 
        /// If "name" is not valid, it returns the default culture "en-US"
        /// </summary>
        /// <param name="name">Culture's name (e.g. en-US)</param>
        public static string GetImplementedCulture(string name)
        {
            // Make sure it's not null
            if (string.IsNullOrEmpty(name))
            {
                return GetDefaultCulture();
            }

            // If it is implemented or from allowed cultures, accept it
            if (_cultures.Where(c => c.Equals(name, StringComparison.InvariantCultureIgnoreCase)).Count() > 0)
            {
                return name;
            }

            // Find a close match. For example, if you have "en-US" defined and the user requests "en-GB", 
            // the function will return closes match that is "en-US" because at least the language is the same (ie English)  
            var n = GetNeutralCulture(name);
            foreach (var c in _cultures)
            {
                if (c.StartsWith(n))
                {
                    return c;
                }
            }

            // return Default culture as no match found
            return GetDefaultCulture(); 
        }


        /// <summary>
        /// Returns default culture name which is the first name decalared (e.g. en-US)
        /// </summary>
        /// <returns></returns>
        public static string GetDefaultCulture()
        {
            // return Default culture
            return _cultures[0];
        }

        public static string GetCurrentCulture()
        {
            return Thread.CurrentThread.CurrentCulture.Name;
        }

        public static string GetCurrentNeutralCulture()
        {
            return GetNeutralCulture(Thread.CurrentThread.CurrentCulture.Name);
        }

        public static bool IsRighToLeft()
        {
            return Thread.CurrentThread.CurrentCulture.TextInfo.IsRightToLeft;

        }

        public static string GetNeutralCulture(string name)
        {
            if (!name.Contains("-"))
            {
                return name;
            }
            return name.Split('-')[0]; // Read first part only. E.g. "en", "es"
        }
    }
}