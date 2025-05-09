using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Location.Core.Helpers.AlertService;
using Location.Core.Helpers.LoggingService;
using Locations.Core.Data.Models;
using Locations.Core.Data.Queries.Interfaces;
using System.Reflection;

namespace Locations.Core.Business.DataAccess.Base
{
    /// <summary>
    /// Base implementation for all services providing common functionality
    /// </summary>
    /// <typeparam name="T">The entity type for this service</typeparam>
    /// <typeparam name="TRepository">The repository type for this service</typeparam>
    public abstract class ServiceBase<T, TRepository> : IServiceBase<T>
        where T : class, new()
        where TRepository : IRepository<T>
    {
        /// <summary>
        /// Event triggered when an error occurs in the service
        /// </summary>
        public event DataErrorEventHandler ErrorOccurred;

        /// <summary>
        /// The repository used by this service
        /// </summary>
        protected readonly TRepository Repository;

        /// <summary>
        /// Alert service for user notifications
        /// </summary>
        protected readonly IAlertService AlertService;

        /// <summary>
        /// Logger service for logging errors and info
        /// </summary>
        protected readonly ILoggerService LoggerService;

        /// <summary>
        /// Creates a new service with dependencies
        /// </summary>
        /// <param name="repository">The repository to use</param>
        /// <param name="alertService">The alert service to use</param>
        /// <param name="loggerService">The logger service to use</param>
        protected ServiceBase(TRepository repository, IAlertService alertService, ILoggerService loggerService)
        {
            Repository = repository ?? throw new ArgumentNullException(nameof(repository));
            AlertService = alertService ?? throw new ArgumentNullException(nameof(alertService));
            LoggerService = loggerService ?? throw new ArgumentNullException(nameof(loggerService));

            // Connect repository events
            if (repository is IRepository<T> repoWithEvents)
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
            LoggerService.LogError(e.Message, e.Exception);

            // Forward the error event
            OnErrorOccurred(e);
        }

        /// <summary>
        /// Raises the error event
        /// </summary>
        public virtual void OnErrorOccurred(DataErrorEventArgs e)
        {
            // Log the error
            LoggerService.LogError(e.Message, e.Exception);

            // Show alert for UI - using the standard methods from IAlertService
            if (AlertService != null)
            {
                if (e.Exception != null)
                {
                    // Use logging since IAlertService doesn't have ShowAlert
                    LoggerService.LogError($"{e.Source}: {e.Message}", e.Exception);
                }
                else
                {
                    LoggerService.LogWarning($"{e.Source}: {e.Message}");
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
                return await Repository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                var errorArgs = CreateErrorEventArgs(ErrorSource.Unknown,
                    $"Error retrieving entity with ID {id}: {ex.Message}", ex);
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
                var result = await Repository.GetAllAsync();
                if (result.IsSuccess)
                {
                    // Convert IList to List if needed
                    var resultList = result.Data as List<T> ?? new List<T>(result.Data);
                    return DataOperationResult<List<T>>.Success(resultList);
                }

                // Create a new failure result with the same error information
                return DataOperationResult<List<T>>.Failure(
                    ErrorSource.Unknown,
                    "Failed to retrieve all entities",
                    null);
            }
            catch (Exception ex)
            {
                var errorArgs = CreateErrorEventArgs(ErrorSource.Unknown,
                    $"Error retrieving all entities: {ex.Message}", ex);
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
                return await Repository.SaveAsync(entity);
            }
            catch (Exception ex)
            {
                var errorArgs = CreateErrorEventArgs(ErrorSource.Unknown,
                    $"Error saving entity: {ex.Message}", ex);
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
                return await Repository.UpdateAsync(entity);
            }
            catch (Exception ex)
            {
                var errorArgs = CreateErrorEventArgs(ErrorSource.Unknown,
                    $"Error updating entity: {ex.Message}", ex);
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
                return await Repository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                var errorArgs = CreateErrorEventArgs(ErrorSource.Unknown,
                    $"Error deleting entity with ID {id}: {ex.Message}", ex);
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
                // Try to get the ID of the entity
                int id = -1;

                // Check if the entity has an ID property
                var idProperty = typeof(T).GetProperty("ID") ?? typeof(T).GetProperty("Id");
                if (idProperty != null && idProperty.PropertyType == typeof(int))
                {
                    id = (int)idProperty.GetValue(entity);
                    if (id > 0)
                    {
                        return await DeleteAsync(id);
                    }
                }

                // If we couldn't get a valid ID, try to use the repository's delete method directly
                // This is a fallback and might not work for all repositories
                try
                {
                    // Try to call DeleteAsync(entity) on the repository
                    var deleteMethod = typeof(TRepository).GetMethod("DeleteAsync",
                        new[] { typeof(T) });

                    if (deleteMethod != null)
                    {
                        var task = (Task<DataOperationResult<bool>>)deleteMethod.Invoke(Repository, new object[] { entity });
                        return await task;
                    }

                    throw new InvalidOperationException("Repository does not support DeleteAsync(entity)");
                }
                catch (Exception innerEx)
                {
                    // Log the error and create a failure result
                    LoggerService.LogError("Failed to delete entity", innerEx);
                    return DataOperationResult<bool>.Failure(
                        ErrorSource.Unknown,
                        $"Failed to delete entity: {innerEx.Message}",
                        innerEx);
                }
            }
            catch (Exception ex)
            {
                var errorArgs = CreateErrorEventArgs(ErrorSource.Unknown,
                    $"Error deleting entity: {ex.Message}", ex);
                OnErrorOccurred(errorArgs);
                return DataOperationResult<bool>.Failure(ErrorSource.Unknown, errorArgs.Message, ex);
            }
        }
    }
}