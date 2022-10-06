using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels
{
    public class DSBLoadQueueModel
    {
        public string Id { get; set; }
        public string ScheduleBuilderId { get; set; }
        public string RegionId { get; set; }
        public string Date { get; set; }
        public string ShiftId { get; set; }
        public int ShiftIndex { get; set; }
        public int DriverRowIndex { get; set; }
        public int TfxUserId { get; set; }
        public int TfxCompanyId { get; set; }
    }
    public class DSBLoadQueueDeleteModel
    {
        public string Id { get; set; }
        public string Date { get; set; }
        public string RegionId { get; set; }
        public int TfxUserId { get; set; }
        public int TfxCompanyId { get; set; }
        public string ShiftId { get; set; }
        public int ShiftIndex { get; set; }
        public int DriverRowIndex { get; set; }
    }
}
