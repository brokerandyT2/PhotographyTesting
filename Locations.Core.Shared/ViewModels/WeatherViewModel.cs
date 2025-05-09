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

        // Private fields for UI state
        private bool _vmIsBusy;
        private string _vmErrorMessage = string.Empty;

        // UI state properties with notification
        public bool VmIsBusy
        {
            get => _vmIsBusy;
            set
            {
                _vmIsBusy = value;
                OnPropertyChanged(nameof(VmIsBusy));
            }
        }

        public string VmErrorMessage
        {
            get => _vmErrorMessage;
            set
            {
                _vmErrorMessage = value;
                OnPropertyChanged(nameof(VmErrorMessage));
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
            RefreshWeatherCommand = new AsyncRelayCommand(RefreshWeatherAsync, () => !VmIsBusy);
        }

        // Constructor with DI
        public WeatherViewModel(IWeatherService weatherService) : this()
        {
            _weatherService = weatherService;
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
                VmErrorMessage = $"Error initializing from DTO: {ex.Message}";
                OnErrorOccurred(new OperationErrorEventArgs(
                    OperationErrorSource.Unknown,
                    VmErrorMessage,
                    ex));
            }
        }

        // Refresh weather data
        private async Task RefreshWeatherAsync()
        {
            if (_weatherService == null) return;

            try
            {
                VmIsBusy = true;
                VmErrorMessage = string.Empty;

                // Get the location ID to refresh weather for
                var locationId = LocationId;

                // Check if we have a valid location ID
                if (locationId <= 0)
                {
                    VmErrorMessage = "Invalid location ID. Cannot refresh weather.";
                    OnErrorOccurred(new OperationErrorEventArgs(
                        OperationErrorSource.InvalidArgument,
                        VmErrorMessage));
                    return;
                }

                // Call the weather service to fetch updated weather data
                var result = await _weatherService.GetWeatherForLocationAsync(locationId);

                if (result.IsSuccess && result.Data != null)
                {
                    // Update properties from the fetched data
                    InitializeFromDTO(result.Data);

                    // Update the last update timestamp
                    LastUpdate = DateTime.Now;
                }
                else
                {
                    VmErrorMessage = result.ErrorMessage ?? "Failed to fetch weather data";
                    OnErrorOccurred(new OperationErrorEventArgs(
                        result.Source,
                        VmErrorMessage,
                        result.Exception));
                }
            }
            catch (Exception ex)
            {
                VmErrorMessage = $"Error refreshing weather: {ex.Message}";
                OnErrorOccurred(new OperationErrorEventArgs(
                    OperationErrorSource.Unknown,
                    VmErrorMessage,
                    ex));
            }
            finally
            {
                VmIsBusy = false;
            }
        }

        // Fetch forecast for the location
        public async Task FetchForecastAsync(int days = 5)
        {
            if (_weatherService == null) return;

            try
            {
                VmIsBusy = true;
                VmErrorMessage = string.Empty;

                // Get the location ID to fetch forecast for
                var locationId = LocationId;

                // Check if we have a valid location ID
                if (locationId <= 0)
                {
                    VmErrorMessage = "Invalid location ID. Cannot fetch forecast.";
                    OnErrorOccurred(new OperationErrorEventArgs(
                        OperationErrorSource.InvalidArgument,
                        VmErrorMessage));
                    return;
                }

                // Call the weather service to fetch forecast data
                var result = await _weatherService.GetForecastForLocationAsync(locationId, days);

                if (result.IsSuccess && result.Data != null)
                {
                    // Update forecast property
                    Forecast = result.Data;
                }
                else
                {
                    VmErrorMessage = result.ErrorMessage ?? "Failed to fetch forecast data";
                    OnErrorOccurred(new OperationErrorEventArgs(
                        result.Source,
                        VmErrorMessage,
                        result.Exception));
                }
            }
            catch (Exception ex)
            {
                VmErrorMessage = $"Error fetching forecast: {ex.Message}";
                OnErrorOccurred(new OperationErrorEventArgs(
                    OperationErrorSource.Unknown,
                    VmErrorMessage,
                    ex));
            }
            finally
            {
                VmIsBusy = false;
            }
        }

        // Method to handle error bubbling
        protected virtual void OnErrorOccurred(OperationErrorEventArgs e)
        {
            ErrorOccurred?.Invoke(this, e);
        }

        // Helper methods for converting and formatting weather data

        public string GetFormattedTemperature(bool isFahrenheit = true)
        {
            if (isFahrenheit)
            {
                return $"{Temperature}°F";
            }
            else
            {
                // Convert to Celsius
                var celsius = (Temperature - 32) * 5 / 9;
                return $"{Math.Round(celsius, 1)}°C";
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
                var kmh = WindSpeed * 1.60934;
                return $"{Math.Round(kmh, 1)} km/h";
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