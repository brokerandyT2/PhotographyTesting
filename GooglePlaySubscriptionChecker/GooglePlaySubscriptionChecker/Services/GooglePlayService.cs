using Google.Apis.Auth.OAuth2;
using Google.Apis.AndroidPublisher.v3;
using Google.Apis.Services;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Google.Apis.AndroidPublisher.v3.Data;

public class GooglePlayService
{
    private readonly AndroidPublisherService _service;

    public GooglePlayService(IConfiguration configuration)
    {
        var credential = GoogleCredential
            .FromFile(configuration["GoogleServiceAccountFile"])
            .CreateScoped(AndroidPublisherService.Scope.Androidpublisher);

        _service = new AndroidPublisherService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = "GooglePlaySubscriptionChecker"
        });
    }

    public async Task<SubscriptionPurchase> GetSubscriptionStatusAsync(
        string packageName, string subscriptionId, string token)
    {
        var request = _service.Purchases.Subscriptions.Get(packageName, subscriptionId, token);
        return await request.ExecuteAsync();
    }
}
