using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Locations.Core.Shared.DTO;
using Locations.Core.Shared.ViewModels.Interface;
using Locations.Core.Shared.ViewModelServices;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Locations.Core.Shared.ViewModels
{
    public partial class WeatherViewModel : WeatherDTO, IWeatherViewModel
    {
        // Observable properties
        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private string _errorMessage = string.Empty;

        // Commands
        public ICommand RefreshWeatherCommand { get; }

        // Event for errors
        public event EventHandler<OperationErrorEventArgs>? ErrorOccurred;

        /// <summary>
        /// Constructor for design-time and when created by SQLite
        /// </summary>
        public WeatherViewModel() : base()
        {
            // Initialize commands
            RefreshWeatherCommand = new AsyncRelayCommand(RefreshWeatherAsync);
        }

        /// <summary>
        /// Refresh weather data
        /// </summary>
        private async Task RefreshWeatherAsync()
        {
            try
            {
                IsBusy = true;
                ErrorMessage = string.Empty;

                // Implementation would typically call a weather service API
                await Task.Delay(1000); // Simulate network request

                // Update weather properties with the fetched data
                LastUpdate = DateTime.Now;
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

        /// <summary>
        /// Raise error event
        /// </summary>
        protected virtual void OnErrorOccurred(OperationErrorEventArgs e)
        {
            ErrorOccurred?.Invoke(this, e);
        }
    }
}