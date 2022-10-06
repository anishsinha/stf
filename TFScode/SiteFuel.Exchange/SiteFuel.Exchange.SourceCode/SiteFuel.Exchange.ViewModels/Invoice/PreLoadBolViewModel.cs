using Newtonsoft.Json;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SiteFuel.Exchange.ViewModels
{
    public class PreLoadBolViewModel
    {
        public int Id { get; set; }
        public string BolNumber { get; set; }
        public bool IsBulkPlant { get; set; }
        public string LiftTicketNumber { get; set; }
        public string BadgeNumber { get; set; }
        public DateTimeOffset? LiftDate { get; set; }
        public DateTimeOffset PickupDate { get; set; }
        public List<PreLoadProductViewModel> Products { get; set; } = new List<PreLoadProductViewModel>();
        public string TraceId { get; set; }
        public DropdownDisplayItem Driver { get; set; }
        public int SupplierCompanyId { get; set; }
        public string Carrier { get; set; }
        public bool IsPreLoadBolCompleted { get; set; }
        public bool IsPickupBOLRetain { get; set; } = false;
        public string TrailerRetainInfo { get; set; } = string.Empty;
        public ImageViewModel Images { get; set; }

        public string LiftStartTime { get; set; }

        public string LiftEndTime { get; set; }

        public ImageViewModel[] ImageList
        {
            get
            {
                if (Images != null && !string.IsNullOrEmpty(Images.FilePath))
                    return Images.BreakFilePathToMany().ToArray();
                else
                    return new List<ImageViewModel>().ToArray();
            }
        }

        public PreLoadBolViewModel CopyObject(PreLoadBolViewModel source)
        {
            var inputString = JsonConvert.SerializeObject(source);
            return JsonConvert.DeserializeObject<PreLoadBolViewModel>(inputString);
        }
    }

    public class PreLoadProductViewModel
    {
        private decimal? _netQuantity = null;
        private decimal? _grossQuantity = null;
        private int? _orderId = null;
        public int FuelTypeId { get; set; }
        public decimal? NetQuantity
        {
            get { return (_netQuantity == null || _netQuantity <= 0) && LiftQuantity > 0 ? LiftQuantity : _netQuantity; }
            set { _netQuantity = value; }
        }
        public decimal? GrossQuantity
        {
            get { return (_grossQuantity == null || _grossQuantity <= 0) && LiftQuantity > 0 ? LiftQuantity : _grossQuantity; }
            set { _grossQuantity = value; }
        }

        public List<PreLoadCompartmentInfoViewModel> CompartmentInfo { get; set; } = new List<PreLoadCompartmentInfoViewModel>();

        public int? OrderId { get { return _orderId > 0 ? _orderId : (int?)null; } set { _orderId = value; } }
        public int? DeliveryScheduleId { get; set; }
        public int? TrackableScheduleId { get; set; }

        public TimeSpan? LiftArrivalTime { get; set; }
        public TimeSpan? BolCreationTime { get; set; }

        //Terminal
        public int? TerminalId { get; set; }
        public string TerminalName { get; set; }

        //Bulk Plant
        public decimal? LiftQuantity { get; set; }
        public int BulkPlantId { get; set; }
        public string BulkPlantName { get; set; }
        public TimeSpan? LiftStartTime { get; set; }
        public TimeSpan? LiftEndTime { get; set; }
        public DropAddressViewModel Address { get; set; }
        public string ProductType { get; set; }
        public string FuelType { get; set; }
        
        public int ProductId { get; set; }
    }

    public class PreLoadCompartmentInfoViewModel
    {
        public string TrailerId { get; set; }
        public string CompartmentId { get; set; }
        public decimal Quantity { get; set; }
        public int UOM { get; set; }
    }
}

