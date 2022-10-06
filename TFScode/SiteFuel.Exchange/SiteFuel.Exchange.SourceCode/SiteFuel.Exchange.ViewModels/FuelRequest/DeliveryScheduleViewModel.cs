using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SiteFuel.Exchange.ViewModels
{
    public class DeliveryScheduleViewModel : StatusViewModel, IComparable<DeliveryScheduleViewModel>
    {
        public DeliveryScheduleViewModel()
        {
            InstanceInitialize();
        }

        public DeliveryScheduleViewModel(Status status)
            : base(status)
        {
            InstanceInitialize(status);
        }

        private void InstanceInitialize(Status status = Status.Failed)
        {
            ScheduleType = (int)DeliveryScheduleType.SpecificDates;
            ScheduleDays = new List<int>();
            ScheduleDayNames = new List<string>();
            ScheduleDate = DateTimeOffset.Now;
            StatusId = (int)DeliveryScheduleStatus.New;
            SplitLoadAddresses = new List<SplitLoadAddressViewModel>();
        }

        public int Id { get; set; }

        [RequiredIfMultiple("ScheduleType", (int)DeliveryScheduleType.SpecificDates, (int)DeliveryScheduleType.Monthly, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public DateTimeOffset ScheduleDate { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string ScheduleStartTime { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string ScheduleEndTime { get; set; }

        [Display(Name = nameof(Resource.lblQuantity), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Range(typeof(Decimal), ApplicationConstants.DecimalMinValue, ApplicationConstants.DecimalMaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        public decimal ScheduleQuantity { get; set; }

        [RequiredIfMultiple("ScheduleType", (int)DeliveryScheduleType.Weekly, (int)DeliveryScheduleType.BiWeekly, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public List<int> ScheduleDays { get; set; }

        public List<string> ScheduleDayNames { get; set; }

        public List<DateTimeOffset> AllScheduleDate { get; set; }

        public int ScheduleDay { get; set; }

        public int ScheduleType { get; set; }

        public string StrScheduleDate { get; set; }

        public string ScheduleTypeName { get; set; }

        public int GroupId { get; set; }

        public int QuantityTypeId { get; set; }

        public int CreatedBy { get; set; }

        public Nullable<int> DriverId { get; set; }

        public string DriverName { get; set; }

        public bool IsDeliveryIn24Hrs { get; set; }

        public bool IsFtlOrder { get; set; }

        public bool IsSplitDrop { get; set; }

        public string PONumber { get; set; }

        public int OrderId { get; set; }

        public string PhoneNumber { get; set; }

        public string DeliveryWindow { get; set; }

        public string FuelType { get; set; }

        public string IsDeliverySchedule { get; set; }

        public Nullable<int> RescheduledTrackableId { get; set; }

        public int StatusId { get; set; }

        public bool IsRescheduled { get; set; }

        public string IsDropCompleted { get; set; }

        public string CustomerName { get; set; }

        public string Location { get; set; }

        public decimal CustomerLatitude { get; set; }

        public decimal CustomerLongitude { get; set; }

        public decimal DriverLatitude { get; set; }

        public decimal DriverLongitude { get; set; }

        public string ScheduleStatus { get; set; }

        public string ContactPerson { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public List<SpecialInstructionViewModel> SpecialInstructions { get; set; }

        public CarrierViewModel Carrier { get; set; } = new CarrierViewModel();

        public SupplierSourceViewModel SupplierSource { get; set; } = new SupplierSourceViewModel();

        public string LoadCode { get; set; }

        public UoM UoM { get; set; }

        public string DisplayUoM { get; set; }

        public ScheduleQuantityType ScheduleQuantityType { get; set; } = ScheduleQuantityType.Quantity;
        public string ScheduleQuantityTypeText { get; set; }

        public List<SplitLoadAddressViewModel> SplitLoadAddresses { get; set; }

        public int CompareTo(DeliveryScheduleViewModel other)
        {
            if (other == null)
                return 0; //false

            if (other.Id == this.Id
                && other.ScheduleType == this.ScheduleType
                && other.ScheduleStartTime == this.ScheduleStartTime
                && other.ScheduleEndTime == this.ScheduleEndTime
                && other.ScheduleQuantity == this.ScheduleQuantity)
            {
                if (other.ScheduleType == (int)DeliveryScheduleType.BiWeekly || other.ScheduleType == (int)DeliveryScheduleType.Weekly)
                {
                    if (!this.ScheduleDays.OrderBy(t => t).SequenceEqual(other.ScheduleDays.OrderBy(t => t)))
                        return 0;
                    return 1;
                }
                else if (other.ScheduleDate.DateTime == this.ScheduleDate.DateTime)
                {
                    return 1;
                }
                return 0;
            }
            else
            {
                return 0;
            }
        }
    }

    public class DeliveryScheduleTime
    {
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}
