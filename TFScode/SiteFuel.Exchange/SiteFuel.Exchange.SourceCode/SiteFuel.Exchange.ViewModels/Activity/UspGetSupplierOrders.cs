using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class UspGetBrokerActivity
    {
        public int Id { get; set; }

        public int ParentOrderId { get; set; }

        public string SupplierPoNumber { get; set; }

        public string BuyerPoNumber { get; set; }

        public string SupplierCompany { get; set; }

        public string BuyerCompany { get; set; }

        public int SupplierCompanyId { get; set; }

        public int BuyerCompanyId { get; set; }

        public string FuelType { get; set; }

        public string SupplierQuantity { get; set; }

        public string BuyerQuantity { get; set; }

        public string SupplierPPG { get; set; }

        public string BuyerPPG { get; set; }

        public string Status { get; set; }

    }
}