using SiteFuel.Exchange.Core.Resolver;
using SiteFuel.Exchange.Quickbooks.Models;
using SiteFuel.Exchange.Quickbooks.SharedEnums;
using SiteFuel.Exchange.Quickbooks.Workflows.Extensions;
using SiteFuel.Exchange.Quickbooks.Workflows.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Quickbooks.Workflows.Adapters.RequestAdapters
{
    public class ItemDiscountAddRequestAdapter : BaseRequestAdapter<OrderItemViewModel>
    {
        public override AdapterResponse Convert(OrderItemViewModel inputViewModel)
        {
            var item = new ItemDiscountAdd
            {
                Name = string.IsNullOrWhiteSpace(inputViewModel.Prefix) ? inputViewModel.Name : $"{inputViewModel.Prefix} {inputViewModel.Name}",
                AccountRef = new BasicInfo { FullName = inputViewModel.AccountName }
            };
            item.Name = item.Name.RemoveSpecialCharacter();
            ItemDiscountAddRq itemDiscountRq = new ItemDiscountAddRq
            {
                ItemDiscountAdd = item
            };
            return new AdapterResponse
            {
                QbXmlObject = itemDiscountRq,
                QbXmlType = QbXmlType.DiscountAdd,
                Status = QbXmlStatus.ReadyToQueue,
                EntityType = inputViewModel.Name.Replace(" ", string.Empty)
            };
        }
    }
}
