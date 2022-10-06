using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
   public class TractorDetailViewModel
    {
        public string Id { get; set; }
        public string TractorId { get; set; }
        public string VIN { get; set; }
        public string Plate { get; set; }
        public string ExpirationDate { get; set; }
        public List<TrailerTypeStatus> TrailerType { get; set; }
        public string Owner { get; set; }
        public List<DriverDetails> Drivers { get; set; }
        public int TfxCreatedBy { get; set; }
        public int TfxCompanyId { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public TractorStatus Status { get; set; } = TractorStatus.Active;
    }
    public class DriverDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

}
