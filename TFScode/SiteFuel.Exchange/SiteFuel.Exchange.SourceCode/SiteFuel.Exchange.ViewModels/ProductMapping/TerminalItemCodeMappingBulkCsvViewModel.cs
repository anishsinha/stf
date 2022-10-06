using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using FileHelpers;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace SiteFuel.Exchange.ViewModels
{
    public class TerminalItemCodeMappingBulkCsvViewModel
    {
        [Name("Terminal Supplier")]
        public string TerminalSupplier { get; set; }

        [Name("Terminal Item Description")]
        public string TerminalItemDescription { get; set; }

        [Name("TFX Product Type")]
        [Optional]
        public string TFXProductType { get; set; }

        [Name("Terminal Item Code")]
        public string TerminalItemCode { get; set; }

        [Name("Effective Date")]
        public string EffectiveDate { get; set; }

        [Name("Expired Date")]
        [Optional]
        public string ExpiryDate { get; set; }

        [Optional]
        public int TerminalSupplierId { get; set; }

        [Optional]
        public int ItemDescriptionId { get; set; }

        [Optional]
        public int ProductTypeId { get; set; }

        [Optional]
        public int CompanyId { get; set; }

        [Optional]
        public int RowNumber { get; set; }
    }

    public class TerminalItemCodeMappingBulkCsvViewModelMap : ClassMap<TerminalItemCodeMappingBulkCsvViewModel>
    {
        public TerminalItemCodeMappingBulkCsvViewModelMap()
        {
            Map(m => m.TerminalSupplier).Name("Terminal Supplier");
            Map(m => m.TerminalItemDescription).Name("Terminal Item Description");
            //Map(m => m.TFXProductType).Name("TFX Product Type");
            Map(m => m.TerminalItemCode).Name("Terminal Item Code");
            Map(m => m.EffectiveDate).Name("Effective Date");
            Map(m => m.ExpiryDate).Name("Expired Date").Optional();
        }
    }
}
