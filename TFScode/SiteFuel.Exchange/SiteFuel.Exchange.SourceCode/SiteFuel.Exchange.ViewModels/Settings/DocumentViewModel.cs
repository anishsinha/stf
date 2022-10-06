using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class DocumentViewModel : StatusViewModel
    {
        public DocumentViewModel()
        {
            
        }

        public DocumentViewModel(Status status)
            : base(status)
        {
          
        }


        public int Id { get; set; }

        public string FileName { get; set; }

        public string ModifiedFileName { get; set; }

        public string FilePath { get; set; }

        public int AddedBy { get; set; }

        public string AddedByName { get; set; }

        public int CompanyId { get; set; }
    }
}
