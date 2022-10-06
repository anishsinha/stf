using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SiteFuel.FreightModels.ForcastingHelpers
{
    public class Sale1Hour
    {
        private DateTime _startTime = DateTime.MinValue;
        private DateTime _endTime = DateTime.MinValue;
        private List<IDemandModel> _demands = new List<IDemandModel>();
        private List<ITankDropModel> _tankDrops = new List<ITankDropModel>();

        public Sale1Hour()
        {
            // Default constructor
        }

        public Sale1Hour(DateTime startTime, DateTime endTime, List<IDemandModel> demands, List<ITankDropModel> tankDrops)
        {
            _startTime = startTime;
            _endTime = endTime;
            _tankDrops = tankDrops.Where(t => t.StartTime >= startTime && t.EndTime < endTime).ToList();

            // Get all demands of mentioned time for calculating 1 hour sale.
            _demands = demands.Where(t => t.CaptureTime >= startTime && t.CaptureTime < endTime).ToList();
            // Get last entry of previous hour for taking difference between 2 demands.
            var lastDemand = demands.Where(t => t.CaptureTime >= startTime.AddHours(-1) && t.CaptureTime < endTime.AddHours(-1)).LastOrDefault();
            if (lastDemand != null)
            {
                _demands.Insert(0, lastDemand);
            }
            else
            {
                if (startTime.Hour == 0)
                {
                    //get opening hour balance from previous day demand data.
                    GetLastDayDemandData(startTime, demands);
                }
                else
                {

                    if (endTime.Hour > 0)
                    {
                        //Get last entry of previous for taking difference between 2 demands.
                        var lastCurrentDayHourDemand = demands.Where(t => t.CaptureTime.Hour < endTime.Hour && t.CaptureTime.Date == endTime.Date && !(t.CaptureTime.Hour >= startTime.Hour && t.CaptureTime.Hour <= endTime.Hour)).OrderBy(t => t.CaptureTime).LastOrDefault();
                        if (lastCurrentDayHourDemand != null)
                        {
                            _demands.Insert(0, lastCurrentDayHourDemand);
                        }
                        else
                        {
                            GetLastDayDemandData(startTime, demands);
                        }
                    }
                    else
                    {
                        //Get last entry of previous for taking difference between 2 demands. --- if hour between 11:PM to 12:00AM. so take last day  dip test reading.
                        var lastCurrentDayHourDemand = demands.Where(t => t.CaptureTime.Hour < startTime.Hour && t.CaptureTime.Date == startTime.Date && !(t.CaptureTime.Hour >= startTime.Hour)).OrderBy(t => t.CaptureTime).LastOrDefault();
                        if (lastCurrentDayHourDemand != null)
                        {
                            _demands.Insert(0, lastCurrentDayHourDemand);
                        }
                        else
                        {
                            GetLastDayDemandData(startTime, demands);
                        }
                    }
                }
            }
            SaleQuantity = getSaleQuantity();
        }

        private void GetLastDayDemandData(DateTime startTime, List<IDemandModel> demands)
        {
            var lastDayCurrentDemand = demands.Where(t => (t.CaptureTime.Date <= startTime.AddDays(-1))).LastOrDefault();
            if (lastDayCurrentDemand != null)
            {
                var demandIndex = _demands.FindIndex(top => top.CaptureTime == lastDayCurrentDemand.CaptureTime && top.Ullage == lastDayCurrentDemand.Ullage);
                if (demandIndex == -1)
                {
                    _demands.Insert(0, lastDayCurrentDemand);
                }
            }
        }

        private decimal getSaleQuantity()
        {
            decimal saleQuantity = 0;
            _demands = _demands.OrderBy(t => t.CaptureTime).ToList();
            //for (int index = 0; index < _demands.Count; index++)
            //{
            //    var currentUllage = (decimal)_demands[index].Ullage;
            //    if (prevUllage > 0)
            //    {
            //        saleQuantity += (currentUllage - prevUllage);
            //    }
            //    prevUllage = currentUllage;
            //}
            if (_demands.Count > 1)
            {
                saleQuantity = (decimal)(_demands[_demands.Count - 1].Ullage - _demands[0].Ullage);
            }
            if (_tankDrops.Any())
            {
                saleQuantity += _tankDrops.Select(t => t.DroppedQuantity).Sum();
            }
            //set saleQuantity zero if we get ullage negative.
            if (saleQuantity < 0)
            {
                saleQuantity = 0;
            }
            return saleQuantity;
        }

        public DateTime StartTime { get { return _startTime; } }
        public DateTime EndTime { get { return _endTime; } }
        public decimal SaleQuantity { get; private set; }
        public DipTestUoM DipTestUoM { get; set; }
    }
}
