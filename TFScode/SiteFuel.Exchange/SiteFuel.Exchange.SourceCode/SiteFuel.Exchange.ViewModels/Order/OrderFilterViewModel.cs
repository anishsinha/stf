using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class OrderFilterViewModel : BaseInputViewModel
    {
        public OrderFilterViewModel()
        {
            Filter = OrderFilterType.All;
        }

        private void InstanceInitialize()
        {
            Filter = OrderFilterType.All;
        }

        public int JobId { get; set; }
        public int OrderId { get; set; }
        public OrderFilterType Filter { get; set; }
        public int FuelTypeId { get; set; }

        public Currency Currency { get; set; }

        public int CountryId { get; set; }
        public List<int> CustomerIds { get; set; } = new List<int>();
        public List<int> LocationIds { get; set; } = new List<int>();
        public List<int> VesselIds { get; set; } = new List<int>();
        public bool IsMarine { get; set; }

        public OrderGridFilterDataViewModel InputFilterDataViewModel { get; set; } = new OrderGridFilterDataViewModel();
    }
    public class OrderGridFilterDataViewModel
    {
        public OrderGridFilterDataViewModel()
        {
            Customers = new List<int>();
            Locations = new List<int>();
            Assets = new List<int>();
            Tanks = new List<int>();
            Vessels = new List<int>();
            IsMarine = false;
        }
        public List<int> Customers { get; set; }
        public List<int> Locations { get; set; }
        public List<int> Assets { get; set; }
        public List<int> Tanks { get; set; }
        public List<int> Vessels { get; set; }
        public bool IsMarine { get; set; }
    }
}
