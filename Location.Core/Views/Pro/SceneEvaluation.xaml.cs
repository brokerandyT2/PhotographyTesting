using Location.Core.Helpers;
using Location.Photography.Shared.ViewModels;
using Locations.Core.Shared;
using Syncfusion.Maui.Toolkit.Carousel;

namespace Location.Core.Views.Pro;

public partial class SceneEvaluation : ContentPage
{
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
        Locations.Core.Business.DataAccess.SettingsService ss = new Locations.Core.Business.DataAccess.SettingsService();
        base.OnNavigatedTo(args);
        var x = ss.GetSettingByName(MagicStrings.SceneEvaluationViewed);
        var z = ss.GetSettingByName(MagicStrings.FreePremiumAdSupported);
        var isAds = z.ToBoolean();

        PageHelpers.CheckVisit(MagicStrings.SceneEvaluationViewed, PageEnums.SceneEvaluation, ss, Navigation);
        PageHelpers.ShowAD(ss.GetSettingByName(MagicStrings.FreePremiumAdSupported).ToBoolean(), Navigation);

    }
    public SceneEvaluation()
    {

        InitializeComponent();
        BindingContext = this;
    }
    private async void EvaluateSceneBtn_Clicked(object sender, EventArgs e)
    {
        SceneEvaluationViewModel viewModel = new SceneEvaluationViewModel();
        
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
}