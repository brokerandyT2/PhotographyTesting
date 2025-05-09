using CommunityToolkit.Mvvm.Input;
using Locations.Core.Shared.ViewModels.Interface;
using Locations.Core.Shared.ViewModelServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Locations.Core.Shared.ViewModels
{
    public class LocationsListViewModel : ViewModelBase, ILocationList
    {
        // Private fields
        private ObservableCollection<LocationViewModel> _locationsCollection;
        private readonly ILocationService? _locationService;

        // Custom refresh command - instead of trying to override the base one
        private ICommand _refreshLocationsCommand;

        // Properties
        public ObservableCollection<LocationViewModel> LocationsCollection
        {
            get => _locationsCollection;
            set
            {
                _locationsCollection = value;
                OnPropertyChanged(nameof(LocationsCollection));
            }
        }

        // ILocationList implementation
        public List<LocationViewModel> locations => LocationsCollection.ToList();

        // Commands
        public ICommand AddLocationCommand { get; }
        public ICommand DeleteLocationCommand { get; }
        public ICommand RefreshLocationsCommand => _refreshLocationsCommand;

        // Default constructor
        public LocationsListViewModel()
        {
            LocationsCollection = new ObservableCollection<LocationViewModel>();

            // Initialize commands
            AddLocationCommand = new AsyncRelayCommand(AddLocationAsync, () => !VmIsBusy);
            DeleteLocationCommand = new AsyncRelayCommand<LocationViewModel>(DeleteLocationAsync, (location) => location != null && !VmIsBusy);

            // Create our own refresh command instead of trying to override the base one
            _refreshLocationsCommand = new AsyncRelayCommand(LoadDataAsync, () => !VmIsBusy);
        }

        // Constructor with DI
        public LocationsListViewModel(ILocationService locationService) : this()
        {
            _locationService = locationService;
        }

        // Load data method
        protected override async Task LoadDataAsync()
        {
            try
            {
                await base.LoadDataAsync();

                if (_locationService == null) return;

                // Load locations from service
                var result = await _locationService.GetLocationsAsync();

                if (result.IsSuccess && result.Data != null)
                {
                    LocationsCollection.Clear();
                    foreach (var location in result.Data)
                    {
                        // Assuming the service returns LocationViewModel or a compatible type
                        LocationsCollection.Add(location);

                        // Subscribe to each location's error events
                        location.ErrorOccurred += OnLocationErrorOccurred;
                    }
                }
                else
                {
                    VmErrorMessage = result.ErrorMessage ?? "Failed to load locations";
                    OnErrorOccurred(new OperationErrorEventArgs(
                        Locations.Core.Shared.ViewModels.OperationErrorSource.Unknown,
                        VmErrorMessage,
                        result.Exception));
                }
            }
            catch (Exception ex)
            {
                VmErrorMessage = $"Error loading locations: {ex.Message}";
                OnErrorOccurred(new OperationErrorEventArgs(
                    Locations.Core.Shared.ViewModels.OperationErrorSource.Unknown,
                    VmErrorMessage,
                    ex));
            }
        }

        // Add a new location
        private async Task AddLocationAsync()
        {
            try
            {
                VmIsBusy = true;
                VmErrorMessage = string.Empty;

                if (_locationService == null) return;

                // Create a new location
                var newLocation = new LocationViewModel();

                // Add to collection first for immediate UI feedback
                LocationsCollection.Add(newLocation);

                // Subscribe to error events
                newLocation.ErrorOccurred += OnLocationErrorOccurred;

                // In a real app, you might navigate to an edit screen here
            }
            catch (Exception ex)
            {
                VmErrorMessage = $"Error adding location: {ex.Message}";
                OnErrorOccurred(new OperationErrorEventArgs(
                    Locations.Core.Shared.ViewModels.OperationErrorSource.Unknown,
                    VmErrorMessage,
                    ex));
            }
            finally
            {
                VmIsBusy = false;
            }
        }

        // Delete a location
        private async Task DeleteLocationAsync(LocationViewModel location)
        {
            try
            {
                if (location == null || _locationService == null) return;

                VmIsBusy = true;
                VmErrorMessage = string.Empty;

                // Call the service to delete the location
                var result = await _locationService.DeleteLocationAsync(location.Id);

                if (result.IsSuccess)
                {
                    // Remove from our collection
                    LocationsCollection.Remove(location);

                    // Unsubscribe from error events
                    location.ErrorOccurred -= OnLocationErrorOccurred;
                }
                else
                {
                    VmErrorMessage = result.ErrorMessage ?? "Failed to delete location";
                    OnErrorOccurred(new OperationErrorEventArgs(
                        Locations.Core.Shared.ViewModels.OperationErrorSource.Unknown,
                        VmErrorMessage,
                        result.Exception));
                }
            }
            catch (Exception ex)
            {
                VmErrorMessage = $"Error deleting location: {ex.Message}";
                OnErrorOccurred(new OperationErrorEventArgs(
                    Locations.Core.Shared.ViewModels.OperationErrorSource.Unknown,
                    VmErrorMessage,
                    ex));
            }
            finally
            {
                VmIsBusy = false;
            }
        }

        // Handle errors from locations
        private void OnLocationErrorOccurred(object sender, OperationErrorEventArgs e)
        {
            // Bubble up errors from locations
            VmErrorMessage = e.Message;
            OnErrorOccurred(e);
        }

        // Cleanup method
        public void Cleanup()
        {
            // Unsubscribe from all location error events
            foreach (var location in LocationsCollection)
            {
                location.ErrorOccurred -= OnLocationErrorOccurred;
            }
        }
    }
}