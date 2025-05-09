using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Locations.Core.Shared.ViewModels.Interface;
using Locations.Core.Shared.ViewModelServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Locations.Core.Shared.ViewModels
{
    public abstract class ViewModelBase : ObservableObject, IDTOBase
    {
        // UI state properties
        private bool _vmIsBusy;
        public bool VmIsBusy
        {
            get => _vmIsBusy;
            set
            {
                _vmIsBusy = value;
                OnPropertyChanged(nameof(VmIsBusy));
            }
        }

        private string _vmErrorMessage = string.Empty;
        public string VmErrorMessage
        {
            get => _vmErrorMessage;
            set
            {
                _vmErrorMessage = value;
                OnPropertyChanged(nameof(VmErrorMessage));
            }
        }

        // Event for error handling
        public event EventHandler<OperationErrorEventArgs>? ErrorOccurred;

        // Commands
        public ICommand RefreshCommand { get; }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        protected ViewModelBase()
        {
            RefreshCommand = new AsyncRelayCommand(LoadDataAsync, () => !VmIsBusy);
        }

        protected virtual async Task LoadDataAsync()
        {
            try
            {
                VmIsBusy = true;
                VmErrorMessage = string.Empty;
                IsRefreshing = true;

                // Actual data loading should be implemented in derived classes
                await Task.Delay(100);
            }
            catch (Exception ex)
            {
                VmErrorMessage = $"Error loading data: {ex.Message}";
                OnErrorOccurred(new OperationErrorEventArgs(
                    OperationErrorSource.Unknown,
                    VmErrorMessage,
                    ex));
            }
            finally
            {
                VmIsBusy = false;
                IsRefreshing = false;
            }
        }

        // Method to handle error bubbling
        protected virtual void OnErrorOccurred(OperationErrorEventArgs e)
        {
            ErrorOccurred?.Invoke(this, e);
        }
    }
}