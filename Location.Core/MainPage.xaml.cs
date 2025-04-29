using System.Globalization;
using Locations.Core.Shared;
using Locations.Core.Business.DataAccess;
using Location.Core.Views;
using static Locations.Core.Shared.Enums.SubscriptionType;
using Newtonsoft.Json;
using static Locations.Core.Shared.Enums.Hemisphere;
#if PHOTOGRAPHY

#endif
namespace Location.Core
{
    public partial class MainPage : TabbedPage
    {

        private static Locations.Core.Business.DataAccess.SettingsService ss = new SettingsService();
        private static Locations.Core.Shared.Enums.SubscriptionType.SubscriptionTypeEnum _subType;

        public static bool IsLoggedIn
        {
            get
            {
                try
                {
#if ANDY
                    return false;
#else
                    return ss.GetSettingByName(MagicStrings.Email).Value != string.Empty ? true : false;
#endif
                }
                catch
                {
                    //I know swallowing an Exception is wrong.  In this case the exception occurs on start up due to the setting not being available.
                    return false;
                }
            }
        }
        public static SubscriptionTypeEnum SubscriptionType
        {
            get
            {
                try
                {
#if ANDY
                    return SubscriptionTypeEnum.Premium;

#else
                    Enum.TryParse(ss.GetSettingByName(MagicStrings.SubscriptionType).Value, out _subType);
                    return _subType;
#endif
                }
                catch
                {
                    //I know swallowing an Exception is wrong.  In this case the exception occurs on start up due to the setting not being available.
                    return SubscriptionTypeEnum.Free;
                }
            }
        }

        public MainPage()
        {

            DataAccess da;
            InitializeComponent();
            Task.Run(async () =>
                {
                    Dispatcher.Dispatch(() => da = new DataAccess());
                }
            );

           

            PermissionStatus ps = Permissions.RequestAsync<Permissions.Camera>().Result;
            PermissionStatus pss = Permissions.RequestAsync<Permissions.LocationWhenInUse>().Result;
            SettingsService ss = new SettingsService();

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
                        //this.Children.Add(new Views.Premium.LightMeter());
                        this.Children.Add(new Views.Premium.SunLocation());
#endif
                    }
                }
            }
            else
            {
                AddDefault();
                Navigation.PushModalAsync(new Login());

            }
            this.Children.Add(new Settings());


        }

        private void AddDefault()
        {
            this.Children.Add(new AddLocation());
            this.Children.Add(new ListLocations());
            this.Children.Add(new Tips());
        }
    }

}
