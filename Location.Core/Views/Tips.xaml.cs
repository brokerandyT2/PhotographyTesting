using Location.Core.Helpers;
using Location.Core.Helpers.AlertService;
using Locations.Core.Business.DataAccess.Interfaces;
using Locations.Core.Business.DataAccess.Services;
using Locations.Core.Data.Queries;
using Locations.Core.Shared;
using Locations.Core.Shared.Enums;
using Locations.Core.Shared.ViewModels;

namespace Location.Core.Views;

public partial class Tips : ContentPageBase
{
    #region Services and Fields

    private readonly TipService<TipViewModel> _tipService;
    private readonly ISettingService<SettingViewModel> _settingsService;
    private static Locations.Core.Shared.Enums.SubscriptionType.SubscriptionTypeEnum _subType;

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor for design-time and XAML preview
    /// </summary>
    public Tips() : base()
    {
        var alertserv = new Location.Core.Helpers.AlertService.EventAlertService();
        var loggingServ = new Locations.Core.Business.Services.LoggerService();


        InitializeComponent();
        _tipService = new TipService<TipViewModel>(new Locations.Core.Data.Queries.TipRepository(_alertService, null),alertserv, new Locations.Core.Business.Services.LoggerService());
        BindingContext = _tipService.GetAllAsync();
        pick.SelectedIndex = 0;
    }

    /// <summary>
    /// Main constructor with DI
    /// </summary>
    public Tips(
        ISettingService<SettingViewModel> settingsService,
        IAlertService alertService) : base(settingsService, alertService, PageEnums.Tips, false)
    {
        _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
        _tipService = new TipService<TipViewModel>(new Locations.Core.Data.Queries.TipRepository(_alertService, null), alertService, new Locations.Core.Business.Services.LoggerService());

        InitializeComponent();

        // Load initial data
        BindingContext = _tipService.GetAllAsync();
        pick.SelectedIndex = 0;
    }

    #endregion

    #region Event Handlers

    /// <summary>
    /// Handle tip type selection change
    /// </summary>
    private void pick_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (pick.SelectedItem is TipTypeViewModel selectedType)
        {
            var tipId = selectedType.Id;
            BindingContext = _tipService.GetByIdAsync(tipId);
        }
    }

    /// <summary>
    /// Handle exposure calculator button press
    /// </summary>
    private void exposurecalc_Pressed(object sender, EventArgs e)
    {
        var button = (Microsoft.Maui.Controls.Button)sender;
        var tipId = (int)button.CommandParameter;

        Navigation.PushModalAsync(new Location.Photography.Premium.ExposureCalculator());
    }

    #endregion

    #region Lifecycle Methods

    /// <summary>
    /// Check subscription when navigated to
    /// </summary>
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        // Check for first visit
        PageHelpers.CheckVisit(MagicStrings.TipsViewed, PageEnums.Tips, _settingsService, Navigation);

        // Check subscription type for UI visibility
        Enum.TryParse(_settingsService.GetSettingByName(MagicStrings.SubscriptionType).Value, out _subType);
        exposurecalc.IsVisible = _subType == SubscriptionType.SubscriptionTypeEnum.Professional ||
                                 _subType == SubscriptionType.SubscriptionTypeEnum.Premium;
    }

    #endregion
}