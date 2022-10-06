using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
using SiteFuel.FreightModels.ForcastingHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightRepository.Helpers
{
    public class TankLevelHelper
    {
        public TankDetailsForChartModel GetTankDetails(TankDetailsModel tank)
        {
            var tankDetails = new TankDetailsForChartModel();
            //Here RunOutLevel is Must Go
            if (tank.FillType == FillType.Percent && tank.FuelCapacity.HasValue)
            {
                tankDetails.RunOutLevel = (tank.MinFill.Value * tank.FuelCapacity.Value) / 100;
            }
            else if (tank.FillType == FillType.UoM && tank.MinFill.HasValue)
            {
                tankDetails.RunOutLevel = tank.MinFill.Value;
            }

            //Retain Means Could Go
            //Could Go
            if (tank.ThresholdDeliveryRequest.HasValue && tank.FuelCapacity.HasValue)
            {
                tankDetails.Retain = (tank.ThresholdDeliveryRequest.Value * tank.FuelCapacity.Value) / 100;
            }

            //SafetyStock Means Should Go
            //Should Go = RunOutLevel //In Mongo DB
            if (tank.RunOutLevel.HasValue && tank.FuelCapacity.HasValue)
            {
                tankDetails.SafetyStock = (tank.RunOutLevel.Value * tank.FuelCapacity.Value) / 100;
            }
            return tankDetails;
        }
        public void GetTankDetails(TankOttoDetails tank)
        {
            //Here RunOutLevel is Must Go
            if (tank.FillType == (int)FillType.Percent && tank.FuelCapacity.HasValue)
            {
                tank.RunOutLevel = (tank.MinFill.Value * tank.FuelCapacity.Value) / 100;
            }
            else if (tank.FillType == (int)FillType.UoM && tank.MinFill.HasValue)
            {
                tank.RunOutLevel = tank.MinFill.Value;
            }

            //Retain Means Could Go
            //Could Go
            if (tank.ThresholdDeliveryRequest.HasValue && tank.FuelCapacity.HasValue)
            {
                tank.Retain = (tank.ThresholdDeliveryRequest.Value * tank.FuelCapacity.Value) / 100;
            }

            //SafetyStock Means Should Go
            //Should Go = RunOutLevel //In Mongo DB
            if (tank.RunOutLevel.HasValue && tank.FuelCapacity.HasValue)
            {
                tank.SafetyStock = (tank.RunOutLevel.Value * tank.FuelCapacity.Value) / 100;
            }
        }
        public List<TankLevelModel> GetTankLevels(List<SaleMonthlyDataModel> monthlyDatas, DateTime startTime, decimal currentInventory, int bandPeriod, int targetHours, decimal retainQty, decimal saftyStockQty, decimal runoutQty)
        {
            var response = new List<TankLevelModel>();
            if (monthlyDatas.Count == 0)
                return response;

            decimal bandTotalSale = 0;
            int loopCounter = 0; int loopEndCounter = targetHours / bandPeriod;
            bool isRetainPassed = currentInventory < retainQty;
            bool isSaftyStockPassed = currentInventory < saftyStockQty;
            bool isRunoutPassed = currentInventory < runoutQty;
            while (loopCounter < loopEndCounter)
            {
                // This is for calculating estimated hours
                for (int index = 0; index < monthlyDatas.Count; index++)
                {
                    loopCounter++;
                    var saleMonthlyData = monthlyDatas[index];
                    bandTotalSale = saleMonthlyData.AverageSale;
                    currentInventory -= bandTotalSale;

                    AddTankLevel(response, isRetainPassed, saleMonthlyData, currentInventory, retainQty, startTime, loopCounter, bandPeriod, 1);
                    AddTankLevel(response, isSaftyStockPassed, saleMonthlyData, currentInventory, saftyStockQty, startTime, loopCounter, bandPeriod, 2);
                    AddTankLevel(response, isRunoutPassed, saleMonthlyData, currentInventory, runoutQty, startTime, loopCounter, bandPeriod, 3);

                    if (currentInventory <= 0 || loopCounter >= loopEndCounter)
                        break;

                    if (currentInventory > 0 && index == monthlyDatas.Count - 1)
                        index = 0;
                }
            }
            return response;
        }

        private void AddTankLevel(List<TankLevelModel> tankLevels, bool isLevelPassed, SaleMonthlyDataModel saleMonthlyData, decimal currentInventory, decimal targetQty, DateTime startTime, int loopCounter, int bandPeriod, int levelType)
        {
            if (!isLevelPassed && !tankLevels.Any(t => t.Type == levelType) && currentInventory <= targetQty)
            {
                var levelTime = startTime.AddHours(loopCounter * bandPeriod);
                var remainingQty = targetQty - currentInventory;
                if (remainingQty >= 0)
                {
                    currentInventory += remainingQty;
                    var bandMinutsewiseSale = saleMonthlyData.AverageSale / (bandPeriod * 60);
                    var minutesForSale = (double)Math.Floor(remainingQty / bandMinutsewiseSale);
                    levelTime.AddMinutes(-minutesForSale);
                    tankLevels.Add(new TankLevelModel()
                    {
                        Quantity = currentInventory,
                        Date = levelTime.ToString(Resource.constFormatDate),
                        Time = levelTime.ToString(Resource.constFormatDateTime),
                        Type = levelType,
                        DateTime = levelTime,
                        Hours = (loopCounter * bandPeriod) - (minutesForSale / 60)
                    });
                }
            }
        }
    }
}
