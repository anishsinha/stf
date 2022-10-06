using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class MobileInvoiceCreateRequestViewModel : InvoiceCreateViewModel
    {
        public MobileInvoiceCreateRequestViewModel()
        {
            InstanceInitialize();
        }

        public MobileInvoiceCreateRequestViewModel(Status status) : base(status)
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            SpecialInstructions = new List<InvoiceXSpecialInstructionViewModel>();
        }
        public List<InvoiceXSpecialInstructionViewModel> SpecialInstructions { get; set; }
        public CustomerSignatureViewModel CustomerSignature { get; set; }
        //public DispatchLocationViewModel PickupLocation { get; set; } 
        //public DispatchLocationViewModel DropLocation { get; set; }
        //public BolDetailViewModel BolDetails { get; set; }
        public int DriverId { get; set; }

        public MobileInvoiceCreateRequestViewModel Clone()
        {
            return (MobileInvoiceCreateRequestViewModel)this.MemberwiseClone();
        }
    }
}
