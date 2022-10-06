using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels
{
    public class DropdownDisplayItem
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
    public class TfxJobsDetailsModel
    {
        public int Id { get; set; }
        public int SequenceNo { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
    public class TfxCarrierDropdownDisplayViewModelItem
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int SequenceNo { get; set; } = 0;
        public string RegionId { get; set; } = string.Empty;
    }

    public class TfxCarrierRegionDetailsModel
    {
        public string Id { get; set; }       
        public string Name { get; set; }        
    }
}
