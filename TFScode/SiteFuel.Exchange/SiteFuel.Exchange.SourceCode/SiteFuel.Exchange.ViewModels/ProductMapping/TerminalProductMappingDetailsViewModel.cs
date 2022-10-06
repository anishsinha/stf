using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
   public class TerminalProductMappingDetailsViewModel
   {
        //TerminalProductMappingDetailsViewModel()
        //{
        //    MappedProducts = new List<DropdownDisplayItem>();
        //}
        public int TerminalId { get; set; }
        public string TerminalControlNumber { get; set; }
        public string TerminalName { get; set; }
        public string AssignedProducts { get; set; }
        public int MappedProductCount { get; set; }
        public List<DropdownDisplayItem> MappedProducts { get; set; }

    }


    public class TerminalProductMappingInput
    {
        public TerminalProductMappingInput()
        {
            NewProductIds = new List<int>();
            RemovedProductIds = new List<int>();
            ExistingMappedProductIds = new List<int>();
            ExternalProductIdXProductIds = new List<ExternalProductIdXProductIdViewModel>();
        }
        public List<int> NewProductIds { get; set; }

        public List<int> RemovedProductIds { get; set; }

        public List<int> ExistingMappedProductIds { get; set; }

        public List<ExternalProductIdXProductIdViewModel> ExternalProductIdXProductIds { get; set; }

        public int TerminalId { get; set; }

        public List<ProductIdXMappingStatusViewModel> ProductIdXMappingStatus { get; set; }
    }

    public class ExternalProductIdXProductIdViewModel
    {
        public int ExternalProductId { get; set; }

        public int ProductId { get; set; }
    }

    public class ProductIdXMappingStatusViewModel
    {
        public bool IsMappingExists { get; set; }

        public int ProductId { get; set; }

        public int TerminalId { get; set; }
    }
}
