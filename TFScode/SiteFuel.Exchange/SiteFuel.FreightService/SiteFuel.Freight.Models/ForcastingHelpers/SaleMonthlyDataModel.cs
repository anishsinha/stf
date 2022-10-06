using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels.ForcastingHelpers
{
    public class SaleMonthlyDataModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime BandStartDate { get; set; }
        public TimeSpan BandStartTime { get; set; }
        public DateTime BandEndDate { get; set; }
        public TimeSpan BandEndTime { get; set; }
        public int BandNumber { get; set; }
        public int SaleTankId { get; set; }
        public decimal TotalSale { get; set; }
        public decimal AverageSale { get; set; }
    }
}
