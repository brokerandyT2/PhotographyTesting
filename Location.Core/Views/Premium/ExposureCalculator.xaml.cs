
using Location.Core.Helpers;
using Locations.Core.Business.Advertising;
using Locations.Core.Business.DataAccess;
using Locations.Core.Shared;
using lps = Location.Photography.Shared;
using lpv = Location.Photography.Shared.ViewModels;
namespace Location.Core.Views.Premium;

public partial class ExposureCalculator : ContentPage
{
    private bool _skipCalculations = true;
    private SettingsService ss = new SettingsService();
    TipService ts = new TipService();
    private bool _adSupport;
    public ExposureCalculator()
    {
        _adSupport = ss.GetSettingByName(MagicStrings.FreePremiumAdSupported).ToBoolean();
        InitializeComponent();
        closeButton.IsVisible = false;
        ShutterSpeed_Picker.IsEnabled = false;

        var x = (lpv.ExposureCalculator)BindingContext;
        ShutterSpeed_Picker.SelectedIndex = 0;
        fstop_Picker.SelectedIndex = 0;
        ISO_Picker.SelectedIndex = 0;
        x.ISOSelected = ISO_Picker.SelectedItem.ToString();
        x.OldISO = ISO_Picker.Items[1].ToString();
        x.FStopSelected = fstop_Picker.SelectedItem.ToString();
        x.OldFstop = fstop_Picker.Items[1].ToString();
        x.ShutterSpeedSelected = ShutterSpeed_Picker.SelectedItem.ToString();
        x.OldShutterSpeed = ShutterSpeed_Picker.Items[1].ToString();
        try
        {

            //BindingContext = x;
            x.FullHalfThirds = lpv.ExposureCalculator.Divisions.Full;
            x.Calculate();
        }
        catch
        {
            //Yup . . swallowing another exception
        }
        _skipCalculations = false;
        exposurefull.IsChecked = true;
        shutter.IsChecked = true;
        //((lpv.ExposureCalculator)BindingContext).Calculate();
    }
    public ExposureCalculator(int tipID)
    {

        var x = ts.Get(tipID);
        lps.ViewModels.ExposureCalculator ec = new lps.ViewModels.ExposureCalculator();
        ec.OldISO = (ISO_Picker.SelectedIndex = ISO_Picker.SelectedIndex + 1).ToString();
        ec.FStopSelected = fstop_Picker.SelectedItem.ToString();
        ec.OldFstop = (fstop_Picker.SelectedIndex = fstop_Picker.SelectedIndex + 1).ToString();
        ec.ShutterSpeedSelected = ShutterSpeed_Picker.SelectedItem.ToString();
        ec.OldShutterSpeed = (ec.ShutterSpeedSelected = ec.ShutterSpeedSelected + 1).ToString();
        ec.FStopSelected = x.Fstop;
        ec.ISOSelected = x.ISO;
        ec.ShutterSpeedSelected = x.Shutterspeed;
        fstop_Picker.SelectedItem = ec.FStopSelected;
        ShutterSpeed_Picker.SelectedItem = ec.ShutterSpeedSelected;
        ISO_Picker.SelectedItem = ec.ISOSelected;
        _skipCalculations = false;
        ec.Calculate();

    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        Locations.Core.Business.DataAccess.SettingsService ss = new Locations.Core.Business.DataAccess.SettingsService();

        PageHelpers.CheckVisit(MagicStrings.ExposureCalcViewed, PageEnums.ExposureCalculator, ss, Navigation);
        PageHelpers.ShowAD(ss.GetSettingByName(MagicStrings.FreePremiumAdSupported).ToBoolean(), Navigation);


    }

    private void closeButton_Pressed(object sender, EventArgs e)
    {
        Navigation.PopModalAsync();
    }
    private void calculate_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (_skipCalculations)
        { return; }
        Microsoft.Maui.Controls.RadioButton rb = (Microsoft.Maui.Controls.RadioButton)sender;
        var x = rb.Content;
        lps.ViewModels.ExposureCalculator ec = (lps.ViewModels.ExposureCalculator)BindingContext;
        if (rb.IsChecked == true)
        {
            switch (Convert.ToInt32(rb.Value))
            {
                case 0:
                    ec.ToCalculate = lps.ViewModels.ExposureCalculator.FixedValue.ShutterSpeeds;
                    ShutterSpeed_Picker.IsEnabled = false;
                    fstop_Picker.IsEnabled = true;
                    ISO_Picker.IsEnabled = true;
                    shutterspeedresult.Text = ShutterSpeed_Picker.SelectedItem.ToString();
                    break;
                case 1:
                    ec.ToCalculate = lps.ViewModels.ExposureCalculator.FixedValue.Apeature;
                    ShutterSpeed_Picker.IsEnabled = true;
                    fstop_Picker.IsEnabled = false;
                    ISO_Picker.IsEnabled = true;
                    fstopresult.Text = (string)fstop_Picker.SelectedItem.ToString(); ;
                    break;

                case 2:
                    ec.ToCalculate = lps.ViewModels.ExposureCalculator.FixedValue.ISOs;
                    ShutterSpeed_Picker.IsEnabled = true;
                    fstop_Picker.IsEnabled = true;
                    ISO_Picker.IsEnabled = false;
                    isoresult.Text = (string)ISO_Picker.SelectedItem.ToString();
                    break;
            }
            PopulateVM();
            ec.Calculate();
        }

    }

    private void exposuresteps_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {

        var x = (Microsoft.Maui.Controls.RadioButton)sender;
        var y = (lps.ViewModels.ExposureCalculator)BindingContext;
        if (Convert.ToDouble(x.Value) == (double)1)
        {
            y.FullHalfThirds = lps.ViewModels.ExposureCalculator.Divisions.Full;
            // exposurefull.IsChecked = true;
            //exposurehalfstop.IsChecked = false;
            // exposurethirdstop.IsChecked = false;
        }
        else if (Convert.ToDouble(x.Value) == .5)
        {
            y.FullHalfThirds = lps.ViewModels.ExposureCalculator.Divisions.Half;
        }
        else
        {
            y.FullHalfThirds = lps.ViewModels.ExposureCalculator.Divisions.Thirds;
        }
        PopulateVM();
        y.Calculate();
    }


    private void ShutterSpeed_Picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_skipCalculations)
        { return; }
        var x = (lps.ViewModels.ExposureCalculator)BindingContext;
        x.OldShutterSpeed = x.ShutterSpeedSelected;
        x.ShutterSpeedSelected = ShutterSpeed_Picker.SelectedItem.ToString();
        x.Calculate();

    }

    private void PopulateVM()
    {
        var x = (lps.ViewModels.ExposureCalculator)BindingContext;

        if (x.ShutterSpeedSelected != ShutterSpeed_Picker.SelectedItem.ToString())
        {
            x.OldShutterSpeed = x.ShutterSpeedSelected;
            x.ShutterSpeedSelected = ShutterSpeed_Picker.SelectedItem.ToString();
        }
        x.OldShutterSpeed = x.ShutterSpeedSelected;
        x.OldFstop = x.FStopSelected;
        x.OldISO = x.ISOSelected;
        x.ShutterSpeedSelected = ShutterSpeed_Picker.SelectedItem.ToString();
        x.FStopSelected = fstop_Picker.SelectedItem.ToString();
        x.ISOSelected = ISO_Picker.SelectedItem.ToString();
        ;
    }

    private void fstop_Picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_skipCalculations)
        { return; }
        var x = (lps.ViewModels.ExposureCalculator)BindingContext;
        x.OldFstop = x.FStopSelected;
        x.FStopSelected = fstop_Picker.SelectedItem.ToString();
        x.Calculate();
    }

    private void ISO_Picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_skipCalculations)
        { return; }
        var x = (lps.ViewModels.ExposureCalculator)BindingContext;
        x.OldISO = x.ISOSelected;
        x.ISOSelected = ISO_Picker.SelectedItem.ToString();
        x.Calculate();
    }


}
