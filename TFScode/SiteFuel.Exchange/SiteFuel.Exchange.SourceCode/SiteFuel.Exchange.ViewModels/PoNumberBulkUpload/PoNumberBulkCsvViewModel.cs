using FileHelpers;

namespace SiteFuel.Exchange.ViewModels
{
    [DelimitedRecord(",")]
    [IgnoreEmptyLines]
    public class PoNumberBulkCsvViewModel
    {
        [FieldQuoted] [FieldOptional]
        public string ProjectName { get; set; }

        [FieldQuoted] [FieldOptional]
        public string InvoiceNumber { get; set; }

        [FieldQuoted] [FieldOptional]
        public string InvoiceDate { get; set; }

        [FieldQuoted] [FieldOptional]
        public string Supplier { get; set; }

        [FieldQuoted] [FieldOptional]
        public string NewPoNumber { get; set; }

        [FieldQuoted] [FieldOptional]
        public string Project { get; set; }

        [FieldQuoted] [FieldOptional]
        public string Address { get; set; }

        [FieldQuoted] [FieldOptional]
        public string Amount { get; set; }
    }
}
