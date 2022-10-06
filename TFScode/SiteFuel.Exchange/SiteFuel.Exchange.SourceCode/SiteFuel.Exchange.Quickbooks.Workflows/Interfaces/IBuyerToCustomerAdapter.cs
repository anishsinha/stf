using SiteFuel.Exchange.Quickbooks.Models;
using SiteFuel.Exchange.Quickbooks.Workflows.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Quickbooks.Workflows.Interfaces
{
    public interface IBuyerToCustomerAdapter
    {
        CustomerAdd Convert(SalesOrderViewModel viewModel);

        string GetQbXml(CustomerAdd customer);
    }
}
