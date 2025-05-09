using CommunityToolkit.Mvvm.ComponentModel;
using Locations.Core.Shared.DTO;
using Locations.Core.Shared.ViewModels.Interface;
using Locations.Core.Shared.ViewModelServices;
using System;

namespace Locations.Core.Shared.ViewModels
{
    public class TipTypeViewModel : ObservableObject, ITipType
    {
        // Private fields
        private int _id;
        private string _name;
        private string _description;
        private int _displayOrder;
        private string _icon;

        // Event for error handling
        public event EventHandler<OperationErrorEventArgs>? ErrorOccurred;

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

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
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

        public int DisplayOrder
        {
            get => _displayOrder;
            set
            {
                _displayOrder = value;
                OnPropertyChanged(nameof(DisplayOrder));
            }
        }

        public string Icon
        {
            get => _icon;
            set
            {
                _icon = value;
                OnPropertyChanged(nameof(Icon));
            }
        }

        // Default constructor
        public TipTypeViewModel()
        {
            _name = string.Empty;
            _description = string.Empty;
            _icon = string.Empty;
        }

        // Constructor with initial values
        public TipTypeViewModel(string name, string description, int displayOrder = 0, string icon = "") : this()
        {
            Name = name;
            Description = description;
            DisplayOrder = displayOrder;
            Icon = icon;
        }

        // Initialize from a DTO
        public void InitializeFromDTO(TipTypeDTO dto)
        {
            if (dto == null) return;

            try
            {
                Id = dto.Id;
                Name = dto.Name;
                Description = dto.Description;
                DisplayOrder = dto.DisplayOrder;
                Icon = dto.Icon;
            }
            catch (Exception ex)
            {
                OnErrorOccurred(new OperationErrorEventArgs(
                    OperationErrorSource.Unknown,
                    $"Error initializing from DTO: {ex.Message}",
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