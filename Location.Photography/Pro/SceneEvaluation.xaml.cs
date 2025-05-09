using Location.Core.Helpers;
using Location.Core.Helpers.AlertService;
using Location.Photography.Base;
using Location.Photography.Resources;
using Location.Photography.Shared.ViewModels;
using Locations.Core.Business.DataAccess.Interfaces;
using Locations.Core.Shared.ViewModels;

namespace Location.Photography.Pro;

public partial class SceneEvaluation : ContentPageBase
{
    #region Fields
    private readonly string[] _colorLabels = new string[] { AppResources.Red, AppResources.Blue, AppResources.Green, AppResources.Contrast };
    #endregion

    #region Constructors
    /// <summary>
    /// Default constructor for design-time and XAML preview
    /// </summary>
    public SceneEvaluation() : base()
    {
        InitializeComponent();

        // Create a design-time view model
        BindingContext = new SceneEvaluationViewModel();

        // Set up radio button labels
        SetupRadioButtonLabels();
    }

    /// <summary>
    /// Main constructor with DI
    /// </summary>
    public SceneEvaluation(
        ISettingService<SettingViewModel> settingsService,
        IAlertService alertService) : base(settingsService, alertService, PageEnums.SceneEvaluation, true)
    {
        InitializeComponent();
        InitializeViewModel();
    }
    #endregion

    #region Initialization
    /// <summary>
    /// Sets up the ViewModel with the required services
    /// </summary>
    private void InitializeViewModel()
    {
        try
        {
            // Create the view model
            var viewModel = new SceneEvaluationViewModel();

            // Subscribe to error events
            viewModel.ErrorOccurred += ViewModel_ErrorOccurred;

            // Set as binding context
            BindingContext = viewModel;

            // Set up radio button labels
            SetupRadioButtonLabels();
        }
        catch (Exception ex)
        {
            HandleError(ex, "Error initializing view model");
        }
    }

    /// <summary>
    /// Sets up the radio button labels
    /// </summary>
    private void SetupRadioButtonLabels()
    {
        // Set radio button labels for color channels
        var radioButtons = new RadioButton[] {
            RedRadioButton,
            BlueRadioButton,
            GreenRadioButton,
            ContrastRadioButton,
            DefaultRadioButton
        };

        // Set the content for each radio button
        for (int i = 0; i < Math.Min(_colorLabels.Length, 4); i++)
        {
            radioButtons[i].Content = _colorLabels[i];
        }

        // Set default selection content
        DefaultRadioButton.Content = AppResources.ViewAll;

        // Set button text
        EvaluateSceneBtn.Text = AppResources.EvaluateScene;
    }
    #endregion

    #region Event Handlers
    /// <summary>
    /// Handle radio button checked changed
    /// </summary>
    private void CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (e.Value)
        {
            var radioButton = sender as RadioButton;
            if (radioButton == null) return;

            // Get the selected color from the radio button value
            string value = radioButton.Value?.ToString();

            // Show/hide histograms based on selection
            switch (value)
            {
                case "R":
                    histogramRed.IsVisible = true;
                    histogramBlue.IsVisible = false;
                    histogramGreen.IsVisible = false;
                    histogramContrast.IsVisible = false;
                    break;
                case "G":
                    histogramRed.IsVisible = false;
                    histogramBlue.IsVisible = false;
                    histogramGreen.IsVisible = true;
                    histogramContrast.IsVisible = false;
                    break;
                case "B":
                    histogramRed.IsVisible = false;
                    histogramBlue.IsVisible = true;
                    histogramGreen.IsVisible = false;
                    histogramContrast.IsVisible = false;
                    break;
                case "C":
                    histogramRed.IsVisible = false;
                    histogramBlue.IsVisible = false;
                    histogramGreen.IsVisible = false;
                    histogramContrast.IsVisible = true;
                    break;
                case "A":
                    histogramRed.IsVisible = true;
                    histogramBlue.IsVisible = true;
                    histogramGreen.IsVisible = true;
                    histogramContrast.IsVisible = true;
                    break;
            }
        }
    }

    /// <summary>
    /// Handle evaluate scene button click
    /// </summary>
    private void EvaluateSceneBtn_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is SceneEvaluationViewModel viewModel)
        {
            // Show processing indicator
            processing.IsVisible = true;
            processing.IsRunning = true;

            // Execute the evaluate command
            viewModel.EvaluateCommand.Execute(null);
        }
    }

    /// <summary>
    /// Handle errors from the view model
    /// </summary>
    private void ViewModel_ErrorOccurred(object sender, Locations.Core.Shared.ViewModels.OperationErrorEventArgs e)
    {
        // Display error to user if it's not already displayed in the UI
        MainThread.BeginInvokeOnMainThread(async () => {
            await DisplayAlert(
                AppResources.Error,
                e.Message,
                AppResources.OK);

            // Hide processing indicator
            processing.IsVisible = false;
            processing.IsRunning = false;
        });
    }
    #endregion

    #region Lifecycle Methods
    /// <summary>
    /// Called when the page appears
    /// </summary>
    protected override void OnPageAppearing(object sender, EventArgs e)
    {
        base.OnPageAppearing(sender, e);

        //TODO:  Implement Page Helpers across this project.

     //   PageHelpers.CheckSubscription(PageEnums.SceneEvaluation, )

        // Re-subscribe to ViewModel events in case the binding context changed
        if (BindingContext is SceneEvaluationViewModel viewModel)
        {
            // Make sure we only subscribe once
            viewModel.ErrorOccurred -= ViewModel_ErrorOccurred;
            viewModel.ErrorOccurred += ViewModel_ErrorOccurred;

            // Default radio button selection
            DefaultRadioButton.IsChecked = true;

            // Show all histograms by default
            histogramRed.IsVisible = true;
            histogramBlue.IsVisible = true;
            histogramGreen.IsVisible = true;
            histogramContrast.IsVisible = true;
        }
    }

    /// <summary>
    /// Called when the page disappears
    /// </summary>
    protected override void OnPageDisappearing(object sender, EventArgs e)
    {
        base.OnPageDisappearing(sender, e);

        // Unsubscribe from ViewModel events
        if (BindingContext is SceneEvaluationViewModel viewModel)
        {
            viewModel.ErrorOccurred -= ViewModel_ErrorOccurred;
        }
    }

    /// <summary>
    /// Called after the page has been displayed
    /// </summary>
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        // Hide processing indicator
        processing.IsVisible = false;
        processing.IsRunning = false;
    }
    #endregion

    #region Error Handling
    /// <summary>
    /// Handle errors during view operations
    /// </summary>
    private void HandleError(Exception ex, string message)
    {
        // Log the error
        System.Diagnostics.Debug.WriteLine($"Error: {message}. {ex.Message}");

        // Display alert to user
        DisplayAlert(AppResources.Error, message, AppResources.OK);

        // Pass the error to the ViewModel if available
        if (BindingContext is SceneEvaluationViewModel viewModel)
        {
            viewModel.VmErrorMessage = $"{message}: {ex.Message}";
            viewModel.IsProcessing = false;
        }

        // Hide processing indicator
        processing.IsVisible = false;
        processing.IsRunning = false;
    }
    #endregion
}