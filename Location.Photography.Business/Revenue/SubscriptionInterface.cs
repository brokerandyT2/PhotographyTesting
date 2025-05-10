// Location.Photography.Business.Revenue/SubscriptionManager.cs
using Location.Photography.Business.Revenue.Interface;
using Locations.Core.Business.DataAccess.Interfaces;
using Locations.Core.Shared;
using Locations.Core.Shared.Enums;
using Locations.Core.Shared.ViewModels;
using Plugin.InAppBilling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Location.Photography.Business.Revenue
{
    public class SubscriptionManagers : ISubscriptionManager
    {
        private readonly ISettingService<SettingsViewModel> _settingService;

        // Maps subscription IDs to their types
        private static readonly Dictionary<string, SubscriptionType.SubscriptionTypeEnum> SubscriptionTypeMap =
            new Dictionary<string, SubscriptionType.SubscriptionTypeEnum>
        {
            { "photography_premium_monthly", SubscriptionType.SubscriptionTypeEnum.Premium },
            { "photography_premium_yearly", SubscriptionType.SubscriptionTypeEnum.Premium },
            { "photography_professional_monthly", SubscriptionType.SubscriptionTypeEnum.Professional },
            { "photography_professional_yearly", SubscriptionType.SubscriptionTypeEnum.Professional }
        };

        // All subscription product IDs
        private static readonly string[] AllSubscriptionIds = SubscriptionTypeMap.Keys.ToArray();

        // Maps subscription types to subscription name strings
        private static readonly Dictionary<SubscriptionType.SubscriptionTypeEnum, string> TypeToStringMap =
            new Dictionary<SubscriptionType.SubscriptionTypeEnum, string>
        {
            { SubscriptionType.SubscriptionTypeEnum.Free, MagicStrings.Free },
            { SubscriptionType.SubscriptionTypeEnum.Premium, MagicStrings.Premium },
            { SubscriptionType.SubscriptionTypeEnum.Professional, MagicStrings.Pro }
        };

        public SubscriptionManagers(ISettingService<SettingsViewModel> settingService)
        {
            _settingService = settingService ?? throw new ArgumentNullException(nameof(settingService));
        }

        public async Task<bool> InitializeAsync()
        {
#if DEBUG
            // Always succeed in debug mode
            return true;
#else
            try
            {
                var connected = await CrossInAppBilling.Current.ConnectAsync();
                if (!connected)
                {
                    Console.WriteLine("Failed to connect to billing service");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing subscription service: {ex.Message}");
                return false;
            }
            finally
            {
                await CrossInAppBilling.Current.DisconnectAsync();
            }
#endif
        }

        public async Task<IEnumerable<SubscriptionProduct>> GetAvailableSubscriptionsAsync()
        {
#if DEBUG
            // Return mock products in debug mode
            return new List<SubscriptionProduct>
            {
                new SubscriptionProduct
                {
                    Id = "photography_premium_monthly",
                    Name = "Premium Monthly",
                    Description = "Access to all premium features with monthly billing",
                    Price = "$4.99",
                    BillingPeriod = "P1M",
                    IsMonthly = true,
                    HasFreeTrial = true,
                    Type = SubscriptionType.SubscriptionTypeEnum.Premium
                },
                new SubscriptionProduct
                {
                    Id = "photography_premium_yearly",
                    Name = "Premium Yearly",
                    Description = "Access to all premium features with annual billing (save 30%)",
                    Price = "$39.99",
                    BillingPeriod = "P1Y",
                    IsMonthly = false,
                    HasFreeTrial = true,
                    Type = SubscriptionType.SubscriptionTypeEnum.Premium
                },
                new SubscriptionProduct
                {
                    Id = "photography_professional_monthly",
                    Name = "Professional Monthly",
                    Description = "Professional features with monthly billing",
                    Price = "$9.99",
                    BillingPeriod = "P1M",
                    IsMonthly = true,
                    HasFreeTrial = true,
                    Type = SubscriptionType.SubscriptionTypeEnum.Professional
                },
                new SubscriptionProduct
                {
                    Id = "photography_professional_yearly",
                    Name = "Professional Yearly",
                    Description = "Professional features with annual billing (save 30%)",
                    Price = "$79.99",
                    BillingPeriod = "P1Y",
                    IsMonthly = false,
                    HasFreeTrial = true,
                    Type = SubscriptionType.SubscriptionTypeEnum.Professional
                }
            };
#else
            try
            {
                var connected = await CrossInAppBilling.Current.ConnectAsync();
                if (!connected)
                    return Enumerable.Empty<SubscriptionProduct>();

                var products = await CrossInAppBilling.Current.GetProductInfoAsync(
                    ItemType.Subscription,
                    AllSubscriptionIds
                );

                if (products?.Any() != true)
                    return Enumerable.Empty<SubscriptionProduct>();

                return products.Select(p => new SubscriptionProduct
                {
                    Id = p.ProductId,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.LocalizedPrice,
                    // Billing period isn't available directly from the API
                    // We can infer it from the product ID
                    BillingPeriod = p.ProductId.Contains("monthly") ? "P1M" : "P1Y",
                    IsMonthly = p.ProductId.Contains("monthly"),
                    // Free trial information isn't directly available
                    HasFreeTrial = false,
                    Type = SubscriptionTypeMap.ContainsKey(p.ProductId)
                        ? SubscriptionTypeMap[p.ProductId]
                        : SubscriptionType.SubscriptionTypeEnum.Free
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting subscription products: {ex.Message}");
                return Enumerable.Empty<SubscriptionProduct>();
            }
            finally
            {
                await CrossInAppBilling.Current.DisconnectAsync();
            }
#endif
        }

        public async Task<PurchaseResult> PurchaseSubscriptionAsync(string productId)
        {
#if DEBUG
            // Mock purchase flow in debug mode
            // Simulate successful purchase 90% of the time
            var random = new Random();
            var successRate = random.Next(1, 101);
            
            if (successRate <= 90)
            {
                // Simulate successful purchase
                DateTime expirationDate;
                if (productId.Contains("monthly"))
                {
                    expirationDate = DateTime.UtcNow.AddMonths(1);
                }
                else
                {
                    expirationDate = DateTime.UtcNow.AddYears(1);
                }
                
                // Store subscription info
                _settingService.GetSettingWithMagicString(MagicStrings.SubscriptionType);
                var subType = SubscriptionTypeMap.ContainsKey(productId)
                    ? TypeToStringMap[SubscriptionTypeMap[productId]]
                    : MagicStrings.Free;
                    
                _settingService.SaveSetting(MagicStrings.SubscriptionType, subType);
                _settingService.SaveSetting(MagicStrings.SubscriptionExpiration, expirationDate.ToString("o"));

                return PurchaseResult.Success;
            }
            else if (successRate <= 95)
            {
                // Simulate payment pending
                return PurchaseResult.Pending;
            }
            else if (successRate <= 97)
            {
                // Simulate already owned
                return PurchaseResult.AlreadyOwned;
            }
            else
            {
                // Simulate user canceled
                return PurchaseResult.Canceled;
            }
#else
            try
            {
                var connected = await CrossInAppBilling.Current.ConnectAsync();
                if (!connected)
                    return PurchaseResult.Error;

                var purchase = await CrossInAppBilling.Current.PurchaseAsync(
                    productId,
                    ItemType.Subscription
                );

                if (purchase == null)
                    return PurchaseResult.Canceled;

                switch (purchase.State)
                {
                    case PurchaseState.Purchased:
                        // For Android, subscriptions must be acknowledged
                        if (!string.IsNullOrEmpty(purchase.PurchaseToken) && !string.IsNullOrEmpty(purchase.Id))
                        {
                            try
                            {
                                // Using the correct method signature for ConsumePurchaseAsync
                                await CrossInAppBilling.Current.ConsumePurchaseAsync(
                                    purchase.PurchaseToken,
                                    purchase.Id,
                                    CancellationToken.None
                                );
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error consuming purchase: {ex.Message}");
                                // Continue even if consumption fails - it might be an unsupported platform
                            }
                        }

                        // Store subscription information
                        if (SubscriptionTypeMap.TryGetValue(productId, out var subscriptionType))
                        {
                            // Store subscription type
                            _settingService.SaveSetting(
                                MagicStrings.SubscriptionType,
                                TypeToStringMap[subscriptionType]
                            );

                            // Determine expiration date - for subscriptions we'd store this information
                            // For monthly subscription, set expiration 1 month from now
                            // For yearly subscription, set expiration 1 year from now
                            var expirationDate = productId.Contains("monthly")
                                ? DateTime.UtcNow.AddMonths(1)
                                : DateTime.UtcNow.AddYears(1);

                            _settingService.SaveSetting(
                                MagicStrings.SubscriptionExpiration,
                                expirationDate.ToString("o")
                            );
                        }

                        return PurchaseResult.Success;

                    case PurchaseState.PaymentPending:
                        return PurchaseResult.Pending;

                    default:
                        return PurchaseResult.Error;
                }
            }
            catch (InAppBillingPurchaseException pEx)
            {
                Console.WriteLine($"Purchase exception: {pEx.Message}, PurchaseError: {pEx.PurchaseError}");

                // Check if the user already owns this product
                if (pEx.PurchaseError == PurchaseError.AlreadyOwned)
                {
                    return PurchaseResult.AlreadyOwned;
                }

                return PurchaseResult.Error;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during purchase: {ex.Message}");
                return PurchaseResult.Error;
            }
            finally
            {
                await CrossInAppBilling.Current.DisconnectAsync();
            }
#endif
        }

        public async Task<bool> IsSubscribedAsync(SubscriptionType.SubscriptionTypeEnum type)
        {
            // First check if we have a valid subscription stored locally
            var subscriptionInfo = await GetCurrentSubscriptionInfoAsync();

            if (subscriptionInfo.IsActive &&
                subscriptionInfo.Type >= type &&
                subscriptionInfo.ExpirationDate.HasValue &&
                subscriptionInfo.ExpirationDate.Value > DateTime.UtcNow)
            {
                return true;
            }

#if DEBUG
            // In debug mode, just rely on the stored info
            return subscriptionInfo.IsActive && subscriptionInfo.Type >= type;
#else
            // If not in debug mode, check with the billing service
            try
            {
                var connected = await CrossInAppBilling.Current.ConnectAsync();
                if (!connected)
                {
                    // Fall back to local data if we can't connect
                    return subscriptionInfo.IsActive && subscriptionInfo.Type >= type;
                }

                var purchases = await CrossInAppBilling.Current.GetPurchasesAsync(ItemType.Subscription);

                // Filter for active subscriptions of the requested type or higher
                var activeSubscriptions = purchases?.Where(p =>
                    p.State == PurchaseState.Purchased &&
                    SubscriptionTypeMap.ContainsKey(p.ProductId) &&
                    SubscriptionTypeMap[p.ProductId] >= type
                );

                var isSubscribed = activeSubscriptions?.Any() == true;

                // Update local storage if we have an active subscription
                if (isSubscribed && activeSubscriptions.Any())
                {
                    var highestSubscription = activeSubscriptions
                        .OrderByDescending(p => SubscriptionTypeMap[p.ProductId])
                        .First();

                    var subType = SubscriptionTypeMap[highestSubscription.ProductId];

                    _settingService.SaveSetting(
                        MagicStrings.SubscriptionType,
                        TypeToStringMap[subType]
                    );

                    // Calculate appropriate expiration date based on subscription type
                    var expirationDate = highestSubscription.ProductId.Contains("monthly")
                        ? DateTime.UtcNow.AddMonths(1)
                        : DateTime.UtcNow.AddYears(1);

                    _settingService.SaveSetting(
                        MagicStrings.SubscriptionExpiration,
                        expirationDate.ToString("o")
                    );
                }

                return isSubscribed;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking subscription: {ex.Message}");

                // Fall back to local data if we encounter an error
                return subscriptionInfo.IsActive && subscriptionInfo.Type >= type;
            }
            finally
            {
                await CrossInAppBilling.Current.DisconnectAsync();
            }
#endif
        }

        public async Task<SubscriptionInfo> GetCurrentSubscriptionInfoAsync()
        {
            // Default to free subscription
            var subscriptionInfo = new SubscriptionInfo
            {
                Type = SubscriptionType.SubscriptionTypeEnum.Free,
                IsActive = true, // Free tier is always active
                ExpirationDate = null,
                AutoRenewing = false
            };

            try
            {
                // Try to get subscription type from setting service
                var storedType = _settingService.GetSettingWithMagicString(MagicStrings.SubscriptionType);

                if (!string.IsNullOrEmpty(storedType))
                {
                    if (storedType == MagicStrings.Premium)
                    {
                        subscriptionInfo.Type = SubscriptionType.SubscriptionTypeEnum.Premium;
                        subscriptionInfo.IsActive = true;
                    }
                    else if (storedType == MagicStrings.Pro)
                    {
                        subscriptionInfo.Type = SubscriptionType.SubscriptionTypeEnum.Professional;
                        subscriptionInfo.IsActive = true;
                    }
                }

                // Try to get expiration date from setting service
                var storedExpiration = _settingService.GetSettingWithMagicString(MagicStrings.SubscriptionExpiration);

                if (!string.IsNullOrEmpty(storedExpiration) &&
                    DateTime.TryParse(storedExpiration, out var expirationDate))
                {
                    subscriptionInfo.ExpirationDate = expirationDate;

                    // Check if subscription has expired
                    if (expirationDate < DateTime.UtcNow)
                    {
                        // Subscription has expired, revert to free
                        subscriptionInfo.Type = SubscriptionType.SubscriptionTypeEnum.Free;
                        subscriptionInfo.IsActive = true;

                        // Clean up expired subscription data
                        _settingService.SaveSetting(MagicStrings.SubscriptionType, MagicStrings.Free);

                        // There's no direct "Remove" method, so we'll just set it to empty
                        _settingService.SaveSetting(MagicStrings.SubscriptionExpiration, string.Empty);
                    }
                }

#if !DEBUG
                // In non-debug mode, check with the billing service if we can
                var connected = await CrossInAppBilling.Current.ConnectAsync();
                if (connected)
                {
                    try
                    {
                        var purchases = await CrossInAppBilling.Current.GetPurchasesAsync(ItemType.Subscription);

                        // Find the highest tier active subscription
                        var activeSubscription = purchases?
                            .Where(p =>
                                p.State == PurchaseState.Purchased &&
                                SubscriptionTypeMap.ContainsKey(p.ProductId)
                            )
                            .OrderByDescending(p => SubscriptionTypeMap[p.ProductId])
                            .FirstOrDefault();

                        if (activeSubscription != null)
                        {
                            subscriptionInfo.Type = SubscriptionTypeMap[activeSubscription.ProductId];
                            subscriptionInfo.IsActive = true;

                            // Since ExpirationDate isn't available directly, we'll calculate it
                            var calculatedExpirationDate = activeSubscription.ProductId.Contains("monthly")
                                ? DateTime.UtcNow.AddMonths(1)
                                : DateTime.UtcNow.AddYears(1);

                            subscriptionInfo.ExpirationDate = calculatedExpirationDate;
                            subscriptionInfo.AutoRenewing = activeSubscription.AutoRenewing;

                            // Update local storage
                            _settingService.SaveSetting(
                                MagicStrings.SubscriptionType,
                                TypeToStringMap[subscriptionInfo.Type]
                            );

                            _settingService.SaveSetting(
                                MagicStrings.SubscriptionExpiration,
                                calculatedExpirationDate.ToString("o")
                            );
                        }
                    }
                    finally
                    {
                        await CrossInAppBilling.Current.DisconnectAsync();
                    }
                }
#else
                // In debug mode, set auto-renewing for active subscriptions
                if (subscriptionInfo.Type != SubscriptionType.SubscriptionTypeEnum.Free && 
                    subscriptionInfo.ExpirationDate.HasValue && 
                    subscriptionInfo.ExpirationDate.Value > DateTime.UtcNow)
                {
                    subscriptionInfo.AutoRenewing = true;
                }
#endif
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting current subscription: {ex.Message}");
                // Fall back to free tier on error
            }

            return subscriptionInfo;
        }

        public async Task<DateTime?> GetSubscriptionExpirationDateAsync()
        {
            try
            {
                // First check setting service
                var storedExpiration = _settingService.GetSettingWithMagicString(MagicStrings.SubscriptionExpiration);

                if (!string.IsNullOrEmpty(storedExpiration) &&
                    DateTime.TryParse(storedExpiration, out var expirationDate))
                {
                    return expirationDate;
                }

#if DEBUG
                // In debug mode, generate a mock expiration date if none exists
                var currentType = _settingService.GetSettingWithMagicString(MagicStrings.SubscriptionType);
                
                // Only generate a mock date if there's an active non-free subscription
                if (!string.IsNullOrEmpty(currentType) && 
                    (currentType == MagicStrings.Premium || currentType == MagicStrings.Pro))
                {
                    // Create a mock expiration date 6 months from now
                    var mockExpirationDate = DateTime.UtcNow.AddMonths(6);
                    
                    // Store it for consistency
                    _settingService.SaveSetting(
                        MagicStrings.SubscriptionExpiration,
                        mockExpirationDate.ToString("o")
                    );
                    
                    return mockExpirationDate;
                }
                
                return null;
#else
                // If not found locally, check with the billing service
                var connected = await CrossInAppBilling.Current.ConnectAsync();
                if (!connected)
                    return null;

                var purchases = await CrossInAppBilling.Current.GetPurchasesAsync(ItemType.Subscription);

                var latestSubscription = purchases?
                    .Where(p =>
                        p.State == PurchaseState.Purchased &&
                        SubscriptionTypeMap.ContainsKey(p.ProductId)
                    )
                    .OrderByDescending(p => p.TransactionDateUtc)
                    .FirstOrDefault();

                if (latestSubscription != null)
                {
                    // Calculate appropriate expiration date based on subscription type
                    var calculatedExpirationDate = latestSubscription.ProductId.Contains("monthly")
                        ? DateTime.UtcNow.AddMonths(1)
                        : DateTime.UtcNow.AddYears(1);

                    // Update local storage
                    _settingService.SaveSetting(
                        MagicStrings.SubscriptionExpiration,
                        calculatedExpirationDate.ToString("o")
                    );

                    return calculatedExpirationDate;
                }

                return null;
#endif
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting expiration date: {ex.Message}");
                return null;
            }
#if !DEBUG
            finally
            {
                await CrossInAppBilling.Current.DisconnectAsync();
            }
#endif
        }

        public async Task<bool> RestorePurchasesAsync()
        {
#if DEBUG
            // Mock restore purchases in debug mode
            // 80% chance of successful restore
            var random = new Random();
            
            if (random.Next(1, 101) <= 80)
            {
                // Simulate finding a subscription from a previous purchase
                var currentType = _settingService.GetSettingWithMagicString(MagicStrings.SubscriptionType);
                
                if (string.IsNullOrEmpty(currentType) || currentType == MagicStrings.Free)
                {
                    // Randomly pick Premium or Professional
                    var subscriptionType = random.Next(0, 2) == 0 
                        ? SubscriptionType.SubscriptionTypeEnum.Premium 
                        : SubscriptionType.SubscriptionTypeEnum.Professional;
                    
                    // Set expiration date 6 months from now
                    var expirationDate = DateTime.UtcNow.AddMonths(6);
                    
                    // Store in setting service
                    _settingService.SaveSetting(
                        MagicStrings.SubscriptionType, 
                        TypeToStringMap[subscriptionType]
                    );
                    
                    _settingService.SaveSetting(
                        MagicStrings.SubscriptionExpiration,
                        expirationDate.ToString("o")
                    );

                    return true;
                }
                
                return true;
            }
            
            return false;
#else
            try
            {
                var connected = await CrossInAppBilling.Current.ConnectAsync();
                if (!connected)
                    return false;

                var purchases = await CrossInAppBilling.Current.GetPurchasesAsync(ItemType.Subscription);

                if (purchases?.Any() != true)
                    return false;

                // Find the highest tier active subscription
                var activeSubscription = purchases
                    .Where(p =>
                        p.State == PurchaseState.Purchased &&
                        SubscriptionTypeMap.ContainsKey(p.ProductId)
                    )
                    .OrderByDescending(p => SubscriptionTypeMap[p.ProductId])
                    .FirstOrDefault();

                if (activeSubscription != null)
                {
                    var subscriptionType = SubscriptionTypeMap[activeSubscription.ProductId];

                    // Store subscription type
                    _settingService.SaveSetting(
                        MagicStrings.SubscriptionType,
                        TypeToStringMap[subscriptionType]
                    );

                    // Calculate appropriate expiration date based on subscription type
                    var calculatedExpirationDate = activeSubscription.ProductId.Contains("monthly")
                        ? DateTime.UtcNow.AddMonths(1)
                        : DateTime.UtcNow.AddYears(1);

                    _settingService.SaveSetting(
                        MagicStrings.SubscriptionExpiration,
                        calculatedExpirationDate.ToString("o")
                    );

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error restoring purchases: {ex.Message}");
                return false;
            }
            finally
            {
                await CrossInAppBilling.Current.DisconnectAsync();
            }
#endif
        }
    }
}