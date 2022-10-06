using SiteFuel.BAL;
using SiteFuel.FreightApi.Controllers;
using SiteFuel.FreightRepository;
using System;
using System.Collections.Generic;
using Xunit;
using System.Web.Http;
using TrueFill.FreightApi.Controllers;
using SiteFuel.FreightModels;
using System.IO;
using SiteFuel.Repository;
using System.Linq;

namespace TrueFill.FreightApi.Tests
{
    public class UnitTest1
    {
        [Fact]
        public async void Test1()
        {
            ITankRepository tankR = new TankRepository();
            ITankDomain tankDomain = new TankDomain(tankR);

            TankController ff = new TankController(tankDomain);
            List<int> tanks = new List<int>() { 100, 101 };
            var xyz = await ff.GetTankList(tanks);

        }

        [Fact]
        public async void Test2()
        {
            ISalesRepository tankR = new SalesRepository();
            ISalesDomain tankDomain = new SalesDomain(tankR);

            SalesController ff = new SalesController(tankDomain);
            List<int> tanks = new List<int>() { 100, 101 };
            try
            {
                var xyz = await ff.GetSalesData(new SalesDataRequestModel());
            }
            catch (Exception ex)
            {
                var exx = ex;
            }

        }

        [Fact]
        public  void Test3()
        {
            IJobRepository tankR = new JobRepository();
            IJobDomain tankDomain = new JobDomain(tankR);

            JobController ff = new JobController(tankDomain);
            List<int> tanks = new List<int>() { 100, 101 };
            try
            {
                // var xyz =  ff.GetDipTestDetails("52155","100",90);
                DemandCaptureChartData m = new DemandCaptureChartData();
                m.siteID = new List<string>() { "52155" };
                m.TankId = new List<string>() { "100" };
                m.noOfDays = 90;

                 var xyz =  ff.GetDipTestDetails(m);
            }
            catch (Exception ex)
            {
                var exx = ex;
            }

        }


        [Fact]
        public async void Test4()
        {
            IForecastingRepository tankR = new ForecastingRepository();
            IForecastingDomain tankDomain = new ForecastingDomain(tankR);

            ForecastingController ff = new ForecastingController(tankDomain);
            List<int> tanks = new List<int>() { 100, 101 };
            try
            {

                //ForecastingController m = new ForecastingController();
                //m.siteID = new List<string>() { "52155" };
                //m.TankId = new List<string>() { "100" };
                //m.noOfDays = 90;

                var xyz = await ff.GetForecastingTankDetails(1,"","","","");
            }
            catch (Exception ex)
            {
                var exx = ex;
            }

        }

        [Fact]
        public async void CreateDemandsTests()
        {
            var domain = new DemandCaptureDomain(new DemandCaptureRepository());

            var demands = ConvertCsvToData();
            await domain.CreateDemand(demands, 10, 1);

        }

        private List<DemandModel> ConvertCsvToData()
        {
            var allLines = File.ReadAllLines(@"D:\TFScode\SiteFuel.Exchange\SiteFuel.FreightService\SiteFuel.Freight.TestSuite\inventory_201911270430 - inventory_201911270430.csv").Skip(1).Take(100);
            var demands = allLines.Select(x => {
                var y = x.Split(',');
                //Console.WriteLine(y[3]);
                return new DemandModel
                {
                    SiteId = y[0],
                    TankId = y[1],
                    StorageId = y[2],
                    CaptureTime = Convert.ToDateTime(y[3]),
                    ProductName = y[4],
                    GrossVolume = float.Parse(string.IsNullOrEmpty(y[5]) ? "0" : y[5]),
                    NetVolume = float.Parse(string.IsNullOrEmpty(y[6]) ? "0" : y[6]),
                    Ullage = float.Parse(string.IsNullOrEmpty(y[7]) ? "0" : y[7]),
                    Level = decimal.Parse(string.IsNullOrEmpty(y[8]) ? "0" : y[8]),
                    // Ignoring temprature at 9
                    WaterGrossLevel = float.Parse(string.IsNullOrEmpty(y[10]) ? "0" : y[10]),
                    WaterNetLevel = float.Parse(string.IsNullOrEmpty(y[11]) ? "0" : y[11]),
                    DataSourceTypeId = 1,                   


                };
            });

            return demands.ToList();
        }

    }
}
