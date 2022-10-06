using Newtonsoft.Json;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.MobileAPI.TPD;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class LiftFileViewModel
    {
        public int Id { get; set; }
        public string LFID { get; set; }
        public int AddedByUserId { get; set; }
        public int CompanyId { get; set; }
        public string ExternalRefId { get; set; }
        public DateTimeOffset AddedDate { get; set; }
        public bool IsActive { get; set; }
        public List<LiftFileRecordsViewModel> LiftFileRecords { get; set; }
    }

    public class LiftFileRecordsViewModel : LiftFileItemsViewModel
    {
        public int Id { get; set; }
        public int LiftFileId { get; set; }
        public LFVRecordStatus Status { get; set; }
        public DateTimeOffset StatusChangedDate { get; set; }
        public DateTimeOffset AddedDate { get; set; }
        public bool IsActive { get; set; }        
    }

    public class LiftFileValidateRequest
    {
        public int Count { get; set; }
        public decimal Liters { get; set; }
        public List<LiftFileItemsViewModel> Data { get; set; } = new List<LiftFileItemsViewModel>();
    }

    public class LiftFileDataViewModel
    {
        public List<LiftFileItemsViewModel> Items { get; set; } = new List<LiftFileItemsViewModel>();
    }
    public class LiftFileItemsViewModel
    {
        [StringLength(512)]
        public string Terminal_Code { get; set; }

        [StringLength(512)]
        public string BOL { get; set; }

        [StringLength(512)]
        public string CIN { get; set; } //this is badge number

        [StringLength(512)]
        public string CarrierID { get; set; }

        [StringLength(512)]
        public string TruckNum { get; set; }

        [StringLength(512)]
        public string CarrierName { get; set; }
        public string CNID { get; set; } //BOL container ID

        [StringLength(8)]
        public string LoadDate { get; set; }

        [StringLength(8)]
        public string EndTime { get; set; }

        [StringLength(8)]
        public string InTime { get; set; }

        [StringLength(8)]
        public string StartTime { get; set; }

        [StringLength(8)]
        public string StopTime { get; set; }

        [StringLength(512)]
        public string TermItemCode { get; set; }

        public decimal CorrectedQty { get; set; } = 0;
        public decimal Gross { get; set; } = 0;
        public decimal Temp { get; set; } = 0;
        public decimal Density { get; set; } = 0;

        [StringLength(512)]
        public string VendorItemCode { get; set; }

        [StringLength(512)]
        public string Filename { get; set; }

        [StringLength(512)]
        public string PO { get; set; } //this is not TFX PO

        public int Driver_Badge { get; set; } // driver badge

        [StringLength(512)]
        public string Vendor_Orginal_Ref { get; set; }

        [StringLength(512)]
        public string MeterRecords { get; set; }
        public string Customer { get; set; }

    }

    public class LiftFileResponseDetails : LiftFileItemsViewModel
    {
       // public string Customer { get; set; }
        public string RecordStatus { get; set; } //lfvrecord status

        [JsonIgnore]
        public int InvoiceFtlDetailId { get; set; }

        [JsonIgnore]
        public int LiftFileRecordId { get; set; }
    }

    public class JDEApiResponse
    {
        public ApiResponseViewModel Response { get; set; } = new ApiResponseViewModel();
        public List<LiftFileResponseDetails> Items { get; set; } = new List<LiftFileResponseDetails>();
    }

    public class BolDataForLFV
    {
        public int InvoiceId { get; set; }
        public decimal DroppedGallons { get; set; }
        public string BolNumber { get; set; }
        public decimal? NetQuantity { get; set; }
        public decimal? GrossQuantity { get; set; }
        public string Carrier { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string TerminalName { get; set; }
        public string AssignedTerminalId { get; set; }
        public string CarrierAssignedCustomerId { get; set; }
        public string BuyerCompanyName { get; set; }
        public string FuelTypeName { get; set; }
        public int InvoiceFtlDetailId { get; set; }
        public DateTimeOffset DropEndDate { get; set; }
        public int InvoiceTypeId { get; set; }
        public int WaitingFor { get; set; }
        public int ProductTypeId { get; set; }
        public string AccouningCompanyId { get; set; }
        public int SupplierPreferredInvoiceTypeId { get; set; }
        public string ChangedDisplayName { get; set; }
        public int? ChangedProductTypeId { get; set; }
        public string BulkPlantAssignedId { get; set; }
    }

    public class LiftFileStatusResponseViewModel
    {
        public LiftFileStatusResponseViewModel()
        {
            InstanceInitialize();
        }
        private void InstanceInitialize()
        {
            data = new List<LiftFileResponseDetails>();
        }
        public int count { get; set; }
        public decimal liters { get; set; }

        public List<LiftFileResponseDetails> data { get; set; }
    }
}
