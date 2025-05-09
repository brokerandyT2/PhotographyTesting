using System;
using System.Collections.Generic;

namespace Locations.Core.Shared.DTO
{
    /// <summary>
    /// Data Transfer Object for all application settings
    /// </summary>
    public class SettingsDTO
    {
        /// <summary>
        /// Gets or sets the hemisphere setting
        /// </summary>
        public SettingDTO Hemisphere { get; set; }

        /// <summary>
        /// Gets or sets the first name setting
        /// </summary>
        public SettingDTO FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name setting
        /// </summary>
        public SettingDTO LastName { get; set; }

        /// <summary>
        /// Gets or sets the email setting
        /// </summary>
        public SettingDTO Email { get; set; }

        /// <summary>
        /// Gets or sets the subscription expiration setting
        /// </summary>
        public SettingDTO SubscriptionExpiration { get; set; }

        /// <summary>
        /// Gets or sets the subscription type setting
        /// </summary>
        public SettingDTO SubscriptionType { get; set; }

        /// <summary>
        /// Gets or sets the unique ID setting
        /// </summary>
        public SettingDTO UniqeID { get; set; }

        /// <summary>
        /// Gets or sets the device info setting
        /// </summary>
        public SettingDTO DeviceInfo { get; set; }

        /// <summary>
        /// Gets or sets the time format setting
        /// </summary>
        public SettingDTO TimeFormat { get; set; }

        /// <summary>
        /// Gets or sets the date format setting
        /// </summary>
        public SettingDTO DateFormat { get; set; }

        /// <summary>
        /// Gets or sets the wind direction setting
        /// </summary>
        public SettingDTO WindDirection { get; set; }

        /// <summary>
        /// Gets or sets the home page viewed setting
        /// </summary>
        public SettingDTO HomePageViewed { get; set; }

        /// <summary>
        /// Gets or sets the list locations viewed setting
        /// </summary>
        public SettingDTO ListLocationsViewed { get; set; }

        /// <summary>
        /// Gets or sets the tips viewed setting
        /// </summary>
        public SettingDTO TipsViewed { get; set; }

        /// <summary>
        /// Gets or sets the exposure calc viewed setting
        /// </summary>
        public SettingDTO ExposureCalcViewed { get; set; }

        /// <summary>
        /// Gets or sets the light meter viewed setting
        /// </summary>
        public SettingDTO LightMeterViewed { get; set; }

        /// <summary>
        /// Gets or sets the scene evaluation viewed setting
        /// </summary>
        public SettingDTO SceneEvaluationViewed { get; set; }

        /// <summary>
        /// Gets or sets the sun calculation viewed setting
        /// </summary>
        public SettingDTO SunCalculationViewed { get; set; }

        /// <summary>
        /// Gets or sets the last bulk weather update setting
        /// </summary>
        public SettingDTO LastBulkWeatherUpdate { get; set; }

        /// <summary>
        /// Gets or sets the language setting
        /// </summary>
        public SettingDTO Language { get; set; }

        /// <summary>
        /// Gets or sets the ad support setting
        /// </summary>
        public SettingDTO AdSupport { get; set; }

        /// <summary>
        /// Gets or sets the temperature format setting
        /// </summary>
        public SettingDTO TemperatureFormat { get; set; }

        /// <summary>
        /// Initializes a new instance of the SettingsDTO class
        /// </summary>
        public SettingsDTO()
        {
            // Initialize all settings with default empty values
            Hemisphere = new SettingDTO("Hemisphere", string.Empty);
            FirstName = new SettingDTO("FirstName", string.Empty);
            LastName = new SettingDTO("LastName", string.Empty);
            Email = new SettingDTO("Email", string.Empty);
            SubscriptionExpiration = new SettingDTO("SubscriptionExpiration", string.Empty);
            SubscriptionType = new SettingDTO("SubscriptionType", string.Empty);
            UniqeID = new SettingDTO("UniqueID", string.Empty);
            DeviceInfo = new SettingDTO("DeviceInfo", string.Empty);
            TimeFormat = new SettingDTO("TimeFormat", string.Empty);
            DateFormat = new SettingDTO("DateFormat", string.Empty);
            WindDirection = new SettingDTO("WindDirection", string.Empty);
            HomePageViewed = new SettingDTO("HomePageViewed", "False");
            ListLocationsViewed = new SettingDTO("ListLocationsViewed", "False");
            TipsViewed = new SettingDTO("TipsViewed", "False");
            ExposureCalcViewed = new SettingDTO("ExposureCalcViewed", "False");
            LightMeterViewed = new SettingDTO("LightMeterViewed", "False");
            SceneEvaluationViewed = new SettingDTO("SceneEvaluationViewed", "False");
            SunCalculationViewed = new SettingDTO("SunCalculationViewed", "False");
            LastBulkWeatherUpdate = new SettingDTO("LastBulkWeatherUpdate", string.Empty);
            Language = new SettingDTO("Language", "en-US");
            AdSupport = new SettingDTO("AdSupport", "True");
            TemperatureFormat = new SettingDTO("TemperatureFormat", string.Empty);
        }

        /// <summary>
        /// Creates a SettingsDTO from a SettingsViewModel
        /// </summary>
        public static SettingsDTO FromViewModel(Locations.Core.Shared.ViewModels.SettingsViewModel viewModel)
        {
            var dto = new SettingsDTO();

            // Copy values from ViewModel to DTO
            if (viewModel.Hemisphere != null)
                dto.Hemisphere = new SettingDTO("Hemisphere", viewModel.Hemisphere.Value);

            if (viewModel.FirstName != null)
                dto.FirstName = new SettingDTO("FirstName", viewModel.FirstName.Value);

            if (viewModel.LastName != null)
                dto.LastName = new SettingDTO("LastName", viewModel.LastName.Value);

            if (viewModel.Email != null)
                dto.Email = new SettingDTO("Email", viewModel.Email.Value);

            if (viewModel.SubscriptionExpiration != null)
                dto.SubscriptionExpiration = new SettingDTO("SubscriptionExpiration", viewModel.SubscriptionExpiration.Value);

            if (viewModel.SubscriptionType != null)
                dto.SubscriptionType = new SettingDTO("SubscriptionType", viewModel.SubscriptionType.Value);

            if (viewModel.UniqeID != null)
                dto.UniqeID = new SettingDTO("UniqueID", viewModel.UniqeID.Value);

            if (viewModel.DeviceInfo != null)
                dto.DeviceInfo = new SettingDTO("DeviceInfo", viewModel.DeviceInfo.Value);

            if (viewModel.TimeFormat != null)
                dto.TimeFormat = new SettingDTO("TimeFormat", viewModel.TimeFormat.Value);

            if (viewModel.DateFormat != null)
                dto.DateFormat = new SettingDTO("DateFormat", viewModel.DateFormat.Value);

            if (viewModel.WindDirection != null)
                dto.WindDirection = new SettingDTO("WindDirection", viewModel.WindDirection.Value);

            if (viewModel.HomePageViewed != null)
                dto.HomePageViewed = new SettingDTO("HomePageViewed", viewModel.HomePageViewed.Value);

            if (viewModel.ListLocationsViewed != null)
                dto.ListLocationsViewed = new SettingDTO("ListLocationsViewed", viewModel.ListLocationsViewed.Value);

            if (viewModel.TipsViewed != null)
                dto.TipsViewed = new SettingDTO("TipsViewed", viewModel.TipsViewed.Value);

            if (viewModel.ExposureCalcViewed != null)
                dto.ExposureCalcViewed = new SettingDTO("ExposureCalcViewed", viewModel.ExposureCalcViewed.Value);

            if (viewModel.LightMeterViewed != null)
                dto.LightMeterViewed = new SettingDTO("LightMeterViewed", viewModel.LightMeterViewed.Value);

            if (viewModel.SceneEvaluationViewed != null)
                dto.SceneEvaluationViewed = new SettingDTO("SceneEvaluationViewed", viewModel.SceneEvaluationViewed.Value);

            if (viewModel.SunCalculationViewed != null)
                dto.SunCalculationViewed = new SettingDTO("SunCalculationViewed", viewModel.SunCalculationViewed.Value);

            if (viewModel.LastBulkWeatherUpdate != null)
                dto.LastBulkWeatherUpdate = new SettingDTO("LastBulkWeatherUpdate", viewModel.LastBulkWeatherUpdate.Value);

            if (viewModel.Language != null)
                dto.Language = new SettingDTO("Language", viewModel.Language.Value);

            if (viewModel.AdSupport != null)
                dto.AdSupport = new SettingDTO("AdSupport", viewModel.AdSupport.Value);

            if (viewModel.TemperatureFormat != null)
                dto.TemperatureFormat = new SettingDTO("TemperatureFormat", viewModel.TemperatureFormat.Value);

            return dto;
        }
    }
}