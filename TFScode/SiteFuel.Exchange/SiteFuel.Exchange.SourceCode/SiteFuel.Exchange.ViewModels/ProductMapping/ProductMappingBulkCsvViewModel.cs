using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using FileHelpers;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace SiteFuel.Exchange.ViewModels
{
    public class ProductMappingBulkCsvViewModel
    {
        [Name("Country Code")]
        public string CountryCode { get; set; }

        [Name("State Code")]
        public string StateCode { get; set; }

        [Name("City")]
        // [Default("California")] - Can give default name like this
        public string City { get; set; }

        [Name("Terminal Name")]
        public string TerminalName { get; set; }

        [Name("Product Name")]
        public string ProductName { get; set; }

        [Name("My Product ID")]
        [Optional]
        public string MyProductId { get; set; }

        [Name("Back Office Product ID")]
        [Optional]
        public string BackOfficeProductId  { get; set; }

        [Name("Driver Product ID")]
        [Optional]
        public string DriverProductId { get; set; }

        //[Name("Terminal Item Code")]
        //[Optional]
        //public string TerminalItemCode { get; set; }

        [Optional]
        public int StateId { get; set; }

        [Optional]
        public int CityId { get; set; }

        [Optional]
        public int? TerminalId { get; set; }

        [Optional]
        public int FuelTypeId { get; set; }

        [Optional]
        public int CompanyId { get; set; }

        [Optional]
        public int RowNumber { get; set; }

        
    }

    public class ProductMappingBulkCsvViewModelMap : ClassMap<ProductMappingBulkCsvViewModel>
    {
        public ProductMappingBulkCsvViewModelMap()
        {
            Map(m => m.CountryCode).Name("Country Code");
            Map(m => m.StateCode).Name("State Code");
            Map(m => m.City).Name("City");
            Map(m => m.TerminalName).Name("Terminal Name");
            Map(m => m.ProductName).Name("Product Name");
            Map(m => m.MyProductId).Name("My Product ID").Optional();
            Map(m => m.BackOfficeProductId).Name("Back Office Product ID").Optional();
            Map(m => m.DriverProductId).Name("Driver Product ID").Optional();
            //Map(m => m.TerminalItemCode).Name("Terminal Item Code").Optional();
        }
    }
}
