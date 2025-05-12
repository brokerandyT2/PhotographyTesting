
using Location.Core.Helpers.AlertService;
using Location.Core.Helpers.LoggingService;
using Locations.Core.Business.DataAccess.Services;
using Locations.Core.Business.StorageSvc;
using Locations.Core.Data.Queries;
using Locations.Core.Shared.ViewModelServices;
using Microsoft.Extensions.DependencyInjection;

namespace Location.Core
{
    public partial class App : Microsoft.Maui.Controls.Application
    {
        private  IServiceProvider _services;

        public App()
        {
            SQLitePCL.Batteries_V2.Init();
            InitializeComponent();
            

            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                try
                {
                    _services = new ServiceCollection() as IServiceProvider;
                    var locationService = _services.GetService(typeof(Location.Core.Helpers.LoggingService.ILoggerService)) as LoggerService;
                    locationService?.LogError("Unhandled exception", args.ExceptionObject as Exception);
                }
                catch
                {
                    // Avoid throwing inside global exception handler
                }
            };
        }


        protected override Window CreateWindow(IActivationState? activationState)
        {
            //MainPage = new NavigationPage(new MainPage());
            return new Window(new NavigationPage(new MainPage()));
        }
    }
}