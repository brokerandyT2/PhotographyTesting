using Locations.Core.Shared.Customizations.Alerts.Implementation;
using Locations.Core.Shared.Customizations.Alerts.Interfraces;
using Locations.Core.Shared.Customizations.Logging.Interfaces;
using Microsoft.Maui.Controls;
using SkiaSharp;
namespace Location.Core
{
    public partial class App : Microsoft.Maui.Controls.Application
    {
        private IAlertService _alertService;
        private ILoggerService _logger;
        public App()
        {
            InitializeComponent();

        }
        public App(IAlertService alertService) : this()
        {

            _alertService = alertService;
            // Inject the alert service here
        }
        public App(IAlertService alertService, ILoggerService logger) : this(alertService)
        {
            _logger = logger;

        }
        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new MainPage());
        }
    }
}