


using Locations.Core.Business;
using System.Globalization;
using Locations.Core.Shared;
using Locations.Core.Business.DataAccess;
using Location.Core.Views;
using static Locations.Core.Shared.Enums.SubscriptionType;
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
#if PHOTOGRAPHY
#if RELEASE
                    return ss.GetSettingByName(MagicStrings.Email).Value != string.Empty ? true : false;
#else
                    return true;
#endif
#endif


                }
                catch
                {
                    //I know swallowing an Exception is wrong.  In this case the exception occurs on start up due to the setting not being available.
                    return false;
                }
                
            }
        }

        //private static bool st = Enum.TryParse(ss.GetSettingByName(MagicStrings.SubscriptionType).Value, out _subType);

        public MainPage()
        {
            InitializeComponent();
            DataAccess da = new DataAccess();

            this.Children.Add(new AddLocation());
            this.Children.Add(new ListLocations());
            this.Children.Add(new Tips());
            SettingsService ss = new SettingsService();
            var z = ss.GetSetting(MagicStrings.AppOpenCounter);
            var x = Convert.ToInt32(z.Value);
            z.Value = (x + 1).ToString();
            var q = ss.SaveSettingWithObjectReturn(z);

            var language = CultureInfo.CurrentCulture.Name;
            var setting =  ss.GetSettingByName(MagicStrings.DefaultLanguage);
            //var devInfo = ss.GetSettingByName(MagicStrings.DeviceInfo);

            if (setting.Value != language)
            {
                setting.Value = language;
                var dispose = ss.Save(setting);
            }
            if (IsLoggedIn)
            {
                if (_subType == SubscriptionTypeEnum.Professional || _subType == SubscriptionTypeEnum.Premium)
                {
#if PHOTOGRAPHY

#endif
                    if (_subType == SubscriptionTypeEnum.Premium)
                    {
#if PHOTOGRAPHY

#endif
                    }
                }
            }


        }


    }

}
