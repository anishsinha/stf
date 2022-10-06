using FileHelpers;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    [DelimitedRecord(",")]
    [IgnoreEmptyLines]
    public class JobsBulkUploadCsvViewModel
    {
        [FieldQuoted][FieldOptional]
        public string Name { get; set; }

        [FieldQuoted][FieldOptional]
        public string JobID { get; set; }

        [FieldQuoted][FieldOptional]
        public string StartDate { get; set; }

        [FieldQuoted][FieldOptional]
        public string EndDate { get; set; }

        [FieldQuoted][FieldOptional]
        public string Address { get; set; }

        [FieldQuoted][FieldOptional]
        public string ZipCode { get; set; }

        [FieldQuoted][FieldOptional]
        public string City { get; set; }

        [FieldQuoted][FieldOptional]
        public string State { get; set; }

        [FieldQuoted][FieldOptional]
        public string CountyName { get; set; }

        [FieldQuoted][FieldOptional]
        public string Country { get; set; }

        [FieldQuoted][FieldOptional]
        public string Currency { get; set; }

        [FieldQuoted][FieldOptional]
        public string UoM { get; set; }

        [FieldQuoted][FieldOptional]
        public string IsGeocodeUsed { get; set; }

        [FieldQuoted][FieldOptional]
        public string Latitude { get; set; }

        [FieldQuoted][FieldOptional]
        public string Longitude { get; set; }

        [FieldQuoted][FieldOptional]
        public string TimeZoneName { get; set; }

        [FieldQuoted][FieldOptional]
        public string InventoryDataCaptureType { get; set; }

        [FieldQuoted][FieldOptional]
        public string IsProFormaPoEnabled { get; set; }

        [FieldQuoted][FieldOptional]
        public string IsRetailJob { get; set; }

        [FieldQuoted][FieldOptional]
        public string IsAutoCreateDREnable { get; set; }

        [FieldQuoted][FieldOptional]
        public string IsTaxExempted { get; set; }

        [FieldQuoted][FieldOptional]
        public string IsAssetTracked { get; set; }

    }
}
