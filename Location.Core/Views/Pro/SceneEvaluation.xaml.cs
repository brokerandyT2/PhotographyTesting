
using Location.Core.Helpers;
using Location.Core.Resources;
using Location.Photography.Shared.ViewModels;
using Locations.Core.Shared;
using Locations.Core.Shared.Customizations.Alerts.Interfraces;


namespace Location.Core.Views.Pro;

public partial class SceneEvaluation : ContentPage
{        Locations.Core.Business.DataAccess.SettingsService ss = new Locations.Core.Business.DataAccess.SettingsService();
    private IAlertService alertServ;

    public SceneEvaluationViewModel Item
    {
        get => BindingContext as SceneEvaluationViewModel;
        set
        {
            BindingContext = value;

        }
    }
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {

        base.OnNavigatedTo(args);

        PageHelpers.CheckVisit(MagicStrings.SceneEvaluationViewed, PageEnums.SceneEvaluation, ss, Navigation);

    }
    public SceneEvaluation(IAlertService alertserv): this()
    {
        this.alertServ = alertserv;

    }
    public SceneEvaluation(IAlertService alertserv, SceneEvaluationViewModel viewModel) : this(alertserv)
    {

        Item = viewModel;
    }
    public SceneEvaluation()
    {

        InitializeComponent();
        BindingContext = this;
        Default.IsChecked = true;
    }
    private async void EvaluateSceneBtn_Clicked(object sender, EventArgs e)
    {
        SceneEvaluationViewModel viewModel = new SceneEvaluationViewModel();
        try
        {
            await Task.Run(async () =>
             {
                 Dispatcher.Dispatch(() =>
                 {
                     processing.IsVisible = true;
                     processing.IsRunning = true;
                 });
                 await viewModel.EvaluateScene(sender);

                 Dispatcher.Dispatch(() =>
             {
                      processing.IsVisible = false;
                      processing.IsRunning = false;
                  });

             });
            Item = viewModel;
        }
        catch (Exception ex)
        {
            await DisplayAlert(AppResources.Error, AppResources.Error, AppResources.OK);
        }
        if (Item.IsError)
        {
            await DisplayAlert(AppResources.Error, Item.alertEventArgs.Message, AppResources.OK);
        }
        // selectors.IsVisible = true;

    }
    private void CheckedChanged(object sender, CheckedChangedEventArgs e)
    {

        Microsoft.Maui.Controls.RadioButton rb = (Microsoft.Maui.Controls.RadioButton)sender;
        var x = rb.Value;

       switch (x) { 
            case "R":
                histogramRed.IsVisible = true;
                histogramGreen.IsVisible = false;
                histogramBlue.IsVisible = false;
                histogramContrast.IsVisible= false;
                break;
            case "B":
                histogramRed.IsVisible = false;
                histogramGreen.IsVisible = false;
                histogramBlue.IsVisible = true;
                histogramContrast.IsVisible = false;
                break;
            case "G":
                histogramRed.IsVisible = false;
                histogramGreen.IsVisible = true;
                histogramBlue.IsVisible = false;
                histogramContrast.IsVisible = false;
                break;
            case "C":
                histogramRed.IsVisible = false;
                histogramGreen.IsVisible = false;
                histogramBlue.IsVisible = false;
                histogramContrast.IsVisible = true;
                break;
            case "A":
                histogramRed.IsVisible = true;
                histogramGreen.IsVisible = true;
                histogramBlue.IsVisible = true;
                histogramContrast.IsVisible = true;
                break;
        } 

    }
}