﻿using Location.Core.Helpers.AlertService;
using Location.Core.Helpers.LoggingService;
using Location.Core.Views;
using Locations.Core.Business.DataAccess.Services;
using Locations.Core.Business.StorageSvc;
using Locations.Core.Data.Queries;
using Locations.Core.Shared;
using Locations.Core.Shared.ViewModels;
using Newtonsoft.Json;
using System.Globalization;
using static Locations.Core.Shared.Enums.SubscriptionType;
using EncryptedSQLite;
using Locations.Core.Shared.ViewModelServices;
#if PHOTOGRAPHY

#endif
namespace Location.Core
{
    public partial class MainPage : TabbedPage
    {
        private static SettingsService<SettingViewModel> ss = new SettingsService<SettingViewModel>(
            new SettingsRepository(
                new EventAlertService(), new LoggerService(DataEncrypted.GetAsyncConnection())), new EventAlertService(), new LoggerService(DataEncrypted.GetAsyncConnection()));
#if DEBUG
        private static SubscriptionTypeEnum _subType = SubscriptionTypeEnum.Free;
#else
    private static SubscriptionTypeEnum _subType;
#endif
        public static bool IsLoggedIn { get; set; } = false;

        internal static string EmailAddress;
        internal static string AppID;

        public static SubscriptionTypeEnum SubscriptionType = _subType;
        public INativeStorageService nativeService;
        private ISettingService _settingService;
        private ILoggerService _loggerService;
       
        public MainPage()
        {
            InitializeComponent();

            // Load email address and determine login status
            EmailAddress = NativeStorageService.GetSetting(MagicStrings.Email);
            IsLoggedIn = !string.IsNullOrEmpty(EmailAddress);
            AppID = NativeStorageService.GetSetting(MagicStrings.UniqueID);
            MainThread.BeginInvokeOnMainThread(() =>
            {
                RequestPermissionsAsync();
            });

            if (!IsLoggedIn)
            {
                // Use Device.InvokeOnMainThreadAsync for immediate execution
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PushAsync(new NavigationPage(new Login()));
                });

                // Return early to prevent the rest of the constructor from executing
                return;
            }
            // Get subscription type
            try
            {
                Enum.TryParse(ss.GetSettingByName(MagicStrings.SubscriptionType).Value, out _subType);
            }
            catch
            {
                _subType = SubscriptionTypeEnum.Free;
            }

            //  DataAccess da = new DataAccess();

            if(_settingService == null)
            {
                IAlertService _alertService = new EventAlertService();
                ILoggerService _loggerService = new LoggerService(DataEncrypted.GetAsyncConnection(), _alertService);
               
            }

            var _alertServ = new Location.Core.Helpers.AlertService.EventAlertService();
            var _loggingServ = new Location.Core.Helpers.LoggingService.LoggerService(DataEncrypted.GetAsyncConnection());

            var _tipRepo = new Locations.Core.Data.Queries.TipRepository(_alertServ, _loggingServ);
            var _locRepo = new LocationRepository(_alertServ, _loggingServ);
            var _tipTypeRepo = new TipTypeRepository(_alertServ, _loggingServ);
            var _settingRepo = new SettingsRepository(_alertServ, _loggingServ);

            var _tipBusiness = new TipService<TipViewModel>(_tipRepo, _alertServ, _loggingServ);
            var _tipTypeBusiness = new TipTypeService<TipTypeViewModel>(_tipTypeRepo, _alertServ, _loggingServ);
            var _locBusiness = new LocationService<LocationViewModel>(_locRepo, _alertServ, _loggingServ);
            var _settingsBusiness = new SettingsService<SettingViewModel>(_settingRepo, _alertServ, _loggingServ);
            // Increment the app open counter
            //var z = ss.GetSettingByName(MagicStrings.AppOpenCounter);
            var z = _settingsBusiness.GetSettingByName(MagicStrings.AppOpenCounter);

            var x = Convert.ToInt32(z.Value);
            z.Value = (x + 1).ToString();

            var q = _settingService.SaveSettingAsync(z);

            var language = CultureInfo.CurrentCulture.Name;
            var languageStored = _settingService.GetSettingAsync(MagicStrings.DefaultLanguage);
            var adSupport = Convert.ToBoolean(_settingService.GetSettingAsync(MagicStrings.FreePremiumAdSupported).Result.Data.Value);

            var subscription = SubscriptionType;









            // Make sure we have saved the systems language
            if (languageStored.Result.Data.Value != language)
            {
                languageStored.Result.Data.Value = language;
                var setvm = new SettingViewModel();

                _settingsBusiness.SaveAsync(languageStored.Result.Data);
            }

            // Capture the device information and save it to the settings
            var settingDi = _settingService.GetSettingAsync(MagicStrings.DeviceInformation);
            var deviceInfo = new Locations.Core.Shared.ViewModels.DeviceInformation();
            var serilized = JsonConvert.SerializeObject(deviceInfo);
            if (JsonConvert.SerializeObject(settingDi.Result.Data) != serilized)
            {
                settingDi.Result.Data.Value = serilized;
                _settingsBusiness.SaveAsync(settingDi.Result.Data);
            }

            if (IsLoggedIn)
            {
                AddDefault();

                if ((subscription == SubscriptionTypeEnum.Professional || subscription == SubscriptionTypeEnum.Premium) || adSupport)
                {
#if PHOTOGRAPHY
                    /*  this.Children.Add(new Views.Pro.SceneEvaluation());
                      this.Children.Add(new Views.Pro.SunCalculations());*/
                    this.Children.Add(new Location.Photography.Pro.SceneEvaluation());
                    this.Children.Add(new Photography.Pro.SunCalculations());
#endif
                    if ((subscription == SubscriptionTypeEnum.Premium) || adSupport)
                    {
#if PHOTOGRAPHY
                        this.Children.Add(new Photography.Premium.ExposureCalculator());
                        this.Children.Add(new Photography.Premium.LightMeter());
                        this.Children.Add(new Photography.Premium.SunLocation());
                        /*   this.Children.Add(new Views.Premium.ExposureCalculator());
                           this.Children.Add(new Views.Premium.LightMeter());
                           this.Children.Add(new Views.Premium.SunLocation());*/
#endif
                    }
                }
            }
            else
            {
                AddDefault();
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PushModalAsync(new Login());
                });
            }
            this.Children.Add(new Location.Core.Views.Settings());
        }

        private void RequestPermissionsAsync()
        {
            // Make sure we're already on the main thread


            // Now we're safely on the main thread
            Task.Run(async () =>
            {
                try
                {
                    var locationStatus = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                    var cameraStatus = await Permissions.RequestAsync<Permissions.Camera>();
                }
                catch (Exception ex)
                {
                    _loggerService.LogError($"Permission request error: {ex.Message}", ex);
                }
            });
        }
        public MainPage(INativeStorageService nativeService, ISettingService settingService, ILoggerService loggerService) : this()
        {
            this.nativeService = nativeService;
            this._settingService = settingService;
            this._loggerService = loggerService;
        }



        private void AddDefault()
        {
            this.Children.Add(new Location.Core.Views.AddLocation());
            this.Children.Add(new Location.Core.Views.ListLocations());
            this.Children.Add(new Location.Core.Views.Tips());
            this.Children.Add(new Location.Photography.Premium.LightMeter());
        }
    }
}