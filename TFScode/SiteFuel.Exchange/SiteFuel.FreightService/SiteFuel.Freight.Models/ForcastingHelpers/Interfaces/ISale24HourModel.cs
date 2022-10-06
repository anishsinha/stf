using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels.ForcastingHelpers
{
    public interface ISale24HourModel
    {
        int Id { get; set; }
        DateTime Date { get; set; }
        int SaleTankId { get; set; }
        SaleTankModel SaleTank { get; set; }
        decimal From00To01 { get; set; }
        decimal From01To02 { get; set; }
        decimal From02To03 { get; set; }
        decimal From03To04 { get; set; }
        decimal From04To05 { get; set; }
        decimal From05To06 { get; set; }
        decimal From06To07 { get; set; }
        decimal From07To08 { get; set; }
        decimal From08To09 { get; set; }
        decimal From09To10 { get; set; }
        decimal From10To11 { get; set; }
        decimal From11To12 { get; set; }
        decimal From12To13 { get; set; }
        decimal From13To14 { get; set; }
        decimal From14To15 { get; set; }
        decimal From15To16 { get; set; }
        decimal From16To17 { get; set; }
        decimal From17To18 { get; set; }
        decimal From18To19 { get; set; }
        decimal From19To20 { get; set; }
        decimal From20To21 { get; set; }
        decimal From21To22 { get; set; }
        decimal From22To23 { get; set; }
        decimal From23To00 { get; set; }
    }
}
