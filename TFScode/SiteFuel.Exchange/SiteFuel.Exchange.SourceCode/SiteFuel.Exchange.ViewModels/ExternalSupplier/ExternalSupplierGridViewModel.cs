using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class ExternalSupplierGridViewModel 
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        public string CompanyType { get; set; }

        public string Website { get; set; }

        public bool InPipedrive { get; set; }

        public string ContactPersonName { get; set; }

        public string Address { get; set; }

        public string AddedBy { get; set; }

        public string DateAdded { get; set; }

    }
}
