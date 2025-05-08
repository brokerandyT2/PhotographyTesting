using Location.Core.Views;
using Locations.Core.Business.DataAccess;
using Locations.Core.Business.StorageSvc;
using Locations.Core.Shared;
using Locations.Core.Shared.Customizations.Alerts.Interfraces;
using Newtonsoft.Json;
using System.Globalization;
using static Locations.Core.Shared.Enums.SubscriptionType;
#if PHOTOGRAPHY

#endif
namespace Location.Core
{
    public partial class MainPage : TabbedPage
    {

        private static SettingsService ss = new SettingsService();
#if DEBUG
        private static SubscriptionTypeEnum _subType = SubscriptionTypeEnum.Premium;
#else
        private static SubscriptionTypeEnum _subType;
#endif
        public static bool IsLoggedIn { get; set; } = false;

        internal static string EmailAddress;
        internal static string AppID;

        public static SubscriptionTypeEnum SubscriptionType = _subType;
        internal IAlertService alertService;

        public INativeStorageService nativeService;

        public MainPage(INativeStorageService nativeService)
        {


            EmailAddress = NativeStorageService.GetSetting(MagicStrings.Email);
            IsLoggedIn = !string.IsNullOrEmpty(EmailAddress);
            AppID = NativeStorageService.GetSetting(MagicStrings.UniqueID);
            if (!IsLoggedIn)
            {
                Navigation.PushAsync(new NavigationPage(new Login()));
            }
            
        }

        public MainPage()
        {
            SettingsService ss = new SettingsService();

            try
            {
                Enum.TryParse(ss.GetSettingByName(MagicStrings.SubscriptionType).Value, out _subType);
            }
            catch
            {

                _subType = SubscriptionTypeEnum.Free;
            }


            InitializeComponent();
            DataAccess da = new DataAccess();

            InitAsync();

            //Increment the app open counter
            var z = ss.GetSetting(MagicStrings.AppOpenCounter);
            var x = Convert.ToInt32(z.Value);
            z.Value = (x + 1).ToString();
            var q = ss.Save(z);

            var language = CultureInfo.CurrentCulture.Name;
            var setting = ss.GetSettingByName(MagicStrings.DefaultLanguage);
            var adSupport = Convert.ToBoolean(ss.GetSettingByName(MagicStrings.FreePremiumAdSupported).Value);

            var subscription = SubscriptionType;

            //Make sure we have saved the systems language
            if (setting.Value != language)
            {
                setting.Value = language;
                var dispose = ss.Save(setting);
            }


            //Capture the device information and save it to the settings.
            var settingDi = ss.GetSettingByName(MagicStrings.DeviceInformation);
            var deviceInfo = new Locations.Core.Shared.ViewModels.DeviceInformation();
            var serilized = JsonConvert.SerializeObject(deviceInfo);
            if (settingDi.Value != serilized)
            {
                settingDi.Value = serilized;
                ss.Save(settingDi);
            }

            //IsLoggedIn = we have an email address (bare minimum currently)
            if (IsLoggedIn)
            {
                AddDefault();

                if ((subscription == SubscriptionTypeEnum.Professional || subscription == SubscriptionTypeEnum.Premium) || adSupport)
                {
#if PHOTOGRAPHY
                    this.Children.Add(new Views.Pro.SceneEvaluation());
                    this.Children.Add(new Views.Pro.SunCalculations());

#endif
                    if ((subscription == SubscriptionTypeEnum.Premium) || adSupport)
                    {
#if PHOTOGRAPHY
                        this.Children.Add(new Views.Premium.ExposureCalculator());
                        this.Children.Add(new Views.Premium.LightMeter());
                        this.Children.Add(new Views.Premium.SunLocation());
#endif
                    }
                }
            }
            else
            {
                AddDefault();
                Navigation.PushModalAsync(new Login());
                //Navigation.PushAsync(new NavigationPage(new Login()));


            }
            this.Children.Add(new Location.Core.Views.Settings());


        }

        public MainPage(IAlertService alertService) : this()
        {
            this.alertService = alertService;
        }

        private async void InitAsync()
        {
            Dispatcher.Dispatch(async () =>
            {
                var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                var statustwo = await Permissions.RequestAsync<Permissions.Camera>();
            });

        }

        private void AddDefault()
        {
            this.Children.Add(new Location.Core.Views.AddLocation());
            this.Children.Add(new Location.Core.Views.ListLocations());
            this.Children.Add(new Location.Core.Views.Tips());

        }
    }

}
