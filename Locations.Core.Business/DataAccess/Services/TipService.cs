using Location.Core.Helpers.AlertService;
using Location.Core.Helpers.LoggingService;

//using Location.Core.Helpers.LoggingService;
using Locations.Core.Business.DataAccess.Interfaces;
using Locations.Core.Data.Models;
using Locations.Core.Data.Queries.Interfaces;
using Locations.Core.Shared.DTO;
using Locations.Core.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataErrorEventArgs = Locations.Core.Data.Models.DataErrorEventArgs;
using ErrorSource = Locations.Core.Data.Models.ErrorSource;

namespace Locations.Core.Business.DataAccess.Services
{
    /// <summary>
    /// Service for tip-related operations
    /// </summary>
    /// <typeparam name="T">The view model type for tips</typeparam>
    public class TipService<T> : ITipService<T>
        where T : TipViewModel, new()
    {
        private readonly ITipRepository _repository;
        private readonly IAlertService _alertService;
        private readonly ILoggerService _loggerService;
        private readonly ITipTypeService<TipTypeViewModel> _tipTypeService;
        private readonly Random _random = new Random();

        /// <summary>
        /// Event triggered when an error occurs in the service
        /// </summary>
        public event DataErrorEventHandler ErrorOccurred;

        /// <summary>
        /// Creates a new instance of the TipService with dependencies
        /// </summary>
        /// <param name="repository">The tip repository</param>
        /// <param name="alertService">The alert service</param>
        /// <param name="loggerService">The logger service</param>
        /// <param name="tipTypeService">The tip type service</param>
        public TipService(
            ITipRepository repository,
            IAlertService alertService,
            ILoggerService loggerService,
            ITipTypeService<TipTypeViewModel> tipTypeService = null)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _alertService = alertService ?? throw new ArgumentNullException(nameof(alertService));
            _loggerService = loggerService ?? throw new ArgumentNullException(nameof(loggerService));
            _tipTypeService = tipTypeService;

            // Connect repository events
            if (_repository is ITipRepository repoWithEvents)
            {
                repoWithEvents.ErrorOccurred += RepositoryOnErrorOccurred;
            }
        }

        /// <summary>
        /// Handles errors from the repository
        /// </summary>
        private void RepositoryOnErrorOccurred(object sender, DataErrorEventArgs e)
        {
            // Log the error
            _loggerService.LogError(e.Message, e.Exception);

            // Forward the error event
            OnErrorOccurred(e);
        }

        /// <summary>
        /// Raises the error event
        /// </summary>
        

        /// <summary>
        /// Creates a new error event
        /// </summary>
        protected virtual DataErrorEventArgs CreateErrorEventArgs(ErrorSource source, string message, Exception ex = null)
        {
            return new DataErrorEventArgs(source, message, ex);
        }

        /// <summary>
        /// Gets an entity by ID
        /// </summary>
        public virtual async Task<DataOperationResult<T>> GetByIdAsync(int id)
        {
            try
            {
                var result = await _repository.GetByIdAsync(id);

                if (result.IsSuccess && result.Data != null)
                {
                    // Convert the result to the expected type T
                    var viewModel = new T();
                    // Create a TipDTO from the viewModel
                    var dto = new TipDTO
                    {
                        Id = result.Data.Id,
                        TipTypeID = result.Data.TipTypeId,
                        Title = result.Data.Title,
                        Content = result.Data.Description,
                        Fstop = result.Data.Apeture,
                        Shutterspeed = result.Data.Shutterspeed,
                        ISO = result.Data.Iso
                    };
                    viewModel.InitializeFromDTO(dto);
                    return DataOperationResult<T>.Success(viewModel);
                }
                else
                {
                    // Create a new failure result for type T
                    return DataOperationResult<T>.Failure(
                        result.ErrorSource,
                        $"Failed to retrieve tip with ID {id}",
                        result.Exception);
                }
            }
            catch (Exception ex)
            {
                var errorArgs = CreateErrorEventArgs(ErrorSource.Unknown,
                    $"Error retrieving tip with ID {id}: {ex.Message}", ex);
                OnErrorOccurred(errorArgs);
                return DataOperationResult<T>.Failure(ErrorSource.Unknown, errorArgs.Message, ex);
            }
        }

        /// <summary>
        /// Gets all entities
        /// </summary>
        public virtual async Task<DataOperationResult<List<T>>> GetAllAsync()
        {
            try
            {
                var result = await _repository.GetAllAsync();

                if (result.IsSuccess && result.Data != null)
                {
                    // Convert the results to a List<T>
                    var viewModels = new List<T>();
                    foreach (var item in result.Data)
                    {
                        var viewModel = new T();
                        // Create a TipDTO from the repository data
                        var dto = new TipDTO
                        {
                            Id = item.Id,
                            TipTypeID = item.TipTypeId,
                            Title = item.Title,
                            Content = item.Description,
                            Fstop = item.Apeture,
                            Shutterspeed = item.Shutterspeed,
                            ISO = item.Iso
                        };
                        viewModel.InitializeFromDTO(dto);
                        viewModels.Add(viewModel);
                    }
                    return DataOperationResult<List<T>>.Success(viewModels);
                }
                else
                {
                    // Create a new failure result
                    return DataOperationResult<List<T>>.Failure(
                        result.ErrorSource,
                        "Failed to retrieve all tips",
                        result.Exception);
                }
            }
            catch (Exception ex)
            {
                var errorArgs = CreateErrorEventArgs(ErrorSource.Unknown,
                    $"Error retrieving all tips: {ex.Message}", ex);
                OnErrorOccurred(errorArgs);
                return DataOperationResult<List<T>>.Failure(ErrorSource.Unknown, errorArgs.Message, ex);
            }
        }

        /// <summary>
        /// Saves an entity
        /// </summary>
        public virtual async Task<DataOperationResult<T>> SaveAsync(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new InvalidOperationException("Entity cannot be null");
                }

                // Create a TipViewModel from the entity
                var tipViewModel = new TipViewModel
                {
                    Id = entity.Id,
                    TipTypeId = entity.TipTypeId,
                    TipTypeName = entity.TipTypeName,
                    Description = entity.Description,
                    Title = entity.Title,
                    Apeture = entity.Apeture,
                    Shutterspeed = entity.Shutterspeed,
                    Iso = entity.Iso,
                    IsDeleted = entity.IsDeleted
                };

                var result = await _repository.SaveAsync(tipViewModel);

                if (result.IsSuccess && result.Data != null)
                {
                    // Convert the result back to T using a DTO
                    var dto = new TipDTO
                    {
                        Id = result.Data.Id,
                        TipTypeID = result.Data.TipTypeId,
                        Title = result.Data.Title,
                        Content = result.Data.Description,
                        Fstop = result.Data.Apeture,
                        Shutterspeed = result.Data.Shutterspeed,
                        ISO = result.Data.Iso
                    };
                    entity.InitializeFromDTO(dto);
                    return DataOperationResult<T>.Success(entity);
                }
                else
                {
                    // Create a new failure result
                    return DataOperationResult<T>.Failure(
                        result.ErrorSource,
                        "Failed to save tip",
                        result.Exception);
                }
            }
            catch (Exception ex)
            {
                var errorArgs = CreateErrorEventArgs(ErrorSource.Unknown,
                    $"Error saving tip: {ex.Message}", ex);
                OnErrorOccurred(errorArgs);
                return DataOperationResult<T>.Failure(ErrorSource.Unknown, errorArgs.Message, ex);
            }
        }

        /// <summary>
        /// Updates an existing entity
        /// </summary>
        public virtual async Task<DataOperationResult<bool>> UpdateAsync(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new InvalidOperationException("Entity cannot be null");
                }

                // Create a TipViewModel from the entity
                var tipViewModel = new TipViewModel
                {
                    Id = entity.Id,
                    TipTypeId = entity.TipTypeId,
                    TipTypeName = entity.TipTypeName,
                    Description = entity.Description,
                    Title = entity.Title,
                    Apeture = entity.Apeture,
                    Shutterspeed = entity.Shutterspeed,
                    Iso = entity.Iso,
                    IsDeleted = entity.IsDeleted
                };

                var result = await _repository.UpdateAsync(tipViewModel);
                return result; // This already returns DataOperationResult<bool>
            }
            catch (Exception ex)
            {
                var errorArgs = CreateErrorEventArgs(ErrorSource.Unknown,
                    $"Error updating tip: {ex.Message}", ex);
                OnErrorOccurred(errorArgs);
                return DataOperationResult<bool>.Failure(ErrorSource.Unknown, errorArgs.Message, ex);
            }
        }

        /// <summary>
        /// Deletes an entity by ID
        /// </summary>
        public virtual async Task<DataOperationResult<bool>> DeleteAsync(int id)
        {
            try
            {
                var result = await _repository.DeleteAsync(id);
                return result; // This already returns DataOperationResult<bool>
            }
            catch (Exception ex)
            {
                var errorArgs = CreateErrorEventArgs(ErrorSource.Unknown,
                    $"Error deleting tip with ID {id}: {ex.Message}", ex);
                OnErrorOccurred(errorArgs);
                return DataOperationResult<bool>.Failure(ErrorSource.Unknown, errorArgs.Message, ex);
            }
        }

        /// <summary>
        /// Deletes an entity
        /// </summary>
        public virtual async Task<DataOperationResult<bool>> DeleteAsync(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException(nameof(entity));
                }

                // Get the ID of the entity
                var id = entity.Id;
                if (id <= 0)
                {
                    throw new InvalidOperationException("Cannot delete tip with invalid ID");
                }

                return await DeleteAsync(id);
            }
            catch (Exception ex)
            {
                var errorArgs = CreateErrorEventArgs(ErrorSource.Unknown,
                    $"Error deleting tip: {ex.Message}", ex);
                OnErrorOccurred(errorArgs);
                return DataOperationResult<bool>.Failure(ErrorSource.Unknown, errorArgs.Message, ex);
            }
        }

        /// <summary>
        /// Gets all tip types
        /// </summary>
        public async Task<OperationResult<List<TipTypeViewModel>>> GetTipTypesAsync()
        {
            try
            {
                if (_tipTypeService == null)
                {
                    return OperationResult<List<TipTypeViewModel>>.Failure(
                        "Tip type service is not available",
                        null,
                        OperationErrorSource.Unknown);
                }

                var result = await _tipTypeService.GetAllSortedAsync();
                if (result.IsSuccess)
                {
                    return OperationResult<List<TipTypeViewModel>>.Success(result.Data);
                }
                else
                {
                    return OperationResult<List<TipTypeViewModel>>.Failure(
                        "Failed to retrieve tip types",
                        null,
                        OperationErrorSource.Unknown);
                }
            }
            catch (Exception ex)
            {
                var errorArgs = CreateErrorEventArgs(ErrorSource.Unknown,
                    $"Error retrieving tip types: {ex.Message}", ex);
                OnErrorOccurred(errorArgs);
                return OperationResult<List<TipTypeViewModel>>.Failure(
                    $"Error retrieving tip types: {ex.Message}",
                    ex,
                    OperationErrorSource.Unknown);
            }
        }

        /// <summary>
        /// Gets a random tip for a specific type
        /// </summary>
        public async Task<OperationResult<TipViewModel>> GetRandomTipForTypeAsync(int tipTypeId)
        {
            try
            {
                // Get all tips for this type
                var tipsResult = await GetTipsForTypeAsync(tipTypeId);

                if (tipsResult.IsSuccess && tipsResult.Data.Count > 0)
                {
                    // Select a random tip
                    int randomIndex = _random.Next(tipsResult.Data.Count);
                    return OperationResult<TipViewModel>.Success(tipsResult.Data[randomIndex]);
                }
                else
                {
                    return OperationResult<TipViewModel>.Failure(
                        $"No tips found for type ID {tipTypeId}",
                        null,
                        OperationErrorSource.Unknown);
                }
            }
            catch (Exception ex)
            {
                var errorArgs = CreateErrorEventArgs(ErrorSource.Unknown,
                    $"Error retrieving random tip for type ID {tipTypeId}: {ex.Message}", ex);
                OnErrorOccurred(errorArgs);
                return OperationResult<TipViewModel>.Failure(
                    $"Error retrieving random tip: {ex.Message}",
                    ex,
                    OperationErrorSource.Unknown);
            }
        }

        /// <summary>
        /// Gets all tips for a specific type
        /// </summary>
        public async Task<OperationResult<List<TipViewModel>>> GetTipsForTypeAsync(int tipTypeId)
        {
            try
            {
                // Get all tips first
                var result = await GetAllAsync();

                if (result.IsSuccess && result.Data != null)
                {
                    // Filter by type ID and convert to TipViewModel
                    var filteredTips = result.Data
                        .Cast<TipViewModel>()
                        .Where(t => t.TipTypeId == tipTypeId && !t.IsDeleted)
                        .ToList();

                    return OperationResult<List<TipViewModel>>.Success(filteredTips);
                }
                else
                {
                    return OperationResult<List<TipViewModel>>.Failure(
                        $"Failed to retrieve tips for type ID {tipTypeId}",
                        null,
                        OperationErrorSource.Unknown);
                }
            }
            catch (Exception ex)
            {
                var errorArgs = CreateErrorEventArgs(ErrorSource.Unknown,
                    $"Error retrieving tips for type ID {tipTypeId}: {ex.Message}", ex);
                OnErrorOccurred(errorArgs);
                return OperationResult<List<TipViewModel>>.Failure(
                    $"Error retrieving tips: {ex.Message}",
                    ex,
                    OperationErrorSource.Unknown);
            }
        }

        public void OnErrorOccurred(Locations.Core.Data.Models.DataErrorEventArgs e)
        {
            // Log the error
            _loggerService.LogError(e.Message, e.Exception);

            // Show alert for UI
            if (_alertService != null)
            {
                if (e.Exception != null)
                {
                    _loggerService.LogError($"{e.Source}: {e.Message}", e.Exception);
                }
                else
                {
                    _loggerService.LogWarning($"{e.Source}: {e.Message}");
                }
            }

            // Raise the event
            ErrorOccurred?.Invoke(this, e);
        }
    }
}