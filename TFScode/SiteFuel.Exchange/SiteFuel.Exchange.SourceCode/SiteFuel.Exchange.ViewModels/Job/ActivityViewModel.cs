
using SiteFuel.Exchange.Utilities;
using System;

namespace SiteFuel.Exchange.ViewModels
{
    public class ActivityViewModel : StatusViewModel
    {
        public ActivityViewModel()
        {
            InstanceInitialize();
        }

        public ActivityViewModel(Status status)
            : base(status)
        {
            InstanceInitialize(status);
        }

        private void InstanceInitialize(Status status = Utilities.Status.Failed)
        {

        }
        public int Id { get; set; }

        public string PoNumber { get; set; }

        public string InvoiceNumber { get; set; }

        public int JobId { get; set; }

        public string JobName { get; set; }

        public string DisplayJobID { get; set; }

        public string Date { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public string Company { get; set; }

        public string AssetName { get; set; }

        public string AssetId { get; set; }

        public string Service { get; set; }

        public string GallonsDelivered { get; set; }

        public string Quantity { get; set; }

        public string PricingFormat { get; set; }

        public string UnitCost { get; set; }

        public decimal Cost { get; set; }

        public int OrderId { get; set; }

        public int InvoiceId { get; set; }

        public string ResaleUnitCost { get; set; }

        public string ResaleCost { get; set; }

        public string ResaleContractNo { get; set; }

        public string AssetContractNo { get; set; }

        public string VehicleId { get; set; }

        public string FuelType { get; set; }

        public bool IsActive { get; set; }
    }
}
