using SiteFuel.Exchange.Utilities;
using System;

namespace SiteFuel.Exchange.ViewModels
{
    public class TerminalSupplierViewModel : StatusViewModel
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public int ProductTypeId { get; set; }

        public Country Country{ get; set; }

        public bool IsActive { get; set; }

        public int AddedBy { get; set; }

        public DateTimeOffset AddedDate { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }
        public string ProductTypeName { get; set; }
       public int CountryId { get; set; }
    }
}
