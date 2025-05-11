using CommunityToolkit.Mvvm.ComponentModel;
using Locations.Core.Shared.DTO;
using Locations.Core.Shared.ViewModels.Interface;
using Locations.Core.Shared.ViewModelServices;
using System;

namespace Locations.Core.Shared.ViewModels
{
    public class DeviceInformation : ObservableObject, IDeviceInformation
    {
        // Event for error handling
        public event EventHandler<OperationErrorEventArgs>? ErrorOccurred;

        // Private backing fields
        private string _deviceId;
        private string _manufacturer;
        private string _model;
        private string _osVersion;
        private string _appVersion;
        private bool _isBusy;
        private bool _isError;
        private string _errorMessage = string.Empty;

        // Standardized properties
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public bool IsError
        {
            get => _isError;
            set => SetProperty(ref _isError, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                if (SetProperty(ref _errorMessage, value))
                {
                    IsError = !string.IsNullOrEmpty(value);
                }
            }
        }

        // Properties with notification
        public string DeviceId
        {
            get => _deviceId;
            set
            {
                _deviceId = value;
                OnPropertyChanged(nameof(DeviceId));
            }
        }

        public string Manufacturer
        {
            get => _manufacturer;
            set
            {
                _manufacturer = value;
                OnPropertyChanged(nameof(Manufacturer));
            }
        }

        public string Model
        {
            get => _model;
            set
            {
                _model = value;
                OnPropertyChanged(nameof(Model));
            }
        }

        public string OSVersion
        {
            get => _osVersion;
            set
            {
                _osVersion = value;
                OnPropertyChanged(nameof(OSVersion));
            }
        }

        public string AppVersion
        {
            get => _appVersion;
            set
            {
                _appVersion = value;
                OnPropertyChanged(nameof(AppVersion));
            }
        }

        // Default constructor
        public DeviceInformation()
        {
            // Initialize with default values
        }

        // Method to initialize from a DTO
        public void InitializeFromDTO(DeviceInfoDTO dto)
        {
            if (dto == null) return;

            try
            {
                // Copy properties from the DTO
                // Implementation depends on actual properties in DeviceInfoDTO
                DeviceId = dto.DeviceId;
                Manufacturer = dto.Manufacturer;
                Model = dto.Model;
                OSVersion = dto.OSVersion;
                AppVersion = dto.AppVersion;
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

        // Method to handle error bubbling
        protected virtual void OnErrorOccurred(OperationErrorEventArgs e)
        {
            ErrorOccurred?.Invoke(this, e);
        }
    }
}