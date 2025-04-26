using Locations.Core.Business.DataAccess;
using Locations.Core.Shared;
using Locations.Core.Shared.Enums;
using Locations.Core.Shared.ViewModels;

namespace Location.Core.Views;

public partial class Tips : ContentPage
{
    TipService ts = new TipService();
    SettingsService settingsService = new SettingsService();
	public Tips()
	{
		InitializeComponent();
        BindingContext = ts.GetDisplay();
        pick.SelectedIndex = 0;
        //exposurecalc.IsEnabled = settingsService.GetSettingByName(MagicStrings.SubscriptionType).Value == SubscriptionType.SubscriptionTypeEnum.Professional.Name() ? true: false;
	}

    private void pick_SelectedIndexChanged(object sender, EventArgs e)
    {
        var id = ((TipTypeViewModel)pick.SelectedItem).Id;
        BindingContext = ts.PopulateTips(id);
    }

    private void exposurecalc_Pressed(object sender, EventArgs e)
    {
                
            Navigation.PushModalAsync(new Views.Premium.ExposureCalculator((int)((Microsoft.Maui.Controls.Button)sender).CommandParameter, true));
       

    }
}