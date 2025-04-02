using Locations.Core.Business.DataAccess;
using Locations.Core.Shared;
using Locations.Core.Shared.ViewModels;

namespace Location.Core.Views;

public partial class Settings : ContentPage
{
	private SettingsService _service = new SettingsService();
	public Settings()
	{
		InitializeComponent();
		BindingContext = _service.GetAllSettings();
	}

    private void Hemisphere_Toggled(object sender, ToggledEventArgs e)
    {
        var x = _service.GetSettingByName(MagicStrings.Hemisphere);
		if (e.Value == true)
		{
			x.Value = MagicStrings.North;
		}
		else
		{
			x.Value = MagicStrings.South;
		}
	((SettingsViewModel)BindingContext).Hemisphere = _service.Save(x, false);
    }

    private void Timeformat_Toggled(object sender, ToggledEventArgs e)
    {
		var x = _service.GetSettingByName(MagicStrings.TimeFormat);
		if (e.Value == true)
		{
			x.Value = MagicStrings.USTimeformat_Pattern;
		}else
		{
			x.Value = MagicStrings.InternationalTimeFormat_Pattern;
		}
        ((SettingsViewModel)BindingContext).TimeFormat = _service.Save(x, false);
    }

    private void DateFormat_Toggled(object sender, ToggledEventArgs e)
    {
        var x = _service.GetSettingByName(MagicStrings.DateFormat);
        if (e.Value == true)
        {
            x.Value = MagicStrings.USDateFormat;
        }
        else
        {
            x.Value = MagicStrings.InternationalFormat;
        }
        ((SettingsViewModel)BindingContext).DateFormat = _service.Save(x, false);
    }

    private void AdFree_Toggled(object sender, ToggledEventArgs e)
    {
        var x = _service.GetSettingByName(MagicStrings.FreePremiumAdSupported);
        if (e.Value == true)
        {
            x.Value = MagicStrings.True_string;
        }
        else
        {
            x.Value = MagicStrings.False_string;
        }
        ((SettingsViewModel)BindingContext).AdSupport = _service.Save(x, false);
    }
}