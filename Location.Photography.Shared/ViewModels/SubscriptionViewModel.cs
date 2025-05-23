﻿// Location.Photography.Shared/ViewModels/SubscriptionViewModel.cs
using CommunityToolkit.Mvvm.Input;
using Location.Photography.Shared.Services;
using Location.Photography.Shared.ViewModels.Interfaces;
using Locations.Core.Shared.Enums;
using Locations.Core.Shared.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Location.Photography.Shared.ViewModels
{
    public partial class SubscriptionViewModel : ViewModelBase, ISubscriptionViewModel
    {
        #region Fields
        private readonly ISubscriptionService _subscriptionService;

        private bool _isLoading;
        private bool _hasActiveSubscription;
        private bool _isPremiumSubscription;
        private bool _isProfessionalSubscription;
        private string _currentSubscriptionInfo;
        private DateTime? _expirationDate;
        private SubscriptionType.SubscriptionTypeEnum _currentSubscriptionType;
        private SubscriptionProductViewModel _selectedSubscription;
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<OperationErrorEventArgs> ErrorOccurred;
        #endregion

        #region Properties
        public ObservableCollection<SubscriptionProductViewModel> AvailableSubscriptions { get; } = new();
        public ObservableCollection<SubscriptionProductViewModel> PremiumSubscriptions { get; } = new();
        public ObservableCollection<SubscriptionProductViewModel> ProfessionalSubscriptions { get; } = new();

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        public bool HasActiveSubscription
        {
            get => _hasActiveSubscription;
            set
            {
                _hasActiveSubscription = value;
                OnPropertyChanged();
            }
        }

        public bool IsPremiumSubscription
        {
            get => _isPremiumSubscription;
            set
            {
                _isPremiumSubscription = value;
                OnPropertyChanged();
            }
        }

        public bool IsProfessionalSubscription
        {
            get => _isProfessionalSubscription;
            set
            {
                _isProfessionalSubscription = value;
                OnPropertyChanged();
            }
        }

        public SubscriptionType.SubscriptionTypeEnum CurrentSubscriptionType
        {
            get => _currentSubscriptionType;
            set
            {
                _currentSubscriptionType = value;
                OnPropertyChanged();

                // Update related properties
                IsPremiumSubscription = value == SubscriptionType.SubscriptionTypeEnum.Premium;
                IsProfessionalSubscription = value == SubscriptionType.SubscriptionTypeEnum.Professional;
            }
        }

        public string CurrentSubscriptionInfo
        {
            get => _currentSubscriptionInfo;
            set
            {
                _currentSubscriptionInfo = value;
                OnPropertyChanged();
            }
        }

        public DateTime? ExpirationDate
        {
            get => _expirationDate;
            set
            {
                _expirationDate = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ExpirationDateDisplay));
            }
        }

        public string ExpirationDateDisplay
        {
            get
            {
                if (!ExpirationDate.HasValue)
                    return string.Empty;

                return ExpirationDate.Value.ToLocalTime().ToString("MMM dd, yyyy");
            }
        }

        // Using the base ErrorMessage property
      

        public bool HasError => IsError;

        public SubscriptionProductViewModel SelectedSubscription
        {
            get => _selectedSubscription;
            set
            {
                _selectedSubscription = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands
        public ICommand LoadSubscriptionsCommand { get; }
        public ICommand PurchaseCommand { get; }
        public ICommand RestorePurchasesCommand { get; }
        #endregion

        #region Constructor
        public SubscriptionViewModel(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService ?? throw new ArgumentNullException(nameof(subscriptionService));

            LoadSubscriptionsCommand = new AsyncRelayCommand(LoadSubscriptionsAsync);
            PurchaseCommand = new AsyncRelayCommand<string>(PurchaseSubscriptionAsync);
            RestorePurchasesCommand = new AsyncRelayCommand(RestorePurchasesAsync);
        }
        #endregion

        #region Methods
        public async Task InitializeAsync()
        {
            try
            {
                IsBusy = true;
                IsLoading = true;
                ErrorMessage = string.Empty;

                var initialized = await _subscriptionService.InitializeAsync();
                if (!initialized)
                {
                    ErrorMessage = "Unable to initialize subscription service.";
                    OnErrorOccurred(new OperationErrorEventArgs(
                        OperationErrorSource.Unknown,
                        ErrorMessage,
                        new Exception("Subscription initialization failed")));
                    return;
                }

                await CheckSubscriptionStatusAsync();
                await LoadSubscriptionsAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error initializing: {ex.Message}";
                OnErrorOccurred(new OperationErrorEventArgs(
                    OperationErrorSource.Unknown,
                    ErrorMessage,
                    ex));
            }
            finally
            {
                IsBusy = false;
                IsLoading = false;
            }
        }

        public async Task CheckSubscriptionStatusAsync()
        {
            try
            {
                IsBusy = true;
                ErrorMessage = string.Empty;

                var currentInfo = await _subscriptionService.GetCurrentSubscriptionInfoAsync();

                CurrentSubscriptionType = currentInfo.Type;
                HasActiveSubscription = currentInfo.Type > SubscriptionType.SubscriptionTypeEnum.Free;
                ExpirationDate = currentInfo.ExpirationDate;

                // Update subscription info text
                switch (currentInfo.Type)
                {
                    case SubscriptionType.SubscriptionTypeEnum.Free:
                        CurrentSubscriptionInfo = "Free Access";
                        break;
                    case SubscriptionType.SubscriptionTypeEnum.Premium:
                        CurrentSubscriptionInfo = "Premium Access";
                        break;
                    case SubscriptionType.SubscriptionTypeEnum.Professional:
                        CurrentSubscriptionInfo = "Professional Access";
                        break;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error checking subscription status: {ex.Message}";
                OnErrorOccurred(new OperationErrorEventArgs(
                    OperationErrorSource.Unknown,
                    ErrorMessage,
                    ex));
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task LoadSubscriptionsAsync()
        {
            try
            {
                IsBusy = true;
                IsLoading = true;
                ErrorMessage = string.Empty;

                AvailableSubscriptions.Clear();
                PremiumSubscriptions.Clear();
                ProfessionalSubscriptions.Clear();

                var subscriptions = await _subscriptionService.GetAvailableSubscriptionsAsync();

                foreach (var subscription in subscriptions)
                {
                    AvailableSubscriptions.Add(subscription);

                    // Also add to type-specific collections
                    if (subscription.Type == SubscriptionType.SubscriptionTypeEnum.Premium)
                    {
                        PremiumSubscriptions.Add(subscription);
                    }
                    else if (subscription.Type == SubscriptionType.SubscriptionTypeEnum.Professional)
                    {
                        ProfessionalSubscriptions.Add(subscription);
                    }
                }

                if (AvailableSubscriptions.Count > 0)
                {
                    SelectedSubscription = AvailableSubscriptions[0];
                }
                else
                {
                    ErrorMessage = "No subscription products available.";
                    OnErrorOccurred(new OperationErrorEventArgs(
                        OperationErrorSource.Unknown,
                        ErrorMessage,
                        new Exception("No subscription products available")));
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error loading subscriptions: {ex.Message}";
                OnErrorOccurred(new OperationErrorEventArgs(
                    OperationErrorSource.Unknown,
                    ErrorMessage,
                    ex));
            }
            finally
            {
                IsBusy = false;
                IsLoading = false;
            }
        }

        public async Task PurchaseSubscriptionAsync(string productId)
        {
            if (string.IsNullOrEmpty(productId))
            {
                ErrorMessage = "No subscription selected.";
                OnErrorOccurred(new OperationErrorEventArgs(
                    OperationErrorSource.Unknown,
                    ErrorMessage,
                    new ArgumentException("No subscription selected")));
                return;
            }

            try
            {
                IsBusy = true;
                IsLoading = true;
                ErrorMessage = string.Empty;

                var result = await _subscriptionService.PurchaseSubscriptionAsync(productId);

                switch (result)
                {
                    case SubscriptionPurchaseResult.Success:
                        // Refresh subscription status
                        await CheckSubscriptionStatusAsync();
                        break;

                    case SubscriptionPurchaseResult.Pending:
                        ErrorMessage = "Your purchase is pending completion. Access will be granted when the payment is processed.";
                        break;

                    case SubscriptionPurchaseResult.Canceled:
                        ErrorMessage = "Purchase was canceled.";
                        break;

                    case SubscriptionPurchaseResult.AlreadyOwned:
                        ErrorMessage = "You already own this subscription. Try restoring purchases.";
                        break;

                    case SubscriptionPurchaseResult.Error:
                    default:
                        ErrorMessage = "An error occurred during purchase.";
                        OnErrorOccurred(new OperationErrorEventArgs(
                            OperationErrorSource.Unknown,
                            ErrorMessage,
                            new Exception("Purchase failed with result: " + result)));
                        break;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error during purchase: {ex.Message}";
                OnErrorOccurred(new OperationErrorEventArgs(
                    OperationErrorSource.Unknown,
                    ErrorMessage,
                    ex));
            }
            finally
            {
                IsBusy = false;
                IsLoading = false;
            }
        }

        public async Task RestorePurchasesAsync()
        {
            try
            {
                IsBusy = true;
                IsLoading = true;
                ErrorMessage = string.Empty;

                var restored = await _subscriptionService.RestorePurchasesAsync();

                if (restored)
                {
                    await CheckSubscriptionStatusAsync();
                    // Success message could be shown
                }
                else
                {
                    ErrorMessage = "No previous purchases found to restore.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error restoring purchases: {ex.Message}";
                OnErrorOccurred(new OperationErrorEventArgs(
                    OperationErrorSource.Unknown,
                    ErrorMessage,
                    ex));
            }
            finally
            {
                IsBusy = false;
                IsLoading = false;
            }
        }



        protected virtual void OnErrorOccurred(OperationErrorEventArgs e)
        {
            ErrorOccurred?.Invoke(this, e);
        }
        #endregion
    }
}