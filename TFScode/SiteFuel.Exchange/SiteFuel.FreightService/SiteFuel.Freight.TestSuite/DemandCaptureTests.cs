using Microsoft.VisualStudio.TestTools.UnitTesting;
using SiteFuel.BAL;
using SiteFuel.FreightModels;
using SiteFuel.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Freight.TestSuite
{
	[TestClass]
	public class DemandCaptureTests
	{

		[TestInitialize]
		public void Initialize()
		{
			
		}

		[TestMethod]
		public async void CreateDemandsTests()
		{
			var domain = new DemandCaptureDomain(new DemandCaptureRepository());

			var demands = ConvertCsvToData();
            await domain.CreateDemand(demands, 100, 1);

		}

		private List<DemandModel> ConvertCsvToData()
		{
			var allLines = File.ReadAllLines("..\\..\\inventory_201911270430 - inventory_201911270430.csv").Skip(1).Take(100);
			var demands = allLines.Select(x => {
				var y = x.Split(',');
				//Console.WriteLine(y[3]);
				return new DemandModel
				{
					SiteId = y[0],
					TankId = y[1],
					StorageId = y[2],
					CaptureTime =Convert.ToDateTime(y[3]),
					ProductName = y[4],
					GrossVolume = float.Parse(string.IsNullOrEmpty(y[5]) ? "0" : y[5]),
					NetVolume = float.Parse(string.IsNullOrEmpty(y[6]) ? "0" : y[6]),
					Ullage = float.Parse(string.IsNullOrEmpty(y[7]) ? "0" : y[7]),
					Level = decimal.Parse(string.IsNullOrEmpty(y[8])? "0" : y[8]),
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
