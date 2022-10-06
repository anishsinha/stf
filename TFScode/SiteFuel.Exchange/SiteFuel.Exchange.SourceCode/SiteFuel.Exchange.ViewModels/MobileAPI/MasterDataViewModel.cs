using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.MobileAPI;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class MasterDataViewModel 
    {
        public MasterDataViewModel()
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            ProductTypes = new List<DropdownDisplayItem>();
            ProductDisplayGroups = new List<DropdownDisplayItem>();
            FuelTypes = new List<ProductListViewModel>();
            OpisFuelTypes = new List<DropdownDisplayItem>();
            PlattsFuelTypes = new List<DropdownDisplayItem>();
            DeliveryTypes = new List<DropdownDisplayItem>();
            RackAvgPricingTypes = new List<DropdownDisplayItem>();
            FeeTypes = new List<MasterDataFeesViewModel>();
            DeliveryFeeTypes = new List<DropdownDisplayItem>();
            WetHoseFeeTypes = new List<DropdownDisplayItem>();
            OverWaterFeeTypes = new List<DropdownDisplayItem>();
            DryRunFeeTypes = new List<DropdownDisplayItem>();
            AdditionalFeeTypes = new List<DropdownDisplayItem>();
            DeliveryScheduleTypes = new List<DropdownDisplayItem>();
            PaymentTerms = new List<DropdownDisplayItem>();
            SupplierQualifications = new List<DropdownDisplayItem>();
            WeekDays = new List<DropdownDisplayExtendedItem>();
            States = new List<StateDropdownExtendedItem>();
            Countries = new List<DropdownDisplayExtendedItem>();
            PricingTypes = new List<DropdownDisplayItem>();
            DeliveryScheduleStatuses = new List<DropdownDisplayItem>();
            JobStatuses = new List<DropdownDisplayItem>();
            OrderStatuses = new List<DropdownDisplayItem>();
            OrderTypes = new List<DropdownDisplayItem>();
            FuelRequestStatuses = new List<DropdownDisplayItem>();
            QuantityTypes = new List<DropdownDisplayItem>();
            InvoiceDeclineReasons = new List<DropdownDisplayItem>();
            StateList = new List<StateListViewModel>();
            UnderGallonFeeTypes = new List<DropdownDisplayItem>();
            FeeConstraintTypes = new List<DropdownDisplayItem>();
        }

        public List<DropdownDisplayItem> ProductTypes { get; set; }

        public List<DropdownDisplayItem> ProductDisplayGroups { get; set; }

        public List<ProductListViewModel> FuelTypes { get; set; }

        public List<DropdownDisplayItem> OpisFuelTypes { get; set; }

        public List<DropdownDisplayItem> PlattsFuelTypes { get; set; }

        public List<DropdownDisplayItem> DeliveryTypes { get; set; } //OneTimeDelivery,MultipleDeliveries

        public List<DropdownDisplayItem> RackAvgPricingTypes { get; set; }

        public List<MasterDataFeesViewModel> FeeTypes { get; set; }  //DeliveryFee,WetHoseFee,OverWaterFee,DryRunFee,AdditionalFee

        public List<DropdownDisplayItem> DeliveryFeeTypes { get; set; }

        public List<DropdownDisplayItem> WetHoseFeeTypes { get; set; }

        public List<DropdownDisplayItem> OverWaterFeeTypes { get; set; }

        public List<DropdownDisplayItem> DryRunFeeTypes { get; set; }

        public List<DropdownDisplayItem> UnderGallonFeeTypes { get; set; }

        public List<DropdownDisplayItem> AdditionalFeeTypes { get; set; }

        public List<DropdownDisplayItem> DeliveryScheduleTypes { get; set; } //Weekly ,Bi-Weekly,Monthly,Specific Date(s)

        public List<DropdownDisplayItem> PaymentTerms { get; set; }

        public List<DropdownDisplayItem> SupplierQualifications { get; set; }

        public List<DropdownDisplayExtendedItem> WeekDays { get; set; }

        public List<StateDropdownExtendedItem> States { get; set; }

        public List<DropdownDisplayExtendedItem> Countries { get; set; }

        public List<DropdownDisplayItem> PricingTypes { get; set; } //RackPrice,PricePerGallon,Tier

        public List<DropdownDisplayItem> DeliveryScheduleStatuses { get; set; }

        public List<DropdownDisplayItem> JobStatuses { get; set; }

        public List<DropdownDisplayItem> OrderStatuses { get; set; }

        public List<DropdownDisplayItem> OrderTypes { get; set; } //Hedge,Spot

        public List<DropdownDisplayItem> FuelRequestStatuses { get; set; }

        public List<DropdownDisplayItem> QuantityTypes { get; set; } //SpecificAmount,Range

        public List<DropdownDisplayItem> InvoiceDeclineReasons { get; set; }

        public List<DropdownDisplayItem> FeeConstraintTypes { get; set; }

        public List<StateListViewModel> StateList { get; set; }
    } 
}
