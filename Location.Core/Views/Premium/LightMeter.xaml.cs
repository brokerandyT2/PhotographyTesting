
using Location.Core.Helpers;
using Location.Photography.Business.LightMeter;
using Locations.Core.Shared;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;

namespace Location.Core.Views.Premium;

public partial class LightMeter : ContentPage
{
    public LunaProDrawable LunaDrawable { get; set; }


    public LightMeter()
    {
        InitializeComponent();
        BindingContext = this;
    }
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        PageHelpers.CheckVisit(MagicStrings.LightMeterViewed, PageEnums.LightMeter, new Locations.Core.Business.DataAccess.SettingsService(), Navigation);
    }
}
