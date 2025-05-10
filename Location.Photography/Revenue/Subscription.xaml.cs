using Location.Photography.Base;
using Location.Core.Helpers.AlertService;
using Locations.Core.Business.DataAccess.Interfaces;
using Locations.Core.Shared.ViewModels;
using Location.Photography.Shared.ViewModels;
using Location.Photography.Shared.Services;
using Location.Core.Helpers;
using Location.Photography.Shared.ViewModels.Interfaces;

namespace Location.Photography.Revenue
{
    public partial class Subscription : ContentPageBase
    {
        #region Services

        private readonly ISubscriptionService _subscriptionService;
        private readonly ISettingService<SettingViewModel> _settingsService;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor for design-time and XAML preview
        /// </summary>
        public Subscription() : base()
        {
            InitializeComponent();

            // Create a default view model for design-time
            BindingContext = new Location.Photography.Shared.ViewModels.SubscriptionViewModel(
                new MockSubscriptionService());
        }

        /// <summary>
        /// Main constructor with DI
        /// </summary>
        public Subscription(
            ISettingService<SettingViewModel> settingsService,
            IAlertService alertService,
            ISubscriptionService subscriptionService) : base(settingsService, alertService, PageEnums.Settings, false)
        {
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            _subscriptionService = subscriptionService ?? throw new ArgumentNullException(nameof(subscriptionService));

            InitializeComponent();

            // Create the view model and set as binding context
            BindingContext = new Location.Photography.Shared.ViewModels.SubscriptionViewModel(_subscriptionService);

            // Subscribe to ViewModel events
            SubscribeToViewModelEvents();
        }

        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Load data when the page appears
        /// </summary>
        protected override async void OnPageAppearing(object sender, EventArgs e)
        {
            base.OnPageAppearing(sender, e);

            try
            {
                // Get the view model
                if (BindingContext is Location.Photography.Shared.ViewModels.SubscriptionViewModel viewModel)
                {
                    // Initialize the view model
                    await viewModel.InitializeAsync();
                }
            }
            catch (Exception ex)
            {
                HandleError(ex, "Error initializing page");
            }
        }

        /// <summary>
        /// Clean up when navigating away
        /// </summary>
        protected override void OnPageDisappearing(object sender, EventArgs e)
        {
            base.OnPageDisappearing(sender, e);

            // Unsubscribe from ViewModel events
            UnsubscribeFromViewModelEvents();
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Subscribe to ViewModel error events
        /// </summary>
        protected override void SubscribeToViewModelEvents()
        {
            if (BindingContext is Location.Photography.Shared.ViewModels.SubscriptionViewModel viewModel)
            {
                viewModel.ErrorOccurred += ViewModel_ErrorOccurred;
            }
        }

        /// <summary>
        /// Unsubscribe from ViewModel error events
        /// </summary>
        protected override void UnsubscribeFromViewModelEvents()
        {
            if (BindingContext is Location.Photography.Shared.ViewModels.SubscriptionViewModel viewModel)
            {
                viewModel.ErrorOccurred -= ViewModel_ErrorOccurred;
            }
        }

        /// <summary>
        /// Handle errors from the ViewModel
        /// </summary>
        private void ViewModel_ErrorOccurred(object sender, Locations.Core.Shared.ViewModels.OperationErrorEventArgs e)
        {
            Microsoft.Maui.Controls.Application.Current.Dispatcher.Dispatch(() =>
            {
                DisplayAlert(Location.Photography.Resources.AppResources.Error, e.Message, Location.Photography.Resources.AppResources.OK);
            });
        }

        #endregion
    }

    /// <summary>
    /// Mock implementation of ISubscriptionService for design-time
    /// </summary>
    internal class MockSubscriptionService : ISubscriptionService
    {
        public Task<bool> InitializeAsync() => Task.FromResult(true);

        public Task<IEnumerable<SubscriptionProductViewModel>> GetAvailableSubscriptionsAsync()
            => Task.FromResult<IEnumerable<SubscriptionProductViewModel>>(new List<SubscriptionProductViewModel>());

        public Task<SubscriptionPurchaseResult> PurchaseSubscriptionAsync(string productId)
            => Task.FromResult(SubscriptionPurchaseResult.Success);

        public Task<bool> IsSubscribedAsync(Locations.Core.Shared.Enums.SubscriptionType.SubscriptionTypeEnum type)
            => Task.FromResult(false);

        public Task<SubscriptionInfoDto> GetCurrentSubscriptionInfoAsync()
            => Task.FromResult(new SubscriptionInfoDto
            {
                Type = Locations.Core.Shared.Enums.SubscriptionType.SubscriptionTypeEnum.Free,
                IsActive = true,
                ExpirationDate = null,
                AutoRenewing = false
            });

        public Task<bool> RestorePurchasesAsync() => Task.FromResult(false);

        public Task<DateTime?> GetSubscriptionExpirationDateAsync() => Task.FromResult<DateTime?>(null);
    }
}