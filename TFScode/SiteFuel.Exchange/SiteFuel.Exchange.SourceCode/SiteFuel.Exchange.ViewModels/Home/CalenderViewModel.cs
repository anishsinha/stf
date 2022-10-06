using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class CalenderViewModel : StatusViewModel
    {
        public CalenderViewModel()
        {
            Customers = new List<DropdownDisplayItem>();
            Orders = new List<DropdownDisplayItem>();
        }

        public CalenderViewModel(Status status)
            : base(status)
        {
            Customers = new List<DropdownDisplayItem>();
            Orders = new List<DropdownDisplayItem>();
        }

        public int id { get; set; }

        public string title { get; set; }

        public string start { get; set; }

        public string textColor { get; set; }

        public string url { get; set; }

        public bool allDay { get; set; }

        public int calendarEventType { get; set; }

        public int eventStatus { get; set; }

        public int parentStatus { get; set; }

        public bool isInvoiceGenerated { get; set; }

        public string subtitle { get; set; }

        public string[] viewableIn { get; set; }

        public string orderStatus { get; set; }

        public List<DropdownDisplayItem> Customers { get; set; }

        public List<DropdownDisplayItem> Orders { get; set; }
    }
}
