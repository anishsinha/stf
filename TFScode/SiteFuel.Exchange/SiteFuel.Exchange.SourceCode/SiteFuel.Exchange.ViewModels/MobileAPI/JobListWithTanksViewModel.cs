using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class JobListWithTanksViewModel
    {
        public int JobId { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }

        public string Country { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public double Distance { get; set; }

        public List<ApiTankDetailViewModel> TankList { get; set; }
    }

    public class JobListWithProductTypes
    {
        public int JobId { get; set; }

        public string JobName { get; set; }

        public double Distance { get; set; }

        public int UoM { get; set; }
        public List<JobProductTypes> JobProductTypes { get; set; } = new List<JobProductTypes>();
    }

    public class JobProductTypes
    {
        public int ProductTypeId { get; set; }

        public string ProductTypeName { get; set; }

        public List<DropdownDisplayItem> Suppliers { get; set; } = new List<DropdownDisplayItem>();

        public List<RecurringDeliveryRequest> RecurringSchedules { get; set; } = new List<RecurringDeliveryRequest>();
    }

    public class RecurringDeliveryRequest
    {
        public string Id { get; set; }

        public int ScheduleType { get; set; }

        public List<string> WeekDayId { get; set; }

        public int MonthDayId { get; set; }

        public string Date { get; set; }

        public int ScheduleQuantityType { get; set; }

        public decimal RequiredQuantity { get; set; }

        public int JobId { get; set; }

        public int TfxSupplierCompanyId { get; set; }

        public int ProductTypeId { get; set; }
    }
}
