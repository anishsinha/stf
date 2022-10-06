using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class NewsfeedMessagesViewModel
    {
        public NewsfeedMessagesViewModel()
        {
            CurrentPage = 1;
            Messages = new List<NewsfeedListViewModel>();
        }

        public int TotalMessages { get; set; }

        public int TotalPages { get; set; }

        public int CurrentPage { get; set; }

        public List<NewsfeedListViewModel> Messages { get; set; }
    }
}
