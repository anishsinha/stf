using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class LFRecordsGridViewModel
    {

        public int LiftFileRecordId { get; set; } //refers Id of LiftFileValidationRecords column

        public string bol { get; set; }

        public string TerminalName { get; set; } // Represents terminal code as received in Parkland API Json

        public string Terminals { get; set; }

        public decimal correctedQuantity { get; set; }

       // public int validatedQuantity { get; set; }

        public string RecordDate { get; set; }

        public string statusChangeDate { get; set; }

        public int Status { get; set; }
         
        public string TerminalItemCode { get; set; }

        public string ProductType { get; set; }

        public string LoadDate { get; set; }
        public int? InvFtlDetailId { get; set; }

       // public string DisplayTerminalName { get; set; }

        public int? InvId { get; set; }
        public string LiftTicketNumber { get; set; }
        public string Reason { get; set; }

        public string CarrierID { get; set; }

        public string FileName { get; set; }

        public int CallId { get; set; }
        public string recordStatus { get; set; } // used in searchby bol filename grid

        public string Terminal { get; set; } //== mapped terminal/Bulk plant as per terminal mapping.

        public string CarrierName { get; set; }

        public bool IsInvoiceFtlDetailListRequired { get; set; }

        public bool IsRecordPushedToExternalApi { get; set; }

        public decimal NetQuantity { get; set; }

        public decimal GrossQuantity { get; set; }

        public string BadgeNumber { get; set; }
        public bool IsFromScratchReport  { get; set; }
        public int? ProductTypeId { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public LfvParameterViewModel LfvValidationParameters { get; set; } = new LfvParameterViewModel();
        public bool IsAdminUser { get; set; }
        public string CIN { get; set; }
        public string ReasonCode { get; set; }
        public string ReasonCategory { get; set; }
        public string Username { get; set; }
        public string ModifiedDate { get; set; }

        public string LFVCarrierPerModifiedDate { get; set; }
        public string IgnoredReason { get; set; }
        public string ForcedIgnoreReason { get; set; }

        public string LFVResolutionTime { get; set; }
        public string TimeToBol { get; set; }
    }


    //LiftFileRecordsWithMissingTFXDeliveryDetails
    public class SupplierBOLReportViewModel
    {
        public int CallId { get; set; } 

        public string BOL { get; set; }

        public string TerminalCode { get; set; } // Represents terminal code as received in Parkland API Json

        public decimal CorrectedQuanity { get; set; }

        public string TerminalItemCode { get; set; }

        public string LoadDate { get; set; }

        public string RecordStatus { get; set; }

        public string Reason { get; set; }

        public string CarrierID { get; set; }

        public string RecordDate { get; set; }
        public string CarrierName { get; set; }
      
        public string Terminal { get; set; }

        public string FileName { get; set; }
        public int Status { get; set; }

        public string ProductType { get; set; }
    }

    //TFXDeliveryDetailsWithMissingLiftFileRecords
    public class CarrierBOLReportViewModel
    {
        public string BOL { get; set; }

        public string TerminalName { get; set; }

        public string LoadDate { get; set; }
        public decimal NetQuantity { get; set; }
        public decimal GrossQuantity { get; set; }
        public string BadgeNumber { get; set; }
        public string CarrierName { get; set; }
        public string CarrierID { get; set; }
        public string FuelTypeName { get; set; }

    }

    public class AccrualReportGridInputViewModel: DataTableAjaxPostModel
    {
        public DateTimeOffset? FromDate { get; set; }
        public DateTimeOffset? ToDate { get; set; }
        public string ProductTypeIds { get; set; }

    }

    public class LFVValidationStatsViewModel
    {
        public List<DropdownDisplayItem> ProductTypesDDL { get; set; } = new List<DropdownDisplayItem>();
        public int TotalLFVRecords { get; set; }
        public int TotalPushedBackRecords { get; set; }
        public int TotalVolume { get; set; }
        public int TotalPushedBackVolume { get; set; }
    }



}
