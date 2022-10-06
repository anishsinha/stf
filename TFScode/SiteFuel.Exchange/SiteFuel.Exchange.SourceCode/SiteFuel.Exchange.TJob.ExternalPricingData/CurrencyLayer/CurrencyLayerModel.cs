namespace SiteFuel.Exchange.TJob.ExternalPricingData.CurrencyLayer
{
	using Newtonsoft.Json;
	using Newtonsoft.Json.Converters;
	using System;
	using System.Collections.Generic;
	using System.Globalization;

	public partial class CurrencyConversion
	{
		[JsonProperty("success")]
		public bool Success { get; set; }

		[JsonProperty("terms")]
		public Uri Terms { get; set; }

		[JsonProperty("privacy")]
		public Uri Privacy { get; set; }

		[JsonProperty("timestamp")]
		public long Timestamp { get; set; }

		[JsonProperty("source")]
		public string Source { get; set; }

		[JsonProperty("quotes")]
		public Dictionary<string, double> Quotes { get; set; }

		public static CurrencyConversion FromJson(string json) => JsonConvert.DeserializeObject<CurrencyConversion>(json, Converter.Settings);

	}

	internal static class Converter
	{
		public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
		{
			MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
			DateParseHandling = DateParseHandling.None,
			Converters =
			{
				new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
			},
		};
	}

}
