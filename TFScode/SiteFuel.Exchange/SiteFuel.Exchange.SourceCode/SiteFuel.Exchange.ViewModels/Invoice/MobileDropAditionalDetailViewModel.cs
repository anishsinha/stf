using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.ViewModels
{
    public class MobileDropAditionalDetailViewModel
    {
        public int OrderId { get; set; }
        public string PoNumber { get; set; }
        public string TimeZoneName { get; set; }
        public PaymentTermViewModel PaymentTerm { get; set; }
        public int InvoiceTypeId { get; set; }
        public int ProductTypeId { get; set; }
        public string InvoiceNotes { get; set; }
        public bool IsVariousOrigin { get; set; }
        public decimal? Allowance { get; set; }
        public FuelSurchargeFreightFeeViewModel FuelSurcharge { get; set; }
        public List<TaxViewModel> OtherTaxDetails { get; set; } = new List<TaxViewModel>();
        public List<FeesViewModel> Fees { get; set; } = new List<FeesViewModel>();
        public List<SpecialInstructionViewModel> SpecialInstructions { get; set; } = new List<SpecialInstructionViewModel>();
        public PickupLocationType PickupLocationType { get; set; }
        public DropAddressViewModel BulkPlantAddress { get; set; }
        public bool IsSignatureRequired { get; set; }
        public bool IsBOLImageRequired { get; set; }
        public bool IsDropImageRequired { get; set; }
        public bool IsBolDetailsRequired { get; set; }
        public int BuyerCompanyId { get; set; }
        public int SupplierCompanyId { get; set; }
        public int JobId { get; set; }
        public bool IsMarineLocation { get; set; }
        public FreightPricingMethod FreightPricingMethod { get; set; } = FreightPricingMethod.Manual;
    }
}
