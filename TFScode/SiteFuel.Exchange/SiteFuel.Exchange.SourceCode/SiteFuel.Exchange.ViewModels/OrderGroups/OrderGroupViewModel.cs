using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class OrderGroupViewModel
    {
        
        public int Id { get; set; }
        public int BuyerCompanyId { get; set; }
        public int JobId { get; set; }
        public int SupplierCompanyId { get; set; }
        public ProductCategory ProductType { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public OrderGroupFrequency RenewalFrequency { get; set; }
        public int RenewalCount { get; set; }
        public OrderGroupType GroupType { get; set; }
        public string GroupPoNumber { get; set; }

        public List<OrderGroupXOrderDetails> OrderList { get; set; } = new List<OrderGroupXOrderDetails>();

        public static string GetCustomGroupNumber(string GroupPoNumber)
        {
            return GroupPoNumber + "_" + Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 3);
        }
    }

    public class OrderGroupXOrderDetails
    {
        public OrderGroupDetailViewModel Order { get; set; }
        public int GroupId { get; set; }
        public int OrderId { get; set; }
        public decimal BlendPercentage { get; set; }
        public decimal MinVolume { get; set; }
        public decimal MaxVolume { get; set; }
    }

    public class OrderBlendedGroupViewModel
    {
        public OrderGroupViewModel GroupDetails { get; set; } = new OrderGroupViewModel();
        public List<DropdownDisplayItem> Jobs { get; set; } = new List<DropdownDisplayItem>();
        public List<DropdownDisplayItem> FuelTypes { get; set; } = new List<DropdownDisplayItem>();
        public List<OrderGroupDetailViewModel> FilteredOrders { get; set; } = new List<OrderGroupDetailViewModel>();

    }
}
