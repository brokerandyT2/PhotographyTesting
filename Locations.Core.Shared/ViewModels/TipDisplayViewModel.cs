using CommunityToolkit.Mvvm.ComponentModel;
using Locations.Core.Shared.DTO;
using Locations.Core.Shared.DTO.Interfaces;
using Locations.Core.Shared.ViewModels.Interface;
using Locations.Core.Shared.ViewModelServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Locations.Core.Shared.ViewModels
{
    public class TipDisplayViewModel : ObservableObject, ITipDisplayViewModel
    {
        // Private fields
        private int _id;
        private string _title;
        private string _description;
        private ObservableCollection<TipTypeViewModel> _displays;

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

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
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

        public ObservableCollection<TipTypeViewModel> Displays
        {
            get => _displays;
            set
            {
                _displays = value;
                OnPropertyChanged(nameof(Displays));
            }
        }

        // Default constructor
        public TipDisplayViewModel()
        {
            _title = string.Empty;
            _description = string.Empty;
            _displays = new ObservableCollection<TipTypeViewModel>();
        }

        // Constructor with initial values
        public TipDisplayViewModel(string title, string description) : this()
        {
            Title = title;
            Description = description;
        }

        // Initialize from a DTO
        public void InitializeFromDTO(TipsDisplayDTO dto)
        {
            if (dto == null) return;

            try
            {
                Id = dto.Id;
                Title = dto.Title;
                Description = dto.Content;

                Displays.Clear();
                if (dto.Tips != null)
                {
                    foreach (var typeDto in dto.Tips)
                    {
                        var typeViewModel = new TipTypeViewModel();
                        //typeViewModel.InitializeFromDTO(typeDto);
                        typeViewModel.ErrorOccurred += OnTipTypeErrorOccurred;
                        Displays.Add(typeViewModel);
                    }
                }
            }
            catch (Exception ex)
            {
                OnErrorOccurred(new OperationErrorEventArgs(
                    OperationErrorSource.Unknown,
                    $"Error initializing from DTO: {ex.Message}",
                    ex));
            }
        }

        // Handle errors from tip types
        private void OnTipTypeErrorOccurred(object sender, OperationErrorEventArgs e)
        {
            // Bubble up errors from tip types
            OnErrorOccurred(e);
        }

        // Method to handle error bubbling
        protected virtual void OnErrorOccurred(OperationErrorEventArgs e)
        {
            ErrorOccurred?.Invoke(this, e);
        }

        // Cleanup method
        public void Cleanup()
        {
            // Unsubscribe from all tip type error events
            foreach (var tipType in Displays)
            {
                tipType.ErrorOccurred -= OnTipTypeErrorOccurred;
            }
        }
    }
}