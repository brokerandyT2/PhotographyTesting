using System;
using System.Collections.ObjectModel;
using Locations.Core.Shared.ViewModels;

namespace Location.Photography.Shared.ViewModels.Interfaces
{
    public interface ISunLocation
    {
        // Properties
        DateTime SelectedDateTime { get; set; }
        DateTime SelectedDate { get; set; }
        TimeSpan SelectedTime { get; set; }
        double Latitude { get; set; }
        double Longitude { get; set; }
        double NorthRotationAngle { get; set; }
        double SunDirection { get; set; }
        double SunElevation { get; set; }
        double DeviceTilt { get; set; }
        bool ElevationMatched { get; set; }
        bool BeginMonitoring { get; set; }
        bool VmIsBusy { get; set; }
        string VmErrorMessage { get; set; }
        ObservableCollection<LocationViewModel> Locations { get; set; }

        // Methods
        void StartSensors();
        void StopSensors();

        // Events
        event EventHandler<OperationErrorEventArgs> ErrorOccurred;
    }
}