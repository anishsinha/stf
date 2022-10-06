using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueFill.DemandCaptureDataAccess.Entities
{
    public class SaleWeeklyData
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "Date")]
        public DateTime Date { get; set; }
        [Column(TypeName = "Date")]
        public DateTime BandStartDate { get; set; }
        [Column(TypeName = "Time")]
        public TimeSpan BandStartTime { get; set; }
        [Column(TypeName = "Date")]
        public DateTime BandEndDate { get; set; }
        [Column(TypeName = "Time")]
        public TimeSpan BandEndTime { get; set; }
        public int BandNumber { get; set; }
        public WeekDay DayId { get; set; }
        public WeekNumber WeekId { get; set; }
        public int SaleTankId { get; set; }
        public decimal TotalSale { get; set; }
        public decimal AverageSale { get; set; }

        [ForeignKey("SaleTankId")]
        public virtual SaleTank SaleTank { get; set; }
    }
}
