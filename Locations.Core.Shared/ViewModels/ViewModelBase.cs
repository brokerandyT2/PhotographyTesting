using Locations.Core.Shared.ViewModels.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Locations.Core.Shared.ViewModels
{
    public abstract class ViewModelBase : IDTOBase
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public ICommand RefreshCommand { get; private set; }

        public ViewModelBase()
        {
            RefreshCommand = new Command(async () => {
                IsRefreshing = true;
                // Add your refresh logic here
                await LoadDataAsync(); // Example: Load updated data
                IsRefreshing = false;
            });
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

        private async Task LoadDataAsync()
        {
            // Simulate loading data
            await Task.Delay(2000);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
