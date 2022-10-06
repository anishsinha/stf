using SiteFuel.Exchange.Core.Resolver;
using SiteFuel.Exchange.Quickbooks.Models;
using SiteFuel.Exchange.Quickbooks.SharedEnums;
using SiteFuel.Exchange.Quickbooks.Workflows.Extensions;
using SiteFuel.Exchange.Quickbooks.Workflows.Mappers;
using SiteFuel.Exchange.Quickbooks.Workflows.Models;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Quickbooks.Workflows.Adapters.RequestAdapters
{
    public class BillAddRequestAdapter : BaseRequestAdapter<QbBillViewModel>
    {
        public override AdapterResponse Convert(QbBillViewModel inputViewModel)
        {
            var billAdd = new BillAdd
            {
                VendorRef = new BasicInfo { FullName = inputViewModel.VendorName },
                VendorAddress = inputViewModel.VendorAddress.ToAddress(),
                TxnDate = inputViewModel.TxnDate.ToDateString(),
                DueDate = inputViewModel.DueDate.ToDateString(),
                //need to confirm about length of PO number from Brandon
                RefNumber = inputViewModel.ReferenceNum.CropToLastChars(20),
                TermsRef = string.IsNullOrWhiteSpace(inputViewModel.PaymentTermName) ? null :
                            new BasicInfo { FullName = inputViewModel.PaymentTermName },
                Memo = inputViewModel.Memo?.CropToLastChars(50)
            };
            var billAddRq = new BillAddRq
            {
                BillAdd = billAdd
            };

            return new AdapterResponse
            {
                QbXmlObject = billAddRq,
                QbXmlType = QbXmlType.BillAdd,
                Status = QbXmlStatus.NotReadyToQueue,
                EntityId = inputViewModel.OrderId,
                EntityType = QbEntityType.Bill.ToString()
            };
        }
    }
}
