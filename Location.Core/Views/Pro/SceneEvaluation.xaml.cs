using Location.Photography.Shared.ViewModels;

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

    public SceneEvaluation()
    {

        InitializeComponent();
        BindingContext = this;
    }
    private void EvaluateSceneBtn_Clicked(object sender, EventArgs e)

    {
        PermissionStatus status = Permissions.RequestAsync<Permissions.Camera>().Result;
        Task.Run(async () =>
        {
            Dispatcher.Dispatch(() =>
            {
                processing.IsVisible = true;
                processing.IsRunning = true;
            });
            SceneEvaluationViewModel viewModel = new SceneEvaluationViewModel();
            viewModel.EvaluateScene(sender);
            Item = viewModel;
            Dispatcher.Dispatch(() =>
            {
                processing.IsVisible = false;
                processing.IsRunning = false;
            });
        });

    }
}