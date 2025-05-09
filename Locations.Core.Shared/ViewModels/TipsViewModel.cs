using CommunityToolkit.Mvvm.Input;
using Locations.Core.Shared.ViewModels.Interface;
using Locations.Core.Shared.ViewModelServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Locations.Core.Shared.ViewModels
{
    public class TipsViewModel : ViewModelBase, ITipsViewmodel
    {
        // Private fields
        private ObservableCollection<TipTypeViewModel> _tipTypes;
        private TipViewModel _selectedTip;
        private readonly ITipService? _tipService;

        // Events
        public new event PropertyChangedEventHandler? PropertyChanged;

        // Properties with notification
        public ObservableCollection<TipTypeViewModel> TipTypes
        {
            get => _tipTypes;
            set
            {
                _tipTypes = value;
                OnPropertyChanged(nameof(TipTypes));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TipTypes)));
            }
        }

        public TipViewModel SelectedTip
        {
            get => _selectedTip;
            set
            {
                _selectedTip = value;
                OnPropertyChanged(nameof(SelectedTip));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedTip)));
            }
        }

        // Commands
        public ICommand SelectTipCommand { get; }
        public ICommand RefreshTipsCommand { get; }

        // Default constructor
        public TipsViewModel() : base()
        {
            _tipTypes = new ObservableCollection<TipTypeViewModel>();
            _selectedTip = new TipViewModel();

            // Initialize commands
            SelectTipCommand = new RelayCommand<TipViewModel>(SelectTip);
            RefreshTipsCommand = new AsyncRelayCommand(LoadDataAsync, () => !VmIsBusy);
        }

        // Constructor with DI
        public TipsViewModel(ITipService tipService) : this()
        {
            _tipService = tipService;
        }

        // Override LoadDataAsync to load tips
        protected override async Task LoadDataAsync()
        {
            try
            {
                await base.LoadDataAsync();

                if (_tipService == null) return;

                // Load tip types from service
                var result = await _tipService.GetTipTypesAsync();

                if (result.IsSuccess && result.Data != null)
                {
                    TipTypes.Clear();
                    foreach (var tipType in result.Data)
                    {
                        var tipTypeViewModel = new TipTypeViewModel();
                        tipTypeViewModel.InitializeFromDTO(tipType);
                        tipTypeViewModel.ErrorOccurred += OnTipTypeErrorOccurred;
                        TipTypes.Add(tipTypeViewModel);
                    }

                    // Load initial tip if any types are available
                    if (TipTypes.Count > 0)
                    {
                        await LoadTipForTypeAsync(TipTypes[0]);
                    }
                }
                else
                {
                    VmErrorMessage = result.ErrorMessage ?? "Failed to load tip types";
                    OnErrorOccurred(new OperationErrorEventArgs(
                        Locations.Core.Shared.ViewModels.OperationErrorSource.Unknown,
                        VmErrorMessage,
                        result.Exception));
                }
            }
            catch (Exception ex)
            {
                VmErrorMessage = $"Error loading tips: {ex.Message}";
                OnErrorOccurred(new OperationErrorEventArgs(
                    Locations.Core.Shared.ViewModels.OperationErrorSource.Unknown,
                    VmErrorMessage,
                    ex));
            }
        }

        // Load a tip for a specific type
        private async Task LoadTipForTypeAsync(TipTypeViewModel tipType)
        {
            try
            {
                if (_tipService == null || tipType == null) return;

                VmIsBusy = true;

                var result = await _tipService.GetRandomTipForTypeAsync(tipType.Id);

                if (result.IsSuccess && result.Data != null)
                {
                    var tipViewModel = new TipViewModel();
                    tipViewModel.InitializeFromDTO(result.Data);
                    tipViewModel.ErrorOccurred += OnTipErrorOccurred;

                    // Unsubscribe from previous tip if exists
                    if (SelectedTip != null)
                    {
                        SelectedTip.ErrorOccurred -= OnTipErrorOccurred;
                    }

                    SelectedTip = tipViewModel;
                }
                else
                {
                    VmErrorMessage = result.ErrorMessage ?? $"Failed to load tip for type: {tipType.Name}";
                    OnErrorOccurred(new OperationErrorEventArgs(
                        Locations.Core.Shared.ViewModels.OperationErrorSource.Unknown,
                        VmErrorMessage,
                        result.Exception));
                }
            }
            catch (Exception ex)
            {
                VmErrorMessage = $"Error loading tip: {ex.Message}";
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

        // Select a tip
        private void SelectTip(TipViewModel tip)
        {
            if (tip == null) return;

            // Unsubscribe from previous tip if exists
            if (SelectedTip != null)
            {
                SelectedTip.ErrorOccurred -= OnTipErrorOccurred;
            }

            // Subscribe to new tip
            tip.ErrorOccurred += OnTipErrorOccurred;

            SelectedTip = tip;
        }

        // Handle errors from tip types
        private void OnTipTypeErrorOccurred(object sender, OperationErrorEventArgs e)
        {
            // Bubble up errors from tip types
            VmErrorMessage = e.Message;
            OnErrorOccurred(e);
        }

        // Handle errors from selected tip
        private void OnTipErrorOccurred(object sender, OperationErrorEventArgs e)
        {
            // Bubble up errors from tip
            VmErrorMessage = e.Message;
            OnErrorOccurred(e);
        }

        // Cleanup method
        public void Cleanup()
        {
            // Unsubscribe from all tip type error events
            foreach (var tipType in TipTypes)
            {
                tipType.ErrorOccurred -= OnTipTypeErrorOccurred;
            }

            // Unsubscribe from selected tip error events
            if (SelectedTip != null)
            {
                SelectedTip.ErrorOccurred -= OnTipErrorOccurred;
            }
        }
    }
}