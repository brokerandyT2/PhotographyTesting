// Location.Photography.Shared/Services/ISubscriptionService.cs
using Location.Photography.Shared.ViewModels.Interfaces;
using Locations.Core.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Location.Photography.Shared.Services
{
    public interface ISubscriptionService
    {
        Task<bool> InitializeAsync();
        Task<IEnumerable<SubscriptionProductViewModel>> GetAvailableSubscriptionsAsync();
        Task<SubscriptionPurchaseResult> PurchaseSubscriptionAsync(string productId);
        Task<bool> IsSubscribedAsync(SubscriptionType.SubscriptionTypeEnum type);
        Task<SubscriptionInfoDto> GetCurrentSubscriptionInfoAsync();
        Task<bool> RestorePurchasesAsync();
        Task<DateTime?> GetSubscriptionExpirationDateAsync();
    }

    public class SubscriptionInfoDto
    {
        public SubscriptionType.SubscriptionTypeEnum Type { get; set; }
        public bool IsActive { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public bool AutoRenewing { get; set; }
    }

    public enum SubscriptionPurchaseResult
    {
        Success,
        Canceled,
        Error,
        AlreadyOwned,
        Pending
    }
}