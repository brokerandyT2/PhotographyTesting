using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Authentication;
using Microsoft.Maui.Controls;

namespace Locations.Core.Shared.ViewModels
{
    public class GoogleLoginViewModel : BindableObject
    {
        private string _name;
        private string _email;
        private string _pictureUrl;
        private bool _isBusy;

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        public string PictureUrl
        {
            get => _pictureUrl;
            set { _pictureUrl = value; OnPropertyChanged(); }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set { _isBusy = value; OnPropertyChanged(); }
        }

        public ICommand LoginCommand { get; }

        private const string clientId = "1064488372076-9juboi2k00j5rta6f5vbl1q3dr5cn56b.apps.googleusercontent.com"; // Replace!
        private const string redirectUri = "com.googleusercontent.apps.1064488372076-9juboi2k00j5rta6f5vbl1q3dr5cn56b:/oauth2redirect"; // Replace!

        public GoogleLoginViewModel()
        {
            LoginCommand = new Command(async () => await LoginAsync());
        }

        private async Task LoginAsync()
        {
            try
            {
                IsBusy = true;

                var authResult = await WebAuthenticator.AuthenticateAsync(
                    new Uri($"https://accounts.google.com/o/oauth2/v2/auth?" +
                            $"client_id={clientId}" +
                            $"&response_type=token" +
                            $"&scope=openid%20profile%20email" +
                            $"&redirect_uri={redirectUri}"),
                    new Uri(redirectUri)
                );

                if (authResult != null && authResult.Properties.ContainsKey("access_token"))
                {
                    var accessToken = authResult.Properties["access_token"];
                    await FetchGoogleUserProfileAsync(accessToken);
                }
            }
            catch (Exception ex)
            {
                // Handle login errors (user canceled, no network, etc.)
                Console.WriteLine($"Login failed: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task FetchGoogleUserProfileAsync(string accessToken)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var response = await httpClient.GetStringAsync("https://www.googleapis.com/oauth2/v2/userinfo");

            if (!string.IsNullOrEmpty(response))
            {
                var userInfo = JsonSerializer.Deserialize<GoogleUserInfo>(response);

                Name = userInfo?.Name ?? "";
                Email = userInfo?.Email ?? "";
                PictureUrl = userInfo?.Picture ?? "";
            }
        }

        private class GoogleUserInfo
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string Picture { get; set; }
        }
    }
}