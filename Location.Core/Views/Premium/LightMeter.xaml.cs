
using Location.Photography.Business.LightMeter;
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
}
