using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class AssignToOrderPreviewViewModel : BaseViewModel
    {
        public AssignToOrderPreviewViewModel()
        {
            InstanceInitialize();
        }

        public AssignToOrderPreviewViewModel(Status status) : base(status)
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            AssignToOrderGrid = new AssignToOrderGridViewModel();
            FuelRequestFee = new FuelRequestFeeViewModel();
            DeliverySchedules = new List<DeliveryScheduleViewModel>();
        }
        public int OrderId { get; set; }
        public int InvoiceId { get; set; }
        public AssignToOrderGridViewModel AssignToOrderGrid { get; set; }
        public int OrderTypeId { get; set; }
        public int DeliveryTypeId { get; set; }
        public int QuantityTypeId { get; set; }
        public decimal GallonsOrdered { get; set; }
        public decimal GallonsDropped { get; set; }
        public decimal GallonsRemaining { get; set; }
        public string DeliveryPercentage { get; set; }
        public string StartDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int Assets { get; set; }
        public string AssignedDriver { get; set; }
        public FuelRequestFeeViewModel FuelRequestFee { get; set; }
        public List<DeliveryScheduleViewModel> DeliverySchedules { get; set; }

        public string DriverCustomerName { get; set; }
        public decimal DriverDroppedGallons { get; set; }
        public DateTimeOffset DropStartDate { get; set; }
        public DateTimeOffset DropEndDate { get; set; }
        public int AssetFilled { get; set; }
        public string DriverName { get; set; }
        public bool IsWetHoseFee { get; set; }
        public bool IsOverWaterFee { get; set; }
        public string DriverDropAddress { get; set; }
        public string DriverDropCity { get; set; }
        public string DriverDropState { get; set; }
        public UoM OrderUoM { get; set; }
        public UoM InvoiceUoM { get; set; }
        public Currency Currency { get; set; }
    }
}
