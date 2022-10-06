using CsvHelper;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.DataAccess;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace SiteFuel.Exchange.Domain
{
    public class BaseDomain : IDomain, IDisposable
    {
        private SiteFuelUow _siteFuelContext;

        public BaseDomain(SiteFuelUow SiteFuelDbContext)
        {
            _siteFuelContext = SiteFuelDbContext;
        }

        public BaseDomain(string connectionString)
        {
            _siteFuelContext = new SiteFuelUow(connectionString);
        }

        protected BaseDomain(BaseDomain domain)
        {
            _siteFuelContext = domain.Context;
        }

        protected SiteFuelUow Context
        {
            get
            {
                return _siteFuelContext;
            }

            set
            {
                _siteFuelContext = value;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources  
                if (_siteFuelContext != null)
                {
                    _siteFuelContext.Dispose();
                    _siteFuelContext = null;
                }
            }

            // free native resources if there are any, below this line.
        }

        protected void CheckEntityAccess(UserContext userContext, int entityId, EntityType entityType)
        {
            var authorizationDomain = new AuthorizationDomain(this);
            if (!authorizationDomain.CheckAccessOnEntity(userContext, entityType, entityId))
                throw new UnauthorizedAccessException("User do have permission to access this entity");
        }

        protected void GetOffsetForTimezones(List<TimeZoneOffsetModel> input)
        {
            DateTime dateTime = DateTime.Now;
            foreach (var timezone in input)
            {
                if (!string.IsNullOrWhiteSpace(timezone.TimeZoneName))
                {
                    TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(timezone.TimeZoneName);
                    timezone.Offset = tzi.GetUtcOffset(dateTime);
                }
            }
        }

        protected static List<T> ReadCSVFile<T>(Stream absolutePath, bool hasHeaderRecord = false)
        {
            var result = new List<T>();
            using (var reader = new StreamReader(absolutePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.HasHeaderRecord = hasHeaderRecord;
                csv.Configuration.HeaderValidated = null;
                csv.Configuration.MissingFieldFound = null;
                csv.Configuration.IgnoreBlankLines = true;
                csv.Configuration.TrimOptions = CsvHelper.Configuration.TrimOptions.Trim;
                csv.Configuration.ShouldSkipRecord = record => record.All(field => String.IsNullOrWhiteSpace(field));
                try
                {
                    result = csv.GetRecords<T>().ToList();
                }
                catch (Exception ex)
                {
                    result = new List<T>();
                    LogManager.Logger.WriteException("BaseDomain", "ReadCSVFile", "csv file read failed : " + ex.Message, ex);
                }
            }
            return result;
        }
        public string GetCompanyWordInfo(string companyName, string[] companyWordsDetails)
        {
            string supplierName;
            if (companyWordsDetails.Length > 1)
            {
                supplierName = (companyWordsDetails[0].ToCharArray()[0].ToString() + companyWordsDetails[companyWordsDetails.Length - 1].ToCharArray()[0].ToString()).ToUpper();
            }
            else
            {
                supplierName = Regex.Replace(companyName, @"\s", "").Substring(0, 2).ToUpper();
            }

            return supplierName;
        }

        public  string GetUniqueKey()
        {
            int maxLength = 6;
            var bytes = new byte[4];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            uint random = BitConverter.ToUInt32(bytes, 0) % 100000000;
            var results = random.ToString().Length <= maxLength ? random.ToString() : random.ToString().Substring(0, maxLength);
            return results;
        }
    }
}

