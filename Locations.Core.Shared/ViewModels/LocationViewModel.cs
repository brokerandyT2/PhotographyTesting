using Locations.Core.Shared.DTO;
using Locations.Core.Shared.ViewModels.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
//using Locations.Core.Business.DataAccess;
namespace Locations.Core.Shared.ViewModels
{

    public class LocationViewModel : LocationDTO, ILocationViewModel
    {

        public ICommand _takePhotoCommand;

        public LocationViewModel() {
            _takePhotoCommand = new Command(TakePhoto);
            StartGPS();
        }
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
            Lattitude = e.Location.Latitude;
            Longitude = e.Location.Longitude;
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
            }
        }

    }
}

