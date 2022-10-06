using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class PDITaxFTPViewModel
    {
        public string OrderNumber { get; set; }
        public string CustomerDescription { get; set; }
        public string TaxDescription { get; set; }
        public string TaxType { get; set; }
        public string TaxMethod { get; set; }
        public string TaxRate { get; set; }
        public string BasedOnUnits { get; set; }
        public string TaxBasis { get; set; }
        public string TaxAmount { get; set; }
        public string TaxExceptionDescription { get; set; }
        public string TaxExceptionOverride { get; set; }
        public string TaxCertificateNo { get; set; }
        public string PDIInvoiceNo { get; set; }
    }

    public class PDITaxFTPViewModelClassMap : ClassMap<PDITaxFTPViewModel>
    {
        public PDITaxFTPViewModelClassMap()
        {
            Map(m => m.OrderNumber).Name("Order Number");
            Map(m => m.CustomerDescription).Name("Customer Description");
            Map(m => m.TaxDescription).Name("Tax Description");
            Map(m => m.TaxType).Name("Tax Type");
            Map(m => m.TaxMethod).Name("Tax Method");
            Map(m => m.TaxRate).Name("Tax Rate");
            Map(m => m.BasedOnUnits).Name("Based On Units");
            Map(m => m.TaxBasis).Name("Tax Basis");
            Map(m => m.TaxAmount).Name("Tax Amount");
            Map(m => m.TaxExceptionDescription).Name("Tax Exception Description");
            Map(m => m.TaxExceptionOverride).Name("Tax Exception Override");
            Map(m => m.TaxCertificateNo).Name("Tax Certificate No");
        }
    }

    public class FTPConfig
    {
        public string Host { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string RemoteDirectory { get; set; }
    }

}
