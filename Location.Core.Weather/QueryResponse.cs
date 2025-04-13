using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Globalization;

namespace OpenWeatherAPI
{
	public class QueryResponse
	{
		public bool ValidRequest { get; }
		public Coordinates Coordinates { get; }
		public List<Weather> WeatherList { get; } = new List<Weather>();
		public string Base { get; }
		public Main Main { get; }
		public double Visibility { get; }
		public Wind Wind { get; }
		public Rain Rain { get; }
		public Snow Snow { get; }
		public Clouds Clouds { get; }
		public Sys Sys { get; }
		public int ID { get; }
		public string Name { get; }
		public int Cod { get; }
		public int Timezone { get; }

		public QueryResponse(string jsonResponse)
		{
			var jsonData = JObject.Parse(jsonResponse);
			if (jsonData == null)
				return;

		}
	}
}
