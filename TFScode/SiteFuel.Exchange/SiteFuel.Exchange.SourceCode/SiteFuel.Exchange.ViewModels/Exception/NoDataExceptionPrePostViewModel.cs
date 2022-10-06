using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{

    public class NoDataExceptionPrePostViewModel
    {
        public NoDataExceptionPrePostViewModel()
        {
            InstanceInitialize();
        }
        private void InstanceInitialize()
        {
            AssetDetails = new List<AssetDetailsForApprovalViewModel>();
        }
        public int InvoiceHeaderId { get; set; }

        public int companyId { get; set; }

        public List<AssetDetailsForApprovalViewModel> AssetDetails { get; set; }
    }

    public class AssetDetailsForApprovalViewModel
    {
        public int AssetId { get; set; }

        [Display(Name = nameof(Resource.lblName), ResourceType = typeof(Resource))]
        public string AssetName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblPreDip), ResourceType = typeof(Resource))]
        public decimal? PreDip { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblPostDip), ResourceType = typeof(Resource))]
        public decimal? PostDip { get; set; }

        public int? OrderId { get; set; }
        public int InvoiceId { get; set; }
        public AssetType AssetType { get; set; }
        [Display(Name ="Dropped Quantity")]
        public decimal DroppedGallons { get; set; }

        [Display(Name = nameof(Resource.lblStartTime), ResourceType = typeof(Resource))]
        public string DropStartTime { get; set; }

        [Display(Name = nameof(Resource.lblEndTime), ResourceType = typeof(Resource))]
        public string DropEndTime { get; set; }
        [Display(Name = nameof(Resource.lblPO), ResourceType = typeof(Resource))]
        public string PONumber { get; set; }
        [Display(Name = nameof(Resource.lblDropTicketNumber), ResourceType = typeof(Resource))]
        public string DropTicketNumber { get; set; }

        public UoM UoM { get; set; }
        public string TankMakeModel { get; set; }

        [Display(Name = nameof(Resource.lblUomShort), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public TankScaleMeasurement TankScaleMeasurement { get; set; }
        public int JobXAssetId { get; set; }
    }

    public class NoDataProcessorModel
    {
        public int DdtId { get; set; }
        public int InvoiceHeaderId { get; set; }
        public int OrderId { get; set; }
        public string DisplayInvoiceNumber { get; set; }
        public string BrokeredChainId { get; set; }
        public int AcceptedBy { get; set; }
        public int AcceptedCompanyId { get; set; }
        public int WaitingFor { get; set; }
        public NoDataExceptionApproval NoDataExceptionApproval { get; set; }
    }

    public class WaitingForDipDataBrokeredInvoicesProcessorModel
    {
        public int AcceptedBy { get; set; }
        public NoDataExceptionPrePostViewModel NoDipDataExceptionModel { get; set; }

    }
}
