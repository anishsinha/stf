using SiteFuel.Exchange.Core.Resolver;
using SiteFuel.Exchange.Quickbooks.Models;
using SiteFuel.Exchange.Quickbooks.SharedEnums;
using SiteFuel.Exchange.Quickbooks.Workflows.Adapters.RequestAdapters;
using SiteFuel.Exchange.Quickbooks.Workflows.Interfaces;
using SiteFuel.Exchange.Quickbooks.Workflows.Mappers;
using SiteFuel.Exchange.Quickbooks.Workflows.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Quickbooks.Workflows.Adapters.RequestAdapters
{
    public class CustomerAddRequestAdapter : BaseRequestAdapter<CustomerViewModel>
    {
        public override AdapterResponse Convert(CustomerViewModel inputViewModel)
        {
            var customer = new CustomerAdd
            {
                Name = inputViewModel.Name,
                CompanyName = inputViewModel.CompanyName,
                BillAddress = inputViewModel.BillAddress?.ToAddress(),
                ShipAddress = inputViewModel.ShipAddress?.ToAddress()
            };
            CustomerAddRq customerAddRq = new CustomerAddRq
            {
                CustomerAdd = customer
            };
            return new AdapterResponse
            {
                QbXmlObject = customerAddRq,
                QbXmlType = QbXmlType.CustomerAdd,
                Status = QbXmlStatus.ReadyToQueue,
                EntityId = inputViewModel.Id,
                EntityType = QbEntityType.Customer.ToString()
            };
        }
    }
}
