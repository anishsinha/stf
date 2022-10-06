using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels.ForcastingHelpers
{
    public class Sale24HourModel : ISale24HourModel
    {
        public Sale24HourModel()
        {
            // Default Constructor
        }
        public Sale24HourModel(DateTime date, List<Sale1Hour> hourlySales, SaleTankModel tank)
        {
            Date = date;
            SaleTank = tank;
            for (int index = 0; index < hourlySales.Count; index++)
            {
                int _startTime = hourlySales[index].StartTime.Hour;
                switch (_startTime)
                {
                    case 0:
                        From00To01 = hourlySales[index].SaleQuantity;
                        break;
                    case 1:
                        From01To02 = hourlySales[index].SaleQuantity;
                        break;
                    case 2:
                        From02To03 = hourlySales[index].SaleQuantity;
                        break;
                    case 3:
                        From03To04 = hourlySales[index].SaleQuantity;
                        break;
                    case 4:
                        From04To05 = hourlySales[index].SaleQuantity;
                        break;
                    case 5:
                        From05To06 = hourlySales[index].SaleQuantity;
                        break;
                    case 6:
                        From06To07 = hourlySales[index].SaleQuantity;
                        break;
                    case 7:
                        From07To08 = hourlySales[index].SaleQuantity;
                        break;
                    case 8:
                        From08To09 = hourlySales[index].SaleQuantity;
                        break;
                    case 9:
                        From09To10 = hourlySales[index].SaleQuantity;
                        break;
                    case 10:
                        From10To11 = hourlySales[index].SaleQuantity;
                        break;
                    case 11:
                        From11To12 = hourlySales[index].SaleQuantity;
                        break;
                    case 12:
                        From12To13 = hourlySales[index].SaleQuantity;
                        break;
                    case 13:
                        From13To14 = hourlySales[index].SaleQuantity;
                        break;
                    case 14:
                        From14To15 = hourlySales[index].SaleQuantity;
                        break;
                    case 15:
                        From15To16 = hourlySales[index].SaleQuantity;
                        break;
                    case 16:
                        From16To17 = hourlySales[index].SaleQuantity;
                        break;
                    case 17:
                        From17To18 = hourlySales[index].SaleQuantity;
                        break;
                    case 18:
                        From18To19 = hourlySales[index].SaleQuantity;
                        break;
                    case 19:
                        From19To20 = hourlySales[index].SaleQuantity;
                        break;
                    case 20:
                        From20To21 = hourlySales[index].SaleQuantity;
                        break;
                    case 21:
                        From21To22 = hourlySales[index].SaleQuantity;
                        break;
                    case 22:
                        From22To23 = hourlySales[index].SaleQuantity;
                        break;
                    case 23:
                        From23To00 = hourlySales[index].SaleQuantity;
                        break;
                }
            }
        }

        public int Id { get; set; }
        public int SaleTankId { get; set; }
        public SaleTankModel SaleTank { get; set; }
        public DateTime Date { get; set; }
        public decimal From00To01 { get; set; }
        public decimal From01To02 { get; set; }
        public decimal From02To03 { get; set; }
        public decimal From03To04 { get; set; }
        public decimal From04To05 { get; set; }
        public decimal From05To06 { get; set; }
        public decimal From06To07 { get; set; }
        public decimal From07To08 { get; set; }
        public decimal From08To09 { get; set; }
        public decimal From09To10 { get; set; }
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
        public decimal From23To00 { get; set; }
    }
}
