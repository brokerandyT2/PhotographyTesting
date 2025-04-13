using Locations.Core.Shared.DTO;

namespace Location.Core.Usercontrols.Controls;

public partial class Weather : ContentPage
{
    private string _highText;
    public string HighText { get => _highText; set { HighTemp.Text = value; } }
    private string _lowText;
    public string LowText { get => _lowText; set { LowTemp.Text = value; } }
    public DailyWeatherWithDetailsDTO Item
    {
        get => (BindingContext) as DailyWeatherWithDetailsDTO;
        set => BindingContext = value;
    }
    public Weather()
	{
		InitializeComponent();
	}
}