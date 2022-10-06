using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.ViewModels
{
    public class InvoiceXSpecialInstructionViewModel : StatusViewModel
    {
        public InvoiceXSpecialInstructionViewModel()
        {
            
        }

        public InvoiceXSpecialInstructionViewModel(Status status)
        {
           
        }

        public int SpecialInstructionId { get; set; }

        public int InvoiceId { get; set; }

        public bool IsInstructionFollowed { get; set; }

        public string Instruction { get; set; }
    }
}
