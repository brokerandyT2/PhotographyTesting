using Location.Core.Helpers.AlertService;
using Location.Core.Helpers.LoggingService;
using Locations.Core.Business.DataAccess.Interfaces;
using Locations.Core.Data.Models;
using Locations.Core.Data.Queries.Interfaces;
using Locations.Core.Shared.DTO;
using Locations.Core.Shared.ViewModels;
using DataErrorEventArgs = Locations.Core.Data.Models.DataErrorEventArgs;
using ErrorSource = Locations.Core.Data.Models.ErrorSource;

namespace Locations.Core.Business.DataAccess.Services
{
    /// <summary>
    /// Service for tip type-related operations
    /// </summary>
    /// <typeparam name="T">The view model type for tip types</typeparam>
    public class TipTypeService<T> : ITipTypeService<T>
        where T : TipTypeViewModel, new()
    {
        private readonly ITipTypeRepository _repository;
        private readonly IAlertService _alertService;
        private readonly ILoggerService _loggerService;

        /// <summary>
        /// Event triggered when an error occurs in the service
        /// </summary>
        public event DataErrorEventHandler ErrorOccurred;

        /// <summary>
        /// Creates a new instance of the TipTypeService with dependencies
        /// </summary>
        /// <param name="repository">The tip type repository</param>
        /// <param name="alertService">The alert service</param>
        /// <param name="loggerService">The logger service</param>
        public TipTypeService(
            ITipTypeRepository repository,
            IAlertService alertService,
            ILoggerService loggerService)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _alertService = alertService ?? throw new ArgumentNullException(nameof(alertService));
            _loggerService = loggerService ?? throw new ArgumentNullException(nameof(loggerService));

            // Connect repository events
            if (_repository is ITipTypeRepository repoWithEvents)
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
        public void OnErrorOccurred(DataErrorEventArgs e)
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
                    // Create a TipTypeDTO from the repository data
                    var dto = new TipTypeDTO
                    {
                        Id = result.Data.Id,
                        Name = result.Data.Name
                    };
                    viewModel.InitializeFromDTO(dto);
                    return DataOperationResult<T>.Success(viewModel);
                }
                else
                {
                    // Create a new failure result for type T
                    return DataOperationResult<T>.Failure(
                        result.ErrorSource,
                        $"Failed to retrieve tip type with ID {id}",
                        result.Exception);
                }
            }
            catch (Exception ex)
            {
                var errorArgs = CreateErrorEventArgs(ErrorSource.Unknown,
                    $"Error retrieving tip type with ID {id}: {ex.Message}", ex);
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
                        // Create a TipTypeDTO from the repository data
                        var dto = new TipTypeDTO
                        {
                            Id = item.Id,
                            Name = item.Name
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
                        "Failed to retrieve all tip types",
                        result.Exception);
                }
            }
            catch (Exception ex)
            {
                var errorArgs = CreateErrorEventArgs(ErrorSource.Unknown,
                    $"Error retrieving all tip types: {ex.Message}", ex);
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

                // Create a TipTypeViewModel from the entity
                var tipTypeViewModel = new TipTypeViewModel
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Description = entity.Description,
                    DisplayOrder = entity.DisplayOrder,
                    Icon = entity.Icon
                };

                var result = await _repository.SaveAsync(tipTypeViewModel);

                if (result.IsSuccess && result.Data != null)
                {
                    // Convert the result back to T using a DTO
                    var dto = new TipTypeDTO
                    {
                        Id = result.Data.Id,
                        Name = result.Data.Name
                    };
                    entity.InitializeFromDTO(dto);
                    return DataOperationResult<T>.Success(entity);
                }
                else
                {
                    // Create a new failure result
                    return DataOperationResult<T>.Failure(
                        result.ErrorSource,
                        "Failed to save tip type",
                        result.Exception);
                }
            }
            catch (Exception ex)
            {
                var errorArgs = CreateErrorEventArgs(ErrorSource.Unknown,
                    $"Error saving tip type: {ex.Message}", ex);
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

                // Create a TipTypeViewModel from the entity
                var tipTypeViewModel = new TipTypeViewModel
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Description = entity.Description,
                    DisplayOrder = entity.DisplayOrder,
                    Icon = entity.Icon
                };

                var result = await _repository.UpdateAsync(tipTypeViewModel);
                return result; // This already returns DataOperationResult<bool>
            }
            catch (Exception ex)
            {
                var errorArgs = CreateErrorEventArgs(ErrorSource.Unknown,
                    $"Error updating tip type: {ex.Message}", ex);
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
                    $"Error deleting tip type with ID {id}: {ex.Message}", ex);
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
                    throw new InvalidOperationException("Cannot delete tip type with invalid ID");
                }

                return await DeleteAsync(id);
            }
            catch (Exception ex)
            {
                var errorArgs = CreateErrorEventArgs(ErrorSource.Unknown,
                    $"Error deleting tip type: {ex.Message}", ex);
                OnErrorOccurred(errorArgs);
                return DataOperationResult<bool>.Failure(ErrorSource.Unknown, errorArgs.Message, ex);
            }
        }

        /// <summary>
        /// Gets a tip type by its name
        /// </summary>
        public async Task<OperationResult<T>> GetByNameAsync(string name)
        {
            try
            {
                // Get all tip types
                var result = await GetAllAsync();

                if (result.IsSuccess && result.Data != null)
                {
                    // Find the tip type with the matching name
                    var tipType = result.Data.FirstOrDefault(t =>
                        string.Equals(t.Name, name, StringComparison.OrdinalIgnoreCase));

                    if (tipType != null)
                    {
                        return OperationResult<T>.Success(tipType);
                    }
                    else
                    {
                        return OperationResult<T>.Failure(
                            $"No tip type found with name '{name}'",
                            null,
                            OperationErrorSource.Unknown);
                    }
                }
                else
                {
                    return OperationResult<T>.Failure(
                        $"Failed to retrieve tip types",
                        null,
                        OperationErrorSource.Unknown);
                }
            }
            catch (Exception ex)
            {
                var errorArgs = CreateErrorEventArgs(ErrorSource.Unknown,
                    $"Error retrieving tip type with name '{name}': {ex.Message}", ex);
                OnErrorOccurred(errorArgs);
                return OperationResult<T>.Failure(
                    $"Error retrieving tip type: {ex.Message}",
                    ex,
                    OperationErrorSource.Unknown);
            }
        }

        /// <summary>
        /// Gets all tip types sorted by display order
        /// </summary>
        public async Task<OperationResult<List<T>>> GetAllSortedAsync()
        {
            try
            {
                var result = await GetAllAsync();

                if (result.IsSuccess && result.Data != null)
                {
                    // Sort by DisplayOrder property
                    var sortedList = result.Data
                        .OrderBy(t => t.DisplayOrder)
                        .ToList();

                    return OperationResult<List<T>>.Success(sortedList);
                }
                else
                {
                    return OperationResult<List<T>>.Failure(
                        $"Failed to retrieve tip types",
                        null,
                        OperationErrorSource.Unknown);
                }
            }
            catch (Exception ex)
            {
                var errorArgs = CreateErrorEventArgs(ErrorSource.Unknown,
                    $"Error retrieving sorted tip types: {ex.Message}", ex);
                OnErrorOccurred(errorArgs);
                return OperationResult<List<T>>.Failure(
                    $"Error retrieving tip types: {ex.Message}",
                    ex,
                    OperationErrorSource.Unknown);
            }
        }

       
    }
}