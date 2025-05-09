using CommunityToolkit.Mvvm.ComponentModel;
using Locations.Core.Shared.DTO;
using Locations.Core.Shared.ViewModels.Interface;
using Locations.Core.Shared.ViewModelServices;
using System;

namespace Locations.Core.Shared.ViewModels
{
    public class TipViewModel : ObservableObject, ITip
    {
        // Private fields
        private int _id;
        private string _tip;
        private int _tipTypeId;
        private string _tipTypeName;
        private string _description;
        private bool _isDeleted;

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

        public string Tip
        {
            get => _tip;
            set
            {
                _tip = value;
                OnPropertyChanged(nameof(Tip));
            }
        }

        public int TipTypeId
        {
            get => _tipTypeId;
            set
            {
                _tipTypeId = value;
                OnPropertyChanged(nameof(TipTypeId));
            }
        }

        public string TipTypeName
        {
            get => _tipTypeName;
            set
            {
                _tipTypeName = value;
                OnPropertyChanged(nameof(TipTypeName));
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

        public bool IsDeleted
        {
            get => _isDeleted;
            set
            {
                _isDeleted = value;
                OnPropertyChanged(nameof(IsDeleted));
            }
        }

        // Default constructor
        public TipViewModel()
        {
            _tip = string.Empty;
            _tipTypeName = string.Empty;
            _description = string.Empty;
        }

        // Initialize from a DTO
        public void InitializeFromDTO(TipDTO dto)
        {
            if (dto == null) return;

            try
            {
                Id = dto.Id;
                Tip = dto.Tip;
                TipTypeId = dto.TipTypeId;
                TipTypeName = dto.TipTypeName;
                Description = dto.Description;
                IsDeleted = dto.IsDeleted;
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