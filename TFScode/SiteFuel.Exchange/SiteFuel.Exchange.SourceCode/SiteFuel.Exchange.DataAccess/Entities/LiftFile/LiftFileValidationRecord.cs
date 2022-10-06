using SiteFuel.Exchange.Utilities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    public partial class LiftFileValidationRecord
    {
        public int Id { get; set; }
        public int LiftFileId { get; set; }
        public LFVRecordStatus Status { get; set; }
        public DateTimeOffset StatusChangedDate { get; set; }
        public DateTimeOffset AddedDate { get; set; }
        public bool IsActive { get; set; }

        [StringLength(512)]
        public string TerminalCode { get; set; }

        [StringLength(512)]
        public string BOL { get; set; }

        [StringLength(512)]
        public string CIN { get; set; }

        [StringLength(512)]
        public string CarrierID { get; set; }

        [StringLength(512)]
        public string TruckNum { get; set; }

        [StringLength(512)]
        public string CarrierName { get; set; }
        public string CNID { get; set; }

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

        public decimal CorrectedQty { get; set; }
        public decimal Gross { get; set; }
        public decimal Temp { get; set; }
        public decimal Density { get; set; }

        [StringLength(512)]
        public string VendorItemCode { get; set; }

        [StringLength(512)]
        public string Filename { get; set; }

        [StringLength(512)]
        public string POFromAPI { get; set; } //this is not TFX PO

        public int DriverBadge { get; set; }

        [StringLength(512)]
        public string VendorOrginalRef { get; set; }

        [StringLength(512)]
        public string MeterRecords { get; set; }

        [StringLength(512)]
        public string Customer { get; set; }

        public bool IsRecordPushedToExternalApi { get; set; }

        [ForeignKey("LiftFileId")]
        public virtual LiftFileDetail LiftFileDetails { get; set; }

        [StringLength(1024)]
        public string Reason { get; set; }

        public int? InvoiceFtlDetailId { get; set; }

        public int? UpdatedBy { get; set; }

        public int? ReasonCodeId { get; set; }

        public DateTimeOffset? UpdatedDate { get; set; }

        [ForeignKey("ReasonCodeId")]
        public virtual ReasonCodeDetail ReasonCodeDetail { get; set; }
    }
}