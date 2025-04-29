
using Location.Core.Resources;
using Locations.Core.Business.DataAccess;
using Locations.Core.Shared;
using Microsoft.Maui.Authentication;
using System.Diagnostics;

namespace Location.Core.Views;

public partial class Login : ContentPage
{

    public Login()
    {
        InitializeComponent();
    }

    private async void Button_Pressed(object sender, EventArgs e)
    {
        var x = BindingContext as LoginViewModel;
         //await x.GoogleLoginAsync();

        if (x.StatusCode == 1)
        {
            SettingsService ss = new SettingsService();

            var name = ss.GetSettingByName(MagicStrings.FirstName);
            name.Value = x.UserFirstName;
            
            var email = ss.GetSettingByName(MagicStrings.Email);
            email.Value = x.UserEmail;
            
            var lastName = ss.GetSettingByName(MagicStrings.LastName);
            lastName.Value = x.UserLastName;

            ss.SaveSettingWithObjectReturn(lastName);
            ss.SaveSettingWithObjectReturn(name);
            ss.SaveSettingWithObjectReturn(email);

            Navigation.PushAsync(new MainPage());
        }
        else
        {
            DisplayAlert(AppResources.Error, AppResources.LoginFailed, AppResources.OK);
        }
    }
}
