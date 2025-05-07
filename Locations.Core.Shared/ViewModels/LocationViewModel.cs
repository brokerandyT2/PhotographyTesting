using CommunityToolkit.Mvvm.ComponentModel;
using Locations.Core.Shared.Alerts.Implementation;
using Locations.Core.Shared.DTO;
using Locations.Core.Shared.ViewModels.Interface;
using System.Windows.Input;
//using Locations.Core.Business.DataAccess;
namespace Locations.Core.Shared.ViewModels
{

    public partial class LocationViewModel : LocationDTO, ILocationViewModel
    {
        public event EventHandler<AlertEventArgs> AlertRaised;
        public ICommand _takePhotoCommand;

        public LocationViewModel() {
            _takePhotoCommand = new Command(TakePhoto);
            AlertTitle = string.Empty;
            AlertMessage = string.Empty;
            StartGPS();
            AlertRaised += this.LocationViewModel_RaiseAlert1; 

        }

        private void LocationViewModel_RaiseAlert1(object? sender, AlertEventArgs e)
        {
            AlertTitle = e.Title;
            AlertMessage = e.Message;

            AlertRaised?.Invoke(this, new AlertEventArgs(e.Title, e.Message));
        }

        private void LocationViewModel_RaiseAlert(object? sender, AlertEventArgs e)
        {
            AlertTitle = e.Title;
            AlertMessage = e.Message;

            AlertRaised?.Invoke(this, new AlertEventArgs(e.Title, e.Message));
        }

        [ObservableProperty]
        private string alertTitle;

        [ObservableProperty]
        private string alertMessage;


        private async void StartGPS()
        {
            if (!Geolocation.IsListeningForeground)
            {
                var request = new GeolocationListeningRequest(GeolocationAccuracy.Medium);
                var success = await Geolocation.StartListeningForegroundAsync(request);
            }
            Geolocation.LocationChanged += Geolocation_LocationChanged;

        }

        private void Geolocation_LocationChanged(object? sender, GeolocationLocationChangedEventArgs e)
        {
            Lattitude = Math.Round(e.Location.Latitude, 4);
            Longitude = Math.Round(e.Location.Longitude,4);
        }

        public void TakePhoto(object sender) {
            if (MediaPicker.Default.IsCaptureSupported)
            {
                FileResult photo = MediaPicker.Default.CapturePhotoAsync().Result;

                if (photo != null)
                {
                    // save the file into local storage
                    string localFilePath = Path.Combine(Microsoft.Maui.Storage.FileSystem.AppDataDirectory, photo.FileName);

                    using Stream sourceStream = photo.OpenReadAsync().Result;
                    using FileStream localFileStream = File.OpenWrite(localFilePath);

                    sourceStream.CopyToAsync(localFileStream).RunSynchronously();
                     Photo= localFilePath;
                }
                else
                {                     // Handle the case when the user cancels the photo capture
                    AlertEventArgs args = new AlertEventArgs("Error", "Photo Not Taken");
                    OnAlertRaised("Error", "Photo Not Taken");
                    // For example, you can show an alert or log a message
                }
            }
        }

    }
}

