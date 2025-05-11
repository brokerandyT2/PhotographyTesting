using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Location.Photography.Shared.ViewModels
{
    public partial class ViewModelBase : ObservableObject
    {
        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private bool isError;

        private string _errorMessage = string.Empty;

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                if (SetProperty(ref _errorMessage, value))
                {
                    // Directly update IsError property when ErrorMessage changes
                    IsError = !string.IsNullOrEmpty(value);
                }
            }
        }

        protected ViewModelBase()
        {
        }
    }
}