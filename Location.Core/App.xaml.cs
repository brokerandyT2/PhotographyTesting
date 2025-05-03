using Locations.Core.Shared.Customizations.Alerts.Interfraces;
using Locations.Core.Shared.Customizations.Logging.Interfaces;
using Microsoft.Maui.Controls;
namespace Location.Core
{
    public partial class App : Microsoft.Maui.Controls.Application
    {
        public App()
        {
            InitializeComponent();
        }
        public App(IAlertService alertService)
        {
            InitializeComponent();

            MainPage = new MainPage(alertService); // Inject the alert service here
        }
        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new MainPage());
        }
    }
}