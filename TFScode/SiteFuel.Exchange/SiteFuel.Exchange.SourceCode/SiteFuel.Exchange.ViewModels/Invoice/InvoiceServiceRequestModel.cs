using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class InvoiceServiceRequestModel
    {
        public UserContext UserContext { get; set; }
    }

    public class SaveDiscountRequestModel : InvoiceServiceRequestModel
    {
        public DiscountViewModel ViewModel { get; set; }
    }

    public class InvoiceGridRequestModel : InvoiceServiceRequestModel
    {
        public InvoiceDataTableViewModel ViewModel { get; set; }
        public int InvoiceType { get; set; }
        public ViewInvoices View { get; set; }
        public int CompanyId { get; set; }
    }

    public class BolGridRequestModel : InvoiceServiceRequestModel
    {
        public InvoiceDataTableViewModel RequestModel { get; set; }
        public ViewInvoices View { get; set; }
    }

    public class EditInvoicePoNumberReqModel : InvoiceServiceRequestModel
    {
        public int InvoiceId { get; set; }
        public string PoNumber { get; set; }

    }

    public class GenerateDtnRequestModel
    {
        public int InvoiceHeaderId { get; set; }
        public string RefId { get; set; }
        public string Password { get; set; }
        public string SiteNumber { get; set; }
    }

    public class GetTerminalPriceModel
    {
        public int OrderId { get; set; }
        public int TerminalId { get; set; }
        public DateTimeOffset DeliveryDate { get; set; }
    }

    public class GetTerminalsModel
    {
        public int OrderId { get; set; }
        public string Terminal { get; set; }
    }

    public class GetBulkPlantsModel
    {
        public int OrderId { get; set; }
        public string BulkPlant { get; set; }
    }

    public class UpdateBDNInvoiceDetailModel : InvoiceServiceRequestModel
    {
        public List<InvoiceBolEditGrid> Model { get; set; }
    }

    public class CreateInvoiceViewModel : InvoiceServiceRequestModel
    {
        public ManualInvoiceViewModel ViewModel { get; set; }
    }

    public class InvoicePdfRequestModel:InvoiceServiceRequestModel
    {
        public int InvoiceHeaderId { get; set; }
        public CompanyType CompanyType { get; set; }
    }

    public class ConvertToInvoiceRequest : InvoiceServiceRequestModel
    {
        public int InvoiceId { get; set; }
        public bool IsConvertToInv { get; set; }
    }

    public class DeclineInvoiceModel : InvoiceServiceRequestModel
    {
        public DeclineInvoiceViewModel Model { get; set; }
    }

    public class BulkUploadRequestModel : InvoiceServiceRequestModel
    {
        public string CsvText { get; set; }
        public string CsvFilePath { get; set; }
        public CompanyType CompanyType { get; set; }
    }

    public class CreateConsolidateInvoiceModel : InvoiceServiceRequestModel
    {
        public InvoiceViewModelNew ViewModel { get; set; }
    }
}
