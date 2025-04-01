using Locations.Core.Shared.ViewModels;

using Nominatim.API.Geocoders;
using Nominatim.API.Interfaces;
using Nominatim.API.Models;
using Nominatim.API.Web;
namespace Locations.Core.Business.GeoLocation
{
    public class GeoLocationAPI
    {
        private readonly HttpClient _httpClient;
        private readonly ReverseGeocoder _reverseGeocoder;
        LocationViewModel _locationViewModel;
        public GeoLocationAPI() {
        throw new NotImplementedException();
        }
        public GeoLocationAPI( ref LocationViewModel location)
        {
            _locationViewModel = location;
            var httpClientFactory = new SimpleHttpClientFactory();
            var nominatimWebInterface = new NominatimWebInterface(httpClientFactory);
            _reverseGeocoder = new ReverseGeocoder(nominatimWebInterface);
        }
        public LocationViewModel GetCityAndState(double latitude, double longitude)
        {
            try
            {
                var result =  _reverseGeocoder.ReverseGeocode(new ReverseGeocodeRequest
                {
                    Latitude = latitude,
                    Longitude = longitude,
                    ZoomLevel =10
                   
                }).Result;

                if (result?.Address != null)
                {
                    string city = result.Address.City ?? result.Address.Town ?? result.Address.Village ?? "Unknown City";
                    string state = result.Address.State ?? "Unknown State";
                    _locationViewModel.City = city;
                    _locationViewModel.State = state;
                    return _locationViewModel;
                }

                return new LocationViewModel() ;
            }
            catch (Exception ex)
            {
                return new LocationViewModel();
            }
        }

    }
    public class SimpleHttpClientFactory : IHttpClientFactory
    {
        private readonly HttpClient _httpClient;

        public SimpleHttpClientFactory()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://nominatim.openstreetmap.org/") // Required for Nominatim API
            };
        }

        public HttpClient CreateClient(string name) => _httpClient;
    }
}
