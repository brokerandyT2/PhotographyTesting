
using Locations.Core.Business.StorageSvc;

namespace Location.Core
{
    public partial class App : Microsoft.Maui.Controls.Application
    {


        public App()
        {
            InitializeComponent();

        }

       
        protected override Window CreateWindow(IActivationState? activationState)
        {
            //MainPage = new NavigationPage(new MainPage());
            return new Window(new NavigationPage(new MainPage(new NativeStorageService())));
        }
    }
}