using System.IO;

namespace SiteFuel.Exchange.ViewModels
{
    public class AttachmentViewModel
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public Stream FileStream { get; set; }
    }
}
