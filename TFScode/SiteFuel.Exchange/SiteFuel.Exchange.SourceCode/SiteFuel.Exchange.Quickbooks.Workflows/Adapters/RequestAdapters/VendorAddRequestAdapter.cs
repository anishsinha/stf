using SiteFuel.Exchange.Core.Resolver;
using SiteFuel.Exchange.Quickbooks.Models;
using SiteFuel.Exchange.Quickbooks.SharedEnums;
using SiteFuel.Exchange.Quickbooks.Workflows.Mappers;
using SiteFuel.Exchange.Quickbooks.Workflows.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Quickbooks.Workflows.Adapters.RequestAdapters
{
    public class VendorAddRequestAdapter : BaseRequestAdapter<PurchaseOrderViewModel>
    {
        public override AdapterResponse Convert(PurchaseOrderViewModel inputViewModel)
        {
            var vendor = new VendorAdd
            {
                Name = inputViewModel.VendorName,
                CompanyName = inputViewModel.VendorCompanyName,
                VendorAddress = inputViewModel.VendorAddress.ToAddress(),
                ShipAddress = inputViewModel.ShipAddress.ToAddress()
            };
            VendorAddRq vendorAddRq = new VendorAddRq
            {
                VendorAdd = vendor
            };
            return new AdapterResponse
            {
                QbXmlObject = vendorAddRq,
                QbXmlType = QbXmlType.VendorAdd,
                Status = QbXmlStatus.ReadyToQueue,
                EntityId = inputViewModel.VendorId,
                EntityType = QbEntityType.Vendor.ToString()
            };
        }
    }
}
