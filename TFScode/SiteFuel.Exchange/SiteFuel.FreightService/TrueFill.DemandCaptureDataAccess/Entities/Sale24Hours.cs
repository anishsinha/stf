using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueFill.DemandCaptureDataAccess.Entities
{
    public class Sale24Hours
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "Date")]
        public DateTime Date { get; set; }
        public int SaleTankId { get; set; }
        public decimal From0To1 { get; set; }
        public decimal From1To2 { get; set; }
        public decimal From2To3 { get; set; }
        public decimal From3To4 { get; set; }
        public decimal From4To5 { get; set; }
        public decimal From5To6 { get; set; }
        public decimal From6To7 { get; set; }
        public decimal From7To8 { get; set; }
        public decimal From8To9 { get; set; }
        public decimal From9To10 { get; set; }
        public decimal From10To11 { get; set; }
        public decimal From11To12 { get; set; }
        public decimal From12To13 { get; set; }
        public decimal From13To14 { get; set; }
        public decimal From14To15 { get; set; }
        public decimal From15To16 { get; set; }
        public decimal From16To17 { get; set; }
        public decimal From17To18 { get; set; }
        public decimal From18To19 { get; set; }
        public decimal From19To20 { get; set; }
        public decimal From20To21 { get; set; }
        public decimal From21To22 { get; set; }
        public decimal From22To23 { get; set; }
        public decimal From23To0 { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal DayTotal { get; private set; }

        [ForeignKey("SaleTankId")]
        public virtual SaleTank SaleTank { get; set; }
    }
}
