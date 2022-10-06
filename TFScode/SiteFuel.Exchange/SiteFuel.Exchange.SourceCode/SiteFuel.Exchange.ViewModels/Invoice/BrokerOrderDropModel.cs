using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class BrokerOrderDropModel
    {
        public PaymentTermViewModel PaymentTerm { get; set; }
        public int OrderId { get; set; }
        public string PoNumber { get; set; }
        public int? TerminalId { get; set; }
        public string TerminalName { get; set; }
        public int DefaultInvoiceType { get; set; }
        public decimal? Allowance { get; set; }
        public int FuelTypeId { get; set; }
        public List<FeesViewModel> Fees { get; set; } = new List<FeesViewModel>();
        public bool IsFuelSurcharge { get; set; }
        public int? FuelSurchargePricingType { get; set; }
        public int ProductTypeId { get; set; }
        public int SupplierCompanyId { get; set; }
        public int BuyerCompanyId { get; set; }
        public int AcceptedBy { get; set; }
        public List<TaxViewModel> OtherTaxes { get; set; }
        public OrderEnforcement OrderEnforcement { get; set; } = OrderEnforcement.EnforceOrderLevelValues;
        public int FuelRequestId { get; set; }

    }
}
