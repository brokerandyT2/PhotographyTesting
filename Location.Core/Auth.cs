
using Duende.IdentityModel.Client;
using Duende.IdentityModel.Jwk;
using Duende.IdentityModel.OidcClient;
using Duende.IdentityModel.OidcClient.Browser;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

#if ANDROID
using Duende.IdentityModel.OidcClient;
using Duende.IdentityModel.OidcClient.Browser;
#endif
namespace Location.Core
{
    public class Auth
    {

        const string clientId = "1064488372076-p7r465ir1idfvh89fdt5ssoaa23p7ume.apps.googleusercontent.com";
        const string redirectUri = "com.x3squaredcircles.locations/localhost:8080";
        const string authority = "https://accounts.google.com/o/oauth2/v2/auth";
        const string scope = "openid profile email";
        const string userInfoEndpoint = "https://openidconnect.googleapis.com/v1/userinfo";
        public async Task<string> PerformOAuthLogin()
        {
            try
            {
                var browser = new WebAuthenticatorBrowser();
                var options = new OidcClientOptions
                {
                    Authority = authority,
                    ClientId = clientId,
                    Scope = scope,
                    RedirectUri = redirectUri,
                    Browser = browser,
                    ProviderInformation = new ProviderInformation
                    {
                        IssuerName = "accounts.google.com",
                        AuthorizeEndpoint = authority,
                        TokenEndpoint = "https://www.googleapis.com/oauth2/v4/token",
                        UserInfoEndpoint = userInfoEndpoint,
                        KeySet = new JsonWebKeySet()
                    },
                };

                var oidcClient = new OidcClient(options);
                var loginResult = await oidcClient.LoginAsync();

                if (loginResult.IsError)
                {
                    throw new Exception($"Failed to authenticate with Google: {loginResult.Error}");
                }

                return loginResult.AccessToken;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string OnWebViewNavigating(WebNavigatingEventArgs e, ContentPage signInContentPage)
        {


            if (e.Url.StartsWith("http://localhost"))
            {

                Uri uri = new Uri(e.Url);
                string query = WebUtility.UrlDecode(uri.Query);
                var queryParams = System.Web.HttpUtility.ParseQueryString(query);
                string authorizationCode = queryParams.Get("code");

                signInContentPage.Navigation.PopModalAsync();
                return authorizationCode;


            }


            return null;
        }

        public static (string, string) ExchangeCodeForAccessToken(string code)
        {

            // Configure the necessary parameters for the token exchange
            const string clientId = "1064488372076-p7r465ir1idfvh89fdt5ssoaa23p7ume.apps.googleusercontent.com";

            const string clientSecret = "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";
            const string redirectUri = "https://localhost:8080";

            // Create an instance of HttpClient
            using (HttpClient client = new HttpClient())
            {
                // Construct the token exchange URL
                const string tokenUrl = "https://oauth2.googleapis.com/token";

                // Create a FormUrlEncodedContent object with the required parameters
                FormUrlEncodedContent content = new FormUrlEncodedContent(new Dictionary<string, string>
              {
                   { "code", code },
                   { "client_id", clientId },
                   { "client_secret", clientSecret },
                   { "redirect_uri", redirectUri },
                   { "grant_type", "authorization_code" }
              });

                // Send a POST request to the token endpoint to exchange the code for an access token
                HttpResponseMessage response = client.PostAsync(tokenUrl, content).Result;

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content
                    string responseContent = response.Content.ReadAsStringAsync().Result;

                    // Parse the JSON response to extract the access token
                    JObject json = JObject.Parse(responseContent);
                    string accessToken = json.GetValue("access_token").ToString();
                    string refreshToken = json.GetValue("refresh_token").ToString();
                    return (accessToken, refreshToken);
                }
                else
                {
                    // Exception:  "Token exchange request failed with status code: {response.StatusCode}"
                }
            }

            return (null, null);
        }

        class WebAuthenticatorBrowser : Duende.IdentityModel.OidcClient.Browser.IBrowser
        {
            public Task<BrowserResult> InvokeAsync(BrowserOptions options,
                                           CancellationToken cancellationToken = default)
            {
                try
                {
                    WebAuthenticatorResult result =
                        WebAuthenticator.AuthenticateAsync(
                                              new Uri(options.StartUrl),
                                              new Uri(options.EndUrl)).Result;

                    var url = new RequestUrl(redirectUri)
                            .Create([.. result.Properties]);

                    var x = new BrowserResult
                    {
                        Response = url,
                        ResultType = BrowserResultType.Success,
                    };
                    return Task.FromResult(x);

                }
                catch (TaskCanceledException ex)
                {
                    var x = new BrowserResult()
                    {
                        ResultType = BrowserResultType.UserCancel,
                        Error = ex.Message
                    };
                    return Task.FromResult(x);
                }
            }
        }
    }
}