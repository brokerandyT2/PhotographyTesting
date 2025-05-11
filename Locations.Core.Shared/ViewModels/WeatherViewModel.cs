using CommunityToolkit.Mvvm.Input;
using Locations.Core.Shared.DTO;
using Locations.Core.Shared.ViewModels.Interface;
using Locations.Core.Shared.ViewModelServices;
using Microsoft.Maui.Devices.Sensors;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Locations.Core.Shared.ViewModels
{
    public class WeatherViewModel : WeatherDTO, IWeatherViewModel
    {
        // Services
        private readonly IWeatherService? _weatherService;

        // Standardized properties
        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
            }
        }

        private bool _isError;
        public bool IsError
        {
            get => _isError;
            set
            {
                _isError = value;
                OnPropertyChanged(nameof(IsError));
            }
        }

        private string _errorMessage = string.Empty;
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
                IsError = !string.IsNullOrEmpty(value);
            }
        }

        // Commands
        public ICommand RefreshWeatherCommand { get; }

        // Event for error handling
        public event EventHandler<OperationErrorEventArgs>? ErrorOccurred;

        // Default constructor
        public WeatherViewModel() : base()
        {
            // Initialize commands
            RefreshWeatherCommand = new AsyncRelayCommand(RefreshWeatherAsync, () => !IsBusy);
        }

        // Constructor with DI
        public WeatherViewModel(IWeatherService weatherService) : this()
        {
            _weatherService = weatherService;
        }
        protected virtual void OnErrorOccurred(Locations.Core.Shared.ViewModels.OperationErrorEventArgs args)
        {
            ErrorOccurred?.Invoke(this, args);
        }
        // Initialize from a DTO
        public void InitializeFromDTO(WeatherDTO dto)
        {
            if (dto == null) return;

            try
            {
                // Copy properties from the DTO
                Id = dto.Id;
                LocationId = dto.LocationId;
                Timestamp = dto.Timestamp;
                Temperature = dto.Temperature;
                Description = dto.Description;
                WindSpeed = dto.WindSpeed;
                WindDirection = dto.WindDirection;
                Humidity = dto.Humidity;
                Pressure = dto.Pressure;
                Visibility = dto.Visibility;
                Precipitation = dto.Precipitation;
                SunriseTime = dto.SunriseTime;
                SunsetTime = dto.SunsetTime;
                CloudCover = dto.CloudCover;
                FeelsLike = dto.FeelsLike;
                UVIndex = dto.UVIndex;
                AirQuality = dto.AirQuality;
                LastUpdate = dto.LastUpdate;
                Forecast = dto.Forecast;
                IsDeleted = dto.IsDeleted;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error initializing from DTO: {ex.Message}";
                OnErrorOccurred(new OperationErrorEventArgs(
                    OperationErrorSource.Unknown,
                    ErrorMessage,
                    ex));
            }
        }

        // Refresh weather data
        private async Task RefreshWeatherAsync()
        {
            if (_weatherService == null) return;

            try
            {
                IsBusy = true;
                ErrorMessage = string.Empty;

                // Get the location ID to refresh weather for
                var locationId = LocationId;

                // Check if we have a valid location ID
                if (locationId <= 0)
                {
                    ErrorMessage = "Invalid location ID. Cannot refresh weather.";
                    OnErrorOccurred(new OperationErrorEventArgs(
                        OperationErrorSource.InvalidArgument,
                        ErrorMessage));
                    return;
                }

                // Call the weather service to fetch updated weather data
                dynamic result = new object();// await _weatherService.GetWeatherForLocationAsync(locationId);

                if (result.IsSuccess && result.Data != null)
                {
                    // Update properties from the fetched data
                    InitializeFromDTO(result.Data);

                    // Update the last update timestamp
                    LastUpdate = DateTime.Now;
                }
                else
                {
                    ErrorMessage = result.ErrorMessage ?? "Failed to fetch weather data";
                    OnErrorOccurred(new OperationErrorEventArgs(
                        result.Source,
                        ErrorMessage,
                        result.Exception));
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error refreshing weather: {ex.Message}";
                OnErrorOccurred(new OperationErrorEventArgs(
                    OperationErrorSource.Unknown,
                    ErrorMessage,
                    ex));
            }
            finally
            {
                IsBusy = false;
            }
        }

        // Fetch forecast for the location
        public async Task FetchForecastAsync(int days = 5)
        {
            if (_weatherService == null) return;

            try
            {
                IsBusy = true;
                ErrorMessage = string.Empty;

                // Get the location ID to fetch forecast for
                var locationId = LocationId;

                // Check if we have a valid location ID
                // Check if we have a valid location ID
                if (locationId <= 0)
                {
                    ErrorMessage = "Invalid location ID. Cannot fetch forecast.";
                    OnErrorOccurred(new OperationErrorEventArgs(
                        OperationErrorSource.InvalidArgument,
                        ErrorMessage));
                    return;
                }

                // Call the weather service to fetch forecast data
                dynamic result = new object();//await _weatherService.GetForecastForLocationAsync(locationId, days);

                if (result.IsSuccess && result.Data != null)
                {
                    // Update forecast property
                    Forecast = result.Data;
                }
                else
                {
                    ErrorMessage = result.ErrorMessage ?? "Failed to fetch forecast data";
                    OnErrorOccurred(new OperationErrorEventArgs(
                        result.Source,
                        ErrorMessage,
                        result.Exception));
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error fetching forecast: {ex.Message}";
                OnErrorOccurred(new OperationErrorEventArgs(
                    OperationErrorSource.Unknown,
                    ErrorMessage,
                    ex));
            }
            finally
            {
                IsBusy = false;
            }
        }

        // Method to handle error bubbling
      

        // Helper methods for converting and formatting weather data

        public string GetFormattedTemperature(bool isFahrenheit = true)
        {
            if (isFahrenheit)
            {
                return $"{Temperature} °F";
            }
            else
            {
                // Convert to Celsius
                return $"{Temperature} °C";
            }
        }

        public string GetFormattedWindSpeed(bool isImperial = true)
        {
            if (isImperial)
            {
                return $"{WindSpeed} mph";
            }
            else
            {
                // Convert to km/h
                return $"{WindSpeed} kph";
            }
        }

        public string GetFormattedSunriseTime(string timeFormat = "h:mm tt")
        {
            return SunriseTime.ToString(timeFormat);
        }

        public string GetFormattedSunsetTime(string timeFormat = "h:mm tt")
        {
            return SunsetTime.ToString(timeFormat);
        }
    }
}