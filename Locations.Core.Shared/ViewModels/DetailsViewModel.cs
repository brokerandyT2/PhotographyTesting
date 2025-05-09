using CommunityToolkit.Mvvm.Input;
using Locations.Core.Shared.ViewModels.Interface;
using Locations.Core.Shared.ViewModelServices;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Locations.Core.Shared.ViewModels
{
    public class DetailsViewModel : ViewModelBase, IDetailsView
    {
        // Private fields
        private LocationViewModel _locationViewModel;
        private WeatherViewModel _weatherViewModel;

        // Properties with proper notification
        public LocationViewModel LocationViewModel
        {
            get => _locationViewModel;
            set
            {
                _locationViewModel = value;
                OnPropertyChanged(nameof(LocationViewModel));
            }
        }

        public WeatherViewModel WeatherViewModel
        {
            get => _weatherViewModel;
            set
            {
                _weatherViewModel = value;
                OnPropertyChanged(nameof(WeatherViewModel));
            }
        }

        // Default constructor
        public DetailsViewModel() : base()
        {
            // Subscribe to error events from sub-viewmodels
        }

        // Constructor with dependencies
        public DetailsViewModel(LocationViewModel locationViewModel, WeatherViewModel weatherViewModel) : this()
        {
            LocationViewModel = locationViewModel;
            WeatherViewModel = weatherViewModel;

            // Subscribe to error events
            if (locationViewModel != null)
                locationViewModel.ErrorOccurred += OnSubViewModelErrorOccurred;

            if (weatherViewModel != null)
                weatherViewModel.ErrorOccurred += OnSubViewModelErrorOccurred;
        }

        // Handle errors from sub-viewmodels
        private void OnSubViewModelErrorOccurred(object sender, OperationErrorEventArgs e)
        {
            // Bubble up errors from child viewmodels
            VmErrorMessage = e.Message;
            OnErrorOccurred(e);
        }

        // Override LoadDataAsync to load data from sub-viewmodels
        protected override async Task LoadDataAsync()
        {
            try
            {
                await base.LoadDataAsync();

                // Parallel loading of sub-viewmodel data if available
                var tasks = new List<Task>();

                if (LocationViewModel != null)
                    tasks.Add(Task.Run(async () => await (LocationViewModel.RefreshCommand as AsyncRelayCommand).ExecuteAsync(null)));

                if (WeatherViewModel != null)
                    tasks.Add(Task.Run(async () => await (WeatherViewModel.RefreshCommand as AsyncRelayCommand).ExecuteAsync(null)));

                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                VmErrorMessage = $"Error refreshing details: {ex.Message}";
                OnErrorOccurred(new OperationErrorEventArgs(
                    OperationErrorSource.Unknown,
                    VmErrorMessage,
                    ex));
            }
        }

        // Cleanup method to unsubscribe from events
        public void Cleanup()
        {
            if (LocationViewModel != null)
                LocationViewModel.ErrorOccurred -= OnSubViewModelErrorOccurred;

            if (WeatherViewModel != null)
                WeatherViewModel.ErrorOccurred -= OnSubViewModelErrorOccurred;
        }
    }
}