using CommunityToolkit.Mvvm.ComponentModel;
using Locations.Core.Shared.Alerts.Implementation;
using Locations.Core.Shared.DTO.Interfaces;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Locations.Core.Shared.DTO
{
    public partial class DTOBase : ObservableObject, IDTOBase
    {
        public event EventHandler<AlertEventArgs> RaiseAlert;
        public bool IsError { get; set; } = false;
        public AlertEventArgs alertEventArgs;
        public ICommand RefreshCommand { get; private set; }

        public DTOBase()
        {
            RefreshCommand = new Command(async () => {
                IsRefreshing = true;
                // Add your refresh logic here
                // Example: Load updated data
                IsRefreshing = false;
            });
            this.RaiseAlert += OnAlertRaised;
        }

       

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                _isRefreshing = value;
                OnPropertyChanged();
            }
        }

        AlertEventArgs IDTOBase.alertEventArgs => throw new NotImplementedException();

        private async Task LoadDataAsync()
        {
            // Simulate loading data
            await Task.Delay(2000);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
       

        private void OnAlertRaised(object? sender, AlertEventArgs e)
        {
            alertEventArgs = e;
            RaiseAlert?.Invoke(this, e);
            IsError = true;
        }

        protected virtual void OnAlertRaised(string title, string message)
        {
            alertEventArgs = new AlertEventArgs
            {
                Title = title,
                Message = message,
                IsError = true
            };
            RaiseAlert?.Invoke(this, alertEventArgs);
            IsError = true;
        }
    }
}
