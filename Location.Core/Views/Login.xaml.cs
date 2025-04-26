using Location.Photography.Business.GoogleLoginService;
using Plugin.Maui.GoogleClient;

namespace Location.Core.Views;

public partial class Login : ContentPage
{
    private const string clientId = "1064488372076-p7r465ir1idfvh89fdt5ssoaa23p7ume.apps.googleusercontent.com";
    private const string redirect = "com.x3squaredcirlecs.locations/callback";
    private const string scope = "openid profile email";

    public Login()
	{
		InitializeComponent();
	}

    private void Login_Clicked(object sender, EventArgs e)
    {




        try
        {
            // Replace with your actual redirect URI
            var options = new WebAuthenticatorOptions
            {
                CallbackUrl = new Uri("http://example.com"),
                Url = new Uri($"https://accounts.google.com/o/oauth2/v2/auth?response_type=token&client_id={clientId}&redirect_uri={redirect}&scope={scope}")
            };
            var result = WebAuthenticator.Default.AuthenticateAsync(options).Result;

            //result.AccessToken will contain the access token
        }
        catch (Exception ex)
        {
            var y = string.Empty;
            //Handle errors
        }


        //Auth x = new Auth();
       // var token = x.PerformOAuthLogin();
        /*var user = x.GetUser(token.Result);
        var userInfo = x.GetUserInfo(token.Result);
        var email = userInfo.Result.Email;
        var name = userInfo.Result.Name;
        var lastName = userInfo.Result.FamilyName;*/
        var z = string.Empty;
    }


}