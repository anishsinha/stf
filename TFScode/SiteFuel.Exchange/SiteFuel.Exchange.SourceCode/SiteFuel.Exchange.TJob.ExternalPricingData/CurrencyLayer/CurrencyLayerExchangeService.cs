using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.TJob.ExternalPricingData.CurrencyLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace SiteFuel.Exchange.TJob.ExternalPricingData
{
	public class CurrencyLayerExchangeService
	{

		public List<Tuple<string, string, decimal>> GetLatestCurrencyRates(string url)
		{
			string jsonString = string.Empty;
			try
			{
				var webReq = (HttpWebRequest)WebRequest.Create(url);
				webReq.Method = "GET";
				var WebResp = (HttpWebResponse)webReq.GetResponse();

				Console.WriteLine(WebResp.StatusCode);
				Console.WriteLine(WebResp.Server);

				using (var stream = WebResp.GetResponseStream())
				{
					StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
					jsonString = reader.ReadToEnd();
				}
				var rates = CurrencyConversion.FromJson(jsonString);

				if (rates != null)
				{
					var msg = $"Conversion service returned success status- {rates.Success}";
					Console.WriteLine(msg);
					LogManager.Logger.WriteDebug("GetLatestCurrencyRates", "CurrencyLayerExchangeService", "msg");
					return ParseCurrencyRates(rates);
				}
			}
			catch (Exception ex)
			{
				LogManager.Logger.WriteException("GetLatestCurrencyRates", "CurrencyLayerExchangeService",
					$"Exchange service error on response-: {jsonString}", ex);
			}
			return new List<Tuple<string, string, decimal>>();
		}

		private List<Tuple<string, string, decimal>> ParseCurrencyRates(CurrencyConversion rates)
		{
			if (rates.Success && rates.Quotes.ContainsKey("USDCAD"))
			{
				var usdRate = (decimal)rates.Quotes["USDCAD"];
				var xRates = new List<Tuple<string, string, decimal>>
					{
						new Tuple<string, string, decimal>("USD", "CAD", usdRate),
						new Tuple<string, string, decimal>("CAD", "USD", 1/usdRate)
					};
				return xRates;
			}
			throw new InvalidDataException("Didn't recieve required currency from servce-USDCAD");
		}
	}
}