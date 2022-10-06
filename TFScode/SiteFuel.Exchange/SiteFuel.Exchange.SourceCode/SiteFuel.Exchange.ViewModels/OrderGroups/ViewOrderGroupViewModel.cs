using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class ViewOrderGroupViewModel : StatusViewModel
    {
        public ViewOrderGroupViewModel()
        {
        }

        public ViewOrderGroupViewModel(Status status)
            : base(status)
        {
        }

        public int TotalGroupCount { get; set; }
        public int ShowCount { get; set; }
        public List<OrderGroupDetailModel> OrderGroupDetails { get; set; }
    }

    public class OrderGroupSearchModel
    {
        public string SearchText { get; set; }
        public int GroupTypeId { get; set; }
        public int CompanyId { get; set; }
        public int JobId { get; set; }
        public int ProductCategoryId { get; set; }
        public int StateId { get; set; }
    }

    public class OrderGroupDetailModel
    {
        public int OrderGroupId { get; set; }
        public string CustomerPoNumber { get; set; }
        public string FuelType { get; set; }
        public string JobName { get; set; }
        public string JobAddress { get; set; }
        public string RenewalFrequency { get; set; }
        public OrderGroupType GroupType { get; set; }
        public string DisplayGroupType { get; set; }
        public decimal BlendedGroupWeightedPPG { get; set; }
        public string DisplayBlendedGroupWeightedPPG { get; set; }
        public ProductCategory ProductType { get; set; }
        public string DisplayProductType { get; set; }
        public bool IsEditOrDeleteAllowed { get; set; }
        public bool CanCurrentUserEditOrDeleteGroup { get; set; }
        public int CreatedBy { get; set; }
        public List<OrderDropDetailViewModel> OrderDrops { get; set; }
    }

    public class OrderDropDetailViewModel
    {
        public string TfxPoNumber { get; set; }
        public string FuelType { get; set; }
        public string DroppedGallons { get; set; }
        public decimal FuelDeliveredPercentage { get; set; }
        public string DropPercentage { get; set; }
        public decimal BlendRatioPercentage { get; set; }
        public decimal MinVolume { get; set; }
        public decimal MaxVolume { get; set; }
        public decimal PPG { get; set; }
        public string DisplayPPG { get; set; }
        public string UoM { get; set; }
        public QuantityType QuantityType { get; set; }
        public int TierCount { get; set; }
    }

    public class OrderGroupDDLViewModel
    {
        public bool IsSupplierCompany { get; set; }
        public bool IsBuyerCompany { get; set; }
        public List<DropdownDisplayItem> GroupTypes { get; set; }
        public List<DropdownDisplayItem> Companies { get; set; }
        public List<DropdownDisplayItem> Jobs { get; set; }
        public List<DropdownDisplayItem> ProductCategories { get; set; }
        public List<DropdownDisplayExtendedItem> States { get; set; }
    }
}
