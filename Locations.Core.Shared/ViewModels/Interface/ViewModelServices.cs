using Locations.Core.Shared.DTO;
using Locations.Core.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Locations.Core.Shared.ViewModelServices
{
   

    #region Weather Service

    public interface IWeatherService
    {
        Task<OperationResult<WeatherDTO>> GetWeatherForLocationAsync(int locationId);
        Task<OperationResult<string>> GetForecastForLocationAsync(int locationId, int days = 5);
        Task<OperationResult<bool>> UpdateAllWeatherAsync();

        event EventHandler<OperationErrorEventArgs> ErrorOccurred;
    }
    #endregion

    #region Settings Service

    public interface ISettingService
    {
        Task<OperationResult<SettingViewModel>> GetSettingAsync(string key);
        Task<OperationResult<bool>> SaveSettingAsync(SettingViewModel settings);
        Task<OperationResult<SettingsViewModel>> GetSettings_Async();

        event EventHandler<OperationErrorEventArgs> ErrorOccurred;
    }

    #endregion

    #region Tip Service

    public interface ITipService
    {
        Task<OperationResult<List<TipTypeDTO>>> GetTipTypesAsync();
        Task<OperationResult<List<TipDTO>>> GetTipsForTypeAsync(int tipTypeId);
        Task<OperationResult<TipDTO>> GetRandomTipForTypeAsync(int tipTypeId);

        event EventHandler<OperationErrorEventArgs> ErrorOccurred;
    }

    #endregion
}