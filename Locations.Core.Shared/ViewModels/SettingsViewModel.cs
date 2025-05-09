using CommunityToolkit.Mvvm.Input;
using Locations.Core.Shared.DTO;
using Locations.Core.Shared.Enums;
using Locations.Core.Shared.ViewModels.Interface;
using Locations.Core.Shared.ViewModelServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using static Locations.Core.Shared.Enums.Hemisphere;

namespace Locations.Core.Shared.ViewModels
{
    public class SettingsViewModel : ViewModelBase, ISettingsViewModel
    {
        // Private member for property change notification
        private event PropertyChangedEventHandler? _propertyChanged;

        // Services
        //private readonly ISettingsService? _settingsService;

        // Commands
        public ICommand SaveSettingsCommand { get; }
        public ICommand ResetSettingsCommand { get; }

        #region Properties
        // General settings
        private SettingViewModel _hemisphere;
        public SettingViewModel Hemisphere
        {
            get => _hemisphere;
            set
            {
                _hemisphere = value;
                OnPropertyChanged(nameof(Hemisphere));
                OnPropertyChanged(nameof(HemisphereNorth));
                _propertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Hemisphere)));
            }
        }

        public bool HemisphereNorth => Hemisphere?.Value == HemisphereChoices.North.Name();

        public SettingViewModel FirstName { get; set; }
        public SettingViewModel LastName { get; set; }
        public SettingViewModel Email { get; set; }

        private SettingViewModel _subscriptionExpiration;
        public SettingViewModel SubscriptionExpiration
        {
            get => _subscriptionExpiration;
            set
            {
                _subscriptionExpiration = value;
                OnPropertyChanged(nameof(SubscriptionExpiration));
                _propertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SubscriptionExpiration)));
            }
        }

        private SettingViewModel _subscriptionType;
        public SettingViewModel SubscriptionType
        {
            get => _subscriptionType;
            set
            {
                _subscriptionType = value;
                OnPropertyChanged(nameof(SubscriptionType));
                _propertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SubscriptionType)));
            }
        }

        public SettingViewModel UniqeID { get; set; }
        public SettingViewModel DeviceInfo { get; set; }

        // Formatting settings
        private SettingViewModel _timeFormat;
        public SettingViewModel TimeFormat
        {
            get => _timeFormat;
            set
            {
                _timeFormat = value;
                OnPropertyChanged(nameof(TimeFormat));
                OnPropertyChanged(nameof(TimeFormatToggle));
                _propertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TimeFormat)));
            }
        }

        public bool TimeFormatToggle => TimeFormat?.Value == MagicStrings.USTimeformat_Pattern;

        private SettingViewModel _dateFormat;
        public SettingViewModel DateFormat
        {
            get => _dateFormat;
            set
            {
                _dateFormat = value;
                OnPropertyChanged(nameof(DateFormat));
                OnPropertyChanged(nameof(DateFormatToggle));
                _propertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DateFormat)));
            }
        }

        public bool DateFormatToggle => DateFormat?.Value == MagicStrings.USDateFormat;

        // Wind direction setting
        public SettingViewModel WindDirection { get; set; }
        public bool WindDirectionBoolean => WindDirection?.Value == MagicStrings.TowardsWind;

        // View tracking settings
        public SettingViewModel HomePageViewed { get; set; }
        public bool HomePageViewedBool => HomePageViewed?.Value == MagicStrings.True_string;

        public SettingViewModel ListLocationsViewed { get; set; }
        public bool ListLocationsViewedBool => ListLocationsViewed?.Value == MagicStrings.True_string;

        public SettingViewModel TipsViewed { get; set; }
        public bool TipsViewedBool => TipsViewed?.Value == MagicStrings.True_string;

        public SettingViewModel ExposureCalcViewed { get; set; }
        public bool ExposureCalcViewedBool => ExposureCalcViewed?.Value == MagicStrings.True_string;

        public SettingViewModel LightMeterViewed { get; set; }
        public bool LightMeterViewedBool => LightMeterViewed?.Value == MagicStrings.True_string;

        public SettingViewModel SceneEvaluationViewed { get; set; }
        public bool SceneEvaluationViewedBool => SceneEvaluationViewed?.Value == MagicStrings.True_string;

        public SettingViewModel SunCalculationViewed { get; set; }
        public bool SunCalculationViewedBool => SunCalculationViewed?.Value == MagicStrings.True_string;

        public SettingViewModel LastBulkWeatherUpdate { get; set; }

        // Language setting
        private SettingViewModel _language;
        public SettingViewModel Language
        {
            get => _language;
            set
            {
                _language = value;
                OnPropertyChanged(nameof(Language));
                _propertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Language)));
            }
        }

        // Ad support setting
        private SettingViewModel _adSupport;
        public SettingViewModel AdSupport
        {
            get => _adSupport;
            set
            {
                _adSupport = value;
                OnPropertyChanged(nameof(AdSupport));
                OnPropertyChanged(nameof(AdSupportboolean));
                _propertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AdSupport)));
            }
        }

        public bool AdSupportboolean => AdSupport?.Value == MagicStrings.True_string;

        // Temperature setting
        public SettingViewModel TemperatureFormat { get; set; }
        public bool TemperatureFormatToggle => TemperatureFormat?.Value == MagicStrings.Fahrenheit;
        #endregion

        // Default constructor
        public SettingsViewModel() : base()
        {
            // Initialize properties with default values
            InitializeDefaultSettings();

            // Commands
            SaveSettingsCommand = new AsyncRelayCommand(SaveSettingsAsync, () => !VmIsBusy);
            ResetSettingsCommand = new AsyncRelayCommand(ResetSettingsAsync, () => !VmIsBusy);
        }

        // Constructor with DI
       /* public SettingsViewModel(ISettingsService settingsService) : this()
        {
            _settingsService = settingsService;
        }*/

        // Initialize default settings
        private void InitializeDefaultSettings()
        {
            // Create default settings with empty values
            Hemisphere = new SettingViewModel("Hemisphere", HemisphereChoices.North.Name());
            FirstName = new SettingViewModel("FirstName", string.Empty);
            LastName = new SettingViewModel("LastName", string.Empty);
            Email = new SettingViewModel("Email", string.Empty);
            SubscriptionExpiration = new SettingViewModel("SubscriptionExpiration", DateTime.Now.AddYears(1).ToString());
            SubscriptionType = new SettingViewModel("SubscriptionType", "Free");
            UniqeID = new SettingViewModel("UniqueID", Guid.NewGuid().ToString());
            DeviceInfo = new SettingViewModel("DeviceInfo", string.Empty);
            TimeFormat = new SettingViewModel("TimeFormat", MagicStrings.USTimeformat_Pattern);
            DateFormat = new SettingViewModel("DateFormat", MagicStrings.USDateFormat);
            WindDirection = new SettingViewModel("WindDirection", MagicStrings.TowardsWind);
            HomePageViewed = new SettingViewModel("HomePageViewed", MagicStrings.False_string);
            ListLocationsViewed = new SettingViewModel("ListLocationsViewed", MagicStrings.False_string);
            TipsViewed = new SettingViewModel("TipsViewed", MagicStrings.False_string);
            ExposureCalcViewed = new SettingViewModel("ExposureCalcViewed", MagicStrings.False_string);
            LightMeterViewed = new SettingViewModel("LightMeterViewed", MagicStrings.False_string);
            SceneEvaluationViewed = new SettingViewModel("SceneEvaluationViewed", MagicStrings.False_string);
            SunCalculationViewed = new SettingViewModel("SunCalculationViewed", MagicStrings.False_string);
            LastBulkWeatherUpdate = new SettingViewModel("LastBulkWeatherUpdate", DateTime.Now.AddDays(-1).ToString());
            Language = new SettingViewModel("Language", "en-US");
            AdSupport = new SettingViewModel("AdSupport", MagicStrings.True_string);
            TemperatureFormat = new SettingViewModel("TemperatureFormat", MagicStrings.Fahrenheit);

            // Subscribe to error events from setting properties
            SubscribeToSettingEvents();
        }

        // Subscribe to error events from all settings
        private void SubscribeToSettingEvents()
        {
            // Get all properties that are SettingViewModel type
            var properties = GetType().GetProperties();

            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(SettingViewModel))
                {
                    var setting = property.GetValue(this) as SettingViewModel;
                    if (setting != null)
                    {
                        setting.ErrorOccurred += OnSettingErrorOccurred;
                    }
                }
            }
        }

        // Handle errors from settings
        private void OnSettingErrorOccurred(object sender, OperationErrorEventArgs e)
        {
            // Bubble up errors from settings
            VmErrorMessage = e.Message;
            OnErrorOccurred(e);
        }

        // Override base load data method
        protected override async Task LoadDataAsync()
        {
            try
            {
                await base.LoadDataAsync();

                //if (_settingsService == null) return;

                // Load settings from service
                dynamic result = new object();//  await _settingsService.GetSettingsAsync();

                if (result.IsSuccess && result.Data != null)
                {
                    // Update from loaded settings using a different approach
                    UpdateFromSettingsDTO(result.Data);
                }
                else
                {
                    VmErrorMessage = result.ErrorMessage ?? "Failed to load settings";
                    OnErrorOccurred(new OperationErrorEventArgs(
                        Locations.Core.Shared.ViewModels.OperationErrorSource.Unknown,
                        VmErrorMessage,
                        result.Exception));
                }
            }
            catch (Exception ex)
            {
                VmErrorMessage = $"Error loading settings: {ex.Message}";
                OnErrorOccurred(new OperationErrorEventArgs(
                    Locations.Core.Shared.ViewModels.OperationErrorSource.Unknown,
                    VmErrorMessage,
                    ex));
            }
        }

        // Update properties from DTO using a different approach to avoid ambiguity
        public void UpdateFromSettingsDTO(object settingsDTO)
        {
            // This is a workaround method to avoid ambiguity with SettingDTO.Value
            // We use reflection to handle the properties instead of direct access

            var dtoType = settingsDTO.GetType();

            // Helper function to update a setting if it exists in the DTO
            void UpdateSetting(SettingViewModel viewModelSetting, string dtoPropertyName)
            {
                var dtoProperty = dtoType.GetProperty(dtoPropertyName);
                if (dtoProperty != null)
                {
                    var dtoSettingValue = dtoProperty.GetValue(settingsDTO);
                    if (dtoSettingValue != null)
                    {
                        // Get the key, value, and description using reflection
                        var keyProperty = dtoSettingValue.GetType().GetProperty("Key");
                        var valueProperty = dtoSettingValue.GetType().GetProperty("Value");
                        var descriptionProperty = dtoSettingValue.GetType().GetProperty("Description");

                        if (keyProperty != null && valueProperty != null)
                        {
                            var key = keyProperty.GetValue(dtoSettingValue)?.ToString() ?? "";
                            var value = valueProperty.GetValue(dtoSettingValue)?.ToString() ?? "";
                            var description = descriptionProperty?.GetValue(dtoSettingValue)?.ToString() ?? "";

                            // Update the view model setting
                            viewModelSetting.Key = key;
                            viewModelSetting.Value = value;
                            viewModelSetting.Description = description;
                        }
                    }
                }
            }

            // Update each setting using the helper function
            UpdateSetting(Hemisphere, "Hemisphere");
            UpdateSetting(FirstName, "FirstName");
            UpdateSetting(LastName, "LastName");
            UpdateSetting(Email, "Email");
            UpdateSetting(SubscriptionExpiration, "SubscriptionExpiration");
            UpdateSetting(SubscriptionType, "SubscriptionType");
            UpdateSetting(UniqeID, "UniqeID");
            UpdateSetting(DeviceInfo, "DeviceInfo");
            UpdateSetting(TimeFormat, "TimeFormat");
            UpdateSetting(DateFormat, "DateFormat");
            UpdateSetting(WindDirection, "WindDirection");
            UpdateSetting(HomePageViewed, "HomePageViewed");
            UpdateSetting(ListLocationsViewed, "ListLocationsViewed");
            UpdateSetting(TipsViewed, "TipsViewed");
            UpdateSetting(ExposureCalcViewed, "ExposureCalcViewed");
            UpdateSetting(LightMeterViewed, "LightMeterViewed");
            UpdateSetting(SceneEvaluationViewed, "SceneEvaluationViewed");
            UpdateSetting(SunCalculationViewed, "SunCalculationViewed");
            UpdateSetting(LastBulkWeatherUpdate, "LastBulkWeatherUpdate");
            UpdateSetting(Language, "Language");
            UpdateSetting(AdSupport, "AdSupport");
            UpdateSetting(TemperatureFormat, "TemperatureFormat");
        }

        // Save settings
        private async Task SaveSettingsAsync()
        {
            try
            {
                VmIsBusy = true;
                VmErrorMessage = string.Empty;

                //if (_settingsService == null) return;

                // Create a manual conversion from ViewModel to DTO
                // This avoids direct property access which was causing ambiguity errors
                var settingsDTO = new DTO.SettingsDTO();

                // Helper method to create a SettingDTO without directly accessing .Value property
                DTO.SettingDTO CreateSettingDTO(string key, string value, string description = "")
                {
                    var dto = new DTO.SettingDTO();
                    dto.Key = key;
                    // Use reflection to set the Value property to avoid ambiguity
                    var valueProperty = typeof(DTO.SettingDTO).GetProperty("Value");
                    valueProperty?.SetValue(dto, value);
                    dto.Description = description;
                    return dto;
                }

                // Manually set each property to avoid ambiguity issues
                var hemisphereProperty = typeof(DTO.SettingsDTO).GetProperty("Hemisphere");
                hemisphereProperty?.SetValue(settingsDTO,
                    CreateSettingDTO(Hemisphere.Key, Hemisphere.Value, Hemisphere.Description));

                var firstNameProperty = typeof(DTO.SettingsDTO).GetProperty("FirstName");
                firstNameProperty?.SetValue(settingsDTO,
                    CreateSettingDTO(FirstName.Key, FirstName.Value, FirstName.Description));

                var lastNameProperty = typeof(DTO.SettingsDTO).GetProperty("LastName");
                lastNameProperty?.SetValue(settingsDTO,
                    CreateSettingDTO(LastName.Key, LastName.Value, LastName.Description));

                var emailProperty = typeof(DTO.SettingsDTO).GetProperty("Email");
                emailProperty?.SetValue(settingsDTO,
                    CreateSettingDTO(Email.Key, Email.Value, Email.Description));

                var subscriptionExpirationProperty = typeof(DTO.SettingsDTO).GetProperty("SubscriptionExpiration");
                subscriptionExpirationProperty?.SetValue(settingsDTO,
                    CreateSettingDTO(SubscriptionExpiration.Key, SubscriptionExpiration.Value, SubscriptionExpiration.Description));

                var subscriptionTypeProperty = typeof(DTO.SettingsDTO).GetProperty("SubscriptionType");
                subscriptionTypeProperty?.SetValue(settingsDTO,
                    CreateSettingDTO(SubscriptionType.Key, SubscriptionType.Value, SubscriptionType.Description));

                var uniqueIDProperty = typeof(DTO.SettingsDTO).GetProperty("UniqeID");
                uniqueIDProperty?.SetValue(settingsDTO,
                    CreateSettingDTO(UniqeID.Key, UniqeID.Value, UniqeID.Description));

                var deviceInfoProperty = typeof(DTO.SettingsDTO).GetProperty("DeviceInfo");
                deviceInfoProperty?.SetValue(settingsDTO,
                    CreateSettingDTO(DeviceInfo.Key, DeviceInfo.Value, DeviceInfo.Description));

                var timeFormatProperty = typeof(DTO.SettingsDTO).GetProperty("TimeFormat");
                timeFormatProperty?.SetValue(settingsDTO,
                    CreateSettingDTO(TimeFormat.Key, TimeFormat.Value, TimeFormat.Description));

                var dateFormatProperty = typeof(DTO.SettingsDTO).GetProperty("DateFormat");
                dateFormatProperty?.SetValue(settingsDTO,
                    CreateSettingDTO(DateFormat.Key, DateFormat.Value, DateFormat.Description));

                var windDirectionProperty = typeof(DTO.SettingsDTO).GetProperty("WindDirection");
                windDirectionProperty?.SetValue(settingsDTO,
                    CreateSettingDTO(WindDirection.Key, WindDirection.Value, WindDirection.Description));

                var homePageViewedProperty = typeof(DTO.SettingsDTO).GetProperty("HomePageViewed");
                homePageViewedProperty?.SetValue(settingsDTO,
                    CreateSettingDTO(HomePageViewed.Key, HomePageViewed.Value, HomePageViewed.Description));

                var listLocationsViewedProperty = typeof(DTO.SettingsDTO).GetProperty("ListLocationsViewed");
                listLocationsViewedProperty?.SetValue(settingsDTO,
                    CreateSettingDTO(ListLocationsViewed.Key, ListLocationsViewed.Value, ListLocationsViewed.Description));

                var tipsViewedProperty = typeof(DTO.SettingsDTO).GetProperty("TipsViewed");
                tipsViewedProperty?.SetValue(settingsDTO,
                    CreateSettingDTO(TipsViewed.Key, TipsViewed.Value, TipsViewed.Description));

                var exposureCalcViewedProperty = typeof(DTO.SettingsDTO).GetProperty("ExposureCalcViewed");
                exposureCalcViewedProperty?.SetValue(settingsDTO,
                    CreateSettingDTO(ExposureCalcViewed.Key, ExposureCalcViewed.Value, ExposureCalcViewed.Description));

                var lightMeterViewedProperty = typeof(DTO.SettingsDTO).GetProperty("LightMeterViewed");
                lightMeterViewedProperty?.SetValue(settingsDTO,
                    CreateSettingDTO(LightMeterViewed.Key, LightMeterViewed.Value, LightMeterViewed.Description));

                var sceneEvaluationViewedProperty = typeof(DTO.SettingsDTO).GetProperty("SceneEvaluationViewed");
                sceneEvaluationViewedProperty?.SetValue(settingsDTO,
                    CreateSettingDTO(SceneEvaluationViewed.Key, SceneEvaluationViewed.Value, SceneEvaluationViewed.Description));

                var sunCalculationViewedProperty = typeof(DTO.SettingsDTO).GetProperty("SunCalculationViewed");
                sunCalculationViewedProperty?.SetValue(settingsDTO,
                    CreateSettingDTO(SunCalculationViewed.Key, SunCalculationViewed.Value, SunCalculationViewed.Description));

                var lastBulkWeatherUpdateProperty = typeof(DTO.SettingsDTO).GetProperty("LastBulkWeatherUpdate");
                lastBulkWeatherUpdateProperty?.SetValue(settingsDTO,
                    CreateSettingDTO(LastBulkWeatherUpdate.Key, LastBulkWeatherUpdate.Value, LastBulkWeatherUpdate.Description));

                var languageProperty = typeof(DTO.SettingsDTO).GetProperty("Language");
                languageProperty?.SetValue(settingsDTO,
                    CreateSettingDTO(Language.Key, Language.Value, Language.Description));

                var adSupportProperty = typeof(DTO.SettingsDTO).GetProperty("AdSupport");
                adSupportProperty?.SetValue(settingsDTO,
                    CreateSettingDTO(AdSupport.Key, AdSupport.Value, AdSupport.Description));

                var temperatureFormatProperty = typeof(DTO.SettingsDTO).GetProperty("TemperatureFormat");
                temperatureFormatProperty?.SetValue(settingsDTO,
                    CreateSettingDTO(TemperatureFormat.Key, TemperatureFormat.Value, TemperatureFormat.Description));

                // Save settings using the existing interface method
                dynamic result = new object();// await _settingsService.SaveSettingsAsync(new SettingsViewModel());

                if (!result.IsSuccess)
                {
                    VmErrorMessage = result.ErrorMessage ?? "Failed to save settings";
                    OnErrorOccurred(new OperationErrorEventArgs(
                        Locations.Core.Shared.ViewModels.OperationErrorSource.Unknown,
                        VmErrorMessage,
                        result.Exception));
                }
            }
            catch (Exception ex)
            {
                VmErrorMessage = $"Error saving settings: {ex.Message}";
                OnErrorOccurred(new OperationErrorEventArgs(
                    Locations.Core.Shared.ViewModels.OperationErrorSource.Unknown,
                    VmErrorMessage,
                    ex));
            }
            finally
            {
                VmIsBusy = false;
            }
        }

        // Reset settings to defaults
        private async Task ResetSettingsAsync()
        {
            try
            {
                VmIsBusy = true;
                VmErrorMessage = string.Empty;

                // Reset to default values
                InitializeDefaultSettings();

                // Save the defaults if we have a service
              /*  if (_settingsService != null)
                {
                    await SaveSettingsAsync();
                }*/
            }
            catch (Exception ex)
            {
                VmErrorMessage = $"Error resetting settings: {ex.Message}";
                OnErrorOccurred(new OperationErrorEventArgs(
                    Locations.Core.Shared.ViewModels.OperationErrorSource.Unknown,
                    VmErrorMessage,
                    ex));
            }
            finally
            {
                VmIsBusy = false;
            }
        }

        // Cleanup method
        public void Cleanup()
        {
            // Unsubscribe from all setting error events
            var properties = GetType().GetProperties();

            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(SettingViewModel))
                {
                    var setting = property.GetValue(this) as SettingViewModel;
                    if (setting != null)
                    {
                        setting.ErrorOccurred -= OnSettingErrorOccurred;
                    }
                }
            }
        }
    }
}