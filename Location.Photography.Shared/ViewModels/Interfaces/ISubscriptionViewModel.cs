// Location.Photography.Shared/ViewModels/Interfaces/ISubscriptionViewModel.cs
using Locations.Core.Shared.Enums;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Location.Photography.Shared.ViewModels.Interfaces
{
    public interface ISubscriptionViewModel
    {
        ObservableCollection<SubscriptionProductViewModel> AvailableSubscriptions { get; }
        ObservableCollection<SubscriptionProductViewModel> PremiumSubscriptions { get; }
        ObservableCollection<SubscriptionProductViewModel> ProfessionalSubscriptions { get; }

        bool IsLoading { get; }
        bool HasActiveSubscription { get; }
        bool IsPremiumSubscription { get; }
        bool IsProfessionalSubscription { get; }
        SubscriptionType.SubscriptionTypeEnum CurrentSubscriptionType { get; }
        string CurrentSubscriptionInfo { get; }
        DateTime? ExpirationDate { get; }
        string ExpirationDateDisplay { get; }
        string ErrorMessage { get; }
        bool HasError { get; }


        ICommand LoadSubscriptionsCommand { get; }
        ICommand PurchaseCommand { get; }
        ICommand RestorePurchasesCommand { get; }

        Task InitializeAsync();
        Task CheckSubscriptionStatusAsync();
        Task LoadSubscriptionsAsync();
        Task PurchaseSubscriptionAsync(string productId);
        Task RestorePurchasesAsync();
    }

    /// <summary>
    /// ViewModel representation of a subscription product
    /// </summary>
    public class SubscriptionProductViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public string BillingPeriod { get; set; }
        public bool IsMonthly { get; set; }
        public bool HasFreeTrial { get; set; }
        public SubscriptionType.SubscriptionTypeEnum Type { get; set; }
    }
}