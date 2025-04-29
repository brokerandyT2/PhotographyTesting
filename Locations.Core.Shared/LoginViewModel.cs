using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input; // <-- Add this namespace!
using System.IdentityModel.Tokens.Jwt; // <-- Add this!
using System.Diagnostics;
using System.Security.Claims;
using System.Text.Json;

namespace Locations.Core.Shared
{
    public partial class LoginViewModel : ObservableObject
    {
        [ObservableProperty]
        private string userFirstName;

        [ObservableProperty]
        private string userLastName;

        [ObservableProperty]
        private string userEmail;

        [ObservableProperty]
        private string statusMessage;
        [ObservableProperty]
        private int statusCode;

        // Command to trigger Google login
        public IRelayCommand GoogleLoginCommands { get; set; }

        public LoginViewModel()
        {
            UserFirstName = "";
            UserLastName = "";
            UserEmail = "";
            StatusMessage = "Please login.";
            StatusCode = 0;

            // Initialize the Google login command
            GoogleLoginCommands = new RelayCommand(async () => await GoogleLoginAsync());
        }

        public async Task GoogleLoginAsync()
        {
            const string clientId = "1064488372076-9juboi2k00j5rta6f5vbl1q3dr5cn56b.apps.googleusercontent.com";
            const string redirectUri = "com.googleusercontent.apps.1064488372076-9juboi2k00j5rta6f5vbl1q3dr5cn56b:/oauth2redirect/";
            const string authorizationEndpoint = "https://accounts.google.com/o/oauth2/v2/auth";
            const string tokenEndpoint = "https://oauth2.googleapis.com/token";
            try
            {
                var authUrl = $"{authorizationEndpoint}?client_id={clientId}" +
                              $"&redirect_uri={Uri.EscapeDataString(redirectUri)}" +
                              $"&response_type=code" +
                              $"&scope=openid%20email%20profile" +
                              $"&access_type=offline" +
                              $"&prompt=consent";

                var authResult = await WebAuthenticator.Default.AuthenticateAsync(
                    new Uri(authUrl),
                    new Uri(redirectUri));

                if (authResult != null && authResult.Properties.TryGetValue("code", out var authCode))
                {
                    // Exchange auth code for id_token
                    var httpClient = new HttpClient();
                    var tokenRequestParams = new Dictionary<string, string>
                        {
                            {"client_id", clientId},
                            {"redirect_uri", redirectUri},
                            {"grant_type", "authorization_code"},
                            {"code", authCode }
                        };

                    var tokenRequestContent = new FormUrlEncodedContent(tokenRequestParams);
                    var tokenResponse = await httpClient.PostAsync(tokenEndpoint, tokenRequestContent);
                    var responseContent = await tokenResponse.Content.ReadAsStringAsync();

                    if (tokenResponse.IsSuccessStatusCode)
                    {
                        var json = JsonDocument.Parse(responseContent);
                        if (json.RootElement.TryGetProperty("id_token", out var idTokenElement))
                        {
                            var idToken = idTokenElement.GetString();

                            // Parse the id_token
                            var handler = new JwtSecurityTokenHandler();
                            var token = handler.ReadJwtToken(idToken);

                            UserFirstName = token.Claims.FirstOrDefault(c => c.Type == "given_name")?.Value ?? "";
                            UserLastName = token.Claims.FirstOrDefault(c => c.Type == "family_name")?.Value ?? "";
                            UserEmail = token.Claims.FirstOrDefault(c => c.Type == "email")?.Value ?? "";

                            StatusMessage = $"Welcome {UserFirstName} {UserLastName}!";
                            StatusCode = 1;
                        }
                        else
                        {
                            StatusMessage = "Token exchange failed: No id_token.";
                        }
                    }
                    else
                    {
                        StatusMessage = $"Token exchange failed: {responseContent}";
                    }
                }
                else
                {
                    StatusMessage = "Authentication failed: No auth code.";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Login failed: {ex.Message}";
                Debug.WriteLine($"Login failed with exception: {ex}");
            }
        }
    }
}