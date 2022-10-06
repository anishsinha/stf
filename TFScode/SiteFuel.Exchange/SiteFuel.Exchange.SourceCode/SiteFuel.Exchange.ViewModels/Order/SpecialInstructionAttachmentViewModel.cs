using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class SpecialInstructionAttachmentViewModel
    {
        public int Id { get; set; }

        public List<AttachmentViewModel> Files { get; set; }
    }
}
