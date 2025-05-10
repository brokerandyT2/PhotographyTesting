// Location.Photography.Business.Revenue.Interface/ISubscriptionManager.cs
using Locations.Core.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Location.Photography.Business.Revenue.Interface
{
    public interface ISubscriptionManager
    {
        Task<bool> InitializeAsync();
        Task<IEnumerable<SubscriptionProduct>> GetAvailableSubscriptionsAsync();
        Task<PurchaseResult> PurchaseSubscriptionAsync(string productId);
        Task<bool> IsSubscribedAsync(SubscriptionType.SubscriptionTypeEnum type);
        Task<SubscriptionInfo> GetCurrentSubscriptionInfoAsync();
        Task<bool> RestorePurchasesAsync();
        Task<DateTime?> GetSubscriptionExpirationDateAsync();
    }

    public class SubscriptionProduct
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

    public class SubscriptionInfo
    {
        public SubscriptionType.SubscriptionTypeEnum Type { get; set; }
        public bool IsActive { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public bool AutoRenewing { get; set; }
    }

    public enum PurchaseResult
    {
        Success,
        Canceled,
        Error,
        AlreadyOwned,
        Pending
    }
}