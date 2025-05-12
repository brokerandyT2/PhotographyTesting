using CommunityToolkit.Mvvm.ComponentModel;
using Locations.Core.Shared.DTO;
using Locations.Core.Shared.ViewModels.Interface;
using System;

namespace Locations.Core.Shared.ViewModels
{
    public class SettingViewModel : ObservableObject, ISetting
    {
        // Private backing fields
        private int _id;
        private string _key;
        private string _value;
        private string _description;
        
        // Properties with notification
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        public string Key
        {
            get => _key;
            set
            {
                _key = value;
                OnPropertyChanged(nameof(Key));
            }
        }

        public string Value
        {
            get => _value;
            set
            {
                _value = value;
                OnPropertyChanged(nameof(Value));
                // Also notify any derived properties
                OnPropertyChanged(nameof(BooleanValue));
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        // Computed property for boolean settings
        public bool BooleanValue
        {
            get => Convert.ToBoolean(Value);
            set
            {
                Value = value.ToString();
                OnPropertyChanged(nameof(BooleanValue));
            }
        }

        // Event for error handling
        public event EventHandler<OperationErrorEventArgs>? ErrorOccurred;

        // Default constructor
        public SettingViewModel()
        {
            _key = string.Empty;
            _value = string.Empty;
            _description = string.Empty;
        }

        // Constructor with initial values
        public SettingViewModel(string key, string value, string description = "") : this()
        {
            Key = key;
            Value = value;
            Description = description;
        }

        // Method to initialize from a DTO
        public void InitializeFromDTO(SettingDTO dto)
        {
            if (dto == null) return;

            try
            {
                Id = dto.Id;
                Key = dto.Key;
                Value = dto.Value;
                Description = dto.Description;
            }
            catch (Exception ex)
            {
                OnErrorOccurred(new OperationErrorEventArgs(
                    Locations.Core.Shared.ViewModels.OperationErrorSource.Unknown,
                    $"Error initializing setting from DTO: {ex.Message}",
                    ex));
            }
        }

        // Convert to DTO
        public SettingDTO ToDTO()
        {
            return new SettingDTO
            {
                Id = this.Id,
                Key = this.Key,
                Value = this.Value,
                Description = this.Description
            };
        }

        // Helper method for converting to boolean
        public bool ToBoolean()
        {
            return Convert.ToBoolean(Value);
        }

        // Method to handle error bubbling
        protected virtual void OnErrorOccurred(OperationErrorEventArgs e)
        {
            ErrorOccurred?.Invoke(this, e);
        }
    }
}