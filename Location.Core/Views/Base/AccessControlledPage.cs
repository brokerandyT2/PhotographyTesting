using Location.Core.Helpers;
using Location.Core.Helpers.AlertService;
using Location.Core.Resources;
using Locations.Core.Business.DataAccess.Interfaces;
using Locations.Core.Business.DataAccess.Services;
using Locations.Core.Shared;
using Locations.Core.Shared.ViewModels;
using Locations.Core.Shared.ViewModelServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Location.Core.Views
{
    /// <summary>
    /// Base class for all content pages in the application.
    /// Handles subscription checking, ad-supported access, and page visit tracking.
    /// </summary>
    public abstract class ContentPageBase : ContentPage
    {
        #region Services and Properties

        // Core services
        protected readonly ISettingService<SettingViewModel> _settingsService;
        protected readonly IAlertService _alertService;

        // Page configuration
        public bool RequiresSubscription { get; set; } = true;
        protected PageEnums CurrentPageType { get; set; }
        protected string PageViewedMagicString { get; set; }

        // Subscription access caching
        private bool _hasValidSubscription = false;
        private bool _hasAdSupportedAccess = false;

        // Lists of page access levels
        private static readonly List<PageEnums> _freePages = new()
        {
            PageEnums.AddLocation,
            PageEnums.ListLocations,
            PageEnums.Tips,
            PageEnums.Settings,
            PageEnums.WeatherDisplay
        };

        private static readonly List<PageEnums> _premiumPages = new()
        {
            PageEnums.ExposureCalculator,
            PageEnums.LightMeter,
            PageEnums.SunLocation,
            PageEnums.SceneEvaluation,
            PageEnums.SunCalculations,
            PageEnums.WeatherDisplay,
            PageEnums.AddLocation,
            PageEnums.ListLocations,
            PageEnums.Tips,
            PageEnums.Settings
        };

        private static readonly List<PageEnums> _proPages = new()
        {
            PageEnums.SceneEvaluation,
            PageEnums.SunCalculations,
            PageEnums.WeatherDisplay,
            PageEnums.AddLocation,
            PageEnums.ListLocations,
            PageEnums.Tips,
            PageEnums.Settings,
            PageEnums.ExposureCalculator,
            PageEnums.LightMeter,
            PageEnums.SunLocation
        };

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor for design-time support
        /// </summary>
        protected ContentPageBase()
        {
            // Register for page lifecycle events
            Appearing += OnPageAppearing;
            Disappearing += OnPageDisappearing;
        }

        /// <summary>
        /// Constructor with dependency injection
        /// </summary>
        /// <param name="alertService">Alert service for displaying messages</param>
        /// <param name="pageType">The type of page for tracking</param>
        /// <param name="requiresSubscription">Whether this page requires subscription</param>
        protected ContentPageBase(IAlertService alertService, PageEnums pageType, bool requiresSubscription = true)
            : this()
        {
            _alertService = alertService;
            CurrentPageType = pageType;
            RequiresSubscription = requiresSubscription;

            // Map page type to the appropriate MagicString
            PageViewedMagicString = GetPageViewedMagicString(pageType);
        }

        /// <summary>
        /// Constructor with full dependency injection 
        /// </summary>
        protected ContentPageBase(
            ISettingService<SettingViewModel> settingsService,
            IAlertService alertService,
            PageEnums pageType,
            bool requiresSubscription = true)
        {
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            _alertService = alertService;
            CurrentPageType = pageType;
            RequiresSubscription = requiresSubscription;

            // Map page type to the appropriate MagicString
            PageViewedMagicString = GetPageViewedMagicString(pageType);

            // Register for page lifecycle events
            Appearing += OnPageAppearing;
            Disappearing += OnPageDisappearing;
        }
        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Handles page appearing event, checks subscription and page visits
        /// </summary>
        protected virtual async void OnPageAppearing(object sender, EventArgs e)
        {
            try
            {
                // Check if this is the first visit to show tutorial
                CheckFirstVisit();

                if (RequiresSubscription && _settingsService != null)
                {
                    await CheckAccessPermission();
                }
            }
            catch (Exception ex)
            {
                HandleError(ex, "Error during page initialization");
            }
        }

        /// <summary>
        /// Handles page disappearing event
        /// </summary>
        protected virtual void OnPageDisappearing(object sender, EventArgs e)
        {
            // Perform any cleanup if needed
        }

        /// <summary>
        /// Called when the page is navigated to
        /// </summary>
        protected override void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);

            // Track this page visit
            TrackPageVisit();
        }

        #endregion

        #region Access Management

        /// <summary>
        /// Checks if the user has permission to access this page, either through subscription or ads
        /// </summary>
        protected virtual async Task CheckAccessPermission()
        {
            try
            {
                // Get subscription details
                var subscriptionType = _settingsService.GetSettingByName(MagicStrings.SubscriptionType);
                var subscriptionExpiration = _settingsService.GetSettingByName(MagicStrings.SubscriptionExpiration);

                // Check if subscription is valid
                bool hasValidSubscription = CheckSubscriptionValidity(subscriptionType.Value, subscriptionExpiration.Value);
                _hasValidSubscription = hasValidSubscription;

                // Check if page is accessible with current subscription
                bool canAccessWithSubscription = CanAccessPageWithSubscription(subscriptionType.Value);

                if (hasValidSubscription && canAccessWithSubscription)
                {
                    // User has valid subscription with access to this page
                    return;
                }

                // Check if ad-supported access is available
                var adSupportedSetting = _settingsService.GetSettingByName(MagicStrings.FreePremiumAdSupported);
                bool isAdSupported = !string.IsNullOrEmpty(adSupportedSetting.Value) && Convert.ToBoolean(adSupportedSetting.Value);

                if (isAdSupported)
                {
                    // Check if user already has ad-supported access
                    bool hasAdAccess = await HasAdSupportedAccess();
                    _hasAdSupportedAccess = hasAdAccess;

                    if (!hasAdAccess)
                    {
                        // If no ad access, show ad
                        await ShowAdForAccess();
                    }
                }
                else
                {
                    // Neither subscription nor ad-supported access is valid, show subscription modal
                    await Navigation.PushModalAsync(new SubscriptionOrAdFeature(CurrentPageType));
                }
            }
            catch (Exception ex)
            {
                HandleError(ex, "Error checking access permission");
            }
        }

        /// <summary>
        /// Checks if the subscription is valid
        /// </summary>
        protected virtual bool CheckSubscriptionValidity(string subscriptionType, string expirationString)
        {
            try
            {
                // Free subscription doesn't need expiration check
                if (subscriptionType == MagicStrings.Free)
                {
                    return true;
                }

                // If premium or pro, check expiration
                if (DateTime.TryParse(expirationString, out DateTime expirationDate))
                {
                    // Valid if expiration is greater than yesterday
                    return expirationDate > DateTime.Now.AddDays(-1);
                }

                return false;
            }
            catch (Exception ex)
            {
                HandleError(ex, "Error checking subscription validity");
                return false;
            }
        }

        /// <summary>
        /// Determines if the current page can be accessed with the given subscription type
        /// </summary>
        protected virtual bool CanAccessPageWithSubscription(string subscriptionType)
        {
            try
            {
                if (subscriptionType == MagicStrings.Premium)
                {
                    return _proPages.Contains(CurrentPageType);
                }
                else if (subscriptionType == MagicStrings.Pro)
                {
                    return _premiumPages.Contains(CurrentPageType);
                }
                else // Free or other
                {
                    return _freePages.Contains(CurrentPageType);
                }
            }
            catch (Exception ex)
            {
                HandleError(ex, "Error checking page access level");
                return false;
            }
        }

        /// <summary>
        /// Checks if the user has current ad-supported access to this page
        /// </summary>
        protected virtual async Task<bool> HasAdSupportedAccess()
        {
            try
            {
                if (_settingsService == null)
                {
                    return false;
                }

                // Get the timestamp key for this page type
                string timestampKey = GetAdViewedTimestampKey(CurrentPageType);

                if (string.IsNullOrEmpty(timestampKey))
                {
                    return false;
                }

                // Get the last time an ad was viewed for this page
                var lastAdViewed = _settingsService.GetSettingByName(timestampKey);

                if (string.IsNullOrEmpty(lastAdViewed.Value))
                {
                    return false;
                }

                // Parse the timestamp
                if (DateTime.TryParse(lastAdViewed.Value, out DateTime lastAdViewedTime))
                {
                    // Get hours per ad from settings
                    var hoursPerAdSetting = _settingsService.GetSettingByName(MagicStrings.AdGivesHours);
                    int hoursPerAd = string.IsNullOrEmpty(hoursPerAdSetting.Value) ? 24 : Convert.ToInt32(hoursPerAdSetting.Value);

                    // Check if the ad-supported access is still valid
                    return DateTime.Now < lastAdViewedTime.AddHours(hoursPerAd);
                }

                return false;
            }
            catch (Exception ex)
            {
                HandleError(ex, "Error checking ad-supported access");
                return false;
            }
        }

        /// <summary>
        /// Shows an ad to gain temporary access to this page
        /// </summary>
        protected virtual async Task ShowAdForAccess()
        {
            try
            {
                // Get hours per ad for display
                var hoursPerAd = _settingsService.GetSettingWithMagicString(MagicStrings.AdGivesHours);

                // Show dialog asking user if they want to watch an ad
                bool userWatchesAd = await DisplayAlert(
                    AppResources.SubscriptionRequired,
                    AppResources.WatchAdForAccess.Replace("{0}", hoursPerAd),
                    AppResources.OK,
                    AppResources.Cancel);

                if (userWatchesAd)
                {
                    // Here you would integrate with your ad provider
                    // For now, we'll simulate an ad being watched
                    await Task.Delay(1500); // Simulate ad loading and display

                    // Update the timestamp for when this ad was watched
                    string timestampKey = GetAdViewedTimestampKey(CurrentPageType);
                    if (!string.IsNullOrEmpty(timestampKey) && _settingsService != null)
                    {
                        // Create a new setting
                        var newSetting = new SettingViewModel
                        {
                            Key = timestampKey,
                            Value = DateTime.Now.ToString()
                        };

                        // Save it using the async method
                        await _settingsService.SaveAsync(newSetting);

                        // Update cached state
                        _hasAdSupportedAccess = true;
                    }
                }
                else
                {
                    // User declined to watch ad, navigate back
                    await Navigation.PopAsync();
                }
            }
            catch (Exception ex)
            {
                HandleError(ex, "Error showing ad");
            }
        }

        #endregion

        #region Page Visit Tracking

        /// <summary>
        /// Checks if this is the first visit to this page type and shows tutorial if needed
        /// </summary>
        protected virtual void CheckFirstVisit()
        {
            try
            {
                if (!string.IsNullOrEmpty(PageViewedMagicString) && _settingsService != null)
                {
                    PageHelpers.CheckVisit(PageViewedMagicString, CurrentPageType, _settingsService, Navigation);
                }
            }
            catch (Exception ex)
            {
                HandleError(ex, "Error checking first visit");
            }
        }

        /// <summary>
        /// Tracks that this page has been visited
        /// </summary>
        protected virtual async void TrackPageVisit()
        {
            try
            {
                if (_settingsService == null || string.IsNullOrEmpty(PageViewedMagicString))
                {
                    return;
                }

                // Get the current setting
                var setting = _settingsService.GetSettingByName(PageViewedMagicString);

                if (setting != null && setting.Id > 0)
                {
                    // Update existing setting
                    setting.Value = MagicStrings.True_string;
                    await _settingsService.UpdateAsync(setting);
                }
                else
                {
                    // Create a new setting
                    var newSetting = new SettingViewModel
                    {
                        Key = PageViewedMagicString,
                        Value = MagicStrings.True_string
                    };

                    await _settingsService.SaveAsync(newSetting);
                }
            }
            catch (Exception ex)
            {
                HandleError(ex, "Error tracking page visit");
            }
        }

        #endregion

        #region Error Handling

        /// <summary>
        /// Handles errors that occur during page operations
        /// </summary>
        protected virtual void HandleError(Exception ex, string message)
        {
            // Log the error
            System.Diagnostics.Debug.WriteLine($"Error: {message}. {ex.Message}");

            // Display alert to user
            DisplayAlert(AppResources.Error, message, AppResources.OK);

            // If this is a ViewModel error, it can be bubbled up through the view model
            if (BindingContext is ViewModelBase viewModel)
            {
                viewModel.VmErrorMessage = $"{message}: {ex.Message}";
            }
        }

        /// <summary>
        /// Subscribes to error events from the ViewModel
        /// </summary>
        protected virtual void SubscribeToViewModelEvents()
        {
            if (BindingContext is ViewModelBase viewModel)
            {
                viewModel.ErrorOccurred += ViewModel_ErrorOccurred;
            }
        }

        /// <summary>
        /// Unsubscribes from error events from the ViewModel
        /// </summary>
        protected virtual void UnsubscribeFromViewModelEvents()
        {
            if (BindingContext is ViewModelBase viewModel)
            {
                viewModel.ErrorOccurred -= ViewModel_ErrorOccurred;
            }
        }

        /// <summary>
        /// Handles error events from the ViewModel
        /// </summary>
        private void ViewModel_ErrorOccurred(object sender, Locations.Core.Shared.ViewModels.OperationErrorEventArgs e)
        {
            Microsoft.Maui.Controls.Application.Current.Dispatcher.Dispatch(() => {
                DisplayAlert(AppResources.Error, e.Message, AppResources.OK);
            });
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Maps page type to the appropriate MagicString for tracking visits
        /// </summary>
        protected virtual string GetPageViewedMagicString(PageEnums pageType)
        {
            return pageType switch
            {
                PageEnums.WeatherDisplay => MagicStrings.WeatherDisplayViewed,
                PageEnums.ExposureCalculator => MagicStrings.ExposureCalcViewed,
                PageEnums.LightMeter => MagicStrings.LightMeterViewed,
                PageEnums.SceneEvaluation => MagicStrings.SceneEvaluationViewed,
                PageEnums.SunLocation => MagicStrings.SunLocationViewed,
                PageEnums.SunCalculations => MagicStrings.SunCalculatorViewed,
                PageEnums.AddLocation => MagicStrings.AddLocationViewed,
                PageEnums.ListLocations => MagicStrings.LocationListViewed,
                PageEnums.Settings => MagicStrings.SettingsViewed,
                PageEnums.Tips => MagicStrings.TipsViewed,
                _ => string.Empty
            };
        }

        /// <summary>
        /// Gets the ad timestamp key for the given page type
        /// </summary>
        protected virtual string GetAdViewedTimestampKey(PageEnums pageType)
        {
            return pageType switch
            {
                PageEnums.WeatherDisplay => MagicStrings.WeatherDisplayAdViewed_TimeStamp,
                PageEnums.ExposureCalculator => MagicStrings.ExposureCalcAdViewed_TimeStamp,
                PageEnums.LightMeter => MagicStrings.LightMeterAdViewed_TimeStamp,
                PageEnums.SunLocation => MagicStrings.SunLocationAdViewed_TimeStamp,
                PageEnums.SceneEvaluation => MagicStrings.SceneEvaluationAdViewed_TimeStamp,
                PageEnums.SunCalculations => MagicStrings.SunCalculatorViewed_TimeStamp,
                _ => string.Empty
            };
        }

        #endregion

        #region Cleanup

        /// <summary>
        /// Clean up resources and event handlers
        /// </summary>
        ~ContentPageBase()
        {
            // Unsubscribe from events
            UnsubscribeFromViewModelEvents();
            Appearing -= OnPageAppearing;
            Disappearing -= OnPageDisappearing;
        }

        #endregion
    }
}