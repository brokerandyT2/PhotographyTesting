using Location.Photography.Business.DataAccess;
using Location.Photography.Business.ExposureCalculator.Interface;
using lps = Location.Photography.Shared;

namespace Location.Photography.UI.Views.Premium;

public partial class ExposureCalculator : ContentPage
{
    TipService ts = new TipService();
    public ExposureCalculator()
    {
        InitializeComponent();
        closeButton.IsVisible = false;
    }
    public ExposureCalculator(lps.ViewModels.Interfaces.IExposureCalculator model)
    {
        BindingContext = model;
        closeButton.IsVisible = true;
    }
    public ExposureCalculator(int tipID)
    { 
        var x = ts.Get(tipID);
        lps.ViewModels.ExposureCalculator ec = new lps.ViewModels.ExposureCalculator();
        ec.FStopSelected = x.Fstop;
        ec.ISOSelected = x.ISO;
        ec.ShutterSpeedSelected=x.Shutterspeed;
        fstop_Picker.SelectedItem = ec.FStopSelected;
        ShutterSpeed_Picker.SelectedItem = ec.ShutterSpeedSelected;
        ISO_Picker.SelectedItem = ec.ISOSelected;
    
    }
    private void calculate_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        RadioButton rb = (RadioButton)sender;
        var x = rb.Content;
        Shared.ViewModels.ExposureCalculator ec = (lps.ViewModels.ExposureCalculator)BindingContext;
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
        }
        PopulateVM();
        ec.Calculate();
    }

    private void exposuresteps_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        var x = (RadioButton)sender;
        var y = (lps.ViewModels.ExposureCalculator)BindingContext;
        if ((int)x.Value == 1)
        {
            y.FullHalfThirds = lps.ViewModels.ExposureCalculator.Divisions.Full;
        }
        else if ((double)x.Value == .5)
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
        var x = (lps.ViewModels.ExposureCalculator)BindingContext;
        x.OldShutterSpeed = x.ShutterSpeedSelected;
        x.ShutterSpeedSelected = ShutterSpeed_Picker.SelectedItem.ToString();
        
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
        var x = (lps.ViewModels.ExposureCalculator)BindingContext;
        x.OldFstop = x.FStopSelected;
        x.FStopSelected = fstop_Picker.SelectedItem.ToString();
    }

    private void ISO_Picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var x = (lps.ViewModels.ExposureCalculator)BindingContext;
        x.OldISO = x.ISOSelected;
        x.ISOSelected = ISO_Picker.SelectedItem.ToString();
    }

    private void closeButton_Pressed(object sender, EventArgs e)
    {
       Navigation.PopAsync();
    }
}