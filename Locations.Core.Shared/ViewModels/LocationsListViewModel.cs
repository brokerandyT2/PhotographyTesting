using Locations.Core.Shared.ViewModels.Interface;
using Locations.Core.Shared.ViewModelServices;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Locations.Core.Shared.ViewModels
{
    public class LocationsListViewModel : ViewModelBase, ILocationList
    {
        private readonly ILocationService _locationService;
        private ObservableCollection<LocationViewModel> _items = new ObservableCollection<LocationViewModel>();
        private bool _isLoading;
        private string _errorMessage;

        public ObservableCollection<LocationViewModel> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public ICommand LoadLocationsCommand { get; }
        public ICommand OpenMapCommand { get; }
        public ICommand SelectLocationCommand { get; }

        public List<LocationViewModel> locations => throw new NotImplementedException();

        // Define the error event using the correct type
        public event EventHandler<Locations.Core.Shared.ViewModels.OperationErrorEventArgs> ErrorOccurred;

        public LocationsListViewModel(ILocationService locationService)
        {
            _locationService = locationService ?? throw new ArgumentNullException(nameof(locationService));

            LoadLocationsCommand = new Command(async () => await LoadLocationsAsync());
            OpenMapCommand = new Command<int>(async (id) => await OpenMapAsync(id));
            SelectLocationCommand = new Command<LocationViewModel>(SelectLocation);
        }

        private async Task LoadLocationsAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;

                var result = await _locationService.GetLocationsAsync();

                Items.Clear();

                if (result.IsSuccess && result.Data != null)
                {
                    foreach (var location in result.Data)
                    {
                        Items.Add(location);
                    }
                }
                else
                {
                    ErrorMessage = result.ErrorMessage ?? "Failed to load locations";
                    OnErrorOccurred(new Locations.Core.Shared.ViewModels.OperationErrorEventArgs(
                        Locations.Core.Shared.ViewModels.OperationErrorSource.Service,
                        ErrorMessage
                        ));
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error loading locations: {ex.Message}";
                OnErrorOccurred(new Locations.Core.Shared.ViewModels.OperationErrorEventArgs(
                    Locations.Core.Shared.ViewModels.OperationErrorSource.Service,
                        ErrorMessage
                        ));
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task OpenMapAsync(int locationId)
        {
            try
            {
                ErrorMessage = string.Empty;
                IsLoading = true;

                var result = await _locationService.GetLocationAsync(locationId);

                if (!result.IsSuccess || result.Data == null)
                {
                    ErrorMessage = result.ErrorMessage ?? "Failed to retrieve location";
                    OnErrorOccurred(new Locations.Core.Shared.ViewModels.OperationErrorEventArgs(
                       Locations.Core.Shared.ViewModels.OperationErrorSource.Service,
                        ErrorMessage
                        ));
                    return;
                }

                var locationData = result.Data;

                // We'll use messaging to communicate with the view
                MessagingCenter.Send(this, "OpenMap", locationData);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error opening map: {ex.Message}";
                OnErrorOccurred(new Locations.Core.Shared.ViewModels.OperationErrorEventArgs(
 Locations.Core.Shared.ViewModels.OperationErrorSource.Service,
                        ErrorMessage
                        ));
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void SelectLocation(LocationViewModel location)
        {
            if (location == null) return;

            // Use messaging to handle navigation
            MessagingCenter.Send(this, "NavigateToLocation", location.Id);
        }

        // Use the correct event args type
        protected virtual void OnErrorOccurred(Locations.Core.Shared.ViewModels.OperationErrorEventArgs args)
        {
            ErrorOccurred?.Invoke(this, args);
        }
    }
}