using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace OpenWeatherAPI
{
	public class OpenWeatherApiClient : IDisposable
	{
		private readonly string _apiKey;
		private readonly string _url;
		private readonly HttpClient _httpClient;
		private readonly bool _useHttps;
		private readonly string _units;
        public OpenWeatherApiClient(string apiKey, string URL, string units, bool useHttps = true)
		{
			_apiKey = apiKey;
            _url = URL ?? throw new ArgumentNullException(nameof(URL));
            _httpClient = new HttpClient();
			_useHttps = useHttps;
			if(units =="F")
			{
				_units = "imperial";
			}
            else
            {
                _units = "metric";
            }
        }
		/// <summary>
		/// DON'T USE THIS CONSTRUCTOR. Use the one with URL. VS was bitching about needing an overload, so VS got one.  Don't Use
		/// </summary>
		/// <param name="apiKey"></param>
		/// <param name="URL"></param>
		/// <param name="useHttps"></param>
		/// 
		[Obsolete("Use the constructor with URL string parameter.  DO not pass a URI. This will be removed in future versions.")]
		public OpenWeatherApiClient(string apiKey, Uri URL, string units, bool useHttps = true)
        {
            _apiKey = apiKey;
            var x = URL ?? throw new ArgumentNullException(nameof(URL));
			_url = x.PathAndQuery;
            _httpClient = new HttpClient();
            _useHttps = useHttps;
        }
        public string GetData(double lat, double lng)
		{
			var query = _url+ $"?lat={lat}&lon={lng}&exclude=minutely,hourly,alerts&units={_units}&appid={_apiKey}";

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, query);
			HttpResponseMessage resp = Task.Run(() => _httpClient.SendAsync(request)).Result;
			var content = resp.Content.ReadAsStringAsync().Result;

            request.Dispose();
            resp.Dispose();

			return content;
        }

		private async Task<Uri> GenerateRequestUrl(string queryString)
		{
			var geo = await Geolocate(queryString).ConfigureAwait(false);

			//string scheme = "http";
			//if (_useHttps)
				//scheme = "https";
			return new Uri("http://api.openweathermap.org/data/3.0/onecall?lat=40.1756&lon=-86&exclude=minutely,hourly,alerts&units=imperial&appid=aa24f449cced50c0491032b2f955d610");

           // return  new Uri($"{scheme}://api.openweathermap.org/data/3.0/onecall?q={queryString}&exclude=minutely,hourly,alerts&units=imperial&appid={_apiKey}");
		}

		public async Task<GeoResponse> Geolocate(string queryString)
		{
            //http://api.openweathermap.org/data/3.0/onecall?lat=40.1756&lon=-86&exclude=minutely,hourly,alerts&units=imperial&appid=aa24f449cced50c0491032b2f955d610
            string scheme = "http";
			if (_useHttps)
				scheme = "https";
			
            var jsonResponse = await _httpClient
				.GetStringAsync(
					new Uri($"{scheme}://api.openweathermap.org/data/3.0/onecall?q={queryString}&exclude=minutely,hourly,alerts&units=imperial&appid={_apiKey}"))
				.ConfigureAwait(false);
			return new GeoResponse(jsonResponse);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="queryString"></param>
		/// <returns>Returns null if the query is invalid.</returns>
		public async Task<QueryResponse> QueryAsync(string queryString)
		{

            var zzz = _httpClient.GetStreamAsync(new Uri("http://api.openweathermap.org/data/3.0/onecall?lat=40.1756&lon=-86&exclude=minutely,hourly,alerts&units=imperial&appid=aa24f449cced50c0491032b2f955d610")).ConfigureAwait(false).GetAwaiter().GetResult();
            var jsonResponse = await _httpClient.GetStringAsync(GenerateRequestUrl(queryString).Result).ConfigureAwait(false);
			var query = new QueryResponse(jsonResponse);
			var req = query.ValidRequest;
            return query.ValidRequest ? query : null;
		}

		/// <summary>
		/// Non-async version. Use for legacy code, use Async version wherever possible.
		/// </summary>
		/// <param name="queryString"></param>
		/// <returns>Returns null if the query is invalid.</returns>
		[Obsolete("Use Async version wherever possible.")]
		public QueryResponse Query(string queryString)
		{
			var x = Task.Run(async () => await QueryAsync(queryString).ConfigureAwait(false)).ConfigureAwait(false).GetAwaiter().GetResult();
			return x;
		}

		private bool _disposed;

		public void Dispose()
		{
			// Dispose of unmanaged resources.
			Dispose(true);
			// Suppress finalization.
			GC.SuppressFinalize(this);
		}
		protected virtual void Dispose(bool disposing)
		{
			if (_disposed)
			{
				return;
			}

			if (disposing)
			{
				// dispose managed state (managed objects).
			}

			// free unmanaged resources (unmanaged objects) and override a finalizer below.
			// set large fields to null.

			_httpClient.Dispose();

			_disposed = true;
		}
	}
}
