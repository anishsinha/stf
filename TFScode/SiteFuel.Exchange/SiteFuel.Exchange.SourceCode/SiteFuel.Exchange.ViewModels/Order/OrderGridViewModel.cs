using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class OrderGridViewModel : StatusViewModel
    {
        public OrderGridViewModel()
        {
            InstanceInitialize();
        }

        public OrderGridViewModel(Status status) 
            : base(status)
        {
            InstanceInitialize(status);
        }

        private void InstanceInitialize(Status status = Utilities.Status.Failed)
        {

        }
        public int Id { get; set; }

        public string PoNumber { get; set; }

        public string Supplier { get; set; }

        public string JobName { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string FuelType { get; set; }

        public string Type { get; set; }

        public int AssetsAssigned { get; set; }

        public string TotalAmount { get; set; }

        public string Quantity { get; set; }

        public string PricePerGallon { get; set; }

        public string FuelDeliveredPercentage { get; set; }

        public string Status { get; set; }

        public string StartDate { get; set; }

        public int InvoiceCount { get; set; }

        public int DDTCount { get; set; }

        public string Eligibility { get; set; }

        public int BrokerFuelRequestId { get; set; }

        public int StatusId { get; set; }

        public int TotalCount { get; set; }

        public string CustomerPoNumber { get; set; }

        public int CustomerOrderId { get; set; }

        public string Location { get; set; }

        public string DeliveryType { get; set; }

        public string OrderType { get; set; }

        public string DisplayUoM { get; set; }

        public string Currency { get; set; }

        public int? OrderGroupId { get; set; }
        public string GroupPoNumber { get; set; }
        public string OrderName { get; set; }
        public string VesselName { get; set; }
    }
}
