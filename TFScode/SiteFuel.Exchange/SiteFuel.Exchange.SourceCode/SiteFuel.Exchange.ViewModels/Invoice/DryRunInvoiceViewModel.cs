using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class DryRunInvoiceViewModel : BaseViewModel
    {
        public DryRunInvoiceViewModel()
        {
            InstanceInitialize();
        }

        public DryRunInvoiceViewModel(Status status) : base(status)
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            DryRunDate = DateTimeOffset.Now.ToString(Resource.constFormatDate);
            DivertedOrderIds = new List<int>();
        }
        public int UserId { get; set; }

        public int OrderId { get; set; }

        public string PoNumber { get; set; }

        [Display(Name = nameof(Resource.lblInvoiceNumber), ResourceType = typeof(Resource))]
        public string SupplierInvoiceNumber { get; set; }

        public int InvoiceId { get; set; }

        public int QuantityTypeId { get; set; }

        public int? TrackableScheduleId { get; set; }

        public int? DriverId { get; set; }

        public int InvoiceNumberId { get; set; }

        [Display(Name = nameof(Resource.lblFuelRemaining), ResourceType = typeof(Resource))]
        public decimal FuelRemaining { get; set; }

        [Display(Name = nameof(Resource.lblDeliveryDate), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string DryRunDate { get; set; }

        [Display(Name = nameof(Resource.lblDeliveryTime), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string DeliveryTime { get; set; }

        [Display(Name = nameof(Resource.lblDryRunFee), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Range(typeof(decimal),ApplicationConstants.DecimalMinValue,ApplicationConstants.DecimalMaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        public decimal DryRunFee { get; set; }

        public DateTimeOffset MinDropDate { get; set; }

        public DateTimeOffset? MaxDropDate { get; set; }

        public List<int> DivertedOrderIds { get; set; }

        public Currency Currency { get; set; }

        public UoM UoM { get; set; }
        public CreationMethod CreationMethod { get; set; } = CreationMethod.SFX;
        public OrderEnforcement OrderEnforcement { get; set; }
    }
}
