using Android.App;
using Android.Content;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Common.Apis;
using Microsoft.Maui.ApplicationModel;
using System.Text.Json;
using System.Web;

namespace Location.Core.Platforms.Android
{
    public class GoogleAuthService
    {
        private const string ClientId = "1064488372076-6qhj231h00ggpo2g6r9sb8j1gh4n7f3u.apps.googleusercontent.com"; // Replace with your real Client ID
        private const string AuthUrl = "https://accounts.google.com/o/oauth2/v2/auth";
        private const string Scope = "openid profile email";
        private const string RedirectUri = "https://localhost"; // For dev/test. Match this with Google Console

        public GoogleAuthService() { }

        public async Task<(string FirstName, string LastName, string Email)> AuthenticateAsync()
        {
            var authUri = $"{AuthUrl}?" +
                $"client_id={ClientId}&" +
                $"redirect_uri={RedirectUri}&" +
                $"response_type=token&" +  // Important: We use 'token' (implicit flow) because you said you don't want a full OAuth code exchange
                $"scope={HttpUtility.UrlEncode(Scope)}&" +
                $"include_granted_scopes=true";

            var authResult = await WebAuthenticator.Default.AuthenticateAsync(
                new Uri(authUri),
                new Uri(RedirectUri));

            if (authResult?.AccessToken == null)
                throw new Exception("Authentication failed");

            // Now use the access token to call Google's UserInfo API
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authResult.AccessToken);

            var userInfoResponse = await client.GetStringAsync("https://www.googleapis.com/oauth2/v3/userinfo");

            var user = JsonSerializer.Deserialize<GoogleUser>(userInfoResponse);

            return (user?.GivenName ?? "", user?.FamilyName ?? "", user?.Email ?? "");
        }

        private class GoogleUser
        {
            public string Sub { get; set; }
            public string Name { get; set; }
            public string GivenName { get; set; }
            public string FamilyName { get; set; }
            public string Email { get; set; }
            public bool EmailVerified { get; set; }
            public string Picture { get; set; }
            public string Locale { get; set; }
        }
    }
}